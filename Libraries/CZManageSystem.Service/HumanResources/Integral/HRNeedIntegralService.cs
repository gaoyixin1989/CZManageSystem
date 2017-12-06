using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.HumanResources.Integral;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.HumanResources.Integral
{
    public class HRNeedIntegralService : BaseService<HRNeedIntegral>, IHRNeedIntegralService
    {
        static Users _user;
        private readonly IRepository<Users> _bw_Users = new EfRepository<Users>();
        public IList<HRNeedIntegral> GetForPagingByCondition(Users user, out int count, HRNeedIntegralQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            //var curData = GetQueryTable(objs); ;
            var curData = from hrvc in this._entityStore.Table
                          join bwu in this._bw_Users.Table
                          on hrvc.UserId equals bwu.UserId
                          select new
                          {
                              hrvc.Id,
                              DpId = bwu.DpId,
                              UserName = bwu.UserName,
                              RealName = bwu.RealName,
                              hrvc.NeedIntegral,
                              hrvc.YearDate,
                              hrvc.UserId,
                              bwu.EmployeeId,
                              hrvc.DoFlag,
                              bwu.Status,
                              bwu.UserType
                          };
            curData = curData.Where(u => u.UserType == 1 && u.Status == 0);
            if (objs.DpId != null)
                curData = curData.Where(u => objs.DpId.Contains(u.DpId));
            if (objs.Year != null && objs.Year != "")
                curData = curData.Where(u => objs.Year.Contains(u.YearDate));
            if (objs.EmployeeID != null && objs.EmployeeID != "")
                curData = curData.Where(u => u.EmployeeId.Contains(objs.EmployeeID));
            if (objs.RealName != null && objs.RealName != "")
                curData = curData.Where(u => u.RealName.Contains(objs.RealName));
            _user = user;
            var mm = new EfRepository<string>().Execute<string>(string.Format("exec HR_GetUser @UserName='{0}'", _user.UserName)).ToList();
            curData = curData.Where(u => mm.Contains(u.UserName.ToString()));
            count = curData.Count();
            var list = curData.OrderByDescending(p => p.YearDate).Skip(pageIndex * pageSize).Take(pageSize).AsEnumerable().Select(x => new HRNeedIntegral()
            {
                Id = x.Id,
                NeedIntegral = x.NeedIntegral,
                DoFlag = x.DoFlag,
                UserId = x.UserId,
                YearDate = x.YearDate
            });
            return list.ToList();
        }
    }
}
