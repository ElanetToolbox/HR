using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HR.Data.Models
{
    public class LoginInfo
    {
        public Employee User { get; set; }
        public List<Employee> Employees { get; set; }

        public List<Position> Positions { get; set; }
        public List<Department> Departments { get; set; }

        public List<Evaluation> EvaluationTemplates { get; set; }
        public List<KeyValuePair<int,JObject>> TemplateJsons { get; set; }

        public JArray EvaluationData { get; set; }

        public LoginInfo()
        {
            Employees = new List<Employee>();
            Positions = new List<Position>();
            Departments = new List<Department>();
            EvaluationTemplates = new List<Evaluation>();
            TemplateJsons = new List<KeyValuePair<int, JObject>>();
        }


        public void FromJObject(JObject obj)
        {
            JArray depts = (JArray)obj.SelectToken("AppVarsTeams");
            JArray pos = (JArray)obj.SelectToken("AppVarsTitles");
            JArray emps = (JArray)obj.SelectToken("Emps");
            //JArray templates  = (JArray)obj.SelectToken("AppVarsTemplates");
            EvaluationData = (JArray)obj.SelectToken("Evaluations");


            string fullPath;

            try
            {
                fullPath = HttpContext.Current.Server.MapPath(@"~\Content\JsonTemplates\Evaluations.json");
            }
            catch
            {
                fullPath = @"C:\Users\chatziparadeisis.i\source\repos\HR\HR.Web\Content\JsonTemplates\Evaluations.json";
            }
            string templateJsonStr = File.ReadAllText(fullPath,Encoding.GetEncoding(1253));
            JArray templates = (JArray)JsonConvert.DeserializeObject(templateJsonStr);

            foreach (var item in depts)
            {
                Department newDept = new Department();
                newDept.id = item.SelectToken("id").ToString();
                newDept.description = item.SelectToken("DropdownDescr").ToString();
                Departments.Add(newDept);
            }

            foreach (var item in pos)
            {
                Position newp = new Position();
                newp.id = (float)item.SelectToken("id");
                newp.description = item.SelectToken("DropdownDescr").ToString();
                Positions.Add(newp);
            }

            foreach (JObject item in emps)
            {
                Employee newEmp = new Employee();
                newEmp.FromJobjectSimple(item);
                if (newEmp.ID != 0)
                {
                    Employees.Add(newEmp);
                }
            }

            foreach (JObject item in templates)
            {
                int tId = (int)item.SelectToken("TemplID");
                KeyValuePair<int, JObject> json = new KeyValuePair<int, JObject>(tId, (JObject)item.DeepClone());
                TemplateJsons.Add(json);
                Evaluation newTemplate = new Evaluation();
                newTemplate = Functions.TemplateFromJobject(item);
                EvaluationTemplates.Add(newTemplate);
            }


            User = Employees.Where(x => x.ID == (int)obj.SelectToken("id")).Single();

        }

    }
}
