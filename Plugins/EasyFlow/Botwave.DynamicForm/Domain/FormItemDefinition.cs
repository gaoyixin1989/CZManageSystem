using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.DynamicForm.Domain
{
    /// <summary>
    /// 表单项定义类.
    /// </summary>
    [Serializable]
    public class FormItemDefinition
    {
        /// <summary>
        /// 表单项数据类型枚举.
        /// </summary>
        public enum DataType
        {
            /// <summary>
            /// 普通字符串.
            /// </summary>
            String = 0,
            /// <summary>
            /// 数值.
            /// </summary>
            Decimal = 1,
            /// <summary>
            /// 大文本.
            /// </summary>
            Text = 2,
            /// <summary>
            /// 文件.
            /// </summary>
            File = 3
        }

        /// <summary>
        /// 表单项类型枚举.
        /// </summary>
        public enum FormItemType
        {
            /// <summary>
            /// 单行文本.
            /// </summary>
            Text = 0,
            /// <summary>
            /// 多行文本.
            /// </summary>
            TextArea = 1,
            /// <summary>
            /// 下拉框.
            /// </summary>
            DropDownList = 2,
            /// <summary>
            /// 文本标签.
            /// </summary>
            Label = 3,
            /// <summary>
            /// 多选框.
            /// </summary>
            CheckBoxList = 4,
            /// <summary>
            /// 单选框.
            /// </summary>
            RadioButtonList = 5,
            /// <summary>
            /// 日期/时间输入框.
            /// </summary>
            Date = 6,
            /// <summary>
            /// 文件上传.
            /// </summary>
            File = 7,
            /// <summary>
            /// 自增多行文本输入.
            /// </summary>
            IncrementTextArea = 8,
            /// <summary>
            /// 支持复杂形式的html.
            /// </summary>
            Html = 9,
            /// <summary>
            /// 隐藏.
            /// </summary>
            Hidden = 10
        }

        /// <summary>
        /// 表单项前缀类.
        /// </summary>
        public class FormItemPrefix
        {
            /// <summary>
            /// 单行文本.
            /// </summary>
            public const string Text = "txt_";
            /// <summary>
            /// 多行文本.
            /// </summary>
            public const string TextArea = "txa_";
            /// <summary>
            /// 下拉框.
            /// </summary>
            public const string DropDownList = "ddl_";
            /// <summary>
            /// 文本标签.
            /// </summary>
            public const string Label = "lbl_";
            /// <summary>
            /// 多选框.
            /// </summary>
            public const string CheckBoxList = "cbl_";
            /// <summary>
            /// 单选框.
            /// </summary>
            public const string RadioButtonList = "rbl_";

            /// <summary>
            /// 日期/时间输入框.
            /// </summary>
            public const string Date = "dat_";
            /// <summary>
            /// 文件上传.
            /// </summary>
            public const string File = "fil_";
            /// <summary>
            /// 自增多行文本输入.
            /// </summary>
            public const string IncrementTextArea = "ita_";
            /// <summary>
            /// 支持复杂形式的html.
            /// </summary>
            public const string Html = "htm_";
            /// <summary>
            /// 隐藏.
            /// </summary>
            public const string Hidden = "hid_";
        }

        /// <summary>
        /// 表单项验证模板类.
        /// </summary>
        public class ValidateTemplate
        {
            /// <summary>
            /// 是否必须填写.
            /// </summary>
            public bool Require;
            /// <summary>
            /// 验证类型.
            /// </summary>
            public string ValidateType;
            /// <summary>
            /// 操作类型.
            /// </summary>
            public string Operator;
            /// <summary>
            /// 目标.
            /// </summary>
            public string Target;
            /// <summary>
            /// 最大值.
            /// </summary>
            public string MaxVal;
            /// <summary>
            /// 最小值.
            /// </summary>
            public string MinVal;
            /// <summary>
            /// 错误信息.
            /// </summary>
            public string ErrorMessage;
        }

        #region properties

        private Guid id;
        private Guid formDefinitionId;
        private string fname;
        private string name;
        private string comment;
        private DataType itemDataType;
        private FormItemType itemType;
        private string dataSource;
        private string dataBinder;
        private string defaultValue;
        private int left;
        private int top;
        private int width;
        private int height;
        private bool rowExclusive;
        private bool require;
        private string validateType;
        private string maxVal;
        private string minVal;
        private string op;
        private string opTarget;
        private string errorMessage;
        private string showSet;
        private string readonlySet;
        private DateTime createdTime;

        /// <summary>
        /// 表单项定义编号.
        /// </summary>
        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 表单定义Id.
        /// </summary>
        public Guid FormDefinitionId
        {
            get { return formDefinitionId; }
            set { formDefinitionId = value; }
        }

        /// <summary>
        /// 字段名称.
        ///     1、用于在预设固定字段时对应相关字段.
        ///     2、直接兼容XQP方式.
        /// </summary>
        public string FName
        {
            get { return fname; }
            set { fname = value; }
        }

        /// <summary>
        /// 表单项名称.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// 备注信息.
        /// </summary>
        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        /// <summary>
        /// 表单项数据类型.
        /// </summary>
        public DataType ItemDataType
        {
            get { return itemDataType; }
            set { itemDataType = value; }
        }

        /// <summary>
        /// 表单项类型.
        /// </summary>
        public FormItemType ItemType
        {
            get { return itemType; }
            set { itemType = value; }
        }

        /// <summary>
        /// 表单项的数据源.
        /// </summary>
        public string DataSource
        {
            get { return dataSource; }
            set { dataSource = value; }
        }

        /// <summary>
        /// 表单项的数据绑定器.
        /// </summary>
        public string DataBinder
        {
            get { return dataBinder; }
            set { dataBinder = value; }
        }

        /// <summary>
        /// 表单项的默认值.
        /// </summary>
        public string DefaultValue
        {
            get { return defaultValue; }
            set { defaultValue = value; }
        }

        /// <summary>
        /// 表单项的单元格位置离表格左侧距离(单元格数).
        /// </summary>
        public int Left
        {
            get { return left; }
            set { left = value; }
        }

        /// <summary>
        /// 表单项的单元格位置离表格上侧距离(单元格数).
        /// </summary>
        public int Top
        {
            get { return top; }
            set { top = value; }
        }

        /// <summary>
        /// 表单项的宽度.
        /// </summary>
        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        /// <summary>
        /// 表单项的高度.
        /// </summary>
        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        /// <summary>
        /// 表单项是否独占一行.
        /// </summary>
        public bool RowExclusive
        {
            get { return rowExclusive; }
            set { rowExclusive = value; }
        }

        /// <summary>
        /// 表单项是否为必填项.
        /// </summary>
        public bool Require
        {
            get { return require; }
            set { require = value; }
        }

        /// <summary>
        /// 表单项的验证类型.
        /// </summary>
        public string ValidateType
        {
            get { return validateType; }
            set { validateType = value; }
        }

        /// <summary>
        /// 表单项的最大值.
        /// </summary>
        public string MaxVal
        {
            get { return maxVal; }
            set { maxVal = value; }
        }

        /// <summary>
        /// 表单项的最小值.
        /// </summary>
        public string MinVal
        {
            get { return minVal; }
            set { minVal = value; }
        }

        /// <summary>
        /// 表单项的比较类型.
        /// </summary>
        public string Op
        {
            get { return op; }
            set { op = value; }
        }

        /// <summary>
        /// 表单项的比较目标表单项.
        /// </summary>
        public string OpTarget
        {
            get { return opTarget; }
            set { opTarget = value; }
        }

        /// <summary>
        /// 表单项显示的错误信息.
        /// </summary>
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; }
        }

        /// <summary>
        /// 表单项的显示集合.
        /// </summary>
        public string ShowSet
        {
            get { return showSet; }
            set { showSet = value; }
        }

        /// <summary>
        /// 表单项的只读集合.
        /// </summary>
        public string ReadonlySet
        {
            get { return readonlySet; }
            set { readonlySet = value; }
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

        /// <summary>
        /// 构造方法.
        /// </summary>
        public FormItemDefinition()
        { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="fName"></param>
        /// <param name="name"></param>
        public FormItemDefinition(string fName, string name)
        {
            this.fname = fName;
            this.name = name;
        }
    }
}
