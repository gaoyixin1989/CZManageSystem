using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Botwave.Workflow.Service
{
    /// <summary>
    /// 流程部署服务接口.
    /// </summary>
    public interface IDeployService
    {
        /// <summary>
        /// 部署流程.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="creator"></param>
        /// <returns></returns>
        DeployActionResult DeployWorkflow(XmlReader reader, string creator);

        /// <summary>
        /// 检测流程定义.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        ActionResult CheckWorkflow(XmlReader reader);

        /// <summary>
        ///  导出流程定义.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        ActionResult ExportWorkflow(XmlWriter writer, Guid workflowId);

        /// <summary>
        /// 删除流程定义.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        ActionResult DeleteWorkflow(Guid workflowId);
    }
}
