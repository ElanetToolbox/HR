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
        // GET: Evaluation
        public ActionResult Index()
        {
            var model = db.GetClearEval();
            //model.Employee = emp;
            return View(model);
        }
    }
}