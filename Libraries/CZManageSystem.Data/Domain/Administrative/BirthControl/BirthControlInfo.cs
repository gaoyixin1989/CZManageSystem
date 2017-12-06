using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.Administrative.BirthControl
{
    public class BirthControlInfo
    {
        public int id { get; set; }
        public Nullable<Guid> UserId { get; set; }
        public string Sex { get; set; }
        public Nullable<DateTime> Birthday { get; set; }
        public string Nation { get; set; }
        public string IdCardNum { get; set; }
        public string StreetBelong { get; set; }
        public string MaritalStatus { get; set; }
        public string PhoneNum { get; set; }
        public Nullable<DateTime> Lastupdatedate { get; set; }
        public string Havebear { get; set; }
        public string Latemarriage { get; set; }
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
        public string FixedJob { get; set; }
        public string SpouseWorkingAddress { get; set; }
        public Nullable<DateTime> SpouseLigationDate { get; set; }
        public string OrganizeGE { get; set; }
        public string SameWorkPlace { get; set; }
        public string ForeMarriageBore { get; set; }
        public string ConfirmStatus { get; set; }
        public string Creator { get; set; }
        public Nullable<DateTime> CreatedTime { get; set; }
        public string LastModifier { get; set; }
        public Nullable<DateTime> LastModTime { get; set; }

    }
}
