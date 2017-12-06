using System;
using System.Collections.Generic;
using System.Text;
using Botwave.DynamicForm.Domain;

namespace Botwave.DynamicForm.Extension.Implements
{
    /// <summary>
    /// 表单设计导入导出部署服务接口
    /// </summary>
    public interface IFormDefinitionDeployService
    {
        FormResult Import(string username, int action, string xml);

        FormResult Export(Guid workflowId);


    }
}
