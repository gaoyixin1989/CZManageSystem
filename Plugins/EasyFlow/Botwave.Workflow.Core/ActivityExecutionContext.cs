using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow
{
    /// <summary>
    /// 流程执行上下文.
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
        /// 活动实例Id.
        /// </summary>
        public Guid ActivityInstanceId
        {
            get { return activityInstanceId; }
            set { activityInstanceId = value; }
        }

        /// <summary>
        /// 执行命令.
        /// </summary>
        public string Command
        {
            get { return command; }
            set { command = value; }
        }

        /// <summary>
        /// 原因.
        /// </summary>
        public string Reason
        {
            get { return reason; }
            set { reason = value; }
        }

        /// <summary>
        /// 结果.
        /// </summary>
        public string Result
        {
            get { return result; }
            set { result = value; }
        }

        /// <summary>
        /// 执行者.
        /// </summary>
        public string Actor
        {
            get { return actor; }
            set { actor = value; }
        }

        /// <summary>
        /// 流程步骤操作人描述.
        /// </summary>
        public string ActorDescription
        {
            get { return actorDescription; }
            set { actorDescription = value; }
        }

        /// <summary>
        /// 外部实体类型.
        /// </summary>
        public string ExternalEntityType
        {
            get { return externalEntityType; }
            set { externalEntityType = value; }
        }

        /// <summary>
        /// 外部实体Id.
        /// </summary>
        public string ExternalEntityId
        {
            get { return externalEntityId; }
            set { externalEntityId = value; }
        }

        /// <summary>
        /// 自定义变量集合.
        /// </summary>
        public IDictionary<string, object> Variables
        {
            get { return variables; }
            set { variables = value; }
        }

        /// <summary>
        /// 步骤任务分派字典(字典：步骤编号,分派人用户名列表).
        /// </summary>
        public IDictionary<Guid, IDictionary<string, string>> ActivityAllocatees
        {
            get { return activityAllocatees; }
            set { activityAllocatees = value; }
        }
        #endregion

        /// <summary>
        /// 构造方法,创建实例对象.
        /// </summary>
        public ActivityExecutionContext()
        {
            this.variables = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// 重写转化字符串格式.
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
        /// 复制当前的流程活动(步骤)执行上下文对象为一个新的实例.
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
        /// 添加指定类型的变量字典到当前字典当中.
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
