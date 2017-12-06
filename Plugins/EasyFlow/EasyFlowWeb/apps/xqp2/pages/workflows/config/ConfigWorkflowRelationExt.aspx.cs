using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Text;
using Botwave.Web;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Extension.Util;
using Botwave.Workflow.Extension.Domain;
using Botwave.Workflow.Extension.Service;
using Botwave.Entities;
using Botwave.XQP.Domain;
using System.Xml;
using System.Linq;
using Botwave.Workflow.Routing.Implements;
using Botwave.Workflow.Routing.Domain;
using System.Text.RegularExpressions;

public partial class apps_xqp2_pages_workflows_config_ConfigWorkflowRelationExt : Botwave.Security.Web.PageBase
{
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(apps_xqp2_pages_workflows_config_ConfigWorkflowRelationExt));

    #region service interfaces

    private IWorkflowDefinitionService workflowDefinitionService = (IWorkflowDefinitionService)Ctx.GetObject("workflowDefinitionService");

    public IWorkflowDefinitionService WorkflowDefinitionService
    {
        set { workflowDefinitionService = value; }
    }

    private IActivityRulesService activityRulesService;
    public IActivityRulesService ActivityRulesService
    {
        set { activityRulesService = value; }
    }
    #endregion

    #region properties

    public string WorkflowId
    {
        get { return (string)ViewState["WorkflowId"]; }
        set { ViewState["WorkflowId"] = value; }
    }

    public string RelationWorkflowId
    {
        get { return (string)ViewState["RelationWorkflowId"]; }
        set { ViewState["RelationWorkflowId"] = value; }
    }

    public string SettingType
    {
        get { return (string)ViewState["SettingType"]; }
        set { ViewState["SettingType"] = value; }
    }

    public string WorkflowName
    {
        get { return (string)ViewState["WorkflowName"]; }
        set { ViewState["WorkflowName"] = value; }
    }

    public string ActivityName
    {
        get { return (string)ViewState["ActivityName"]; }
        set { ViewState["ActivityName"] = value; }
    }

    public string TbHtml
    {
        get { return (string)ViewState["TbHtml"]; }
        set { ViewState["TbHtml"] = value; }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string id = Request.QueryString["id"];
            string wid = Request.QueryString["wid"];
            string copyType = Request.QueryString["c"];
            CZWorkflowRelationSetting activity = new CZWorkflowRelationSetting();

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(wid))
            {
                ShowError(MessageHelper.Message_ArgumentException);
            }

            activity = CZWorkflowRelationSetting.SelectById(int.Parse(id));

            if (activity == null)
            {
                ShowError("未保存指定实例.", AppPath + "apps/xqp2/pages/workflows/config/ConfigWorkflowRelation.aspx?wid=" + wid);
            }

            this.WorkflowId = wid;
            this.WorkflowName = activity.WorkflowName;
            this.ActivityName = activity.ActivityName;
            this.LoadActivity(activity);
        }
    }

    #region load

    protected void LoadActivity(CZWorkflowRelationSetting activity)
    {
        this.ltlActivityName.Text = activity.ActivityName;
        string users = activity.RelationCreator;
        string fieldsAssemble = activity.FieldsAssemble;
        this.txtUsers.Text = users;
        if (!string.IsNullOrEmpty(fieldsAssemble))
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(fieldsAssemble);
            XmlNodeList list = doc.SelectSingleNode("Root").SelectNodes("item");
            StringBuilder sb = new StringBuilder();
            foreach (XmlNode node in list)
            {
                sb.AppendFormat("<tr><td>{0}</td><td>{1}</td><td><a href=\"#\">删除</a></td></tr>", node.Attributes["FName"].Value, node.Attributes["RelationFName"].Value);
            }
            TbHtml = sb.ToString();
        }
        IList<WorkflowDefinition> wfDefinitions = workflowDefinitionService.GetWorkflowDefinitionList();
        string relationWorkflowId = wfDefinitions.Where(f => f.WorkflowName == activity.RelationWorkflowName).First().WorkflowId.ToString();
        RelationWorkflowId = relationWorkflowId;
        drdlFName.DataSource = activityRulesService.GetFormItemDefinitions(WorkflowId);
        drdlFName.DataTextField = "Name";
        drdlFName.DataValueField = "FName";
        drdlFName.DataBind();
        drdlFName.Dispose();
        drdlFName.Items.Insert(0, new ListItem("- 请选择 -", ""));
        drdlRelationFName.DataSource = drdlFieldName.DataSource = activityRulesService.GetFormItemDefinitions(relationWorkflowId);
        drdlRelationFName.DataTextField = "Name";
        drdlRelationFName.DataValueField = "FName";
        drdlRelationFName.DataBind();
        drdlRelationFName.Dispose();
        drdlRelationFName.Items.Insert(0, new ListItem("- 请选择 -", ""));
        drdlFieldName.DataTextField = "Name";
        drdlFieldName.DataValueField = "FName";
        drdlFieldName.DataBind();
        drdlFieldName.Dispose();
        drdlFieldName.Items.Insert(0, new ListItem("- 请选择 -", ""));

        listResults.DataSource = activityRulesService.GetRelationRules(WorkflowId,ActivityName);
        listResults.DataBind();
    }

   

    #endregion

    protected void listResults_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            // 删除
            string ruleId = e.CommandArgument.ToString();
            if (activityRulesService.ActivityRulesDetailDelete(ruleId) > 0)
            {
                ShowSuccess("删除规则成功！");
            }
            else
            {
                ShowError("删除规则失败。");
                //throw new Exception("删除用户失败。");
            }
        }
        else if (e.CommandName == "Edit")
        {
            RulesDetail info = activityRulesService.GetActivityRule(new Guid(e.CommandArgument.ToString()));
            txtDescription.Text = info.Description;
            txtCommandRules.Text = info.Conditions;
        }
    }
    protected void drdlFName_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataRow itemDataSource = activityRulesService.GetFormItemDataSource(this.WorkflowId, drdlFName.SelectedValue);
        if (itemDataSource == null)
            this.ltlControl.Text = "<input type='text' id='txtVal' />";
        else if (itemDataSource["itemType"].ToString() == "2" || itemDataSource["itemType"].ToString() == "4" || itemDataSource["itemType"].ToString() == "5")
        {
            StringBuilder strDataSource = new StringBuilder(@"<select id='txtVal'><option value='' selected = 'selected'>- 请选择 -</option>");
            string[] arr = itemDataSource["datasource"].ToString().Split(',');
            foreach (string str in arr)
            {
                strDataSource.Append("<option value='" + str + "'>" + str + "</option>");
            }
            strDataSource.Append("</select>");
            this.ltlControl.Text = strDataSource.ToString();
        }
    }

    #region 保存

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string workflowId = this.WorkflowId;
        string workflowName = this.WorkflowName;
        string activityName = this.ActivityName;
        CZWorkflowRelationSetting activity = new CZWorkflowRelationSetting();
        activity = CZWorkflowRelationSetting.SelectById(int.Parse(Request.QueryString["id"]));

        //foreach (FieldControlInfo item in controlItems)
        //{
        //    string msg = string.Format("{0}-{1}-{2}-{3}-{4}-{5}", item.WorkflowName, item.ActivityName, item.FieldName, item.Id.ToString(), item.FieldValue, item.TargetUsers);
        //    log.Warn(msg);
        //}

        activity.FieldsAssemble=hidXml.Value;
        activity.RelationCreator = txtUsers.Text;
        activity.Update();
        //log.WarnFormat("{0}-{1}-{2}-{3}-{4}-{5}", activity.CommandRules, activity.AllocatorUsers, activity.AllocatorResource, activity.ExtendAllocators, activity.ExtendAllocatorArgs, activity.DefaultAllocator);
        //log.WarnFormat("{0}-{1}-{2}-{3}-{4}", assignAllocator.AllocatorUsers, assignAllocator.AllocatorResource, assignAllocator.ExtendAllocators, assignAllocator.ExtendAllocatorArgs, assignAllocator.DefaultAllocator);

        ShowSuccess("更新通知人分派设置成功.", AppPath + "apps/xqp2/pages/workflows/config/ConfigWorkflowRelation.aspx?wid=" + workflowId);
    }

    protected void btn_Add_Click(object sender, EventArgs e)
    {
        RulesDetail rulesDetail = new RulesDetail();
        rulesDetail.Ruleid = Guid.NewGuid();
        rulesDetail.ActivityName = this.ActivityName;
        rulesDetail.Workflowid = new Guid(this.WorkflowId);
        rulesDetail.NextActivityName = "-";
        rulesDetail.StepType = 2;
        rulesDetail.ParentRuleId = Guid.Empty;
        txtCommandRules.Text = Botwave.XQP.Commons.XQPHelper.DecodeBase64("utf-8", txtCommandRules.Text);
        //rulesDetail.Conditions = txtCommandRules.Text.Replace("FNumber", "to_number(FNumber)");
        rulesDetail.Conditions = txtCommandRules.Text;
        rulesDetail.Description = txtDescription.Text;
        rulesDetail.Title = this.WorkflowName + "-" + ltlActivityName.Text;
        rulesDetail.Status = 1;
        rulesDetail.Creator = CurrentUserName;
        rulesDetail.Createdtime = DateTime.Now;
        rulesDetail.LastModifier = CurrentUserName;
        rulesDetail.LastModtime = DateTime.Now;
        string fieldsStr = rulesDetail.Conditions.Trim();
        foreach (ListItem item in drdlFName.Items)
        {
            if (Regex.IsMatch(fieldsStr, item.Value, RegexOptions.IgnoreCase) && !string.IsNullOrEmpty(item.Value))
                rulesDetail.FieldsAssemble += "_" + item.Value + "_;";
        }
        if (activityRulesService.ExistActivityRules(rulesDetail) > -1)
        {
            activityRulesService.ActivityRulesDetailUpdateByActName(rulesDetail);
            ShowSuccess("成功创建规则！");
        }
        else
        {
            activityRulesService.ActivityRulesDetailInsert(rulesDetail);
            ShowSuccess("成功创建规则！");
        }
    }

    private static string FormatResourceId(string resourceId, bool isChecked)
    {
        if (string.IsNullOrEmpty(resourceId))
            return string.Empty;

        string results = resourceId.ToUpper();
        bool isContain = results.StartsWith(ResourceHelper.PrefixDisableResource, StringComparison.OrdinalIgnoreCase);
        string disablePattern = ResourceHelper.PrefixDisableResource.ToUpper();

        if (isChecked)
        {
            // 允许权限控制
            return (isContain ? results.Replace(disablePattern, "") : results);
        }
        else
        {
            return (isContain ? results : disablePattern + results);
        }
    }
    #endregion
    
   
}
