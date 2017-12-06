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
using Botwave.DynamicForm.Renders;

namespace Botwave.DynamicForm.Extension.Renders
{
    /// <summary>
    /// 表单的表格呈现类.
    /// </summary>
    public class DivRender
    {
        /// <summary>
        /// 呈现指定表单定义到 HtmlTextWriter 输出流中.
        /// </summary>
        /// <param name="htw"></param>
        /// <param name="form"></param>
        /// <param name="appendRowNumber"></param>
        public static void Render(HtmlTextWriter htw, FormDefinition form, bool appendRowNumber)
        {
            HtmlGenericControl tbForm = RenderForm(form.Items);

            //AppendRowNumber(tbForm, appendRowNumber);

            tbForm.RenderControl(htw);
        }

        #region RenderForm

        /// <summary>
        /// 呈现指定表单项为表格.
        /// </summary>
        /// <param name="itemlist"></param>
        /// <returns></returns>
        private static HtmlGenericControl RenderForm(IList<FormItemDefinition> itemlist)
        {
            DivRenderStrategy.Init();

            HtmlGenericControl tbForm = DivControlCreator.CreateTableSchema();

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

                HtmlGenericControl tcItem;
                HtmlGenericControl tr;

                try
                {
                    DivRenderStrategy.AppendItemCell(item, tbForm, out tr, out tcItem);
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
        private static void AppendInputControl(FormItemDefinition item, FormItemDefinition.ValidateTemplate validate, ref HtmlGenericControl tcItem)
        {
            StringWriter swInput = new StringWriter();
            //HtmlTextWriter htw = new HtmlTextWriter(swInput);
            switch (item.ItemType)
            {
                case FormItemDefinition.FormItemType.Text:		//单行输入
                    DivControlCreator.RenderTextInput(swInput, item, validate);
                    break;
                case FormItemDefinition.FormItemType.TextArea:	//多行输入
                    DivControlCreator.RenderTextAreaInput(swInput, item, validate);
                    break;
                case FormItemDefinition.FormItemType.DropDownList:		//下拉框


                    DivControlCreator.RenderSelect(swInput, item, validate);
                    break;
                case FormItemDefinition.FormItemType.Label:	//标签显示
                    DivControlCreator.RenderSpan(swInput, item);
                    break;
                case FormItemDefinition.FormItemType.CheckBoxList:	//多选框
                    DivControlCreator.RenderRadioOrCheckBoxList(swInput, item, validate, false);
                    break;
                case FormItemDefinition.FormItemType.RadioButtonList:		//单选框
                    DivControlCreator.RenderRadioOrCheckBoxList(swInput, item, validate, true);
                    break;
                case FormItemDefinition.FormItemType.Date:		//日期输入
                    DivControlCreator.RenderDateInput(swInput, item, validate);
                    break;
                case FormItemDefinition.FormItemType.File:		//文件上传
                    DivControlCreator.RenderFileInput(swInput, item, validate);
                    break;
                case FormItemDefinition.FormItemType.IncrementTextArea:     //自增多行输入
                    break;
                case FormItemDefinition.FormItemType.Html:      //复杂自定义HTML
                    DivControlCreator.RenderHTMLContainer(swInput, item);
                    break;
                case FormItemDefinition.FormItemType.Hidden:      //隐藏
                    DivControlCreator.RenderHiddenInput(swInput, item);
                    break;
                default:
                    break;
            }

            if (validate.Require)
                DivControlCreator.RenderRequireHint(swInput);

            if (DbUtils.ToString(item.Comment).Length > 0)
                DivControlCreator.RenderComment(swInput, item);

            ((Table)tcItem.Controls[0]).Rows[0].Cells[1].Text = swInput.ToString();
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
        private static void AppendRowNumber(HtmlGenericControl tbForm, bool appendRowNumber)
        {
            int startIndex = 1;
            int width = 4;

            for (int i = 0; i < tbForm.Controls.Count; i++)
            {
                if (appendRowNumber)
                {
                    TableCell tc = new TableCell(); 
                    tc.HorizontalAlign = HorizontalAlign.Right;
                    tc.Width = Unit.Percentage(width);
                    tc.Controls.Add(new LiteralControl(Convert.ToString(i + startIndex)));
                    //tbForm.Rows[i].Cells.AddAt(0, tc);
                }

            }
        }
        
        #endregion

        #endregion
    }
}