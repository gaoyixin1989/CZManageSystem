using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Botwave.Commons;
using Botwave.XQP.Domain;

public partial class apps_czmcc_pages_WorkflowRelationAjax : System.Web.UI.Page
{
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(apps_czmcc_pages_WorkflowRelationAjax));

    protected void Page_Load(object sender, EventArgs e)
    {
        string action = Request["action"];
        Response.Clear();
        this.Response.ContentType = "text/plain";
        this.Response.AddHeader("Cache-Control", "no-cache");

        switch (action)
        {
            case "save":
                Response.Write(Save());
                break;
            case "delete":
                Response.Write(Delete());
                break;
            case "relation":
                Response.Write(GetRelationHtml());
                break;
            case "relationlist":
                Response.Write(GetRelationListHtml());
                break;
            case "attention-add":
                Response.Write(AttentionAdd());
                break;
            case "attention-del":
                Response.Write(AttentionDelete());
                break;
            default:
                break;
        }
        Response.End();
    }

    protected string Save()
    {
        Guid? relationID = DbUtils.ToGuid(Request["rel"]);
        if (!relationID.HasValue)
            return "传入参数错误，未知关联标识。";
        Guid? targetID = DbUtils.ToGuid(Request["target"]);
        if (!targetID.HasValue)
            return "传入参数错误，未知历史工单标识参数。";
        bool isRefAttachment = (Request["attachment"] == "1");

        CZWorkflowRelation item = new CZWorkflowRelation(relationID.Value, 
            targetID.Value, 
            isRefAttachment, 
            Botwave.Security.LoginHelper.UserName);
        item.Update();

        return "保存成功";
    }

    protected string Delete()
    {
        try
        {
            int id = DbUtils.ToInt32(Request["id"]);
            CZWorkflowRelation.Delete(id);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return "删除关联工单出现错误：" + ex.Message;
        }
        return "";
    }

    protected string GetRelationHtml()
    {
        Guid? relationID = DbUtils.ToGuid(Request["rel"]);
        if (!relationID.HasValue)
            return "";

        bool started = (Request["started"] == "true");
        bool edtiable = (Request["editable"] == "true");
        try
        {
            return CZWorkflowRelation.BuildRelations(relationID.Value, Botwave.Security.LoginHelper.UserName, started, edtiable);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return "获取关联工单出现错误：" + ex.Message;
        }
    }

    protected string GetRelationListHtml()
    {
        Guid? relationID = DbUtils.ToGuid(Request["rel"]);
        if (!relationID.HasValue)
            return "";

        bool started = (Request["started"] == "true");
        bool edtiable = (Request["editable"] == "true");
        try
        {
            return CZWorkflowRelationSetting.BuildRelations(relationID.Value, Botwave.Security.LoginHelper.UserName, started, edtiable);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return "获取关联工单出现错误：" + ex.Message;
        }
    }

    protected string AttentionAdd()
    {
        Guid? workflowInstanceId = DbUtils.ToGuid(Request["wiid"]);
        if (!workflowInstanceId.HasValue)
            return "";
        int type = DbUtils.ToInt32(Request["type"]);
        string actor = DbUtils.ToString(Request["actor"]);

        try
        {
            if (CZWorkflowAttention.Create(workflowInstanceId.Value, type, string.Empty, actor))
            {
                return "成功加入关注。";
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return "加入关注出现错误：" + ex.Message;
        }
        return "";
    }

    protected string AttentionDelete()
    {
        Guid? workflowInstanceId = DbUtils.ToGuid(Request["wiid"]);
        if (!workflowInstanceId.HasValue)
            return "";

        string actor = DbUtils.ToString(Request["actor"]);
        ;
        try
        {
            if (CZWorkflowAttention.Delete(workflowInstanceId.Value, actor))
            {
                return "成功取消关注。";
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return "取消关注出现错误：" + ex.Message;
        }
        return "";
    }
}
