using System;
using System.Collections.Generic;

namespace Botwave.Workflow.Domain
{
    /// <summary>
    /// 活动集合项.
    /// </summary>
    public class ActivitySet
    {
        private Guid _setId;
        private Guid _activityId;

        /// <summary>
        /// 集合标识.
        /// </summary>
        public Guid SetId
        {
            get { return _setId; }
            set { _setId = value; }
        }

        /// <summary>
        /// 当前的活动标识.
        /// </summary>
        public Guid ActivityId
        {
            get { return _activityId; }
            set { _activityId = value; }
        }
    }
}
