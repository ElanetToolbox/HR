using HR.Data.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Data.Services
{
    public class ApiLoginData
    {
        public LoginInfo GetLogin(string name, string pass)
        {
            var client = new RestClient("https://api.elanet.gr/wp-json/hr-app/v4/login");
            client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddParameter("user", name);
            request.AddParameter("pass", pass);
            IRestResponse response = client.Execute(request);
            string result = response.Content;
            dynamic info = JsonConvert.DeserializeObject(result);

            LoginInfo loginInfo = new LoginInfo();
            loginInfo.FromJObject(info);

            return loginInfo;
        }
        public IEnumerable<Employee> GetAll()
        {
            var client = new RestClient("https://api.elanet.gr/wp-json/hr-app/v3/users/");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddParameter("limit", 300);
            IRestResponse response = client.Execute(request);
            string result = response.Content;
            dynamic emps = JsonConvert.DeserializeObject(result);
            var wh = emps.Emps;
            var elist = new List<object>(wh);
            List<Employee> employees = new List<Employee>();
            foreach (JObject emp in elist)
            {
                Employee newEmp = new Employee();
                newEmp.FromJobjectSimple(emp);
                employees.Add(newEmp);
            }
            var c = employees.Where(x => x.Phone != "0").Count();
            return employees.Where(x => x.Phone != "0");
        }
    }
}
