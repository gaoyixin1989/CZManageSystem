using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Domain.Administrative.BirthControl
{
    public class VW_Birthcontrol_SpouseGE_Data
    {
        public string Id { get; set; }
        public string EmployeeId { get; set; }
        public string UserName { get; set; }
        public string RealName { get; set; }
        public string DpName { get; set; }
        public string SpouseName { get; set; }
        public string ifGE { get; set; }
        public Nullable<int> BornSituation { get; set; }


    }
}
