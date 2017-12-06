using System;
using System.Collections.Generic;

namespace Botwave.Workflow.Domain
{
    /// <summary>
    /// 要发布的活动集合类.
    /// </summary>
    public class DeployActivitySet : ActivitySet
    {
        #region 非持久化属性

        private string _activityName;

        /// <summary>
        /// 活动名称.
        /// </summary>
        public string ActivityName
        {
            get { return _activityName; }
            set { _activityName = value; }
        }

        #endregion
    }
}
