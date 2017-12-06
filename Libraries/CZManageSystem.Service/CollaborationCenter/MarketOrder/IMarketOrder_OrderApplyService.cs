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
/// 营销订单-营销订单工单
/// </summary>
namespace CZManageSystem.Service.CollaborationCenter.MarketOrder
{
    public interface IMarketOrder_OrderApplyService : IBaseService<MarketOrder_OrderApply>
    {
        IList<MarketOrder_OrderApply> GetForPaging(out int count, OrderApplyQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);

        dynamic Import(Stream fileStream, Users user);
    }
}
