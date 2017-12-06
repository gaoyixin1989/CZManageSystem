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
/// 历史项目暂估申请表
/// </summary>
namespace CZManageSystem.Service.CollaborationCenter.Invest
{
    public interface IInvestAgoEstimateApplyService : IBaseService<InvestAgoEstimateApply>
    {
        IList<InvestAgoEstimateApply> GetForPaging(out int count, AgoEstimateApplyQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
