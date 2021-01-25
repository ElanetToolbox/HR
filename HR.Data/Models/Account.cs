using HR.Data.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Data.Models
{
    public class Account
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool canEdit { get; set; }
        public bool canEvaluate { get; set; }

        public void CreateUser(JObject acc)
        {
            ID = (int)acc.SelectToken("id");
        }
    }
}
