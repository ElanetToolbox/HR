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
        public List<Team> Teams { get; set; }
        public string email { get; set; }
        public string Seat { get; set; }
        public string Phone { get; set; }
        public string Supervisors { get; set; }
        public bool isEditor => editors().Contains(ID);
        public bool isHR => hr().Contains(ID);

        public List<Evaluation> Evaluations { get; set; }

        [JsonIgnore]
        public int Age => DoB == null ? 0 : Functions.GetAge(DoB.Value);
        [JsonIgnore]
        public string FullName => LastName + " " + FirstName;

        public void FromJobjectSimple(JObject obj)
        {
            Evaluations = new List<Evaluation>();
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
            if (dict.ContainsKey("FYEO_DateHired"))
            {
                if (dict["FYEO_DateHired"] != null && dict["FYEO_DateHired"].ToString() != "") 
                {
                    DateHired = DateTime.Parse(dict["FYEO_DateHired"].ToString()); 
                }
            }
            if (dict.ContainsKey("FYEO_DoB"))
            {
                if (dict["FYEO_DoB"] != null && dict["FYEO_DoB"].ToString() != "")
                {
                    DoB = DateTime.Parse(dict["FYEO_DoB"].ToString());
                }
            }
            if (dict.ContainsKey("FYEO_Directorate"))
            {
                if (dict["FYEO_Directorate"] != null)
                {
                    Directorate = dict["FYEO_Directorate"].ToString();
                }
            }
            if (dict.ContainsKey("FYEO_Specialty"))
            {
                Specialty = dict["FYEO_Specialty"].ToString();
            }
            if (dict.ContainsKey("HRApp_TitleComputed"))
            {
                Position = dict["HRApp_TitleComputed"].ToString();
            }
            if (dict.ContainsKey("Email"))
            {
                email = dict["Email"].ToString();
            }
            if (dict.ContainsKey("Seat"))
            {
                Seat = dict["Seat"].ToString();
            }
            if (dict.ContainsKey("Phone"))
            {
                Phone = dict["Phone"].ToString();
            }
            if (dict.ContainsKey("HRApp_Teams"))
            {
                Teams = new List<Team>();
                try
                {
                    JArray jTeams = (JArray)obj.SelectToken("HRApp_Teams");
                    foreach (var t in jTeams)
                    {
                        Team newTeam = new Team();
                        newTeam.CreateTeam(t);
                        Teams.Add(newTeam);

                    }
                }
                catch { }
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

        public void GetSupervisor(IEnumerable<Employee> emps)
        {
            List<Employee> supervisors = new List<Employee>();
            foreach (var team in Teams.Where(x=>x.Position != 0))
            {
                var tEmps = emps.Where(x => x.Teams.Where(y => y.Name == team.Name).Any()).OrderByDescending(x => x.Teams.Where(y => y.Name == team.Name).Single().Position).First();
                supervisors.Add(tEmps);
            }
            Supervisors = string.Join(",", supervisors.Select(x=>x.FullName).Distinct());
        }

        public List<int> editors()
        {
            var l = new List<int>();
            l.Add(23);
            return l;
        }

        public List<int> hr()
        {
            var l = new List<int>();
            l.Add(99);
            return l;
        }
    }
}
