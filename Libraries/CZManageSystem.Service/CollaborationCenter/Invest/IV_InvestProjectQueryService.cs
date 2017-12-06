using CZManageSystem.Data.Domain.CollaborationCenter.Invest;
using CZManageSystem.Data.Domain.HumanResources.ShiftManages;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

namespace CZManageSystem.Service.CollaborationCenter.Invest
{
    public interface IV_InvestProjectQueryService : IBaseService<V_InvestProjectQuery>
    {
        IList<V_InvestProjectQuery> GetForPaging(out int count, VInvestProjectQueryQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);

    }
}
