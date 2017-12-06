using Aspose.Cells;
using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.Administrative.VehicleManages;
using CZManageSystem.Data.Domain.MarketPlan;

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace CZManageSystem.Service.MarketPlan
{
    public class Ucs_MarketPlanLogService : BaseService<Ucs_MarketPlanLog>, IUcs_MarketPlanLogService
    {
        public  IEnumerable<dynamic> GetForPaging(out int count, MarketPlanLogQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var curTable = this._entityStore.Table;
            //if (objs != null)
            //{
            //    var exp = ExpressionFactory(objs);
            //    curTable = curTable.Where(exp);
            //}
            if (objs.Name != null)
                curTable = curTable.Where(a => a.Name.Contains(objs.Name));
            if (objs.StartTime != null)
                curTable = curTable.Where(a => a.Creattime >= objs.StartTime);
            if (objs.EndTime != null)
            {
                var dt = Convert.ToDateTime(objs.EndTime).AddDays(1);
                curTable = curTable.Where(a => a.Creattime <= dt);
            }

            return new PagedList<Ucs_MarketPlanLog>(curTable.OrderByDescending(c => c.Creattime), pageIndex <= 0 ? 0 : pageIndex, pageSize, out count);
        }
    }
}
