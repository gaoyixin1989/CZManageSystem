using System;
using System.Collections.Generic;
using System.Text;
using Botwave.WebServiceClients;

namespace Botwave.Workflow.Practices.BWOA.WfPlugin
{
    public class SendEventHandler
    {
        public static Botwave.WebServiceClients.ActionResult SendEvent(int operType,string actor,string userName,string workflowName,string title,string nextActivity)
        {
            //事件处理
            Botwave.WebServiceClients.ActionResult result = new Botwave.WebServiceClients.ActionResult();
            string strMsg = "";
            if (operType == 0)
            {
                strMsg = string.Format("<a href='../apps/event/pages/list.aspx?user={0}'>{0}</a>的{1}工单 {2} 进入{3}", userName, workflowName, title, nextActivity);
            }
            else
            {
                strMsg = string.Format("<a href='../apps/event/pages/list.aspx?user={0}'>{0}</a>的{1}工单 {2} 被退回", userName, workflowName, title);
            }
            result = Botwave.WebServiceClients.ServiceFactory.GetEventWebService().SendEvent("oa", "oa", actor, DateTime.Now.ToString(), strMsg, null);

            return result;
        }
    }
}
