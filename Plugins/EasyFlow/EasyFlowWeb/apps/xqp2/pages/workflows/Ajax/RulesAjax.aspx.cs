using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Botwave.Workflow.Routing.Implements;
using System.Text;

public partial class apps_xqp2_pages_workflows_Ajax_RulesAjax : Botwave.XQP.Web.Security.PageBase
{
    public string htmlStr;
    private IActivityRulesService activityRulesService = (IActivityRulesService)Ctx.GetObject("activityRulesService");

    public IActivityRulesService ActivityRulesService
    {
        set { activityRulesService = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string workflowId=Context.Request["wid"];
            string fname = Context.Request["fname"];
            DataRow itemDataSource = activityRulesService.GetFormItemDataSource(workflowId,fname);
            if (itemDataSource == null)
                htmlStr = "<input type='text' id='txtVal' />";
            else if (itemDataSource["itemType"].ToString() == "2" || itemDataSource["itemType"].ToString() == "4" || itemDataSource["itemType"].ToString() == "5")
            {
                StringBuilder strDataSource = new StringBuilder(@"<select id='txtVal'><option value='' selected = 'selected'>- 请选择 -</option>");
                string[] arr = itemDataSource["datasource"].ToString().Split(',');
                foreach (string str in arr)
                {
                    strDataSource.Append("<option value='" + str + "'>" + str + "</option>");
                }
                strDataSource.Append("</select>");
                htmlStr = strDataSource.ToString();
            }
            else
            {
                htmlStr = "<input type='text' id='txtVal' />";
            }
        }
    }
}