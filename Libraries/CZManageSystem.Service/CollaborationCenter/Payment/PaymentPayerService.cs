﻿using System;
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
    public partial class PaymentPayerService : BaseService<PaymentPayer>, IPaymentPayerService
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
            var source = objs == null ? this._entityStore.Table.OrderBy(c => c.PayerID) : this._entityStore.Table.OrderBy(c => c.PayerID).Where(ExpressionFactory(objs));
            PagedList<PaymentPayer> pageList = new PagedList<PaymentPayer>(source, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count);
            count = pageList.TotalCount;
            return pageList;
        }
    }
}
