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
/// 合同信息
/// </summary>
namespace CZManageSystem.Service.CollaborationCenter.Invest
{
    public interface IInvestContractService : IBaseService<InvestContract>
    {
        IList<InvestContract> GetForPaging(out int count, InvestContractQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
        dynamic Import(Stream fileStream, Users user);
        dynamic Import_ModifyUser(Stream fileStream, Users user);
    }
}
