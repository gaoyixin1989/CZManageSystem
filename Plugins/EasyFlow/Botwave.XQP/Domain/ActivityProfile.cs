using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Botwave.Extension.IBatisNet;

namespace Botwave.XQP.Domain
{
    /// <summary>
    /// 流程步骤设置类.
    /// </summary>
    public class ActivityProfile
    {
        #region properties

        private int _id;
        private Guid _activityId;
        private string _workflowName;
        private string _activityName;
        private bool _isReview;
        private string _reviewActors;
        private int _reviewActorCount = -1;
        private bool _reviewValidateType = true;
        private bool _visible;
        private string _extendAllocators;
        private string _extendAllocatorArgs;

        /// <summary>
        /// 编号.
        /// </summary>
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 流程活动实例编号(只从数据库中获取，并无插入更新操作).
        /// </summary>
        public Guid ActivityId
        {
            get { return _activityId; }
            set { _activityId = value; }
        }

        /// <summary>
        /// 流程名称.
        /// </summary>
        public string WorkflowName
        {
            get { return _workflowName; }
            set { _workflowName = value; }
        }

        /// <summary>
        /// 流程步骤名称.
        /// </summary>
        public string ActivityName
        {
            get { return _activityName; }
            set { _activityName = value; }
        }

        /// <summary>
        /// 是否启用抄送.
        /// </summary>
        public bool IsReview
        {
            get { return _isReview; }
            set { _isReview = value; }
        }

        /// <summary>
        /// 抄送人.
        /// </summary>
        public string ReviewActors
        {
            get { return _reviewActors; }
            set { _reviewActors = value; }
        }

        /// <summary>
        /// 抄送人员数限制(即最多抄送人数)，-1 表示不限制人数. 0 表示 全选或全不选.
        /// </summary>
        public int ReviewActorCount
        {
            get { return _reviewActorCount; }
            set { _reviewActorCount = value; }
        }

        /// <summary>
        /// 抄送人员选择验证类型。true 表示必须全选或全不选；false 表示自由选择.
        /// </summary>
        public bool ReviewValidateType
        {
            get { return _reviewValidateType ; }
            set { _reviewValidateType = value; }
        }

        /// <summary>
        /// 可见性.
        /// </summary>
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        public int _manageActorCount = -1;
        /// <summary>
        /// 处理人人数.
        /// </summary>
        public int ManageActorCount
        {
            get { return _manageActorCount; }
            set { _manageActorCount = value; }
        }

        public bool _manageVisible;
        /// <summary>
        /// 是否全选
        /// </summary>
        public bool ManageVisible
        {
            get { return _manageVisible; }
            set { _manageVisible = value; }
        }

        public string ExtendAllocators
        {
            get { return _extendAllocators; }
            set { _extendAllocators = value; }
        }

        public string ExtendAllocatorArgs
        {
            get { return _extendAllocatorArgs; }
            set { _extendAllocatorArgs = value; }
        }
        #endregion

        public ActivityProfile()
        { }

        public ActivityProfile(int id, string workflowName, string activityName, bool isReview, string reviewActors, bool visible)
            : this(id, workflowName, activityName, isReview, reviewActors, 0, true, visible)
        { }

        public ActivityProfile(int id, string workflowName, string activityName, bool isReview, string reviewActors, int reviewActorCount, bool reviewValidateType, bool visible)
        {
            this._id = id;
            this._workflowName = workflowName;
            this._activityName = activityName;
            this._isReview = isReview;
            this._reviewActors = reviewActors;
            this._reviewActorCount = reviewActorCount;
            this._reviewValidateType = reviewValidateType;
            this._visible = visible;
        }

        public ActivityProfile(int id, string workflowName, string activityName, bool isReview, string reviewActors, int reviewActorCount, bool reviewValidateType, bool visible, string extendAllocators, string extendAllocatorArgs)
        {
            this._id = id;
            this._workflowName = workflowName;
            this._activityName = activityName;
            this._isReview = isReview;
            this._reviewActors = reviewActors;
            this._reviewActorCount = reviewActorCount;
            this._reviewValidateType = reviewValidateType;
            this._visible = visible;
            this._extendAllocators = extendAllocators;
            this._extendAllocatorArgs = extendAllocatorArgs;
        }

        public ActivityProfile(int id, string workflowName, string activityName, int ManageActorCount, bool ManageVisible)
        {
            this._id = id;
            this._workflowName = workflowName;
            this._activityName = activityName;
            this._manageActorCount = ManageActorCount;
            this._manageVisible = ManageVisible;
        }

        #region method

        public void Insert()
        {
            if (this.ID == -1)
                IBatisMapper.Insert("xqp_ActivityProfile_Insert", this);
            else
                IBatisMapper.Update("xqp_ActivityProfile_UpdateReview_ByID", this);
        }

        public void ManageInsert()
        {
            if (this.ID == -1)
                IBatisMapper.Insert("xqp_ActivityProfile_InsertOne", this);
            else
                IBatisMapper.Update("xqp_ActivityProfile_UpdateReview_ByIDOne", this);
        }

        /// <summary>
        /// 获取指定流程步骤的配置.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public static ActivityProfile GetProfile(Guid workflowId, Guid activityId)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("WorkflowId", workflowId);
            parameters.Add("ActivityId", activityId);
            return IBatisMapper.Load<ActivityProfile>("xqp_ActivityProfile_Select_Profiles_ByWorkflowAndActivityId", parameters);
        }

        /// <summary>
        /// 获取指定流程步骤配置集合列表.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        public static IList<ActivityProfile> GetProfiles(Guid workflowId)
        {
            return IBatisMapper.Select<ActivityProfile>("xqp_ActivityProfile_Select_Profiles_ByWorkflowId", workflowId);
        }

        /// <summary>
        /// 获取配置字典集合.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        public static IDictionary<Guid, ActivityProfile> GetProfileDictionary(Guid workflowId)
        {
            IList<ActivityProfile> profiles = GetProfiles(workflowId);
            IDictionary<Guid, ActivityProfile> results = new Dictionary<Guid, ActivityProfile>();
            foreach (ActivityProfile item in profiles)
            {
                results[item.ActivityId] = item;
            }
            return results;
        }

        /// <summary>
        /// 获取指定流程的下行抄送配置.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public static IList<ActivityProfile> GetNextProfiles(Guid workflowId, Guid activityId)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("WorkflowId", workflowId);
            parameters.Add("ActivityId", activityId);
            return IBatisMapper.Select<ActivityProfile>("xqp_ActivityProfile_Select_NextProfiles_ByWorkflowAndActivityId", parameters);
        }

        /// <summary>
        /// 获取指定流程的抄送的起始配置.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        public static IList<ActivityProfile> GetStartProfiles(Guid workflowId)
        {
            return IBatisMapper.Select<ActivityProfile>("xqp_ActivityProfile_Select_StartProfiles_ByWorkflowId", workflowId);
        }

        #endregion
    }
}
