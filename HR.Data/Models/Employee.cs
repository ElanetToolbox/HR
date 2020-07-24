using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Data.Models
{
    [JsonObject(MemberSerialization.OptOut)]
    public class Employee
    {
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        //public string FullName { get; set; }
        public DateTime? DateHired { get; set; }
        public DateTime? DoB { get; set; }
        public string Directorate { get; set; }
        public string Specialty { get; set; }
        public string Position { get; set; }
        public string Photo { get; set; }
        public string Supervisor { get; set; }
        public decimal MixedIncome { get; set; }
        public IEnumerable<string> Documents { get; set; }

        [JsonIgnore]
        public int Age => DoB == null ? 0: Functions.GetAge(DoB.Value);
        [JsonIgnore]
        public string FullName => LastName + " " + FirstName;
    }
}
