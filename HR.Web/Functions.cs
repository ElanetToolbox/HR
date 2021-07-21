using HR.Data.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using HR.Data.Services;
using IronXL;
using GemBox.Spreadsheet;

namespace HR.Web
{
    public static class Functions
    {
        public static NumberFormatInfo decimalFormat => GetDecimalFormat();
        public static string GetFileNameFromPath(string fName)
        {
            return Path.GetFileName(fName);
        }

        public static List<KeyValuePair<string,string>> GetFloors()
        {
            var l = new List<KeyValuePair<string,string>>();
            l.Add(new KeyValuePair<string, string>("p7","Πανεπιστημίου 7ος"));
            l.Add(new KeyValuePair<string, string>("p3","Πανεπιστημίου 3ος"));
            l.Add(new KeyValuePair<string, string>("e2","Ευριππίδου"));
            return l;
        }

        public static List<KeyValuePair<string,string>> GetRooms()
        {
            var l = new List<KeyValuePair<string,string>>();
            l.Add(new KeyValuePair<string,string>("",""));
            l.Add(new KeyValuePair<string,string>("a1","A.1"));
            l.Add(new KeyValuePair<string,string>("a2","A.2"));
            l.Add(new KeyValuePair<string,string>("a3","A.3"));
            l.Add(new KeyValuePair<string,string>("a4","A.4"));
            l.Add(new KeyValuePair<string,string>("a5","A.5"));
            l.Add(new KeyValuePair<string,string>("a6","A.6"));
            l.Add(new KeyValuePair<string,string>("a7","A.7"));
            l.Add(new KeyValuePair<string,string>("a8","A.8"));
            l.Add(new KeyValuePair<string,string>("a9","A.9"));
            l.Add(new KeyValuePair<string,string>("a10","A.10"));
            l.Add(new KeyValuePair<string,string>("a11","A.11"));
            l.Add(new KeyValuePair<string,string>("a12","A.12"));
            l.Add(new KeyValuePair<string,string>("a13","A.13"));
            l.Add(new KeyValuePair<string,string>("a14","A.14"));
            l.Add(new KeyValuePair<string,string>("a15","A.15"));
            l.Add(new KeyValuePair<string,string>("b1","Β.1"));
            l.Add(new KeyValuePair<string,string>("b2","Β.2"));
            l.Add(new KeyValuePair<string,string>("b3","Β.3"));
            l.Add(new KeyValuePair<string,string>("b4","Β.4"));
            l.Add(new KeyValuePair<string,string>("b5","Β.5"));
            l.Add(new KeyValuePair<string,string>("b6","Β.6"));
            l.Add(new KeyValuePair<string,string>("b7","Β.7"));
            l.Add(new KeyValuePair<string,string>("b8","Β.8"));
            l.Add(new KeyValuePair<string,string>("b9","Β.9"));
            l.Add(new KeyValuePair<string,string>("b10","Β.10"));
            l.Add(new KeyValuePair<string,string>("b11","Β.11"));
            l.Add(new KeyValuePair<string,string>("b12","Β.12"));
            l.Add(new KeyValuePair<string,string>("Rec","Γραμματεία"));
            return l;
        }
        
        public static List<int> BaseEvalIDs()
        {
            List<int> l = new List<int>();
            l.Add(8);
            l.Add(9);
            l.Add(10);
            return l;
        }

        public static DateTime DateFromInput(string input)
        {
            input = input.Substring(input.IndexOf(',') + 1);
            //return DateTime.ParseExact(input, "yyyy-MM-dd", CultureInfo.CurrentCulture);;
            DateTime date;
            if (input.Contains("-"))
            {
                date = DateTime.ParseExact(input, "dd-MM-yyyy", CultureInfo.CurrentCulture);
            }
            else
            {
                date = DateTime.ParseExact(input, "dd/MM/yyyy", CultureInfo.CurrentCulture);
            }
            return date;
        }
        
        public static int GetTemplateID(Employee emp,Employee eval)
        {
            if (emp.Teams.Where(x => x.Name == "SYST3" && x.Position < 70).Any())
            {
                return 10;
            }
            if(emp.Teams.Where(x=>x.Position >= 70 || x.Position == 60.1 || x.Position==60.2).Any() || emp.ID == 99)
            {
                return 9;
            }
            return 8;
        }

        public static List<float> FixPositionArray(Array pos)
        {
            List<float> result = new List<float>();
            return result;
        }

        private static NumberFormatInfo GetDecimalFormat()
        {
            NumberFormatInfo result = new NumberFormatInfo();
            result.NumberDecimalSeparator = ".";
            return result;
        }
        
