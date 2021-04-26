using HR.Data.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

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
            return DateTime.ParseExact(input, "yyyy-MM-dd", CultureInfo.CurrentCulture);;
        }
        
        public static int GetTemplateID(Employee emp,Employee eval)
        {
            if(emp.Teams.Where(x=>x.Name == "SYST3").Any())
            {
                return 10;
            }
            if(emp.Teams.Where(x=>x.Position >= 70 || x.Position == 60.1 || x.Position==60.2).Any())
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

    }
}