using System;
using System.Collections.Generic;

namespace Botwave.Workflow.Domain
{
    /// <summary>
    /// 待办任务信息.
    /// </summary>
    public class TodoInfo
    {
        /// <summary>
        /// 默认操作/通过 0.
        /// </summary>
        public static readonly int OpDefault = 0;

        /// <summary>
        /// 退还操作.
        /// </summary>
        public static readonly int OpBack = 1;

        /// <summary>
        /// 指派.
        /// </summary>
        public static readonly int OpAssign = 2;

        /// <summary>
        /// 未读.
        /// </summary>
        const int UnReaded = 0;

        /// <summary>
        /// 已读.
        /// </summary>
        const int Readed = 1;

        /// <summary>
        /// 已处理.
        /// </summary>
        const int Processed = 2;

        private Guid activityInstanceId;
        private string userName;
        private int state;
        private string proxyName;
        private int operateType;

        /// <summary>
        /// 构造方法.
        /// </summary>
        public TodoInfo()
        { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="activityInstanceId">流程步骤实例 ID.</param>
        /// <param name="userName">待处理用户名.</param>
        /// <param name="proxyName">代理人用户名(即授权人的用户名).</param>
        /// <param name="operateType">处理（操作）类型.</param>
        public TodoInfo(Guid activityInstanceId, string userName, string proxyName, int operateType)
            : this(activityInstanceId, userName, 0, proxyName, operateType)
        { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="activityInstanceId">流程步骤实例 ID.</param>
        /// <param name="userName">待处理用户名.</param>
        /// <param name="state">状态.</param>
        /// <param name="proxyName">代理人用户名（即授权人用户名）.</param>
        /// <param name="operateType">处理（操作）类型.</param>
        public TodoInfo(Guid activityInstanceId, string userName, int state, string proxyName, int operateType)
        {
            this.activityInstanceId = activityInstanceId;
            this.userName = userName;
            this.state = state;
            this.proxyName = proxyName;
            this.operateType = operateType;
        }

        #region gets / sets

        /// <summary>
        /// 流程步骤实例 ID.
        /// </summary>
        public Guid ActivityInstanceId
        {
            get { return activityInstanceId; }
            set { activityInstanceId = value; }
        }

        /// <summary>
        /// 待处理用户名.
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        /// <summary>
        /// 状态：0未读，1已读，2已处理.
        /// </summary>
        public int State
        {
            get { return state; }
            set { state = value; }
        }

        /// <summary>
        /// 代理人用户名(即授权人的用户名).
        /// </summary>
        public string ProxyName
        {
            get { return proxyName; }
            set { proxyName = value; }
        }

        /// <summary>
        /// 处理（操作）类型.
        /// </summary>
        public int OperateType
        {
            get { return operateType; }
            set { operateType = value; }
        }
        #endregion

        #region 非持久化属性

        private string _workItemTitle;
        private string _activityName;
        private string _realName;

        /// <summary>
        /// 流程实例标题.
        /// </summary>
        public string WorkItemTitle
        {
            get { return _workItemTitle; }
            set { _workItemTitle = value; }
        }

        /// <summary>
        /// 步骤名称.
        /// </summary>
        public string ActivityName
        {
            get { return _activityName; }
            set { _activityName = value; }
        }

        /// <summary>
        /// 用户真实姓名.
        /// </summary>
        public string RealName
        {
            get { return _realName; }
            set { _realName = value; }
        }

        #endregion

        /// <summary>
        /// 是否未读.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static bool IsUnReaded(int state)
        {
            return state == UnReaded;
        }

        /// <summary>
        /// 是否已处理.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static bool IsProcessed(int state)
        {
            return state == Processed;
        }

        /// <summary>
        /// 是否未读.
        /// </summary>
        /// <param name="todo"></param>
        /// <returns></returns>
        public static bool IsUnReaded(TodoInfo todo)
        {
            return todo.State == UnReaded;
        }

        /// <summary>
        /// 是否已处理.
        /// </summary>
        /// <param name="todo"></param>
        /// <returns></returns>
        public static bool IsProcessed(TodoInfo todo)
        {
            return todo.State == Processed;
        }
    }
}
