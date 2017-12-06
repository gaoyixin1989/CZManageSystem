using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Botwave.Web;
using Botwave.DynamicForm;
using Botwave.DynamicForm.Services;
using Botwave.DynamicForm.Domain;
using Botwave.DynamicForm.Extension.Implements;
using Botwave.DynamicForm.Extension.Domain;

public partial class contrib_dynamicform_pages_config_UC_GetOuterData : Botwave.Security.Web.PageBase
{
    #region properties
    private IFormDefinitionService formDefinitionService = (IFormDefinitionService)Ctx.GetObject("formDefinitionService");
    private IGetDataService getDataService = (IGetDataService)Ctx.GetObject("getDataService");

    public IFormDefinitionService FormDefinitionService
    {
        get { return formDefinitionService; }
        set { formDefinitionService = value; }
    }

    public IGetDataService GetDataService
    {
        get { return getDataService; }
        set { getDataService = value; }
    }
    #endregion

    public string WorkflowId
    {
        get { return (string)ViewState["WorkflowId"]; }
        set { ViewState["WorkflowId"] = value; }
    }

    public string FormDefinitionId
    {
        get { return (string)ViewState["FormDefinitionId"]; }
        set { ViewState["FormDefinitionId"] = value; }
    }

    public string FormItemDefinitionId
    {
        get { return (string)ViewState["FormItemDefinitionId"]; }
        set { ViewState["FormItemDefinitionId"] = value; }
    }

