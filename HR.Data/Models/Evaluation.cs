using IronXL;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Data.Models
{
    public class Evaluation
    {
        public int EvalID { get; set; }
        public int TemplateID { get; set; }
        public List<EvalSection> Sections { get; set; }
        public int EvaluatorID { get; set; }
        public int EvalueeID { get; set; }
        public DateTime Date { get; set; }
        public List<ScoreTableEntry> ScoreTable { get; set; }
        public bool isNew { get; set; }
        public personaldata personaldata { get; set; }
        public templatedata templatedata { get; set; }

        public void CreateSections(JArray jSections)
        {
            Sections = new List<EvalSection>();
            foreach (var s in jSections)
            {
                EvalSection newSection = new EvalSection();
                newSection.Create((JObject)s);
                Sections.Add(newSection);
            }
        }

        public void CreateScoreTable(JArray entries)
        {
            ScoreTable = new List<ScoreTableEntry>();
            foreach (JObject entry in entries)
            {
                ScoreTableEntry newEntry = new ScoreTableEntry();
                newEntry.Min = float.Parse(entry.SelectToken("minvalue").ToString());
                newEntry.Max = float.Parse(entry.SelectToken("maxvalue").ToString());
                newEntry.Text = entry.SelectToken("scoretext").ToString();
                ScoreTable.Add(newEntry);
            }
        }

        public void FillFromForm(NameValueCollection form)
        {
            foreach (var section in Sections)
            {
                foreach (var q in section.questions)
                {
                    var qtext = q.text.Replace("\n","\r\n");
                    q.savedvalue = form[qtext];
                }
            }
        }

        public bool isComplete()
        {
            foreach (var section in Sections)
            {
                foreach (var q in section.questions)
                {
                    if(q.savedvalue == null || string.IsNullOrWhiteSpace(q.savedvalue))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public float GetScore()
        {
            float fullGrade = 0;
            foreach (var s in Sections.Where(x=>x.SectionType != "text")) 
            {
                var sum = s.questions.Sum(x => x.savedvalue != null ? int.Parse(x.savedvalue) : 0);
                var final = sum * s.Weight / s.questions.Count();
                fullGrade += final;
            }
            return fullGrade;
        }

        public string GetScoreText()
        {
            return ScoreTable.Where(x => GetScore() > x.Min).Where(x => GetScore() <= x.Max).Single().Text;
        }

        public string GetJson()
        {
            personaldata = new personaldata("2020", EvalueeID, EvaluatorID);
            var settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.Auto;
            string json = JsonConvert.SerializeObject(this,settings);
            var x = json.Length;
            return json;
        }

        public void CreateFromExcel(string path)
        {
            Sections = new List<EvalSection>();

            WorkBook wb = WorkBook.LoadExcel(path);
            WorkSheet infoSheet = wb.GetWorkSheet("Sections");

            var d = infoSheet.Columns[0].Rows.ToList();
            d.Remove(d.First());
            d.RemoveAll(x=>x.Int32Value == 0);

            int i = 1;
            foreach (var number in d)
            {
                EvalSection section = new EvalSection();
                section.Order = infoSheet.GetCellAt(i,0).Int32Value;
                section.Title = infoSheet.GetCellAt(i, 1).StringValue;
                section.SectionType = Functions.GetSectionType(infoSheet.GetCellAt(i, 2).StringValue);
                section.Weight = infoSheet.GetCellAt(i, 3).FloatValue;
                section.CreateFromExcel(wb.GetWorkSheet(section.Title));
                Sections.Add(section);
                i++;
            }

            ScoreTable = new List<ScoreTableEntry>();
            WorkSheet scoreSheet = wb.GetWorkSheet("ScoreBoard");
            d = scoreSheet.Columns[0].Rows.ToList();
            d.Remove(d.First());
            i = 1;
            foreach (var item in d)
            {
                ScoreTableEntry newScore = new ScoreTableEntry();
                newScore.Min = scoreSheet.GetCellAt(i, 0).FloatValue;
                newScore.Max = scoreSheet.GetCellAt(i, 1).FloatValue;
                newScore.Text = scoreSheet.GetCellAt(i, 2).StringValue;
                ScoreTable.Add(newScore);
                i++;
            }
        }

        public void CreateSectionsFromJArray(JArray array)
        {
            Sections = new List<EvalSection>();
            foreach (JObject item in array)
            {
                EvalSection newSection = new EvalSection();
                JArray questionArray = (JArray)item.SelectToken("questions");
                item.Remove("questions");
                newSection = item.ToObject<EvalSection>();
                if(newSection.SectionType == "matrix")
                {
                    List<LinkertQuestion> questions = questionArray.ToObject<List<LinkertQuestion>>();
                    newSection.questions = new List<IQuestion>();
                    foreach (var q in questions)
                    {
                        q.setDesc();
                        newSection.questions.Add(q);
                    }
                }
                else if (newSection.SectionType == "text")
                {
                    List<TextQuestion> questions = questionArray.ToObject<List<TextQuestion>>();
                    newSection.questions = new List<IQuestion>();
                    foreach (var q in questions)
                    {
                        newSection.questions.Add(q);
                    }
                }
                Sections.Add(newSection);
            }
        }

        public void FillEvaluation(JObject evalData)
        {
            EvalID = (int)evalData.SelectToken("EvalID");
            Date = (DateTime)evalData.SelectToken("TimeCreated");
            EvalueeID = (int)evalData.SelectToken("EmployeeID");
            EvaluatorID = (int)evalData.SelectToken("EvaluatorID");
            personaldata.Year = evalData.SelectToken("forYear").ToString();
            personaldata.isMidTerm = (bool)evalData.SelectToken("isMidTerm");
            personaldata.EvaluatorID = EvaluatorID;
            personaldata.EmployeeID = EvalueeID;
            var sectionsArray = (JArray)evalData.SelectToken("Sections");
            foreach (var item in Sections)
            {
                var sectionJson = (JArray)sectionsArray.Where(x => (int)x.SelectToken("Order") == item.Order).Single().SelectToken("questions");
                for (int i = 0; i < item.questions.Count; i++)
                {
                    //var derp = sectionJson[i].SelectToken("savedvalue");
                    item.questions[i].savedvalue = (string)sectionJson[i].SelectToken("savedvalue");

                }

            }
        }
    }

    public class personaldata
    {
        public string Year { get; set; }
        public bool isMidTerm { get; set; }
        public int EmployeeID { get; set; }
        public int EvaluatorID { get; set; }

        public personaldata(string year,int empID,int evID,bool mT = false)
        {
            Year = year;
            EmployeeID = empID;
            EvaluatorID = evID;
            isMidTerm = mT;
        }
    }

}
