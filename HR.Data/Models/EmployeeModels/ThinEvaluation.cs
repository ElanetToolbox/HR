using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Data.Models.EmployeeModels
{
    public class ThinEvaluation
    {
        public int ID { get; set; }
        public int EvaluatorID { get; set; }

        public void Create(JToken eval)
        {
            ID = (int)eval[0];
            var d = eval[1];
        }
    }



}
