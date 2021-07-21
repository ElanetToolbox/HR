using HR.Data.Models;
using HR.Data.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HR.Web.Controllers
{
    public class EmployeesController : Controller
    {
        ApiEmpData db;
        Employee currentEmp;

        public EmployeesController()
        {
            db = new ApiEmpData();
        }

        public ActionResult Index(Account user = null)
        {
            rContext currentContext = Session[nameof(currentContext)] as rContext;
            IEnumerable<Employee> model;
            model = currentContext.Underlings;
            model.ToList().ForEach(x => x.GetEvalStatus(currentContext.User.ID));
            string emps = string.Join("\n", currentContext.Subordinates.Select(x => x.FullName).ToList());
            ViewBag.Title = "Προσωπικό";
            return View(model);
        }

        public ActionResult Evaluation()
        {
            rContext currentContext = Session[nameof(currentContext)] as rContext;
            ViewBag.Title = "Αξιολογήσεις";
            return View("Index", currentContext.Subordinates);
        }

        public ActionResult Details(int ID=0)
        {
            rContext currentContext = Session[nameof(currentContext)] as rContext;
            if (ID == 0)
            {
                ID = currentContext.User.ID;
            }
            currentContext.GetEmpEvaluations(ID);
            var model = currentContext.Emps.Where(x=>x.ID == ID).Single();
            model.GetSupervisor(currentContext.Emps);
            currentEmp = model;
            //currentEmp.Evaluations = new List<Evaluation>();
            //currentEmp.Evaluations = new ApiEvaluationData().GetEmpEvaluations(ID);
            return View("Details",model);
        }

        public ActionResult Evaluate(int empID)
        {
            return RedirectToAction("Index", "Evaluation", empID);
        }

        public PartialViewResult TeamsPartial(Team model)
        {
            var result = PartialView("_TeamSelect", model);
            return result;
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            rContext currentContext = Session[nameof(currentContext)] as rContext;
            Employee employee = currentContext.Emps.Where(x=>x.ID == id).Single();
            currentContext.Positions = currentContext.Positions;
            currentContext.Departments = currentContext.Departments;
            return View(employee);
        }
        
        [HttpPost]
        public ActionResult Edit(Employee emp)
        {
            rContext currentContext = Session[nameof(currentContext)] as rContext;
            emp.Teams = new List<Team>();
            var positions = Request.Form["positions"].Split(',');
            var departments = Request.Form["departments"].Split(',');
            emp.DateHired = Functions.DateFromInput(Request.Form["DateHired"]);
            emp.DoB = Functions.DateFromInput(Request.Form["DoB"]);
            emp.MapRef = Request.Form["Floor"] + "-" + Request.Form["Room"] + "-" + Request.Form["Spot"];
            positions = positions.Where(x => x != "0").ToArray();
            for (int i = 0; i < positions.Length; i++)
            {
                if(!string.IsNullOrWhiteSpace(positions[i]) && !string.IsNullOrWhiteSpace(departments[i]))
                {
                    Team newTeam = new Team();
                    newTeam.Position = float.Parse(positions[i],Functions.decimalFormat);
                    newTeam.Name = departments[i];
                    emp.Teams.Add(newTeam);
                }
            }
            db.UpdateEmployee(emp);
            currentContext.UpdateEmployee(emp);
            return Details(emp.ID);
        }
    }
}