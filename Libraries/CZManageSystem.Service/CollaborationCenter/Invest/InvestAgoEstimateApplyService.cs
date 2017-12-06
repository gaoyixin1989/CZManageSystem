using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.CollaborationCenter.Invest;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.CollaborationCenter.Invest
{
    public class InvestAgoEstimateApplyService : BaseService<InvestAgoEstimateApply>, IInvestAgoEstimateApplyService
    {
        public IList<InvestAgoEstimateApply> GetForPaging(out int count, AgoEstimateApplyQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var curTable = GetQueryTable(objs);
            count = curTable.Count();
            var list = curTable.OrderBy(p => "").Skip(pageIndex * pageSize).Take(pageSize);
            return list.ToList();
        }

        /// <summary>
        /// 编辑查询条件
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        private IQueryable<InvestAgoEstimateApply> GetQueryTable(AgoEstimateApplyQueryBuilder obj = null)
        {
            var curTable = this._entityStore.Table;
            if (obj != null)
            {
                AgoEstimateApplyQueryBuilder obj2 = (AgoEstimateApplyQueryBuilder)CloneObject(obj);

                if (obj.WF_State.HasValue) {//流程状态
                    curTable = curTable.Where(u => u.Tracking_Workflow.State == obj.WF_State.Value);
                    obj2.WF_State = null;
                }
                

                var exp = ExpressionFactory(obj2);
                if (exp != null)
                    curTable = curTable.Where(exp);

            }

            return curTable;
        }
    }
}
