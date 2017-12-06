using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Admin.Models
{
    public class GdPayViewModel
    {
        public string  EmployerId { get; set; }
        public DateTime  Billcyc { get; set; }
        public int PayId { get; set; }  
        public Nullable<DateTime> updatetime { get; set; }
        public string Value { get; set; }
    }
}
