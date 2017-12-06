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
/// 投资项目信息
/// </summary>
namespace CZManageSystem.Service.CollaborationCenter.Invest
{
    public interface IInvestProjectService : IBaseService<InvestProject>
    {
        IList<InvestProject> GetForPaging(out int count, InvestProjectQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
        dynamic Import(Stream fileStream, Users user);
    }
}
