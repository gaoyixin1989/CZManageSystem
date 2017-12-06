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
    public partial class DataDictionaryService : BaseService<DataDictionary>, IDataDictionaryService
    {
        public override bool Insert(DataDictionary entity)
        {
            if (entity == null)
                return false;
            entity.DDId = (entity.DDId == Guid.Parse("00000000-0000-0000-0000-000000000000")) ? Guid.NewGuid() : entity.DDId;
            entity.EnableFlag = entity.EnableFlag ?? false;
            entity.DefaultFlag = entity.DefaultFlag ?? false;
            entity.OrderNo = entity.OrderNo == null ? 99999 : entity.OrderNo;
            entity.Createdtime = entity.Createdtime ?? DateTime.Now;
            return this._entityStore.Insert(entity);
        }
        public override bool Update(DataDictionary entity)
        {
            if (entity == null)
                return false;
            var model = FindById(entity.DDId);
            model.DDName = entity.DDName == null ? model.DDName : entity.DDName;
            model.DDValue = entity.DDValue == null ? model.DDValue : entity.DDValue;
            model.DDText = entity.DDText == null ? model.DDText : entity.DDText;
            model.ValueType = entity.ValueType == null ? model.ValueType : entity.ValueType;
            model.EnableFlag = entity.EnableFlag ?? model.EnableFlag;
            model.DefaultFlag = entity.DefaultFlag ?? model.DefaultFlag;
            model.OrderNo = entity.OrderNo ?? model.OrderNo;
            model.Createdtime = entity.Createdtime ?? model.Createdtime;
            model.Creator = entity.Creator == null ? model.Creator : entity.Creator;
            model.LastModTime = entity.LastModTime ?? DateTime.Now;
            model.LastModifier = entity.LastModifier == null ? model.LastModifier : entity.LastModifier;
            return this._entityStore.Update(model);
        }

        /// <summary>
        /// 获取字典名称分组后的名称列表
        /// </summary>
        /// <returns></returns>
        public IList<string> GetDictNameGroup()
        {
            return this._entityStore.Table.GroupBy(u => u.DDName).Select(u => u.FirstOrDefault().DDName).OrderBy(u => u).ToList();
        }

        public IList<DataDictionary> QueryDataByPage(out int count, int pageIndex = 0, int pageSize = int.MaxValue, string DDName = null, string searchDDName = null)
        {
            var curData = this._entityStore.Table;
            if (!string.IsNullOrEmpty(DDName))
                curData = curData.Where(u => u.DDName == DDName);
            if (!string.IsNullOrEmpty(searchDDName))
                curData = curData.Where(u => u.DDName.Contains(searchDDName));
            return new PagedList<DataDictionary>(curData.OrderBy(c => c.DDName).ThenBy(c => c.OrderNo), pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count);

        }

    }
}