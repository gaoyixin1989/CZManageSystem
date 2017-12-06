using CZManageSystem.Data.Domain.CollaborationCenter.Invest;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

/// <summary>
/// 历史项目暂估申请明细表
/// </summary>
namespace CZManageSystem.Service.CollaborationCenter.Invest
{
    public interface IInvestAgoEstimateApplyDetailService : IBaseService<InvestAgoEstimateApplyDetail>
    {
        dynamic Import(Stream fileStream, Users user,Guid applyID);
    }
}
