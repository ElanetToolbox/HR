using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Data
{
    public static class Functions
    {
        public static NumberFormatInfo decimalFormat => GetDecimalFormat();

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

        private static NumberFormatInfo GetDecimalFormat()
        {
            NumberFormatInfo result = new NumberFormatInfo();
            result.NumberDecimalSeparator = ".";
            return result;
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
