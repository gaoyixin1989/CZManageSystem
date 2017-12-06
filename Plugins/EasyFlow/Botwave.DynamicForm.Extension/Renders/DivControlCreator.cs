using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using Botwave.Commons;
using Botwave.DynamicForm;
using Botwave.DynamicForm.Domain;

namespace Botwave.DynamicForm.Extension.Renders
{
    /// <summary>
    /// HTML控件生成类.
    /// </summary>
    public class DivControlCreator
    {
        /// <summary>
        /// 控件名称前缀.
        /// 表单项命名规则：统一前缀 + 类型前缀 + 名称，如 bwdf_txt_项目.
        /// </summary>
        public static readonly string CONTROL_PREFIX = "bwdf_";

        /// <summary>
        /// 文件上传的虚拟路径.
        /// </summary>
        public static readonly string UPLOADPATH = System.Configuration.ConfigurationSettings.AppSettings["UploadVirtualPath"];

        #region Create Table Item

        /// <summary>
        /// 创建表格架构.
        /// </summary>
        /// <returns></returns>
        public static HtmlGenericControl CreateTableSchema()
        {
            HtmlGenericControl tbBody = new HtmlGenericControl("div");
            tbBody.ID = "TB_FORM_DATA";
            tbBody.Attributes.Add("class" ,"dataTable form");
            return tbBody;
        }

        /// <summary>
        /// 创建表格列.
        /// </summary>
        /// <returns></returns>
        public static HtmlGenericControl CreateRow()
        {
            HtmlGenericControl tr = new HtmlGenericControl("div");
            tr.Attributes.Add("class", "row");
            return tr;
        }

        /// <summary>
        /// 创建表格单列.
        /// </summary>
        /// <returns></returns>
        public static HtmlGenericControl CreateSingleRow()
        {
            HtmlGenericControl tr = new HtmlGenericControl("div");
            tr.Attributes.Add("class", "row");
            return tr;
        }

        /// <summary>
        /// 创建表格双列.
        /// </summary>
        /// <returns></returns>
        public static HtmlGenericControl CreateDoubleRow()
        {
            HtmlGenericControl tr = new HtmlGenericControl("div");
            tr.Attributes.Add("class", "row");
            return tr;
        }

        /// <summary>
        /// 创建表格的标题单元格.
        /// </summary>
        /// <returns></returns>
        public static HtmlGenericControl CreateTitleCell()
        {
            return CreateTitleCell("&nbsp;");
        }

        /// <summary>
        /// 创建表格的标题单元格.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static HtmlGenericControl CreateTitleCell(string title)
        {
            HtmlGenericControl cell = new HtmlGenericControl("div"); // 换成 <th> 标签
            cell.Attributes.Add("class", "col-xs-12 col-md-4 col");
            Table table = new Table();
            table.Style.Value = "width: 100%; min-height:38px";
            TableRow tr = new TableRow();
            TableHeaderCell tcTitle = new TableHeaderCell(); // 换成 <th> 标签
            tcTitle.Text = title;
            tcTitle.Style.Value = "width: 6.5em;";
            tr.Cells.Add(tcTitle);
            table.Rows.Add(tr);
            cell.Controls.Add(table);
            return cell;
        }

        /// <summary>
        /// 创建表格的输入框单元格.
        /// </summary>
        /// <returns></returns>
        public static TableCell CreateInputCell()
        {
            return CreateInputCell(0);
        }

        /// <summary>
        /// 创建表格的输入框单元格.
        /// </summary>
        /// <param name="colspan"></param>
        /// <returns></returns>
        public static TableCell CreateInputCell(int colspan)
        {
            TableCell tcInput = new TableCell();
            tcInput.Text = "&nbsp;";

            if (colspan > 1)
            {
                tcInput.ColumnSpan = colspan;
            }
            else
            {
               // tcInput.Width = Unit.Percentage(20);
            }

            return tcInput;
        }
        #endregion

        #region RenderControls

        /// <summary>
        /// 呈现文本输入框(text) HTML.
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="item"></param>
        /// <param name="validate"></param>
        public static void RenderTextInput(StringWriter sw, FormItemDefinition item, FormItemDefinition.ValidateTemplate validate)
        {
            sw.Write(String.Format("<input type=\"text\" id=\"{0}\" name=\"{0}\" class=\"form-control\" ", CONTROL_PREFIX + FormItemDefinition.FormItemPrefix.Text + item.FName));
            sw.Write(String.Format("value=\"{0}\"", String.Format("$!tc.GetValue(&quot;{0}&quot;)", item.FName)));
            if (DbUtils.ToInt32(item.Width) > 0)
            {
                sw.Write(String.Format(" style=\"{0}\"", String.Format("width:{0};", DbUtils.ToInt32(item.Width))));
            }
            AppendValidateAttributes(sw, validate);
            sw.Write(" />");
        }

