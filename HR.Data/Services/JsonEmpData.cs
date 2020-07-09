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
            JsonSerializerSettings set = new JsonSerializerSettings();
            set.DateFormatString = @"dd/mm/yyyy";
            string jsonRaw = System.IO.File.ReadAllText(@"C:\Users\chatziparadeisis.i\source\repos\HR\HR.Data\tests\hr_data.json");
            var result = JsonConvert.DeserializeObject<Employees>(jsonRaw,set);
            return null;
        }
    }
}
