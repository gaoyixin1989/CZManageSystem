using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.HumanResources.AnnualLeave;
using CZManageSystem.Data.Domain.HumanResources.Vacation;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.HumanResources.AnnualLeave
{
    public class HRAnnualleaveStaticService : BaseService<HRAnnualLeaveStatic>, IHRAnnualleaveStaticService
    {
        static Users _user;
        ISysUserService _userService = new SysUserService();
        ISysDeptmentService _sysDeptmentService = new SysDeptmentService();
        private readonly IRepository<Users> _bw_Users = new EfRepository<Users>();
        private readonly IRepository<HRAnnualLeave> _hrannualleave = new EfRepository<HRAnnualLeave>();
        private readonly IRepository<HRVacationAnnualLeave> _hrvacationannualleave = new EfRepository<HRVacationAnnualLeave>();
        //public IList<HRAnnualLeaveStatic> GetForPagingByCondition(out int count, int pageIndex, int pageSize, Users user, HRAnnualLeaveStaticQueryBuilder objs = null)
        //{
        //    _user = user;

        //    var curData = from bwu in this._bw_Users.Table
        //                  join hrvc in this._hrannualleave.Table
        //                  on bwu.UserId equals hrvc.UserId
        //                  join hrvca in (
        //                    from t in _hrvacationannualleave.Table
        //                    where t.YearDate == objs.Year
        //                    group t by new { t.YearDate,t.UserID}
        //                    into s
        //                    select new
        //                    {
        //                        s.Key.UserID,
        //                        s.Key.YearDate,
        //                        SpendDays =s.Sum(p=>p.SpendDays)
        //                    }
        //                  ) on hrvc.UserId equals hrvca.UserID  into temp
        //                  from tt in temp.DefaultIfEmpty()
        //                  where hrvc.VYears == objs.Year
        //                  select new
        //                  {
        //                      hrvc.VYears,
        //                      DpId = bwu.DpId,
        //                      UserName = bwu.UserName,
        //                      RealName = bwu.RealName,
        //                      hrvc.VDays,
        //                      hrvc.FdYearVDays,
        //                      hrvc.UserId,
        //                      bwu.EmployeeId,
        //                      SpendDays = tt.SpendDays == null ? 0 : tt.SpendDays
        //                  };

        //    if (objs.DpId != null)
        //        curData = curData.Where(u => objs.DpId.Contains(u.DpId));
        //    if (objs.EmployeeId != null && objs.EmployeeId != "")
        //        curData = curData.Where(u => objs.EmployeeId.Contains(u.EmployeeId));
        //    if (objs.RealName != null && objs.RealName != "")
        //        curData = curData.Where(u => objs.RealName.Contains(u.RealName));
        //    _user = user;
        //    var mm = new EfRepository<string>().Execute<string>(string.Format("exec HR_GetUser @UserName='{0}'", _user.UserName)).ToList();
        //    curData = curData.Where(u => mm.Contains(u.UserName.ToString()));
        //    count = curData.Count();

        //    var list = curData.OrderByDescending(p => p.VYears).Skip(pageIndex * pageSize).Take(pageSize).AsEnumerable().Select(x => new HRAnnualLeaveStatic()
        //    {
        //        RealName = x.RealName,
        //        Years = Convert.ToDecimal(x.VYears),
        //        VDays = Convert.ToDecimal(x.VDays),
        //        EmployeeId = x.EmployeeId,
        //        SpendDays = x.SpendDays,
        //        Leftdays = Convert.ToDecimal(x.VDays) - x.SpendDays,
        //        UserId = x.UserId,
        //        UseDate = getAnnualLeaveDate(x.UserId, Convert.ToDecimal(x.VYears)),
        //        DpName = _sysDeptmentService.FindById(x.DpId).DpName
        //    });
        //    return list.ToList();
        //}

        public IList<HRAnnualLeaveStatic> GetForPagingByCondition(out int count, Users user, int pageIndex = 0, int pageSize = int.MaxValue,  HRAnnualLeaveStaticQueryBuilder objs = null)
        {
            _user = user;

            var mm3 = new EfRepository<HRAnnualLeaveStatic>().ExecuteResT<HRAnnualLeaveStatic>(string.Format("exec HRAnnualLeaveStatic '{0}','{1}','{2}','{3}','{4}'", _user.UserName, objs.Year, objs.EmployeeId, objs.RealName, objs.DpId));
            List<HRAnnualLeaveStatic> resultList = new List<HRAnnualLeaveStatic>();
            foreach (var x in mm3)
            {
                resultList.Add(x);
            }

            PagedList<HRAnnualLeaveStatic> pageList = new PagedList<HRAnnualLeaveStatic>(resultList, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);
            count = resultList.Count();
            return pageList;
        }
        private string getAnnualLeaveDate(Guid userid, decimal year)
        {
            var strResult = new EfRepository<string>().Execute<string>(string.Format("select  dbo.fun_GetAnnualLeaveUseDate ('{0}',{1})", userid, year)).ToList()[0];
            return strResult;
        }
    }
}
