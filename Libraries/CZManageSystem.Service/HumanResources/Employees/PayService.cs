using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.HumanResources.Employees;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CZManageSystem.Service.HumanResources.Employees
{
    public class PayService : BaseService<Pay>, IPayService
    {
        static SystemContext dbContext = new SystemContext("SqlConnectionHR");
        public PayService() : base(dbContext)
        { }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="count">返回记录总数</param>
        /// <param name="objs">条件数组</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public override IEnumerable<dynamic> GetForPaging(out int count, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            try
            {
                var source = objs == null ? this._entityStore.Table.OrderByDescending(c => c.billcyc) : this._entityStore.Table.OrderByDescending(c => c.billcyc).Where(ExpressionFactory(objs));
                PagedList<Pay> pageList = new PagedList<Pay>(source, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count);
                count = pageList.TotalCount;
                return pageList.Select(s => new
                {
                    EmployerId = s.employerid,
                    AccountingCycle = GetDate( s.billcyc.ToString()), 
                    UpdateTime = s.更新时间, 
                    FixedIncome = s.固定收入,
                    SeniorityPay = s.工龄工资,
                    ReviewMonthlyAward = s.月度考核奖,
                    PhoneSubsidies = s.话费补助,
                    TravelAllowance = s.交通补贴,
                    NightShiftAllowance = s.值夜夜班津贴,
                    HolidayOvertimePay = s.节假日加班工资,
                    Other = s.其它,
                    MotorCombinedPrize = s.机动奖合计,
                    TotalIncome = s.总收入,
                    SocialSecurityDeductions = s.社保扣款,
                    MedicalInsuranceDeductions = s.医保扣款,
                    HousingFundDeductions = s.住房公积金,
                    DormitoryUtilitiesRent = s.宿舍房租及水电费,
                    OtherDeductions = s.其它扣款,
                    SocialSecurityEnterprise = s.社保企,
                    MedicalInsuranceEnterprise = s.医保企,
                    HousingFundEnterprise = s.住房公积金企,
                    TaxableIncome = s.应纳税所得额,
                    IncomeTax = s.个人所得税,
                    SalaryPaid = s.实发,
                    Remark = s.备注 
                });
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #region 方法
        string GetDate(string date)
        {
            if (date.Length != 6)
                return "";
            DateTime dt = DateTime.ParseExact(date, "yyyyMM", System.Globalization.CultureInfo.InvariantCulture);
            return dt.ToString("yyyy年M月");
        }
        #endregion
    }
}
