using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.DynamicForm.Plugin
{
    /// <summary>
    /// 上传表单项附件服务.
    /// </summary>
    public interface IUploadFileHandler
    {
        /// <summary>
        /// 上传文件.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        string Upload(object file);
    }
}
