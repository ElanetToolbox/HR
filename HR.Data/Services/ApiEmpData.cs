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
    public class ApiEmpData 
    {
        public void UpdateEmployee(Employee emp)
        {
            var client = new RestClient("https://api.elanet.gr/wp-json/hr-app/v4/users/"+emp.ID);
            client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            client.Timeout = -1;
            var request = new RestRequest(Method.PATCH);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            request.AddParameter("firstname", emp.FirstName);
            request.AddParameter("lastname", emp.LastName);
            request.AddParameter("fullname", emp.FullName);
            request.AddParameter("birthdate", emp.DoB.Value.ToString("dd/MM/yyyy"));
            request.AddParameter("datehired", emp.DateHired.Value.ToString("dd/MM/yyyy"));
            request.AddParameter("phone", emp.Phone);
            request.AddParameter("depts", emp.TeamsString());
            request.AddParameter("directorate", emp.Directorate);
            request.AddParameter("specialty", emp.Specialty);
            request.AddParameter("specialtitle", emp.SpecialPosition);
            request.AddParameter("MapRef", emp.MapRef);

            IRestResponse response = client.Execute(request);
        }
    }
}
