using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.Commons;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Extension.Util;
using Botwave.Workflow.Extension.Domain;
using Botwave.Workflow.Extension.Service;
using Botwave.DynamicForm.Domain;
using Botwave.DynamicForm.Services;
using Botwave.XQP.Commons;
using Botwave.XQP.Service;
using Botwave.DynamicForm.Extension.Implements;
using Botwave.DynamicForm.Extension.Domain;

public partial class apps_xqp2_pages_workflows_controls_WorkflowBusinessStat : Botwave.Security.Web.UserControlBase
{
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(apps_xqp2_pages_workflows_controls_WorkflowBusinessStat));

    #region service

    private IWorkflowDefinitionService workflowDefinitionService = (IWorkflowDefinitionService)Ctx.GetObject("workflowDefinitionService");
    private IFormInstanceService formInstanceService = (IFormInstanceService)Ctx.GetObject("formInstanceService");
    private IWorkflowResourceService workflowResourceService = (IWorkflowResourceService)Ctx.GetObject("workflowResourceService");
    private IWorkflowReportService workflowReportService = (IWorkflowReportService)Ctx.GetObject("workflowReportService");
    private IDataListDefinitionService dataListDefinitionService = (IDataListDefinitionService)Ctx.GetObject("dataListDefinitionService");
    private IFormDefinitionService formDefinitionService = (IFormDefinitionService)Ctx.GetObject("formDefinitionService");
    private IItemDataListSettingService itemDataListSettingService = (IItemDataListSettingService)Ctx.GetObject("itemDataListSettingService");
    private IDataListInstanceService dataListInstanceService = (IDataListInstanceService)Ctx.GetObject("dataListInstanceService");

    public IWorkflowDefinitionService WorkflowDefinitionService
    {
        get { return workflowDefinitionService; }
        set { workflowDefinitionService = value; }
    }

    public IFormInstanceService FormInstanceService
    {
        get { return formInstanceService; }
        set { formInstanceService = value; }
    }

    public IWorkflowResourceService WorkflowResourceService
    {
        get { return workflowResourceService; }
        set { workflowResourceService = value; }
    }

    public IWorkflowReportService WorkflowReportService
    {
        get { return workflowReportService; }
        set { workflowReportService = value; }
    }

    public IDataListDefinitionService DataListDefinitionService
    {
        set { dataListDefinitionService = value; }
    }

    public IFormDefinitionService FormDefinitionService
    {
        set { formDefinitionService = value; }
    }

    public IItemDataListSettingService ItemDataListSettingService
    {
        set { this.itemDataListSettingService = value; }
    }

    public IDataListInstanceService DataListInstanceService
    {
        set { this.dataListInstanceService = value; }
    }
    #endregion

    private string advanceResource = "A019";

    /// <summary>
    /// 流程名称.
    /// </summary>
    public string WorkflowName
    {
        get { return (string)(ViewState["WorkflowName"]); }
        set { ViewState["WorkflowName"] = value; }
    }

    /// <summary>
    /// 流程名称.
    /// </summary>
    public string UserName
    {
        get { return (string)(ViewState["UserName"]); }
        set { ViewState["UserName"] = value; }
    }

    /// <summary>
    /// 高级权限资源.
    /// </summary>
    public string AdvanceResource
    {
        get { return advanceResource; }
        set { advanceResource = value; }
    }

    public bool EnableAdvanceReport
    {
        get { return ViewState["EnableAdvanceReport"] == null ? false : (bool)ViewState["EnableAdvanceReport"]; }
        set { ViewState["EnableAdvanceReport"] = value; }
    }

    public DataTable DtInfo
    {
        get { return ViewState["DtInfo"] == null ? null : (DataTable)ViewState["DtInfo"]; }
        set { ViewState["DtInfo"] = value; }
    }

    protected IDictionary<string,string> Workflows=new Dictionary<string, string>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            dataBusiness.Style.Add("word-break", "keep-all");
            txtStartDT.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-01");
            txtEndDT.Text = DateTime.Now.ToString("yyyy-MM-dd");

            Botwave.Security.LoginUser user = CurrentUser;
            this.UserName = user.RealName;
            this.EnableAdvanceReport = HasAdvanceReport(user);
            LoadWorkflowData(user);
            LoadData(0, 0);
        }
    }

    private void LoadWorkflowData(Botwave.Security.LoginUser user)
    {
        IList<WorkflowDefinition> workflows = workflowDefinitionService.GetWorkflowDefinitionList();
        
        ddlWorkflowList.Items.Insert(0, new ListItem("---请选择---", ""));
        if (workflows != null && workflows.Count > 0)
        {
            //foreach (WorkflowDefinition definition in workflows)
            //{
            //    string name = definition.WorkflowName;
            //    string alias = definition.WorkflowAlias;
            //    ddlWorkflowList.Items.Add(new ListItem((string.IsNullOrEmpty(alias) ? string.Empty : (alias + "-")) + name, name));
            //}
            DataTable dt = new DataTable();
            dt.Columns.Add("alias");
            dt.Columns.Add("name");
            dt.Columns.Add("wname");
            foreach (WorkflowDefinition definition in workflows)
            {
                string name = definition.WorkflowName;
                string alias = definition.WorkflowAlias;
                //ddlWorkflowList.Items.Add(new ListItem((string.IsNullOrEmpty(alias) ? string.Empty : (alias + "-")) + name, name));
                DataRow dw = dt.NewRow();
                dw[0] = alias;
                dw[1] = name;
                dw[2] = (string.IsNullOrEmpty(alias) ? string.Empty : (alias + "-")) + name;
                dt.Rows.Add(dw);
            }
            DataRow[] dws = dt.Select("", "alias");
            foreach (DataRow dw in dws)
            {
                ddlWorkflowList.Items.Add(new ListItem(DbUtils.ToString(dw[2]), DbUtils.ToString(dw[2])));
            }
        }
        //workflows = WorkflowUtility.GetAllowedWorkflows(workflows, user.Resources, "0002");
        workflows = Botwave.XQP.Util.CZWorkflowUtility.GetAllowedWorkflows(workflows, user.Resources, "0002","0005");
        foreach (WorkflowDefinition w in workflows)
        {
            Workflows.Add(w.WorkflowName,w.WorkflowName);
        }
        this.WorkflowName = ddlWorkflowList.SelectedValue;
    }

    private void LoadData(int recordCount, int pageIndex)
    {
        string startDT = txtStartDT.Text.Trim();
        string endDT = txtEndDT.Text.Trim();
        string textValue = string.Empty;
        string value = string.Empty;
 
        if (!string.IsNullOrEmpty(WorkflowName))
            WorkflowName = WorkflowName.IndexOf("-") > -1 ? WorkflowName.Substring(3) : WorkflowName;
        string owner = (this.EnableAdvanceReport ? null : this.UserName);
        
        if (!this.EnableAdvanceReport)
            owner = AllowReport(WorkflowName) ? null : this.UserName;
       
        DataTable dtBusiness = workflowReportService.GetWorkflowBusinessPager(owner, WorkflowName, startDT, endDT,txtDepts.Value,ddlState.SelectedValue, pageIndex, listPager.ItemsPerPage, ref recordCount);
        if (dtBusiness == null)
            return;
        //附加表单项实例数据
        foreach (DataRow row in dtBusiness.Rows)
        {
            if (string.IsNullOrEmpty(Botwave.Commons.DbUtils.ToString(row["WorkflowInstanceId"])))
                continue;
            Guid instanceId = new Guid(Botwave.Commons.DbUtils.ToString(row["WorkflowInstanceId"]));
            IList<FormItemInstance> itemInstanceList = formInstanceService.GetFormInstanceById(instanceId, true).Items;
            foreach (FormItemInstance itemInstance in itemInstanceList)
            {
                if (itemInstance.Definition == null)
                    continue;
                //if (itemInstance.Definition.ItemType.Equals(FormItemDefinition.FormItemType.Html)) continue;
                if (itemInstance.Definition.ItemType.Equals(FormItemDefinition.FormItemType.Html))
                {
                    if (dataListDefinitionService.GetDataListItemDefinitionsByFormItemDefinitionId(itemInstance.Definition.Id).Count > 0)
                    {
                        if (!dtBusiness.Columns.Contains(itemInstance.Definition.Name))
                        {
                            dtBusiness.Columns.Add(new DataColumn(itemInstance.Definition.Name));
                            row[itemInstance.Definition.Name] = string.Format("<a href=\"#\" onclick=\"OpenSelectionDialog('{0}','{1}')\">查看</a>", instanceId, itemInstance.Definition.Id);
                            continue;
                        }
                        else
                        {
                            row[itemInstance.Definition.Name] = string.Format("<a href=\"#\" onclick=\"OpenSelectionDialog('{0}','{1}')\">查看</a>", instanceId, itemInstance.Definition.Id);
                            continue;
                        }
                    }
                    continue;
                }
                if (!dtBusiness.Columns.Contains(itemInstance.Definition.Name))
                {
                    dtBusiness.Columns.Add(new DataColumn(itemInstance.Definition.Name));
                }
                textValue = (itemInstance.TextValue == null ? "" : itemInstance.TextValue);
                value = (itemInstance.Value == null ? "" : itemInstance.Value);
                row[itemInstance.Definition.Name] = itemInstance.Definition.ItemDataType == FormItemDefinition.DataType.Text ? textValue : value;
            }
        }
        DtInfo = dtBusiness;

        dataBusiness.DataSource = dtBusiness;
        dataBusiness.DataBind();

        listPager.TotalRecordCount = recordCount;
        listPager.DataBind();
    }

    private DataTable LoadDataToExcel(int recordCount, int pageIndex)
    {
        log.Info(DateTime.Now + "--->WorkflowReportService = " + workflowReportService);
        log.Info(DateTime.Now + "--->DataListDefinitionService = " + dataListDefinitionService);
        log.Info(DateTime.Now + "--->formDefinitionService = " + formDefinitionService);
        string startDT = txtStartDT.Text.Trim();
        string endDT = txtEndDT.Text.Trim();
        string textValue = string.Empty;
        string value = string.Empty;
        if (!string.IsNullOrEmpty(WorkflowName))
            WorkflowName = WorkflowName.IndexOf("-") > -1 ? WorkflowName.Substring(3) : WorkflowName;
        string owner = (this.EnableAdvanceReport ? null : this.UserName);
        if (!this.EnableAdvanceReport)
            owner = AllowReport(WorkflowName) ? null : this.UserName;
        DataTable wfFormDt = workflowReportService.LoadWorkflowFormData(owner, WorkflowName, startDT, endDT, txtDepts.Value, ddlState.SelectedValue, 0, 0, ref recordCount);

        return wfFormDt;
    }

    public DataTable LoadDataAll()
    {
        log.Info(DateTime.Now + "--->WorkflowReportService = " + workflowReportService);
        log.Info(DateTime.Now + "--->DataListDefinitionService = " + dataListDefinitionService);
        log.Info(DateTime.Now + "--->formDefinitionService = " + formDefinitionService);
        string startDT = txtStartDT.Text.Trim();
        string endDT = txtEndDT.Text.Trim();
        string textValue = string.Empty;
        string value = string.Empty;
        if (!string.IsNullOrEmpty(WorkflowName))
            WorkflowName = WorkflowName.IndexOf("-") > -1 ? WorkflowName.Substring(3) : WorkflowName;
        string owner = (this.EnableAdvanceReport ? null : this.UserName);
        if (!this.EnableAdvanceReport)
            owner = AllowReport(WorkflowName) ? null : this.UserName;
        int recordCount = -1;

        DataTable wfFormDt = workflowReportService.LoadWorkflowFormData(owner, WorkflowName, startDT, endDT, txtDepts.Value, ddlState.SelectedValue, null, null, ref recordCount);

        return wfFormDt;
    }

    /// <summary>
    /// 是否允许统计该流程的所有工单
    /// </summary>
    /// <param name="workflowName"></param>
    /// <returns></returns>
    private bool AllowReport(string workflowName)
    {
        IList<WorkflowDefinition> workflows = workflowDefinitionService.GetWorkflowDefinitionList();
        if (workflows != null && workflows.Count > 0)
        {
            IList<WorkflowDefinition> allowWorkflows = WorkflowUtility.GetAllowedWorkflows(workflows, CurrentUser.Resources, "0005");
            //log.Info("aCount:" + allowWorkflows.Count);
            foreach (WorkflowDefinition item in allowWorkflows)
            {
                if (item.WorkflowName == workflowName)
                {
                    //log.Info("aCountTrue:" + workflowName);
                    return true;
                }
            }
        }
        return false;
    }

    protected void listPager_PageIndexChanged(object sender, Botwave.Web.Controls.PageChangedEventArgs e)
    {
        LoadData(listPager.TotalRecordCount, e.NewPageIndex);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        WorkflowName = ddlWorkflowList.SelectedValue;
        this.listPager.CurrentPageIndex = -1;
        LoadData(0, 0);
    }

    protected void dataBusiness_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        e.Item.Cells[0].Visible = false;
    }

    /// <summary>
    /// 导出为 Excel 文件.
    /// </summary>
    public void ExportExcel()
    {
        string fileName = (this.ddlWorkflowList.SelectedItem == null ? this.ddlWorkflowList.SelectedValue : this.ddlWorkflowList.SelectedItem.Text);
        fileName = fileName + "业务报表";
        DataGrid grid = new DataGrid();
        grid.CssClass = "tblGrayClass";
        grid.ItemDataBound += new DataGridItemEventHandler(dataBusiness_ItemDataBound);
        grid.DataSource = LoadDataToExcel(0, 0);
        grid.DataBind();
        if (grid.Items.Count > 0)
        {
            Botwave.XQP.Commons.XQPHelper.ExportExcel(grid, fileName);
        }
    }

    /// <summary>
    /// 导出所有数据为 Excel 文件.
    /// </summary>
    public void ExportExcelAll()
    {
        string fileName = (this.ddlWorkflowList.SelectedItem == null ? this.ddlWorkflowList.SelectedValue : this.ddlWorkflowList.SelectedItem.Text);
        fileName = fileName + "业务报表";
        DataGrid grid = new DataGrid();
        grid.CssClass = "tblGrayClass";
        grid.ItemDataBound += new DataGridItemEventHandler(dataBusiness_ItemDataBound);
        grid.DataSource = LoadDataAll();
        grid.DataBind();
        if (grid.Items.Count > 0)
        {
            Botwave.XQP.Commons.XQPHelper.ExportExcel(grid, fileName);
        }
    }

    protected bool HasAdvanceReport(Botwave.Security.LoginUser user)
    {
        if (user == null)
            return false;
        if (string.IsNullOrEmpty(this.advanceResource))
            return true;

        bool result = false;
        if (user.Properties.ContainsKey("Report_Advance_Enable"))
        {
            result = Convert.ToBoolean(user.Properties["Report_Advance_Enable"]);
        }
        else
        {
            result = user.Resources.ContainsKey(this.advanceResource);
            user.Properties["Report_Advance_Enable"] = result;
        }
        return result;
    }


}
