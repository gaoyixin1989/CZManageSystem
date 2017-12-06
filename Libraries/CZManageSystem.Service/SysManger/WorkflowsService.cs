using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CZManageSystem.Core;
using CZManageSystem.Core.Caching;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.SysManger;
using System.Linq.Expressions;

namespace CZManageSystem.Service.SysManger
{
    public partial class WorkflowsService : BaseService<Workflows>, IWorkflowsService
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
            var source = objs == null ? this._entityStore.Table.OrderBy(c => c.WorkflowId) : this._entityStore.Table.OrderBy(c => c.WorkflowId).Where(ExpressionFactory(objs));
            PagedList<Workflows> pageList = new PagedList<Workflows>(source, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count);
            count = pageList.TotalCount;
            return pageList;
            //.Select(u => new
            //{
            //    u.WorkflowId,
            //    u.WorkflowName,
            //    u.Remark,
            //    u.Version
            //});
        }
        public IEnumerable<dynamic> GetList(out int count, out List<Guid> list, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var source = objs == null ? this._entityStore.Table.OrderBy(c => c.WorkflowId) : this._entityStore.Table.OrderBy(c => c.WorkflowId).Where(ExpressionFactory(objs));
            PagedList<Workflows> pageList = new PagedList<Workflows>(source, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count);
            count = pageList.TotalCount;
            list = pageList.Select(s => s.WorkflowId) .ToList();
            return pageList
            .Select(u => new
            {
                u.WorkflowId,
                u.WorkflowName,
                u.Remark,
                u.Version
            });
        }
    }
}