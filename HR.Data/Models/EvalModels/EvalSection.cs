using IronXL;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Data.Models
{
    public class EvalSection
    {
        public int Order { get; set; }
        public string Title { get; set; }
        public string Header => GetHeader();
        public string SectionType { get; set; }
        public float Weight { get; set; }
        public float Grade { get; set; }
        public List<IQuestion> questions { get; set; }

        public void Create(JObject jSection)
        {
            Title = (string)jSection.SelectToken("name");
            questions = new List<IQuestion>();
            Order = (int)jSection.SelectToken("order");
            Weight = (float)jSection.SelectToken("scoreweight");
            SectionType = (string)jSection.SelectToken("type");
            JArray jQuestions = (JArray)jSection.SelectToken("rows");
            foreach (var q in jQuestions)
            {
                if(SectionType == "matrix")
                {
                    LinkertQuestion newQuestion = new LinkertQuestion();
                    newQuestion.CreateOld((JObject)q);
                    questions.Add(newQuestion);
                }
                if(SectionType == "text")
                {
                    TextQuestion newQuestion = new TextQuestion();
                    newQuestion.Create((JObject)q);
                    questions.Add(newQuestion);
                }
            }
            if (SectionType == "text")
            {
                TextQuestion newQuestion = new TextQuestion();
                newQuestion.order = 3;
                newQuestion.text = "Παρατηρήσεις";
                questions.Add(newQuestion);
            }
        }

        private string GetHeader()
        {
            if(Weight > 0)
            {
                return Title + " (" + Weight * 100 + "%)";
            }
            return Title;
        }

        public void CreateFromExcel(WorkSheet sheet)
        {
            questions = new List<IQuestion>();
            var d = sheet.Columns[0].Rows.ToList();
            d.Remove(d.First());
            d.RemoveAll(x=>x.Int32Value == 0);

            int i = 1;
            foreach (var row in d)
            {
                var qRow = sheet.GetRow(i);
                switch (SectionType)
                {
                    case "matrix":
                        var lq = new LinkertQuestion();
                        lq.order = 1;
                        lq.options = new List<LinkertOption>();
                        lq.Title = sheet.GetCellAt(i, 1).StringValue;
                        lq.text = sheet.GetCellAt(i, 2).StringValue;
                        var columns = qRow.Columns.ToList();
                        columns.RemoveRange(0, 3);
                        for (int j = 0; j < columns.Count; j+=3)
                        {
                            string grades = columns.ElementAt(j).StringValue;
                            if (grades.Contains("-"))
                            {
                                List<string> gradeList = grades.Split('-').ToList();
                                foreach (var g in gradeList)
                                {
                                    LinkertOption opt = new LinkertOption();
                                    opt.value = int.Parse(g);
                                    opt.text = columns[j + 1].StringValue;
                                    opt.description = columns[j + 2].StringValue;
                                    lq.options.Add(opt);
                                }
                            }
                            else
                            {
                                LinkertOption opt = new LinkertOption();
                                opt.value = int.Parse(columns[j].StringValue);
                                opt.text = columns[j + 1].StringValue;
                                opt.description = columns[j + 2].StringValue;
                                lq.options.Add(opt);
                            }
                        }
                        questions.Add(lq);
                        break;
                    case "text":
                        var tq = new TextQuestion();
                        tq.order = i;
                        tq.text = sheet.GetCellAt(i, 1).StringValue;
                        questions.Add(tq);
                        break;
                    default:
                        break;
                }
                i++;
            }
        }
        
        public void CreateFromJobject(JObject obj)
        {
            JArray questionArray = (JArray)obj.SelectToken("questions");
            obj.Remove("questions");



        }

    }
}
