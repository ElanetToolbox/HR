using HR.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HR.Web.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Verify(Account acc)
        {
            if (acc.Username == "1" && acc.Password == "1")
            {
                return RedirectToAction("Index", "Employees");
                ////return View("~/Views/Employees/Index.cshtml");
            }
            else
            {
                return View("");
            }
        }
    }
}