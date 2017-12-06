using System;
using System.Text;
using System.IO;

namespace Botwave.Commons
{
	/// <summary>
	/// 文件辅助类.
	/// </summary>
	public static class FileUtils
	{
        /// <summary>
        /// 创建指定文件.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="content"></param>
		public static void CreateFile(string fileName, string content)
		{
			using (StreamWriter sw = new StreamWriter(fileName, false, Encoding.GetEncoding(936)))
			{
				sw.Write(content);
			}
		}

        /// <summary>
        /// 添加指定内容到指定文件.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="content"></param>
		public static void AppendToFile(string fileName, string content)
		{
			StreamWriter sw;

			if (!File.Exists(fileName))
			{
				sw = File.CreateText(fileName);
			}
			else
			{
				sw = File.AppendText(fileName);
			}

			sw.Write(content);
			sw.Flush();
			sw.Close();
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

		/// <summary>
		/// 根据路径（非URL）获取文件名.
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static string GetFileNameByPath(string filePath)
		{
			if (String.IsNullOrEmpty(filePath))
			{
                throw new ArgumentException("文件名不能为空", "filePath");
			}
		
			int idx = filePath.LastIndexOf('\\');
			if (idx == -1)
			{
				return "";
			}

			return filePath.Substring(idx + 1);
		}

		/// <summary>
		/// 根据URL获取文件名.
		/// </summary>
		/// <param name="fileUrl"></param>
		/// <returns></returns>
		public static string GetFileNameByUrl(string fileUrl)
		{
			if (String.IsNullOrEmpty(fileUrl))
			{
				throw new ArgumentException("文件url不能为空", "fileUrl");
			}
		
			int idx = fileUrl.LastIndexOf('/');
			if (idx == -1 || fileUrl[idx - 1] == '/')
			{
				return "";
			}

			string str = fileUrl.Substring(idx + 1);
			int i = str.IndexOf('?');
			if (i != -1)
			{
				return str.Substring(0, i);
			}

			return fileUrl.Substring(idx + 1);
		}

        /// <summary>
        /// 获取文件名称（不包括文件后缀名）.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetFileName(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
                return String.Empty;

            int idx = fileName.LastIndexOf(".");
            if (idx == -1)
                return fileName;
            return fileName.Remove(idx);
        }
	}
}
