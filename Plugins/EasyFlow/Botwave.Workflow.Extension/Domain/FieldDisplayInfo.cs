using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Extension.Domain
{
    /// <summary>
    /// 显示字段信息.
    /// </summary>
    [Serializable]
    public class FieldDisplayInfo
    {
        #region gets / sets

        private int id;
        private string workflowName;
        private string fieldName;
        private string headerText;
        private int tableType;

        /// <summary>
        /// ID.
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 显示的流程名称.
        /// </summary>
        public string WorkflowName
        {
            get { return workflowName; }
            set { workflowName = value; }
        }

        /// <summary>
        /// 显示的字段名称.
        /// </summary>
        public string FieldName
        {
            get { return fieldName; }
            set { fieldName = value; }
        }

        /// <summary>
        /// 显示的标题文本.
        /// </summary>
        public string HeaderText
        {
            get { return headerText; }
            set { headerText = value; }
        }

        /// <summary>
        /// 显示控制的表格类型.
        /// </summary>
        public int TableType
        {
            get { return tableType; }
            set { tableType = value; }
        }
        #endregion

    }
}
