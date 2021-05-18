using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Data.Models
{
    public interface IQuestion
    {
        int order { get; set; }
        string text { get; set; }
        string savedvalue { get; set; }
    }
}
