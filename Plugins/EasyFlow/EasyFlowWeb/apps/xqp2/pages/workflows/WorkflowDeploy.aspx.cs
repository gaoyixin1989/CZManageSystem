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

public partial class xqp2_contrib_workflow_pages_WorkflowDeploy : Botwave.Security.Web.PageBase
{
    private static string ReturnUrl = AppPath + "apps/xqp2/pages/workflows/workflowDeploy.aspx";
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
            BindData();
        }
    }

    private void BindData()
    {
        string key = Request.QueryString["key"];
        IList<WorkflowDefinition> list = workflowDefinitionService.GetAllWorkflowDefinition();
        Botwave.Workflow.Extension.Util.ResourceHelper.Resources_WorkflowCommons = new string[] { 
            "流程协作", "提单","报表统计","查看保密单","高级查询","流程管理" };

        if (!string.IsNullOrEmpty(key))
        {
            txtKeywords.Text = key;
            IList<WorkflowDefinition> templist = new List<WorkflowDefinition>();
            foreach (WorkflowDefinition item in list)
            {
                string workflowAlias = item.WorkflowAlias == null ? "N" : item.WorkflowAlias;
                if (workflowAlias.IndexOf(key) != -1 || item.WorkflowName.IndexOf(key) != -1)
                    templist.Add(item);
            }
            rpFlowList.DataSource = templist;
            rpFlowList.DataBind();
        }
        else
        {
            rpFlowList.DataSource = list;
            rpFlowList.DataBind();
        }
        //rpFlowList.DataSource = list;
        //rpFlowList.DataBind();
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        HttpPostedFile httpFile = this.fuAttachment.PostedFile;
        // application/x-xml, text/xml
        if (!(httpFile.ContentType == "text/xml" || httpFile.ContentType == "application/x-xml"))
        {
            lblInfo.Text = "只允许上传 <font color=red>.xml</font> 格式的文件，请重新选择！";
            return;
        }
        if (httpFile.ContentLength <= 0)
        {
            lblInfo.Text = "文件太小，请重新选择！";
            return;
        }
        string saveFileName = GetSaveFileName(httpFile);
        this.fuAttachment.SaveAs(saveFileName);

        acitonResult = new Botwave.Workflow.ActionResult();
        XmlReader xmlReader = XmlReader.Create(saveFileName);
        if (xmlReader == null)
            throw new Exception("未正确创建 XML 输入流，请检查文件.");
        acitonResult = deployService.CheckWorkflow(xmlReader);
        if (acitonResult.Success)
        {
            using (xmlReader = XmlReader.Create(saveFileName))
            {
                acitonResult = deployService.DeployWorkflow(xmlReader, CurrentUser.UserName);
                if (xmlReader.ReadState != ReadState.Closed)
                    xmlReader.Close();
            }
            if (acitonResult.Success)
            {
                if (File.Exists(saveFileName))
                    File.Delete(saveFileName);
                //Logger.Info(acitonResult.Message);
                //WriteNomalLog("部署流程", "部署流程成功");
                ShowSuccess(acitonResult.Message, ReturnUrl);
            }
        }

        lblInfo.Text = acitonResult.Message;
    }

    /// <summary>
    /// 获取上传文件保存的路径.
    /// </summary>
    /// <param name="httpFile"></param>
    /// <returns></returns>
    private string GetSaveFileName(HttpPostedFile httpFile)
    {
        string fileName = httpFile.FileName;//获取上传文件的全路径
        string extensionName = System.IO.Path.GetExtension(fileName);//获取扩展名


        string tempFileName = DateTime.Now.ToString("yyyyMMddHHmmfff") + extensionName;

        string saveFileName = Path.Combine(Request.PhysicalApplicationPath + Botwave.GlobalSettings.Instance.TemporaryDir, tempFileName);//合并两个路径为上传到服务器上的全路径]
        return saveFileName.Replace("/", "\\");
    }

    protected void rpFlowList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        WorkflowDefinition item = e.Item.DataItem as WorkflowDefinition;
        LinkButton btnSetEnable = e.Item.FindControl("btnSetEnable") as LinkButton;
        if (!item.Enabled)
        {
            btnSetEnable.Text = "启用";
            btnSetEnable.CssClass = "ico_enable";
        }
        else
        {
            btnSetEnable.OnClientClick = "return confirm('确定要停用流程?');";
        }

        if (!String.IsNullOrEmpty(item.WorkflowAlias))
        {
            string workflowAlias = item.WorkflowAlias;
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
        else if (e.CommandName == "Delete")
        {
            acitonResult = deployService.DeleteWorkflow(workflowId);
            if (!acitonResult.Success)
            {
                XQPHelper.PushWorkflowList();
                WriteExLog(CurrentUserName,"删除流程",acitonResult.Message, "删除流程失败");
                ShowError(acitonResult.Message, ReturnUrl);
            }
            else
            {
                this.UpdateResourceVisible(workflowId, false);
                WriteNomalLog(CurrentUserName, "删除流程", "删除流程["+workflowId+"]成功");
                BindData();
            }
        }
        else if (e.CommandName == "SetEnabled")
        {
            LinkButton btnSetEnable = e.Item.FindControl("btnSetEnable") as LinkButton;
            bool enabled = (btnSetEnable.Text == "启用");

            workflowDefinitionService.UpdateWorkflowEnabled(workflowId, enabled);
            this.UpdateResourceVisible(workflowId, enabled);
            XQPHelper.PushWorkflowList();
            WriteNomalLog(CurrentUserName, "部署流程", "启用/停用流程[" + workflowId + "]成功");
            BindData();
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
        else
            lblInfo.Text = acitonResult.Message;
    }

    private void UpdateResourceVisible(Guid workflowId, bool isVisible)
    {
        workflowResourceService.UpdateWorkflowResourceVisible(workflowId, isVisible);
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
        Response.Redirect("workflowDeploy.aspx?key=" + key);
    }

    protected void btnRulesUpload_Click(object sender, EventArgs e)
    {
        if (!RulesItem.HasFile)
            return;
        string path = System.Web.HttpContext.Current.Server.MapPath(AppPath + "App_Data/Temp/");
        //string nowdate = "EQ_" + DateTime.Now.ToString("yyyyMMddhhmmss");
        string pageFileName = path + RulesItem.FileName;
        if (File.Exists(pageFileName)) File.Delete(pageFileName);
        RulesItem.SaveAs(pageFileName);
        FileInfo file = new FileInfo(RulesItem.FileName);
        if (file.Extension == ".xls" || file.Extension == ".xlsx")
        {
            DataSet resultSet = ExcelHelper.ImportToDataSet(pageFileName);
            StringBuilder sb = new StringBuilder();
            foreach (DataTable result in resultSet.Tables)
            {
                if (result.Rows.Count > 0)
                {
                    string workflowName = result.TableName;
                    WorkflowDefinition wd = workflowDefinitionService.GetCurrentWorkflowDefinition(workflowName);
                    //ClientScript.RegisterStartupScript(this.GetType(), "js", "<script>alert('" + workflowName + "')</script>");
                    if (wd == null)
                    {
                        sb.Append("找不到流程[" + result.TableName + "]的定义<br/>");
                        continue;
                    }
                    activityRulesHelper.ImportRules(result, wd.WorkflowId);
                    sb.Append("成功导入[" + workflowName + "]的规则<br/>");
                }
                else
                    sb.Append("找不到流程[" + result.TableName + "]的规则定义<br/>");
            }
            WriteNomalLog(CurrentUserName, "部署流程",sb.ToString());
            ShowSuccess("操作成功<br/>"+sb.ToString());
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "js", "<script>alert('请上传Excel格式的文件！')</script>");
            return;
        }
    }

    //导入流程表单
    protected void btnItemUpload_Click(object sender, EventArgs e)
    {
        //string id = TextBoxId2.Text.Trim();
        //string account = TextBoxAccount2.Text.Trim();
        //string password = TextBoxPassword2.Text.Trim();
        string action = DropDownList1.SelectedValue;
        if (action == string.Empty)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "js", "<script>alert('请选择操作方式')</script>");
            return;
        }
        if (!fuItem.HasFile)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "js", "<script>alert('请上传文件')</script>");
            return;
        }

        string filepath = Server.MapPath(AppPath + "App_Data/Temp/" + fuItem.FileName);
        if (File.Exists(filepath)) File.Delete(filepath);
        fuItem.SaveAs(filepath);

        FileStream file = new FileStream(filepath, FileMode.Open);
        StreamReader reader = new StreamReader(file);
        string xml = reader.ReadToEnd();
        FormResult result = formDefinitionDeployService.Import( "admin", int.Parse(action), xml);
        //Label2.Text = result.Message;
        WriteNomalLog(CurrentUserName, "部署流程", result.Message);
        ShowSuccess(result.Message, ReturnUrl);
        reader.Close();
        file.Close();
    }
}
