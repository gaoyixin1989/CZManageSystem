using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Plugin;
using Botwave.Extension.IBatisNet;

namespace Botwave.Workflow.IBatisNet
{
    public class CommentService : ICommentService
    {
        private IPostCommentHandler postCommentHandler;

        public IPostCommentHandler PostCommentHandler
        {
            set { postCommentHandler = value; }
        }

        #region ICommentService Members

        public void AddComment(Comment comment)
        {
            IBatisMapper.Insert("bwwf_Comment_Insert", comment);
            if (comment.WorkflowInstanceId.HasValue)
            {
                Guid workflowInstanceId = comment.WorkflowInstanceId.Value;
                // 更新流程实例的 CommentCount 属性
                IBatisMapper.Update("bwwf_Workflows_Update_CommentCount", workflowInstanceId);
                //// 递增计算 CommentCount.
                //IBatisMapper.Update("bwwf_Workflows_Update_CommentCount_Increased", workflowInstanceId);
            }

            ProcessPostChain(comment);
        }

        public int GetCommentCount(Guid workflowInstanceId)
        {
            return IBatisMapper.Mapper.QueryForObject<int>("bwwf_Workflows_Select_CommentCount", workflowInstanceId);
        }

        public IList<Comment> GetWorkflowComments(Guid worklfowInstanceId)
        {
            return IBatisMapper.Select<Comment>("bwwf_Comment_Select_WorkflowInstanceId", worklfowInstanceId);
        }

        public IList<Comment> GetActivityComments(Guid activityInstanceId)
        {
            return IBatisMapper.Select<Comment>("bwwf_Comment_Select_ActivityInstanceId", activityInstanceId);
        }
        #endregion

        private void ProcessPostChain(Comment comment)
        {
            if (null != postCommentHandler)
            {
                postCommentHandler.Execute(comment);
                IPostCommentHandler next = postCommentHandler.Next;
                while (null != next)
                {
                    next.Execute(comment);
                    next = next.Next;
                }
            }
        }
    }
}
