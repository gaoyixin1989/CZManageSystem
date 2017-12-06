using System;
using System.Collections.Generic;

namespace CZManageSystem.Data.Domain.Composite
{
    public partial class VoteAppQid
    {
        public int ApplyID { get; set; }
        public int QuesionID { get; set; }
        public int ThemeID { get; set; }
        public Nullable<int> SortOrder { get; set; }
    }
}
