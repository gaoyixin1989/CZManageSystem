using Aspose.Cells;
using CZManageSystem.Core;
using CZManageSystem.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using CZManageSystem.Data.Domain.MarketPlan;

namespace CZManageSystem.Service.MarketPlan
{
    public class Ucs_MarketPlanMonitorService : BaseService<Ucs_MarketPlanMonitor>,IUcs_MarketPlanMonitorService
    {
        public  IEnumerable<dynamic> GetForPaging(out int count, MarketPlanMonitorQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var curTable = this._entityStore.Table;
            if (objs.ImportName != null)
                curTable = curTable.Where(a => a.ImportName.Contains(objs.ImportName));
            if (objs.StartTime != null)
                curTable = curTable.Where(a => a.Creattime >= objs.StartTime);
            if (objs.EndTime != null)
            {
                var dt = Convert.ToDateTime(objs.EndTime).AddDays(1);
                curTable = curTable.Where(a => a.Creattime <= dt);
            }

            return new PagedList<Ucs_MarketPlanMonitor>(curTable.OrderByDescending(c => c.Creattime), pageIndex <= 0 ? 0 : pageIndex, pageSize, out count);
        }
    }
}
