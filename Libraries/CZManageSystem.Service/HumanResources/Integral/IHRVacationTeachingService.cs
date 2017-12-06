using CZManageSystem.Data.Domain.HumanResources.Vacation;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

namespace CZManageSystem.Service.HumanResources.Integral
{
    public interface IHRVacationTeachingService : IBaseService<HRVacationTeaching>
    {
        IList<HRVacationTeaching> GetForPagingByCondition(Users user, out int count, HRVacationTeachingQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);

        IList<HRVacationTeaching> GetForPersonalPagingByCondition(out int count, HRVacationTeachingQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue); 
         dynamic ImportVacationCourses(string filename, Stream fileStream, Users user);
    }
}
