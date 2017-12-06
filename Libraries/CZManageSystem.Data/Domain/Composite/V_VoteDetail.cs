using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.Composite
{
    public class V_VoteDetail
    {
        public string ApplyTitle { get; set; }
        public string ThemeType { get; set; }
        public string Creator { get; set; }
        public string IsNiming { get; set; }
        public int QuestionID { get; set; }
        public string QuestionTitle { get; set; }
        public Nullable<int> AnswerNum { get; set; }
        public string UserName { get; set; }
        public string RealName { get; set; }
        public Guid UserID { get; set; }
        public Nullable<Guid> CreatorID { get; set; }
        public int ApplyID { get; set; }
        public Nullable<DateTime> CreateTime { get; set; }
        public Nullable<int> ThemeID { get; set; }

    }
}
