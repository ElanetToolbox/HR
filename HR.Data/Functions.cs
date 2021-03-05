using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Data
{
    public static class Functions
    {
        public static int GetAge(DateTime birthDate)
        {
            DateTime now = DateTime.Now;
            int age = now.Year - birthDate.Year;

            if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
                age--;

            return age;
        }

        public static string GetSectionType(string cellValue)
        {
            switch (cellValue)
            {
                case "Linkert":
                    return "matrix";
                case "Text":
                    return "text";
                default:
                    return "";
            }
        }
    }
}
