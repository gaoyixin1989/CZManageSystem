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
    public class Tracking_Todo_CompletedService : BaseService<Tracking_Todo_Completed>, ITracking_Todo_CompletedService
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
            try
            {
                var source = this._entityStore.Table;
                if (objs == null)
                    source = source.OrderByDescending(u => u.finishedtime);
                else
                    source = source.Where(ExpressionFactory(objs)).OrderByDescending(u => u.finishedtime);

                PagedList<Tracking_Todo_Completed> pageList = new PagedList<Tracking_Todo_Completed>(source, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count);
                count = pageList.TotalCount;
                return pageList;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
