using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Botwave.XQP.Domain
{
    /// <summary>
    /// 流程活动定义类(潮州).
    /// </summary>
    [Serializable]
    public class CZActivityDefinition : Botwave.Workflow.Domain.ActivityDefinition
    {
        private int _canPrint;
        private int _printAmount;
        private int _canEdit;
        private bool _returnToPrev;
        private bool _isMobile;
        private bool _isTimeOutContinue = false;

        /// <summary>
        /// 是否开启打印（-1为禁用）
        /// </summary>
        public int CanPrint
        {
            get { return _canPrint; }
            set { _canPrint = value; }
        }

        /// <summary>
        /// 打印次数 0 为不限制
        /// </summary>
        public int PrintAmount
        {
            get { return _printAmount; }
            set { _printAmount = value; }
        }

        /// <summary>
        /// 处理意见是否可编辑 -1 为不可编辑
        /// </summary>
        public int CanEdit
        {
            get { return _canEdit; }
            set { _canEdit = value; }
        }

        /// <summary>
        /// 退还后能否再次提回此步骤
        /// </summary>
        public bool ReturnToPrev
        {
            get { return _returnToPrev; }
            set { _returnToPrev = value; }
        }

        /// <summary>
        /// 是否允许手机审批
        /// </summary>
        public bool IsMobile
        {
            get { return _isMobile; }
            set { _isMobile = value; }
        }

        /// <summary>
        /// 是否允许超时时自动处理该步骤
        /// </summary>
        public bool IsTimeOutContinue
        {
            get { return _isTimeOutContinue; }
            set { _isTimeOutContinue = value; }
        }
        public CZActivityDefinition()
            : base()
        {   }

        /// <summary>
        /// 获取指定流程实例编号的流程步骤定义列表.
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        public static IList<CZActivityDefinition> GetWorkflowActivities(Guid workflowInstanceId)
        {
            return Botwave.Extension.IBatisNet.IBatisMapper.Select<CZActivityDefinition>("cz_bwwf_ActivityDefinition_Select_WorkflowInstanceId", workflowInstanceId);
        }

        /// <summary>
        /// 获取指定流程步骤编号的流程步骤定义列表.
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        public static CZActivityDefinition GetWorkflowActivityByActivityId(Guid activityId)
        {
            return Botwave.Extension.IBatisNet.IBatisMapper.Load<CZActivityDefinition>("cz_bwwf_ActivityDefinition_Select_ActivityId", activityId);
        }

        /// <summary>
        /// 更新步骤打印设置
        /// </summary>
        /// <param name="activityDefinition"></param>
        /// <returns></returns>
        public static int UpdateWorkflowActivityPrint(CZActivityDefinition activityDefinition)
        {
            return Botwave.Extension.IBatisNet.IBatisMapper.Update("cz_bwwf_ActivityDefinition_Update", activityDefinition);
        }

        public static IList<Botwave.Workflow.Domain.ActivityDefinition> GetActivityDefinitionsByInstanceIdTo(string workflowname, string activityname)
        {
            Hashtable parameters = new Hashtable(2);
            parameters.Add("Workflowname", workflowname);
            parameters.Add("Activityname", activityname);
            return Botwave.Extension.IBatisNet.IBatisMapper.Select<Botwave.Workflow.Domain.ActivityDefinition>("bwwf_Activity_Select_By_ActivityInstanceId_To", parameters);
        }
    }
}
