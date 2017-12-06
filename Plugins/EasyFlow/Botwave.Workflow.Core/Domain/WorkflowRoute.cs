using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Botwave.Workflow.Domain
{
    /// <summary>
    /// 流程分支路由.
    /// </summary>
    public class WorkflowRoute : NameObjectCollectionBase
    {
        private string[] _allKeys;
        private ActivityDefinition[] _all;

        /// <summary>
        /// 构造方法.
        /// </summary>
        public WorkflowRoute()
            : base(StringComparer.OrdinalIgnoreCase)
        { }

        /// <summary>
        /// 所有索引键名(步骤名称).
        /// </summary>
        public string[] AllKeys
        {
            get
            {
                if (this._allKeys == null)
                {
                    this._allKeys = base.BaseGetAllKeys();
                }
                return this._allKeys;
            }
        }

        /// <summary>
        /// 根据步骤索引进行索引.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ActivityDefinition this[int index]
        {
            get
            {
                return this.Get(index);
            }
        }

        /// <summary>
        /// 根据步骤名称进行索引.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActivityDefinition this[string name]
        {
            get
            {
                return this.Get(name);
            }
        }

        /// <summary>
        /// 添加步骤.
        /// </summary>
        /// <param name="activity"></param>
        public void Add(ActivityDefinition activity)
        {
            this.Add(activity, true);
        }

        /// <summary>
        /// 添加步骤.
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="append"></param>
        public void Add(ActivityDefinition activity, bool append)
        {
            if (append)
                base.BaseAdd(activity.ActivityName, activity);
            else
                base.BaseSet(activity.ActivityName, activity);
        }

        /// <summary>
        /// 设置步骤.
        /// </summary>
        /// <param name="activity"></param>
        public void Set(ActivityDefinition activity)
        {
            this.Add(activity, false);
        }

        /// <summary>
        /// 清除.
        /// </summary>
        public void Clear()
        {
            this._allKeys = null;
            this._all = null;
            this.BaseClear();
        }

        /// <summary>
        /// 获取指定索引位置的步骤.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ActivityDefinition Get(int index)
        {
            return (ActivityDefinition)base.BaseGet(index);
        }

        /// <summary>
        /// 获取指定步骤名称（键值）的步骤.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActivityDefinition Get(string name)
        {
            return (ActivityDefinition)base.BaseGet(name);
        }

        /// <summary>
        /// 删除指定步骤名称（键值）的步骤.
        /// </summary>
        /// <param name="name"></param>
        public void Remove(string name)
        {
            base.BaseRemove(name);
        }

        /// <summary>
        /// 删除指定索引位置的步骤.
        /// </summary>
        /// <param name="index"></param>
        public void Remove(int index)
        {
            base.BaseRemoveAt(index);
        }

        /// <summary>
        /// 将指定位置的步骤赋值到数组中.
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="index"></param>
        public void CopyTo(Array dest, int index)
        {
            if (this._all == null)
            {
                int count = this.Count;
                this._all = new ActivityDefinition[count];
                for (int i = 0; i < count; i++)
                {
                    this._all[i] = this.Get(i);
                }
            }
            this._all.CopyTo(dest, index);
        }

        /// <summary>
        /// 取得指定步骤列表索引位置和指定索引长度的子路由.
        /// </summary>
        /// <param name="startIndex">子流程路由的起始位置（即步骤列表的索引序号）.</param>
        /// <param name="length">子流程路由的取得的步骤列表长度（即步骤数）.</param>
        /// <returns></returns>
        public WorkflowRoute SubRoute(int startIndex, int length)
        {
            WorkflowRoute route = new WorkflowRoute();
            int count = length > this.Count ? this.Count : length;
            for (int i = 0; i < count; i++)
            {
                route.Add(this.Get(i));
            }
            return route;
        }

        /// <summary>
        /// 以箭头("->")符号表示流程步骤的执行顺序.并将路由中全部步骤以顺序显示出来.
        /// </summary>
        /// <returns></returns>
        public string ToRouteString()
        {
            int count = this.Count;
            if (count == 0)
                return string.Empty;
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                if (i == count - 1)
                    builder.Append(this.AllKeys[i]);
                else
                    builder.AppendFormat("{0} -> ", this.AllKeys[i]);
            }
            return builder.ToString();
        }

    }
}
