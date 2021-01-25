using HR.Data.Models;
using HR.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HR.Web.Controllers
{
    public class EvaluationController : Controller
    {
        ApiEvaluationData db;
        public EvaluationController()
        {
            db = new ApiEvaluationData();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = db.GetClearEval();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(Evaluation eval)
        {
            var z = Request.Form;
            var model = db.GetClearEval();
            model.FillFromForm(z);
            var d = model.isComplete();
            if (model.isComplete())
            {
                return RedirectToAction("Index", "Employees");
            }
            else
            {
                return View(model);
            }
            //return View(model);
        }
    }
}