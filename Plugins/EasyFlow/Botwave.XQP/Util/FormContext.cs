using System;
using System.Collections.Generic;
using System.Text;
using Botwave.DynamicForm.Renders;

namespace Botwave.XQP.Util
{
    public class FormContext
    {
        private IDictionary<string, object> variables;

        public IDictionary<string, object> Variables
        {
            get { return variables; }
            set { variables = value; }
        }

        public FormContext()
        {
            
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="form"></param>
        public FormContext(System.Collections.Specialized.NameValueCollection form, System.Web.HttpFileCollection files)
        {
            this.variables = GetFormVariables(form, files);
        }

        static IDictionary<string, object> GetFormVariables(System.Collections.Specialized.NameValueCollection form, System.Web.HttpFileCollection files)
        {
            //表单项前缀长度(表单项命名规则：统一前缀 + 类型前缀 + 名称，如 bwdf_txt_项目)
            const int lengthOfTypePrefix = 4;

            //用字典来存储真实的表单项输入值
            IDictionary<string, object> dict = new Dictionary<string, object>();

            //从form中提取与动态表单相关的值
            foreach (string key in form.AllKeys)
            {
                //如果是动态表单项
                if (key.StartsWith(ControlCreator.CONTROL_PREFIX))
                {
                    //带类型前缀的表单项名称(如 txt_项目)
                    string ctlNameWithTypePrefix = key.Substring(ControlCreator.CONTROL_PREFIX.Length);

                    //类型前缀(如 txt)
                    string typePrefix = ctlNameWithTypePrefix.Substring(0, lengthOfTypePrefix);

                    //表单项名称
                    string ctlName = ctlNameWithTypePrefix.Substring(lengthOfTypePrefix);

                    if (!dict.ContainsKey(ctlName))
                        dict.Add(ctlName, form[key]);                   
                }
            }
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
            return dict;
        }
    }
}