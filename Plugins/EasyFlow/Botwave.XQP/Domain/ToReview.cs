using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Botwave.Workflow.Domain;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow.Extension.Domain;

namespace Botwave.XQP.Domain
{
    /// <summary>
    /// 流程活动待阅信息类.
    /// </summary>
    [Serializable]
    public class ToReview
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(ToReview));

        private int id;
        private Guid activityInstanceId;
        private Guid activityId;
        private string userName;
        private int state;
        private DateTime createdTime;
        private DateTime? reviewTime;
        private string sender;
        private Guid senderActivityInstanceId;

        /// <summary>
        /// 编号.
        /// </summary>
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 流程活动（步骤）实例编号.
        /// </summary>
        public Guid ActivityInstanceId
        {
            get { return activityInstanceId; }
            set { activityInstanceId = value; }
        }

        /// <summary>
        /// 流程活动定义编号.
        /// </summary>
        public Guid ActivityId
        {
            get { return activityId; }
            set { activityId = value; }
        }

        /// <summary>
        /// 待阅人用户名.
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        /// <summary>
        /// 状态.
        /// 0 : 未阅;
        /// 1 : 已阅.
        /// </summary>
        public int State
        {
            get { return state; }
            set { state = value; }
        }

        /// <summary>
        /// 已阅时间.
        /// </summary>
        public DateTime? ReviewTime
        {
            get { return reviewTime; }
            set { reviewTime = value; }
        }

        /// <summary>
        /// 待阅发送人.
        /// </summary>
        public string Sender
        {
            get { return sender; }
            set { sender = value; }
        }

        /// <summary>
        /// 发送人所处理的流程实例编号.
        /// </summary>
        public Guid SenderActivityInstanceId
        {
            get { return senderActivityInstanceId; }
            set { senderActivityInstanceId = value; }
        }

        /// <summary>
        /// 创建时间.
        /// </summary>
        public DateTime CreatedTime
        {
            get { return createdTime; }
            set { createdTime = value; }
        }

        public ToReview()
        { }

        public ToReview(Guid activityInstanceId, string userName, string sender)
        {
            this.activityInstanceId = activityInstanceId;
            this.userName = userName;
            this.sender = sender;
            this.state = 0;
            this.reviewTime = null;
            this.createdTime = DateTime.Now;
        }

        public void Insert()
        {
            if (this.ActivityInstanceId != Guid.Empty && !string.IsNullOrEmpty(this.userName))
            {
                IBatisMapper.Insert("bwwf_Tracking_ToReview_Insert", this);
            }
        }

        public static void UpdateReview(Guid activityInstanceId, string userName)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("State", 1);
            parameters.Add("ActivityInstanceId", activityInstanceId);
            parameters.Add("UserName", userName);

            IBatisMapper.Update("bwwf_Tracking_ToReview_Update_Review_ByUser", parameters);
        }

        public static bool EnableReview(Guid activityInstanceId, string userName)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("UserName", userName);
            parameters.Add("ActivityInstanceId", activityInstanceId);

            int count = IBatisMapper.Mapper.QueryForObject<int>("bwwf_Tracking_ToReview_Select_Count_ByUser", parameters);
            return (count > 0);
        }

        /// <summary>
        /// 获取流程实例的抄送人数据.
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        public static IList<string> GetReviewActors(Guid workflowInstanceId)
        {
            IList<string> results = new List<string>();
            string sql = @"select distinct tr.UserName  from bwwf_Tracking_ToReview tr
	                                        left join vw_bwwf_Tracking_Activities_All ta ON ta.ActivityInstanceId = tr.ActivityInstanceId
                                        where workflowInstanceId = '{0}'";
            DataTable resultTable = IBatisDbHelper.ExecuteDataset(CommandType.Text, string.Format(sql, workflowInstanceId)).Tables[0];
            foreach (DataRow row in resultTable.Rows)
            {
                results.Add(row[0].ToString());
            }
            return results;
        }

        public static DataTable GetReviewTable(string userName, string workflowName, string keywords, int pageIndex, int pageSize, ref int recordCount)
        {
            keywords = Botwave.Commons.DbUtils.FilterSQL(keywords);

            string tableName = "vw_bwwf_Tracking_ToReview_Detail";
            string fieldKey = "ID";
            string fieldShow = @"ActivityInstanceId, UserName, State, ReviewTime, RealName, WorkflowInstanceId, ActivityId, ActivityName, SortOrder, SheetId, Title, Creator, CreatorName, StartedTime,
                      WorkflowAlias, AliasImage, ToReviewActors";
            string fieldOrder = "CreatedTime DESC";

            StringBuilder where = new StringBuilder();
            where.AppendFormat("(State = 0) AND (UserName = '{0}')", userName);
            if (!string.IsNullOrEmpty(workflowName))
                where.AppendFormat(" AND (WorkflowName = '{0}')", workflowName);
            if (!string.IsNullOrEmpty(keywords))
            {
                where.AppendFormat(" AND ((SheetId LIKE '%{0}%')", keywords);
                where.AppendFormat(" OR (Title LIKE '%{0}%')", keywords);
                where.AppendFormat(" OR (WorkflowAlias LIKE '%{0}%')", keywords);
                where.AppendFormat(" OR (CreatorName LIKE '%{0}%')", keywords);
                where.AppendFormat(" OR (ToReviewActors LIKE '%{0}%')", keywords);
                where.AppendFormat(" OR (ActivityInstanceId LIKE '%{0}%'))", keywords);
            }

            return IBatisDbHelper.GetPagedList(tableName, fieldKey, pageIndex, pageSize, fieldShow, fieldOrder, where.ToString(), ref recordCount);
        }

        /// <summary>
        /// 获取待阅列表，Wap版。
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="workflowName"></param>
        /// <param name="keywords"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public static DataTable GetMpReviewTable(string userName, string workflowName, string keywords, int pageIndex, int pageSize, ref int recordCount)
        {
            keywords = Botwave.Commons.DbUtils.FilterSQL(keywords);

            string tableName = "vw_mp_bwwf_Tracking_ToReview_Detail";
            string fieldKey = "ID";
            string fieldShow = @"ActivityInstanceId, UserName, State, ReviewTime, RealName, WorkflowInstanceId, ActivityId, ActivityName, SortOrder, SheetId, Title, Creator, CreatorName, StartedTime,
                      WorkflowAlias, AliasImage, ToReviewActors";
            string fieldOrder = "CreatedTime DESC";

            StringBuilder where = new StringBuilder();
            where.AppendFormat("(State = 0) AND (UserName = '{0}')", userName);
            if (!string.IsNullOrEmpty(workflowName))
                where.AppendFormat(" AND (WorkflowName = '{0}')", workflowName);
            if (!string.IsNullOrEmpty(keywords))
            {
                where.AppendFormat(" AND ((SheetId LIKE '%{0}%')", keywords);
                where.AppendFormat(" OR (Title LIKE '%{0}%')", keywords);
                where.AppendFormat(" OR (WorkflowAlias LIKE '%{0}%')", keywords);
                where.AppendFormat(" OR (CreatorName LIKE '%{0}%')", keywords);
                where.AppendFormat(" OR (ToReviewActors LIKE '%{0}%')", keywords);
                where.AppendFormat(" OR (ActivityInstanceId LIKE '%{0}%'))", keywords);
            }

            return IBatisDbHelper.GetPagedList(tableName, fieldKey, pageIndex, pageSize, fieldShow, fieldOrder, where.ToString(), ref recordCount);
        }

        public static DataTable GetReviewTable(Guid workflowInstanceId)
        {
            string sql = @"SELECT [ID], ActivityInstanceId, UserName, [State], ReviewTime, RealName, WorkflowInstanceId, ActivityId, ActivityName, SortOrder, CreatedTime, SenderName
                                  FROM vw_bwwf_Tracking_ToReview WHERE WorkflowInstanceId = '{0}' ORDER BY SortOrder";
            sql = string.Format(sql, workflowInstanceId);
            return IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        /// <summary>
        /// 获取指定发送者流程活动实例编号的抄送消息表，用于处理提醒页面(notify.aspx).
        /// </summary>
        /// <param name="senderActivityInstanceId"></param>
        /// <returns></returns>
        public static DataTable GetReviewTableBySender(Guid senderActivityInstanceId)
        {
            string sql = @"SELECT [ID], ActivityInstanceId, UserName, [State], ReviewTime, RealName, WorkflowInstanceId, ActivityId, ActivityName, SortOrder, CreatedTime, SenderName
                                  FROM vw_bwwf_Tracking_ToReview WHERE SenderActivityInstanceId = '{0}' ORDER BY [ID]";
            sql = string.Format(sql, senderActivityInstanceId);
            return IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        /// <summary>
        /// 获取已阅数据表.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="workflowName"></param>
        /// <param name="keywords"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public static DataTable GetReviewDoneTable(string userName, string workflowName, string keywords, string startTime, string endTime, int pageIndex, int pageSize, ref int recordCount)
        {
            keywords = Botwave.Commons.DbUtils.FilterSQL(keywords);

            string tableName = "vw_bwwf_Tracking_ToReview_Detail";
            string fieldKey = "ID";
            string fieldShow = @"ActivityInstanceId, UserName, State, ReviewTime, RealName, WorkflowInstanceId, ActivityId, ActivityName, SortOrder, SheetId, Title, Creator, CreatorName, 
                      WorkflowAlias, AliasImage, ToReviewActors";
            string fieldOrder = "CreatedTime DESC";

            StringBuilder where = new StringBuilder();
            where.AppendFormat("(State = 1) AND (UserName = '{0}')", userName);
            if (!string.IsNullOrEmpty(workflowName))
                where.AppendFormat(" AND (WorkflowName = '{0}')", workflowName);
            if (!string.IsNullOrEmpty(keywords))
            {
                where.AppendFormat(" AND ((SheetId LIKE '%{0}%')", keywords);
                where.AppendFormat(" OR (Title LIKE '%{0}%')", keywords);
                where.AppendFormat(" OR (WorkflowAlias LIKE '%{0}%')", keywords);
                where.AppendFormat(" OR (CreatorName LIKE '%{0}%')", keywords);
                where.AppendFormat(" OR (ToReviewActors LIKE '%{0}%')", keywords);
                where.AppendFormat(" OR (ActivityInstanceId LIKE '%{0}%'))", keywords);
            }

            DateTime outputTime;
            if (!string.IsNullOrEmpty(startTime) && DateTime.TryParse(startTime, out outputTime))
                where.AppendFormat(" AND (ReviewTime >= '{0}')", outputTime);
            if (!string.IsNullOrEmpty(endTime) && DateTime.TryParse(endTime, out outputTime))
                where.AppendFormat(" AND (ReviewTime <= '{0}')", outputTime.AddDays(1).AddSeconds(-1));

            return IBatisDbHelper.GetPagedList(tableName, fieldKey, pageIndex, pageSize, fieldShow, fieldOrder, where.ToString(), ref recordCount);
        }

        /// <summary>
        /// 获取已阅数据表，Wap版.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="workflowName"></param>
        /// <param name="keywords"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public static DataTable GetMpReviewDoneTable(string userName, string workflowName, string keywords, string startTime, string endTime, int pageIndex, int pageSize, ref int recordCount)
        {
            keywords = Botwave.Commons.DbUtils.FilterSQL(keywords);

            string tableName = "vw_mp_bwwf_Tracking_ToReview_Detail";
            string fieldKey = "ID";
            string fieldShow = @"ActivityInstanceId, UserName, State, ReviewTime, RealName, WorkflowInstanceId, ActivityId, ActivityName, SortOrder, SheetId, Title, Creator, CreatorName, 
                      WorkflowAlias, AliasImage, ToReviewActors";
            string fieldOrder = "CreatedTime DESC";

            StringBuilder where = new StringBuilder();
            where.AppendFormat("(State = 1) AND (UserName = '{0}')", userName);
            if (!string.IsNullOrEmpty(workflowName))
                where.AppendFormat(" AND (WorkflowName = '{0}')", workflowName);
            if (!string.IsNullOrEmpty(keywords))
            {
                where.AppendFormat(" AND ((SheetId LIKE '%{0}%')", keywords);
                where.AppendFormat(" OR (Title LIKE '%{0}%')", keywords);
                where.AppendFormat(" OR (WorkflowAlias LIKE '%{0}%')", keywords);
                where.AppendFormat(" OR (CreatorName LIKE '%{0}%')", keywords);
                where.AppendFormat(" OR (ToReviewActors LIKE '%{0}%')", keywords);
                where.AppendFormat(" OR (ActivityInstanceId LIKE '%{0}%'))", keywords);
            }

            DateTime outputTime;
            if (!string.IsNullOrEmpty(startTime) && DateTime.TryParse(startTime, out outputTime))
                where.AppendFormat(" AND (ReviewTime >= '{0}')", outputTime);
            if (!string.IsNullOrEmpty(endTime) && DateTime.TryParse(endTime, out outputTime))
                where.AppendFormat(" AND (ReviewTime <= '{0}')", outputTime.AddDays(1).AddSeconds(-1));

            return IBatisDbHelper.GetPagedList(tableName, fieldKey, pageIndex, pageSize, fieldShow, fieldOrder, where.ToString(), ref recordCount);
        }
        public static void SendReviewMessage(string sender, string receiver, string message, Guid activityInstanceId)
        {

        }

        public static string FormtReviewMessage(string title, string creator, string message)
        {
            return message;
        }

        /// <summary>
        /// 发送待阅(抄送).
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <param name="message"></param>
        /// <param name="sender"></param>
        /// <param name="workflowId"></param>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public static bool OnPendingReview(Guid activityInstanceId, string message, string sender, Guid workflowId, Guid activityId)
        {
            ActivityProfile aprofile = ActivityProfile.GetProfile(workflowId, activityId);
            if (aprofile == null || aprofile.IsReview == false || string.IsNullOrEmpty(aprofile.ReviewActors))
                return false;

            string[] profileActors = aprofile.ReviewActors.Split(',', '，');
            if (profileActors == null || profileActors.Length == 0)
                return false;

            // 获取可用的用户列表.
            StringBuilder where = new StringBuilder();
            foreach (string actor in profileActors)
            {
                where.AppendFormat("'{0}',", actor.Trim());
            }
            where.Length = where.Length - 1;
            string sql = string.Format("SELECT DISTINCT [UserName] FROM bw_Users WHERE UserName IN ({0})", where);
            DataTable resultTable = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
            if (resultTable == null || resultTable.Rows.Count == 0)
                return false;

            IList<string> reviewActors = new List<string>();
            foreach (DataRow row in resultTable.Rows)
            {
                string actor = Botwave.Commons.DbUtils.ToString(row[0]);
                if (!reviewActors.Contains(actor))
                    reviewActors.Add(actor);
            }

            return OnPendingReview(activityInstanceId, message, sender, reviewActors);
        }

        /// <summary>
        /// 发送待阅(抄送).
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <param name="message"></param>
        /// <param name="sender"></param>
        /// <param name="workflowId"></param>
        /// <param name="activities"></param>
        /// <returns></returns>
        public static bool OnPendingReview(Guid activityInstanceId, string message, string sender, Guid workflowId, IList<Guid> activities)
        {
            if (activities == null || activities.Count == 0)
                return false;
            NameValueCollection profileActors = new NameValueCollection(StringComparer.OrdinalIgnoreCase);
            foreach (Guid activityId in activities)
            {
                ActivityProfile aprofile = ActivityProfile.GetProfile(workflowId, activityId);
                if (aprofile == null || aprofile.IsReview == false || string.IsNullOrEmpty(aprofile.ReviewActors))
                    continue;

                string[] actors = aprofile.ReviewActors.Split(',', '，');
                if (actors == null || actors.Length == 0)
                    continue;

                foreach (string item in actors)
                    profileActors[item] = item;
            }
            if (profileActors.Count == 0)
                return false;

            // 获取可用的用户列表.
            StringBuilder where = new StringBuilder();
            foreach (string actor in profileActors.Keys)
            {
                where.AppendFormat("'{0}',", actor.Trim());
            }
            where.Length = where.Length - 1;
            string sql = string.Format("SELECT DISTINCT [UserName] FROM bw_Users WHERE UserName IN ({0})", where);
            DataTable resultTable = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
            if (resultTable == null || resultTable.Rows.Count == 0)
                return false;

            IList<string> reviewActors = new List<string>();
            foreach (DataRow row in resultTable.Rows)
            {
                string actor = Botwave.Commons.DbUtils.ToString(row[0]);
                if (!reviewActors.Contains(actor))
                    reviewActors.Add(actor);
            }

            return OnPendingReview(activityInstanceId, message, sender, reviewActors);
        }

        /// <summary>
        /// 发送待阅(抄送).
        /// </summary>
        /// <param name="senderActivityInstanceId"></param>
        /// <param name="message"></param>
        /// <param name="sender"></param>
        /// <param name="workflowId"></param>
        /// <param name="actors"></param>
        /// <returns></returns>
        public static bool OnPendingReview(Guid senderActivityInstanceId, string message, string sender, Guid workflowId, IList<ReviewActor> actors)
        {
            return OnPendingReview(senderActivityInstanceId, message, sender, workflowId, string.Empty, actors);
        }

        /// <summary>
        /// 发送待阅(抄送).
        /// </summary>
        public static bool OnPendingReview(Guid senderActivityInstanceId, string message, string sender, Guid workflowId, string workflowTitle, Guid activityId, Guid activityInstanceId, IList<string> actors)
        {
            IList<ReviewActor> listRA = new List<ReviewActor>();
            foreach (var item in actors)
            {
                listRA.Add(new ReviewActor() { ActivityId = activityId, ActivityInstanceId = activityInstanceId, Actor = item });
            }
            return OnPendingReview(senderActivityInstanceId, message, sender, workflowId, workflowTitle, listRA);
        }

        /// <summary>
        /// 发送待阅(抄送).
        /// </summary>
        /// <param name="senderActivityInstanceId"></param>
        /// <param name="message"></param>
        /// <param name="sender"></param>
        /// <param name="workflowId"></param>
        /// <param name="workflowTitle"></param>
        /// <param name="actors"></param>
        /// <returns></returns>
        public static bool OnPendingReview(Guid senderActivityInstanceId, string message, string sender, Guid workflowId, string workflowTitle, IList<ReviewActor> actors)
        {
            if (actors == null || actors.Count == 0 || string.IsNullOrEmpty(message))
                return false;

            #region 发送时间段处理
            IList<Botwave.XQP.Domain.CZReminderTimeSpan> timeSpans = Botwave.XQP.Domain.CZReminderTimeSpan.Select(workflowId);
            bool isWorkflowLimit = false;
            bool isActivityLimit = false;
            int bh, bm, eh, em;
            foreach (Botwave.XQP.Domain.CZReminderTimeSpan timeSpan in timeSpans)
            {
                if (timeSpan.RemindType == 2)
                    continue;
                bh = timeSpan.BeginHours;
                bm = timeSpan.BeginMinutes;
                eh = timeSpan.EndHours;
                em = timeSpan.EndMinutes;
                TimeSpan tsNow = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
                TimeSpan tsBegin = new TimeSpan(bh, bm, 0);
                TimeSpan tsEnd = new TimeSpan(eh, em, 0);
                if (tsNow >= tsBegin && tsNow < tsEnd)
                {
                    isWorkflowLimit = true;
                    break;
                }
            }
            if (!isWorkflowLimit)
            {
                timeSpans = Botwave.XQP.Domain.CZReminderTimeSpan.SelectByActivityInstanceId(senderActivityInstanceId);
                foreach (Botwave.XQP.Domain.CZReminderTimeSpan timeSpan in timeSpans)
                {
                    if (timeSpan.RemindType == 1)
                        continue;
                    bh = timeSpan.BeginHours;
                    bm = timeSpan.BeginMinutes;
                    eh = timeSpan.EndHours;
                    em = timeSpan.EndMinutes;
                    TimeSpan tsNow = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
                    TimeSpan tsBegin = new TimeSpan(bh, bm, 0);
                    TimeSpan tsEnd = new TimeSpan(eh, em, 0);
                    if (tsNow >= tsBegin && tsNow < tsEnd)
                    {
                        isActivityLimit = true;
                        break;
                    }
                }
            }
            else
            {
                log.Info("[workflowId：" + workflowId + "]此时间段不发送提醒信息");
                return false;
            }
            if (isActivityLimit)
            {
                log.Info("[ActivityInstanceId：" + senderActivityInstanceId + "]此时间段不发送提醒信息");
                return false;
            }
            #endregion

            IList<NotifyActor> notifyActors = IBatisMapper.Select<NotifyActor>("bwwf_WorkflowNotifyActors_Select_Next", senderActivityInstanceId);
            // 循环待阅人.
            foreach (ReviewActor actor in actors)
            {
                string userName = actor.Actor;
                Guid activityInstanceId = actor.ActivityInstanceId;

                ToReview item = new ToReview(activityInstanceId, userName, sender);
                item.ActivityId = actor.ActivityId;
                item.SenderActivityInstanceId = senderActivityInstanceId;
                item.Insert();

                string url = Botwave.XQP.Service.Plugins.WorkflowPostHelper.TransformViewUrlByActivityInstanceId(activityInstanceId.ToString()) + "&type=review";
                Botwave.GMCCServiceHelpers.AsynExtendedPendingJobHelper.AddPendingMsg(userName, sender, workflowTitle, url, ActivityInstance.EntityType, activityInstanceId.ToString());
                int reviewType = WorkflowNotify.GetReviewType(userName, activityInstanceId);
                // 发送短信提醒.
                if (reviewType == 1 || reviewType == 3)
                    Botwave.GMCCServiceHelpers.CZ.AsynNotifyHelper.SendSMS(actor.Actor, sender, message, ActivityInstance.EntityType, actor.ActivityInstanceId.ToString());
                // 发送邮件提醒.
                if (reviewType == 1 || reviewType == 2)
                    Botwave.GMCCServiceHelpers.CZ.AsynNotifyHelper.SendEmail(actor.Actor, sender, message, ActivityInstance.EntityType, actor.ActivityInstanceId.ToString());

            }
            return false;
        }

        /// <summary>
        /// 新增待阅信息.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <param name="message"></param>
        /// <param name="sender"></param>
        /// <param name="reviewActors"></param>
        /// <returns></returns>
        public static bool OnPendingReview(Guid activityInstanceId, string message, string sender, ICollection<string> reviewActors)
        {
            if (string.IsNullOrEmpty(message))
                return false;
            // 循环待阅人.
            foreach (string reviewActor in reviewActors)
            {
                // 插入到待阅列表.
                ToReview item = new ToReview(activityInstanceId, reviewActor, sender);
                item.Insert();

                // 发送消息通知.
                Botwave.GMCCServiceHelpers.CZ.AsynNotifyHelper.SendSMS(reviewActor, sender, message, ActivityInstance.EntityType, activityInstanceId.ToString());
            }
            return true;
        }

        /// <summary>
        /// 获取指定流程活动定义编号列表的抄送人列表.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="activityIDList"></param>
        /// <returns></returns>
        public static IList<ReviewActor> GetReviewActors(Guid workflowId, IList<Guid> activityIDList)
        {
            return GetReviewActors(workflowId, Guid.Empty, activityIDList);
        }

        /// <summary>
        /// 获取指定流程活动定义编号列表的抄送人列表.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="activityInstanceId"></param>
        /// <param name="activityIDList"></param>
        /// <returns></returns>
        public static IList<ReviewActor> GetReviewActors(Guid workflowId, Guid activityInstanceId, IList<Guid> activityIDList)
        {
            IList<ReviewActor> results = new List<ReviewActor>();
            if (activityIDList == null || activityIDList.Count == 0)
                return results;

            foreach (Guid activityId in activityIDList)
            {
                ActivityProfile profile = ActivityProfile.GetProfile(workflowId, activityId);
                if (profile == null || profile.IsReview == false || string.IsNullOrEmpty(profile.ReviewActors))
                    continue;

                string[] actorArray = profile.ReviewActors.Split(',', '，');
                if (actorArray == null || actorArray.Length == 0)
                    continue;
                IList<string> actors = GetActorNames(actorArray);
                foreach (string actor in actors)
                {
                    results.Add(new ReviewActor(activityId, activityInstanceId, actor));
                }
            }
            return results;
        }

        /// <summary>
        /// 获取指定用户名数组的用户列表集合.
        /// </summary>
        /// <param name="actors"></param>
        /// <returns></returns>
        public static IList<string> GetActorNames(string[] actors)
        {
            if (actors == null || actors.Length == 0)
                return new List<string>();

            StringBuilder where = new StringBuilder();
            foreach (string actor in actors)
            {
                where.AppendFormat("'{0}',", actor.Trim());
            }
            where.Length = where.Length - 1;
            string sql = string.Format("SELECT DISTINCT [UserName] FROM bw_Users WHERE UserName IN ({0})", where);
            DataTable resultTable = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
            if (resultTable == null || resultTable.Rows.Count == 0)
                return new List<string>();

            IList<string> results = new List<string>();
            foreach (DataRow row in resultTable.Rows)
            {
                string actor = Botwave.Commons.DbUtils.ToString(row[0]);
                if (!results.Contains(actor))
                    results.Add(actor);
            }
            return results;
        }

        /// <summary>
        /// 删除已推送的待阅.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <param name="actor"></param>
        /// <returns></returns>
        public static int DeletePendingMsg(Guid activityInstanceId, string actor)
        {
            string sql = @"insert into IAMS_PendingMsg(systemid, sysAccount, sysPassword, MsgID, ActionType, IssuedTime)
                                        select systemid, sysAccount, sysPassword, MsgID, '3', (convert(char(19), getdate(), 20)) from IAMS_PendingMsg
                                        where EntityType = @EntityType and ActionType = '1' and EntityId =@EntityId and Owner=@Owner
	                                        and MsgID not in 
	                                        (
		                                        select MsgID from IAMS_PendingMsg where ActionType = '3' and state='1'
	                                        )";
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@EntityType", SqlDbType.VarChar, 50),
                new SqlParameter("@EntityId", SqlDbType.VarChar, 50),
                new SqlParameter("@Owner", SqlDbType.VarChar, 50)
            };
            parameters[0].Value = ActivityInstance.EntityType;
            parameters[1].Value = activityInstanceId.ToString();
            parameters[2].Value = actor;

            return Botwave.Commons.SqlHelper.ExecuteNonQuery(CommandType.Text, sql, parameters);
        }

        #region class

        [Serializable]
        public class ReviewActor
        {
            private Guid activityId;
            private Guid activityInstanceId;
            private string actor;

            public Guid ActivityId
            {
                get { return activityId; }
                set { activityId = value; }
            }

            public Guid ActivityInstanceId
            {
                get { return activityInstanceId; }
                set { activityInstanceId = value; }
            }

            public string Actor
            {
                get { return actor; }
                set { actor = value; }
            }

            public ReviewActor()
            { }

            public ReviewActor(Guid activityId, string actor)
                : this(activityId, Guid.Empty, actor)
            { }

            public ReviewActor(Guid activityId, Guid activityInstanceId, string actor)
            {
                this.activityId = activityId;
                this.activityInstanceId = activityInstanceId;
                this.actor = actor;
            }
        }
        #endregion
    }
}
