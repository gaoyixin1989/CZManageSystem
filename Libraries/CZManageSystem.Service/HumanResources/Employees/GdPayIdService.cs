using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.HumanResources.Employees;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.HumanResources.Employees
{
    public class GdPayIdService : BaseService<GdPayId>, IGdPayIdService
    {
        static SystemContext dbContext = new SystemContext("SqlConnectionHR");
        public GdPayIdService() : base(dbContext)
        { }
        
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
            try
            {
                var source = objs == null ? this._entityStore.Table.OrderBy(c => c.payid) : this._entityStore.Table.OrderBy(c => c.payid).Where(ExpressionFactory(objs));
                PagedList<GdPayId> pageList = new PagedList<GdPayId>(source, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count);
                count = pageList.TotalCount;
                return pageList.Select(s => new
                {
                    s.bz,
                    s.DataType,
                    s.Inherit,
                    s.payid,
                    s.payname,
                    s.pid,
                    s.RowExclusive,
                    s.sort
                });
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="count">返回记录总数</param>
        /// <param name="objs">条件数组</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetForPaging_(out int count, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            try
            { 
                var source = objs == null ? this._entityStore.Table.OrderBy(c => c.payid) : this._entityStore.Table.OrderBy(c => c.payid).Where(ExpressionFactory(objs));
                PagedList<GdPayId> pageList = new PagedList<GdPayId>(source, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count);
                count = pageList.TotalCount;
                return pageList.Select(s => new
                {//t1.payid,t1.payname,t1.bz,t1.sort,t4.payname as title
                    s.bz,
                    s.payid,
                    s.payname,
                    s.pid,
                    s.sort,
                   title = this._entityStore.FindByFeldName (f=>f.payid ==s.pid )?.payname 
                });
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
