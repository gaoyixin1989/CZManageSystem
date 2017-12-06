using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;

namespace CZManageSystem.Data.Domain.Composite
{
    public partial class VoteApply
    {
        public int ApplyID { get; set; }
        public Nullable<System.Guid> WorkflowInstanceId { get; set; }
        public string ApplyTitle { get; set; }
        public string ApplySn { get; set; }
        public string Creator { get; set; }
        public Nullable<System.Guid> CreatorID { get; set; }
        public string ThemeType { get; set; }
        public Nullable<int> ThemeID { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public string IsNiming { get; set; }
        public string Attids { get; set; }
        public string Remark { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public string MemberName { get; set; }
        public string MemberIDs { get; set; }
        public Nullable<int> MemberType { get; set; }
        public string MobilePhone { get; set; } 
        public string TempdeptID { get; set; }
        public string TempdeptName { get; set; }
        public string TempuserID { get; set; }
        public string TempuserName { get; set; }
        public Nullable<int> IsProc { get; set; }
        public virtual VoteThemeInfo VoteThemeInfo { get; set; }
        public virtual Tracking_Workflow TrackingWorkflow { get; set; }
         
    }
}
