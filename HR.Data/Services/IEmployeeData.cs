using HR.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Data.Services
{
    public interface IEmployeeData
    {
        IEnumerable<Employee> GetAll();
    }
}
