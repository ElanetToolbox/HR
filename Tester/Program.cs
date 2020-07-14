using HR.Data.Models;
using HR.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            IEmployeeData empDataService = new JsonEmpData();
            var derp = empDataService.GetAll();
            foreach (var d in derp)
            {
                d.Documents = new List<string> { @"C:\Users\chatziparadeisis.i\source\repos\HR\HR.Data\tests\testPdf.pdf" };
            }
            empDataService.Save(new Employees { Emps = derp.ToList() }); 
        }
    }
}
