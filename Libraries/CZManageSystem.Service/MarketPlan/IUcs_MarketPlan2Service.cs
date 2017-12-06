using CZManageSystem.Data.Domain.MarketPlan;
using System.IO;
using ZManageSystem.Core;
using CZManageSystem.Data.Domain.SysManger;

namespace CZManageSystem.Service.MarketPlan
{
    public interface IUcs_MarketPlan2Service : IBaseService<Ucs_MarketPlan2>
    {
        dynamic ImportDelUcs_MarketPlan2(Stream fileStream, Users user);
    }
}
