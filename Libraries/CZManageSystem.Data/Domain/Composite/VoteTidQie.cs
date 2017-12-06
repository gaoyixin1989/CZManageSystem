using System;
using System.Collections.Generic;

namespace CZManageSystem.Data.Domain.Composite
{
    public partial class VoteTidQie
    {
        public int ThemeID { get; set; }
        public int QuestionID { get; set; }
        public Nullable<int> SortOrder { get; set; }
        public virtual VoteQuestion VoteQuestion { get; set; }
        public virtual VoteThemeInfo VoteThemeInfo { get; set; }
    }
}
