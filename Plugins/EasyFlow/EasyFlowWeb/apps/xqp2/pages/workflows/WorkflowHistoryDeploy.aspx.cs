using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Extension;
using Botwave.Workflow.Extension.Service;
using Botwave.XQP.Commons;
using Botwave.Workflow.Routing.Implements;
using Botwave.DynamicForm.Domain;
using Botwave.DynamicForm.Extension.Implements;

public partial class xqp2_contrib_workflow_pages_WorkflowHistoryDeploy : Botwave.Security.Web.PageBase
{
    private static string ReturnUrl = AppPath + "apps/xqp2/pages/workflows/WorkflowHistoryDeploy.aspx";
    private static string AliasImageFormat = AppPath + "contrib/workflow/res/groups/flow_{0}.gif";

    #region service interfaces

    private IDeployService deployService = (IDeployService)Ctx.GetObject("deployService");
    private IWorkflowDefinitionService workflowDefinitionService = (IWorkflowDefinitionService)Ctx.GetObject("workflowDefinitionService");
    private IWorkflowResourceService workflowResourceService = (IWorkflowResourceService)Ctx.GetObject("workflowResourceService");
    private IActivityRulesHelper activityRulesHelper = (IActivityRulesHelper)Ctx.GetObject("activityRulesHelper");
    private IFormDefinitionDeployService formDefinitionDeployService = (IFormDefinitionDeployService)Ctx.GetObject("FormDefinitionDeployService");

    public IDeployService DeployService
    {
        get { return deployService; }
        set { deployService = value; }
    }
    public IWorkflowDefinitionService WorkflowDefinitionService
    {
        get { return workflowDefinitionService; }
        set { workflowDefinitionService = value; }
    }

    public IWorkflowResourceService WorkflowResourceService
    {
        get { return workflowResourceService; }
        set { workflowResourceService = value; }
    }
    public IActivityRulesHelper ActivityRulesHelper
    {
        get { return activityRulesHelper; }
        set { activityRulesHelper = value; }
    }
    public IFormDefinitionDeployService FormDefinitionDeployService
    {
        set { formDefinitionDeployService = value; }
        get { return formDefinitionDeployService; }
    }
    #endregion

