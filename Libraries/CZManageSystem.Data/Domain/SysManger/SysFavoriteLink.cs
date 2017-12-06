using System;
using System.Collections.Generic;

namespace CZManageSystem.Data.Domain.SysManger
{
    public partial class SysFavoriteLink
    {
        public int FavoriteLinkId { get; set; }
        public string FavoriteLinkName { get; set; }
        public System.Guid UserId { get; set; }
        public string FavoriteLinkUrl { get; set; }
        public Nullable<int> OrderNo { get; set; }
        public Nullable<bool> EnableFlag { get; set; }
        public string Remark { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
    }
}
