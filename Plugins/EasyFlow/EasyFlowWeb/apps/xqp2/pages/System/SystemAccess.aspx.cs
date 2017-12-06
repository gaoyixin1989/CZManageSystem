using System;
using System.Data;
using System.Web.UI.WebControls;
using Botwave.Extension.IBatisNet;
using System.Text;
using Botwave.XQP.API.Service;

public partial class apps_xqp2_pages_System_SystemAccess : Botwave.Security.Web.PageBase
{
    private IWorkflowAPIService workflowAPIService;
    public WorkflowAPIService WorkflowAPIService
    {
        set { workflowAPIService = value; }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataBind();
            //BindWorkFlows();
        }
    }

    private void DataBind()
    {
        string sql = "select Workflowid,WorkflowName from bwwf_workflows where  iscurrent=1 and isdeleted = 0";
        DataTable dt = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];

        drdlWorkflow.DataSource = dt.DefaultView;
        drdlWorkflow.DataTextField = dt.Columns[1].ColumnName;
        drdlWorkflow.DataValueField = dt.Columns[0].ColumnName;
        drdlWorkflow.DataBind();
        drdlWorkflow.Dispose();
        drdlWorkflow.Items.Insert(0, new ListItem("- 请选择 -", ""));
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string workflowid = drdlWorkflow.SelectedValue.ToString();
        string SystemID = txtSystemID.Text;
        string url = txtwebservice.Text;
        if (CheckWorkflow(workflowid) == 0)
        {
            string insertsql = string.Format(@"insert into xqp_SystemAccess (workflowid,systemid,url,createdtime)
values('{0}','{1}','{2}',getdate())", workflowid, SystemID, url);
            IBatisDbHelper.ExecuteNonQuery(CommandType.Text, insertsql);
        }
        else
        {
            //update
            string updatesql = string.Format("update xqp_SystemAccess set SystemID='{0}',Url='{1}',CreatedTime=getdate() where workflowid='{2}'", SystemID, url, workflowid);
            IBatisDbHelper.ExecuteNonQuery(CommandType.Text, updatesql);
        }
        string activityname = "";
        //遍历checkboxlist
        foreach (ListItem li in CheckBoxList1.Items)
        {
            if (li.Selected)
            {
                activityname += "'" + li.Text + "',";
            }
        }
        activityname = activityname.Substring(0, activityname.Length - 1);
        string activitysql = string.Format(@"update bwwf_activities set postHandler='XqpSystemPostHandler' where workflowid='{0}' and activityname in({1})
                                            ", workflowid, activityname);
        IBatisDbHelper.ExecuteNonQuery(CommandType.Text, activitysql);
        string activitysql1 = string.Format(@"update bwwf_activities set postHandler='' where workflowid='{0}' and activityname not in({1})
                                            ", workflowid, activityname);
        IBatisDbHelper.ExecuteNonQuery(CommandType.Text, activitysql1);
        ShowSuccess("保存成功！");
    }


    private int CheckWorkflow(string workflowid)
    {
        string sql = "select count(0) from xqp_SystemAccess where workflowid='" + workflowid + "'";
        return Convert.ToInt32(IBatisDbHelper.ExecuteScalar(CommandType.Text, sql).ToString());
    }

    protected void drdlWorkflow_SelectedIndexChanged(object sender, EventArgs e)
    {
        string workflowid = drdlWorkflow.SelectedValue.ToString();
        if (!string.IsNullOrEmpty(workflowid))
        {
            string sql = @"select activityname, postHandler
 from bwwf_Activities where workflowid='" + workflowid + @"'
 and state <> 2";
            DataTable dt = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
            CheckBoxList1.DataSource = dt;
            CheckBoxList1.DataTextField = dt.Columns[0].ColumnName;
            CheckBoxList1.DataValueField = dt.Columns[1].ColumnName;
            CheckBoxList1.DataBind();
            foreach (ListItem li in CheckBoxList1.Items)
            {
                if (!string.IsNullOrEmpty(li.Value))
                {
                    li.Selected = true;
                }
                else
                    li.Selected = false;
            }

            string sql1 = string.Format("select url,systemId from xqp_SystemAccess where workflowid='{0}'",drdlWorkflow.SelectedValue.ToUpper());
            DataTable dw = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql1).Tables[0];
            if (dw.Rows.Count != 0)
            {
                txtwebservice.Text = dw.Rows[0][0].ToString();
                txtSystemID.Text = dw.Rows[0][1].ToString();
            }
            else
            {
                txtwebservice.Text = "";
                txtSystemID.Text = "";
            }
        }
    }
    
    /*private void BindWorkFlows()
    {
        int index = 0;
        DataTable dt = IBatisDbHelper.ExecuteDataset(CommandType.Text, "select workflowname, workflowid from sz_ef_wftimersetting").Tables[0];
        int _i = 1;
        int _n = 0;
        StringBuilder builder = new StringBuilder();
        bool isChecked = false;
        builder.AppendLine("<table border=\"0\" cellspacing=\"0\" style=\"margin:0; padding:0\">");
        builder.AppendLine("<tr><td><input type=\"checkbox\" id=\"chkAll\" onclick=\"onToggleNotify('chkFlow', this.checked);\"/>全选</td><td></td><td></td><td></td></tr>");
        builder.AppendLine("<tr><td>");
        foreach (DataRow dw in dt.Rows)
        {
            builder.AppendFormat("<input type=\"checkbox\" id=\"chkFlow" + _i.ToString() + "\" name=\"chkFlows\" value=\"" + dw["workflowid"] + "\" /><label for=\"chkFlow" + _i.ToString() + "\">" + dw["workflowname"] + "</label>");
            _n = _i % 5;
            if (_n == 0)
                builder.AppendLine("</td></tr><tr><td>");
            else
                builder.AppendLine("</td><td>");
            _i++;
        }
        builder.AppendLine("</td><td></td><td></td><td></td></tr>");
        //////////////////////
        builder.AppendLine("</table>");
        ltlFlowName.Text = builder.ToString();
    }*/
}

