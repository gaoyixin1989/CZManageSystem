using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Extension.Domain
{
    /// <summary>
    /// 字段信息.
    /// </summary>
    [Serializable]
    public class FieldInfo
    {
        #region gets / sets

        private Guid id;
        private string fieldName;
        private string displayName;
        private string dataSource;
        private int itemDataType;

        /// <summary>
        /// 字段编号.
        /// </summary>
        public Guid Id
        {
            get { return id; }
            set { id = value; }
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
        /// 显示名称.
        /// </summary>
        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }

        /// <summary>
        /// 字段数据源.
        /// </summary>
        public string DataSource
        {
            get { return dataSource; }
            set { dataSource = value; }
        }

        /// <summary>
        /// 数据类型.
        /// </summary>
        public int ItemDataType
        {
            get { return itemDataType; }
            set { itemDataType = value; }
        }

        /// <summary>
        /// 字段显示标题.
        /// </summary>
        public string HeaderText
        {
            get { return string.Format("({0}){1}", this.fieldName, this.displayName); }
        }

        #endregion

        #region methods

        /// <summary>
        /// 转换为字段控制信息列表.
        /// </summary>
        /// <param name="workflowName"></param>
        /// <param name="activityName"></param>
        /// <returns></returns>
        public IList<FieldControlInfo> ToEmptyFieldControls(string workflowName, string activityName)
        {
            if (string.IsNullOrEmpty(dataSource))
                return new List<FieldControlInfo>();

            string[] fieldValues = dataSource.Split(',', '，');
            if (fieldValues == null || fieldValues.Length == 0)
                return new List<FieldControlInfo>();

            IList<FieldControlInfo> items = new List<FieldControlInfo>();
            foreach (string itemValue in fieldValues)
            {
                string fieldValue = itemValue.Trim();
                FieldControlInfo item = new FieldControlInfo(workflowName, activityName, this.fieldName, fieldValue);
                item.Id = -1; // 设置为 -1 表示为空字段控制项（即不存在与数据库）
                items.Add(item);
            }
            return items;
        }

        #endregion
    }
}
