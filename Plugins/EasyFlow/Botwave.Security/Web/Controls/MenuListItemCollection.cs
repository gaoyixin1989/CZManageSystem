using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Botwave.Security.Web.Controls
{
    /// <summary>
    /// 菜单列表项控件集合.
    /// </summary>
    public class MenuListItemCollection : ControlCollection
    {
        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="owner"></param>
        public MenuListItemCollection(Control owner)
            : base(owner)
        { }

        /// <summary>
        /// 新增列表项.
        /// </summary>
        /// <param name="item"></param>
        public override void Add(Control item)
        {
            if (item is MenuListItem)
            {
                base.Add(item);
            }
        }

        /// <summary>
        /// 新增列表项到知道索引位置.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public override void AddAt(int index, Control item)
        {
            if (item is MenuListItem)
            {
                base.AddAt(index, item);
            }
        }

        /// <summary>
        /// 获取指定索引位置的列表项.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public new MenuListItem this[int i]
        {
            get
            {
                return (MenuListItem)base[i];
            }
        }
    }
}
