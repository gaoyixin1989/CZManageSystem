using System;
using System.Collections.Generic;
using System.Text;

using Botwave.Workflow.Domain;

namespace Botwave.Workflow.Plugin
{
    /// <summary>
    /// 会签的后续处理器接口.
    /// </summary>
    public interface IPostCountersignedHandler
    {
        /// <summary>
        /// 下一后续处理器对象.
        /// </summary>
        IPostCountersignedHandler Next { get; set; }

        /// <summary>
        /// 执行.
        /// </summary>
        /// <param name="comment"></param>
        void Execute(Countersigned comment);
    }
}
