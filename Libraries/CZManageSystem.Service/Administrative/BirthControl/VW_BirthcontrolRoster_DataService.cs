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
    public class VW_BirthcontrolRoster_DataService : BaseService<VW_BirthcontrolRoster_Data>, IVW_BirthcontrolRoster_DataService
    {
        static Users _user;
        public IList<VW_BirthcontrolRoster_Data> GetForPagingByCondition(Users user, out int count, BirthControlRosterBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var curData = GetQueryTable(objs); ;
            _user = user;
            var mm = new EfRepository<string>().Execute<string>(string.Format("exec BirthControl_GetUser @UserName='{0}'", _user.UserName)).ToList();
            curData = curData.Where(u => mm.Contains(u.UserName.ToString()));
            count = curData.Count();
            var list = curData.OrderBy(p => "").Skip(pageIndex * pageSize).Take(pageSize);
            return list.ToList();
        }
        /// <summary>
        /// 编辑查询条件
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        private IQueryable<VW_BirthcontrolRoster_Data> GetQueryTable(BirthControlRosterBuilder obj = null)
        {
            var curTable = this._entityStore.Table;
            if (obj != null)
            {
                BirthControlRosterBuilder obj2 = (BirthControlRosterBuilder)CloneObject(obj);
                if (obj.DpId != null && obj.DpId.Count > 0)
                {//状态
                    curTable = curTable.Where(u => obj.DpId.Contains(u.DpId));
                    obj2.DpId = null;
                }

                var exp = ExpressionFactory(obj2);
                if (exp != null)
                    curTable = curTable.Where(exp);

            }

            return curTable;
        }
    }
}
