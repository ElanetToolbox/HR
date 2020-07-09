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
        }
    }
}
