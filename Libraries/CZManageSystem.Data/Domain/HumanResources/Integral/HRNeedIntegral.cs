using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Domain.HumanResources.Integral
{
    public class HRNeedIntegral
    {
        public Guid Id { get; set; }
        public Nullable<Guid> UserId { get; set; }
        public string UserName { get; set; }
        public string YearDate { get; set; }
        public string NeedIntegral { get; set; }
        public Nullable<int> DoFlag { get; set; }

    }
    public class HRNeedIntegralQueryBuilder
    {
        public string EmployeeID { get; set; }//地点
        public List<string> DpId { get; set; }//部门ID
        public string RealName { get; set; }//名称
        public string Year { get; set; }
    }

}
