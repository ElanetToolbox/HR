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
            rContext c = new rContext();
            ApiEmpData e_api = new ApiEmpData();
            ApiEvaluationData ev_api = new ApiEvaluationData();
            c.Emps = e_api.GetAll().ToList();
            c.Emps.ForEach(x => x.GetEvalStatus(99));
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

        private static void ExportEmpEvals(int bossID = 0)
        {
            ApiEmpData api = new ApiEmpData();
            ApiEvaluationData evApi = new ApiEvaluationData();
            rContext c = new rContext();
            c.Emps = api.GetAll().ToList();
            if (bossID != 0)
            {
                Employee b = api.Get(bossID);
                c.User = b;
                c.GetSubordinates();
            }
            else
            {
                c.Subordinates = c.Emps;
            }
            List<Evaluation> evals = new List<Evaluation>();
            foreach (var e in c.Subordinates)
            {
                IEnumerable<Evaluation> ev;
                if (bossID != 0)
                {
                    ev = evApi.GetEmpEvaluations(e.ID).Where(x => x.EvaluatorID == bossID);
                }
                else
                {
                    ev = evApi.GetEmpEvaluations(e.ID);
                }
                evals.AddRange(ev);
            }

            int row = 2;
            WorkBook wb = WorkBook.LoadExcel(@"C:\Users\chatziparadeisis.i\Documents\hr_template.xlsx");
            WorkSheet ws = wb.GetWorkSheet("Sheet1");
            foreach (var eval in evals)
            {
                bool isManager = eval.Sections.First().questions.Count() > 5;
                string Name = c.Emps.Where(x => x.ID == eval.EvalueeID).Single().FullName;
                string Grade = eval.GetScore().ToString().Replace(".",",");
                List<string> answers = eval.Sections.OrderBy(x => x.Order).SelectMany(x => x.questions).Select(x => x.savedvalue).ToList();
                ws.SetCellValue(row, 0, Name);
                ws.SetCellValue(row, 1, Grade);
                int col = 2;
                foreach (var a in answers)
                {
                    ws.SetCellValue(row, col, a);
                    if (col == 6 && !isManager)
                    {
                        col += 2;
                    }
                    col++;
                }
                row++;
            }
            if (bossID == 0)
            {
                wb.SaveAs(@"C:\Users\chatziparadeisis.i\Documents\hr_export\total.xlsx");
            }
            else
            {
                wb.SaveAs(@"C:\Users\chatziparadeisis.i\Documents\hr_export\"+c.User.LastName+".xlsx");
            }
            wb.Close();
        }
    }
}
