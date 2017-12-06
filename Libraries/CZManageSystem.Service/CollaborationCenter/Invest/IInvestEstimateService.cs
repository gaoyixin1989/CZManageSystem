using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

namespace CZManageSystem.Service.CollaborationCenter.Invest
{
    public interface IInvestEstimateService : IBaseService<InvestEstimate>
    {
        IEnumerable<dynamic> GetForPaging_(out int count, ref dynamic total, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