        public static byte[] ExportEmpEvals(rContext context,int bossID = 0)
        {
            IronXL.License.LicenseKey = "IRONXL.HATZOS42.29628-21B36C3B1D-S24Y2GNSF43GQL4D-A2ZI7IGHCW72-XUNNSB4HYNLW-EISSKWRMLVUJ-SRGSDVXK55LO-RM7T7V-TP755XBEMZSAUA-DEPLOYMENT.TRIAL-IJWULE.TRIAL.EXPIRES.23.JUL.2021";
            rContext c = context;
            List<Evaluation> evals = new List<Evaluation>();
            IEnumerable<Employee> emps;
            if(bossID == 0)
            {
                emps = c.Underlings;
            }
            else
            {
                emps = c.Subordinates;
            }
            foreach (var e in emps)
            {
                IEnumerable<Evaluation> ev;
                c.GetEmpEvaluations(e.ID);
                if (bossID != 0)
                {
                    ev = e.Evaluations.Where(x=>x.EvaluatorID == bossID);
                }
                else
                {
                    ev = e.Evaluations;
                }
                evals.AddRange(ev);
            }

            int row = 2;
            int initialCol = 2;
            int jumpCol = 6;
            string homeDir = AppContext.BaseDirectory;
            string fullPath;
            WorkBook wb;
            if (bossID != 0)
            {
                try
                {
                    fullPath = HttpContext.Current.Server.MapPath(@"~\Content\ReportTemplates\hr_template.xlsx");
                    wb = WorkBook.LoadExcel(fullPath);
                }
                catch
                {
                    fullPath = @"C:\ReportTemplates\hr_template.xlsx";
                    wb = WorkBook.LoadExcel(fullPath);
                }
            }
            else
            {
                fullPath = HttpContext.Current.Server.MapPath(@"~\Content\ReportTemplates\hr_total_template.xlsx");
                wb = WorkBook.LoadExcel(fullPath);
                initialCol+=2;
                jumpCol+=2;
            }
            WorkSheet ws = wb.GetWorkSheet("Sheet1");
            foreach (var eval in evals)
            {
                bool isManager = eval.Sections.First().questions.Count() > 5;
                string Name = c.Emps.Where(x => x.ID == eval.EvalueeID).Single().FullName;
                string Grade = eval.GetScore().ToString().Replace(".",",");
                List<string> answers = eval.Sections.OrderBy(x => x.Order).SelectMany(x => x.questions).Select(x => x.savedvalue).ToList();
                ws.SetCellValue(row, 0, Name);
                ws.SetCellValue(row, 1, Grade);
                if(bossID == 0)
                {
                    string evalName = c.Emps.Where(x => x.ID == eval.EvaluatorID).Single().FullName;
                    ws.SetCellValue(row, 3, Grade);
                    ws.SetCellValue(row, 2, evalName);
                    ws.SetCellValue(row, 1, c.Emps.Where(x => x.ID == eval.EvalueeID).Single().DateHired);
                }
                int col = initialCol;
                foreach (var a in answers)
                {
                    ws.SetCellValue(row, col, a);
                    if (col == jumpCol && !isManager)
                    {
                        col += 2;
                    }
                    col++;
                }
                row++;
            }
            var result = wb.ToBinary();
            wb.Close();
            return result;
        }

        public static byte[] ExportEmpEvals1(rContext context, int bossID = 0)
        {
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            rContext c = context;
            List<Evaluation> evals = new List<Evaluation>();
            IEnumerable<Employee> emps;
            if(bossID == 0)
            {
                emps = c.Underlings;
            }
            else
            {
                emps = c.Subordinates;
            }
            foreach (var e in emps)
            {
                IEnumerable<Evaluation> ev;
                c.GetEmpEvaluations(e.ID);
                if (bossID != 0)
                {
                    ev = e.Evaluations.Where(x=>x.EvaluatorID == bossID);
                }
                else
                {
                    ev = e.Evaluations;
                }
                evals.AddRange(ev);
            }

            int row = 2;
            int initialCol = 2;
            int jumpCol = 6;
            string homeDir = AppContext.BaseDirectory;
            string fullPath;
            ExcelFile wb;
            if (bossID != 0)
            {
                try
                {
                    fullPath = HttpContext.Current.Server.MapPath(@"~\Content\ReportTemplates\hr_template.xlsx");
                }
                catch
                {
                    fullPath = @"C:\ReportTemplates\hr_template.xlsx";
                }
            }
            else
            {
                fullPath = HttpContext.Current.Server.MapPath(@"~\Content\ReportTemplates\hr_total_template.xlsx");
                initialCol+=2;
                jumpCol+=2;
            }
            wb = ExcelFile.Load(fullPath);
            ExcelWorksheet ws = wb.Worksheets.First();
            foreach (var eval in evals.Take(100))
            {
                bool isManager = eval.Sections.First().questions.Count() > 5;
                string Name = c.Emps.Where(x => x.ID == eval.EvalueeID).Single().FullName;
                string Grade = eval.GetScore().ToString().Replace(".",",");
                List<string> answers = eval.Sections.OrderBy(x => x.Order).SelectMany(x => x.questions).Select(x => x.savedvalue).ToList();
                ws.Cells[row, 0].Value = Name;
                ws.Cells[row, 1].Value = Grade;
                if(bossID == 0)
                {
                    string evalName = c.Emps.Where(x => x.ID == eval.EvaluatorID).Single().FullName;
                    ws.Cells[row, 3].Value = Grade;
                    ws.Cells[row, 2].Value = evalName;
                    ws.Cells[row, 1].Value = c.Emps.Where(x => x.ID == eval.EvalueeID).Single().DateHired;
                }
                int col = initialCol;
                foreach (var a in answers)
                {
                    ws.Cells[row, col].Value = a;
                    if (col == jumpCol && !isManager)
                    {
                        col += 2;
                    }
                    col++;
                }
                row++;
            }
            wb.Save(@"C:\Users\chatziparadeisis.i\Documents\eme covid\a\gem.xlsx");
            return null;
        }

        public static string NumToLetter(int num)
        {
            switch (num)
            {
                case 1:
                    return "Α";
                case 2:
                    return "Β";
                case 3:
                    return "Γ";
                case 4:
                    return "Δ";
                default:
                    return null;
            }
        }


    }
}