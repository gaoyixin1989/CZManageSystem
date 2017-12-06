using CZManageSystem.Data;
using CZManageSystem.Data.Domain.HumanResources.ShiftManages;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.HumanResources.ShiftManages
{
    public class ShiftZhibanService : BaseService<ShiftZhiban>, IShiftZhibanService
    {
        public IList<ShiftZhiban> GetForPaging(out int count, ZhibanQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
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
        private IQueryable<ShiftZhiban> GetQueryTable(ZhibanQueryBuilder obj = null)
        {
            var curTable = this._entityStore.Table;
            if (obj != null)
            {
                ZhibanQueryBuilder obj2 = (ZhibanQueryBuilder)CloneObject(obj);
                if (obj.State != null && obj.State.Length > 0)
                {//状态
                    curTable = curTable.Where(u => obj.DeptId.Contains(u.DeptId));
                    obj2.DeptId = null;
                }

                var exp = ExpressionFactory(obj2);
                if (exp != null)
                    curTable = curTable.Where(exp);

            }

            return curTable;
        }
    }
}
