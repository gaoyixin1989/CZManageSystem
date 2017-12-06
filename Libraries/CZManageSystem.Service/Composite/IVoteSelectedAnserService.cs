
using CZManageSystem.Data.Domain.Composite;
using System.Collections.Generic;
using ZManageSystem.Core;

namespace CZManageSystem.Service.Composite
{
    public interface IVoteSelectedAnserService : IBaseService<VoteSelectedAnser>
    {
        IEnumerable<dynamic> GetForPaging_(out int count, string ApplyTitle = null, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}