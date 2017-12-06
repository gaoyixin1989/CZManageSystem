using System;
using System.Collections.Generic;
using Botwave.Workflow.Domain;

namespace Botwave.Workflow.Service
{
    /// <summary>
    /// ����Ϸ���ӿ�.
    /// </summary>
    public interface IActivitySetService
    {
        /// <summary>
        /// ��ȡָ������ϱ�ʶ�Ļ����.
        /// </summary>
        /// <param name="activitySetId"></param>
        /// <returns></returns>
        IList<ActivitySet> GetActivitySets(Guid activitySetId);

        /// <summary>
        /// ��ȡָ������ϱ�ʶ�Ļ ID �б�.
        /// </summary>
        /// <param name="activitySetId"></param>
        /// <returns></returns>
        IList<Guid> GetActivityIdSets(Guid activitySetId);

        /// <summary>
        /// ��ȡָ������ ID ����һ������б�.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        IList<ActivitySet> GetNextActivitySets(Guid workflowId);
    }
}
