using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Domain.HumanResources.Attendance
{
    public class ProvisionsOfAttendance
    {
        public decimal Count { get; set; }
    }
    public class Summarizing
    { 
        public decimal BeLate { get; set; }
        public decimal LeaveEarly { get; set; }
        public decimal Absenteeism { get; set; }
        public decimal Other { get; set; }
        public int Headcount { get; set; }
        public decimal CommunalLeave { get; set; }
        public decimal CasualLeave { get; set; }
        public decimal MaternityLeave { get; set; }
        public decimal MarriageLeave { get; set; }
        public decimal SickLeave { get; set; }
        public decimal FamilyPlanningLeave { get; set; }
        public decimal AnnualLeave { get; set; }
    }
}
