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
        //public Employee Get(int ID)
        //{
        //    string apiUrl = @"https://www.api.elanet.gr/wp-json/hr-app/v3/users/";
        //    //string apiUrl = @"https://www.elanet.gr/wp-json/hr-app/v1/users/"+ID;
        //    WebRequest rGet = WebRequest.Create(apiUrl);
        //    rGet.Method = "POST";
        //    //request.AddParameter("user", "tsidimas.o");
        //    HttpWebResponse resp = null;
        //    resp = (HttpWebResponse)rGet.GetResponse();

        //    string result = null;
        //    using (Stream stream = resp.GetResponseStream())
        //    {
        //        StreamReader sr = new StreamReader(stream);
        //        result = sr.ReadToEnd();
        //        sr.Close();
        //    }
        //    dynamic emp = JsonConvert.DeserializeObject(result);
        //    Employee newEmp = new Employee();
        //    newEmp.FromJobjectFull(emp[0]);
        //    return newEmp;
        //}

        public Employee Get(int ID)
        {
            //string apiUrl = @"https://www.api.elanet.gr/wp-json/hr-app/v3/users/";
            ////string apiUrl = @"https://www.elanet.gr/wp-json/hr-app/v1/users/"+ID;
            //WebRequest rGet = WebRequest.Create(apiUrl);
            //rGet.Method = "POST";
            ////request.AddParameter("user", "tsidimas.o");
            //HttpWebResponse resp = null;
            //resp = (HttpWebResponse)rGet.GetResponse();

            var client = new RestClient("https://api.elanet.gr/wp-json/hr-app/v3/users/");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddParameter("id", ID);
            IRestResponse response = client.Execute(request);
            string result = response.Content;
            //HttpWebResponse resp = (HttpWebResponse)response;


            //string result = null;
            //using (Stream stream = resp.GetResponseStream())
            //{
            //    StreamReader sr = new StreamReader(stream);
            //    result = sr.ReadToEnd();
            //    sr.Close();
            //}
            dynamic emp = JsonConvert.DeserializeObject(result);
            var w = emp.Emps[0];
            Employee newEmp = new Employee();
            newEmp.FromJobjectFull(w);
            return newEmp;
        }

        public IEnumerable<Employee> GetAll()
        {
            string apiUrl = @"https://api.elanet.gr/wp-json/hr-app/v3/users/";
            WebRequest rGet = WebRequest.Create(apiUrl);
            rGet.Method = "POST";
            HttpWebResponse resp = null;
            resp = (HttpWebResponse)rGet.GetResponse();

            string result = null;
            using(Stream stream = resp.GetResponseStream())
            {
                StreamReader sr = new StreamReader(stream);
                result = sr.ReadToEnd();
                sr.Close();
            }
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
            return employees;
        }

        public void Save(Employees emps)
        {
            throw new NotImplementedException();
        }
    }
}
