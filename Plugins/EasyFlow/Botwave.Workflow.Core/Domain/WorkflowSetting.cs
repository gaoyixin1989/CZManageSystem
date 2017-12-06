using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Domain
{
    /// <summary>
    /// 流程设置类.
    /// </summary>
    public class WorkflowSetting
    {
        /// <summary>
        /// 默认流程设置实例对象.
        /// </summary>
        public static readonly WorkflowSetting Default = Empty();

        #region gets / sets

        private string _workflowName;
        private string _basicFields;
        private string _workflowAlias;
        private string _aliasImage;
        private int _taskNotifyMinCount;
        private int _undoneMaxCount;
        
        /// <summary>
        /// 流程名称.
        /// </summary>
        public string WorkflowName
        {
            get { return _workflowName; }
            set { _workflowName = value; }
        }

        /// <summary>
        /// 流程别名.
        /// </summary>
        public string WorkflowAlias
        {
            get { return _workflowAlias; }
            set { _workflowAlias = value; }
        }

        /// <summary>
        /// 流程别名图片.
        /// </summary>
        public string AliasImage
        {
            get { return _aliasImage; }
            set { _aliasImage = value; }
        }

        /// <summary>
        /// 属性显示类型.
        /// 用四位按顺序标识 期望完成时间、保密设置、紧急程度、重要级别.
        /// 如:
        ///     1111 代表四个属性都需要;
        ///     1100 代表只需要期望完成时间、保密设置.
        /// </summary>
        public string BasicFields
        {
            get { return _basicFields; }
            set { _basicFields = value; }
        }

        /// <summary>
        /// 任务通知提醒的最小提醒数.当提醒数等于或者超过该值时，方能通知提醒待办人.
        /// </summary>
        public int TaskNotifyMinCount
        {
            get { return _taskNotifyMinCount; }
            set { _taskNotifyMinCount = value; }
        }

        /// <summary>
        /// 未完成工单的最大数.
        /// </summary>
        public int UndoneMaxCount
        {
            get { return _undoneMaxCount; }
            set { _undoneMaxCount = value; }
        }

        /// <summary>
        /// 构造方法.
        /// </summary>
        public WorkflowSetting()
        {
            this._basicFields = "0111";
            this._taskNotifyMinCount = -1;
            this._undoneMaxCount = -1;
        }

        /// <summary>
        /// 构造方法.
        /// </summary>
        public WorkflowSetting(string workflowName, string workflowAlias, string aliasImage)
            : this()
        {
            this._workflowName = workflowName;
            this._workflowAlias = workflowAlias;
            this._aliasImage = aliasImage;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 当前实例的空对象.
        /// </summary>
        /// <returns></returns>
        private static WorkflowSetting Empty()
        {
            return new WorkflowSetting(null, null, null);
        }

        /// <summary>
        /// 获取基本字段数.
        /// </summary>
        /// <returns></returns>
        public int GetBasicFieldsCount()
        {
            //如果没有设置，则认为全有
            const int ALL = 4;
            if (null == _basicFields || _basicFields.Length != ALL)
            {
                return ALL;
            }

            int count = 0;
            for (int i = 0; i < ALL; i++)
            {
                if (_basicFields[i] == '1')
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// 是否包含指定基本字段.
        /// </summary>
        /// <param name="fieldType"></param>
        /// <returns></returns>
        public bool HasBasicField(BasicFieldType fieldType)
        {
            //如果没有设置，则认为有,
            //如果格式错误（不是四位），也认为有.
            if (null == _basicFields || _basicFields.Length != 4)
            {
                return true;
            }

            bool hasField = false;
            switch (fieldType)
            {
                case BasicFieldType.ExpectFinishedTime:
                    hasField = (_basicFields[0] == '1');
                    break;
                case BasicFieldType.Secrecy:
                    hasField = (_basicFields[1] == '1');
                    break;
                case BasicFieldType.Urgency:
                    hasField = (_basicFields[2] == '1');
                    break;
                case BasicFieldType.Importance:
                    hasField = (_basicFields[3] == '1');
                    break;
            }
            return hasField;
        }

        /// <summary>
        /// 基本字段类型.
        /// </summary>
        public enum BasicFieldType
        {
            /// <summary>
            /// 期望完成时间.
            /// </summary>
            ExpectFinishedTime = 0,
            /// <summary>
            /// 保密级别.
            /// </summary>
            Secrecy = 1,
            /// <summary>
            /// 紧急程度.
            /// </summary>
            Urgency = 2,
            /// <summary>
            /// 重要级别.
            /// </summary>
            Importance = 3
        }

        /// <summary>
        /// 保密性描述.
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string ToSecrecyDescription(int status)
        {
            return (status == 1) ? "保密" : "不保密";
        }

        /// <summary>
        /// 紧急程度描述.
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string ToUrgencyDescription(int status)
        {
            string desc = "一般";
            switch (status)
            {
                case 1:
                    desc = "紧急";
                    break;
                case 2:
                    desc = "特别紧急";
                    break;
                case 3:
                    desc = "最紧急";
                    break;
            }
            return desc;
        }

        /// <summary>
        /// 重要级别描述.
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string ToImportanceDescription(int status)
        {
            string desc = "一般";
            switch (status)
            {
                case 1:
                    desc = "重要";
                    break;
                case 2:
                    desc = "特别重要";
                    break;
            }
            return desc;
        }

        #endregion
    }
}
