using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

/// <summary>
/// 营销订单-营销方案维护
/// </summary>
namespace CZManageSystem.Service.CollaborationCenter.MarketOrder
{
    public interface IMarketOrder_MarketService : IBaseService<MarketOrder_Market>
    {
        dynamic Import(Stream fileStream, Users user);
    }
}