        /// <summary>
        /// 呈现多行文本输入框(textarea) HTML.
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="item"></param>
        /// <param name="validate"></param>
        public static void RenderTextAreaInput(StringWriter sw, FormItemDefinition item, FormItemDefinition.ValidateTemplate validate)
        {
            sw.Write(String.Format("<textarea id=\"{0}\" name=\"{0}\" class=\"form-control\" ", CONTROL_PREFIX + FormItemDefinition.FormItemPrefix.TextArea + item.FName));
            if (DbUtils.ToInt32(item.Height) > 0)
                sw.Write(String.Format("rows=\"{0}\" style=\"Height:{1}px;\" Height=\"{1}\" ", item.Height.ToString(), item.Height * 30));
            if (DbUtils.ToInt32(item.Width) > 0)
                sw.Write(String.Format("cols=\"{0}\"", item.Width.ToString()));
            AppendValidateAttributes(sw, validate);
            sw.Write(String.Format(" >$!tc.GetValue(&quot;{0}&quot;)</textarea>", item.FName));
        }

        /// <summary>
        /// 呈现日期输入框 HTML.
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="item"></param>
        /// <param name="validate"></param>
        public static void RenderDateInput(StringWriter sw, FormItemDefinition item, FormItemDefinition.ValidateTemplate validate)
        {
            sw.Write(String.Format("<input name=\"{0}\" id=\"{0}\" value=\"$!tc.GetValue(&quot;{1}&quot;)\" class=\"form-control form_datetime\" ", CONTROL_PREFIX + FormItemDefinition.FormItemPrefix.Date + item.FName, item.FName));
            sw.Write("type=\"text\" readonly ");
            AppendValidateAttributes(sw, validate);
            sw.Write(" />");
        }

        /// <summary>
        /// 呈现单选框或者复选框 HTML.
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="item"></param>
        /// <param name="validate"></param>
        /// <param name="isRadio"></param>
        public static void RenderRadioOrCheckBoxList(StringWriter sw, FormItemDefinition item, FormItemDefinition.ValidateTemplate validate, bool isRadio)
        {
            string defaultVal = DbUtils.ToString(item.DefaultValue);
            string strValue = DbUtils.ToString(item.DataSource);
            string[] strArrValue = strValue.Split(',');
            for (int i = 0; i < strArrValue.Length; i++)
            {
                strArrValue[i] = strArrValue[i].Trim();
            }

            for (int i = 0; i < strArrValue.Length; i++)
            {
                sw.Write("<input type=\"");
                if (isRadio)
                {
                    sw.Write(String.Format("radio\" id=\"{0}_{1}\" ", CONTROL_PREFIX + FormItemDefinition.FormItemPrefix.RadioButtonList + item.FName, i));
                    sw.Write(String.Format("name=\"{0}\" ", CONTROL_PREFIX + FormItemDefinition.FormItemPrefix.RadioButtonList + item.FName));
                }
                else
                {
                    sw.Write(String.Format("checkbox\" id=\"{0}_{1}\" ", CONTROL_PREFIX + FormItemDefinition.FormItemPrefix.CheckBoxList + item.FName, i));
                    sw.Write(String.Format("name=\"{0}\" ", CONTROL_PREFIX + FormItemDefinition.FormItemPrefix.CheckBoxList + item.FName));
                }
                sw.Write(String.Format("value=\"{0}\" ", strArrValue[i]));
                //sw.Write(String.Format("$!tc.GetFlag(\"{0}\",\"{1}\",\"checked\") ", item.FName, strArrValue[i]));
                if (defaultVal.Equals(strArrValue[i]))
                    sw.Write("checked ");

                if (i == strArrValue.Length - 1)
                    AppendValidateAttributes(sw, validate);

                sw.Write(" />");

                sw.Write("<span>");
                sw.Write(strArrValue[i]);
                sw.Write("</span>");
            }
        }

        /// <summary>
        /// 呈现下拉列表(select) HTML.
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="item"></param>
        /// <param name="validate"></param>
        public static void RenderSelect(StringWriter sw, FormItemDefinition item, FormItemDefinition.ValidateTemplate validate)
        {
            string defaultVal = DbUtils.ToString(item.DefaultValue);
            string strValue = DbUtils.ToString(item.DataSource);
            string[] strArrValue = strValue.Split(',');
            for (int i = 0; i < strArrValue.Length; i++)
            {
                strArrValue[i] = strArrValue[i].Trim();
            }

            sw.Write(String.Format("<select id=\"{0}\" name=\"{0}\" class=\"{1}\"", CONTROL_PREFIX + FormItemDefinition.FormItemPrefix.DropDownList + item.FName, "form-control"));

            AppendValidateAttributes(sw, validate);

            sw.Write(" >");

            for (int i = 0; i < strArrValue.Length; i++)
            {
                sw.Write("<option ");
                sw.Write(String.Format("value=\"{0}\"", strArrValue[i]));
                //sw.Write(String.Format(" $!tc.GetFlag(\"{0}\",\"{1}\",\"selected\")", item.FName, strArrValue[i]));
                if (defaultVal.Equals(strArrValue[i]))
                    sw.Write("selected ");
                sw.Write(">");
                sw.Write(strArrValue[i]);
                sw.Write("</option>");
            }
            sw.Write("</select>");
        }

