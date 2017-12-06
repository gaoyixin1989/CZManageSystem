using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.Administrative.VehicleManages;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.Administrative.VehicleManages
{
    public class CarApplyRentService: BaseService<CarApplyRent>, ICarApplyRentService
    {
        ITracking_TodoService tracking_TodoService = new Tracking_TodoService();
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
            var source = objs == null ? this._entityStore.Table.OrderBy(c => c.ApplyRentId) : this._entityStore.Table.OrderBy(c => c.ApplyRentId).Where(ExpressionFactory(objs));
            PagedList<CarApplyRent> pageList = new PagedList<CarApplyRent>(source, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count, true);
            count = pageList.TotalCount;
            #region Select
            return pageList.Select(u => new
            {
                u.ApplyRentId,
                u.CorpId,
                u.ApplyTitle,
                CreateTime = Convert.ToDateTime(u.CreateTime).ToString("yyyy-MM-dd"),
                u.DeptName,
                u.ApplyCant,
                u.Driver,
                u.Mobile,
                u.TimeOut,
                u.StartTime,
                u.EndTime,
                u.Starting,
                u.Destination1,
                u.Destination2,
                u.Destination3,
                u.Destination4,
                u.Destination5,
                u.PersonCount,
                u.Road,
                u.UseType,
                u.Request,
                u.Attach,
                u.Field00,
                u.Field01,
                u.Field02,
                u.Allocator,
                u.AllotTime,
                u.Remark,
                u.Htype,
                u.CarTonnage,
                u.Enquiry,
                u.AllotRight,
                u.WorkflowInstanceId,
                u.TrackingWorkflow?.State,
                ActorName = string.IsNullOrEmpty(u.WorkflowInstanceId?.ToString()) ? u.ApplyCant : GetActorName(u.WorkflowInstanceId)
            });
            #endregion
        }

        string GetActorName(Guid? workflowInstanceId)
        {
            var list = tracking_TodoService.List().Where(w => w.WorkflowInstanceId == workflowInstanceId).Select(s => s.ActorName);
            return string.Join(",", list);
        }
    }
}
