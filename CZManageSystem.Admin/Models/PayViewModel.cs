using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Admin.Models
{
    public class PayViewModel
    {
        public string EmployerId { set; get; }
        public DateTime Billcyc { set; get; }
        public DateTime UpdateTime { set; get; }//更新时间  
        public decimal FixedIncome { set; get; }//固定收入 
        public decimal SeniorityPay { set; get; }//工龄工资 
        public decimal ReviewMonthlyAward { set; get; }//月度考核奖 
        public decimal PhoneSubsidies { set; get; }//话费补助 
        public decimal TravelAllowance { set; get; }//交通补贴 
        public decimal NightShiftAllowance { set; get; }//值夜夜班津贴 
        public decimal HolidayOvertimePay { set; get; }//节假日加班工资 
        public decimal Other { set; get; }//其它 
        public decimal MotorCombinedPrize { set; get; }//机动奖合计 
        public decimal TotalIncome { set; get; }//总收入 
        public decimal SocialSecurityDeductions { set; get; }//社保扣款 
        public decimal MedicalInsuranceDeductions { set; get; }//医保扣款 
        public decimal HousingFundDeductions { set; get; }//住房公积金 
        public decimal DormitoryUtilitiesRent { set; get; }//宿舍房租及水电费 
        public decimal OtherDeductions { set; get; }//其它扣款 
        public decimal SocialSecurityEnterprise { set; get; }//社保企 
        public decimal MedicalInsuranceEnterprise { set; get; }//医保企 
        public decimal HousingFundEnterprise { set; get; }//住房公积金企 
        public decimal TaxableIncome { set; get; }//应纳税所得额 
        public decimal IncomeTax { set; get; }//个人所得税 
        public decimal SalaryPaid { set; get; }//实发 
        public string Remark { set; get; }//备注

    }
}
