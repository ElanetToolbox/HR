using HR.Data.Models.EmployeeModels;
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
        public int EpanekID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string FullName { get; set; }
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
        public string SupervisorString { get; set; }
        public string MapRef { get; set; }
        public bool isActive { get; set; }
        public string floor => string.IsNullOrWhiteSpace(MapRef) ? "" : MapRef.Split('-')[0];
        public string room => string.IsNullOrWhiteSpace(MapRef) ? "" : MapRef.Split('-')[1];
        public string spot => string.IsNullOrWhiteSpace(MapRef) ? "" : MapRef.Split('-')[2];
        public bool isEditor => editors().Contains(ID);
        public bool isHR => hr().Contains(ID);
        public bool isDirector => GetDirector();

        public string SpecialPosition { get; set; }

        public List<Evaluation> Evaluations { get; set; }
        private string evalStr { get; set; }
        public bool isEvaluated { get; set; }

        [JsonIgnore]
        public int Age => DoB == null ? 0 : Functions.GetAge(DoB.Value);

        public void FromJobjectSimple(JObject obj)
        {
            Evaluations = new List<Evaluation>();
            Dictionary<string, object> dict = obj.ToObject<Dictionary<string, object>>();
            if (dict.ContainsKey("id"))
            {
                ID = int.Parse(dict["id"].ToString());
            }
            if (dict.ContainsKey("epanek_id"))
            {
                EpanekID = int.Parse(dict["epanek_id"].ToString());
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
                try
                {
                    Specialty = dict["FYEO_Specialty"].ToString();
                }
                catch { Specialty = ""; }
            }
            if (dict.ContainsKey("HRApp_TitleComputed"))
            {
                Position = dict["HRApp_TitleComputed"].ToString();
            }
            if (dict.ContainsKey("Email"))
            {
                try
                {
                    email = dict["Email"].ToString();
                }
                catch
                {
                    email = "";
                }
            }
            if (dict.ContainsKey("Seat"))
            {
                Seat = dict["Seat"].ToString();
            }
            if (dict.ContainsKey("Phone"))
            {
                Phone = dict["Phone"].ToString();
            }
            if (dict.ContainsKey("IsActive"))
            {
                var a = dict["IsActive"].ToString();
                if(a == "1")
                {
                    isActive = true;
                }
                //isActive = bool.Parse(dict["isActive"].ToString());
            }
            if (dict.ContainsKey("HRApp_Special_Position"))
            {
                try
                {
                    SpecialPosition = dict["HRApp_Special_Position"].ToString();
                }
                catch { SpecialPosition = ""; }
                if (!string.IsNullOrWhiteSpace(SpecialPosition))
                {
                    if (!string.IsNullOrWhiteSpace(Position))
                    {
                        Position = SpecialPosition + ", " + Position;
                    }
                    else
                    {
                        Position = SpecialPosition;
                    }
                }
            }
            if (dict.ContainsKey("MapRef"))
            {
                try
                {
                    MapRef = dict["MapRef"].ToString();
                }
                catch
                {
                    MapRef = "";
                }
            }
            if (dict.ContainsKey("FriendlyFullname"))
            {
                FullName = dict["FriendlyFullname"].ToString();
            }
            if (dict.ContainsKey("FYEO_Evaluations"))
            {
                evalStr = dict["FYEO_Evaluations"].ToString();
                //JArray jEvals = (JArray)obj.SelectToken("FYEO_Evaluations");
                //foreach (var e in jEvals)
                //{
                //    ThinEvaluation tEval = new ThinEvaluation();
                //    tEval.Create(e);
                //}
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
            if(SpecialPosition == "Γενικός Διευθυντής")
            {
                Team nt = new Team();
                nt.Position = 99;
                Teams.Add(nt);
            }
        }
        
        public void GetEvalStatus(int evaluatorID)
        {
            try
            {
                if (evaluatorID == 99 || evaluatorID == 8 || evaluatorID == 114)
                {
                    isEvaluated = evalStr.Contains("isMidTerm");
                }
                else
                {
                    isEvaluated = evalStr.Contains(evaluatorID.ToString() + "}");
                }
            }
            catch
            {

            }
        }

        private bool GetDirector()
        {
            try
            {
                if(Teams.Where(x=>x.Position == 90).Any())
                {
                    return true;
                }
                if(SpecialPosition == "Νομική Σύμβουλος" || SpecialPosition == "Υπεύθυνος Ανθρωπίνων πόρων")
                {
                    return true;
                }

            }
            catch { }
            return false;
        }

        public string TeamsString()
        {
            string teams = "";
            int i = 0;
            foreach (var t in Teams)
            {
                if (i > 0)
                {
                    teams += "+";
                }
                teams += t.Position.ToString(Functions.decimalFormat) + "@[" + t.Name+"]";
                i++;
            }
            return teams;
        }

        public void FromJobjectFull(JObject obj)
        {
            FromJobjectSimple(obj);
            Dictionary<string, object> dict = obj.ToObject<Dictionary<string, object>>();
            if (dict.ContainsKey("photo_blob_html_image"))
            {
                try
                {
                    photo_blob_html_image = dict["photo_blob_html_image"].ToString();
                }
                catch
                {
                    photo_blob_html_image = "";
                }
            }
        }

        public void GetSupervisor(IEnumerable<Employee> emps)
        {
            List<Employee> supervisors = new List<Employee>();
            if (isDirector)
            {
                var dManager = emps.Where(x => x.SpecialPosition == "Γενικός Διευθυντής");
                supervisors.AddRange(dManager);
            }
            else
            {
                foreach (var team in Teams.Where(x => x.Position != 0))
                {
                    //var tEmps = emps.Where(x => x.Teams.Where(y => y.Name == team.Name).Any()).OrderByDescending(x => x.Teams.Where(y => y.Name == team.Name).First().Position);
                    var tEmps = emps.Where(x=>x.ID != ID).Where(x => x.Teams.Where(y => y.Name == team.Name && y.Position > 70 && y.Position > team.Position + 1).Any());
                    supervisors.AddRange(tEmps);
                }
                if (supervisors.Count == 0)
                {
                    var dManager = emps.Where(x => x.Teams.Where(y => y.Position == 90).Any()).Where(x => x.Directorate == Directorate);
                    supervisors.AddRange(dManager);
                }
            }
            SupervisorString = string.Join(", ", supervisors.Where(x=>x.ID != ID).Select(x=>x.FullName).Distinct());
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
