using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using Botwave.StateGraph;

namespace Botwave.Workflow.Extension.WorkflowMap
{
    /// <summary>
    /// 流程图的 HttpHandler 类.
    /// </summary>
    public class WorkflowImageHandler : IHttpHandler
    {
        #region IHttpHandler 成员

        /// <summary>
        /// 获取一个值，该值指示其他请求是否可以使用 System.Web.IHttpHandler 实例.
        /// </summary>
        public bool IsReusable
        {
            get { return false; }
        }

        /// <summary>
        /// 处理请求.
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            string wid = context.Request.QueryString["wid"];             // WorkflowId
            string aid = context.Request.QueryString["aid"];               // ActivityId
            string width = context.Request.QueryString["width"];       // width
            string index = context.Request.QueryString["index"];       // ActivityIndex
            string aname = context.Request.QueryString["aname"];   // ActivityName

            if (string.IsNullOrEmpty(wid))
            {
                context.Response.ContentType = "text/html";
                context.Response.Write("错误：找不到相应的流程信息！");
                context.Response.End();
                return;
            }
            else
            {
                context.Response.ContentType = "image/jpeg";
                context.Response.AppendHeader("Content-Disposition", "filename=workflow.jpg");
            }

            Guid workflowId = new Guid(wid);

            Guid? activityId = null;
            if (!string.IsNullOrEmpty(aid))
                activityId = new Guid(aid);
            int thumbWidth = string.IsNullOrEmpty(width)?0:int.Parse(width);
            int activityIndex = string.IsNullOrEmpty(index) ? -1 : Convert.ToInt32(index);
            string activityName = string.IsNullOrEmpty(aname) ? string.Empty : HttpUtility.UrlDecode(aname);

            if (string.IsNullOrEmpty(activityName) && activityIndex == -1)
            {
                RenderHelper.RenderWorkflowMap(workflowId, context.Response.OutputStream, thumbWidth);
            }
            else
            {
                if (!WorkflowMapManager.FillRectangle(workflowId, activityName, context.Response.OutputStream, thumbWidth))
                {
                    IList<GraphNode> nodes;
                    if (activityId == null)
                        nodes = RenderHelper.GetGraphNodes(workflowId);
                    else
                        nodes = RenderHelper.GetGraphNodes(workflowId, activityId.Value);

                    if (nodes.Count > 0)
                    {
                        WebPreview preview;
                        if (activityId .HasValue)
                            preview = new WebPreview(nodes);
                        else
                            preview = new WebPreview(nodes, false);
                        if (!string.IsNullOrEmpty(activityName))
                            preview.SelectedNodeName = activityName;
                        if (activityIndex > -1)
                            preview.SelectedNodeIndex = activityIndex;

                        preview.Render(context, thumbWidth);
                    }
                }
            }
            context.Response.End();
        }

        #endregion
    }
}
