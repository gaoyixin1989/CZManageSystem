using System;
using System.Collections.Generic;

namespace CZManageSystem.Data.Models
{
    public partial class Cziams_Ranks
    {
        public int ID { get; set; }
        public string RankID { get; set; }
        public string ParentRankID { get; set; }
        public string RankName { get; set; }
        public string RankDescription { get; set; }
    }
}
