using HR.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Windows;

namespace HR.Web.Filters
{
    public class CheckLoginFilter:ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var accountCookie = HttpContext.Current.Request.Cookies.Get("account");
            if (string.IsNullOrEmpty(accountCookie.Value))
            {
                var controller = filterContext.Controller;
                if (controller.GetType() != typeof(AccountController))
                {
                    var values = new RouteValueDictionary(new { action = "Login", controller = "Account", code = 1 });
                    filterContext.Result = new RedirectToRouteResult(values);
                }
            }
        }
    }
}