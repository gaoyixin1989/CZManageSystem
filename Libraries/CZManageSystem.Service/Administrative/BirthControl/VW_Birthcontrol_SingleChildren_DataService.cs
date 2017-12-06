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
    public class VW_Birthcontrol_SingleChildren_DataService : BaseService<VW_Birthcontrol_SingleChildren_Data>, IVW_Birthcontrol_SingleChildren_DataService
    {
        static Users _user;
        public IList<VW_Birthcontrol_SingleChildren_Data> GetForPagingByCondition(Users user, out int count, BirthControlGEBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            //var curTable = GetQueryTable(objs);
            var curTable = this._entityStore.Table;
            if (objs != null)
            {
                var exp = ExpressionFactory(objs);
                curTable = curTable.Where(exp);
            }

            _user = user;
            var mm = new EfRepository<string>().Execute<string>(string.Format("exec BirthControl_GetUser @UserName='{0}'", _user.UserName)).ToList();
            curTable = curTable.Where(u => mm.Contains(u.UserName.ToString()));
            count = curTable.Count();
            var list = curTable.OrderBy(p => "").Skip(pageIndex * pageSize).Take(pageSize);
            return list.ToList();
        }
    }
}
