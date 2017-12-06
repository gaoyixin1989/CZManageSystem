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
    public class Tracking_TodoService : BaseService<Tracking_Todo>, ITracking_TodoService
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

            var source = objs == null ? this._entityStore.Table.OrderByDescending(c => c.CreatedTime) : this._entityStore.Table.Where(ExpressionFactory(objs)).OrderByDescending(c => c.CreatedTime);
            PagedList<Tracking_Todo> pageList = new PagedList<Tracking_Todo>(source.Skip(0).Take(100), pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count, false);
            count = pageList.TotalCount;

            return pageList.Select(c => new
            {
                c.ActivityInstanceId,
                c.ActivityName,
                c.Actor,
                c.AliasImage,
                c.CreatedTime,
                c.Creator,
                c.CreatorName,
                c.FinishedTime,
                c.Importance,
                c.IsCompleted,
                c.OperateType,
                c.ProxyName,
                c.SheetId,
                c.StartedTime,
                c.State,
                c.Title,
                c.TodoActors,
                c.Urgency,
                c.UserName,
                c.WorkflowAlias,
                c.WorkflowInstanceId,
                c.WorkflowName,
                c.TrackingType ,
                c.ExternalEntityId,
                c.ExternalEntityType 
            }).ToList();
        }
    }
}
