using System;
using System.Collections.Generic;

namespace CZManageSystem.Data.Models
{
    public partial class Cziams_Employees
    {
        public int ID { get; set; }
        public string EmployeeID { get; set; }
        public string DepartmentID { get; set; }
        public string EmployeeSign { get; set; }
        public string EmployeeName { get; set; }
        public string CardID { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string WorkTelNo { get; set; }
        public string Address { get; set; }
        public Nullable<int> Gender { get; set; }
        public string OrderNo { get; set; }
        public string EmployeeEx1 { get; set; }
        public string EmployeeEx2 { get; set; }
        public string EmployeeEx3 { get; set; }
        public string EmployeeEx4 { get; set; }
        public string EmployeeEx5 { get; set; }
    }
}
