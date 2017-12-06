using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Plugin;
using Botwave.Workflow.Extension.Util;

namespace Botwave.Workflow.Extension.Service.Plugins
{
    /// <summary>
    /// 评论的后续处理器实现类.
    /// </summary>
    public class PostCommentHandler : IPostCommentHandler
    {
        #region IPostCommentHandler 成员

        private IPostCommentHandler next = null;

        /// <summary>
        /// 下一后续处理器.
        /// </summary>
        public IPostCommentHandler Next
        {
            get { return next; }
            set { next = value; }
        }

        /// <summary>
        /// 执行.
        /// </summary>
        /// <param name="comment"></param>
        public void Execute(Comment comment)
        {

        }

        #endregion
    }
}
