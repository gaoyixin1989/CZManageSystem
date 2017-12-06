using System;
using System.Collections.Generic;

namespace CZManageSystem.Data.Domain.Composite
{
    public partial class VoteQuestionTemp
    {
        public VoteQuestionTemp()
        {
            this.VoteAnserTemps = new List<VoteAnserTemp>(); 
        }

        public int QuestionID { get; set; }
        public string QuestionTitle { get; set; }
        public string QuestionType { get; set; }
        public Nullable<int> AnswerNum { get; set; }
        public Nullable<int> State { get; set; }
        public string Creator { get; set; }
        public Nullable<System.Guid> CreatorID { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public string Remark { get; set; }
        public Nullable<int> IsDel { get; set; }
        public Nullable<int> MaxValue { get; set; }
        public Nullable<int> MinValue { get; set; }
        public Nullable<int> SortOrder { get; set; }
        public virtual ICollection<VoteAnserTemp> VoteAnserTemps { get; set; } 
    }
}
