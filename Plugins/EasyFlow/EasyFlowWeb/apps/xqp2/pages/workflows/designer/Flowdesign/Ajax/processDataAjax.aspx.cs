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
using Newtonsoft.Json;
using Botwave.Workflow.Routing.Implements;
using System.Data;
using Botwave.Workflow.Routing.Domain;

public partial class apps_xqp2_pages_workflows_designer_Flowdesign_Ajax_ProcessdataAjax : Botwave.Security.Web.PageBase
{
    #region service interfaces

    private IActivityDefinitionService activityDefinitionService = (IActivityDefinitionService)Ctx.GetObject("activityDefinitionService");
    private IActivitySetService activitySetService = (IActivitySetService)Ctx.GetObject("activitySetService");
    private IWorkflowSettingService workflowSettingService = (IWorkflowSettingService)Ctx.GetObject("workflowSettingService");
    private ITaskAssignService taskAssignService = (ITaskAssignService)Ctx.GetObject("taskAssignService");
    private IActivityRulesService activityRulesService = (IActivityRulesService)Ctx.GetObject("activityRulesService");

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

    public ITaskAssignService TaskAssignService
    {
        set { taskAssignService = value; }
    }

    public IActivityRulesService ActivityRulesService
    {
        set { activityRulesService = value; }
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
    protected override void OnInit(EventArgs e) { }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string wid = Context.Request["wid"];
            if (!string.IsNullOrEmpty(wid))
            {
                this.WorkflowKey = Server.HtmlEncode(wid);
               
                Guid workflowId = new Guid(this.WorkflowKey);
                WorkflowSetting setting = workflowSettingService.GetWorkflowSetting(workflowId);
                IList<WorkflowActivity> activityDefinitionList = WorkflowActivity.GetActivities(new Guid(this.WorkflowKey));
                WorkflowComponent profile = WorkflowComponent.GetWorkflow(new Guid(this.WorkflowKey));
                if (profile == null)
                    profile = new WorkflowComponent();
                if (setting == null)
                    setting = WorkflowSetting.Default;
                StringBuilder sb = new StringBuilder();
                string profileJson = JsonConvert.SerializeObject(profile);
                string settingJson = JsonConvert.SerializeObject(setting);
                //string profileJson = LitJson.JsonMapper.ToJson(profile);
                //string settingJson = LitJson.JsonMapper.ToJson(setting);
                sb.Append("{ \"total\": " + activityDefinitionList.Count + ", \"flow_id\": \"" + this.WorkflowKey + "\",\"setting\":" + settingJson + ",\"profile\":" + profileJson + ",\"list\": [");
                if (activityDefinitionList.Count == 0)
                {
                    WorkflowActivity czActivityDefinition = new WorkflowActivity();
                    
                    AllocatorOption assignment = new AllocatorOption();
                    string assignmentJson = JsonConvert.SerializeObject(assignment);
                    //string assignmentJson = LitJson.JsonMapper.ToJson(assignment);
                    czActivityDefinition.State = 0;
                    czActivityDefinition.ActivityName = "提单";
                    string json = JsonConvert.SerializeObject(czActivityDefinition);
                    //string json = LitJson.JsonMapper.ToJson(czActivityDefinition);
                    sb.Append("{ \"id\": \"" + 1 + "\", \"process_type\": \"start-activity\",\"process_id\":\"" + Guid.NewGuid() + "\", \"process_name\": \"提单\", \"process_to\": \"\", \"icon\": \"icon-ok\", \"style\": \"width:121px;height:41px;line-height:41px;color:#0e76a8;left:193px;top:132px;\" ,").Append("\"activity\":" + json + ",\"assignment\":" + assignmentJson + ",\"rules\":\"" + Botwave.XQP.Commons.XQPHelper.EncodeBase64("utf-8", "[]") + "\" },");
                    czActivityDefinition.State=2;
                    czActivityDefinition.ActivityName = "完成";
                     json = JsonConvert.SerializeObject(czActivityDefinition);
                    //json = LitJson.JsonMapper.ToJson(czActivityDefinition);
                     sb.Append("{ \"id\": \"" + 2 + "\", \"process_type\": \"end-activity\",\"process_id\":\"" + Guid.NewGuid() + "\", \"process_name\": \"完成\", \"process_to\": \"\", \"icon\": \"icon-stop\", \"style\": \"width:121px;height:41px;line-height:41px;color:#0e76a8;left:193px;top:250px;\" ,").Append("\"activity\":" + json + ",\"assignment\":" + assignmentJson + ",\"rules\":\"" + Botwave.XQP.Commons.XQPHelper.EncodeBase64("utf-8", "[]") + "\" },");
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
                            sb.Append("{ \"id\": \"" + activity.SortOrder + "\", \"process_type\": \"end-activity\",\"process_id\":\"" + activity.ActivityId + "\", \"process_name\": \"" + activity.ActivityName + "\", \"process_to\":\"");
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
                        if (nextActivities.Count > 0)
                            sb = sb.Remove(sb.Length - 1, 1);

                        //CZActivityDefinition czActivityDefinition = CZActivityDefinition.GetWorkflowActivityByActivityId(activity.ActivityId);
                        string json = JsonConvert.SerializeObject(activity);
                        //string json = LitJson.JsonMapper.ToJson(activity);
                        AllocatorOption assignmentAllocator = taskAssignService.GetAssignmentAllocator(activity.ActivityId);
                        string assignmentJson = JsonConvert.SerializeObject(assignmentAllocator);
                        //string assignmentJson = LitJson.JsonMapper.ToJson(assignmentAllocator);
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
                            sb.Append("\", \"icon\": \"icon-star\", \"style\": \"width:auto;height:30px;line-height:30px;color:#0e76a8;left:" + (activity.X <= 0 ? x : activity.X) + "px;top:" + (activity.Y <= 0 ? y : activity.Y) + "px;\" ,");
                        }
                        sb.Append("\"activity\":" + json + ",\"assignment\":" + assignmentJson + " ,");
                        int recordCount = 0;
                        //DataTable source = activityRulesService.GetActivityRules(WorkflowKey,activity.ActivityName, 0, 999, ref recordCount);
                        //source.Columns["condition"].ColumnName = "conditions";
                        //string ruleJson = JsonConvert.SerializeObject(source);
                        IList<RulesDetail> source = activityRulesService.GetActivityRules(WorkflowKey, activity.ActivityName);
                        //string ruleJson = LitJson.JsonMapper.ToJson(source);
                        string ruleJson = JsonConvert.SerializeObject(source);
                        sb.Append("\"rules\":\"" + Botwave.XQP.Commons.XQPHelper.EncodeBase64("utf-8", ruleJson) + "\"} ,");
                        y += 80;
                    }


                }
                sb = sb.Remove(sb.Length - 1, 1);
                sb.Append("]}");
                ProcessData = sb.ToString();
            }
            else
            {
                ProcessData = "{}";
            }
        }
    }
}