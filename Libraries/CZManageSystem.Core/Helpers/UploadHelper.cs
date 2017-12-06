using CZManageSystem.Core.FileManager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CZManageSystem.Core.Helpers
{
    public class UploadHelper
    {

        public static string UploadRootDirectory = "";

        public static string UploadDirectory = "";

        public static string UserName = "";

        public static string PassWord = "";

        public static string Url = "";

        public UploadHelper(string FilePath)
        {
            UploadRootDirectory = ConfigurationManager.AppSettings["url"].ToString();
            UserName = ConfigurationManager.AppSettings["userName"].ToString();
            PassWord = ConfigurationManager.AppSettings["password"].ToString();
            if(FilePath!="")
                if (ConfigurationManager.AppSettings[FilePath]==null)
                {
                    UploadDirectory = "";
                }
                else
                {
                    UploadDirectory = ConfigurationManager.AppSettings[FilePath].ToString();
                }
            if (string.IsNullOrEmpty(UploadDirectory))
                UploadDirectory = "Tmp";
            Url = UploadRootDirectory ;
        }
        /// <summary>
        /// 上传文件，返回保存的文件名.
        /// </summary>
        /// <param name="postedFile"></param>
        /// <returns></returns>
        public string Upload(HttpPostedFileBase postedFile, string fileName)
        {
            IFileService FileService = new WebDAVFileService(Url, UploadDirectory, UserName, PassWord);

            if (!FileService.Exist(UploadDirectory))
            {
                FileService.MakeFileFloder(UploadDirectory);
            }
            if (FileService.Upload(postedFile.InputStream, System.IO.Path.GetFileName(fileName)))
            {
                return (Url.EndsWith("/") ? Url : Url + "/") + UploadDirectory + "/" + fileName;
            }
            return null;
        }


        public string Download(string file,string filename)
        {
            IFileService FileService = new WebDAVFileService(Url, "", UserName, PassWord);
            FileService.Download(file, filename);
            return null;
        }
    }
}
