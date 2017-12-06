using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.XQP.API.Entity
{
    public class SearchResult : ApiResult
    {
        public SearchList[] Search { get; set; }
    }

    public class SearchList
    {
        /// <summary>
        /// 工单唯一标识
        /// </summary>
        public string WorkflowInstanceId { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string WorkflowAlias { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 受理号
        /// </summary>
        public string SheetId { get; set; }

        /// <summary>
        /// 发起人
        /// </summary>
        public string CreatorName { get; set; }

        /// <summary>
        /// 当前步骤
        /// </summary>
        public string ActivityName { get; set; }

        /// <summary>
        /// 当前处理人
        /// </summary>
        public string CurrentActors { get; set; }

        /// <summary>
        /// 发起时间
        /// </summary>
        public string StartedTime { get; set; }
    }
}
