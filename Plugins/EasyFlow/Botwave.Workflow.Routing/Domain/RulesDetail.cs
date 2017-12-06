using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Botwave.Workflow.Routing.Domain
{
    /// <summary>
    /// 路由规则详细信息
    /// </summary>
    [Serializable]
    public class RulesDetail
    {
        private Guid ruleid;
        private Guid workflowid;
        private Guid workflowinstanceid;
        private Guid activityId;
        private Guid nextActivityId;
        private string activityName;
        private string nextActivityName;
        private int stepType;
        private string conditions;
        private string description;
        private string title;
        private string creator;
        private DateTime createdtime;
        private string lastModifier;
        private DateTime lastModtime;
        private string fName;
        private string fValue;
        private string fNumber;
        private Guid parentRuleId;
        private int organizationType;
        private int status;
        private string fieldsAssemble;

        /// <summary>
        /// 规则ID
        /// </summary>
        public Guid Ruleid
        {
            get { return ruleid; }
            set { ruleid = value; }
        }

        /// <summary>
        /// 流程ID
        /// </summary>
        public Guid Workflowid
        {
            get { return workflowid; }
            set { workflowid = value; }
        }

        /// <summary>
        /// 表单实例ID
        /// </summary>
        public Guid Workflowinstanceid
        {
            get { return workflowinstanceid; }
            set { workflowinstanceid = value; }
        }

        /// <summary>
        /// 当前步骤名称
        /// </summary>
        public string ActivityName
        {
            get { return activityName; }
            set { activityName = value; }
        }

        /// <summary>
        /// 下行步骤名称
        /// </summary>
        public string NextActivityName
        {
            get { return nextActivityName; }
            set { nextActivityName = value; }
        }

        /// <summary>
        /// 当前步骤ID
        /// </summary>
        public Guid ActivityId
        {
            get { return activityId; }
            set { activityId = value; }
        }

        /// <summary>
        /// 下行步骤ID
        /// </summary>
        public Guid NextActivityId
        {
            get { return nextActivityId; }
            set { nextActivityId = value; }
        }

        /// <summary>
        /// 步骤类型(0:起始步骤;1:流转步骤;2:父流程流转规则步骤)
        /// </summary>
        public int StepType
        {
            get { return stepType; }
            set { stepType = value; }
        }

        /// <summary>
        /// 规则条件
        /// </summary>
        public string Conditions
        {
            get { return conditions; }
            set { conditions = value; }
        }

        /// <summary>
        /// 规则描述
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// 规则标题
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// 作者
        /// </summary>
        public string Creator
        {
            get { return creator; }
            set { creator = value; }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Createdtime
        {
            get { return createdtime; }
            set { createdtime = value; }
        }

        /// <summary>
        /// 修改人
        /// </summary>
        public string LastModifier
        {
            get { return lastModifier; }
            set { lastModifier = value; }
        }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime LastModtime
        {
            get { return lastModtime; }
            set { lastModtime = value; }
        }

        /// <summary>
        /// 字段标识
        /// </summary>
        public string FName
        {
            get { return fName; }
            set { fName = value; }
        }

        /// <summary>
        /// 字段值
        /// </summary>
        public string FValue
        {
            get { return fValue ; }
            set { fValue = value; }
        }

        /// <summary>
        /// 字段值
        /// </summary>
        public string FNumber
        {
            get { return fNumber; }
            set { fNumber = value; }
        }

        /// <summary>
        /// 父规则ID
        /// </summary>
        public Guid ParentRuleId
        {
            get { return parentRuleId; }
            set { parentRuleId = value; }
        }

        /// <summary>
        /// 组织控制类型(0：当前步骤；1：提单人)
        /// </summary>
        public int OrganizationType
        {
            get { return organizationType; }
            set { organizationType = value; }
        }

        /// <summary>
        /// 状态(0：禁用；1：启用)
        /// </summary>
        public int Status
        {
            get { return status; }
            set { status = value; }
        }

        /// <summary>
        /// 字段集合，如 _F1_:_F2_ 
        /// </summary>
        public string FieldsAssemble
        {
            get { return fieldsAssemble; }
            set { fieldsAssemble = value; }
        }
    }
}
