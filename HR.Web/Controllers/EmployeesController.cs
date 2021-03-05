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
        IEmployeeData db;
        Employee currentEmp;

        public EmployeesController()
        {
            db = new ApiEmpData();
        }

        public ActionResult Index(Account user = null)
        {
            IEnumerable<Employee> model;
            if (Functions.Emps == null)
            {
                model = db.GetAll();
            }
            else
            {
                model = Functions.Underlings;
            }
            ViewBag.Title = "Προσωπικό";
            return View(model);
        }

        public ActionResult Evaluation()
        {
            ViewBag.Title = "Αξιολογήσεις";
            return View("Index", Functions.Subordinates);
        }

        public ActionResult Details(int ID)
        {
            var model = db.Get(ID);
            model.GetSupervisor(Functions.Emps);
            currentEmp = model;
            currentEmp.Evaluations = new List<Evaluation>();
            currentEmp.Evaluations = new ApiEvaluationData().GetEmpEvaluations(ID);
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
            Employee employee = db.Get(id);
            Functions.Positions = db.GetPositions().ToList();
            Functions.Departments = db.GetDepartments().ToList();
            return View(employee);
        }
        
        [HttpPost]
        public ActionResult Edit(Employee emp)
        {
            emp.Teams = new List<Team>();
            var positions = Request.Form["positions"].Split(',');
            var departments = Request.Form["departments"].Split(',');
            emp.DateHired = Functions.DateFromInput(Request.Form["DateHired"]);
            emp.DoB = Functions.DateFromInput(Request.Form["DoB"]);
            emp.MapRef = Request.Form["Floor"] + "-" + Request.Form["Room"] + "-" + Request.Form["Spot"];
            for (int i = 0; i < positions.Length; i++)
            {
                if(!string.IsNullOrWhiteSpace(positions[i]) && !string.IsNullOrWhiteSpace(departments[i]))
                {
                    emp.Teams.Add(new Team { Name = departments[i], Position = float.Parse(positions[i]) });
                }
            }
            db.UpdateEmployee(emp);
            return Details(emp.ID);
        }
    }
}