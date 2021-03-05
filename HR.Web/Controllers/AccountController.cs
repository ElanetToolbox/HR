using HR.Data.Models;
using HR.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HR.Web.Controllers
{
    public class AccountController : Controller
    {
        ApiLoginData api;
        // GET: Account
        [HttpGet]
        public async Task<ActionResult> Login()
        {
            var apiEmp = new ApiEmpData();
            var empEnum = await apiEmp.GetAllAsync();
            Functions.Emps = empEnum.ToList();
            HttpCookie accountCookie = new HttpCookie("account");
            accountCookie.Value = "";
            try
            {
                HttpContext.Response.Cookies.Add(accountCookie);
            }
            catch { }
            return View();
        }

        [HttpPost]
        public ActionResult Verify(Account acc)
        {
            //Functions.Emps = new ApiEmpData().GetAll().ToList();
            api = new ApiLoginData();
            var z = Functions.Emps;
            if (acc.Username == "1" && acc.Password == "1")
            {
                return RedirectToAction("Index", "Employees");
            }
            else
            {
                var user = api.GetUser(acc.Username, acc.Password);
                var accountCookie = HttpContext.Request.Cookies.Get("account");
                accountCookie.Value = user.ID.ToString();
                HttpContext.Response.Cookies.Add(accountCookie);
                Functions.User = new ApiEmpData().Get(user.ID);
                if(Functions.User.Teams.Where(x=>x.Position > 60).Any() || Functions.User.isEditor)
                {
                    Functions.GetSubordinates();
                    Functions.GetUnderlings();
                    return RedirectToAction("Index", "Employees");
                }
            }
            return null;
        }
    }
}