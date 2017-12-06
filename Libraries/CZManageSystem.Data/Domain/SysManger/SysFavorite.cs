using System;
using System.Collections.Generic;

namespace CZManageSystem.Data.Domain.SysManger
{
    public partial class SysFavorite 
    {
        public int FavoriteId { get; set; }
        public Guid WorkflowId { get; set; }
        public string WorkflowName { get; set; }
        public Guid  UserId { get; set; }
        public Nullable<int> OrderNo { get; set; }
        public Nullable<bool> EnableFlag { get; set; }
        public string Remark { get; set; }

        public int Type { get; set; }
    }
}
