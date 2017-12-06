using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.XQP.Domain
{
    /// <summary>
    /// 流程活动定义类(潮州).
    /// </summary>
    public class CZWorkflowInstance : Botwave.Workflow.Domain.WorkflowInstance
    {
        private int _printCount;

        /// <summary>
        /// 打印次数 0 为不限制
        /// </summary>
        public int PrintCount
        {
            get { return _printCount; }
            set { _printCount = value; }
        }

        public CZWorkflowInstance()
            : base()
        {   }

        /// <summary>
        /// 获取指定流程实例定义列表.
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        public static CZWorkflowInstance GetWorkflowInstance(Guid workflowInstanceId)
        {
            return Botwave.Extension.IBatisNet.IBatisMapper.Load<CZWorkflowInstance>("cz_bwwf_WorkflowInstance_Select_Wiid", workflowInstanceId);
        }

        /// <summary>
        /// 更新流程实例
        /// </summary>
        /// <param name="activityDefinition"></param>
        /// <returns></returns>
        public static int UpdateWorkflowInstance(CZWorkflowInstance workflowInstance)
        {
            return Botwave.Extension.IBatisNet.IBatisMapper.Update("cz_bwwf_WorkflowInstance_Update", workflowInstance);
        }
    }
}
