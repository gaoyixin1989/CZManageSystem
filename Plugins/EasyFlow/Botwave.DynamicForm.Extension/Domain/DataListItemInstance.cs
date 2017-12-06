using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Botwave.DynamicForm.Extension.Domain
{
    /// <summary>
    /// DataList表单项实例.
    /// </summary>
    [Serializable]
    public class DataListItemInstance
    {
        private Guid id;
        private Guid formInstanceId;
        private Guid dataListItemDefinitionId;
        private string _value;
        private int rowNumber;
        private int colunmNumber;
        private DataListItemDefinition definition;

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
        public Guid DataListItemDefinitionId
        {
            get { return dataListItemDefinitionId; }
            set { dataListItemDefinitionId = value; }
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
        /// 所在的行数
        /// </summary>
        public int RowNumber
        {
            get { return rowNumber; }
            set { rowNumber = value; }
        }

        public int ColumnNumber
        {
            get { return colunmNumber; }
            set { colunmNumber = value; }
        }

        /// <summary>
        /// 表单项定义对象.
        /// </summary>
        public DataListItemDefinition Definition
        {
            get { return definition; }
            set { definition = value; }
        }
    }
}
