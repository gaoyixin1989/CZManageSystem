
using CZManageSystem.Data.Domain.HumanResources.Employees; 
using ZManageSystem.Core;
using System.IO;

namespace CZManageSystem.Service.HumanResources.Employees
{
    public interface IHRLzUserInfoService : IBaseService<HRLzUserInfo>
    {
        dynamic Import(Stream fileStream, string userName);
    }
}