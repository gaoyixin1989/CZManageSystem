using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.SysManger
{
	public class Activities
	{        
        public Guid WorkflowId { get; set; }//流程活动ID
        public Guid ActivityId { get; set; }//流程ID
        public string ActivityName { get; set; }//流程活动名称
        public int State { get; set; }//流程活动的状态
        public Nullable<int> SortOrder { get; set;}//
		public Nullable<Guid> PrevActivitySetId { get; set; }//上一活动集合ID
        public Nullable<Guid> NextActivitySetId { get; set; }//下一活动集合ID
        public string JoinCondition { get; set; }//合并条件
        public string SplitCondition { get; set; }//分支条件
        public string CommandRules { get; set; }//活动处理的命令执行规则
        public string ExecutionHandler { get; set; }//活动执行逻辑的实现类型
        public string PostHandler { get; set; }//活动执行后的后续处理的实现类型
        public string AllocatorResource { get; set; }//所需资源，用于分配任务。默认名称：resource
        public string AllocatorUsers { get; set; }//可分派任务的用户。默认名称：users
        public string ExtendAllocators { get; set; }//扩展的任务分派实例对象
        public string ExtendAllocatorArgs { get; set; }//扩展的任务分派实例对象参数表达式
        public string DefaultAllocator { get; set; }//默认的任务分派实例对象名称.
        public string DecisionType { get; set; }//分支决策类型：manual手动
                                                //        auto自动
                                                //默认为manual
                                                //(有些活动不需要进行路径选择，直接根据上下文决定)
        public string DecisionParser { get; set;}//
		public string CountersignedCondition { get; set;}//
		public Nullable<Guid> ParallelActivitySetId { get; set;}//
		public string RejectOption { get; set;}//
		public Nullable<int> CanPrint { get; set;}//
		public Nullable<int> PrintAmount { get; set;}//
		public Nullable<int> CanEdit { get; set;}//
		public Nullable<bool> ReturnToPrev { get; set;}//
		public Nullable<bool> IsMobile { get; set;}//
		public Nullable<bool> IsTimeOutContinue { get; set;}//


        //public virtual ICollection<Tracking_Activities_Completed> Tracking_Activities_Completeds { get; set; }
        public virtual ICollection<ActivitySet> ActivitySets { get; set; }

    }
}
