using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Domain.Administrative.BirthControl
{
    public class BirthControlStatic
    {
        //public string[] UserId { get; set; }//状态
        public int OfficialStaffCount { get; set; }
        public int OfficialMenStaffCount { get; set; }
        public int OfficialWomenStaffCount { get; set; }
        public int SingleChild { get; set; }
        public int TwoChild { get; set; }
        public int MoreChild { get; set; }
        public int FirstMarry { get; set; }
        public int MarryButNotChild { get; set; }
        public int LigationWomen { get; set; }
        public int LigationMemSpouse { get; set; }
        public int MaryBearAgeMen { get; set; }
        public int MaryBearAgeWomen { get; set; }
        public int SameWork { get; set; }
        public int SameWorkOneChild { get; set; }
        public int SameWorkTwoChild { get; set; }
        public int SameWorkMoreChild { get; set; }
        public int LateMarriage { get; set; }
    }
    public class BirthControlStaticBuilder
    {
        public string DpId { get; set; }
        public string StartTime { get; set; }

        public string EndTime { get; set; }
    }
}
