using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using Botwave.Workflow.Service;
using Botwave.Workflow.Domain;
using Botwave.XQP.Domain;
using Botwave.GMCCServiceHelpers;
using Botwave.Web.Controls;

public partial class apps_xqp2_pages_workflows_report_ReportTimeRataState : Botwave.Security.Web.PageBase
{
    private IWorkflowDefinitionService workflowDefinitionService = (IWorkflowDefinitionService)Ctx.GetObject("workflowDefinitionService");

    public IWorkflowDefinitionService WorkflowDefinitionService
    {
        set { workflowDefinitionService = value; }
    }

    public int DtCount
    {
        get { return ViewState["DtCount"] == null ? 0 : (int)ViewState["DtCount"]; }
        set { ViewState["DtCount"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadWorkflowData();
            txtStart.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            //txtEnd.Text = DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Month).ToString() + "-" + "1";
            //DateTime endTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-mm-dd"));
            txtEnd.Text = DateTime.Now.ToString("yyyy-MM-dd");
            Rept_Databind(0, 0);
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        //this.Rept_Databind(0, 0);
        Response.Redirect("ReportTimeRataState.aspx?Start=" + Server.UrlEncode(txtStart.Text.Trim()) + "&End=" + Server.UrlEncode(txtEnd.Text.Trim()) + "&SheetId=" + Server.UrlEncode(txtSheetId.Text.Trim()) + "&wf=" + Server.UrlEncode(ddlWorkflowList.SelectedValue));
    }

    private void LoadWorkflowData()
    {
        IList<WorkflowDefinition> workflows = workflowDefinitionService.GetWorkflowDefinitionList();
        ddlWorkflowList.Items.Clear();
        ddlWorkflowList.Items.Insert(0, new ListItem("－ 全部 －", ""));
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
        this.ddlWorkflowList.SelectedValue = Request.QueryString["wf"];
    }

    public void Rept_Databind(int recordCount, int pageIndex)
    {
        string Start = Request.QueryString["Start"];
        string End = Request.QueryString["End"];
        string SheetId = Request.QueryString["SheetId"];
        string wname = Request.QueryString["wf"];
        if (!string.IsNullOrEmpty(wname))
            wname = wname.IndexOf("-") > -1 ? wname.Substring(3) : wname;
        StringBuilder sb = new StringBuilder();
        if (!string.IsNullOrEmpty(Start) && !string.IsNullOrEmpty(End))
        {
            txtStart.Text = Start = string.IsNullOrEmpty(Start) ? Server.UrlDecode(txtStart.Text.Trim()) : Start;
            txtEnd.Text = End = string.IsNullOrEmpty(End) ? Server.UrlDecode(txtEnd.Text.Trim()) : End;
            sb.AppendFormat(" createdtime between convert(datetime,'{0} 00:00:00') and convert(datetime,'{1} 23:59:59')", Start, End);
            txtSheetId.Text = SheetId = string.IsNullOrEmpty(SheetId) ? Server.UrlDecode(txtSheetId.Text.Trim()) : SheetId;
            if (!string.IsNullOrEmpty(SheetId))
                sb.AppendFormat(" and sheetid like '%{0}%'", SheetId);
            if (ddlWorkflowList.SelectedIndex > 0)
                sb.AppendFormat(" and workflowname='{0}'", wname);
            DataTable source = SZIntelligentRemind.GetTimeRateReprotAll(sb.ToString(), pageIndex, listPagerTodoTask.ItemsPerPage, ref recordCount);

            if (source != null)
            {
                DtCount = recordCount;
                foreach (DataRow dw in source.Rows)
                {
                    if (string.IsNullOrEmpty(DbUtils.ToString(dw["finishedtime"])))
                        dw["hours"] = Math.Round(((float)((DateTime.Now - Convert.ToDateTime(dw["createdtime"])).Days * 24 * 60 + (DateTime.Now - Convert.ToDateTime(dw["createdtime"])).Hours * 60 + (DateTime.Now - Convert.ToDateTime(dw["createdtime"])).Minutes) / 60),2);
                    else
                        dw["hours"] = Math.Round(((float)((Convert.ToDateTime(dw["finishedtime"]) - Convert.ToDateTime(dw["createdtime"])).Days * 24 * 60 + (Convert.ToDateTime(dw["finishedtime"]) - Convert.ToDateTime(dw["createdtime"])).Hours * 60 + (Convert.ToDateTime(dw["createdtime"]) - Convert.ToDateTime(dw["createdtime"])).Minutes) / 60),2);
                    if (DbUtils.ToString(dw["stayhours"]) == "-1")
                        dw["istimeout"] = "未超时";
                    //else
                    //{
                    //    if (Convert.ToDouble(dw["hours"]) > Convert.ToDouble(dw["stayhours"]))
                    //        dw["istimeout"] = "已超时";
                    //    else
                    //        dw["istimeout"] = "未超时";
                    //}
                }
                Repeater1.DataSource = source;
                Repeater1.DataBind();
            }
        }

        this.listPagerTodoTask.TotalRecordCount = recordCount;
        this.listPagerTodoTask.DataBind();
    }

    protected void listTodoPager_PageIndexChanged(object sender, PageChangedEventArgs e)
    {
        this.Rept_Databind(listPagerTodoTask.TotalRecordCount, e.NewPageIndex);
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        string Start = Request.QueryString["Start"];
        string End = Request.QueryString["End"];
        string SheetId = Request.QueryString["SheetId"];
        string wname = Request.QueryString["wf"];
        if (!string.IsNullOrEmpty(wname))
            wname = wname.IndexOf("-") > -1 ? wname.Substring(3) : wname;
        StringBuilder sb = new StringBuilder();
        int recordCount = 0;
        if (!string.IsNullOrEmpty(Start) && !string.IsNullOrEmpty(End) && DtCount > 0)
        {
            txtStart.Text = Start = string.IsNullOrEmpty(Start) ? Server.UrlDecode(txtStart.Text.Trim()) : Start;
            txtEnd.Text = End = string.IsNullOrEmpty(End) ? Server.UrlDecode(txtEnd.Text.Trim()) : End;
            sb.AppendFormat(" createdtime between convert(datetime,'{0} 00:00:00') and convert(datetime,'{1} 23:59:59')", Start, End);
            txtSheetId.Text = SheetId = string.IsNullOrEmpty(SheetId) ? Server.UrlDecode(txtSheetId.Text.Trim()) : SheetId;
            if (!string.IsNullOrEmpty(SheetId))
                sb.AppendFormat(" and sheetid like '%{0}%'", SheetId);
            if (ddlWorkflowList.SelectedIndex > 0)
                sb.AppendFormat(" and workflowname='{0}'", wname);
            DataTable source = SZIntelligentRemind.GetTimeRateReprotAll(sb.ToString(), 0, DtCount, ref recordCount);

            if (source != null)
            {
                foreach (DataRow dw in source.Rows)
                {
                    if (string.IsNullOrEmpty(DbUtils.ToString(dw["finishedtime"])))
                        dw["hours"] = ((float)((DateTime.Now - Convert.ToDateTime(dw["createdtime"])).Days * 24 * 60 + (DateTime.Now - Convert.ToDateTime(dw["createdtime"])).Hours * 60 + (DateTime.Now - Convert.ToDateTime(dw["createdtime"])).Minutes) / 60).ToString("0.00");
                    else
                        dw["hours"] = ((float)((DateTime.Now - Convert.ToDateTime(dw["createdtime"])).Days * 24 * 60 + (DateTime.Now - Convert.ToDateTime(dw["finishedtime"])).Hours * 60 + (DateTime.Now - Convert.ToDateTime(dw["createdtime"])).Minutes) / 60).ToString("0.00");
                    if (DbUtils.ToString(dw["stayhours"]) == "-1")
                        dw["istimeout"] = "未超时";
                    //else
                    //{
                    //    if (Convert.ToDouble(dw["hours"]) > Convert.ToDouble(dw["stayhours"]))
                    //        dw["istimeout"] = "已超时";
                    //    else
                    //        dw["istimeout"] = "未超时";
                    //}
                }
                source.Columns["workflowname"].ColumnName = "流程名称";
                source.Columns["sheetid"].ColumnName = "工单受理号";
                source.Columns["title"].ColumnName = "工单标题";
                source.Columns["activityname"].ColumnName = "处理步骤";
                source.Columns["realname"].ColumnName = "处理人";
                source.Columns["createdtime"].ColumnName = "步骤开始时间";
                source.Columns["finishedtime"].ColumnName = "步骤结束时间";
                source.Columns["hours"].ColumnName = "处理时长";
                source.Columns["stayhours"].ColumnName = "允许滞留时间";
                //source.Columns["istimeout"].ColumnName = "是否超时";
                DataGrid grid = new DataGrid();
                grid.CssClass = "tblGrayClass";
                grid.ItemDataBound += new DataGridItemEventHandler(dataBusiness_ItemDataBound);
                grid.DataSource = source;
                grid.DataBind();
                Botwave.XQP.Commons.XQPHelper.ExportExcel(grid, "智能提醒统计报表");
            }
        }
    }

    protected void dataBusiness_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        e.Item.Cells[0].Visible = false;
        e.Item.Cells[1].Visible = false;
    }
}