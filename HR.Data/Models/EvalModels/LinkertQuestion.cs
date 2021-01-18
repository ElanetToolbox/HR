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
        public int Order { get; set; }
        public string fullText { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public List<LinkertOption> options { get; set; }
        public int savedvalue { get; set; }
        public string descriptions { get; set; }

        public void Create(JObject jQuestion)
        {
            options = new List<LinkertOption>();
            Order = (int)jQuestion.SelectToken("order");
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
            int i = 1;
            descriptions = "";
            foreach (var opt in options)
            {
                descriptions += i.ToString() + " - " + opt.description + "\n";
                i++;
            }
        }

        public void SplitText()
        {
            fullText = fullText.Replace("=>", " ");
            int i = fullText.IndexOf("\n");
            Title = fullText.Substring(0, i+1);
            Text = fullText.Substring(i+1);
        }
        
    }
}
