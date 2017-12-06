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
    public class HRIntegralStaticService : BaseService<HRIntegralStatic>, IHRIntegralStaticService
    {
        static Users _user;
        public IList<HRIntegralStatic> GetForPagingByCondition(out int count, int pageIndex, int pageSize, Users user, IntegralStaticQueryBuilder objs = null)
        {
            _user = user;
            var mm3 = new EfRepository<HRIntegralStatic>().ExecuteResT<HRIntegralStatic>(string.Format("exec HRIntegral_GetStaticData '{0}','{1}','{2}','{3}','{4}'", _user.UserName, objs.Year, objs.EmployeeId, objs.RealName, objs.DpId));
            List<HRIntegralStatic> resultList = new List<HRIntegralStatic>();
            foreach (var x in mm3)
            {
                resultList.Add(x);
            }

            PagedList<HRIntegralStatic> pageList = new PagedList<HRIntegralStatic>(resultList, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);
            count = resultList.Count();
            return pageList;
        }



        //public IList<object> GetForPagingByCondition(out int count, int pageIndex, int pageSize, Users user, string DpId = null, string EmployeeId = null, string RealName = null, string Year = null)
        //{
        //    _user = user;
        //    var mm3 = new EfRepository<HRIntegralStatic>().ExecuteResT<HRIntegralStatic>(string.Format("exec HRIntegral_GetStaticData '{0}','{1}','{2}','{3}','{4}'", _user.UserName, Year, EmployeeId, RealName, DpId));
        //    List<object> resultList = new List<object>();
        //    foreach (var x in mm3)
        //    {
        //        resultList.Add(x);
        //    }

        //    PagedList<object> pageList = new PagedList<object>(resultList, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);
        //    count = resultList.Count();
        //    return pageList;
        //}
    }
}
