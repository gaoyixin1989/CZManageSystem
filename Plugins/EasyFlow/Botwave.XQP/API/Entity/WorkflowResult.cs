using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace Botwave.XQP.API.Entity
{
    public  class WorkflowResult:ApiResult
    {
        /// <summary>
        /// 流程列表
        /// </summary>
        public Workflow[] Workflows { get; set; }
    }

    public class Workflow
    {
        /// <summary>
        /// 流程ID
        /// </summary>
        public string WorkflowID { get; set; }

        /// <summary>
        /// 流程名称
        /// </summary>
        public string WorkflowName { get; set; }

        /// <summary>
        /// 流程别名
        /// </summary>
        public string WorkflowAlias { get; set; }

        /// <summary>
        /// 流程分组
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
