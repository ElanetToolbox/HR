using HR.Data.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HR.Data.Services
{
    public class JsonEmpData : IEmployeeData
    {
        string jsonPath = @"C:\Users\chatziparadeisis.i\source\repos\HR\HR.Data\tests\hr_data.json";
        JsonSerializerSettings set = new JsonSerializerSettings {DateFormatString = "d/M/yyyy" };
        List<Employee> Employees;

        public JsonEmpData()
        {
            string jsonRaw = File.ReadAllText(jsonPath);
            var result = JsonConvert.DeserializeObject<Employees>(jsonRaw,set);
            Employees = result.Emps;
        }

        public Employee Get(int ID)
        {
            return Employees.FirstOrDefault(x => x.ID == ID);
        }

        public IEnumerable<Employee> GetAll()
        {
            return Employees.OrderBy(x=>x.FullName);
        }

        public void Save(Employees emps)
        {
            File.WriteAllText(jsonPath, JsonConvert.SerializeObject(emps, set));
        }
    }
}
