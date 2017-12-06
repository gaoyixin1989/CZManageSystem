using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CZManageSystem.Core;
using CZManageSystem.Core.Caching;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.SysManger;
using System.Linq.Expressions;

namespace CZManageSystem.Service.SysManger
{
    public partial class SysUserService : BaseService<Users>, ISysUserService
    {
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="count">返回记录总数</param>
        /// <param name="objs">条件数组</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public override IEnumerable<dynamic> GetForPaging(out int count, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var source = objs == null ? this._entityStore.Table.OrderBy(c => c.UserId) : this._entityStore.Table.OrderBy(c => c.UserId).Where(ExpressionFactory(objs));
            source = source.Where(u => u.UserType!=null);
            PagedList<Users> pageList = new PagedList<Users>(source, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count);
            count = pageList.TotalCount;
            return pageList.Select(u => new
            {
                u.UserId,
                u.UserName,
                u.RealName,
                u.Mobile,
                u.Email,
                CreatedTime = u.CreatedTime.ToString()
            });
        }


        public IList<Users> GetForPagingByCondition(out int count, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var source = objs == null ? this._entityStore.Table.OrderBy(p => p.UserType) : this._entityStore.Table.OrderBy(p => p.UserType).Where(ExpressionFactory(objs));
            source = source.Where(u => u.UserType != null);
            PagedList<Users> pageList = new PagedList<Users>(source, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count);
            count = pageList.TotalCount;
            return pageList.ToList();
        }

        public IEnumerable<dynamic> GetForPaging_shortData(out int count, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var source = objs == null ? this._entityStore.Table.OrderBy(c => c.UserId) : this._entityStore.Table.OrderBy(c => c.UserId).Where(ExpressionFactory(objs));
            source = source.Where(u => u.UserType != null);
            PagedList<Users> pageList = new PagedList<Users>(source, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count);
            count = pageList.TotalCount;
            return pageList.Select(u => new
            {
                u.UserId,
                u.UserName,
                u.RealName,
                u.DpId,
                u.Mobile
            });
        }
    }
}