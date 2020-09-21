using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Data.Models
{
    [JsonObject(MemberSerialization.OptOut)]
    public class Employee
    {
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime? DateHired { get; set; }
        public DateTime? DoB { get; set; }
        public string Directorate { get; set; }
        public string Specialty { get; set; }
        public string Position { get; set; }
        public string photo_blob_html_image { get; set; }
        public string Supervisor { get; set; }
        public decimal MixedIncome { get; set; }
        public IEnumerable<string> Documents { get; set; }

        [JsonIgnore]
        public int Age => DoB == null ? 0 : Functions.GetAge(DoB.Value);
        [JsonIgnore]
        public string FullName => LastName + " " + FirstName;

        public void FromJobjectSimple(JObject obj)
        {
            Dictionary<string, object> dict = obj.ToObject<Dictionary<string, object>>();
            if (dict.ContainsKey("id"))
            {
                ID = int.Parse(dict["id"].ToString());
            }
            if (dict.ContainsKey("LastName"))
            {
                LastName = dict["LastName"].ToString();
            }
            if (dict.ContainsKey("FirstName"))
            {
                FirstName = dict["FirstName"].ToString();
            }
            if (dict.ContainsKey("DateHired"))
            {
                if (dict["DateHired"].ToString() != "") 
                {
                    DateHired = DateTime.Parse(dict["DateHired"].ToString()); 
                }
            }
            if (dict.ContainsKey("DoB"))
            {
                if (dict["DoB"].ToString() != "")
                {
                    DoB = DateTime.Parse(dict["DoB"].ToString());
                }
            }
            if (dict.ContainsKey("Directorate"))
            {
                Directorate = dict["Directorate"].ToString();
            }
            if (dict.ContainsKey("Specialty"))
            {
                Specialty = dict["Specialty"].ToString();
            }
            if (dict.ContainsKey("Position"))
            {
                Position = dict["Position"].ToString();
            }
        }

        public void FromJobjectFull(JObject obj)
        {
            FromJobjectSimple(obj);
            Dictionary<string, object> dict = obj.ToObject<Dictionary<string, object>>();
            if (dict.ContainsKey("photo_blob_html_image"))
            {
                photo_blob_html_image = dict["photo_blob_html_image"].ToString();
            }
        }
    }
}