    private Botwave.Workflow.ActionResult acitonResult;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindData(0,0);
        }
    }

    private void BindData(int recordCount, int pageIndex)
    {
        string key = Request.QueryString["key"];
        IList<WorkflowDefinition> list = workflowDefinitionService.GetAllWorkflowDefinition(); string Workflows = "";
        if (!Botwave.XQP.Util.CZWorkflowUtility.HasAdvanceSearch(CurrentUser, "A007"))
        {
            IList<WorkflowDefinition> allowWorkflows = Botwave.XQP.Util.CZWorkflowUtility.GetAllowedWorkflows(list, CurrentUser.Resources, new string[] { "0005" });
            StringBuilder sb = new StringBuilder();
            
            foreach (WorkflowDefinition item in allowWorkflows)
            {
                sb.Append(item.WorkflowName + ",");
            }
            if (sb.Length > 0)
            {
                sb = sb.Remove(sb.Length - 1, 1);
                Workflows = sb.ToString();
            }
        }
        DataTable dt = Botwave.XQP.Domain.CZWorkflowDefinition.GetHistoryWorkflowDefinition(Workflows, key, pageIndex, listPager.ItemsPerPage, ref recordCount);
        
        rpFlowList.DataSource = dt;
        rpFlowList.DataBind();
        listPager.TotalRecordCount = recordCount;
        listPager.DataBind();
    }

    protected void listPager_PageIndexChanged(object sender, Botwave.Web.Controls.PageChangedEventArgs e)
    {
        this.BindData(listPager.TotalRecordCount, e.NewPageIndex);
    }
    protected void rpFlowList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView item = e.Item.DataItem as DataRowView;
        LinkButton btnSetEnable = e.Item.FindControl("btnSetEnable") as LinkButton;
        if (!Botwave.Commons.DbUtils.ToBoolean(item["Enabled"]))
        {
            btnSetEnable.Text = "启用";
            btnSetEnable.CssClass = "ico_enable";
        }
        else
        {
            btnSetEnable.OnClientClick = "return confirm('确定要停用流程?');";
        }

        if (!String.IsNullOrEmpty(Botwave.Commons.DbUtils.ToString(item["WorkflowAlias"])))
        {
            string workflowAlias = Botwave.Commons.DbUtils.ToString(item["WorkflowAlias"]);
            string aliasImageUrl = String.Format(AliasImageFormat, workflowAlias.ToLower());
            if (!string.IsNullOrEmpty(aliasImageUrl))
            {
                Literal ltlWorkflowAlias = e.Item.FindControl("ltlWorkflowAlias") as Literal;
                ltlWorkflowAlias.Text = string.Format("<img alt=\"{0}\" class=\"groupImage\" src=\"{1}\" />", workflowAlias, aliasImageUrl);
            }
        }
    }

    protected void rpFlowList_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        Guid workflowId = new Guid(e.CommandArgument.ToString());
        if (e.CommandName == "Export")
        {
            ExportFlow(workflowId);
        }
        else if (e.CommandName == "ExportRules")//导出规则
        {
            WorkflowDefinition wd = workflowDefinitionService.GetWorkflowDefinition(new Guid(e.CommandArgument.ToString()));
            string fileName = wd.WorkflowName;
            DataTable dt = activityRulesHelper.GetRulesInfo(e.CommandArgument.ToString());
            if (dt != null && dt.Rows.Count > 0)
            {
                dt.TableName = wd.WorkflowName;
                Botwave.XQP.Commons.ExcelHelper.Export(this.Page, dt, fileName);
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "js", "<script>alert('此流程未定义规则！')</script>");
            }
        }
        else if (e.CommandName == "ExportItem")
        {
            //导出表单
            string workflowid = e.CommandArgument.ToString();
            ExportItem(workflowid);
        }
    }
    private void ExportFlow(Guid flowId)
    {
        acitonResult = new Botwave.Workflow.ActionResult();
        string fileName = DateTime.Now.ToString("yyyyMMddHHmm") + DateTime.Now.Ticks + ".xml";
        string xmlFile = Path.Combine(Request.PhysicalApplicationPath + Botwave.GlobalSettings.Instance.TemporaryDir, fileName);

        XmlDocument doc = new XmlDocument();
        doc.LoadXml("<root></root>");
        doc.Save(xmlFile);

        XmlWriterSettings settings = new XmlWriterSettings();
        settings.Indent = true;
        settings.Encoding = System.Text.Encoding.UTF8;
        XmlWriter writer = XmlWriter.Create(xmlFile, settings);
        acitonResult = deployService.ExportWorkflow(writer, flowId);
        if (acitonResult.Success)
        {
            WorkflowDefinition wd = workflowDefinitionService.GetWorkflowDefinition(flowId);
            Response.Redirect(string.Format("download.ashx?path={0}&displayName={1}", AppPath + "App_Data/Temp/" + fileName, HttpUtility.UrlDecode(wd.WorkflowName)));
        }
    }

    private void ExportItem(string workflowid)
    {
        string id = "";
        string account = "";
        string password = "";
        //string formName = TextBoxFormName.Text.Trim();
        WorkflowDefinition wd = workflowDefinitionService.GetWorkflowDefinition(new Guid(workflowid));
        string workflowname = wd.WorkflowName;
        FormResult result = formDefinitionDeployService.Export(new Guid(workflowid));
        if (result.Success == 1)
        {

            string fileNameStr = workflowname + "表单.xml";
            //指定文件的完整路径
            string filepath = Server.MapPath(AppPath + "App_Data/Temp/" + fileNameStr);
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
            FileStream file = new FileStream(filepath, FileMode.Create);
            StreamWriter writer = new StreamWriter(file);
            writer.Write(result.Data);
            writer.Close();
            file.Close();
            DownloadFile(Response, filepath, string.Empty, true);
        }
        else
        {
            //LabelMessgae1.Text = result.Message;
            ShowError(result.Message, ReturnUrl);
        }
    }

    /// <summary>
    /// 下载文件.
    /// </summary>
    /// <param name="res"></param>
    /// <param name="filePath">文件路径</param>
    /// <param name="displayName">显示名称，不包括扩展名</param>
    /// <param name="shouldDelete">是否删除</param>
    private static void DownloadFile(HttpResponse res, string filePath, string displayName, bool shouldDelete)
    {
        if (!File.Exists(filePath))
        {
            // res.Write(FileNotExistsMessage);
        }
        else
        {
            FileInfo file = new FileInfo(filePath);
            if (String.IsNullOrEmpty(displayName))
            {
                displayName = HttpUtility.UrlEncode(file.Name);
            }
            else
            {
                displayName = HttpUtility.UrlEncode(displayName) + file.Extension;
            }

            res.Clear();
            res.AddHeader("Pragma", "public");
            res.AddHeader("Expires", "0");
            res.AddHeader("Cache-Control", "must-revalidate, post-check=0, pre-check=0");
            res.AddHeader("Content-Type", "application/force-download");
            res.AddHeader("Content-Type", "application/octet-stream");
            res.AddHeader("Content-Type", "application/download");
            res.AddHeader("Content-Disposition", String.Format("attachment; filename={0}", displayName));
            res.AddHeader("Content-Transfer-Encoding", "binary");
            res.AddHeader("Content-Length", file.Length.ToString());

            res.WriteFile(filePath);
            res.Flush();

            if (shouldDelete)
            {
                File.Delete(filePath);
            }
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string key = HttpUtility.UrlEncode(txtKeywords.Text);
        Response.Redirect("workflowHistoryDeploy.aspx?key=" + key);
    }
}
