using System;
using System.Collections.Generic;

namespace CZManageSystem.Data.Models
{
    public partial class Cziams_Posts
    {
        public int ID { get; set; }
        public string PostID { get; set; }
        public string ParentPostID { get; set; }
        public string DepartmentID { get; set; }
        public string RankID { get; set; }
        public string PostSign { get; set; }
        public string PostName { get; set; }
        public string PostDescription { get; set; }
    }
}
