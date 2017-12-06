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
using Botwave.Report.Common;
using System.Collections.Generic;

public partial class ReportConfig : Botwave.Security.Web.PageBase
{
    //配置信息
    IDictionary<string, ShowField> fieldsDict = new Dictionary<string, ShowField>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindPageData();
        }
    }
    private void BindPageData()
    {
        //从数据库中读取配置信息
        IList<ShowField> fields = ReportConfigDAL.ListReportData();
        foreach (ShowField field in fields)
        {
            fieldsDict.Add(field.ObjName, field);
        }
        rptConfig.DataSource = ReportConfigDAL.GetAllTableAndView().Tables[0];
        rptConfig.DataBind();
    }

    protected void rptConfig_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            Label lblName = e.Item.FindControl("lblName") as Label;
            if (lblName != null && lblName.Text.Length > 0)
            {
                CheckBox cb = e.Item.FindControl("CheckBox1") as CheckBox;
                if (cb != null && fieldsDict.ContainsKey(lblName.Text))
                    cb.Checked = true;

                TextBox txt = e.Item.FindControl("TextBox1") as TextBox;
                if (txt != null && fieldsDict.ContainsKey(lblName.Text))
                {
                    txt.Text = fieldsDict[lblName.Text].Description;
                }

                Repeater repeater = e.Item.FindControl("rptTableData") as Repeater;
                if (repeater != null)
                {
                    repeater.DataSource = ReportConfigDAL.GetColumnsNames(lblName.Text).Tables[0];
                    repeater.DataBind();
                }
            }

            Label lblTye = e.Item.FindControl("lblType") as Label;
            lblTye.Text = lblTye.Text.Trim() == "U" ? "表" : "视图";
        }
    }
    protected void rptTableData_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            string coloumnName = ((Label)e.Item.FindControl("lblColumnName")).Text;
            string tableName = ((Label)e.Item.FindControl("lblTableName")).Text;

            if (fieldsDict.ContainsKey(tableName))
            {
                CheckBox cb = e.Item.FindControl("CheckBox2") as CheckBox;
                if (cb != null && fieldsDict[tableName].Columns.ContainsKey(coloumnName))
                    cb.Checked = true;

                TextBox txt = e.Item.FindControl("TextBox2") as TextBox;
                if (txt != null && fieldsDict[tableName].Columns.ContainsKey(coloumnName))
                {
                    txt.Text = fieldsDict[tableName].Columns[coloumnName];
                }
            }
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        IList<ShowField> fields = new List<ShowField>();
        foreach (RepeaterItem item in rptConfig.Items)
        {
            if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
            {
                if (((CheckBox)item.FindControl("CheckBox1")).Checked)
                {
                    ShowField field = new ShowField();
                    Label lblName = item.FindControl("lblName") as Label;
                    TextBox txt = item.FindControl("TextBox1") as TextBox;
                    field.ObjName = lblName.Text;
                    field.Description = txt.Text;

                    Repeater repeaterColumn = item.FindControl("rptTableData") as Repeater;
                    foreach (RepeaterItem columnItem in repeaterColumn.Items)
                    {
                        if (((CheckBox)columnItem.FindControl("CheckBox2")).Checked)
                        {
                            Label lblColunm = columnItem.FindControl("lblColumnName") as Label;
                            TextBox txt2 = columnItem.FindControl("TextBox2") as TextBox;
                            field.Columns.Add(lblColunm.Text, txt2.Text);
                        }
                    }
                    fields.Add(field);
                }
            }
        }

        //保存到XML文件
        //ReportParser.Save(fields);

        //保存到SQL数据库
        ReportConfigDAL.SaveReportData(fields);
    }
}