    public string EntityType
    {
        get { return (string)ViewState["EntityType"]; }
        set { ViewState["EntityType"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        System.Web.UI.HtmlControls.HtmlGenericControl child = new System.Web.UI.HtmlControls.HtmlGenericControl("link");
        Page handler = (Page)HttpContext.Current.Handler;
        handler.Header.Controls.Clear();//清理
        WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/common.js");
        WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/jquery-latest.pack.js");
        WebUtils.RegisterScriptReference(this.Page, AppPath + "res/js/Base64.js");
        if (!IsPostBack)
        {
            string workflowId = WorkflowId = Request.QueryString["wid"];
            string formDefinitionId = FormDefinitionId = Request.QueryString["fdId"];
            string formItemDefinitionId = FormItemDefinitionId = Request.QueryString["fid"];
            string entityType = EntityType = Request.QueryString["EntityType"];
            BindData(FormItemDefinitionId);
        }
    }

    private void BindData(string formItemDefinitionId)
    {
        FormItemDefinition formItemDefinition = formDefinitionService.GetFormItemDefinitionById(new Guid(formItemDefinitionId));
        if (formItemDefinition != null)
        {
            FormItemExtension formItemExtension = getDataService.GetFormItemExtensionById(new Guid(formItemDefinitionId));
            if (formItemExtension != null)
            {
                radWay_0.Checked = formItemExtension.GetDataType == 0;
                radWay1.Checked = formItemExtension.GetDataType == 1;
                radWay2.Checked = formItemExtension.GetDataType == 2;
                radWay3.Checked = formItemExtension.GetDataType == 3;
                switch (formItemExtension.GetDataType)
                {
                    case 0:
                        radWay_0.Checked = true;
                        break;
                    case 1:
                        radWay1.Checked = true;
                        TB_JS.Value = formItemExtension.SourceString;
                        break;
                    case 2:
                        radWay2.Checked = true;
                        txtSQL.Value = formItemExtension.SourceString;
                        txtSqlConnection.Text = formItemExtension.GetDataSource;
                        break;
                    case 3:
                        radWay3.Checked = true;
                        txtWSConnection.Text = formItemExtension.GetDataSource;
                        txtWSFunction.Text = formItemExtension.SourceString;
                        break;
                    default:
                        break;
                }
            }
            if (!formItemDefinition.ItemType.Equals(FormItemDefinition.FormItemType.Text) && !formItemDefinition.ItemType.Equals(FormItemDefinition.FormItemType.TextArea))
            {
                radWay1.Checked = false;
                radWay1.Attributes.Add("style","display:none");
               
            }
            if (formItemDefinition.ItemType.Equals(FormItemDefinition.FormItemType.Html))
            {
                radWay3.Checked = false;
                radWay3.Attributes.Add("style", "display:none");
            }

        }
    }

    private bool saveData()
    {
        TB_JS.Value = Botwave.XQP.Commons.XQPHelper.DecodeBase64("utf-8", TB_JS.Value);
        txtSQL.Value = Botwave.XQP.Commons.XQPHelper.DecodeBase64("utf-8", txtSQL.Value);
        txtSqlConnection.Text = Botwave.XQP.Commons.XQPHelper.DecodeBase64("utf-8", txtSqlConnection.Text);
        txtWSFunction.Text = Botwave.XQP.Commons.XQPHelper.DecodeBase64("utf-8", txtWSFunction.Text);
        txtWSConnection.Text = Botwave.XQP.Commons.XQPHelper.DecodeBase64("utf-8", txtWSConnection.Text);
        FormItemExtension formItemExtension = getDataService.GetFormItemExtensionById(new Guid(FormItemDefinitionId));
        if (formItemExtension == null)
        {
            formItemExtension = new FormItemExtension();
        }
        
        formItemExtension.FormItemDefinitionId = new Guid(FormItemDefinitionId);
        IList<FormItemDefinition> definitions = formDefinitionService.GetFormItemDefinitionsByFormDefinitionId(new Guid(FormDefinitionId));
        if (radWay_0.Checked)
        {
            formItemExtension.GetDataType = 0;
            formItemExtension.GetDataSource = null;
            formItemExtension.SourceString = null;
        }

        else if (radWay1.Checked)
        {
            int index = 0;
            formItemExtension.GetDataType = 1;
            formItemExtension.GetDataSource = null;
            formItemExtension.SourceString = TB_JS.Value;
            string str = formItemExtension.SourceString;
            IList<string> docC = new List<string>();
            int num = 0;
            foreach (string doc in formItemExtension.SourceString.Split('+', '-', '*', '/', '%', '（', ')'))
            {
                int count = System.Text.RegularExpressions.Regex.Matches(doc, "@").Count;//计算匹配字符个数
                if (count > 2)
                {
                    Response.Write("<script>alert('您填写的表达公式不正确，导致一些数值类型的字段没有被正确的替换。');</script>");
                    return false;
                }
                if (!docC.Contains(doc))
                {
                    docC.Add(doc);
                    num++;
                }
            }
            
            if(num >2){//限制只能有两个字段
                Response.Write("<script>alert('您填写的表达公式不正确，只能有两个字段。');</script>");
                return false;
            }
            foreach (FormItemDefinition definition in definitions)
            {
                //if (definition.ItemDataType != FormItemDefinition.DataType.Decimal)
                //    continue;

                if (docC.Contains("@"+definition.FName+"@"))
                {
                    if (definition.ItemDataType != FormItemDefinition.DataType.Decimal)
                    {
                        Response.Write("<script>alert('您填写的表达公式不正确，一些字段的数值类型不是数字。');</script>");
                        return false;
                    }
                }
                str = str.Replace("@" + definition.FName + "@", "");
            }
            if (str.Contains("@") || str.Length == formItemExtension.SourceString.Length)
            {
                Response.Write("<script>alert('您填写的表达公式不正确，导致一些数值类型的字段没有被正确的替换。');</script>");
                return false;
            }
        }
        else if (radWay2.Checked)//sql
        {
            formItemExtension.GetDataType = 2;
            formItemExtension.GetDataSource = txtSqlConnection.Text;
            formItemExtension.SourceString = txtSQL.Value;
            if (!CheckDataPrams(definitions, formItemExtension.SourceString))
            {
                Response.Write("<script>alert('您的SQL表达式不正确，导致一些参数没有被正确的替换。');</script>");
                return false;
            }
        }
        else if (radWay3.Checked)
        {
            formItemExtension.GetDataType = 3;
            formItemExtension.GetDataSource = txtWSConnection.Text;
            formItemExtension.SourceString = txtWSFunction.Text;
            if (formItemExtension.SourceString.Split(':', '：').Length == 1 || formItemExtension.SourceString.Split(':', '：').Length > 2)
            {
                Response.Write("<script>alert('您的WebService参数表达式不正确，导致一些参数没有被正确的替换。');</script>");
                return false;
            }
            else if (!CheckDataPrams(definitions, formItemExtension.SourceString.Split(':','：')[1]))
            {
                Response.Write("<script>alert('您的WebService参数表达式不正确，导致一些参数没有被正确的替换。');</script>");
                return false;
            }
        }

        if (getDataService.ExistFormItemExtension(new Guid(FormItemDefinitionId)))
            getDataService.UpdateFormItemExtension(formItemExtension);
        else
            getDataService.InserFormItemExtension(formItemExtension);
        BindData(FormItemDefinitionId);
        return true;
    }

    private bool CheckDataPrams(IList<FormItemDefinition> definitions, string prams)
    {
        IDictionary<string, string> dict = new Dictionary<string, string>();
        dict.Add("#WorkflowName#",string.Empty);
        dict.Add("#Wiid#", string.Empty);
        dict.Add("#Title#", string.Empty);
        dict.Add("#SheetId#", string.Empty);
        dict.Add("#CurrentUser#", string.Empty);
        dict.Add("#DpId#", string.Empty);
        dict.Add("#Aiid#", string.Empty);
        dict.Add("#ActivityName#", string.Empty);
        foreach (FormItemDefinition temp in definitions)
        {
            if (!dict.ContainsKey("#" + temp.FName + "#"))
                dict.Add("#" + temp.FName + "#", string.Empty);
        }
        foreach (KeyValuePair<string, string> temp in dict)
        {
            prams = prams.Replace(temp.Key, temp.Value);
        }
        foreach (KeyValuePair<string, string> temp in dict)
        {
            if(prams.Contains(temp.Key))
            return false;
        }
        return true;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (saveData())
            Response.Write("<script>alert('保存成功！');</script>");
        BindData(FormItemDefinitionId);
    }

    protected void btnSaveClose_Click(object sender, EventArgs e)
    {
        if (saveData())
            Response.Write("<script>alert('保存成功！');window.parent.close();</script>");
    }
}
