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
    public interface IHRVacationCoursesService : IBaseService<HRVacationCourses>
    {
        dynamic ImportVacationCourses(string filename,Stream fileStream, Users user);
        IList<HRVacationCourses> GetForPersonalPagingByCondition(out int count, HRVacationCoursesQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
        IList<HRVacationCourses> GetForPagingByCondition(Users user, out int count, HRVacationCoursesQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
