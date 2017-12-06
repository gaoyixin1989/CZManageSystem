using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Botwave.Workflow.Service
{
    /// <summary>
    /// ���̲������ӿ�.
    /// </summary>
    public interface IDeployService
    {
        /// <summary>
        /// ��������.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="creator"></param>
        /// <returns></returns>
        DeployActionResult DeployWorkflow(XmlReader reader, string creator);

        /// <summary>
        /// ������̶���.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        ActionResult CheckWorkflow(XmlReader reader);

        /// <summary>
        ///  �������̶���.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        ActionResult ExportWorkflow(XmlWriter writer, Guid workflowId);

        /// <summary>
        /// ɾ�����̶���.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        ActionResult DeleteWorkflow(Guid workflowId);
    }
}