        /// <summary>
        /// 呈现隐藏字段(hidden) HTML.
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="item"></param>
        public static void RenderHiddenInput(StringWriter sw, FormItemDefinition item)
        {
            sw.Write("<input type=\"hidden\"");
            sw.Write(String.Format(" name=\"{0}\" id=\"{0}\"", CONTROL_PREFIX + FormItemDefinition.FormItemPrefix.Hidden + item.FName));
            sw.Write(" value=\"");
            sw.Write(String.Format("$!tc.GetValue(&quot;{0}&quot;)", item.FName));
            sw.Write("\"");
            sw.Write(" />");
        }

        /// <summary>
        /// 呈现文件上传输入框 HTML.
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="item"></param>
        /// <param name="validate"></param>
        public static void RenderFileInput(StringWriter sw, FormItemDefinition item, FormItemDefinition.ValidateTemplate validate)
        {
            sw.Write(String.Format("#if(!$!tc.GetValue(&quot;{0}&quot;) || $!tc.GetValue(&quot;{0}&quot;) == \"\")", item.FName));
            sw.Write(String.Format("<input type=\"file\" id=\"{0}\" name=\"{0}\"", CONTROL_PREFIX + FormItemDefinition.FormItemPrefix.File + item.FName));
            AppendValidateAttributes(sw, validate);
            sw.Write(" />");
            sw.Write("#else");
            sw.Write(string.Format("<a href='{0}$!tc.GetValue(&quot;{1}&quot;)' class='ico_download' title='下载{2}' target='_blank'>{2}</a>", UPLOADPATH, item.FName, item.Name));
            sw.Write("#end");
        }

        /// <summary>
        /// 呈现块状文本区(span) HTML.
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="item"></param>
        public static void RenderSpan(StringWriter sw, FormItemDefinition item)
        {
            sw.Write("<span id=\"");
            sw.Write(CONTROL_PREFIX + FormItemDefinition.FormItemPrefix.Label + item.FName);
            sw.Write("'\">");
            sw.Write(String.Format("$!tc.GetValue(&quot;{0}&quot;)", item.FName));
            sw.Write("</span>");
        }

        /// <summary>
        /// 呈现 HTML 容器.
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="item"></param>
        public static void RenderHTMLContainer(StringWriter sw, FormItemDefinition item)
        {
            sw.Write("<div id=\"divHTMLContainer_" + item.FName + "\" class=\"dataTable table-responsive\">");
            sw.Write(String.Format("$!tc.GetValue(&quot;{0}&quot;)", item.FName));
            sw.Write(String.Format("<input type=\"hidden\" value=\"\" id=\"{0}\" name=\"{0}\">", CONTROL_PREFIX + FormItemDefinition.FormItemPrefix.Html + item.FName));
            sw.Write("</div>");
        }

        /// <summary>
        /// 呈现表单定义的备注信息.
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="item"></param>
        public static void RenderComment(StringWriter sw, FormItemDefinition item)
        {
            sw.Write("(");
            sw.Write(item.Comment);
            sw.Write(")");
        }

        /// <summary>
        /// 呈现必须填写的提示星号(*).
        /// </summary>
        /// <param name="sw"></param>
        public static void RenderRequireHint(StringWriter sw)
        {
            sw.Write("<span class=\"require\">*</span>");
        }
        #endregion

        #region AppendValidateAttributes
        /// <summary>
        /// 附加控件验证逻辑.
        /// </summary>        
        private static void AppendValidateAttributes(StringWriter sw, FormItemDefinition.ValidateTemplate validate)
        {
            if (validate.Require)
            {
                sw.Write(" require='true'");
            }
            else sw.Write(" require='false'");

            if (validate.ValidateType.Length > 0)
            {
                sw.Write(" ValidateType='" + validate.ValidateType + "'");
            }

            if (validate.Operator.Length > 0)
            {
                sw.Write(" Operator='" + validate.Operator + "'");
            }

            if (validate.Target.Length > 0)
            {
                sw.Write(" to='" + validate.Target + "'");
            }

            if (validate.MaxVal.Length > 0)
            {
                sw.Write(" max='" + validate.MaxVal + "'");
            }

            if (validate.MinVal.Length > 0)
            {
                sw.Write(" min='" + validate.MinVal + "'");
            }

            if (validate.ErrorMessage.Length > 0)
            {
                sw.Write(" msg='" + validate.ErrorMessage + "'");
            }
        }
        
        #endregion
    }
}
