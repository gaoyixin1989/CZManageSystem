using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Botwave.FileManager
{
    /// <summary>
    /// 文档管理辅助类.
    /// </summary>
    public sealed class FileManagerHelper
    {
        /// <summary>
        /// 文件上传删除服务实现对象.
        /// </summary>
        public static readonly IFileService FileService;

        /// <summary>
        /// 配置中的属性节点.
        /// </summary>
        public static readonly NameValueCollection Properties;

        /// <summary>
        /// 静态构造方法.
        /// </summary>
        static FileManagerHelper()
        {
            FileManagerSectionHandler.Initialize();
            Properties = FileManagerSectionHandler.CurrentProperties;  // 先设置属性.
            Type t = FileManagerSectionHandler.FileServiceType;
            if (t != null)
                FileService = Activator.CreateInstance(t) as IFileService;
            if (t == null)
                FileService = new Support.SharedDirectoryFileService();
        }

        /// <summary>
        /// 获取指定目录的共享目录方式的文件服务实现类.
        /// </summary>
        /// <param name="sharedDirectory">上传文件保存目录.</param>
        /// <returns></returns>
        public static IFileService GetSharedDirectoryService(string sharedDirectory)
        {
            if (string.IsNullOrEmpty(sharedDirectory))
                throw new ArgumentException("sharedDirectory 参数不能为 null 或者空值.");
            return new Support.SharedDirectoryFileService(sharedDirectory);
        }

        /// <summary>
        /// 获取指定文件完全限定名称的文件名(不包括文件路径).
        /// </summary>
        /// <param name="fullFileName">文件完全限定名称, 即包括文件完整路径.</param>
        /// <returns></returns>
        public static string GetFileName(string fullFileName)
        {
            fullFileName = fullFileName.Replace("/", "\\");
            if (fullFileName.EndsWith("\\") || fullFileName.LastIndexOf("\\") < 0)
                return fullFileName;
            int index = fullFileName.LastIndexOf("\\");
            return fullFileName.Substring(index + 1);
        }

        /// <summary>
        /// 获取指定文件名的扩展名.
        /// </summary>
        /// <param name="fileName">要获取扩展名的文件名.</param>
        /// <returns></returns>
        public static string GetFileExtensionName(string fileName)
        {
            if (fileName.LastIndexOf('.') < 0)
                return string.Empty;
            int index = fileName.LastIndexOf('.');
            return fileName.Substring(index);
        }
    }
}
