using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Domain
{
    /// <summary>
    /// ������裩����.
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
        /// ���̶���Id.
        /// </summary>
        public Guid WorkflowId
        {
            get { return workflowId; }
            set { workflowId = value; }
        }

        /// <summary>
        /// �����.
        /// </summary>
        public string ActivityName
        {
            get { return activityName; }
            set { activityName = value; }
        }

        /// <summary>
        /// ״̬:
        /// 0 ��ʼ״̬;
        /// 1 ������;
        /// 2 ���״̬.
        /// </summary>
        public int State
        {
            get { return state; }
            set { state = value; }
        }

        /// <summary>
        /// ���̻�����裩�������.
        /// </summary>
        public int SortOrder
        {
            get { return sortOrder; }
            set { sortOrder = value; }
        }

        /// <summary>
        /// ��һ����� Id.
        /// </summary>
        public Guid PrevActivitySetId
        {
            get { return prevActivitySetId; }
            set { prevActivitySetId = value; }
        }

        /// <summary>
        /// ��һ����� Id.
        /// </summary>
        public Guid NextActivitySetId
        {
            get { return nextActivitySetId; }
            set { nextActivitySetId = value; }
        }

        /// <summary>
        /// ���л���� Id.
        /// </summary>
        public Guid ParallelActivitySetId
        {
            get { return parallelActivitySetId; }
            set { parallelActivitySetId = value; }
        }

        /// <summary>
        /// �ϲ�����.
        /// </summary>
        public string JoinCondition
        {
            get { return joinCondition; }
            set { joinCondition = value; }
        }

        /// <summary>
        /// ��֧����.
        /// </summary>
        public string SplitCondition
        {
            get { return splitCondition; }
            set { splitCondition = value; }
        }

        /// <summary>
        /// ��ǩ����.
        /// </summary>
        public string CountersignedCondition
        {
            get { return countersignedCondition; }
            set { countersignedCondition = value; }
        }

        /// <summary>
        /// ������ִ���������.
        /// </summary>
        public string CommandRules
        {
            get { return commandRules; }
            set { commandRules = value; }
        }

        /// <summary>
        /// ���̴����ʵ������.
        /// </summary>
        public string ExecutionHandler
        {
            get { return axecutionHandler; }
            set { axecutionHandler = value; }
        }

        /// <summary>
        /// �����ʵ������.
        /// </summary>
        public string PostHandler
        {
            get { return postHandler; }
            set { postHandler = value; }
        }

        /// <summary>
        /// ��֧/���������߽�����.
        /// ��Ҫ�������Զ�������������Դ�����Ҫ�Զ������ʵ�ֵ����.
        /// </summary>
        public string DecisionParser
        {
            get { return decisionParser; }
            set { decisionParser = value; }
        }

        /// <summary>
        /// ��֧������(��Щ�����Ҫ����·��ѡ��ֱ�Ӹ��������ľ���).
        /// manual�ֶ�;
        /// auto�Զ�;
        /// ���Ϊ������Ϊ��manual.
        /// </summary>
        public string DecisionType
        {
            get { return decisionType; }
            set { decisionType = value; }
        }

        /// <summary>
        /// Ĭ�ϵ��˻�ѡ��:
        /// initial		�˻���ʼ(���̷���/�ᵥ)״̬;
        /// previous 	��һ��;
        /// none		�������˻�;
        /// ������������ (��������������������).
        /// </summary>
        public string RejectOption
        {
            get { return rejectOption; }
            set { rejectOption = value; }
        }

        #endregion

        #region �ǳ־û�����

        private IList<string> prevActivityNames = new List<string>();
        private IList<string> nextActivityNames = new List<string>();

        /// <summary>
        /// ��һ���������.
        /// </summary>
        public IList<string> PrevActivityNames
        {
            get { return prevActivityNames; }
            set { prevActivityNames = value; }
        }

        /// <summary>
        /// ��һ���������.
        /// </summary>
        public IList<string> NextActivityNames
        {
            get { return nextActivityNames; }
            set { nextActivityNames = value; }
        }

        #endregion

        /// <summary>
        /// �Ƿ��ֶ���������.
        /// </summary>
        /// <param name="typeDesc">������������.</param>
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
