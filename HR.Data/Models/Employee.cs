using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Data.Models
{
    public class Employee
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public DateTime DateHired { get; set; }
        public DateTime DoB { get; set; }
        public string Directorate { get; set; }
        public string Specialty { get; set; }
        public string Position { get; set; }
    }
}
