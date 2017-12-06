using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.XQP.ImportExport
{
    /// <summary>
    /// 表单定义导出
    /// </summary>
    public interface IFormDefinitionExporter
    {
        /// <summary>
        /// 导出表单定义
        /// </summary>
        /// <param name="formDefinitionId">表单定义Id</param>
        /// <param name="filename">文件名/路径</param>
        /// <returns>标题/名称</returns>
        string Export(Guid formDefinitionId, string filename);
    }
}
