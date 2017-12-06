<%@ WebHandler Language="C#" Class="PrintHandler" %>

using System;
using System.Web;

public class PrintHandler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        string wiid = context.Request["wiid"];
        string aiid = context.Request["aiid"];
        string command=context.Request["command"];
        if (command == "update")
        {
            Botwave.XQP.Domain.CZWorkflowInstance czWorkflowInstance = Botwave.XQP.Domain.CZWorkflowInstance.GetWorkflowInstance(new Guid(wiid));
            if (czWorkflowInstance != null)
            {
                if (czWorkflowInstance.State >= 2)
                {
                    Botwave.XQP.Domain.WorkflowProfile profile = Botwave.XQP.Domain.WorkflowProfile.LoadByWorkflowId(czWorkflowInstance.WorkflowId);

                    if (profile.PrintAmount > 0 && (profile.PrintAndExp == 0 || profile.PrintAndExp == 2))
                    {
                        Botwave.XQP.Domain.CZWorkflowInstance.UpdateWorkflowInstance(czWorkflowInstance);
                    }
                }
                else
                {
                    Botwave.XQP.Domain.CZActivityInstance activityInstance = Botwave.XQP.Domain.CZActivityInstance.GetWorkflowActivity(new Guid(aiid));
                    Botwave.XQP.Domain.CZActivityDefinition activityDefinition = Botwave.XQP.Domain.CZActivityDefinition.GetWorkflowActivityByActivityId(activityInstance.ActivityId);
                    if (activityDefinition.CanPrint > -1 && activityDefinition.PrintAmount > 0)
                        Botwave.XQP.Domain.CZActivityInstance.WorkflowActivitiesUpdate(activityInstance);
                }
            }
        }
        context.Response.ContentType = "text/plain";
        context.Response.Write("Hello World");
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}