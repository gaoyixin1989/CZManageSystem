﻿using CZManageSystem.Data;
using CZManageSystem.Data.Domain.HumanResources.Vacation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

namespace CZManageSystem.Service.HumanResources.Vacation
{
    public class HRVacationApplyService : BaseService<HRVacationApply>, IHRVacationApplyService
    {
        public IList<HRVacationApply> GetForPaging(out int count, VacationApplyQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
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
        private IQueryable<HRVacationApply> GetQueryTable(VacationApplyQueryBuilder obj = null)
        {
            var curTable = this._entityStore.Table;
            if (obj != null)
            {
                VacationApplyQueryBuilder obj2 = (VacationApplyQueryBuilder)CloneObject(obj);

                if (obj.WF_State != null && obj.WF_State.Count > 0)
                {//流程状态
                    curTable = curTable.Where(u => obj.WF_State.Contains(u.Tracking_Workflow.State));
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
