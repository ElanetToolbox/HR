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

        public EmployeesController()
        {
            db = new JsonEmpData();
        }
        // GET: Employees
        public ActionResult Index()
        {
            var model = db.GetAll();
            return View(model);
        }

        public ActionResult Details(int ID)
        {
            var model = db.Get(ID);
            return View(model);
        }
    }
}