using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Botwave.XQP.Web;

public partial class contrib_mobile_ajax_CommonAjax : Botwave.XQP.Web.Security.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string command=this.Context.Request["command"];
        WorkflowExtensionService ws = new WorkflowExtensionService();
        switch (command)
        {
            case "loginname":
                Response.Write(CurrentUserName);
                break;
            case "todo":
                Response.Write(ws.GetTodoCount(CurrentUserName));
                break;
            case "toreview":
                Response.Write(ws.GetToReviewCount(CurrentUserName));
                break;
            case "draft":
                Response.Write(ws.GetDraftCount(CurrentUserName));
                break;
            case "reading":
                string actor = CurrentUserName;
                string ids = Context.Request["aiids"];
                if (ids.Length > 0)
                {
                    foreach (string id in ids.Split(','))
                    {
                        if (id.Length > 0)
                        {
                            Guid activityInstanceId = new Guid(id);
                            Botwave.XQP.Domain.ToReview.UpdateReview(activityInstanceId, actor);
                            Botwave.XQP.Domain.ToReview.DeletePendingMsg(activityInstanceId, actor);
                        }
                    }
                    Response.Write("操作成功");
                }
                else
                Response.Write("没有选择记录");
                break;
            default:
                Response.Write(string.Empty);
                break;
        }
        Response.End();
    }
}