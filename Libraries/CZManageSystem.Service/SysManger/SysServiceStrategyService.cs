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
    public partial class SysServiceStrategyService : BaseService<SysServiceStrategy>, ISysServiceStrategyService
    {
        public override bool Insert(SysServiceStrategy entity)
        {
            if (entity == null)
                return false;
            entity.EnableFlag = entity.EnableFlag ?? false;
            entity.LogFlag = entity.LogFlag ?? false;
            entity.Createdtime = entity.Createdtime ?? DateTime.Now;
            return this._entityStore.Insert(entity);
        }
        public override bool Update(SysServiceStrategy entity)
        {
            if (entity == null)
                return false;
            var model = FindById(entity.Id);
            model.ServiceId = entity.ServiceId ?? model.ServiceId;
            model.ValidTime = entity.ValidTime ?? model.ValidTime;
            model.NextRunTime = entity.NextRunTime ?? model.NextRunTime;
            model.PeriodNum = entity.PeriodNum ?? model.PeriodNum;
            model.PeriodType = entity.PeriodType == null ? model.PeriodType : entity.PeriodType;
            model.EnableFlag = entity.EnableFlag ?? model.EnableFlag;
            model.LogFlag = entity.LogFlag ?? model.LogFlag;
            model.Remark = entity.Remark == null ? model.Remark : entity.Remark;
            model.Createdtime = entity.Createdtime ?? model.Createdtime;
            model.Creator = entity.Creator == null ? model.Creator : entity.Creator;
            model.LastModTime = entity.LastModTime ?? DateTime.Now;
            model.LastModifier = entity.LastModifier == null ? model.LastModifier : entity.LastModifier;

            return this._entityStore.Update(model);
        }

        public IList<SysServiceStrategy> QueryDataByServiceName(out int count, int pageIndex = 0, int pageSize = int.MaxValue, string ServiceName = null)
        {
            var curData = this._entityStore.Table;
            if (!string.IsNullOrEmpty(ServiceName))
                curData = curData.Where(u => u.SysServices.ServiceName.Contains(ServiceName));
            return new PagedList<SysServiceStrategy>(curData.OrderBy(c => c.Id), pageIndex <= 0 ? 0 : pageIndex, pageSize, out count);
        }

        public IList<SysServiceStrategy> GetValidServiceStrategyData()
        {
            //var curData = this._entityStore.Table.Where(u => (u.EnableFlag ?? false) == true && u.ValidTime.Value.Date >= DateTime.Now.Date);
            var queryCondition = new
            {
                ValidTime_start = DateTime.Now.Date
            };
            var curData = this._entityStore.Table.Where(u => (u.EnableFlag ?? false)).Where(ExpressionFactory(queryCondition));

            return curData.ToList();
        }
    }
}