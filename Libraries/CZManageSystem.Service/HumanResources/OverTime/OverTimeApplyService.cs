using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.HumanResources.Vacation;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.HumanResources.OverTime
{
    public class OverTimeApplyService : BaseService<HROverTimeApply>, IOverTimeApplyService
    {
        ITracking_TodoService tracking_TodoService = new Tracking_TodoService();
        ISysUserService _userService = new SysUserService();
        static Users _user;
        public override IEnumerable<dynamic> GetForPaging(out int count, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var source = objs == null ? this._entityStore.Table.OrderByDescending(c => c.CreateTime) : this._entityStore.Table.OrderByDescending(c => c.CreateTime).Where(ExpressionFactory(objs));
            PagedList<HROverTimeApply> pageList = new PagedList<HROverTimeApply>(source, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count, true);
            count = pageList.TotalCount;
            #region Select
            return pageList.Select(u => new
            {
                ApplyId=u.ApplyID,
                u.ApplyTitle,
                u.ApplySn,                
                u.ApplyUserName,
                StartTime = Convert.ToDateTime(u.StartTime).ToString("yyyy-MM-dd HH:mm"),
                EndTime = Convert.ToDateTime(u.EndTime).ToString("yyyy-MM-dd HH:mm"),
                CreateTime = Convert.ToDateTime(u.CreateTime).ToString("yyyy-MM-dd"),
                u.PeriodTime,
                u.ApplyPost,
                u.ManageName,
                u.ManagePost,
                u.Address,
                u.OvertimeType,
                u.Reason,
                u.Editor,
                u.Newpt,
                u.WorkflowInstanceId,
                u.TrackingWorkflow?.State,
                ActorName = string.IsNullOrEmpty(u.WorkflowInstanceId?.ToString()) ? u.ApplyUserName : GetActorName(u.WorkflowInstanceId)
            });
            #endregion
        }

        public IList<HROverTimeApply> GetForPagingByCondition(Users user, out int count, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var curData = objs == null ? this._entityStore.Table.OrderByDescending(c => c.CreateTime) : this._entityStore.Table.OrderByDescending(c => c.CreateTime).Where(ExpressionFactory(objs));
            _user = user;
           // var mm = new EfRepository<string>().Execute<string>(string.Format("exec HR_GetUser @UserName='{0}'", _user.UserName)).ToList();
            //var mm2 = _userService.List().Where(u => mm.Contains(u.UserName)).Select(u => u.UserId).ToList();
            //curData = curData.Where(u => mm2.Contains(u.Editor.Value));
            count = curData.Count();
            var list = curData.OrderByDescending(c => c.CreateTime).Skip(pageIndex * pageSize).Take(pageSize);
                //.Select(u=>
                //new 
                //{
                //    ApplyId = u.ApplyID,
                //    u.ApplyTitle,
                //    u.ApplySn,
                //    u.ApplyUserName,
                //    StartTime = Convert.ToDateTime(u.StartTime).ToString("yyyy-MM-dd HH:mm"),
                //    EndTime = Convert.ToDateTime(u.EndTime).ToString("yyyy-MM-dd HH:mm"),
                //    CreateTime = Convert.ToDateTime(u.CreateTime).ToString("yyyy-MM-dd"),
                //    u.PeriodTime,
                //    u.ApplyPost,
                //    u.ManageName,
                //    u.ManagePost,
                //    u.Address,
                //    u.OvertimeType,
                //    u.Reason,
                //    u.Editor,
                //    u.Newpt,
                //    u.WorkflowInstanceId,
                //    u.TrackingWorkflow.State,
                //    ActorName = string.IsNullOrEmpty(u.WorkflowInstanceId.ToString()) ? u.ApplyUserName : GetActorName(u.WorkflowInstanceId)
                //}
                //);
            return list.ToList();
        }
        string GetActorName(Guid? workflowInstanceId)
        {
            var list = tracking_TodoService.List().Where(w => w.WorkflowInstanceId == workflowInstanceId).Select(s => s.ActorName);
            return string.Join(",", list);
        }

    }
}
