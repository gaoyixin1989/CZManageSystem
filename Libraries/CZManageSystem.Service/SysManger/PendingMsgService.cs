using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

namespace CZManageSystem.Service.SysManger
{
    public partial class PendingMsgService: BaseService<IAMS_PendingMsg>, IPendingMsgService
    {
        /// <summary>
        /// 待办工作分页
        /// </summary>
        /// <param name="count">返回记录总数</param>
        /// <param name="objs">条件数组</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public override IEnumerable<dynamic> GetForPaging(out int count, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var curTable = this._entityStore.Table; 
            if (objs != null)
            {
                var exp = ExpressionFactory(objs);
                curTable = curTable.Where(exp);
            }
            return new PagedList<IAMS_PendingMsg>(curTable.OrderBy(p => p.ProcessedDT).Skip(0).Take(100), pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count);
        }
    }
}
