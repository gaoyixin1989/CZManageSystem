using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow
{
    /// <summary>
    /// ����ִ��������.
    /// </summary>
    public class ActivityExecutionContext
    {
        #region properties

        private Guid activityInstanceId = Guid.Empty;
        private string command;
        private string reason;
        private string result;
        private string actor;
        private string actorDescription = null;
        private string externalEntityType;
        private string externalEntityId;
        private IDictionary<string, object> variables;
        private IDictionary<Guid, IDictionary<string, string>> activityAllocatees;

        /// <summary>
        /// �ʵ��Id.
        /// </summary>
        public Guid ActivityInstanceId
        {
            get { return activityInstanceId; }
            set { activityInstanceId = value; }
        }

        /// <summary>
        /// ִ������.
        /// </summary>
        public string Command
        {
            get { return command; }
            set { command = value; }
        }

        /// <summary>
        /// ԭ��.
        /// </summary>
        public string Reason
        {
            get { return reason; }
            set { reason = value; }
        }

        /// <summary>
        /// ���.
        /// </summary>
        public string Result
        {
            get { return result; }
            set { result = value; }
        }

        /// <summary>
        /// ִ����.
        /// </summary>
        public string Actor
        {
            get { return actor; }
            set { actor = value; }
        }

        /// <summary>
        /// ���̲������������.
        /// </summary>
        public string ActorDescription
        {
            get { return actorDescription; }
            set { actorDescription = value; }
        }

        /// <summary>
        /// �ⲿʵ������.
        /// </summary>
        public string ExternalEntityType
        {
            get { return externalEntityType; }
            set { externalEntityType = value; }
        }

        /// <summary>
        /// �ⲿʵ��Id.
        /// </summary>
        public string ExternalEntityId
        {
            get { return externalEntityId; }
            set { externalEntityId = value; }
        }

        /// <summary>
        /// �Զ����������.
        /// </summary>
        public IDictionary<string, object> Variables
        {
            get { return variables; }
            set { variables = value; }
        }

        /// <summary>
        /// ������������ֵ�(�ֵ䣺������,�������û����б�).
        /// </summary>
        public IDictionary<Guid, IDictionary<string, string>> ActivityAllocatees
        {
            get { return activityAllocatees; }
            set { activityAllocatees = value; }
        }
        #endregion

        /// <summary>
        /// ���췽��,����ʵ������.
        /// </summary>
        public ActivityExecutionContext()
        {
            this.variables = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// ��дת���ַ�����ʽ.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return new StringBuilder().AppendFormat("[activityInstanceId:{0}]\r\n", activityInstanceId.ToString())
                .AppendFormat("[command:{0}]\r\n", null == command ? String.Empty : command)
                .AppendFormat("[reason:{0}]\r\n", null == reason ? String.Empty : reason)
                .AppendFormat("[result:{0}]\r\n", null == result ? String.Empty : result)
                .AppendFormat("[actor:{0}]\r\n", null == actor ? String.Empty : actor)
                .AppendFormat("[externalEntityType:{0}]\r\n", null == externalEntityType ? String.Empty : externalEntityType)
                .AppendFormat("[externalEntityId:{0}]\r\n", null == externalEntityId ? String.Empty : externalEntityId)
                .AppendFormat("[variables:{0}]\r\n", null == variables ? String.Empty : variables.ToString())
                .ToString();
        }

        /// <summary>
        /// ���Ƶ�ǰ�����̻(����)ִ�������Ķ���Ϊһ���µ�ʵ��.
        /// </summary>
        /// <returns></returns>
        public ActivityExecutionContext Clone()
        {
            ActivityExecutionContext context = new ActivityExecutionContext();
            context.ActivityInstanceId = this.ActivityInstanceId;
            context.Actor = this.Actor;
            context.Command = this.Command;
            context.ExternalEntityId = this.ExternalEntityId;
            context.ExternalEntityType = this.ExternalEntityType;
            context.Variables = this.Variables;
            context.Reason = this.Reason;
            context.Result = this.Result;
            return context;
        }

        /// <summary>
        /// ���ָ�����͵ı����ֵ䵽��ǰ�ֵ䵱��.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="inputDict"></param>
        public void AppendVariables<TValue>(IDictionary<string, TValue> inputDict)
        {
            foreach (string key in inputDict.Keys)
            {
                this.variables[key] = inputDict[key];
            }
        }
    }
}
