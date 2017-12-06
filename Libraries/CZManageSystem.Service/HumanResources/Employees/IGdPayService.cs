
using CZManageSystem.Data.Domain.HumanResources.Employees;
using System.IO;
using ZManageSystem.Core;

namespace CZManageSystem.Service.HumanResources.Employees
{
    public interface IGdPayService : IBaseService<GdPay>
    {
         dynamic Import(Stream fileStream);
    }
}