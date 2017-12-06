using CZManageSystem.Data;
using CZManageSystem.Data.Domain.Administrative.Dinning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.Administrative.Dinning
{
    public class View_EXT_XF_AccountService : BaseService<view_EXT_XF_Account>, IView_EXT_XF_AccountService
    {
        static SystemContext dbContext = new SystemContext("YKTBalanceConnect");
        public View_EXT_XF_AccountService() : base(dbContext)
        { }

        public IList<view_EXT_XF_Account> GetForPagingByCondition(out int count, view_EXT_XF_AccountQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var curData = objs == null ? this._entityStore.Table.OrderBy(c => c.JobNumber) : this._entityStore.Table.OrderBy(c => c.JobNumber).Where(ExpressionFactory(objs));
           
            var list = curData.OrderByDescending(p => p.JobNumber).Skip(pageIndex * pageSize).Take(pageSize).AsEnumerable().Select(x => new view_EXT_XF_Account()
            {
                JobNumber = x.JobNumber,
                SystemNumber = x.SystemNumber,
                Name = x.Name,
                DepartmentName = x.DepartmentName,
                BelAmount  = x.BelAmount
            });
            count = curData.Count();
            return list.ToList();
        }

    }
}
