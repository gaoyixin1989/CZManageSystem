using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Domain
{
    /// <summary>
    /// 活动（步骤）定义.
    /// </summary>
    public class ActivityDefinition : AllocatorOption
    {
        #region Get / Set

        private Guid workflowId;
        private string activityName;
        private int state;
        private int sortOrder;
        private Guid prevActivitySetId;
        private Guid nextActivitySetId;
        private Guid parallelActivitySetId;
        private string joinCondition;
        private string splitCondition;
        private string countersignedCondition;
        private string commandRules;
        private string axecutionHandler;
        private string postHandler;
        private string decisionParser;
        private string decisionType;
        private string rejectOption;

        /// <summary>
        /// 流程定义Id.
        /// </summary>
        public Guid WorkflowId
        {
            get { return workflowId; }
            set { workflowId = value; }
        }

        /// <summary>
        /// 活动名称.
        /// </summary>
        public string ActivityName
        {
            get { return activityName; }
            set { activityName = value; }
        }

        /// <summary>
        /// 状态:
        /// 0 初始状态;
        /// 1 进行中;
        /// 2 完成状态.
        /// </summary>
        public int State
        {
            get { return state; }
            set { state = value; }
        }

        /// <summary>
        /// 流程活动（步骤）排序序号.
        /// </summary>
        public int SortOrder
        {
            get { return sortOrder; }
            set { sortOrder = value; }
        }

        /// <summary>
        /// 上一活动集合 Id.
        /// </summary>
        public Guid PrevActivitySetId
        {
            get { return prevActivitySetId; }
            set { prevActivitySetId = value; }
        }

        /// <summary>
        /// 下一活动集合 Id.
        /// </summary>
        public Guid NextActivitySetId
        {
            get { return nextActivitySetId; }
            set { nextActivitySetId = value; }
        }

        /// <summary>
        /// 并行活动集合 Id.
        /// </summary>
        public Guid ParallelActivitySetId
        {
            get { return parallelActivitySetId; }
            set { parallelActivitySetId = value; }
        }

        /// <summary>
        /// 合并条件.
        /// </summary>
        public string JoinCondition
        {
            get { return joinCondition; }
            set { joinCondition = value; }
        }

        /// <summary>
        /// 分支条件.
        /// </summary>
        public string SplitCondition
        {
            get { return splitCondition; }
            set { splitCondition = value; }
        }

        /// <summary>
        /// 会签条件.
        /// </summary>
        public string CountersignedCondition
        {
            get { return countersignedCondition; }
            set { countersignedCondition = value; }
        }

        /// <summary>
        /// 活动处理的执行命令规则.
        /// </summary>
        public string CommandRules
        {
            get { return commandRules; }
            set { commandRules = value; }
        }

        /// <summary>
        /// 流程处理的实现类型.
        /// </summary>
        public string ExecutionHandler
        {
            get { return axecutionHandler; }
            set { axecutionHandler = value; }
        }

        /// <summary>
        /// 后处理的实现类型.
        /// </summary>
        public string PostHandler
        {
            get { return postHandler; }
            set { postHandler = value; }
        }

        /// <summary>
        /// 分支/任务分配决策解析器.
        /// 主要用于在自定义规则析中难以处理需要自定义代码实现的情况.
        /// </summary>
        public string DecisionParser
        {
            get { return decisionParser; }
            set { decisionParser = value; }
        }

        /// <summary>
        /// 分支策类型(有些活动不需要进行路径选择，直接根据上下文决定).
        /// manual手动;
        /// auto自动;
        /// 如果为空则认为是manual.
        /// </summary>
        public string DecisionType
        {
            get { return decisionType; }
            set { decisionType = value; }
        }

        /// <summary>
        /// 默认的退回选择:
        /// initial		退回起始(流程发起/提单)状态;
        /// previous 	上一步;
        /// none		不允许退回;
        /// 其它步骤名称 (可以在流程配置中设置).
        /// </summary>
        public string RejectOption
        {
            get { return rejectOption; }
            set { rejectOption = value; }
        }

        #endregion

        #region 非持久化属性

        private IList<string> prevActivityNames = new List<string>();
        private IList<string> nextActivityNames = new List<string>();

        /// <summary>
        /// 上一活动名称数组.
        /// </summary>
        public IList<string> PrevActivityNames
        {
            get { return prevActivityNames; }
            set { prevActivityNames = value; }
        }

        /// <summary>
        /// 下一活动名称数组.
        /// </summary>
        public IList<string> NextActivityNames
        {
            get { return nextActivityNames; }
            set { nextActivityNames = value; }
        }

        #endregion

        /// <summary>
        /// 是否手动决策类型.
        /// </summary>
        /// <param name="typeDesc">决策类型描述.</param>
        /// <returns></returns>
        public static bool IsManualDecision(string typeDesc)
        {
            if (String.IsNullOrEmpty(typeDesc))
            {
                return true;
            }

            if ("manual".Equals(typeDesc, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        } 
    }
}
