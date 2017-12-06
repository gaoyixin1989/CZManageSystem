using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.SysManger
{
    public class SysDeptmentService: BaseService<Depts>, ISysDeptmentService
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
            return new PagedList<Depts>(this._entityStore.Table.Where(u => u.Type == 1).OrderBy(u=>u.DeptOrderNo).ThenBy(c => c.DpId ), pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count)
                //.Select(u => new
                //{
                //    u.UserId,
                //    u.RealName,
                //    u.Mobile,
                //    u.Email,
                //    u.CreatedTime
                //})
                ;
        }


    }
}
