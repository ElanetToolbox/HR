using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Data.Models
{
    public class LinkertQuestion : IQuestion
    {
        public int order { get; set; }
        public string fullText { get; set; }
        public string text { get; set; }
        public string Title { get; set; }
        public List<LinkertOption> options { get; set; }
        public string savedvalue { get; set; }
        public string descriptions { get; set; }

        public void CreateOld(JObject jQuestion)
        {
            options = new List<LinkertOption>();
            order = (int)jQuestion.SelectToken("order");
            fullText = (string)jQuestion.SelectToken("text");
            SplitText();
            JArray jOptions = (JArray)jQuestion.SelectToken("columns");
            foreach (var o in jOptions)
            {
                LinkertOption newOption = new LinkertOption();
                newOption.Create((JObject)o);
                options.Add(newOption);
            }
            setDesc();
        }

        public void setDesc()
        {
            //int i = 1;
            descriptions = "";
            for (int j = 1; j <= options.Count; j++)
            {
                string currentDesc = options.Where(x => x.value == j).Single().text;
                List<LinkertOption> sameOptions = options.Where(x => x.text == currentDesc).ToList();
                string numbers = string.Join(",",sameOptions.Select(x=>x.value.ToString()));
                descriptions += numbers + " - " + currentDesc + "\n";
                j += sameOptions.Count() - 1;
            }
            //foreach (var opt in options)
            //{
            //    descriptions += i.ToString() + " - " + opt.description + "\n";
            //    i++;
            //}
        }

        public void SplitText()
        {
            fullText = fullText.Replace("=>", " ");
            int i = fullText.IndexOf("\n");
            Title = fullText.Substring(0, i+1);
            text = fullText.Substring(i+1);
        }
        
    }
}
