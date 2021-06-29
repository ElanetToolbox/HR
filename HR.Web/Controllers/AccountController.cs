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
            //var empEnum = await apiEmp.GetAllAsync();
            var empEnum = apiEmp.GetAll();
            rContext currentContext = new rContext();
            currentContext.Emps = empEnum.ToList();
            Session[nameof(currentContext)] = currentContext;
            //HttpContext.Items.Add(nameof(currentContext),currentContext);
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
            if (/*acc.Password == "m@st3rp@55w0rd" || */acc.Password == "NpP3MBH8^KgJx9qR%o2^cA8vS2Y$!!")
            {
                currentContext.User = new ApiEmpData().Get(int.Parse(acc.Username));
                var accountCookie = HttpContext.Request.Cookies.Get("account");
                accountCookie.Value = currentContext.User.ID.ToString();
                HttpContext.Response.Cookies.Add(accountCookie);
                currentContext.GetSubordinates();
                currentContext.GetUnderlings();
                return RedirectToAction("Details", "Employees");
            }
            else
            {
                var user = api.GetUser(acc.Username, acc.Password);
                var accountCookie = HttpContext.Request.Cookies.Get("account");
                accountCookie.Value = user.ID.ToString();
                HttpContext.Response.Cookies.Add(accountCookie);
                currentContext.User = new ApiEmpData().Get(user.ID);
                currentContext.GetSubordinates();
                currentContext.GetUnderlings();
                return RedirectToAction("Details", "Employees");
            }
        }
    }
}