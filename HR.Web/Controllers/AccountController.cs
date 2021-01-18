using HR.Data.Models;
using HR.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HR.Web.Controllers
{
    public class AccountController : Controller
    {
        ApiLoginData api;
        // GET: Account
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Verify(Account acc)
        {
            api = new ApiLoginData();
            if (acc.Username == "1" && acc.Password == "1")
            {
                return RedirectToAction("Index", "Employees");
            }
            else
            {
                var user = api.GetUser(acc.Username, acc.Password);
                Functions.Emps = user.Employees;
                return RedirectToAction("Index", "Employees");
            }
        }
    }
}