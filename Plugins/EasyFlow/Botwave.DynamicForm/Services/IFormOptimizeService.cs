using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.DynamicForm.Services
{
    /// <summary>
    /// 表单优化服务接口.
    /// </summary>
    public interface IFormOptimizeService
    {
        /// <summary>
        /// 获取指定表单实例的表单内容字符串.
        /// </summary>
        /// <param name="formInstanceId"></param>
        /// <returns></returns>
        string GetFormContent(Guid formInstanceId);

        /// <summary>
        /// 保存指定表单实例的表单内容数据.
        /// </summary>
        /// <param name="formInstanceId"></param>
        /// <param name="formContent"></param>
        /// <returns></returns>
        void SaveFormContent(Guid formInstanceId, string formContent);
    }
}
