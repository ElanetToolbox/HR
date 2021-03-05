using HR.Data.Models;
using HR.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using Newtonsoft.Json.Converters;
using RestSharp;
using IronXL;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            ApiEmpData api = new ApiEmpData();
            Employee e = api.Get(12);
            string s = e.TeamsString();
            //Evaluation newEval = new Evaluation();

            //newEval.CreateFromExcel(@"C:\Users\chatziparadeisis.i\Documents\temp_test.xlsx");

            //newEval.templatedata = new templatedata();
            //newEval.templatedata.Type = 1;
            //newEval.templatedata.Cycle = "2020";
            //newEval.templatedata.Criteria = "1,2,3";

            //api.UploadEvaluationTemplate(newEval);


        }

    }
}
