
using CZManageSystem.Data.Domain.Administrative;
using System.Collections.Generic;
using System.Linq;
using ZManageSystem.Core;

namespace CZManageSystem.Service.Administrative
{
    public interface IBoardroomApplyService : IBaseService<BoardroomApply>
    {
        IQueryable<BoardroomApply> GetForPaging(out int count, BoardroomApplyQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}