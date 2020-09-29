using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace HR.Data.Models
{
	public class User
	{
		public int id { get; set; }
		public bool is_HR { get; set; }
		public bool is_Editor { get; set; }
		public List<Employee> subordinates { get; set; }

		public void FromJsonObject(JObject obj)
		{
            Dictionary<string, object> dict = obj.ToObject<Dictionary<string, object>>();
			if (dict.ContainsKey("id"))
			{
				id = int.Parse(dict["id"].ToString());
			}
			if (dict.ContainsKey("is_HR"))
			{
				is_HR = bool.Parse(dict["is_HR"].ToString());
			}
			if (dict.ContainsKey("is_Editor"))
			{
				is_Editor = bool.Parse(dict["is_Editor"].ToString());
			}
			if (dict.ContainsKey("can_evaluate"))
			{
				subordinates = new List<Employee>();
				//JArray ja = (JArray)dict["can_evaluate"];
				//List<object> emps = ((JArray)dict["can_evaluate"]).ToObject<List<object>>();
				Dictionary<string, object> eval_dict = ((JObject)dict["can_evaluate"]).ToObject<Dictionary<string, object>>();
				List<object> emps = new List<object>();
				foreach (KeyValuePair<string, object> item in eval_dict)
				{
					List<object> vEmps = JsonConvert.DeserializeObject<List<object>>(item.Value.ToString());
					emps.AddRange(vEmps);
				}
				foreach (JObject item in emps)
				{
					Employee newEmp = new Employee();
					newEmp.FromJobjectSimple(item);
					subordinates.Add(newEmp);
				}
			}
			//foreach (string propName in GetType().GetProperties().Select(x=>x.Name))
			//{
			//	if (dict.ContainsKey(propName))
			//	{
			//		var propInfo = GetType().GetProperty(propName);
			//		propInfo.SetValue(this,dict)

			//	}
			//}
		}
	}
}
