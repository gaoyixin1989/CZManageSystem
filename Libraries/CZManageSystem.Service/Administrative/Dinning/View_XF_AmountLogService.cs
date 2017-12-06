using CZManageSystem.Data;
using CZManageSystem.Data.Domain.Administrative.Dinning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.Administrative.Dinning
{
    public class View_XF_AmountLogService : BaseService<view_XF_AmountLog>, IView_XF_AmountLogService
    {
        static SystemContext dbContext = new SystemContext("YKTBalanceConnect");
        public View_XF_AmountLogService() : base(dbContext)
        { }

        public IList<view_XF_AmountLog> GetForPagingByCondition(out int count, view_XF_AmountLogQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var curData = objs == null ? this._entityStore.Table.OrderBy(c => c.CreateTime) : this._entityStore.Table.OrderBy(c => c.CreateTime).Where(ExpressionFactory(objs));

            var list = curData.OrderByDescending(p => p.CreateTime).Skip(pageIndex * pageSize).Take(pageSize).AsEnumerable().Select(x => new view_XF_AmountLog()
            {
                JobNumber = x.JobNumber,
                AddAmount = x.AddAmount,
                TypeContent = x.TypeContent,
                Name = x.Name,
                CreateTime= x.CreateTime,
                DepartmentName = x.DepartmentName,
                BelAmount = x.BelAmount
            });
            count = curData.Count();
            return list.ToList();
        }


    }
}
