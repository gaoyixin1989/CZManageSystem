using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using Botwave.Commons;
using Botwave.DynamicForm;
using Botwave.DynamicForm.Domain;
using Botwave.DynamicForm.Services;

namespace Botwave.DynamicForm.Renders
{
    /// <summary>
    /// 表单的表格呈现类.
    /// </summary>
    public class TableRender
    {
        /// <summary>
        /// 呈现指定表单定义到 HtmlTextWriter 输出流中.
        /// </summary>
        /// <param name="htw"></param>
        /// <param name="form"></param>
        /// <param name="appendRowNumber"></param>
        public static void Render(HtmlTextWriter htw, FormDefinition form, bool appendRowNumber)
        {
            Table tbForm = RenderForm(form.Items);

            AppendRowNumber(tbForm, appendRowNumber);

            tbForm.RenderControl(htw);
        }

        #region RenderForm

        /// <summary>
        /// 呈现指定表单项为表格.
        /// </summary>
        /// <param name="itemlist"></param>
        /// <returns></returns>
        private static Table RenderForm(IList<FormItemDefinition> itemlist)
        {
            TableRenderStrategy.Init();

            Table tbForm = ControlCreator.CreateTableSchema();

            foreach (FormItemDefinition item in itemlist)
            {
                FormItemDefinition.ValidateTemplate validate = new FormItemDefinition.ValidateTemplate();
                validate.Require = DbUtils.ToBoolean(item.Require);
                validate.ValidateType = DbUtils.ToString(item.ValidateType);
                validate.Operator = DbUtils.ToString(item.Op);
                validate.Target = DbUtils.ToString(item.OpTarget);
                validate.MaxVal = DbUtils.ToString(item.MaxVal);
                validate.MinVal = DbUtils.ToString(item.MinVal);
                validate.ErrorMessage = DbUtils.ToString(item.ErrorMessage);

                if (!String.IsNullOrEmpty(item.ShowSet))
                    item.RowExclusive = true;

                TableCell tcItem;
                TableRow tr;

                try
                {
                    TableRenderStrategy.AppendItemCell(item, tbForm, out tr, out tcItem);
                }
                catch (ArgumentException ex)
                {
                    throw new FormItemRenderException(item, ex);
                }
                catch (Exception ex)
                {
                    throw new FormItemRenderException(item, ex, true);
                }

                AppendInputControl(item, validate, ref tcItem);

                if (!String.IsNullOrEmpty(item.ShowSet))
                    tr.ID = "tr_" + item.FName + "_" + item.ShowSet;
            }

            return tbForm;
        }

        #region AppendInputControl

        /// <summary>
        /// 附加输入控件.
        /// </summary>
        private static void AppendInputControl(FormItemDefinition item, FormItemDefinition.ValidateTemplate validate, ref TableCell tcItem)
        {
            StringWriter swInput = new StringWriter();
            //HtmlTextWriter htw = new HtmlTextWriter(swInput);
            switch (item.ItemType)
            {
                case FormItemDefinition.FormItemType.Text:		//单行输入
                    ControlCreator.RenderTextInput(swInput, item, validate);
                    break;
                case FormItemDefinition.FormItemType.TextArea:	//多行输入
                    ControlCreator.RenderTextAreaInput(swInput, item, validate);
                    break;
                case FormItemDefinition.FormItemType.DropDownList:		//下拉框


                    ControlCreator.RenderSelect(swInput, item, validate);
                    break;
                case FormItemDefinition.FormItemType.Label:	//标签显示
                    ControlCreator.RenderSpan(swInput, item);
                    break;
                case FormItemDefinition.FormItemType.CheckBoxList:	//多选框
                    ControlCreator.RenderRadioOrCheckBoxList(swInput, item, validate, false);
                    break;
                case FormItemDefinition.FormItemType.RadioButtonList:		//单选框
                    ControlCreator.RenderRadioOrCheckBoxList(swInput, item, validate, true);
                    break;
                case FormItemDefinition.FormItemType.Date:		//日期输入
                    ControlCreator.RenderDateInput(swInput, item, validate);
                    break;
                case FormItemDefinition.FormItemType.File:		//文件上传
                    ControlCreator.RenderFileInput(swInput, item, validate);
                    break;
                case FormItemDefinition.FormItemType.IncrementTextArea:     //自增多行输入
                    break;
                case FormItemDefinition.FormItemType.Html:      //复杂自定义HTML
                    ControlCreator.RenderHTMLContainer(swInput, item);
                    break;
                case FormItemDefinition.FormItemType.Hidden:      //隐藏
                    ControlCreator.RenderHiddenInput(swInput, item);
                    break;
                default:
                    break;
            }

            if (validate.Require)
                ControlCreator.RenderRequireHint(swInput);

            if (DbUtils.ToString(item.Comment).Length > 0)
                ControlCreator.RenderComment(swInput, item);

            tcItem.Text = swInput.ToString();
        }

        ////将对应的控件附加到单元格
        //private static void AppendControlToCell(TableCell tcItem, params Control[] arrCtl)
        //{
        //    if (null == tcItem || null == arrCtl) return;

        //    foreach (Control ctl in arrCtl)
        //    {
        //        if (null != ctl)
        //        {
        //            tcItem.Controls.Add(ctl);
        //        }
        //    }
        //}

        /// <summary>
        /// 附加行号.
        /// </summary>
        /// <param name="tbForm"></param>
        /// <param name="appendRowNumber"></param>
        private static void AppendRowNumber(Table tbForm, bool appendRowNumber)
        {
            int startIndex = 1;
            int width = 4;

            for (int i = 0; i < tbForm.Rows.Count; i++)
            {
                if (appendRowNumber)
                {
                    TableCell tc = new TableCell(); 
                    tc.HorizontalAlign = HorizontalAlign.Right;
                    tc.Width = Unit.Percentage(width);
                    tc.Controls.Add(new LiteralControl(Convert.ToString(i + startIndex)));
                    tbForm.Rows[i].Cells.AddAt(0, tc);
                }

            }
        }
        
        #endregion

        #endregion
    }
}