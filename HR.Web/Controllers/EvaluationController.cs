﻿using HR.Data.Models;
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
        public ActionResult Index(int id,int evalID = 0 ,bool view = false)
        {
            rContext currentContext = Session[nameof(currentContext)] as rContext;
            var emp = currentContext.Emps.Where(x => x.ID == id).FirstOrDefault();
            Evaluation model;
            if (view)
            {
                ViewBag.Disabled = "disabled";
            }
            if(evalID == 0)
            {
                int templateID = Functions.GetTemplateID(emp, currentContext.User);
                model = currentContext.EvaluationTemplates.Where(x => x.TemplateID == templateID).Single();
                model.EvaluatorID = currentContext.User.ID;
                model.EvalueeID = id;
            }
            else
            {
                model = emp.Evaluations.Where(x => x.EvalID == evalID).Single();
                //model = db.GetByID(evalID);
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult IndexError(Evaluation eval)
        {
            ViewBag.Error = true;
            return View(eval);
        }

        [HttpPost]
        public ActionResult Index(Evaluation eval)
        {
            rContext currentContext = Session[nameof(currentContext)] as rContext;
            var z = Request.Form;
            var model = eval;
            var emp = currentContext.Emps.Where(x => x.ID == eval.EvalueeID).First();
            int tID = Functions.GetTemplateID(emp,currentContext.User);
            //var clearEval = db.GetTemplateById(tID);
            var clearEval = currentContext.EvaluationTemplates.Where(x=>x.TemplateID == tID).Single();
            model.ScoreTable = clearEval.ScoreTable;
            model.Sections = clearEval.Sections;
            model.EvalueeID = eval.EvalueeID;
            model.EvaluatorID = eval.EvaluatorID;
            model.FillFromForm(z);
            model.Date = DateTime.Now;
            if (model.isComplete())
            {
                //if (Functions.BaseEvalIDs().Contains(eval.EvalID))
                if (eval.EvalID == 0)
                {
                    db.UploadEval(model);
                }
                else
                {
                    db.UpdateEval(model);
                }
                return RedirectToAction("Details", "Employees", new { id = model.EvalueeID });
            }
            else
            {
                return IndexError(model);
            }
        }
    }
}