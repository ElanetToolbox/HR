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
        public string SectionType { get; set; }
        public float Weight { get; set; }
        public float Grade { get; set; }
        public List<IQuestion> questions { get; set; }

        public void Create(JObject jSection)
        {
            Title = (string)jSection.SelectToken("name");
            questions = new List<IQuestion>();
            Order = (int)jSection.SelectToken("order");
            SectionType = (string)jSection.SelectToken("type");
            JArray jQuestions = (JArray)jSection.SelectToken("rows");
            foreach (var q in jQuestions)
            {
                if(SectionType == "matrix")
                {
                    LinkertQuestion newQuestion = new LinkertQuestion();
                    newQuestion.Create((JObject)q);
                    questions.Add(newQuestion);
                }
                if(SectionType == "text")
                {
                    TextQuestion newQuestion = new TextQuestion();
                    newQuestion.Create((JObject)q);
                    questions.Add(newQuestion);
                }
            }
        }

        public float GetGrade()
        {
            if(SectionType == "text")
            {
                return 0;
            }
            else
            {
                int pureGrade = 0;
                foreach (var q in questions)
                {
                    LinkertQuestion question = (LinkertQuestion)q;
                    pureGrade += question.savedvalue;
                }
                Grade = Weight * pureGrade;
                return Weight * pureGrade;
            }
        }

    }
}
