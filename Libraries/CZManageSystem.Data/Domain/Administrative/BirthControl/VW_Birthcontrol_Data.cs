using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Domain.Administrative.BirthControl
{
    public class VW_Birthcontrol_Data
    {

        public string Id { get; set; }
        public string InfoStatus { get; set; }
        public string StatusColor { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string DpId { get; set; }
        public string DpName { get; set; }
        public string DpfullName { get; set; }
        public string RealName { get; set; }
        public string EmployeeId { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Status { get; set; }
        public Nullable<DateTime> JoinTime { get; set; }
        public string IsFormal { get; set; }
        public string Sex { get; set; }
        public Nullable<DateTime> Birthday { get; set; }
        public string Nation { get; set; }
        public string IdCardNum { get; set; }
        public string StreetBelong { get; set; }
        public string MaritalStatus { get; set; }
        public string PhoneNum { get; set; }
        public Nullable<DateTime> Lastupdatedate { get; set; }
        public string Havebear { get; set; }
        public Nullable<DateTime> FirstMarryDate { get; set; }
        public Nullable<DateTime> DivorceDate { get; set; }
        public Nullable<DateTime> RemarryDate { get; set; }
        public Nullable<DateTime> WidowedDate { get; set; }
        public Nullable<DateTime> LigationDate { get; set; }
        public string BRemark { get; set; }
        public string SpouseName { get; set; }
        public string Spousesex { get; set; }
        public Nullable<DateTime> SpouseBirthday { get; set; }
        public string SpouseIdCardNum { get; set; }
        public string SpouseAccountbelong { get; set; }
        public string SpousePhone { get; set; }
        public string SpouseMaritalStatus { get; set; }
        public string fixedjob { get; set; }
        public string SpouseWorkingAddress { get; set; }
        public Nullable<DateTime> SpouseLigationDate { get; set; }
        public string organizeGE { get; set; }
        public string sameworkplace { get; set; }
        public string Latemarriage { get; set; }
        public string foremarriagebore { get; set; }
        public string confirmstatus { get; set; }
        public string Creator { get; set; }
        public Nullable<DateTime> CreatedTime { get; set; }
        public string LastModifier { get; set; }
        public Nullable<DateTime> LastModTime { get; set; }

    }

    public class BirthControlInfoBuilder
    {
        //public string[] UserId { get; set; }//状态
        public string InfoStatus { get; set; }//最大人数_下限
        public string Status { get; set; }//最大人数_上限
        public string EmployeeID { get; set; }//地点
        public List<string> DpId { get; set; }//部门ID
        public string RealName { get; set; }//名称
    }   

}
