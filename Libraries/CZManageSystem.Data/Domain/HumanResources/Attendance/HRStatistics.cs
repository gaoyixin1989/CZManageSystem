using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Domain.HumanResources.Attendance
{
    public class HRStatistics
    {
        public string DpId { get; set; }
        public string DpName { get; set; }
        public string DpFullName { get; set; }
        public int Ccount { get; set; }// --总人数 
        public int BeOnDuty { get; set; }// --上班
        public int OffDuty { get; set; }// --下班
        public int Abnormal { get; set; } //--异常
        public int HaveHolidaysByTurns { get; set; }// --轮休
        public int HaveAHoliday { get; set; }// --休假
        public int Other { get; set; }// --其他
    }
}
