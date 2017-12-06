using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Domain
{
    /// <summary>
    /// 流程实例类.
    /// </summary>
    public class WorkflowInstance
    {
        #region getter/setter

        private Guid workflowInstanceId;
        private Guid workflowId;
        private string sheetId;
        private int state;
        private string creator;
        private DateTime startedTime;
        private DateTime? finishedTime;
        private string title;
        private int secrecy;
        private int urgency;
        private int importance;
        private DateTime? expectFinishedTime;
        private string requirement;

        /// <summary>
        /// 构造方法，创建实例对象.
        /// </summary>
        public WorkflowInstance()
        {  }

        /// <summary>
        /// 流程实例 Id.
        /// </summary>
        public Guid WorkflowInstanceId
        {
            get { return workflowInstanceId; }
            set { workflowInstanceId = value; }
        }

        /// <summary>
        /// 流程定义 Id.
        /// </summary>
        public Guid WorkflowId
        {
            get { return workflowId; }
            set { workflowId = value; }
        }

        /// <summary>
        /// 流水工单号.
        /// </summary>
        public string SheetId
        {
            get { return sheetId; }
            set { sheetId = value; }
        }

        /// <summary>
        /// 状态.
        /// -1 草稿状态;
        /// 0 初始状态;
        /// 1 流程运行中;
        /// 2 流程已完成;
        /// 99 流程已取消.
        /// </summary>
        public int State
        {
            get { return state; }
            set { state = value; }
        }

        /// <summary>
        /// 流程实例创建人.
        /// </summary>
        public string Creator
        {
            get { return creator; }
            set { creator = value; }
        }

        /// <summary>
        /// 流程实例启动时间.
        /// </summary>
        public DateTime StartedTime
        {
            get { return startedTime; }
            set { startedTime = value; }
        }

        /// <summary>
        /// 流程实例完成时间.
        /// </summary>
        public DateTime? FinishedTime
        {
            get { return finishedTime; }
            set { finishedTime = value; }
        }

        /// <summary>
        /// 标题.
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// 保密程度:
        /// 0 不保密，1 保密，2 高级保密.
        /// </summary>
        public int Secrecy
        {
            get { return secrecy; }
            set { secrecy = value; }
        }

        /// <summary>
        /// 紧急程度:
        /// 0 一般;
        /// 1 紧急;
        /// 2 很紧急;
        /// 3 最紧急.
        /// </summary>
        public int Urgency
        {
            get { return urgency; }
            set { urgency = value; }
        }

        /// <summary>
        /// 重要程度(级别):
        /// 0 一般;
        /// 1 重要;
        /// 2 很重要;
        /// 3 最重要.
        /// </summary>
        public int Importance
        {
            get { return importance; }
            set { importance = value; }
        }

        /// <summary>
        /// 期望完成时间.
        /// </summary>
        public DateTime? ExpectFinishedTime
        {
            get { return expectFinishedTime; }
            set { expectFinishedTime = value; }
        }

        /// <summary>
        /// 具体需求内容.
        /// </summary>
        public string Requirement
        {
            get { return requirement; }
            set { requirement = value; }
        }

        #endregion

        #region 非持久化

        private string externalEntityType;
        private string externalEntityId;

        /// <summary>
        /// 外部实例类型.
        /// </summary>
        public string ExternalEntityType
        {
            get { return externalEntityType; }
            set { externalEntityType = value; }
        }

        /// <summary>
        /// 外部实体 Id.
        /// </summary>
        public string ExternalEntityId
        {
            get { return externalEntityId; }
            set { externalEntityId = value; }
        }

        #endregion

        /// <summary>
        /// 转换为字符串形式.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("workflowInstanceId:{0}\r\n", this.workflowInstanceId);
            sb.AppendFormat("workflowId:{0}\r\n", this.workflowId);
            sb.AppendFormat("state:{0}\r\n", this.state);
            sb.AppendFormat("creator:{0}\r\n", this.creator);
            sb.AppendFormat("title:{0}\r\n", this.title);
            sb.AppendFormat("urgency:{0}\r\n", WorkflowHelper.ConvertUrgency2String(this.urgency));
            sb.AppendFormat("isSecrecy:{0}\r\n", WorkflowHelper.ConvertSecrecy2String(this.secrecy));
            sb.AppendFormat("requirement:{0}\r\n", this.requirement);
            return sb.ToString();
        }
    }
}
