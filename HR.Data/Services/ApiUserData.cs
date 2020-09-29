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
	public class ApiUserData : IUserData
	{
		public User Get(string username, string password)
		{
            var client = new RestClient("https://www.api.elanet.gr/wp-json/hr-app/v3/login");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddParameter("user", username);
            request.AddParameter("pass", password);
            IRestResponse response = client.Execute(request);
            dynamic user = JsonConvert.DeserializeObject(response.Content);
			if(response.Content == "[]")
			{
				return null;
			}
			User newUser = new User();
			newUser.FromJsonObject(user);
			return newUser;
		}
	}
}
