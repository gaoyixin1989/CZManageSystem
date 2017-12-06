using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Botwave.DynamicForm.Extension.Domain
{
    public class FormItemExtension
    {
        private Guid formItemDefinitionId;
        /// <summary>
        /// 表单项定义ID
        /// </summary>
        public Guid FormItemDefinitionId
        {
            get { return formItemDefinitionId; }
            set { formItemDefinitionId = value; }
        }

        private int getDataType;
        /// <summary>
        /// 数据获取方式（0：不做任何操作；1：表单计算；2：SQL方式获取；3：WebService方式获取）
        /// </summary>
        public int GetDataType
        {
            get { return getDataType; }
            set { getDataType = value; }
        }


        private string getDataSource;
        /// <summary>
        /// 获取数据的数据源
        /// </summary>
        public string GetDataSource
        {
            get { return getDataSource; }
            set { getDataSource = value; }
        }

        
        private string sourceString;
        /// <summary>
        ///获取数据字符串 
        /// </summary>
        public string SourceString
        {
            get { return sourceString; }
            set { sourceString = value; }
        }

        private int fillDataType;
        /// <summary>
        /// 数据填充方式
        /// </summary>
        public int FillDataType
        {
            get { return fillDataType; }
            set { fillDataType = value; }
        }

        private string fillDataSource;
        /// <summary>
        /// 填充数据的数据源
        /// </summary>
        public string FillDataSource
        {
            get { return fillDataSource; }
            set { fillDataSource = value; }
        }

        private string fillDataString;
        /// <summary>
        /// 填充数据字符串
        /// </summary>
        public string FillDataString
        {
            get { return fillDataString; }
            set { fillDataString = value; }
        }
       
        private int validateType;

        /// <summary>
        /// js校验类型
        /// </summary>
        public int ValidateType
        {
            get { return validateType; }
            set { validateType = value; }
        }

        private string validateDescription;

        /// <summary>
        /// js校验类型描述
        /// </summary>
        public string ValidateDescription
        {
            get { return validateDescription; }
            set { validateDescription = value; }
        }

        private string validateFunction;

        /// <summary>
        /// js校验方法
        /// </summary>
        public string ValidateFunction
        {
            get { return validateFunction; }
            set { validateFunction = value; }
        }

        private string itemsLinkageJson;
        
        /// <summary>
        /// 字段联动json字符串，当联动方式为SQL时则为SQL语句
        /// </summary>
        public string ItemsLinkageJson
        {
            get { return itemsLinkageJson; }
            set { itemsLinkageJson = value; }
        }

        private int itemsLinkageType;

        /// <summary>
        /// 字段联动方式（1：字段内容联动、2：通过sql联动(未开发)）
        /// </summary>
        public int ItemsLinkageType
        {
            get { return itemsLinkageType; }
            set { itemsLinkageType = value; }
        }

        private string itemsLinkageSource;
        /// <summary>
        /// 字段联动方式为sql联动时的数据源
        /// </summary>
        public string ItemsLinkageSource
        {
            get { return itemsLinkageSource; }
            set { itemsLinkageSource = value; }
        }

        private string dataEncode;
        /// <summary>
        /// 字段内容加密（格式：start:数字;end:数字）
        /// </summary>
        public string DataEncode
        {
            get { return dataEncode; }
            set { dataEncode = value; }
        }

        private int itemsRulesType;
        /// <summary>
        /// 字段规则类型：0：不做任何设置 1：通过选项联动
        /// </summary>
        public int ItemsRulesType
        {
            get { return itemsRulesType; }
            set { itemsRulesType = value; }
        }

        private string itemsRulesJson;
        /// <summary>
        /// 字段联动的Json格式
        /// </summary>
        public string ItemsRulesJson
        {
            get { return itemsRulesJson; }
            set { itemsRulesJson = value; }
        }
    }
}
