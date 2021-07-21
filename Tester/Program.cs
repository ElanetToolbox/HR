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
using HR.Web;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            ApiLoginData api = new ApiLoginData();
            LoginInfo d = api.GetLogin("deligiannis", "konzoom93");
            //var d = api.GetAll().ToList();
            //d.ForEach(x => x.GetEvalStatus(99));
            //var ss = d.Where(x => !x.isEvaluated).Select(x=>x.FullName).ToList();
            //string res = string.Join("\n", ss);
            //c.User = e_api.Get();
            //var h = c.Emps.Where(x=>x.Teams.Where(y=>y.Name != null).Where(y=>y.Name.Contains("Ν11ΕΑ")).Any()).ToList();
            //WorkBook wb = WorkBook.Create(ExcelFileFormat.XLSX);
            //WorkSheet ws = wb.CreateWorkSheet("1");
            //int row = 1;
            //foreach (var item in h)
            //{
            //    ws.SetCellValue(row, 1, item.FullName);
            //    ws.SetCellValue(row, 2, item.DateHired.Value.ToString("dd/MM/yyyy"));
            //    row++;
            //}
            //wb.SaveAs(@"C:\Users\chatziparadeisis.i\Documents\hr_report.xlsx");
            //foreach (var item in c.Subordinates)
            //{
            //    try
            //    {
            //        item.Evaluations = ev_api.GetEmpEvaluations(item.ID);
            //        var eval = item.Evaluations.Where(x => x.EvaluatorID == 60).Single();
            //        var q = eval.Sections[2].questions.Last();
            //        if (q.savedvalue.Contains("Η παρούσα αξιολόγηση πραγματοποιήθηκε από τους: Κίτσου Κατερίνα και Χαϊδεμενάκη Μέγκυ") == false)
            //        {
            //            q.savedvalue = q.savedvalue + "\n" + "Η παρούσα αξιολόγηση πραγματοποιήθηκε από τους: Κίτσου Κατερίνα και Χαϊδεμενάκη Μέγκυ";
            //            ev_api.UpdateEval(eval);
            //        }
            //    }
            //    catch
            //    {

            //    }

            //}


            //ExportEmpEvals(17);
            #region change evaluation
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
            //Evaluation newEval = new Evaluation();

            //newEval.CreateFromExcel(@"C:\Users\chatziparadeisis.i\Documents\hrapp\systems_test.xlsx");

            //newEval.templatedata = new templatedata();
            //newEval.templatedata.Type = 2;
            //newEval.templatedata.Cycle = "2021";
            //newEval.templatedata.Criteria = "0";

            //api.UploadEvaluationTemplate(newEval);
            #endregion
        }

    }
}
