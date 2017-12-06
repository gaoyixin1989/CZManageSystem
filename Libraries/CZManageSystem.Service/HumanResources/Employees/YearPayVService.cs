using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.HumanResources.Employees; 
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.HumanResources.Employees
{
    public class YearPayVService : BaseService<YearPayV>, IYearPayVService
    {
        static SystemContext dbContext = new SystemContext("SqlConnectionHR"); 
        public YearPayVService() : base(dbContext)
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
            var source = objs == null ? this._entityStore.Table.OrderBy(c => c.employerid) : this._entityStore.Table.OrderBy(c => c.employerid).Where(ExpressionFactory(objs));
            PagedList<YearPayV> pageList = new PagedList<YearPayV>(source, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count, true);
            count = pageList.TotalCount;
            return pageList.Select(s => new
            {
                EmployerId = s.employerid,
                DeptName = s.deptname,
                Billcyc = s.billcyc,
                Bz = s.bz,
                Pos = s.pos,
                Postr = s.postr,
                Sjhm = s.sjhm,
                EmployerName = s.姓名,
                NetPayroll = s.实发奖金合计,
                DoublePayNetPayroll = s.年终双薪奖实发,
                DoublePay = s.年终双薪奖应发,
                YearEndAppraisaAwardNetPayroll = s.年终考核奖实发,
                YearEndAppraisaAward = s.年终考核奖应发,
                Total = s.应发合计,
                IndividualIncomeTax = s.应扣个所
            });
        }
        
    }
}
