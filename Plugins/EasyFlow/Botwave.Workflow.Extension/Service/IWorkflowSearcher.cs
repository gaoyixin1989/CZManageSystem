using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Botwave.Commons;

namespace Botwave.Workflow.Extension.Service
{
    /// <summary>
    /// 流程高级查询接口.
    /// </summary>
    public interface IWorkflowSearcher
    {
        /// <summary>
        /// 搜索.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        DataTable Search(AdvancedSearchCondition condition, int pageIndex, int pageSize, ref int recordCount);
    }

    /// <summary>
    /// 流程高级查询条件类.
    /// </summary>
    public class AdvancedSearchCondition
    {
        private const string DefaultMinTime = "2006-10-1";
        private const string DefaultMaxTime = "2018-1-1";

        private string beginTime;
        private string endTime;
        private string title;
        private string sheetId;
        private string workflowName;
        private string activityName;
        private string creatorName;
        private string processorName;
        private string keywords;
        private string actor;
        private bool isProcessed = false;
        private IList<string> workflows;


        private string orderField;
        public string OrderField
        {
            get { return orderField; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    orderField = DbUtils.FilterSQL(value);
                }
            }
        }


        /// <summary>
        /// 开始时间.
        /// </summary>
        public string BeginTime
        {
            get { return beginTime; }
            set 
            {
                if (!String.IsNullOrEmpty(value))
                {
                    if (!Validator.IsDateTime(value))
                    {
                        throw new FormatException("时间格式错误");
                    }
                    beginTime = value;
                }
                else
                {
                    beginTime = DefaultMinTime;
                }
            }
        }

        /// <summary>
        /// 结束时间.
        /// </summary>
        public string EndTime
        {
            get { return endTime; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    if (!Validator.IsDateTime(value))
                    {
                        throw new FormatException("时间格式错误");
                    }
                    endTime = value;
                }
                else
                {
                    endTime = DefaultMaxTime;
                }
            }
        }

        /// <summary>
        /// 工单标题.
        /// </summary>
        public string Title
        {
            get { return title; }
            set 
            {
                if (!String.IsNullOrEmpty(value))
                {
                    title = DbUtils.FilterSQL(value); 
                }                
            }
        }

        /// <summary>
        /// 受理号.
        /// </summary>
        public string SheetId
        {
            get { return sheetId; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    sheetId = DbUtils.FilterSQL(value);
                }
            }
        }

        /// <summary>
        /// 流程名称.
        /// </summary>
        public string WorkflowName
        {
            get { return workflowName; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    workflowName = value;
                }
            }
        }

        /// <summary>
        /// 活动/步骤名称.
        /// </summary>
        public string ActivityName
        {
            get { return activityName; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    activityName = value;
                }
            }
        }

        /// <summary>
        /// 发起人.
        /// </summary>
        public string CreatorName
        {
            get { return creatorName; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    creatorName = DbUtils.FilterSQL(value);
                }
            }
        }

        /// <summary>
        /// 处理人.
        /// </summary>
        public string ProcessorName
        {
            get { return processorName; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    processorName = DbUtils.FilterSQL(value);
                }
            }
        }

        /// <summary>
        /// 内容关键字.
        /// </summary>
        public string Keywords
        {
            get { return keywords; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    keywords = DbUtils.FilterSQL(value);
                }
            }
        }

        /// <summary>
        /// 当前操作人.
        /// </summary>
        public string Actor
        {
            get { return actor; }
            set { actor = value; }
        }

        /// <summary>
        /// 处理过的. true 表示只查询当前用户处理过的工单记录列表.
        /// </summary>
        public bool IsProcessed
        {
            get { return isProcessed; }
            set { isProcessed = value; }
        }

        public IList<string> Workflows { get; set; }

        /// <summary>
        /// 是否属于复杂查询条件.
        /// </summary>
        /// <returns></returns>
        public bool IsComplex()
        {
            //如果ActivityName、ProcessorName、Keywords有一个不为空，则为complex(复杂情况);
            return !(String.IsNullOrEmpty(activityName)
                && String.IsNullOrEmpty(processorName)
                && String.IsNullOrEmpty(Keywords));
        }
    }
}
