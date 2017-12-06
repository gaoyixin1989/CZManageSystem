using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Botwave.Workflow.Service;
using Botwave.Workflow.Domain;
using Botwave.XQP.Designer;
using System.Text;
using Botwave.XQP.Domain;
//using Newtonsoft.Json;

public partial class apps_xqp2_pages_workflows_designer_Flowdesign : Botwave.Security.Web.PageBase
{
    #region service interfaces

    private IActivityDefinitionService activityDefinitionService = (IActivityDefinitionService)Ctx.GetObject("activityDefinitionService");
    private IActivitySetService activitySetService = (IActivitySetService)Ctx.GetObject("activitySetService");
    private IWorkflowSettingService workflowSettingService = (IWorkflowSettingService)Ctx.GetObject("workflowSettingService");

    public IActivityDefinitionService ActivityDefinitionService
    {
        set { activityDefinitionService = value; }
    }

    public IActivitySetService ActivitySetService
    {
        set { activitySetService = value; }
    }

    public IWorkflowSettingService WorkflowSettingService
    {
        set { workflowSettingService = value; }
    }
    #endregion

    protected static string RedirectUrl = Botwave.Web.WebUtils.GetAppPath() + "apps/xqp2/pages/workflows/config/ConfigWorkflow.aspx?wid=";

    public string WorkflowKey
    {
        get { return (string)ViewState["WorkflowKey"]; }
        set { ViewState["WorkflowKey"] = value; }
    }
    public string ProcessData
    {
        get { return (string)ViewState["ProcessData"]; }
        set { ViewState["ProcessData"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["wid"] != null)
            {
                this.WorkflowKey = Server.HtmlEncode(this.Request.QueryString["wid"]);
                //WorkflowProfile profile = WorkflowProfile.LoadByWorkflowId(new Guid(this.WorkflowKey));
                Guid workflowId = new Guid(this.WorkflowKey);
                workflowProfileModal.WorkflowId = workflowId;
                WorkflowSetting setting = workflowSettingService.GetWorkflowSetting(workflowId);
                if (setting == null)
                {
                    setting = new WorkflowSetting();
                    setting.WorkflowName = "新建流程";
                }
                    ltlFlowName.Text = setting.WorkflowName;
                workflowProfileModal.LoadWorkflow(setting);
                workflowProfileModal.LoadWorkflowProfile(workflowId);
                IList<WorkflowActivity> activityDefinitionList = WorkflowActivity.GetActivities(new Guid(this.WorkflowKey));
                StringBuilder sb = new StringBuilder();
                sb.Append("{ \"total\": " + activityDefinitionList.Count + ", \"flow_id\": \"" + this.WorkflowKey + "\",\"list\": [");
                if (activityDefinitionList.Count == 0)
                {
                    sb.Append("{ \"id\": \""+1+"\", \"process_type\": \"start-activity\",\"process_id\":\""+Guid.NewGuid()+"\", \"process_name\": \"提单\", \"process_to\": \"\", \"icon\": \"icon-ok\", \"style\": \"width:121px;height:41px;line-height:41px;color:#0e76a8;left:193px;top:132px;\" },");
                    sb.Append("{ \"id\": \"" + 2 + "\", \"process_type\": \"start-activity\",\"process_id\":\"" + Guid.NewGuid() + "\", \"process_name\": \"完成\", \"process_to\": \"\", \"icon\": \"icon-stop\", \"style\": \"width:121px;height:41px;line-height:41px;color:#0e76a8;left:193px;top:250px;\" },");
                }
                else
                {
                    IList<ActivitySet> activitySetList = activitySetService.GetNextActivitySets(new Guid(this.WorkflowKey));
                    int x = 100;
                    int y = 50;
                    for (int i = 0; i < activityDefinitionList.Count; i++)
                    {
                        WorkflowActivity activity = activityDefinitionList[i];
                        IList<ActivitySet> nextActivities = activitySetList.Where(a => a.SetId == activity.NextActivitySetId).ToList();
                        if (activity.State == 0)
                        {
                            sb.Append("{ \"id\": \"" + activity.SortOrder + "\", \"process_type\": \"start-activity\",\"process_id\":\"" + activity.ActivityId + "\", \"process_name\": \"" + activity.ActivityName + "\", \"process_to\":\"");
                        }
                        else if (activity.State == 2)
                        {
                            sb.Append("{ \"id\": \"" + activity.SortOrder + "\", \"process_type\": \"start-activity\",\"process_id\":\"" + activity.ActivityId + "\", \"process_name\": \"" + activity.ActivityName + "\", \"process_to\":\"");
                        }
                        else
                        {
                            sb.Append("{ \"id\": \"" + activity.SortOrder + "\", \"process_type\": \"activity\",\"process_id\":\"" + activity.ActivityId + "\", \"process_name\": \"" + activity.ActivityName + "\", \"process_to\":\"");
                        }
                        foreach (ActivitySet next in nextActivities)
                        {
                            WorkflowActivity nextActivity = activityDefinitionList.Where(a => a.ActivityId == next.ActivityId).First();
                            sb.Append(nextActivity.SortOrder + ",");
                        }
                        if (nextActivities.Count>0)
                            sb = sb.Remove(sb.Length - 1, 1);
                       
                        CZActivityDefinition czActivityDefinition = CZActivityDefinition.GetWorkflowActivityByActivityId(activity.ActivityId);
                        //string json = JsonConvert.SerializeObject(czActivityDefinition);
                        string json = LitJson.JsonMapper.ToJson(czActivityDefinition);
                        if (activity.State == 0)
                        {
                            sb.Append("\", \"icon\": \"icon-ok\", \"style\": \"width:121px;height:41px;line-height:41px;color:#0e76a8;left:" + (activity.X <= 0 ? x : activity.X) + "px;top:" + (activity.Y <= 0 ? y : activity.Y) + "px;\" ,");
                        }
                        else if (activity.State == 2)
                        {
                            sb.Append("\", \"icon\": \"icon-stop\", \"style\": \"width:121px;height:41px;line-height:41px;color:#0e76a8;left:" + (activity.X <= 0 ? x : activity.X) + "px;top:" + (activity.Y <= 0 ? y : activity.Y) + "px;\" ,");
                        }
                        else
                        {
                            sb.Append("\", \"icon\": \"icon-star\", \"style\": \"width:120px;height:30px;line-height:30px;color:#0e76a8;left:" + (activity.X <= 0 ? x : activity.X) + "px;top:" + (activity.Y <= 0 ? y : activity.Y) + "px;\" ,");
                        }
                        sb.Append("\"properties\":"+json+"},");
                        y += 80;
                    }
                   
                    
                }
                sb = sb.Remove(sb.Length - 1, 1);
                sb.Append("]}");
                ProcessData = sb.ToString();
            }
            else
            {
                if (Request.QueryString["m"] == null)
                {
                    Response.Write("参数错误！");
                    //Response.Redirect("../../workflows/workflowDeploy.aspx");
                }
            }
            
        }
    }
    protected override void OnInit(EventArgs e) { }
}