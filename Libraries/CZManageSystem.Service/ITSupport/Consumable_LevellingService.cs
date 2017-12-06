using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.ITSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.ITSupport
{
    /// <summary>
    /// 耗材调平申请表
    /// </summary>
    public class Consumable_LevellingService : BaseService<Consumable_Levelling>, IConsumable_LevellingService
    {
        public override IEnumerable<dynamic> GetForPaging(out int count, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var curTable = this._entityStore.Table;
            if (objs != null)
            {
                var exp = ExpressionFactory(objs);
                curTable = curTable.Where(exp);
            }

            return new PagedList<Consumable_Levelling>(curTable.OrderByDescending(c => c.ApplyTime), pageIndex <= 0 ? 0 : pageIndex, pageSize, out count);
        }

    }
}
