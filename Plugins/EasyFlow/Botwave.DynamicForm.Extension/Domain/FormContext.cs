using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using Botwave.DynamicForm.Domain;
using Botwave.DynamicForm.Renders;

namespace Botwave.DynamicForm.Extension.Domain
{
    /// <summary>
    /// 表单上下文类.
    /// <example>
    /// <code> 使用示例：
    /// HttpRequest request; // 页面(Page) 的 Request 属性对象.
    /// FormContext context = new FormContext(request.Form, request.Files);
    /// context.Variables;  // 表单变量对象字典
    /// </code>
    /// </example>
    /// </summary>
    public class FormDataListContext
    {
        #region properties
        private string fName;
        private int columnNumber;
        private int rowNumber;
        private string _value;

        private IDictionary<string, object> variables;

        public string FName
        {
            get { return fName; }
            set { fName = value; }
        }

        public int ColumnNumber
        {
            get { return columnNumber; }
            set { columnNumber = value; }
        }

        public int RowNumber
        {
            get { return rowNumber; }
            set { rowNumber = value; }
        }

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }


        /// <summary>
        /// 表单变量名值对字典集合.
        /// </summary>
        public IDictionary<string, object> Variables
        {
            get { return variables; }
            set { variables = value; }
        }

        #endregion

        /// <summary>
        /// 构造方法.
        /// </summary>
        public FormDataListContext()
        {

        }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="form">页面表单变量.</param>
        /// <param name="files">上传的文件集合.</param>
        public FormDataListContext(NameValueCollection form, HttpFileCollection files)
        {
            this.variables = GetFormVariables(form, files);
        }

        /// <summary>
        /// 获取表单变量字典.
        /// </summary>
        /// <param name="form">Web 请求的表单字段值字典.</param>
        /// <param name="files">Web 请求的上传文件集合.</param>
        /// <returns></returns>
        protected static IDictionary<string, object> GetFormVariables(NameValueCollection form, HttpFileCollection files)
        {
            //表单项前缀长度(表单项命名规则：统一前缀 + 类型前缀 + 名称，如 bwdf_txt_项目)
            const int lengthOfTypePrefix = 4;
            //用字典来存储真实的表单项输入值.
            IDictionary<string, object> dict = new Dictionary<string, object>();

            //从form中提取与动态表单相关的值.
            if (form != null && form.Count > 0)
            {
                foreach (string key in form.AllKeys)
                {
                    //如果是动态表单项.
                    if (key.StartsWith(ControlCreator.CONTROL_PREFIX))
                    {
                        //带类型前缀的表单项名称(如 txt_项目).
                        string ctlNameWithTypePrefix = key.Substring(ControlCreator.CONTROL_PREFIX.Length);
                        //类型前缀(如 txt).
                        string typePrefix = ctlNameWithTypePrefix.Substring(0, lengthOfTypePrefix);
                        //表单项名称.
                        string ctlName = ctlNameWithTypePrefix.Substring(lengthOfTypePrefix);

                        if (!dict.ContainsKey(ctlName))
                            dict.Add(ctlName, form[key]);
                    }
                }
            }
            if (files != null && files.Count > 0)
            {
                foreach (string key in files.AllKeys)
                {
                    if (key.StartsWith(ControlCreator.CONTROL_PREFIX))
                    {
                        string ctlNameWithTypePrefix = key.Substring(ControlCreator.CONTROL_PREFIX.Length);
                        string typePrefix = ctlNameWithTypePrefix.Substring(0, lengthOfTypePrefix);
                        string ctlName = ctlNameWithTypePrefix.Substring(lengthOfTypePrefix);

                        if (!dict.ContainsKey(ctlName))
                            dict.Add(ctlName, files[key]);
                    }
                }
            }
            return dict;
        }
    }
}
