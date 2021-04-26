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
            e.CreateScoreTable(eval.scoretable);
            return e;
        }

        public void UploadEval(Evaluation e)
        {
            var client = new RestClient("https://api.elanet.gr/wp-json/hr-app/v3/evaluations/");
            client.Timeout = -1;
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", e.GetJson(), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
        }

        public void UpdateEval(Evaluation e)
        {
            var client = new RestClient("https://api.elanet.gr/wp-json/hr-app/v3/evaluations/"+e.EvalID);
            client.Timeout = -1;
            var request = new RestRequest(Method.PATCH);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", e.GetJson(), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
        }

        public Evaluation GetByID(int evalID)
        {
            var client = new RestClient("https://api.elanet.gr/wp-json/hr-app/v3/evaluations/");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("criteria", "EvalID=" + evalID);
            IRestResponse response = client.Execute(request);
            dynamic evals = JsonConvert.DeserializeObject(response.Content);
            var jEvals =(JArray)JsonConvert.DeserializeObject(evals.Evaluations.Value);
            return GetFromToken(jEvals[0]);
        }

        public List<Evaluation> GetEmpEvaluations(int EvalueeID)
        {
            var client = new RestClient("https://api.elanet.gr/wp-json/hr-app/v3/evaluations/");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("criteria", "EmployeeID=" + EvalueeID);
            IRestResponse response = client.Execute(request);
            dynamic evals = JsonConvert.DeserializeObject(response.Content);
            try
            {
                var jEvals = (JArray)JsonConvert.DeserializeObject(evals.Evaluations.Value);
                List<Evaluation> results = new List<Evaluation>();
                foreach (var e in jEvals)
                {
                    Evaluation newEval = GetFromToken(e);
                    results.Add(newEval);
                }
                return results.ToList();
            }
            catch
            {
                return new List<Evaluation>();
            }
        }

        private static Evaluation GetFromToken(JToken e)
        {
            string s = e.Last.ToString();
            Evaluation newEval = new Evaluation();
            JObject eval = (JObject)JsonConvert.DeserializeObject(s);
            newEval.Date = DateTime.Parse(eval.SelectToken(nameof(newEval.Date)).ToString());
            newEval.EvalID = int.Parse(e.First.ToString());
            newEval.EvaluatorID = int.Parse(eval.SelectToken(nameof(newEval.EvaluatorID)).ToString());
            newEval.EvalueeID = int.Parse(eval.SelectToken(nameof(newEval.EvalueeID)).ToString());
            newEval.personaldata = JsonConvert.DeserializeObject<personaldata>(eval.SelectToken(nameof(newEval.personaldata)).ToString());
            newEval.ScoreTable = JsonConvert.DeserializeObject<List<ScoreTableEntry>>(eval.SelectToken(nameof(newEval.ScoreTable)).ToString());
            newEval.Sections = new List<EvalSection>();
            JArray jSections = (JArray)eval.SelectToken(nameof(newEval.Sections));
            foreach (var sec in jSections)
            {
                EvalSection newSection = new EvalSection();
                newSection.Order = int.Parse(sec.SelectToken(nameof(newSection.Order)).ToString());
                newSection.Weight = float.Parse(sec.SelectToken(nameof(newSection.Weight)).ToString());
                newSection.Grade = float.Parse(sec.SelectToken(nameof(newSection.Grade)).ToString());
                newSection.Title = sec.SelectToken(nameof(newSection.Title)).ToString();
                newSection.SectionType = sec.SelectToken(nameof(newSection.SectionType)).ToString();
                JArray jQuestions = (JArray)sec.SelectToken(nameof(newSection.questions));
                newSection.questions = new List<IQuestion>();
                foreach (var q in jQuestions)
                {
                    if (newSection.SectionType == "text")
                    {
                        newSection.questions.Add(JsonConvert.DeserializeObject<TextQuestion>(q.ToString()));
                    }
                    else
                    {
                        var lq = JsonConvert.DeserializeObject<LinkertQuestion>(q.ToString());
                        lq.setDesc();
                        newSection.questions.Add(lq);
                    }
                }
                newEval.Sections.Add(newSection);
            }
            return newEval;
        }

        public void UploadEvaluationTemplate(Evaluation e)
        {
            var client = new RestClient("https://api.elanet.gr/wp-json/hr-app/v3/templates/");
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Content-Type", "application/json");
            string j = e.GetJson();
            request.AddParameter("application/json", e.GetJson(), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
        }

        public Evaluation GetTemplate()
        {
            var client = new RestClient("https://api.elanet.gr/wp-json/hr-app/v3/templates/");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("criteria", "TemplID=4");
            IRestResponse response = client.Execute(request);
            string result = response.Content;
            JObject eval = (JObject)JsonConvert.DeserializeObject(result);
            var d = (JArray)JsonConvert.DeserializeObject(eval.SelectToken("Templates").ToString());
            var x = d.First();

            return GetFromToken(x);
        }

        public Evaluation GetTemplateById(int id)
        {
            var client = new RestClient("https://api.elanet.gr/wp-json/hr-app/v3/templates/");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("criteria", "TemplID="+id.ToString());
            IRestResponse response = client.Execute(request);
            string result = response.Content;
            JObject eval = (JObject)JsonConvert.DeserializeObject(result);
            var d = (JArray)JsonConvert.DeserializeObject(eval.SelectToken("Templates").ToString());
            var x = d.First();

            return GetFromToken(x);
        }
    }
}
