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
        public List<EvalSection> Sections { get; set; }
        public Employee Employee { get; set; }

        public void CreateSections(JArray jSections)
        {
            Sections = new List<EvalSection>();
            foreach (var s in jSections)
            {
                EvalSection newSection = new EvalSection();
                newSection.Create((JObject)s);
                Sections.Add(newSection);
            }
            int x = 1;
        }

        public void FillFromForm(NameValueCollection form)
        {
            var test = form.AllKeys;
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
                    if(q.savedvalue == null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
