using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.DynamicForm.Domain
{
    /// <summary>
    /// 表单项实例.
    /// </summary>
    [Serializable]
    public class FormItemInstance
    {
        private Guid id;
        private Guid formInstanceId;
        private Guid formItemDefinitionId;
        private string _value;
        private string textValue;
        private decimal decimalValue;
        private int fileCount;
        private FormItemDefinition definition;

        /// <summary>
        /// 表单项实例 Id.
        /// </summary>
        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 表单实例Id.
        /// </summary>
        public Guid FormInstanceId
        {
            get { return formInstanceId; }
            set { formInstanceId = value; }
        }

        /// <summary>
        /// 表单项定义 Id.
        /// </summary>
        public Guid FormItemDefinitionId
        {
            get { return formItemDefinitionId; }
            set { formItemDefinitionId = value; }
        }

        /// <summary>
        /// 值(通用的文本值).
        /// </summary>
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// 大文本值.
        /// </summary>
        public string TextValue
        {
            get { return textValue; }
            set { textValue = value; }
        }

        /// <summary>
        /// 数字值.
        /// </summary>
        public decimal DecimalValue
        {
            get { return decimalValue; }
            set { decimalValue = value; }
        }

        /// <summary>
        /// 文件/附件数量.
        /// </summary>
        public int FileCount
        {
            get { return fileCount; }
            set { fileCount = value; }
        }

        /// <summary>
        /// 表单项定义对象.
        /// </summary>
        public FormItemDefinition Definition
        {
            get { return definition; }
            set { definition = value; }
        }
    }
}
