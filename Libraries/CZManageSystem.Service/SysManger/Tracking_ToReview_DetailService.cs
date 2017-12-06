using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.SysManger
{
    public class Tracking_ToReview_DetailService : BaseService<Tracking_ToReview_Detail>,ITracking_ToReview_DetailService
    {
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
            var source = objs == null ? this._entityStore.Table.OrderByDescending(c => c.CreatedTime) : this._entityStore.Table.OrderByDescending(c => c.CreatedTime).Where(ExpressionFactory(objs));
            PagedList<Tracking_ToReview_Detail> pageList = new PagedList<Tracking_ToReview_Detail>(source.Skip(0).Take(100), pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count);
            count = pageList.TotalCount;
            return pageList;
        }
    }
}
