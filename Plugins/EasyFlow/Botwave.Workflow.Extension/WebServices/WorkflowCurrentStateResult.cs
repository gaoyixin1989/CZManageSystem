using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Extension.WebServices
{
    /// <summary>
    /// 流程当前状态结果.
    /// </summary>
    [Serializable]
    public class WorkflowCurrentStateResult
    {
        /// <summary>
        /// 当前流程状态.
        /// -100 查询失败;
        /// -1 草稿状态;
        /// 0 初始状态;
        /// 1 流程运行中;
        /// 2 流程已完成;
        /// 99 流程已取消.
        /// </summary>
        public int State = 0;

        /// <summary>
        /// 返回查询是否成功.
        /// </summary>
        public bool IsSuccess = true;

        /// <summary>
        /// 结果集合数组.
        /// </summary>
        public ActivityResult[] Activities;
    }
}
