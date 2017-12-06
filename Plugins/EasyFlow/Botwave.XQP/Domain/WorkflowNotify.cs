using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Botwave.Extension.IBatisNet;

namespace Botwave.XQP.Domain
{
    /// <summary>
    /// 流程处理提醒设置类.
    /// </summary>
    public class WorkflowNotify
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(WorkflowNotify));

        #region gets / sets
        private Guid _userId;
        private string _workflowName;
        private short _notifyType;
        private short _reviewType;

        /// <summary>
        /// 提醒设置的用户编号.
        /// </summary>
        public Guid UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        /// <summary>
        /// 提醒设置的流程名称.
        /// </summary>
        public string WorkflowName
        {
            get { return _workflowName; }
            set { _workflowName = value; }
        }

        /// <summary>
        /// 提醒的类型.
        /// 0 都禁止提醒(禁止邮件和短信提醒).
        /// 1 都允许提醒(默认).
        /// 2 启用邮件提醒.
        /// 3 启用短信提醒.
        /// (即：1、2表示可以邮件提醒；1、3表示可以短信提醒.)
        /// </summary>
        public short NotifyType
        {
            get { return _notifyType; }
            set { _notifyType = value; }
        }
        /// <summary>
        /// 提醒的类型.
        /// 0 都禁止提醒(禁止邮件和短信提醒).
        /// 1 都允许提醒(默认).
        /// 2 启用邮件提醒.
        /// 3 启用短信提醒.
        /// (即：1、2表示可以邮件提醒；1、3表示可以短信提醒.)
        /// </summary>
        public short ReviewType
        {
            get { return _reviewType; }
            set { _reviewType = value; }
        }
        
        #endregion

        public WorkflowNotify()
        { }

        public WorkflowNotify(Guid userId, string workflowName, short notifyType)
        {
            this._userId = userId;
            this._workflowName = workflowName;
            this._notifyType = notifyType;
        }

        public WorkflowNotify(Guid userId, string workflowName, short notifyType, short reviewType)
        {
            this._userId = userId;
            this._workflowName = workflowName;
            this._notifyType = notifyType;
            this._reviewType = reviewType;
        }

        /// <summary>
        /// 插入流程提醒设置.
        /// </summary>
        public void Insert()
        {
            IBatisMapper.Insert("xqp_WorkflowNotify_Insert", this);
        }

        /// <summary>
        /// 插入流程提醒设置列表.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static bool Insert(Guid userId, IList<WorkflowNotify> items)
        {
            IBatisMapper.Mapper.BeginTransaction();
            try
            {
                WorkflowNotify.Delete(userId);
                if (items != null && items.Count > 0)
                {
                    foreach (WorkflowNotify item in items)
                    {
                        item.Insert();
                    }
                }
                IBatisMapper.Mapper.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                IBatisMapper.Mapper.RollBackTransaction();
                return false;
            }
        }

        /// <summary>
        /// 更新流程提醒设置.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int Update()
        {
            return IBatisMapper.Update("xqp_WorkflowNotify_Update", this);
        }

        /// <summary>
        /// 删除指定用户的提醒设置数据.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static int Delete(Guid userId)
        {
            return IBatisMapper.Delete("xqp_WorkflowNotify_Delete", userId);
        }

        /// <summary>
        /// 获取指定用户和步骤实例的提醒类型(0 都禁止提醒.1 都允许提醒.2 启用邮件提醒.3 启用短信提醒.).
        /// </summary>
        /// <param name="userName">用户名.</param>
        /// <param name="activityInstanceId">步骤实例编号.</param>
        /// <returns>(0 都禁止提醒.1 都允许提醒.2 启用邮件提醒.3 启用短信提醒.)默认值为 1.</returns>
        public static int GetNotifyType(string userName, Guid activityInstanceId)
        {
            Hashtable parameters = new Hashtable(2);
            parameters.Add("UserName", userName);
            parameters.Add("ActivityInstanceId", activityInstanceId);

            object result = IBatisMapper.Mapper.QueryForObject("xqp_WorkflowNotify_Select_NotifyType", parameters);
            if (result == null)
                return 1; // 默认情况下都允许
            return (int)result;
        }

        /// <summary>
        /// 获取指定用户和步骤实例的待阅提醒类型(0 都禁止提醒.1 都允许提醒.2 启用邮件提醒.3 启用短信提醒.).
        /// </summary>
        /// <param name="userName">用户名.</param>
        /// <param name="activityInstanceId">步骤实例编号.</param>
        /// <returns>(0 都禁止提醒.1 都允许提醒.2 启用邮件提醒.3 启用短信提醒.)默认值为 1.</returns>
        public static int GetReviewType(string userName, Guid activityInstanceId)
        {
            Hashtable parameters = new Hashtable(2);
            parameters.Add("UserName", userName);
            parameters.Add("ActivityInstanceId", activityInstanceId);

            object result = IBatisMapper.Mapper.QueryForObject("xqp_WorkflowNotify_Select_ReviewType", parameters);
            if (result == null)
                return 1; // 默认情况下都允许
            return (int)result;
        }

        /// <summary>
        /// 获取指定用户与流程的消息通知设置类型.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        public static int GetNotifyTypeByWorkflow(string userName, Guid workflowId)
        {
            Hashtable parameters = new Hashtable(2);
            parameters.Add("UserName", userName);
            parameters.Add("WorkflowId", workflowId);
            WorkflowNotify result = IBatisMapper.Load<WorkflowNotify>("xqp_WorkflowNotify_Select_Notify_ByUserAndWorkflow", parameters);
            return (result == null ? 1 : result.NotifyType);
        }

        /// <summary>
        /// 获取指定用户的流程提醒设置表数据.用来显示.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static DataTable GetNotifyTable(Guid userId)
        {
            string sql = @"
            SELECT DISTINCT w.WorkflowName, 
                CAST((CASE WHEN (wn.NotifyType = 0) OR (wn.NotifyType = 3) THEN 0 ELSE 1 END) AS bit) AS EnableEmail, 
                CAST((CASE WHEN (wn.NotifyType = 0) OR (wn.NotifyType = 2) THEN 0 ELSE 1 END) AS bit ) AS EnableSms,
                CAST((CASE WHEN (wn.ReviewType = 0) OR (wn.ReviewType = 3) THEN 0 ELSE 1 END) AS bit) AS EnableReviewEmail, 
                CAST((CASE WHEN (wn.ReviewType = 0) OR (wn.ReviewType = 2) THEN 0 ELSE 1 END) AS bit ) AS EnableReviewSms
            FROM bwwf_Workflows w LEFT JOIN  (
                    SELECT nt.WorkflowName, nt.NotifyType, nt.ReviewType
                    FROM xqp_WorkflowNotify nt 
                    WHERE (nt.UserId = '{0}')
                ) 
                wn ON w.WorkflowName = wn.WorkflowName 
            WHERE (w.IsDeleted = 0) and (w.IsCurrent = 1) and (w.Enabled = 1)
            ORDER BY w.WorkflowName";
            sql = string.Format(sql, userId.ToString());
            DataSet ds = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql);
            return ds.Tables[0];
        }
    }
}
