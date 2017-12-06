using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.HumanResources.Vacation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.HumanResources.AnnualLeave
{
    public class HRVacationAnnualLeaveConfigService : BaseService<HRVacationAnnualLeaveConfig>, IHRVacationAnnualLeaveConfigService
    {
        //public override IEnumerable<dynamic> GetForPaging(out int count, HRVacationAnnualLeaveConfigQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        //{
        //    var source = objs == null ? this._entityStore.Table.OrderByDescending(c => c.Annual) : this._entityStore.Table.OrderByDescending(c => c.Annual).Where(ExpressionFactory(objs));
        //    PagedList<HRVacationAnnualLeaveConfig> pageList = new PagedList<HRVacationAnnualLeaveConfig>(source, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count, true);
        //    count = pageList.TotalCount;
        //    #region Select
        //    return pageList.Select(u => new
        //    {
        //        u.ID,
        //        u.Annual,
        //        u.LimitMonth,
        //        u.SpanTime
        //    });
        //    #endregion
        //}


        public IList<HRVacationAnnualLeaveConfig> GetForPagingByCondition(out int count, HRVacationAnnualLeaveConfigQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var curData = objs == null ? this._entityStore.Table.OrderByDescending(c => c.Annual) : this._entityStore.Table.OrderByDescending(c => c.Annual).Where(ExpressionFactory(objs));
            count = curData.Count();
            var list = curData.OrderByDescending(c => c.Annual).Skip(pageIndex * pageSize).Take(pageSize);            
            return list.ToList();
        }
    }
}
