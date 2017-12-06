using System;
using System.Collections.Generic;

namespace CZManageSystem.Data.Domain.Composite
{
    public partial class VoteAnser
    {
        public VoteAnser()
        {
            this.VoteSelectedAnsers = new List<VoteSelectedAnser>();
        }

        public int AnserID { get; set; }
        public Nullable<int> QuestionID { get; set; }
        public string AnserContent { get; set; }
        public Nullable<decimal> AnserScore { get; set; }
        public Nullable<int> SortOrder { get; set; }
        public Nullable<int> MaxValue { get; set; }
        public Nullable<int> MinValue { get; set; }
        public virtual VoteQuestion VoteQuestion { get; set; }
        public virtual ICollection<VoteSelectedAnser> VoteSelectedAnsers { get; set; }
    }
}
