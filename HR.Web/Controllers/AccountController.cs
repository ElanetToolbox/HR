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
            rContext currentContext = new rContext();
            Session[nameof(currentContext)] = currentContext;
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
            api = new ApiLoginData();
            rContext currentContext = Session[nameof(currentContext)] as rContext;
            LoginInfo info = api.GetLogin(acc.Username, acc.Password);
            var accountCookie = HttpContext.Request.Cookies.Get("account");
            accountCookie.Value = info.User.ID.ToString();
            HttpContext.Response.Cookies.Add(accountCookie);
            currentContext.Initialize(info);
            return RedirectToAction("Details", "Employees");
        }
    }
}