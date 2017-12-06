using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Botwave.Extension.IBatisNet;

namespace Botwave.XQP.Domain
{
    /// <summary>
    /// 流程步骤智能提醒(催单)控制.
    /// </summary>
    public class IntelligentRemindControl
    {
        #region gets/ sets

        private int id;
        private string workflowName;
        private string activityName;
        private int urgency;
        private int stayHours;
        private int remindTimes;
        private string remindType;
        private string creator;
        private DateTime createdTime;

        /// <summary>
        /// ID.
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 流程Name.
        /// </summary>
        public string WorkflowName
        {
            get { return workflowName; }
            set { workflowName = value; }
        }

        /// <summary>
        /// 流程步骤名称.
        /// </summary>
        public string ActivityName
        {
            get { return activityName; }
            set { activityName = value; }
        }

        /// <summary>
        /// 工单紧急程度值.
        /// 0:表示一般;
        /// 1:表示紧急;
        /// 2:表示最紧急.
        /// </summary>
        public int Urgency
        {
            get { return urgency; }
            set { urgency = value; }
        }

        /// <summary>
        /// 允许滞留时间.
        /// </summary>
        public int StayHours
        {
            get { return stayHours; }
            set { stayHours = value; }
        }

        /// <summary>
        /// 提醒方式(1电子邮件，2短信，3电子邮件+短信).
        /// </summary>
        public string RemindType
        {
            get { return remindType; }
            set { remindType = value; }
        }

        /// <summary>
        /// 提醒次数（-1表无限制）.
        /// </summary>
        public int RemindTimes
        {
            get { return remindTimes; }
            set { remindTimes = value; }
        }

        /// <summary>
        /// 创建人.
        /// </summary>
        public string Creator
        {
            get { return creator; }
            set { creator = value; }
        }

        /// <summary>
        /// 创建时间.
        /// </summary>
        public DateTime CreatedTime
        {
            get { return createdTime; }
            set { createdTime = value; }
        }
        #endregion

        #region methods

        /// <summary>
        /// 创建智能提醒控制.
        /// </summary>
        public void Insert()
        {
            if (!IsExists(workflowName, activityName, urgency))
            {
                IBatisMapper.Insert("xqp_IntelligentRemindControl_Insert", this);
            }
            else
            {
                this.Update();
            }
        }

        /// <summary>
        /// 更新智能提醒控制.
        /// </summary>
        /// <returns></returns>
        public int Update()
        {
            return IBatisMapper.Update("xqp_IntelligentRemindControl_Update_ByWorkflowName", this);
        }

        /// <summary>
        /// 检查指定智能提醒控制是否以及存在.
        /// </summary>
        /// <param name="workflowName"></param>
        /// <param name="activityName"></param>
        /// <param name="urgency"></param>
        /// <returns></returns>
        public static bool IsExists(string workflowName, string activityName, int urgency)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("WorkflowName", workflowName);
            parameters.Add("ActivityName", activityName);
            parameters.Add("Urgency", urgency);
            object result = IBatisMapper.Mapper.QueryForObject("xqp_IntelligentRemindControl_Select_IsExists", parameters);
            return (result != null);
        }

        /// <summary>
        /// 获取指定流程名字与流程步骤名称的智能提醒控制列表.
        /// </summary>
        /// <param name="workflowName"></param>
        /// <param name="activityName"></param>
        /// <returns></returns>
        public static IList<IntelligentRemindControl> GetIntelligentRemindControls(string workflowName, string activityName)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("WorkflowName", workflowName);
            parameters.Add("ActivityName", activityName);

            return IBatisMapper.Select<IntelligentRemindControl>("xqp_IntelligentRemindControl_Select_ByWorkflowActivityName", parameters);
        }

        #endregion
    }
}
