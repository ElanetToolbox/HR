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
            ApiEvaluationData api = new ApiEvaluationData();
            //var t = api.GetTemplateById(5);
            //var emps = api.GetAll().OrderBy(x=>x.MapRef);
            //string places = string.Join("\n", emps.Select(x => x.MapRef));

            //WorkBook wb = new WorkBook(ExcelFileFormat.XLSX);
            //WorkSheet ws = wb.CreateWorkSheet("1");
            //ws.SetCellValue(0, 0, "Όνομα");
            //ws.SetCellValue(0, 1, "Ιδιότητα");
            //int row = 1;
            //foreach (var emp in emps)
            //{
            //    ws.SetCellValue(row, 0, emp.FullName);
            //    ws.SetCellValue(row, 1, emp.Position);
            //    row++;
            //}
            //wb.SaveAs(@"C:\Users\chatziparadeisis.i\Desktop\5-copied\jobs.xlsx");
            Evaluation newEval = new Evaluation();

            newEval.CreateFromExcel(@"C:\Users\chatziparadeisis.i\Documents\hrapp\systems_test.xlsx");

            newEval.templatedata = new templatedata();
            newEval.templatedata.Type = 2;
            newEval.templatedata.Cycle = "2021";
            newEval.templatedata.Criteria = "0";

            api.UploadEvaluationTemplate(newEval);


        }

    }
}
