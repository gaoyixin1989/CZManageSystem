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
    //public partial class SysServiceStrategyLogService : BaseService<SysServiceStrategyLog>, ISysServiceStrategyLogService
    //{
    //    public override bool Insert(SysServiceStrategyLog entity)
    //    {
    //        if (entity == null)
    //            return false;
    //        entity.LogTime = entity.LogTime ?? DateTime.Now;
    //        return this._entityStore.Insert(entity);
    //    }
    //    public override bool Update(SysServiceStrategyLog entity)
    //    {
    //        if (entity == null)
    //            return false;
    //        var model = FindById(entity.LogId);
    //        model.LogTime = entity.LogTime ?? DateTime.Now;
    //        model.ServiceStrategyId = entity.ServiceStrategyId ?? model.ServiceStrategyId;
    //        model.Result = entity.Result ?? model.Result;
    //        model.Description = entity.Description ?? model.Description;

    //        return this._entityStore.Update(model);
    //    }

    //    /// <summary>
    //    /// 分页
    //    /// </summary>
    //    /// <param name="count">返回记录总数</param>
    //    /// <param name="objs">条件数组</param>
    //    /// <param name="pageIndex">页码</param>
    //    /// <param name="pageSize">每页条数</param>
    //    /// <returns></returns>
    //    public override IEnumerable<dynamic> GetForPaging(out int count, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
    //    {
    //        //var curTable = GetQueryTable(objs);
    //        var curTable = this._entityStore.Table;
    //        if (objs != null)
    //        {
    //            var exp = ExpressionFactory(objs);
    //            curTable = curTable.Where(exp);
    //        }

    //        return new PagedList<SysServiceStrategyLog>(curTable.OrderByDescending(c => c.LogTime).OrderByDescending(c => c.LogId), pageIndex, pageSize, out count);
    //    }

    //}
}