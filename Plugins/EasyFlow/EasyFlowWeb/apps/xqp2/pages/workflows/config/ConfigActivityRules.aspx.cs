using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Botwave.Workflow.Routing;
using Botwave.Workflow.Routing.Implements;
using Botwave.Web;
using Botwave.Workflow.Routing.Domain;
using Botwave.Workflow.Extension.Util;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using System.Text;
using System.Text.RegularExpressions;
using Botwave.Commons;

public partial class apps_xqp2_pages_workflows_config_ConfigActivityRules : Botwave.Security.Web.PageBase
{
    private IActivityRulesService activityRulesService = (IActivityRulesService)Ctx.GetObject("activityRulesService");
    private IActivityDefinitionService activityDefinitionService = (IActivityDefinitionService)Ctx.GetObject("activityDefinitionService");

    public IActivityRulesService ActivityRulesService
    {
        set { activityRulesService = value; }
    }

    public IActivityDefinitionService ActivityDefinitionService
    {
        set { activityDefinitionService = value; }
    }

    public string ActivityId
    {
        get { return (string)ViewState["ActivityId"]; }
        set { ViewState["ActivityId"] = value; }
    }

    public string WorkflowId
    {
        get { return (string)ViewState["WorkflowId"]; }
        set { ViewState["WorkflowId"] = value; }
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

    public string NextActivityName
    {
        get { return (string)ViewState["NextActivityName"]; }
        set { ViewState["NextActivityName"] = value; }
    }

    public bool Type
    {
        get { return (bool)ViewState["Type"]; }
        set { ViewState["Type"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string workflowid = Request.QueryString["wfid"];
            string activityId = Request.QueryString["aid"];
            this.Type = Request.QueryString["type"] == null ? false : true;
            if (string.IsNullOrEmpty(workflowid) || string.IsNullOrEmpty(activityId))
            {
                ShowError(MessageHelper.Message_ArgumentException);
            }
            this.BindData(workflowid, activityId);
            Search(0, 0);
        }
    }

    private void BindData(string workflowid, string activityid)
    {
        drdlFName.DataSource = activityRulesService.GetFormItemDefinitions(workflowid);
        drdlFName.DataTextField = "Name";
        drdlFName.DataValueField = "FName";
        drdlFName.DataBind();
        drdlFName.Dispose();
        drdlFName.Items.Insert(0, new ListItem("- 请选择 -", ""));
        drdlFName.Items.Insert(1, new ListItem("提单人部门", "提单人部门"));
        drdlFName.Items.Insert(2, new ListItem("提单人姓名", "提单人姓名"));
        drdlFName.Items.Insert(3, new ListItem("当前部门", "当前部门"));
        drdlFName.Items.Insert(4, new ListItem("当前用户姓名", "当前用户姓名"));

        drdlNextActivity.DataSource = activityRulesService.GetNextActivitys(activityid);
        drdlNextActivity.DataTextField = "nextname";
        drdlNextActivity.DataValueField = "nextactid";
        drdlNextActivity.DataBind();
        drdlNextActivity.Dispose();
        drdlNextActivity.Items.Insert(0, new ListItem("- 请选择 -", ""));

        string workflowName = WorkflowUtility.GetWorkflowName(new Guid(workflowid));
        this.WorkflowName = workflowName;
        ActivityDefinition activity = activityDefinitionService.GetActivityDefinition(new Guid(activityid));
        this.WorkflowId = workflowid;
        this.ActivityId = activityid;
        ActivityName = activity.ActivityName;
        ltlActivityName.Text = this.ActivityName;
    }

    protected void Search(int pageIndex, int recordCount)
    {
    
        DataTable source = activityRulesService.GetActivityRules(this.WorkflowId , this.ActivityName, pageIndex, listPager.ItemsPerPage, ref recordCount);
        listResults.DataSource = source;
        listResults.DataBind();

        this.listPager.TotalRecordCount = recordCount;
        this.listPager.DataBind();
    }

    protected void listPager_PageIndexChanged(object sender, Botwave.Web.Controls.PageChangedEventArgs e)
    {
        Search(listPager.TotalRecordCount, e.NewPageIndex);
    }

    protected void btn_Add_Click(object sender, EventArgs e)
    {
        RulesDetail rulesDetail = new RulesDetail();
        rulesDetail.Ruleid = Guid.NewGuid();
        rulesDetail.ActivityName= this.ActivityName;
        rulesDetail.Workflowid = new Guid(this.WorkflowId);
        rulesDetail.NextActivityName = drdlNextActivity.SelectedItem.Text;
        rulesDetail.StepType = 1;
        rulesDetail.ParentRuleId = Guid.Empty;
        txtCommandRules.Text = Botwave.XQP.Commons.XQPHelper.DecodeBase64("utf-8", txtCommandRules.Text);
        //rulesDetail.Conditions = txtCommandRules.Text.Replace("FNumber", "to_number(FNumber)");
        rulesDetail.Conditions = txtCommandRules.Text;
        rulesDetail.Description = txtDescription.Text;
        rulesDetail.Title = this.WorkflowName + "-" + ltlActivityName.Text + "→" + drdlNextActivity.SelectedItem.Text;
        rulesDetail.Status = 1;
        rulesDetail.Creator = CurrentUserName;
        rulesDetail.Createdtime = DateTime.Now;
        rulesDetail.LastModifier = CurrentUserName;
        rulesDetail.LastModtime = DateTime.Now;
        string fieldsStr = rulesDetail.Conditions.Trim();
        foreach (ListItem item in drdlFName.Items)
        {
            if (Regex.IsMatch(fieldsStr,item.Value,RegexOptions.IgnoreCase) && !string.IsNullOrEmpty(item.Value))
                rulesDetail.FieldsAssemble += "_"+item.Value + "_;";
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
            drdlNextActivity.SelectedItem.Selected = false;
            drdlNextActivity.Items.FindByText(info.NextActivityName).Selected = true;
        }
    }
    protected void drdlFName_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataRow itemDataSource = activityRulesService.GetFormItemDataSource(this.WorkflowId, drdlFName.SelectedValue);
        if(itemDataSource==null)
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
}
