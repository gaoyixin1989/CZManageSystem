using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Allocator
{
    /// <summary>
    /// 当被授权人接受任务时通知授权人.
    /// </summary>
    public interface IUserProxyNotifier
    {
        /// <summary>
        /// 通知授权人.
        /// </summary>
        /// <param name="activityInstanceId">流程活动（步骤）编号.</param>
        /// <param name="relations">被授权人,授权人.</param>
        void Notify(Guid activityInstanceId, IDictionary<string, string> relations);
    }
}
