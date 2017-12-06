using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CZManageSystem.Core;
using CZManageSystem.Data.Domain.SysManger;
using System.Linq.Expressions;
using ZManageSystem.Core;

namespace CZManageSystem.Service.SysManger
{
    public interface ISysUserService : IBaseService<Users>
    {
        IList<Users> GetForPagingByCondition(out int count, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
        IEnumerable<dynamic> GetForPaging_shortData(out int count, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}