using System;
using System.Collections.Generic;
using System.Text;
using Brettle.Web.NeatUpload;
using com.gmcc.itc;

namespace Botwave.FileManagement
{
    public class FileService
    {
        public static bool UploadFile(InputFile file, AuxiliaryClass.UploadType type, string fileName, string saveDir, com.gmcc.itc.FileManager fileManager)
        {
            switch (type)
            {
                case AuxiliaryClass.UploadType.Localhost:
                     UploadToLocalhost(file, fileName, saveDir);
                     return true;
                case AuxiliaryClass.UploadType.SharedDirectory:
                     UploadToSharedDirectory(file, fileName, saveDir);
                    return true;
                case AuxiliaryClass.UploadType.WebDAV:
                    return UploadToWebDAV(file, fileName, fileManager);
                case AuxiliaryClass.UploadType.FTP://梢后再实现
                    return true;
            }
            return true;
        }

        private static void UploadToLocalhost(InputFile file, string fileName, string saveDir)
        {
            string saveFileName = System.IO.Path.Combine(saveDir, fileName);//合并两个路径为上传到服务器上的全路径
            file.MoveTo(saveFileName, Brettle.Web.NeatUpload.MoveToOptions.Overwrite);
        }

        private static void UploadToSharedDirectory(InputFile file, string fileName, string saveDir)
        {
            string saveFileName = System.IO.Path.Combine(saveDir, fileName);//合并两个路径为上传到服务器上的全路径
            file.MoveTo(saveFileName, Brettle.Web.NeatUpload.MoveToOptions.Overwrite);
        }

        private static bool UploadToWebDAV(InputFile file, string fileName, com.gmcc.itc.FileManager fileManager)
        {
            System.IO.Stream sm = file.FileContent;
            if (!fileManager.UploadFile(file.FileContent, fileName))
            {
                sm.Close();     //释放资源 kamael 2013-02-19
                return false; 
            }
            sm.Close();
            return true;
        }

        /// <summary>
        /// 删除文件服务器上的文件
        /// </summary>
        /// <param name="fileName">上传了的唯一的文件全称</param>
        /// <returns></returns>
        public static bool DeleteFileFromServer(string fileName)
        {
            com.gmcc.itc.FileManager fileManager = new com.gmcc.itc.FileManager(System.Web.HttpContext.Current.Server.MapPath("~/FileSever.config"));
            if (fileManager.DeleteFile(fileName))
            {
                return true;
            }

            return false;
        }
    }
}
