using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.DynamicForm.Plugin
{
    /// <summary>
    /// 上传文件处理的空实现类.
    /// </summary>
    public class EmptyUploadFileHandler : IUploadFileHandler
    {
        #region IUploadFileHandler 成员

        /// <summary>
        /// 上传.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string Upload(object file)
        {
            return string.Empty;
        }

        #endregion
    }
}
