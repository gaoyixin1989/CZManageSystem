using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Botwave.XQP.Domain;
using Botwave.Web;
using System.Text;
using Botwave.Workflow.Service;
using Botwave.Workflow.Domain;

public partial class apps_xqp2_pages_workflows_config_ConfigActivityRemark : Botwave.Security.Web.PageBase
{
    private IActivityDefinitionService activityDefinitionService = (IActivityDefinitionService)Ctx.GetObject("activityDefinitionService");
    public IActivityDefinitionService ActivityDefinitionService
    {
        set { activityDefinitionService = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string wfid = Request.QueryString["wid"];
            if (string.IsNullOrEmpty(wfid))
            {
                ShowError(MessageHelper.Message_ArgumentException);
            }
            this.LoadData(new Guid(wfid));
        }
    }

    private void LoadData(Guid workflowid)
    {
        IList<ActivityDefinition> profiles = activityDefinitionService.GetActivitiesByWorkflowId(workflowid);
        WorkflowProfile wprofile = WorkflowProfile.LoadByWorkflowId(workflowid);
        profiles = profiles.Where(p =>  p.State < 2).ToList();
        StringBuilder str = new StringBuilder();
        int index = 0;
        foreach (ActivityDefinition profile in profiles)
        {
            str.AppendFormat("<tr><th style=\"font-weight:bold\">{0}</th><td><div><iframe id = \"Iframe{1}\" name = \"Iframe{1}\" src=\"{2}\" frameborder=\"0\" width=\"100%\" scrolling=\"no\" onload=\"this.height=Iframe{1}.document.body.scrollHeight\"></iframe></div></td></tr>\n", profile.ActivityName, index, string.Format(AppPath + "apps/xqp2/pages/workflows/config/ActivityRemark.aspx?aid={0}&act={1}&wname={2}", profile.ActivityId, Server.UrlEncode(profile.ActivityName), Server.UrlEncode(wprofile.WorkflowName)));
            index++;
        }
        ltlActivity.Text = str.ToString();
    }
}
