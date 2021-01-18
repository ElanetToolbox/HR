using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Data.Models
{
    public class LinkertOption
    {
        public int value { get; set; }
        public string text { get; set; }
        public string description { get; set; }

        public void Create(JObject jOption)
        {
            value = (int)jOption.SelectToken("value");
            text = (string)jOption.SelectToken("text");
            description = (string)jOption.SelectToken("description");
        }
    }
}
