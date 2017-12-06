using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Domain.HumanResources.Integral
{
    public class HRCintegralConfig
    {
        public int Id { get; set; }
        public Nullable<double> Mindays { get; set; }
        public Nullable<double> Maxdays { get; set; }
        public Nullable<int> Integral { get; set; }
        public Nullable<double> Times { get; set; }
        public string BuseFormula { get; set; }

    }

}
