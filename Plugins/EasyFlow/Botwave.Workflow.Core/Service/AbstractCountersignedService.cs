using System;
using System.Collections.Generic;
using System.Text;

using Botwave.Workflow.Domain;
using Botwave.Workflow.Plugin;

namespace Botwave.Workflow.Service
{
    /// <summary>
    /// 会签服务的抽象基础实现类.
    /// </summary>
    public abstract class AbstractCountersignedService : ICountersignedService
    {
        private IPostCountersignedHandler postCountersignedHandler;

        /// <summary>
        /// 会签的后续处理器对象.
        /// </summary>
        public IPostCountersignedHandler PostCountersignedHandler
        {
            get { return postCountersignedHandler; }
            set { postCountersignedHandler = value; }
        }

        #region ICountersignedService Members

        /// <summary>
        /// 执行会签处理.
        /// </summary>
        /// <param name="countersigned"></param>
        public void Sign(Botwave.Workflow.Domain.Countersigned countersigned)
        {
            DoSign(countersigned);

            Botwave.Commons.Threading.ISyncCaller caller = new SyncPostCountersignedHandler(postCountersignedHandler, countersigned);
            Botwave.Commons.Threading.SyncCallerHost.Run(caller);
        }

        /// <summary>
        /// 获取指定流程活动(步骤)实例编号的待办信息列表.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        public abstract IList<Botwave.Workflow.Domain.TodoInfo> GetTodoList(Guid activityInstanceId);

        /// <summary>
        /// 获取指定流程活动(步骤)实例编号的会签信息列表.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        public abstract IList<Botwave.Workflow.Domain.Countersigned> GetCountersignedList(Guid activityInstanceId);

        #endregion

        /// <summary>
        /// 处理会签.
        /// </summary>
        /// <param name="countersigned"></param>
        protected abstract void DoSign(Botwave.Workflow.Domain.Countersigned countersigned);
    }
}
