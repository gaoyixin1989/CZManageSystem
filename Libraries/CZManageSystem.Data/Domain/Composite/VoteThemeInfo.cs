using System;
using System.Collections.Generic;

namespace CZManageSystem.Data.Domain.Composite
{
    public partial class VoteThemeInfo
    {
        public VoteThemeInfo()
        {
            this.VoteApplies = new List<VoteApply>();
            this.VoteJoinPersons = new List<VoteJoinPerson>();
            this.VoteTidQies = new List<VoteTidQie>();
        }

        public int ThemeID { get; set; }
        public string ThemeName { get; set; }
        public string ThemetypeID { get; set; }
        public string Creator { get; set; }
        public Nullable<System.Guid> CreatorID { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public string Remark { get; set; }
        public Nullable<int> IsDel { get; set; }
        public virtual ICollection<VoteApply> VoteApplies { get; set; }
        public virtual ICollection<VoteJoinPerson> VoteJoinPersons { get; set; }
        public virtual ICollection<VoteTidQie> VoteTidQies { get; set; }
    }
}
