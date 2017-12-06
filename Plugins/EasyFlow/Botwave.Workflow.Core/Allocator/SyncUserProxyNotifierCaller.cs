using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Allocator
{
    /// <summary>
    /// 用户授权人通知同步调用服务.
    /// </summary>
    public class SyncUserProxyNotifierCaller : Botwave.Commons.Threading.ISyncCaller
    {
        private IUserProxyNotifier userProxyNotifier;
        private Guid activityInstanceId;
        private IDictionary<string, string> relations;

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="userProxyNotifier"></param>
        /// <param name="activityInstanceId"></param>
        /// <param name="relations"></param>
        public SyncUserProxyNotifierCaller(IUserProxyNotifier userProxyNotifier,
            Guid activityInstanceId, IDictionary<string, string> relations)
        {
            this.userProxyNotifier = userProxyNotifier;
            this.activityInstanceId = activityInstanceId;
            this.relations = relations;
        }

        #region ISyncCaller Members

        /// <summary>
        /// 调用.
        /// </summary>
        public void Call()
        {
            if (null != userProxyNotifier)
            {
                userProxyNotifier.Notify(activityInstanceId, relations);
            }
        }

        #endregion
    }
}
