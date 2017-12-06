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
    public class Consumable_MakeupService : BaseService<Consumable_Makeup>, IConsumable_MakeupService
    {
        public override IEnumerable<dynamic> GetForPaging(out int count, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var curTable = this._entityStore.Table;
            if (objs != null)
            {
                var exp = ExpressionFactory(objs);
                curTable = curTable.Where(exp);
            }

            return new PagedList<Consumable_Makeup>(curTable.OrderByDescending(c => c.AppTime), pageIndex <= 0 ? 0 : pageIndex, pageSize, out count);
        }

    }
}
