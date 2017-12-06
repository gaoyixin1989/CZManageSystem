
using CZManageSystem.Data.Domain.HumanResources.Attendance;
using CZManageSystem.Data.Domain.SysManger;
using System.IO;
using ZManageSystem.Core;

namespace CZManageSystem.Service.HumanResources.Attendance
{
    public interface IHRHolidaysService : IBaseService<HRHolidays>
    {
        dynamic Import(Users user,Stream fileStream);
    }
}