using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HR.Web.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult Index()
        {
            rContext currentContext = Session[nameof(currentContext)] as rContext;
            return View();
        }

        [HttpGet]
        public FileContentResult ExportUser()
        {
            rContext currentContext = Session[nameof(currentContext)] as rContext;
            byte[] report;
            report = Functions.ExportEmpEvals(currentContext, currentContext.User.ID);
            return File(report, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Report.xlsx");
        }

        [HttpGet]
        public FileContentResult ExportPersonel()
        {
            rContext currentContext = Session[nameof(currentContext)] as rContext;
            byte[] report;
            report = Functions.ExportEmpEvals(currentContext);
            return File(report, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Report.xlsx");
        }
    }
}