using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Data.Models.EmployeeModels
{
    public class ThinEvaluation
    {
        public int Year { get; set; }
        public bool isMidTerm { get; set; }
        public int EvaluatorID { get; set; }
        public int EvalueeID { get; set; }
    }
}
