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
/// 合同已付金额表
/// </summary>
namespace CZManageSystem.Service.CollaborationCenter.Invest
{
    public interface IInvestContractPayService : IBaseService<InvestContractPay>
    {
        IList<InvestContractPay> GetForPaging(out int count, InvestContractPayQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);

        dynamic Import(Stream fileStream, Users user);
    }
}
