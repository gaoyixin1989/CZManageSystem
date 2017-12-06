using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.Administrative.BirthControl
{
    public class BirthControlRoster
    {
        public int id { get; set; }
        public Guid UserId { get; set; }
        public string FemaleName { get; set; }
        public DateTime FemaleBirthday { get; set; }
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
        public string FemeleLigation { get; set; }
        public string PutAnnulus { get; set; }
        public string Others { get; set; }
        public string Remark { get; set; }
        public string Creator { get; set; }
        public Nullable<DateTime> CreatedTime { get; set; }
        public string LastModifier { get; set; }
        public Nullable<DateTime> LastModTime { get; set; }



    }
}
