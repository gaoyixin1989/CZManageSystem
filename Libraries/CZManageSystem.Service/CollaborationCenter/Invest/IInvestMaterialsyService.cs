using CZManageSystem.Data.Domain.CollaborationCenter.Invest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

/// <summary>
/// 物资采购
/// </summary>
namespace CZManageSystem.Service.CollaborationCenter.Invest
{
    public interface IInvestMaterialsService : IBaseService<InvestMaterials>
    {
        dynamic Import(Stream fileStream);
    }
}
