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
    public partial class SysServicesService : BaseService<SysServices>, ISysServicesService
    {
        public override bool Insert(SysServices entity)
        {
            if (entity == null)
                return false;
            entity.Createdtime = entity.Createdtime ?? DateTime.Now;
            return this._entityStore.Insert(entity);
        }
        public override bool Update(SysServices entity)
        {
            if (entity == null)
                return false;
            var model = FindById(entity.ServiceId);
            model.ServiceName = entity.ServiceName == null ? model.ServiceName : entity.ServiceName;
            model.AssemblyName = entity.AssemblyName == null ? model.AssemblyName : entity.AssemblyName;
            model.ClassName = entity.ClassName == null ? model.ClassName : entity.ClassName;
            model.ServiceDesc = entity.ServiceDesc == null ? model.ServiceDesc : entity.ServiceDesc;
            model.Remark = entity.Remark == null ? model.Remark : entity.Remark;
            model.Createdtime = entity.Createdtime ?? model.Createdtime;
            model.Creator = entity.Creator == null ? model.Creator : entity.Creator;
            model.LastModTime = entity.LastModTime ?? DateTime.Now;
            model.LastModifier = entity.LastModifier == null ? model.LastModifier : entity.LastModifier;
            return this._entityStore.Update(model);
        }

    }
}