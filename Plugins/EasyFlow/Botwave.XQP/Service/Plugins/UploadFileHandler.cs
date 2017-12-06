using System;
using System.Collections.Generic;
using System.Text;
using Botwave.DynamicForm.Plugin;
using Botwave.XQP.Commons;

namespace Botwave.XQP.Service.Plugins
{
    public class UploadFileHandler : IUploadFileHandler
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(UploadFileHandler));

        public string Upload(object file)
        {
            System.Web.HttpPostedFile postedFile = file as System.Web.HttpPostedFile;
            if (postedFile == null)
                return string.Empty;
            return UploadHelper.Upload(postedFile);
        }
    }
}
