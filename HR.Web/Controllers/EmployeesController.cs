using HR.Data.Models;
using HR.Data.Services;
using System;
using System.Collections.Generic;
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

        public ActionResult Index(Account user)
        {
            IEnumerable<Employee> model;
            if (Functions.Emps == null)
            {
                model = db.GetAll();
            }
            else
            {
                model = ViewBag.Session.Underlings;
            }
            return View(model);
        }

        public ActionResult Evaluation()
        {
            return View("Index", Functions.Subordinates);
        }

        public ActionResult Details(int ID)
        {
            var model = db.Get(ID);
            model.GetSupervisor(Functions.Emps);
            currentEmp = model;
            return View(model);
        }

        public ActionResult Evaluate()
        {
            return RedirectToAction("Index", "Evaluation");
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
            var d = Request.Form.AllKeys;
            var positions = Request.Form["positions"].Split(',');
            var departments = Request.Form["departments"].Split(',');
            emp.Teams = new List<Team>();
            for (int i = 0; i < positions.Length; i++)
            {
                if(!string.IsNullOrWhiteSpace(positions[i]) && !string.IsNullOrWhiteSpace(departments[i]))
                {
                    emp.Teams.Add(new Team { Name = departments[i], Position = float.Parse(positions[i]) });
                }
            }
            return View();
        }
    }
}