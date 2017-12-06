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
    public class VW_Birthcontrol_GE_DataService : BaseService<VW_Birthcontrol_GE_Data>, IVW_Birthcontrol_GE_DataService
    {
        static Users _user;
        public IList<VW_Birthcontrol_GE_Data> GetForPagingByCondition(Users user, out int count, BirthControlGEBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            //var curTable = GetQueryTable(objs);
            var curTable = this._entityStore.Table;
            //if (objs != null)
            //{
            //    var exp = ExpressionFactory(objs);
            //    curTable = curTable.Where(exp);
            //}
            if (objs.EmployeeId != null)
                curTable = curTable.Where(u => u.EmployeeId.Contains(objs.EmployeeId));
            if(objs.BornSituation!=null)
            {
                if(objs.BornSituation == 1|| objs.BornSituation == 2)
                {
                    curTable = curTable.Where(u => u.BornSituation == objs.BornSituation);
                }
                else if(objs.BornSituation == 3)
                {
                    curTable = curTable.Where(u => u.BornSituation >= objs.BornSituation);
                }
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
