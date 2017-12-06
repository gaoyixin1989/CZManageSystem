using System;
using System.Collections.Generic;

namespace Botwave.Workflow.Domain
{
    /// <summary>
    /// 要发布的活动定义类.
    /// </summary>
    public class DeployActivityDefinition : ActivityDefinition
    {
        /// <summary>
        /// 构造方法.
        /// </summary>
        public DeployActivityDefinition()
        {
            this._prevActivitySets = new List<DeployActivitySet>();
            this._nextActivitySets = new List<DeployActivitySet>();
        }

        #region 非持久化属性

        private AllocatorOption _assignmentAllocator;
        private string _prevActivityNames;
        private string _nextActivityNames;
        private IList<DeployActivitySet> _prevActivitySets;
        private IList<DeployActivitySet> _nextActivitySets;

        /// <summary>
        /// 转交工单的任务分派选项.
        /// </summary>
        public AllocatorOption AssignmentAllocator
        {
            get { return _assignmentAllocator; }
            set { _assignmentAllocator = value; }
        }

        /// <summary>
        /// 前一活动名称字符串(多个名称以","隔开).
        /// </summary>
        public new string PrevActivityNames
        {
            get { return _prevActivityNames; }
            set { _prevActivityNames = value; }
        }

        /// <summary>
        /// 下一活动名称字符串(多个名称以","隔开).
        /// </summary>
        public new string NextActivityNames
        {
            get { return _nextActivityNames; }
            set { _nextActivityNames = value; }
        }

        /// <summary>
        /// 前一活动集合.
        /// </summary>
        public IList<DeployActivitySet> PrevActivitySets
        {
            get { return _prevActivitySets; }
            set { _prevActivitySets = value; }
        }

        /// <summary>
        /// 下一活动集合.
        /// </summary>
        public IList<DeployActivitySet> NextActivitySets
        {
            get { return _nextActivitySets; }
            set { _nextActivitySets = value; }
        }

        #endregion
    }
}
