using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CZManageSystem.Core;
using CZManageSystem.Core.Caching;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.Composite;
using System.Linq.Expressions;
using CZManageSystem.Data.Domain.CollaborationCenter.Payment;

namespace CZManageSystem.Service.CollaborationCenter.Payment
{
    public partial class PaymentCompanyHallService : BaseService<PaymentCompanyHall>, IPaymentCompanyHallService
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
            var source = objs == null ? this._entityStore.Table.OrderBy(c => c.DcId) : this._entityStore.Table.OrderBy(c => c.DcId).Where(ExpressionFactory(objs));
            PagedList<PaymentCompanyHall> pageList = new PagedList<PaymentCompanyHall>(source, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count);
            count = pageList.TotalCount;
            return pageList.Select(s => new
            {
                s.DcId,
                s.DpId,
                //PaymentPayees = s.PaymentPayees,
                s.CompanyId,
                s.Depts?.DpFullName
            });
        }
    }
}
