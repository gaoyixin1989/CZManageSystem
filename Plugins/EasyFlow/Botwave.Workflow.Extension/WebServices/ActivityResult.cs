using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Extension.WebServices
{
    /// <summary>
    /// 流程步骤结果类.
    /// </summary>
    [Serializable]
    public class ActivityResult
    {
        #region gets /sets

        private string activityName;
        private string[] actors;

        /// <summary>
        /// 流程步骤名称.
        /// </summary>
        public string ActivityName
        {
            get { return activityName; }
            set { activityName = value; }
        }

        /// <summary>
        /// 流程步骤操作人列表.
        /// </summary>
        public string[] Actors
        {
            get { return actors; }
            set { actors = value; }
        }
        #endregion

        #region constructor

        /// <summary>
        /// 构造方法.
        /// </summary>
        public ActivityResult()
        {
            this.actors = new string[0];
        }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="capacity"></param>
        public ActivityResult(int capacity)
        {
            this.actors = new string[capacity];
        }

        /// <summary>
        /// 构造方法.
        /// </summary>
        public ActivityResult(string activityName)
            : this()
        {
            this.activityName = activityName;
        }

        /// <summary>
        /// 构造方法.
        /// </summary>
        public ActivityResult(string activityName, string actor)
        {
            this.activityName = activityName;
            this.actors = new string[1];
            if (!string.IsNullOrEmpty(actor))
                actors[0] = actor;
        }

        /// <summary>
        /// 构造方法.
        /// </summary>
        public ActivityResult(string activityName, string[] actors)
        {
            this.activityName = activityName;
            this.actors = actors;
        }
        #endregion

        /// <summary>
        /// 转换为流程步骤结果对象.
        /// </summary>
        /// <param name="activityName"></param>
        /// <param name="actors"></param>
        /// <returns></returns>
        public static ActivityResult ToActivityResult(string activityName, params string[] actors)
        {
            return new ActivityResult(activityName, actors);
        } 
    }
}
