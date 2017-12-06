using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text;
using Botwave.Commons;
using Botwave.Report.Common;
using System.Collections.Generic;
using Botwave.Report.DataAccess;
using Botwave.Report;

public partial class contrib_report_pages_DrawGrapConfig : Botwave.Security.Web.PageBase
{
    protected string GrapItemID
    {
        get { return rblType.SelectedValue; }
    }
    private int ReportID
    {
        get
        {
            return DbUtils.ToInt32(Request.QueryString["id"], 0);
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindPage(ReportID);
        }
    }
    private void BindPage(int id)
    {
        ReportEntity report = ReportDAL.GetReportByID(id);
        IList<ReportItem> ri = new List<ReportItem>();

        if (report.SourceType == 1)
        {
            ri = ReportDAL.GetReportItemByReportID(id);
        }
        else if (report.SourceType == 2)
        {
            DataSet ds = ReportDAL.ExecuteReportSql(report.ReportSql);
            foreach (DataColumn dc in ds.Tables[0].Columns)
            {
                ReportItem r = new ReportItem();
                r.Field = dc.ColumnName;
                r.FieldName = dc.ColumnName;
                ri.Add(r);
            }

        }
        else if (report.SourceType == 3)
        {
            DataTable dtemp = ReportItemDAL.GetListByReportID(ReportID).Tables[0];
            IDictionary<string, string> para = new Dictionary<string, string>();
            foreach (DataRow dr in dtemp.Rows)
            {
                para.Add(dr["Parameter"].ToString(), string.Format("{0}|{1}", dr["DataType"].ToString(), dr["DefaultValue"].ToString()));
            }
            DataTable dt = ReportViewDAL.GetDataSetBySP(report.ReportSql, para).Tables[0];
            foreach (DataColumn dc in dt.Columns)
            {
                ReportItem r = new ReportItem();
                r.Field = dc.ColumnName;
                r.FieldName = dc.ColumnName;
                ri.Add(r);
            }
        }
        ddlXFieldName.DataSource = ri;
        ddlXFieldName.DataBind();

        cblYFieldNames.DataSource = ri;
        cblYFieldNames.DataBind();

        ddlDataField.DataSource = ri;
        ddlDataField.DataBind();

        ddlTagField.DataSource = ri;
        ddlTagField.DataBind();

        cblYBar.DataSource = ri;
        cblYBar.DataBind();

        cblYLine.DataSource = ri;
        cblYLine.DataBind();

        BindList();
    }
    private void BindList()
    {
        dgList.DataSource = DrawGrapDAL.ListReportItem(ReportID);
        dgList.DataBind();
    }
    protected void dgList_DeleteCommand(object source, DataGridCommandEventArgs e)
    {
        int gid = DbUtils.ToInt32(dgList.DataKeys[e.Item.ItemIndex]);
        string script = "";
        if (DrawGrapDAL.Delete(gid) > 0)
            script = "<script>alert('删除成功')</script>";
        else
            script = "<script>alert('删除失败')</script>";
        Page.RegisterClientScriptBlock(Guid.NewGuid().ToString(), script);

        BindList();
    }

    protected void dgList_EditCommand(object source, DataGridCommandEventArgs e)
    {
        int gid = DbUtils.ToInt32(dgList.DataKeys[e.Item.ItemIndex]);

        BindGrapItem(gid);
    }
    private void BindGrapItem(int itemID)
    {
        GrapConfig gc = DrawGrapDAL.GetModel(itemID);

        txtHeight.Text = gc.Height.Value.ToString();
        txtTitle.Text = gc.Title;
        rblType.SelectedValue = gc.Type.ToString();
        txtWidth.Text = gc.Width.Value.ToString();
        ddlXFieldName.SelectedValue = gc.XFieldName;
        txtXTitle.Text = gc.XTitle;
        ddlXType.SelectedValue = gc.XType.Value.ToString();

        txtYTitle.Text = gc.YTitle;

        if (gc.Type == 2)
        {
            ddlTagField.SelectedValue = gc.TagNames;
            ddlDataField.SelectedValue = gc.XFieldName;
        }
        else if (gc.Type == 3)
        {
            //绑定数据到柱线图的柱图
            foreach (string s in gc.YFieldNames.Split(','))
                foreach (ListItem li in cblYBar.Items)
                    if (li.Text.Equals(s))
                        li.Selected = true;

            //绑定数据到柱线图的线图
            foreach (string s in gc.TagNames.Split(','))
                foreach (ListItem li in cblYLine.Items)
                    if (li.Text.Equals(s))
                        li.Selected = true;
        }
        else
        {
            foreach (string s in gc.YFieldNames.Split(','))
                foreach (ListItem li in cblYFieldNames.Items)
                    if (li.Text.Equals(s))
                        li.Selected = true;
        }

        btnSave.CommandName = gc.ID.ToString();
        btnSave.Text = "修改";
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        GrapConfig gc = new GrapConfig();
        gc.Height = DbUtils.ToInt32(txtHeight.Text, 0);
        gc.ReportID = ReportID;
        gc.Title = txtTitle.Text;
        gc.Type = DbUtils.ToInt32(rblType.SelectedValue);
        gc.Width = DbUtils.ToInt32(txtWidth.Text);

        if (gc.Type == 2)
        {
            gc.XFieldName = ddlDataField.SelectedValue;
            gc.YFieldNames = ddlTagField.SelectedValue;
            gc.TagNames = ddlTagField.SelectedValue;
        }
        else
        {
            gc.XFieldName = ddlXFieldName.SelectedValue;
            gc.XTitle = txtXTitle.Text;
            gc.YTitle = txtYTitle.Text;
            gc.XType = DbUtils.ToInt32(ddlXType.SelectedValue);

            if (gc.Type == 3)
            {
                //保持柱线图的柱图
                StringBuilder yfields = new StringBuilder();
                foreach (ListItem li in cblYBar.Items)
                    if (li.Selected)
                        yfields.AppendFormat(",{0}", li.Text);
                if (yfields.Length > 1)
                    yfields.Remove(0, 1);
                gc.YFieldNames = yfields.ToString();

                //保持柱线图的线图
                yfields = new StringBuilder();
                foreach (ListItem li in cblYLine.Items)
                    if (li.Selected)
                        yfields.AppendFormat(",{0}", li.Text);
                if (yfields.Length > 1)
                    yfields.Remove(0, 1);
                gc.TagNames = yfields.ToString();
            }
            else
            {
                StringBuilder yfields = new StringBuilder();
                foreach (ListItem li in cblYFieldNames.Items)
                    if (li.Selected)
                        yfields.AppendFormat(",{0}", li.Text);

                if (yfields.Length > 1)
                    yfields.Remove(0, 1);

                gc.YFieldNames = yfields.ToString();
                gc.TagNames = yfields.ToString();
            }
        }
        gc.ID = DbUtils.ToInt32(btnSave.CommandName, 0);

        string script = "";
        if (DrawGrapDAL.Save(gc) > 0)
            script = "<script>alert('保存成功')</script>";
        else
            script = "<script>alert('保存失败')</script>";
        Page.RegisterClientScriptBlock(Guid.NewGuid().ToString(), script);

        BindList();
    }
    protected void dgList_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemIndex > -1)
        {
            LinkButton deleteButton = (LinkButton)e.Item.Cells[4].Controls[0];
            deleteButton.Attributes["OnClick"] = "return confirm('你确认要删除吗？')";
        }
    }
}
