using System;
using System.Collections.Generic;
using System.Text;
using Botwave.DynamicForm.Domain;
using Botwave.DynamicForm.Services;
using NVelocity;
using NVelocity.App;
using Botwave.DynamicForm.Binders;
using Botwave.DynamicForm.Extension.Implements;
using Botwave.DynamicForm.Extension.Domain;

namespace Botwave.DynamicForm.Extension.Binders
{
    /// <summary>
    /// 默认表单绑定器实现类.
    /// </summary>
    public class DefaultFormItemDataBinder : IFormItemDataBinder
    {
        private IFormInstanceService formInstanceService;
        private IFormDefinitionService formDefinitionService;
        private IGetDataService getDataService;

        /// <summary>
        /// 表单实例服务.
        /// </summary>
        public IFormInstanceService FormInstanceService
        {
            set { this.formInstanceService = value; }
        }

        public IFormDefinitionService FormDefinitionService
        {
            get { return formDefinitionService; }
            set { formDefinitionService = value; }
        }

        public IGetDataService GetDataService
        {
            get { return getDataService; }
            set { getDataService = value; }
        }

        #region IFormItemDataBinder Members

        /// <summary>
        /// 绑定模板数据.
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="formInstanceId"></param>
        /// <param name="template"></param>
        /// <param name="dict"></param>
        public void Bind(System.IO.StringWriter sw, Guid formInstanceId, string template, IDictionary<string, object> dict)
        {
            VelocityEngine engine = VelocityEngineFactory.GetVelocityEngine();

            VelocityContext vc = new VelocityContext();

            IDictionary<Guid, FormItemExtension> itemDict = new Dictionary<Guid, FormItemExtension>();

            FormDefinition formDefinition = formDefinitionService.GetFormDefinitionByFormInstanceId(formInstanceId);
            if (formDefinition != null)
            {
                IList<FormItemExtension> items = getDataService.GetFormItemExtensionSettingsByFormdefinitionId(formDefinition.Id);
                foreach (FormItemExtension item in items)
                {
                    if (!itemDict.ContainsKey(item.FormItemDefinitionId))
                        itemDict.Add(item.FormItemDefinitionId, item);
                }
            }
            IList<FormItemInstance> instnaceList = formInstanceService.GetFormItemInstancesByFormInstanceId(formInstanceId, true);
            foreach (FormItemInstance instnace in instnaceList)
            {
                if (itemDict.ContainsKey(instnace.FormItemDefinitionId))
                {
                    if (!string.IsNullOrEmpty(itemDict[instnace.FormItemDefinitionId].DataEncode))
                    {
                        string dataEncode=itemDict[instnace.FormItemDefinitionId].DataEncode;
                        int start = Convert.ToInt32(dataEncode.Split(':', '：')[0]);
                        int end = Convert.ToInt32(dataEncode.Split(':', '：')[1]);
                        if (instnace.Value.Length > start)
                        {
                            if (instnace.Value.Substring(start - 1).Length > end)
                            {
                                string encodeStr = instnace.Value.Substring(start - 1, instnace.Value.Length - end - 1);
                                char[] chars=encodeStr.ToCharArray();
                                for (int i = 0; i < chars.Length; i++)
                                {
                                    chars[i] = '*';
                                }
                                encodeStr = new string(chars);
                                instnace.Value = instnace.Value.Substring(0, start - 1) + encodeStr + instnace.Value.Substring(instnace.Value.Length - end - 1, instnace.Value.Length - 1);
                            }
                        }
                    }
                }
            }
            FormItemContext context = new FormItemContext(instnaceList);

            vc.Put("tc", context);
            vc.Put("dc", dict);

            engine.Evaluate(vc, sw, "template tag", template);
        }

        #endregion
    }
}
