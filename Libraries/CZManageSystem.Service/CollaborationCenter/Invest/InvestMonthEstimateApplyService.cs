using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

namespace CZManageSystem.Service.CollaborationCenter.Invest
{
    public class InvestMonthEstimateApplyService :BaseService<InvestMonthEstimateApply>,IInvestMonthEstimateApplyService
    {
        ITracking_TodoService tracking_TodoService = new Tracking_TodoService();
        IDataDictionaryService _dataDictionaryService = new DataDictionaryService();

        public override IEnumerable<dynamic> GetForPaging(out int count, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var source = objs == null ? this._entityStore.Table.OrderBy(c => c.ApplyID) : this._entityStore.Table.OrderBy(c => c.ApplyID).Where(ExpressionFactory(objs));
            PagedList<InvestMonthEstimateApply> pageList = new PagedList<InvestMonthEstimateApply>(source, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count, true);
            count = pageList.TotalCount;
            #region Select
            return pageList.Select(u => new
            {
                u.Series,
                u.ApplyDpCode,
                u.Mobile,
                ApplyTime = Convert.ToDateTime(u.ApplyTime).ToString("yyyy-MM-dd HH:mm:ss"),
                u.Title,
                u.Status,
                u.Applicant,
                u.ApplyID,
                u.WorkflowInstanceId,
                u.TrackingWorkflow?.State,
                ActorName = string.IsNullOrEmpty(u.WorkflowInstanceId?.ToString()) ? u.Applicant : GetActorName(u.WorkflowInstanceId)
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
