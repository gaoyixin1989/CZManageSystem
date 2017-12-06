using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Allocator;

namespace Botwave.Workflow.Extension.Service.Plugins
{
    /// <summary>
    /// 授权人通知服务实现类.
    /// </summary>
    public class UserProxyNotifier : IUserProxyNotifier 
    {
        #region IUserProxyNotifier 成员

        /// <summary>
        /// 发送提醒信息.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <param name="relations"></param>
        public void Notify(Guid activityInstanceId, IDictionary<string, string> relations)
        {

        }

        #endregion
    }
}
