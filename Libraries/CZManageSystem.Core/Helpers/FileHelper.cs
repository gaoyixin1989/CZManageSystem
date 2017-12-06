using System;
using System.IO;
using System.Text;

namespace CZManageSystem.Core.Helpers
{
   public class FileHelper
    {
       public static string GetContentType(string extensionName)
       {
           extensionName = extensionName.Replace(".", "");
           var str = "text/html";
            switch (extensionName.ToLower())
            {
                //图片
                case "jpeg":
                case "jpg":
                case "jpe":
                    str = "image/jpeg";
                    break;
                case "gif":
                    str = "image/gif";
                    break;
                case "bmp":
                    str = "image/bmp";
                    break;
                case "png":
                    str = "image/png";
                    break;
                // 文档
                case "rtf":
                    str = "text/rtf";
                    break;
                case "txt":
                    str = "text/plain";
                    break;//
                //excel
                case "xql":
                case "xsd":
                case "xslt":
                    str = "text/xml";
                    break;
                //xml
                case "xls":
                    str = "application/vnd.ms-excel";
                    break;
                case "doc":
                case "docx":
                    str = "application/msword";
                    break;
                case "rtx":
                    str = "text/richtext";
                    break;
                //压缩文件
                case "zip":
                    str = "application/zip";
                    break;
                //可执行文件
                case "exe":
                    str = "application/octet-stream";
                    break;
                case "html":
                case "htx":
                case "htm":
                    str = "text/html";
                    break;
            }

            return str;
        }

       /// <summary>
       /// 换算文件大小
       /// </summary>
       /// <param name="bb"></param>
       /// <returns></returns>
       public static string ShortLength(long bb)
       {
           var k = bb / 1024;
           if (k >= 1)
           {
               var m = k / 1024;
               if (m >= 1)
               {
                   return m + "M";
               }
               return k + "k";
           }
           return bb + "byte";
       }


        /// <summary>
        /// 计算文件大小
        /// </summary>
        /// <param name="bb"></param>
        /// <returns></returns>
        public static string FileSize(string size)
        {
            long site = long.Parse(size);
            string FactSize = "";
            if (site < 1024)
                FactSize = site.ToString("F2") + " Byte";
            else if (site >= 1024.00 && site < 1048576)
                FactSize = (site / 1024.00).ToString("F2") + " K";
            else if (site >= 1048576 && site < 1073741824)
                FactSize = (site / 1024.00 / 1024.00).ToString("F2") + " M";
            else if (site >= 1073741824)
                FactSize = (site / 1024.00 / 1024.00 / 1024.00).ToString("F2") + " G";
            return FactSize;
        }

        /// <summary>
		/// 获取唯一文件名.
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public static string GetUniqueFileName(string fileName)
        {
            string sPostfixName = GetPostfixName(fileName);

            StringBuilder sb = new StringBuilder();
            sb.Append(Guid.NewGuid().ToString());
            sb.Append('.');
            sb.Append(sPostfixName);

            return sb.ToString();
        }

        /// <summary>
        /// 获取文件后缀名.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetPostfixName(string fileName)
        {
            int idx = fileName.LastIndexOf(".");
            return fileName.Substring(idx + 1);
        }

    }
}
