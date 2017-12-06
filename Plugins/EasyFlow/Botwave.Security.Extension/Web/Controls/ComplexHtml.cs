using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Botwave.Security.Extension.Web.Controls
{
    /// <summary>
    /// 复杂 HTML 控件类.
    /// </summary>
    public class ComplexHtml
    {
        #region checkbox table

        /// <summary>
        /// HTML 复选框表格类.
        /// </summary>
        public class CheckBoxTable : HtmlBuilderBase
        {
            #region properties

            private int _repeatColumns;
            private string _itemName;
            private string _itemIdPrefix;
            private string _itemCssClass;
            private string _itemSelectedCssClass;
            private string _emptyCell = "<td></td>";
            private IList<CheckBoxGroup> _groups;

            /// <summary>
            /// 重复列数.
            /// </summary>
            public int RepeatColumns
            {
                get { return _repeatColumns; }
                set { _repeatColumns = value; }
            }

            /// <summary>
            /// 复选框项的名称(Name).
            /// </summary>
            public string ItemName
            {
                get { return _itemName; }
                set { _itemName = value; }
            }

            /// <summary>
            /// 复选框项 ID 的前缀.
            /// </summary>
            public string ItemIdPrefix
            {
                get { return _itemIdPrefix; }
                set { _itemIdPrefix = value; }
            }

            /// <summary>
            /// 复选框项的样式类.
            /// </summary>
            public string ItemCssClass
            {
                get { return _itemCssClass; }
                set { _itemCssClass = value; }
            }

            /// <summary>
            /// 复选框被选中的样式类.
            /// </summary>
            public string ItemSelectedCssClass
            {
                get { return _itemSelectedCssClass; }
                set { _itemSelectedCssClass = value; }
            }

            /// <summary>
            /// 空单元格字显示字符串.
            /// </summary>
            public string EmptyCell
            {
                get { return _emptyCell; }
                set { _emptyCell = value; }
            }

            /// <summary>
            /// 复选框组列表.
            /// </summary>
            public IList<CheckBoxGroup> Groups
            {
                get { return _groups; }
                set { _groups = value; }
            }

            #endregion

            #region construtor

            /// <summary>
            /// 构造方法.
            /// </summary>
            public CheckBoxTable()
                : base()
            {
                this._itemCssClass = string.Empty;
                this._itemSelectedCssClass = string.Empty;

                this._groups = new List<CheckBoxGroup>();
            }

            #endregion

            /// <summary>
            /// 生成.
            /// </summary>
            /// <returns></returns>
            public override string Build()
            {
                if (this._groups == null || this._groups.Count == 0)
                    return string.Empty;
                StringBuilder builder = new StringBuilder();
                StringWriter writer = new StringWriter(builder);

                if (string.IsNullOrEmpty(this.Id))
                    this.Id = "chkbox";
                if (string.IsNullOrEmpty(this.Name))
                    this.Name = "chkbox";
                if (string.IsNullOrEmpty(this._itemName))
                    this._itemName = this.Name + "_item";
                if (string.IsNullOrEmpty(this._itemIdPrefix))
                    this._itemIdPrefix = this.Id + "_item";

                string groupId = this.Id + "_group_";
                string groupName = this.Name + "_group";
                string rowId = this.Id + "_row_";
                string rowName = this.Name + "_row";

                writer.WriteLine(string.Format("<table width=\"100%\"{0}>", BuildAttributes(this.Attributes)));

                int index = 1;
                foreach (CheckBoxGroup group in this._groups)
                {
                    group.GroupIndex = index;
                    if (string.IsNullOrEmpty(group.Id))
                        group.Id = groupId + index;
                    if (string.IsNullOrEmpty(group.Name))
                        group.Name = groupName;

                    if (string.IsNullOrEmpty(group.RowIdPrefix))
                        group.RowIdPrefix = rowId + index;
                    if (string.IsNullOrEmpty(group.RowName))
                        group.RowName = rowName;

                    group.BuildTable(writer, this._repeatColumns, this._itemName, this._itemIdPrefix, this._itemCssClass, this._itemSelectedCssClass);
                    index++;
                }

                writer.WriteLine("</table>");

                return writer.ToString();
            }
        }
        #endregion

        #region CheckBox Group

        /// <summary>
        ///  HTML 复选框控件组.
        /// </summary>
        [Serializable]
        public class CheckBoxGroup : HtmlBuilderBase
        {
            #region properties

            private int _groupIndex;
            private string _rowName;
            private string _rowIdPrefix;
            private bool _rowVisible = true;
            private IList<CheckBox> _items;

            /// <summary>
            /// 组索引.
            /// </summary>
            public int GroupIndex
            {
                get { return _groupIndex; }
                set { _groupIndex = value; }
            }

            /// <summary>
            /// 行("tr")名称.
            /// </summary>
            internal string RowName
            {
                get { return _rowName; }
                set { _rowName = value; }
            }

            /// <summary>
            /// 行("tr") ID 的前缀.
            /// </summary>
            internal string RowIdPrefix
            {
                get { return _rowIdPrefix; }
                set { _rowIdPrefix = value; }
            }

            /// <summary>
            /// 行("tr")的可视性.
            /// </summary>
            public bool RowVisible
            {
                get { return _rowVisible; }
                set { _rowVisible = value; }
            }

            /// <summary>
            /// 复选框集合.
            /// </summary>
            public IList<CheckBox> Items
            {
                get { return _items; }
                set { _items = value; }
            }
            #endregion

            #region construtor

            /// <summary>
            /// 构造方法.
            /// </summary>
            public CheckBoxGroup()
                : base()
            {
                this._items = new List<CheckBox>();
            }

            /// <summary>
            /// 构造方法.
            /// </summary>
            /// <param name="text"></param>
            public CheckBoxGroup(string text)
                : this(text, string.Empty)
            {
                this._items = new List<CheckBox>();
            }

            /// <summary>
            /// 构造方法.
            /// </summary>
            /// <param name="text"></param>
            /// <param name="value"></param>
            public CheckBoxGroup(string text, string value)
                : base(text, value)
            {
                this._items = new List<CheckBox>();
            }

            #endregion

            /// <summary>
            /// 绑定为表格形式.
            /// </summary>
            /// <param name="writer"></param>
            /// <param name="repeateColumns"></param>
            /// <param name="itemName"></param>
            /// <param name="itemIdPrefix"></param>
            /// <param name="itemCssClass"></param>
            /// <param name="itemSelectedCssClass"></param>
            public void BuildTable(TextWriter writer, int repeateColumns,
                string itemName, string itemIdPrefix,
                string itemCssClass, string itemSelectedCssClass)
            {
                this.BuildTable(writer, repeateColumns, itemName, itemIdPrefix, itemCssClass, itemSelectedCssClass, "<td></td>");
            }

            /// <summary>
            /// 绑定为表格形式.
            /// </summary>
            /// <param name="writer"></param>
            /// <param name="repeateColumns"></param>
            /// <param name="itemName"></param>
            /// <param name="itemIdPrefix"></param>
            /// <param name="itemCssClass"></param>
            /// <param name="itemSelectedCssClass"></param>
            /// <param name="emptyCell"></param>
            public void BuildTable(TextWriter writer, int repeateColumns,
                string itemName, string itemIdPrefix,
                string itemCssClass, string itemSelectedCssClass,
                string emptyCell)
            {
                if (this._items == null || this._items.Count == 0)
                    return;

                writer.WriteLine(string.Format("<tr><td colspan=\"{1}\">{0}</td></tr>", this.GetGroupText(), repeateColumns));
                int count = this._items.Count;
                int rowIndex = 0;

                string visibilityAttribute = this.RowVisible ? string.Empty : " style=\"display:none\"";

                for (int index = 0; index < count; index++)
                {
                    string itemId = string.Format("{0}_{1}_{2}", itemIdPrefix, this._groupIndex, index);

                    CheckBox item = this._items[index];
                    item.Id = itemId;
                    item.Name = itemName;
                    if (string.IsNullOrEmpty(item.CssClass))
                    {
                        item.CssClass = item.Checked ? itemSelectedCssClass : itemCssClass;
                    }
                    int columnIndex = index % repeateColumns;
                    if (columnIndex == 0)
                    {
                        string rowId = this.RowIdPrefix + "_" + rowIndex;
                        writer.WriteLine("<tr id=\"{0}\" name=\"{1}\" parentid=\"{2}\"{3}>", rowId, this.RowName, this.Id, visibilityAttribute);
                        rowIndex++;
                    }

                    writer.WriteLine(string.Format("\t<td>{0}</td>", item.Build()));

                    if (columnIndex == repeateColumns - 1)
                    {
                        writer.WriteLine("</tr>");
                    }
                }
                // 输出空位置，凑足 repeateColumns.
                int modValue = count % repeateColumns;
                if (modValue > 0)
                {
                    if (string.IsNullOrEmpty(emptyCell))
                        emptyCell = "<td></td>";
                    for (; modValue < repeateColumns; modValue++)
                    {
                        writer.Write(emptyCell);
                    }
                    writer.WriteLine("</tr>");
                }
            }

            /// <summary>
            /// 获取组文本.
            /// </summary>
            /// <returns></returns>
            public string GetGroupText()
            {
                if (string.IsNullOrEmpty(this.Text))
                    return string.Empty;
                StringBuilder builder = new StringBuilder();
                builder.Append("<div width=\"100%\"");
                if (!string.IsNullOrEmpty(this.Id))
                    builder.AppendFormat(" id=\"{0}\"", this.Id);
                if (!string.IsNullOrEmpty(this.Name))
                    builder.AppendFormat(" name=\"{0}\"", this.Name);
                if (!string.IsNullOrEmpty(this.CssClass))
                    builder.AppendFormat(" class=\"{0}\"", this.CssClass);
                builder.Append(BuildAttributes(this.Attributes));
                builder.AppendFormat(">{0}</div>", this.Text);
                return builder.ToString();
            }
        }

        #endregion

        #region CheckBox

        /// <summary>
        /// HTML 复选框控件.
        /// </summary>
        [Serializable]
        public class CheckBox : HtmlBuilderBase
        {
            #region properties

            private int _index;
            private bool _checked;

            /// <summary>
            /// 索引.
            /// </summary>
            public int Index
            {
                get { return _index; }
                set { _index = value; }
            }

            /// <summary>
            /// 是否被选中.
            /// </summary>
            public bool Checked
            {
                get { return _checked; }
                set { _checked = value; }
            }

            #endregion

            /// <summary>
            /// 构造方法.
            /// </summary>
            public CheckBox()
                : base()
            {
                this._checked = false;
            }

            /// <summary>
            /// 构造方法.
            /// </summary>
            /// <param name="text"></param>
            /// <param name="value"></param>
            public CheckBox(string text, string value)
                : base(text, value)
            { }

            /// <summary>
            /// 构造方法.
            /// </summary>
            /// <param name="id"></param>
            /// <param name="name"></param>
            /// <param name="text"></param>
            /// <param name="value"></param>
            public CheckBox(string id, string name, string text, string value)
                : base(id, name, text, value)
            { }

            /// <summary>
            /// 生成控件 HTML.
            /// </summary>
            /// <returns></returns>
            public override string Build()
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("<input type=\"checkbox\"");
                if (!string.IsNullOrEmpty(Id))
                    builder.AppendFormat(" id=\"{0}\"", Id);
                if (!string.IsNullOrEmpty(Name))
                    builder.AppendFormat(" name=\"{0}\"", Name);
                if (this._checked)
                    builder.Append(" checked=\"checked\"");
                if (!string.IsNullOrEmpty(Value))
                    builder.AppendFormat(" value=\"{0}\"", Value);
                if (!string.IsNullOrEmpty(CssClass))
                    builder.AppendFormat(" class=\"{0}\"", CssClass);
                builder.Append(BuildAttributes(this.Attributes) + " />");
                if (!string.IsNullOrEmpty(this.Text))
                {
                    if (!string.IsNullOrEmpty(this.Id))
                        builder.AppendFormat("<label for=\"{0}\">{1}</label>", this.Id, this.Text);
                    else
                        builder.Append(this.Text);
                }
                return builder.ToString();
            }
        }

        #endregion

        #region builder Base

        /// <summary>
        /// HTML 元素生成基础类.
        /// </summary>
        [Serializable]
        public class HtmlBuilderBase
        {
            #region properties

            private string _id;
            private string _name;
            private string _text;
            private string _value;
            private string _cssClass;
            private NameValueCollection _attributes;

            /// <summary>
            /// 控件 ID.
            /// </summary>
            public string Id
            {
                get { return _id; }
                set { _id = value; }
            }

            /// <summary>
            /// 控件名称.
            /// </summary>
            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }

            /// <summary>
            /// 显示文本.
            /// </summary>
            public string Text
            {
                get { return _text; }
                set { _text = value; }
            }

            /// <summary>
            /// 值.
            /// </summary>
            public string Value
            {
                get { return _value; }
                set { _value = value; }
            }

            /// <summary>
            /// 样式类.
            /// </summary>
            public string CssClass
            {
                get { return _cssClass; }
                set { _cssClass = value; }
            }

            /// <summary>
            /// 属性集合.
            /// </summary>
            public NameValueCollection Attributes
            {
                get { return _attributes; }
                set { _attributes = value; }
            }

            #endregion

            #region constructor

            /// <summary>
            /// 构造方法.
            /// </summary>
            public HtmlBuilderBase()
            {
                this._attributes = new NameValueCollection();
            }

            /// <summary>
            /// 构造方法.
            /// </summary>
            /// <param name="text"></param>
            /// <param name="value"></param>
            public HtmlBuilderBase(string text, string value)
                : this()
            {
                this._text = text;
                this._value = value;
            }

            /// <summary>
            /// 构造方法.
            /// </summary>
            /// <param name="id"></param>
            /// <param name="name"></param>
            /// <param name="text"></param>
            /// <param name="value"></param>
            public HtmlBuilderBase(string id, string name, string text, string value)
                : this()
            {
                this._id = id;
                this._name = name;
                this._text = text;
                this._value = value;
            }
            #endregion

            /// <summary>
            /// 生成 HTML.
            /// </summary>
            /// <returns></returns>
            public virtual string Build()
            {
                return string.Empty;
            }

            /// <summary>
            /// 生成属性字符串.
            /// </summary>
            /// <returns></returns>
            public static string BuildAttributes(NameValueCollection attributes)
            {
                if (attributes == null || attributes.Count == 0)
                    return string.Empty;
                StringBuilder builder = new StringBuilder();
                foreach (string key in attributes.AllKeys)
                {
                    builder.AppendFormat(" {0}=\"{1}\"", key, attributes[key]);
                }
                return builder.ToString();
            }
        }
        #endregion
    }
}
