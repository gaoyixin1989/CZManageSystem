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
/// 营销订单-号码段维护
/// </summary>
namespace CZManageSystem.Service.CollaborationCenter.MarketOrder
{
    public interface IMarketOrder_NumberService : IBaseService<MarketOrder_Number>
    {
        dynamic Import(Stream fileStream, Users user);
    }
}
