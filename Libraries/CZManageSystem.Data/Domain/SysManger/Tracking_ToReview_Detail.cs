using CZManageSystem.Core;
using System;
using System.Collections.Generic;

namespace CZManageSystem.Data.Domain.SysManger
{
    public partial class Tracking_ToReview_Detail
    {
       
        public Guid ActivityInstanceId { get; set; }
        public string UserName { get; set; }
        public int State { get; set; }
        public Nullable<DateTime> ReviewTime { get; set; }
        public string RealName { get; set; }
        public Nullable<Guid> WorkflowInstanceId { get; set; }
        public Nullable<Guid> ActivityId { get; set; }
        public string ActivityName { get; set; }
        public Nullable<int> SortOrder { get; set; }
        public string SheetId { get; set; }
        public string Title { get; set; }
        public string Creator { get; set; }
        public string CreatorName { get; set; }
        public Nullable<DateTime> StartedTime { get; set; }
        public string WorkflowAlias { get; set; }
        public string AliasImage { get; set; }
        public string ToReviewActors { get; set; }

        public DateTime CreatedTime { get; set; }
        

    }
}
