using HR.Data.Models;
using HR.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using Newtonsoft.Json.Converters;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            //IEmployeeData empDataService = new JsonEmpData();
            //var derp = empDataService.GetAll();
            //empDataService.Save(new Employees { Emps = derp.ToList() }); 
            string apiUrl = @"https://www.elanet.gr/wp-json/hr-app/v1/users/182";
            WebRequest rGet = WebRequest.Create(apiUrl);
            rGet.Method = "GET";
            HttpWebResponse resp = null;
            resp = (HttpWebResponse)rGet.GetResponse();

            string result = null;
            using(Stream stream = resp.GetResponseStream())
            {
                StreamReader sr = new StreamReader(stream);
                result = sr.ReadToEnd();
                sr.Close();
            }
            dynamic emp = JsonConvert.DeserializeObject(result);
            JObject e = emp[0];
            int x = 1;
        }

    }
}
