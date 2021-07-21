using HR.Data.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Data
{
    public static class Functions
    {
        public static NumberFormatInfo decimalFormat => GetDecimalFormat();

        public static int GetAge(DateTime birthDate)
        {
            DateTime now = DateTime.Now;
            int age = now.Year - birthDate.Year;

            if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
                age--;

            return age;
        }

        public static string GetSectionType(string cellValue)
        {
            switch (cellValue)
            {
                case "Linkert":
                    return "matrix";
                case "Text":
                    return "text";
                default:
                    return "";
            }
        }

        private static NumberFormatInfo GetDecimalFormat()
        {
            NumberFormatInfo result = new NumberFormatInfo();
            result.NumberDecimalSeparator = ".";
            return result;
        }

        public static string NumToLetter(int num)
        {
            switch (num)
            {
                case 1:
                    return "Α";
                case 2:
                    return "Β";
                case 3:
                    return "Γ";
                case 4:
                    return "Δ";
                default:
                    return null;
            }
        }

        public static Evaluation GetFromToken(JObject e)
        {
            //string s = e.Last.ToString();
            Evaluation newEval = new Evaluation();
            JObject eval = e;
            //JObject eval = (JObject)JsonConvert.DeserializeObject(s);
            newEval.Date = DateTime.Parse(eval.SelectToken(nameof(newEval.Date)).ToString());
            newEval.EvalID = int.Parse(e.First.ToString());
            newEval.EvaluatorID = int.Parse(eval.SelectToken(nameof(newEval.EvaluatorID)).ToString());
            newEval.EvalueeID = int.Parse(eval.SelectToken(nameof(newEval.EvalueeID)).ToString());
            newEval.personaldata = JsonConvert.DeserializeObject<personaldata>(eval.SelectToken(nameof(newEval.personaldata)).ToString());
            newEval.ScoreTable = JsonConvert.DeserializeObject<List<ScoreTableEntry>>(eval.SelectToken(nameof(newEval.ScoreTable)).ToString());
            newEval.Sections = new List<EvalSection>();
            JArray jSections = (JArray)eval.SelectToken(nameof(newEval.Sections));
            foreach (var sec in jSections)
            {
                EvalSection newSection = new EvalSection();
                newSection.Order = int.Parse(sec.SelectToken(nameof(newSection.Order)).ToString());
                newSection.Weight = float.Parse(sec.SelectToken(nameof(newSection.Weight)).ToString());
                newSection.Grade = float.Parse(sec.SelectToken(nameof(newSection.Grade)).ToString());
                newSection.Title = sec.SelectToken(nameof(newSection.Title)).ToString();
                newSection.SectionType = sec.SelectToken(nameof(newSection.SectionType)).ToString();
                JArray jQuestions = (JArray)sec.SelectToken(nameof(newSection.questions));
                newSection.questions = new List<IQuestion>();
                foreach (var q in jQuestions)
                {
                    if (newSection.SectionType == "text")
                    {
                        newSection.questions.Add(JsonConvert.DeserializeObject<TextQuestion>(q.ToString()));
                    }
                    else
                    {
                        var lq = JsonConvert.DeserializeObject<LinkertQuestion>(q.ToString());
                        lq.setDesc();
                        newSection.questions.Add(lq);
                    }
                }
                newEval.Sections.Add(newSection);
            }
            return newEval;
        }

        public static Evaluation TemplateFromJobject(JObject item)
        {
            Evaluation newTemplate;
            var evalObject = (JObject)item.SelectToken("JSONDATA");
            var sectionsArray = (JArray)evalObject.SelectToken("Sections");
            evalObject.Remove("Sections");
            newTemplate = evalObject.ToObject<Evaluation>();
            newTemplate.TemplateID = (int)item.SelectToken("TemplID");
            newTemplate.CreateSectionsFromJArray(sectionsArray);
            return newTemplate;
        }

    }
}
