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
        // GET: Employees
        public ActionResult Index(Account user)
        {
            //var model = db.GetAll();
            var model = Functions.Emps;
            return View(model);
        }

        public ActionResult Details(int ID)
        {
            var model = db.Get(ID);
            currentEmp = model;
            return View(model);
        }

        public ActionResult Evaluate()
        {
            return RedirectToAction("Index", "Evaluation");
        }
    }
}