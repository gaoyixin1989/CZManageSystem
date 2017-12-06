using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;

namespace CZManageSystem.Data.Domain.Composite
{
    public partial class VoteSelectedAnser
    {
        public int SelectedAnserID { get; set; }
        public Nullable<System.Guid> UserID { get; set; }
        public string Respondent { get; set; }
        public Nullable<int> ThemeID { get; set; }
        public Nullable<int> QuestionID { get; set; }
        public Nullable<int> AnserID { get; set; }
        public string OtherContent { get; set; }
        public virtual VoteAnser VoteAnser { get; set; }
        public virtual VoteQuestion VoteQuestion { get; set; }
    }
}
