using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Botwave.Workflow.Extension.Service;

public partial class apps_czmcc_pages_WorkflowRelationPopup : Botwave.Web.PageBase
{
    protected int RowIndex = 1;

    private IWorkflowSearcher workflowSearcher = (IWorkflowSearcher)Ctx.GetObject("workflowSearcher");

    public IWorkflowSearcher WorkflowSearcher
    {
        set { workflowSearcher = value; }
    }

    public string UserName
    {
        get { return (string)ViewState["UserName"]; }
        set { ViewState["UserName"] = value; }
    }

    public string Rel
    {
        get { return ViewState["rel"] == null ? string.Empty : (string)ViewState["rel"]; }
        set { ViewState["rel"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.UserName = Botwave.Security.LoginHelper.UserName;
            this.Rel = Server.HtmlEncode(Request.QueryString["rel"]);
            Bind(0, 0);
        }
    }

    public void Bind(int recordCount, int pageIndex)
    {
        string keywords = this.txtKeywords.Text.Trim();
        //DataTable data = Botwave.XQP.Domain.CZWorkflowRelation.GetWorkflowInstances(this.UserName, keywords, pageIndex, this.listPager.ItemsPerPage, ref recordCount);
        AdvancedSearchCondition condition = new AdvancedSearchCondition();
        condition.BeginTime = "2008-01-01";
        condition.EndTime = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
        condition.ProcessorName = this.UserName;
        condition.Keywords = keywords;
        condition.OrderField = "finishedtime desc";
        int refCount = 0;
        DataTable data = workflowSearcher.Search(condition,0,20,ref refCount);
        this.rptList.DataSource = data;
        this.rptList.DataBind();

        this.listPager.TotalRecordCount = recordCount;
        this.listPager.DataBind();
    }

    protected void listPager_PageIndexChanged(object sender, Botwave.Web.Controls.PageChangedEventArgs e)
    {
        this.Bind(listPager.TotalRecordCount, e.NewPageIndex);
    }

    protected void buttonOK_Click(object sender, EventArgs e)
    {
        this.listPager.CurrentPageIndex = -1;
        Bind(0, 0);
    }
}
