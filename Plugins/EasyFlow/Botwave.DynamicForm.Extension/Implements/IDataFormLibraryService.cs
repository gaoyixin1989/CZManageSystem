using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Botwave.DynamicForm.Domain;

namespace Botwave.DynamicForm.Extension.Implements
{
    public interface IDataFormLibraryService
    {
        string IsFormLibrary();

        /// <summary>
        /// 插入表单定义
        /// </summary>
        /// <param name="definition"></param>
        /// <returns></returns>
        Guid InsertFormDefinition(FormDefinition definition);

        /// <summary>
        /// 更新表单定义
        /// </summary>
        /// <param name="definition"></param>
        /// <returns></returns>
        int UpdateFormDefinition(FormDefinition definition);

        /// <summary>
        /// 删除表单定义
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int DeleteFormDefinition(Guid id);
    }
}
