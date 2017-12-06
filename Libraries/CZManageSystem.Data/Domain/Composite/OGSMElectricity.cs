using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Domain.Composite
{
    public class OGSMElectricity
    {
        public int Id { get; set; }
        public string USR_NBR { get; set; }
        public Nullable<int> PAY_MON { get; set; }

        public string ElectricityMeter { get; set; }
        public string Electricity { get; set; }
        public string Remark { get; set; }
        public string Creator { get; set; }
        public Nullable<DateTime> CreatedTime { get; set; }
        public string LastModifier { get; set; }
        public Nullable<DateTime> LastModTime { get; set; }

    }
    public class OGSMElectricityQueryBuilder
    {
        public string USR_NBR { get; set; }
        public int PAY_MON { get; set; }
        public string ElectricityMeter { get; set; }
    }
}
