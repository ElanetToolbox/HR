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
            ApiEvaluationData api = new ApiEvaluationData();
            var temp = api.GetTemplateById(9);
            var pEv = api.GetEmpEvaluations(104).Where(x=>x.EvaluatorID == 76).Single();
            var q1 = temp.Sections.First().questions.Where(x => x.text.Contains("Αξιολογείται πρωτίστως η ικανότητα αντίληψης, κατανόησης και διαχείρισης των συναισθημάτων")).Single();
            pEv.Sections.First().questions.Add(q1);
            var q2 = temp.Sections.First().questions.Where(x => x.text.Contains("Αξιολογούνται συγκεκριμένες ικανότητες η ύπαρξη των οποίων μπορεί να εξασφαλίσει ότι το στέλεχος")).Single();
            pEv.Sections.First().questions.Add(q2);
            api.UpdateEval(pEv);
            //int bossID = 17;
            //ExportEmpEvals(bossID);
            #region
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

        private static void ExportEmpEvals(int bossID)
        {
            ApiEmpData api = new ApiEmpData();
            ApiEvaluationData evApi = new ApiEvaluationData();
            Employee b = api.Get(bossID);
            rContext c = new rContext();
            c.Emps = api.GetAll().ToList();
            c.User = b;
            c.GetSubordinates();
            List<Evaluation> evals = new List<Evaluation>();
            foreach (var e in c.Subordinates)
            {
                var ev = evApi.GetEmpEvaluations(e.ID).Where(x => x.EvaluatorID == bossID);
                evals.AddRange(ev);
            }

            int row = 2;
            WorkBook wb = WorkBook.LoadExcel(@"C:\Users\chatziparadeisis.i\Documents\hr_template.xlsx");
            WorkSheet ws = wb.GetWorkSheet("Sheet1");
            foreach (var eval in evals)
            {
                string Name = c.Emps.Where(x => x.ID == eval.EvalueeID).Single().FullName;
                string Grade = eval.GetScore().ToString();
                List<string> answers = eval.Sections.OrderBy(x => x.Order).SelectMany(x => x.questions).Select(x => x.savedvalue).ToList();
                ws.SetCellValue(row, 0, Name);
                ws.SetCellValue(row, 1, Grade);
                int col = 2;
                foreach (var a in answers)
                {
                    ws.SetCellValue(row, col, a);
                    col++;
                }
                row++;
            }
            wb.SaveAs(@"C:\Users\chatziparadeisis.i\Documents\hr_export\chatzikonstantis.xlsx");
            wb.Close();
        }
    }
}
