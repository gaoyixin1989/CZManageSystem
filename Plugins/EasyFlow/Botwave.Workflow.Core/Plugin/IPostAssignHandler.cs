using System;
using System.Collections.Generic;
using System.Text;

using Botwave.Workflow.Domain;

namespace Botwave.Workflow.Plugin
{
    /// <summary>
    /// 任务指派之后的处理器.
    /// </summary>
    public interface IPostAssignHandler
    {
        /// <summary>
        /// 下一迭代的后续处理器.
        /// </summary>
        IPostAssignHandler Next { get; set; }

        /// <summary>
        /// 执行.
        /// </summary>
        /// <param name="assignment"></param>
        void Execute(Assignment assignment);
    }
}
