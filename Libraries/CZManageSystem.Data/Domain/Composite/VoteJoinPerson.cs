using System;
using System.Collections.Generic;

namespace CZManageSystem.Data.Domain.Composite
{
    public partial class VoteJoinPerson
    {
        public int JoinPersonID { get; set; }
        public int ThemeID { get; set; }
        public Nullable<System.Guid> UserID { get; set; }
        public string UserName { get; set; }
        public string RealName { get; set; }
        public string Remark { get; set; }
        public virtual VoteThemeInfo VoteThemeInfo { get; set; }
    }
}
