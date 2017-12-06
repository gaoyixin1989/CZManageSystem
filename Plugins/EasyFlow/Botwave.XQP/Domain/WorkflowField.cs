using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.XQP.Domain
{
    /// <summary>
    /// 流程字段类.
    /// </summary>
    [Serializable]
    public class WorkflowField
    {
        private string _fieldName;
        private string _headerText;

        /// <summary>
        /// 构造方法.
        /// </summary>
        public WorkflowField()
        { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="headerText"></param>
        public WorkflowField(string fieldName, string headerText)
        {
            this._fieldName = fieldName;
            this._headerText = headerText;
        }

        /// <summary>
        /// 字段名称.
        /// </summary>
        public string FieldName
        {
            get { return _fieldName; }
            set { _fieldName = value; }
        }

        /// <summary>
        /// 表格头部文本.
        /// </summary>
        public string HeaderText
        {
            get { return _headerText; }
            set { _headerText = value; }
        }
    }
}
