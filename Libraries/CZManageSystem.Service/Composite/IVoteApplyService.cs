
using CZManageSystem.Data.Domain.Composite;
using CZManageSystem.Data.Domain.SysManger;
using System.Collections.Generic;
using ZManageSystem.Core;

namespace CZManageSystem.Service.Composite
{
    public interface IVoteApplyService : IBaseService<VoteApply>
    {
        IEnumerable<dynamic> GetForDetailPaging(out int count, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
        IEnumerable<dynamic> GetForPaging_(out int count, Users user, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}