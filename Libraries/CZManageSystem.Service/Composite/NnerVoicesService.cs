using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.Composite;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.Composite
{
    public class NnerVoicesService : BaseService<InnerVoices>, INnerVoicesService
    {

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="count">返回记录总数</param>
        /// <param name="objs">条件数组</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        //public override IEnumerable<dynamic> GetForPaging(out int count, InnerVoicesQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        //{
        //    var curTable = GetQueryTable(objs);

        //    var source = objs == null ? this._entityStore.Table.OrderByDescending(c => c.CreateTime)
        //        : this._entityStore.Table.OrderByDescending(c => c.CreateTime).Where(ExpressionFactory(objs));
        //    PagedList<InnerVoices> pageList = new PagedList<InnerVoices>(source, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count);
        //    count = pageList.TotalCount;
        //    return pageList.Select(u => new
        //    {
        //        u.Id,
        //        u.DeptName,
        //        u.WorkflowInstanceId,
        //        u.TrackingWorkflow,
        //        u.Applytitle,
        //        u.Applysn,
        //        u.Creator,
        //        u.Creatorid,
        //        u.Themetype,
        //        u.Content,
        //        u.IsNiming,
        //        u.Attids,
        //        u.Remark,
        //        u.IsInfo,
        //        u.Username,
        //        u.Phone,
        //        CreateTime = u.CreateTime.ToString()
        //    });
        //}

        public IQueryable<InnerVoices> GetForPaging(out int count, InnerVoicesQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var curTable = GetQueryTable(objs);
            count = curTable.Count();
            var list = curTable.OrderByDescending(p => p.CreateTime).Skip(pageIndex * pageSize).Take(pageSize);
            return list;
        }

        /// <summary>
        /// 编辑查询条件
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        private IQueryable<InnerVoices> GetQueryTable(InnerVoicesQueryBuilder obj = null)
        {
            var curTable = this._entityStore.Table;
            if (obj != null)
            {
                InnerVoicesQueryBuilder obj2 = (InnerVoicesQueryBuilder)CloneObject(obj);
                if (obj.isSumbit.HasValue)
                {//状态
                    curTable = curTable.Where(u => u.WorkflowInstanceId.HasValue == obj.isSumbit);
                    obj2.isSumbit = null;
                }

                var exp = ExpressionFactory(obj2);
                if (exp != null)
                    curTable = curTable.Where(exp);

            }

            return curTable;
        }

    }
}
