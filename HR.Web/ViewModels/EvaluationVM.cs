using HR.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.ViewModels
{
    public class EvaluationVM
    {
        public Account Evaluator { get; set; }
        public Employee Employee { get; set; }
        public Evaluation Evaluation { get; set; }

        public EvaluationVM(Account ev,Employee emp,Evaluation eval)
        {
            Evaluator = ev;
            Employee = emp;
            Evaluation = eval;
        }
    }
}