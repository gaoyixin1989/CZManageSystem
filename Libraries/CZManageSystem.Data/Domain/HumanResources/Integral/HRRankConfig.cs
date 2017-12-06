using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Domain.HumanResources.Integral
{
    public class HRRankConfig
    {
        public int Id { get; set; }
        public Nullable<int> SGrade { get; set; }
        public Nullable<int> EGrade { get; set; }
        public Nullable<int> Integral { get; set; }

    }

}
