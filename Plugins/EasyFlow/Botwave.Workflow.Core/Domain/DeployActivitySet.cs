using System;
using System.Collections.Generic;

namespace Botwave.Workflow.Domain
{
    /// <summary>
    /// Ҫ�����Ļ������.
    /// </summary>
    public class DeployActivitySet : ActivitySet
    {
        #region �ǳ־û�����

        private string _activityName;

        /// <summary>
        /// �����.
        /// </summary>
        public string ActivityName
        {
            get { return _activityName; }
            set { _activityName = value; }
        }

        #endregion
    }
}
