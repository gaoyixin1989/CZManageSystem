using System;
using System.Collections.Generic;

namespace CZManageSystem.Data.Domain.Composite
{
    public partial class VoteAnserTemp
    { 
        public int AnserID { get; set; }
        public Nullable<int> QuestionID { get; set; }
        public string AnserContent { get; set; }
        public Nullable<decimal> AnserScore { get; set; }
        public Nullable<int> SortOrder { get; set; }
        public Nullable<int> MaxValue { get; set; }
        public Nullable<int> MinValue { get; set; }
        public virtual VoteQuestionTemp VoteQuestionTemp { get; set; } 
    }
}
