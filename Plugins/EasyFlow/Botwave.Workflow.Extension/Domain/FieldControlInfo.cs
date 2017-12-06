using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Extension.Domain
{
    /// <summary>
    /// 字段控制的属性类.
    /// </summary>
    [Serializable]
    public class FieldControlInfo
    {
        #region gets / sets

        private int id;
        private string workflowName;
        private string activityName;
        private string fieldName;
        private string fieldValue;
        private string extCondition;
        private string targetUsers;
        private DateTime createdTime;
        
        /// <summary>
        /// 字段控制编号.
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 流程名称.
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
        /// 字段名称.
        /// </summary>
        public string FieldName
        {
            get { return fieldName; }
            set { fieldName = value; }
        }

        /// <summary>
        /// 字段值.
        /// </summary>
        public string FieldValue
        {
            get { return fieldValue; }
            set { fieldValue = value; }
        }

        /// <summary>
        /// 扩展条件.
        /// </summary>
        public string ExtCondition
        {
            get { return extCondition; }
            set { extCondition = value; }
        }

        /// <summary>
        /// 模板用户字符串.
        /// </summary>
        public string TargetUsers
        {
            get { return targetUsers; }
            set { targetUsers = value; }
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

        #region constructor

        /// <summary>
        /// 构造方法.
        /// </summary>
        public FieldControlInfo()
        { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="workflowName"></param>
        /// <param name="activityName"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        public FieldControlInfo(string workflowName, string activityName, string fieldName, string fieldValue)
        {
            this.workflowName = workflowName;
            this.activityName = activityName;
            this.fieldName = fieldName;
            this.fieldValue = fieldValue;
        }

        #endregion
    }
}
