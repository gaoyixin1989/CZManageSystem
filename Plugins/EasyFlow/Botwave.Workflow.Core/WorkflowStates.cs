using System;

namespace Botwave.Workflow
{
    /// <summary>
    /// 流程状态枚举.
    /// </summary>
    public enum WorkflowStates
    {
        /// <summary>
        /// 初始状态.
        /// </summary>
        Initial = 0,
        /// <summary>
        /// 运行中.
        /// </summary>
        Executing = 1,
        /// <summary>
        /// 完成.
        /// </summary>
        Complete = 2,
        /// <summary>
        /// 取消.
        /// </summary>
        Cancel = 99
    }
}
