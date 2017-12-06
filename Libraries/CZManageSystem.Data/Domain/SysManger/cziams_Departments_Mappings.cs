using System;
using System.Collections.Generic;

namespace CZManageSystem.Data.Models
{
    public partial class Cziams_Departments_Mappings
    {
        public int ID { get; set; }
        public string DepartmentID { get; set; }
        public string MappingDepartmentID { get; set; }
        public string OUOrder { get; set; }
    }
}
