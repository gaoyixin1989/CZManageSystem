using System;
using System.Collections.Generic;

namespace Botwave.Workflow.Domain
{
    /// <summary>
    /// Ҫ�����Ļ������.
    /// </summary>
    public class DeployActivityDefinition : ActivityDefinition
    {
        /// <summary>
        /// ���췽��.
        /// </summary>
        public DeployActivityDefinition()
        {
            this._prevActivitySets = new List<DeployActivitySet>();
            this._nextActivitySets = new List<DeployActivitySet>();
        }

        #region �ǳ־û�����

        private AllocatorOption _assignmentAllocator;
        private string _prevActivityNames;
        private string _nextActivityNames;
        private IList<DeployActivitySet> _prevActivitySets;
        private IList<DeployActivitySet> _nextActivitySets;

        /// <summary>
        /// ת���������������ѡ��.
        /// </summary>
        public AllocatorOption AssignmentAllocator
        {
            get { return _assignmentAllocator; }
            set { _assignmentAllocator = value; }
        }

        /// <summary>
        /// ǰһ������ַ���(���������","����).
        /// </summary>
        public new string PrevActivityNames
        {
            get { return _prevActivityNames; }
            set { _prevActivityNames = value; }
        }

        /// <summary>
        /// ��һ������ַ���(���������","����).
        /// </summary>
        public new string NextActivityNames
        {
            get { return _nextActivityNames; }
            set { _nextActivityNames = value; }
        }

        /// <summary>
        /// ǰһ�����.
        /// </summary>
        public IList<DeployActivitySet> PrevActivitySets
        {
            get { return _prevActivitySets; }
            set { _prevActivitySets = value; }
        }

        /// <summary>
        /// ��һ�����.
        /// </summary>
        public IList<DeployActivitySet> NextActivitySets
        {
            get { return _nextActivitySets; }
            set { _nextActivitySets = value; }
        }

        #endregion
    }
}
