using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.XQP.ImportExport
{
    /// <summary>
    /// 表单定义导入
    /// </summary>
    public interface IFormDefinitionImporter
    {
        /// <summary>
        /// 导入表单定义
        /// </summary>
        /// <param name="formDefinitionId">表单定义Id,如果为空则新增,否则更新</param>
        /// <param name="name">表单定义名称</param>
        /// <param name="filename">文件名/路径</param>
        /// <returns>ActionResult实体</returns>
        ActionResult Import(Guid formDefinitionId,
            string name,
            string filename);
    }
}
