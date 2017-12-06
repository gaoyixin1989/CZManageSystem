using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

namespace CZManageSystem.Service.CollaborationCenter.Invest
{
    public interface IInvestTempEstimateService : IBaseService<InvestTempEstimate>
    {
        IList<InvestTempEstimate> GetForPaging(out int count, StopInvestTempEstimateQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
        dynamic Import(Stream fileStream, Users user);
    }
}
