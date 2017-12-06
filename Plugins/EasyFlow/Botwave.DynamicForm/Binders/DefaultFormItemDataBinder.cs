using System;
using System.Collections.Generic;
using System.Text;
using Botwave.DynamicForm.Domain;
using Botwave.DynamicForm.Services;
using NVelocity;
using NVelocity.App;

namespace Botwave.DynamicForm.Binders
{
    /// <summary>
    /// 默认表单绑定器实现类.
    /// </summary>
    public class DefaultFormItemDataBinder : IFormItemDataBinder
    {
        private IFormInstanceService formInstanceService;

        /// <summary>
        /// 表单实例服务.
        /// </summary>
        public IFormInstanceService FormInstanceService
        {
            set { this.formInstanceService = value; }
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

            IList<FormItemInstance> instnaceList = formInstanceService.GetFormItemInstancesByFormInstanceId(formInstanceId, true);
            FormItemContext context = new FormItemContext(instnaceList);

            vc.Put("tc", context);
            vc.Put("dc", dict);

            engine.Evaluate(vc, sw, "template tag", template);
        }

        #endregion
    }
}
