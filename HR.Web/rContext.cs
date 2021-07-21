using HR.Data.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace HR.Web
{
    public class rContext
    {
        public Employee User { get; set; }
        public List<Employee> Underlings { get; set; }
        public List<Employee> Subordinates { get; set; }
        public List<Employee> Emps { get; set; }

        public List<Position> Positions { get; set; }
        public List<Department> Departments { get; set; }

        public List<Evaluation> EvaluationTemplates { get; set; }
        public List<KeyValuePair<int,JObject>> TemplateJsons { get; set; }

        public JArray EvaluationData { get; set; }

        public rContext()
        {

            Subordinates = new List<Employee>();
            Underlings = new List<Employee>();
            Emps = new List<Employee>();
            Positions = new List<Position>();
            Departments = new List<Department>();
        }

        public void GetSubordinates()
        {
            if (User.isHR)
            {
                return;
            }
            foreach (var team in User.Teams)
            {
                if (team.Position < 70)
                {
                    break;
                }
                if (team.Position >= 70 && team.Position < 90)
                {
                    var z = Emps.Where(x=>x.ID != User.ID).Where(x => x.Teams.Where(y => y.Name == team.Name).Where(y => y.Position < team.Position).Any()).ToList();
                    Subordinates.AddRange(z);
                }
                else if (team.Position >=90 && team.Position<99)
                {
                    var z = Emps.Where(x=>x.ID != User.ID).Where(x=>x.Directorate == User.Directorate).Where(x => x.Teams.Where(y=> y.Position >= 70).Any()).ToList();
                    z.RemoveAll(x =>x.ID != 82 && x.Teams.Where(y => y.Position < 70 && y.Name == "DRASEIS").Any());
                    Subordinates.AddRange(z);
                }
                else
                {
                    var z = Emps.Where(x=>x.isDirector || x.ID == 177).ToList();
                    Subordinates.AddRange(z);
                }
            }
            if(User.ID == 164)
            {
                rContext newContext = new rContext();
                newContext.Emps = Emps;
                newContext.User = Emps.Where(x => x.ID == 181).Single();
                newContext.GetSubordinates();
                Subordinates.AddRange(newContext.Subordinates.Where(x=>x.ID != 164));
            }
            Subordinates = Subordinates.Distinct().ToList();
            Subordinates.ForEach(x => x.GetEvalStatus(User.ID));
        }
        
        public void UpdateEmployee(Employee emp)
        {
            var oldEmp = Emps.Where(x => x.ID == emp.ID).Single();
            Emps.Remove(oldEmp);
            Emps.Add(emp);
        }

        public void GetUnderlings()
        {
            Underlings = new List<Employee>();
            if (User.isHR || User.isEditor)
            {
                Underlings = Emps;
                return;
            }
            foreach (var team in User.Teams)
            {
                if (team.Position < 70)
                {
                    break;
                }
                if (team.Position >= 70 && team.Position < 90)
                {
                    if (team.Name == "DRASEIS")
                    {
                        List<Team> sTeams = new List<Team>();
                        foreach (var sub in Subordinates)
                        {
                            var sT = sub.Teams.Where(x => x.Position >= 70 && team.Position < 90).ToList();
                            sTeams.AddRange(sT);
                        }
                        sTeams = sTeams.Distinct().ToList();
                        foreach (var item in sTeams)
                        {
                            var z = Emps.Where(x => x.ID != User.ID).Where(x => x.Teams.Where(y => y.Name == item.Name).Where(y => y.Position < item.Position).Any()).ToList();
                            Underlings.AddRange(z);
                        }
                        Underlings = Underlings.Distinct().ToList();
                    }
                    else
                    {
                        Underlings = Subordinates;
                    }
                }
                else if (team.Position >=90 && team.Position<99)
                {
                    var z = Emps.Where(x=>x.ID != User.ID).Where(x=>x.Directorate == User.Directorate).ToList();
                    Underlings.AddRange(z);
                }
                else
                {
                    Underlings = Emps.Where(x=>x.ID != User.ID).ToList();
                }
            }
            Subordinates = Subordinates.Distinct().ToList();
        }

        public void Initialize(LoginInfo info)
        {
            User = info.User;
            Positions = info.Positions;
            Departments = info.Departments;
            Emps = info.Employees;
            EvaluationTemplates = info.EvaluationTemplates;
            EvaluationData = info.EvaluationData;
            TemplateJsons = info.TemplateJsons;
            GetSubordinates();
            GetUnderlings();
        }

        public void GetEmpEvaluations(int EmpID)
        {
            var evals = EvaluationData.Where(x => (int)x.SelectToken("EmployeeID") == EmpID);
            var emp = Emps.Where(x => x.ID == EmpID).Single();
            foreach (JObject item in evals)
            {
                int evaluatorID = (int)item.SelectToken("EvaluatorID");
                var evaluator = Emps.Where(x => x.ID == evaluatorID).Single();
                int templateID = Functions.GetTemplateID(emp, evaluator);
                var templateJson = TemplateJsons.Where(x => x.Key == templateID).Single().Value;
                Evaluation newEval = HR.Data.Functions.TemplateFromJobject((JObject)templateJson.DeepClone());
                newEval.FillEvaluation(item);
                if (!emp.Evaluations.Where(x => x.EvalID == newEval.EvalID).Any())
                {
                    emp.Evaluations.Add(newEval);
                }
            }
        }


    }
}