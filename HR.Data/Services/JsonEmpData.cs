using HR.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HR.Data.Services
{
    public class JsonEmpData : IEmployeeData
    {
        public IEnumerable<Employee> GetAll()
        {
            string jsonRaw = System.IO.File.ReadAllText(@"C:\Users\chatziparadeisis.i\source\repos\HR\HR.Data\tests\hr_data.json");
            var result = JsonConvert.DeserializeObject<Employee>(jsonRaw);
            return null;
        }
    }
}
