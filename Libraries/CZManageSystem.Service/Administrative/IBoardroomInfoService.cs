
using CZManageSystem.Data.Domain.Administrative;
using CZManageSystem.Data.Domain.SysManger;
using System.Collections.Generic;
using System.IO;
using ZManageSystem.Core;

namespace CZManageSystem.Service.Administrative
{
    public interface IBoardroomInfoService:IBaseService<BoardroomInfo>
    {
        IList<BoardroomInfo> GetForPaging(out int count, BoardroomInfoQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
        dynamic ImportBoardroomInfo(Stream fileStream, Users user);
    }
}