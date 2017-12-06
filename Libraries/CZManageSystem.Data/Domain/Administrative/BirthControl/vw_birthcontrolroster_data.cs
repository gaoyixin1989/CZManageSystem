using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Domain.Administrative.BirthControl
{
    public class VW_BirthcontrolRoster_Data
    {
        public string Id { get; set; }
        public string InfoStatus { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string DpId { get; set; }
        public string DpName { get; set; }
        public string DpfullName { get; set; }
        public string RealName { get; set; }
        public string EmployeeId { get; set; }
        public string FemaleName { get; set; }
        public Nullable<DateTime> FemaleBirthday { get; set; }
        public string FemaleWorkingPlace { get; set; }
        public string MaleName { get; set; }
        public string MaleWorkingPlace { get; set; }
        public string FirstEmbryoSex { get; set; }
        public Nullable<DateTime> FirstEmbryoBirthday { get; set; }
        public string SecondEmbryoSex { get; set; }
        public Nullable<DateTime> SecondEmbryoBirthday { get; set; }
        public string OverThreeChildrenMele { get; set; }
        public string OverThreeChildrenFemele { get; set; }
        public string MeleLigation { get; set; }
        public string femeleLigation { get; set; }
        public string PutAnnulus { get; set; }
        public string Others { get; set; }
        public string Remark { get; set; }
        public string Creator { get; set; }
        public Nullable<DateTime> CreatedTime { get; set; }
        public string LastModifier { get; set; }
        public Nullable<DateTime> LastModTime { get; set; }

    }
    public class BirthControlRosterBuilder
    {
        public string InfoStatus { get; set; }//状态
        public List<string> DpId { get; set; }//部门ID
        public string RealName { get; set; }//名称
    }

}
