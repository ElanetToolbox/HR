using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Data.Models
{
    public class TextQuestion : IQuestion
    {
        public int order { get; set; }
        public string text { get; set; }
        public string savedvalue { get; set; }

        public void Create(JObject jOption)
        {
            order = (int)jOption.SelectToken("order");
            text = (string)jOption.SelectToken("text");
            savedvalue = (string)jOption.SelectToken("savedvalue");
        }
    }
}
