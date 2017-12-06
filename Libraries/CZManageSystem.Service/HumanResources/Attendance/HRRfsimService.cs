using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.HumanResources.Attendance;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CZManageSystem.Service.HumanResources.Attendance
{
    public class HRRfsimService : BaseService<HRRfsim>, IHRRfsimService
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
                var source = objs == null ? this._entityStore.Table.OrderByDescending(c => c.CDateTime  ) : this._entityStore.Table.OrderByDescending(c => c.CDateTime).Where(ExpressionFactory(objs));
                PagedList<HRRfsim> pageList = new PagedList<HRRfsim>(source , pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count);
                count = pageList.TotalCount;
                return pageList ;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
       
    }
}
