using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CZManageSystem.Core;
using CZManageSystem.Core.Caching;
using CZManageSystem.Data;
using System.Linq.Expressions;
using CZManageSystem.Data.Domain.HumanResources.Knowledge;

namespace CZManageSystem.Service.HumanResources.Knowledge
{
    public partial class SysKnowledgeService : BaseService<SysKnowledge>, ISysKnowledgeService
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
            var curTable = this._entityStore.Table;
            if (objs != null)
            {
                var exp = ExpressionFactory(objs);
                curTable = curTable.Where(exp);
            }

            return new PagedList<SysKnowledge>(curTable.OrderByDescending(c=>c.OrderNo).ThenByDescending(c => c.Createdtime), pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count);
        }
        
    }
}