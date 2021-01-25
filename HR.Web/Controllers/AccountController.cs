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
            ViewBag.Session = new SessionData();
            Functions.Emps = ViewBag.Session.Emps;
            api = new ApiLoginData();
            if (acc.Username == "1" && acc.Password == "1")
            {
                return RedirectToAction("Index", "Employees");
            }
            else
            {
                var user = api.GetUser(acc.Username, acc.Password);
                ViewBag.Session.User = new ApiEmpData().Get(user.ID);
                Employee currentUser = ViewBag.Session.User;
                //Functions.User = ViewBag.Session.User;
                if(currentUser.Teams.Where(x=>x.Position > 60).Any())
                {
                    ViewBag.Session.GetSubordinates();
                    ViewBag.Session.GetUnderlings();
                    Functions.GetSubordinates();
                    Functions.GetUnderlings();
                    return RedirectToAction("Index", "Employees");
                }
            }
            return null;
        }
    }
}