
using CZManageSystem.Data.Domain.Composite;
using System;
using System.Collections.Generic;
using ZManageSystem.Core;

namespace CZManageSystem.Service.Composite
{
    public interface IVoteQuestionService : IBaseService<VoteQuestion>
    {
        IEnumerable<dynamic> GetForPaging_(out int count, Guid CreatorID, string ApplyTitle = null, string ThemeID = null, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}