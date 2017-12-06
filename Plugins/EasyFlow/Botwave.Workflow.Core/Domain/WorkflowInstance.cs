using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Domain
{
    /// <summary>
    /// ����ʵ����.
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
        /// ���췽��������ʵ������.
        /// </summary>
        public WorkflowInstance()
        {  }

        /// <summary>
        /// ����ʵ�� Id.
        /// </summary>
        public Guid WorkflowInstanceId
        {
            get { return workflowInstanceId; }
            set { workflowInstanceId = value; }
        }

        /// <summary>
        /// ���̶��� Id.
        /// </summary>
        public Guid WorkflowId
        {
            get { return workflowId; }
            set { workflowId = value; }
        }

        /// <summary>
        /// ��ˮ������.
        /// </summary>
        public string SheetId
        {
            get { return sheetId; }
            set { sheetId = value; }
        }

        /// <summary>
        /// ״̬.
        /// -1 �ݸ�״̬;
        /// 0 ��ʼ״̬;
        /// 1 ����������;
        /// 2 ���������;
        /// 99 ������ȡ��.
        /// </summary>
        public int State
        {
            get { return state; }
            set { state = value; }
        }

        /// <summary>
        /// ����ʵ��������.
        /// </summary>
        public string Creator
        {
            get { return creator; }
            set { creator = value; }
        }

        /// <summary>
        /// ����ʵ������ʱ��.
        /// </summary>
        public DateTime StartedTime
        {
            get { return startedTime; }
            set { startedTime = value; }
        }

        /// <summary>
        /// ����ʵ�����ʱ��.
        /// </summary>
        public DateTime? FinishedTime
        {
            get { return finishedTime; }
            set { finishedTime = value; }
        }

        /// <summary>
        /// ����.
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// ���̶ܳ�:
        /// 0 �����ܣ�1 ���ܣ�2 �߼�����.
        /// </summary>
        public int Secrecy
        {
            get { return secrecy; }
            set { secrecy = value; }
        }

        /// <summary>
        /// �����̶�:
        /// 0 һ��;
        /// 1 ����;
        /// 2 �ܽ���;
        /// 3 �����.
        /// </summary>
        public int Urgency
        {
            get { return urgency; }
            set { urgency = value; }
        }

        /// <summary>
        /// ��Ҫ�̶�(����):
        /// 0 һ��;
        /// 1 ��Ҫ;
        /// 2 ����Ҫ;
        /// 3 ����Ҫ.
        /// </summary>
        public int Importance
        {
            get { return importance; }
            set { importance = value; }
        }

        /// <summary>
        /// �������ʱ��.
        /// </summary>
        public DateTime? ExpectFinishedTime
        {
            get { return expectFinishedTime; }
            set { expectFinishedTime = value; }
        }

        /// <summary>
        /// ������������.
        /// </summary>
        public string Requirement
        {
            get { return requirement; }
            set { requirement = value; }
        }

        #endregion

        #region �ǳ־û�

        private string externalEntityType;
        private string externalEntityId;

        /// <summary>
        /// �ⲿʵ������.
        /// </summary>
        public string ExternalEntityType
        {
            get { return externalEntityType; }
            set { externalEntityType = value; }
        }

        /// <summary>
        /// �ⲿʵ�� Id.
        /// </summary>
        public string ExternalEntityId
        {
            get { return externalEntityId; }
            set { externalEntityId = value; }
        }

        #endregion

        /// <summary>
        /// ת��Ϊ�ַ�����ʽ.
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
