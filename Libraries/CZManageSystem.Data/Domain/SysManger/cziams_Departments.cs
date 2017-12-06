using System;
using System.Collections.Generic;

namespace CZManageSystem.Data.Models
{
    public partial class Cziams_Departments
    {
        public int ID { get; set; }
        public string DepartmentID { get; set; }
        public string ParentDepartmentID { get; set; }
        public string DepartmentSign { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentFullName { get; set; }
        public string DepartmentDescription { get; set; }
        public Nullable<int> DepartmentLevel { get; set; }
        public string DepartmentEx1 { get; set; }
        public string DepartmentEx2 { get; set; }
        public string DepartmentEx3 { get; set; }
        public string DepartmentEx4 { get; set; }
        public string DepartmentEx5 { get; set; }
    }
}
