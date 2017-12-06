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
    public partial class UsersGrounpService : BaseService<UsersGrounp>, IUsersGrounpService
    {
        public override bool Insert(UsersGrounp entity)
        {
            if (entity == null)
                return false;
            entity.CreatedTime = entity.CreatedTime ?? DateTime.Now;
            return this._entityStore.Insert(entity);
        }
        public override bool Update(UsersGrounp entity)
        {
            if (entity == null)
                return false;
            var model = FindById(entity.Id);
            model.GroupName = entity.GroupName == null ? model.GroupName : entity.GroupName;
            model.CreatedTime = entity.CreatedTime ?? model.CreatedTime;
            model.UserId = entity.UserId ?? model.UserId.Value;
            model.Remark = entity.Remark == null ? model.Remark : entity.Remark;
            return this._entityStore.Update(model);
        }


    }
}