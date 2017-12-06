using CZManageSystem.Data.Domain.Administrative.Dinning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

namespace CZManageSystem.Service.Administrative.Dinning
{
    public interface IView_EXT_XF_AccountService : IBaseService<view_EXT_XF_Account>
    {
        IList<view_EXT_XF_Account> GetForPagingByCondition(out int count, view_EXT_XF_AccountQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
