using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Botwave.Commons;
using Botwave.XQP.Domain;

public partial class apps_czmcc_pages_WorkflowInterface : Botwave.Web.PageBase
{
    protected int rowIndex = 1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Bind();
        }
    }

    private void Bind()
    {
        IList<CZWorkflowInterface> items = CZWorkflowInterface.GetAll();
        this.rptList.DataSource = items;
        this.rptList.DataBind();
    }

    protected string FormatStatus(object value)
    {
        int status = DbUtils.ToInt32(value);

        return status == 1 ? "<span style='color:green'>启用</span>" : "<span style='color:red'>禁用</span>";
    }
}
