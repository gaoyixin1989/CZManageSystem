using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.OperatingFloor.ComeBack
{
    public class ComebackApplyService : BaseService<ComebackApply>, IComebackApplyService
    {
        public IEnumerable<dynamic> GetApplyInfoListData(out int count, ComebackInfoQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var curTable = this._entityStore.Table;

            if (objs.YearStart != null)
                curTable = curTable.Where(a => a.Year >= objs.YearStart);
            if (objs.YearEnd != null)
                curTable = curTable.Where(a => a.Year <= objs.YearEnd);
            if (objs.BudgetDept != null)
                curTable = curTable.Where(a => a.BudgetDept.Contains(objs.BudgetDept));
            if (objs.ApplyUser != null)
                curTable = curTable.Where(a => a.BudgetDept.Contains(objs.ApplyUser));
            count = 0;
            return new PagedList<ComebackApply>(curTable.OrderByDescending(c => c.ApplyId), pageIndex <= 0 ? 0 : pageIndex, pageSize, out count).Select(s => new
            {
                s.ApplyId,
                s.WorkflowInstanceId,
                s.Title,
                s.Series,
                s.Mobile,
                s.Status,
                ApplyTime = s.ApplyTime.HasValue ? s.ApplyTime.Value.ToString("yyyy-MM-dd") : "",
                s.ApplyDept,
                s.ApplyUser,
                s.BudgetDept,
                s.SourceTypeID,
                TimeStart = s.TimeStart.HasValue ? s.TimeStart.Value.ToString("yyyy-MM-dd") : "",
                TimeEnd = s.TimeEnd.HasValue ? s.TimeEnd.Value.ToString("yyyy-MM-dd") : "",
                s.SourceChildId,
                s.ProjName,
                s.PrevProjName,
                s.PrevProjCode,
                s.ProjManager,
                s.AppAmount,
                s.ProjAnalysis,
                s.Year,
                s.Remark,
                s.AppAmountHanshui
            });
        }
    }
}
