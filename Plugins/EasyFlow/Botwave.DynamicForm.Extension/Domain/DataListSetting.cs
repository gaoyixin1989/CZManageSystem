using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Botwave.DynamicForm.Extension.Domain
{
    public class DataListSetting
    {
        private Guid formItemDefinitionId;
        private int type;
        private int columns;
        private int rows;

        /// <summary>
        /// 表单项定义ID
        /// </summary>
        public Guid FormItemDefinitionId
        {
            get { return formItemDefinitionId; }
            set { formItemDefinitionId = value; }
        }

        /// <summary>
        /// DataList的类型（0表示固定行数，1表示可自由增加删除行数）
        /// </summary>
        public int Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// DataList的列数
        /// </summary>
        public int Columns
        {
            get { return columns; }
            set { columns = value; }
        }

        /// <summary>
        /// DataList的行数
        /// </summary>
        public int Rows
        {
            get { return rows; }
            set { rows = value; }
        }
    }
}
