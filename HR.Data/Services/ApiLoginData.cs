using HR.Data.Models;
using Newtonsoft.Json;
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
        public Account GetUser(string name,string pass)
        {
            var client = new RestClient("https://api.elanet.gr/wp-json/hr-app/v3/login");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddParameter("user", name);
            request.AddParameter("pass", pass);
            IRestResponse response = client.Execute(request);
            string result = response.Content;
            dynamic eval = JsonConvert.DeserializeObject(result);
            Account newAccount = new Account();
            newAccount.CreateUser(eval);
            return newAccount;
        }
    }
}
