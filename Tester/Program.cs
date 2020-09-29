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
using RestSharp;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new RestClient("https://www.elanet.gr/wp-json/hr-app/v3/login");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddParameter("user", "tsidimas.o");
            request.AddParameter("pass", "everton21##!");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            int x = 1;
        }

    }
}
