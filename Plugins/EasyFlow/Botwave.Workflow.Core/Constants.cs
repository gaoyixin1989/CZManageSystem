using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow
{
    /// <summary>
    /// 流程状态常量.
    /// </summary>
    public static class WorkflowConstants
    {
        /// <summary>
        /// 初始状态.
        /// </summary>
        public static readonly int Initial = 0;
        
        /// <summary>
        /// 流程运行中.
        /// </summary>
        public static readonly int Executing = 1;

        /// <summary>
        /// 流程已完成.
        /// </summary>
        public static readonly int Complete = 2;
        
        /// <summary>
        /// 流程已取消.
        /// </summary>
        public static readonly int Cancel = 99;
    }

}
