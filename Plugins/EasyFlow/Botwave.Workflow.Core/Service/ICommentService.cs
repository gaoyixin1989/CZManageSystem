using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Botwave.Workflow.Domain;

namespace Botwave.Workflow.Service
{
    /// <summary>
    /// �������۷���ӿ�.
    /// </summary>
    public interface ICommentService
    {
        /// <summary>
        /// �������.
        /// </summary>
        /// <param name="comment"></param>
        void AddComment(Comment comment);

        /// <summary>
        /// ��ȡָ������ʵ����������.
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        int GetCommentCount(Guid workflowInstanceId);
        
        /// <summary>
        /// ��ȡָ������ʵ��ID�����������б�.
        /// </summary>
        /// <param name="worklfowInstanceId"></param>
        /// <returns></returns>
        IList<Comment> GetWorkflowComments(Guid worklfowInstanceId);

        /// <summary>
        /// ��ȡָ���ʵ��ID�Ĳ��������б�.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        IList<Comment> GetActivityComments(Guid activityInstanceId);
    }
}
