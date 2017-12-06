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
/// 已转资合同表
/// </summary>
namespace CZManageSystem.Service.CollaborationCenter.Invest
{
    public interface IInvestTransferPayService : IBaseService<InvestTransferPay>
    {
        dynamic Import(Stream fileStream, Users user);
        bool CheckRepeat(Guid ID, string ProjectID, string ContractID);
    }
}
