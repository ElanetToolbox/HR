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
    public class ApiEvaluationData
    {
        public Evaluation GetClearEval()
        {
            var client = new RestClient("https://api.elanet.gr/wp-json/hr-app/v3/evaluations/current");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            IRestResponse response = client.Execute(request);
            string result = response.Content;
            dynamic eval = JsonConvert.DeserializeObject(result);
            Evaluation e = new Evaluation();
            e.CreateSections(eval.questions);
            return e;
        }
    }
}
