using HR.Data.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HR.Data.Services
{
    public class ApiEmpData : IEmployeeData
    {
        public Employee Get(int ID)
        {
            var client = new RestClient("https://api.elanet.gr/wp-json/hr-app/v3/users/");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddParameter("id", ID);
            IRestResponse response = client.Execute(request);
            string result = response.Content;

            dynamic emp = JsonConvert.DeserializeObject(result);
            var w = emp.Emps[0];
            Employee newEmp = new Employee();
            newEmp.FromJobjectFull(w);
            return newEmp;
        }

        public IEnumerable<Employee> GetMany(string IDs)
        {
            var client = new RestClient("https://api.elanet.gr/wp-json/hr-app/v3/users/");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddParameter("id", IDs);
            IRestResponse response = client.Execute(request);
            string result = response.Content;
            dynamic emp = JsonConvert.DeserializeObject(result);
            var w = emp.Emps[0];
            Employee newEmp = new Employee();
            newEmp.FromJobjectFull(w);
            return null;
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
            foreach(JObject emp in elist)
            {
                Employee newEmp = new Employee();
                newEmp.FromJobjectSimple(emp);
                employees.Add(newEmp);
            }
            return employees.Where(x=>x.Phone != "0");
        }

        public IEnumerable<Position> GetPositions()
        {
            var client = new RestClient("https://api.elanet.gr/wp-json/hr-app/v3/jobtitles/");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddParameter("limit", 300);
            IRestResponse response = client.Execute(request);
            string result = response.Content;
            JObject jobs = (JObject)JsonConvert.DeserializeObject(result);
            //List<JObject> plist = new List<JObject>(jobs.HR_Titles);
            JArray plist = (JArray)jobs.SelectToken("HR_Titles");
            List<Position> positions = new List<Position>();
            foreach (var item in plist)
            {
                Position newp = new Position();
                newp.id = (float)item.SelectToken("id");
                newp.description = item.SelectToken("DropdownDescr").ToString();
                positions.Add(newp);
            }
            return positions;
        }

        public IEnumerable<Department> GetDepartments()
        {
            var client = new RestClient("https://api.elanet.gr/wp-json/hr-app/v3/teams/");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddParameter("limit", 300);
            IRestResponse response = client.Execute(request);
            string result = response.Content;
            JObject jobs = (JObject)JsonConvert.DeserializeObject(result);
            //List<JObject> plist = new List<JObject>(jobs.HR_Titles);
            JArray plist = (JArray)jobs.SelectToken("Teams");
            List<Department> depts = new List<Department>();
            foreach (var item in plist)
            {
                Department newp = new Department();
                newp.id = item.SelectToken("id").ToString();
                newp.description = item.SelectToken("DropdownDescr").ToString();
                depts.Add(newp);
            }
            return depts;
        }

        public void Save(Employees emps)
        {
            throw new NotImplementedException();
        }
    }
}
