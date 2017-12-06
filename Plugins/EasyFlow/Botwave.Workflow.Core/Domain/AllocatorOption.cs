using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Domain
{
    /// <summary>
    /// 分派选项类(主要用于转交与流程活动分派可处理用户).
    /// </summary>
    public class AllocatorOption
    {
        #region gets / sets

        private Guid activityId;
        private string allocatorResource;
        private string allocatorUsers;
        private string extendAllocators;
        private string extendAllocatorArgs;
        private string defaultAllocator;

        /// <summary>
        /// 流程活动（步骤）定义编号.
        /// </summary>
        public Guid ActivityId
        {
            get { return activityId; }
            set { activityId = value; }
        }

        /// <summary>
        /// 分配资源.
        /// </summary>
        public string AllocatorResource
        {
            get { return allocatorResource; }
            set { allocatorResource = value; }
        }

        /// <summary>
        /// 分配用户字符串（多个用户之间以逗号，即","或者"，"隔开）.
        /// </summary>
        public string AllocatorUsers
        {
            get { return allocatorUsers; }
            set { allocatorUsers = value; }
        }

        /// <summary>
        /// 扩展分配器名称字符串（多个分配器以逗号，即","或者"，"隔开,即"分配器名称1,分配器名称2"）.
        /// </summary>
        public string ExtendAllocators
        {
            get { return extendAllocators; }
            set { extendAllocators = value; }
        }

        /// <summary>
        /// 扩展分配器的输入参数字符串，分配器名称以逗号隔开（分配器参数表示为"分配器名称1:参数1,分配器名称2:参数2"）.
        /// </summary>
        public string ExtendAllocatorArgs
        {
            get { return extendAllocatorArgs; }
            set { extendAllocatorArgs = value; }
        }

        /// <summary>
        /// 默认适用的分配器名称.
        /// </summary>
        public string DefaultAllocator
        {
            get { return defaultAllocator; }
            set { defaultAllocator = value; }
        }
        #endregion

        /// <summary>
        /// 构造方法.
        /// </summary>
        public AllocatorOption()
        { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="allocatorResource"></param>
        /// <param name="allocatorUsers"></param>
        /// <param name="extendAllocators"></param>
        /// <param name="extendAllocatorArgs"></param>
        /// <param name="defaultAllocator"></param>
        public AllocatorOption(string allocatorResource,
            string allocatorUsers,
            string extendAllocators,
            string extendAllocatorArgs,
            string defaultAllocator)
        {
            this.allocatorResource = allocatorResource;
            this.allocatorUsers = allocatorUsers;
            this.extendAllocators = extendAllocators;
            this.extendAllocatorArgs = extendAllocatorArgs;
            this.defaultAllocator = defaultAllocator;
        }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="item"></param>
        public AllocatorOption(ActivityDefinition item)
        {
            this.activityId = item.ActivityId;
            this.allocatorResource = item.AllocatorResource;
            this.allocatorUsers = item.AllocatorUsers;
            this.extendAllocators = item.ExtendAllocators;
            this.extendAllocatorArgs = item.ExtendAllocatorArgs;
            this.defaultAllocator = item.DefaultAllocator;
        }
    }
}
