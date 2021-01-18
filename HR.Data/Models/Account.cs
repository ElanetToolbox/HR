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
        public string Role { get; set; }
        public List<Employee> Employees { get; set; }

        public void CreateUser(JObject acc)
        {
            JObject depts = (JObject)acc.SelectToken("can_evaluate");
            string empIDs ="";
            foreach (KeyValuePair<string,JToken> item in depts)
            {
                empIDs += (string)item.Value + ",";
            }
            empIDs = empIDs.Substring(0, empIDs.Length - 1);
            List<string> emps = empIDs.Split(',').Distinct().ToList();
            Employees = new List<Employee>();
            ApiEmpData api = new ApiEmpData();
            foreach (string emp in emps)
            {
                if (emp != "") 
                {
                    Employees.Add(api.Get(int.Parse(emp)));
                }
            }
            Employees = Employees.OrderBy(x => x.LastName).ToList();
        }
    }
}
