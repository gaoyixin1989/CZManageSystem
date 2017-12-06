using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Domain.Composite
{
    public partial class OGSMMonth
    {
        public int Id { get; set; }
        public Nullable<int> PAY_MON { get; set; }
        public string USR_NBR { get; set; }
        public string IsPayment { get; set; }
        public Nullable<System.DateTime> PaymentTime { get; set; }
        public Nullable<System.DateTime> AccountTime { get; set; }
        public Nullable<decimal> AccountMoney { get; set; }
        public string AccountNo { get; set; }
        public Nullable<decimal> CMPower2G { get; set; }
        public Nullable<decimal> CMPower3G { get; set; }
        public Nullable<decimal> CMPower4G { get; set; }
        public Nullable<decimal> CUPower2G { get; set; }
        public Nullable<decimal> CUPower3G { get; set; }
        public Nullable<decimal> CUPower4G { get; set; }
        public Nullable<decimal> CTPower2G { get; set; }
        public Nullable<decimal> CTPower3G { get; set; }
        public Nullable<decimal> CTPower4G { get; set; }
        public string Remark { get; set; }
        public string Creator { get; set; }
        public Nullable<System.DateTime> CreatedTime { get; set; }
        public string LastModifier { get; set; }
        public Nullable<System.DateTime> LastModTime { get; set; }


        //public virtual OGSM OGSM { get; set; }
    }
    public class OGSMMonthQueryBuilder
    {
        public string USR_NBR { get; set; }
        public string IsPayment { get; set; }
        public Nullable<System.DateTime> PaymentTime_Start { get; set; }
        public Nullable<System.DateTime> PaymentTime_End { get; set; }
        public Nullable<System.DateTime> AccountTime_Start { get; set; }
        public Nullable<System.DateTime> AccountTime_End { get; set; }
        public string PAY_MON_Start { get; set; }
        public string PAY_MON_End { get; set; }
    }
    public partial class OGSMMonth_Export
    {
        public Nullable<int> PAY_MON { get; set; }
        public string Group_Name { get; set; }
        public string Town { get; set; }
        public string USR_NBR { get; set; }
        public string PowerStation { get; set; }
        public string BaseStation { get; set; }
        public string PowerType { get; set; }
        public string PropertyRight { get; set; }
        public string IsRemove { get; set; }
        public Nullable<System.DateTime> RemoveTime { get; set; }
        public string IsShare { get; set; }
        public Nullable<System.DateTime> ContractStartTime { get; set; }
        public Nullable<System.DateTime> ContractEndTime { get; set; }
        public string IsPayment { get; set; }
        public Nullable<System.DateTime> PaymentTime { get; set; }
        public Nullable<System.DateTime> AccountTime { get; set; }
        public Nullable<decimal> AccountMoney { get; set; }
        public string AccountNo { get; set; }
        public Nullable<decimal> CMPower2G { get; set; }
        public Nullable<decimal> CMPower3G { get; set; }
        public Nullable<decimal> CMPower4G { get; set; }
        public Nullable<decimal> CUPower2G { get; set; }
        public Nullable<decimal> CUPower3G { get; set; }
        public Nullable<decimal> CUPower4G { get; set; }
        public Nullable<decimal> CTPower2G { get; set; }
        public Nullable<decimal> CTPower3G { get; set; }
        public Nullable<decimal> CTPower4G { get; set; }
        public string Price { get; set; }
        public string Address { get; set; }
        public Nullable<int> PAY_CYC { get; set; }
        public string Property { get; set; }
        public string LinkMan { get; set; }
        public string Mobile { get; set; }
        public string IsWarn { get; set; }
        public Nullable<int> WarnCount { get; set; }
        public string Remark { get; set; }
    }

}
