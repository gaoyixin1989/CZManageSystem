using System;
using System.Collections.Generic;
using System.Text;

using Botwave.Workflow.Domain;

namespace Botwave.Workflow.Plugin
{
    /// <summary>
    /// 流程评论的后续处理器.
    /// </summary>
    public interface IPostCommentHandler
    {
        /// <summary>
        /// 下一后续处理器对象.
        /// </summary>
        IPostCommentHandler Next { get; set; }

        /// <summary>
        /// 执行.
        /// </summary>
        /// <param name="comment"></param>
        void Execute(Comment comment);
    }
}
