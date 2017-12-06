using System;
using System.Collections.Generic;
using System.Text;

using Botwave.Workflow.Domain;

namespace Botwave.Workflow.Plugin
{
    /// <summary>
    /// �������۵ĺ���������.
    /// </summary>
    public interface IPostCommentHandler
    {
        /// <summary>
        /// ��һ��������������.
        /// </summary>
        IPostCommentHandler Next { get; set; }

        /// <summary>
        /// ִ��.
        /// </summary>
        /// <param name="comment"></param>
        void Execute(Comment comment);
    }
}
