using System;
using System.Collections.Generic;

namespace Botwave.Workflow.Domain
{
    /// <summary>
    /// �������.
    /// </summary>
    public class ActivitySet
    {
        private Guid _setId;
        private Guid _activityId;

        /// <summary>
        /// ���ϱ�ʶ.
        /// </summary>
        public Guid SetId
        {
            get { return _setId; }
            set { _setId = value; }
        }

        /// <summary>
        /// ��ǰ�Ļ��ʶ.
        /// </summary>
        public Guid ActivityId
        {
            get { return _activityId; }
            set { _activityId = value; }
        }
    }
}
