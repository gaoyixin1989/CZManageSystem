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
    public partial class SysNoticeService : BaseService<SysNotice>, ISysNoticeService
    {
        public override bool Insert(SysNotice entity)
        {
            if (entity == null)
                return false;
            entity.ValidTime = entity.ValidTime ?? DateTime.Now;
            entity.EnableFlag = entity.EnableFlag ?? false;
            entity.OrderNo = entity.OrderNo == null ? 0 : entity.OrderNo;
            entity.Createdtime = entity.Createdtime ?? DateTime.Now;
            return this._entityStore.Insert(entity);
        }
        public override bool Update(SysNotice entity)
        {
            if (entity == null)
                return false;
            var model = FindById(entity.NoticeId);
            model.Title = entity.Title == null ? model.Title : entity.Title;
            model.Content = entity.Content == null ? model.Content : entity.Content;
            model.ValidTime = entity.ValidTime ?? model.ValidTime.Value;
            model.EnableFlag = entity.EnableFlag ?? model.EnableFlag.Value;
            model.OrderNo = entity.OrderNo ?? model.OrderNo.Value;
            model.Createdtime = entity.Createdtime == null ? model.Createdtime : entity.Createdtime;
            model.Creator = entity.Creator == null ? model.Creator : entity.Creator;
            return this._entityStore.Update(model);
        }

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
            //var curTable = GetQueryTable(objs);
            var curTable = this._entityStore.Table;
            if (objs != null)
            {
                var exp = ExpressionFactory(objs);
                curTable = curTable.Where(exp);
            }

            return new PagedList<SysNotice>(curTable.OrderByDescending(c=>c.OrderNo).ThenByDescending(c => c.Createdtime), pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count);
        }

        /// <summary>
        /// 编辑查询条件
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        public IQueryable<SysNotice> GetQueryTable(object objs = null)
        {
            var curTable = this._entityStore.Table;
            DateTime dtTemp = new DateTime();
            if (objs != null)
            {
                foreach (var p in objs.GetType().GetProperties())
                {
                    object obj = p.GetValue(objs);
                    if (obj != null && obj.ToString() != "")
                    {
                        if (p.Name == "Title")
                            curTable = curTable.Where(u => u.Title.Contains(obj.ToString()));
                        if (p.Name == "Createdtime_Start" && DateTime.TryParse(obj.ToString(), out dtTemp))
                        {
                            curTable = curTable.Where(u => u.Createdtime != null && u.Createdtime.Value >= dtTemp);
                        }
                        if (p.Name == "Createdtime_End" && DateTime.TryParse(obj.ToString(), out dtTemp))
                        {
                            curTable = curTable.Where(u => u.Createdtime != null && u.Createdtime.Value <= dtTemp);
                        }
                    }
                }
            }
            return curTable;
        }

    }
}