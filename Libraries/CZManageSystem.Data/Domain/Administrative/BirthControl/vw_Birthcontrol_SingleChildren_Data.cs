using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Domain.Administrative.BirthControl
{
    public class VW_Birthcontrol_SingleChildren_Data
    {
        public string Id { get; set; }
        public Nullable<Guid> UserId { get; set; }
        public string RealName { get; set; }
        public string UserName { get; set; }
        public string DpName { get; set; }
        public string Name { get; set; }
        public Nullable<DateTime> Birthday { get; set; }
        public string sameworkplace { get; set; }
        public string SpouseName { get; set; }



    }

}
