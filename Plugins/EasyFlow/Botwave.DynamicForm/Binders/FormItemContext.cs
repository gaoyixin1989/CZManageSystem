using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Web;
using Botwave.DynamicForm;
using Botwave.DynamicForm.Domain;

namespace Botwave.DynamicForm.Binders
{
    /// <summary>
    /// 表单项绑定上下文.
    /// <example>
    /// <code>结合 NVelocity 对表单模板内容进行解析.
    /// <![CDATA[
    ///    IFormInstanceService formInstanceService; // 设置其实现对象.
    ///    VelocityEngine engine = VelocityEngineFactory.GetVelocityEngine();
    ///    VelocityContext vc = new VelocityContext();  
    ///    IList<FormItemInstance> instnaceList = formInstanceService.GetFormItemInstancesByFormInstanceId(formInstanceId, true);
    ///    FormItemContext context = new FormItemContext(instnaceList);
    ///    vc.Put("tc", context);
    ///    vc.Put("dc", dict);
    ///    engine.Evaluate(vc, sw, "template tag", template);
    /// ]]>
    /// </code>
    /// </example>
    /// </summary>
    public class FormItemContext
    {
        private IList<FormItemInstance> itemInstanceList;
        private IDictionary<string, string> itemInstanceDict;

        /// <summary>
        /// 构造函数.
        /// </summary>
        /// <param name="fii"></param>
        public FormItemContext(IList<FormItemInstance> fii)
        {
            this.itemInstanceList = fii;
            this.itemInstanceDict = GetItemInstanceDict();
        }

        /// <summary>
        /// 获取指定的表单项值.
        /// </summary>
        /// <param name="name">字段(表单项)名称.</param>
        /// <returns></returns>
        public string GetValue(string name)
        {
            if (itemInstanceDict.ContainsKey(name))
                return itemInstanceDict[name].Replace("\\n", "\r\n");
            return null;
        }

        /// <summary>
        /// 根据字段名称，值判断是否选中.
        /// </summary>
        /// <param name="name">字段名称.</param>
        /// <param name="targetValue">目标值.</param>
        /// <param name="flag">标记.</param>
        /// <returns></returns>
        public string GetFlag(string name, string targetValue, string flag)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(targetValue))
                return "";

            if (itemInstanceDict.ContainsKey(name))
            {
                string[] arrValues = itemInstanceDict[name].Split(',');
                for (int i = 0; i < arrValues.Length; i++)
                {
                    if (targetValue.Equals(arrValues[i]))
                    {
                        return flag;
                    }
                }
            }
            return "";
        }

        /// <summary>
        /// 获取表单项实例字典.
        /// </summary>
        /// <returns></returns>
        private IDictionary<string, string> GetItemInstanceDict()
        {
            IList<FormItemInstance> instanceList = itemInstanceList;
            itemInstanceDict = new Dictionary<string, string>();
            string val = String.Empty;
            if (null != instanceList && instanceList.Count > 0)
            {
                foreach (FormItemInstance item in instanceList)
                {
                    if (item.Definition.ItemDataType == FormItemDefinition.DataType.Text)
                        val = String.IsNullOrEmpty(item.TextValue) ? "" : item.TextValue;
                    else if (item.Definition.ItemDataType == FormItemDefinition.DataType.Decimal)
                        val = String.IsNullOrEmpty(item.DecimalValue.ToString()) ? "" : item.DecimalValue.ToString();
                    else val = String.IsNullOrEmpty(item.Value) ? "" : item.Value;

                    if (!itemInstanceDict.ContainsKey(item.Definition.FName))
                        itemInstanceDict.Add(item.Definition.FName, val);
                }
            }
            //else
            //{
            //    for (int i = 0; i < instanceList.Count; i++)
            //    {
            //        FormItemDefinition item = instanceList[i].Definition;
            //        val = String.IsNullOrEmpty(item.DefaultValue) ? "" : item.DefaultValue;
            //        if (!itemInstanceDict.ContainsKey(item.FName))
            //            itemInstanceDict.Add(item.FName, val);
            //    }
            //}
            return itemInstanceDict;
        }
    }
}
