using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.Administrative.BirthControl;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.Administrative.BirthControl
{
    public class BirthControlLogService : BaseService<BirthControlLog>, IBirthControlLogService
    {
        static Users _user;
        public override IEnumerable<dynamic> GetForPaging(out int count, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            //var curTable = GetQueryTable(objs);
            var curTable = this._entityStore.Table;
            if (objs != null)
            {
                var exp = ExpressionFactory(objs);
                curTable = curTable.Where(exp);
            }

            return new PagedList<BirthControlLog>(curTable.OrderByDescending(c => c.OpTime), pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count);
        }
    }
}
