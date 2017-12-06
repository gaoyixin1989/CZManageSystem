using System;
using System.Text;
using System.IO;

namespace Botwave.Commons
{
	/// <summary>
	/// �ļ�������.
	/// </summary>
	public static class FileUtils
	{
        /// <summary>
        /// ����ָ���ļ�.
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
        /// ���ָ�����ݵ�ָ���ļ�.
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
		/// ��ȡΨһ�ļ���.
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
		/// ��ȡ�ļ���׺��.
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public static string GetPostfixName(string fileName)
		{
			int idx = fileName.LastIndexOf(".");
			return fileName.Substring(idx + 1);
		}

		/// <summary>
		/// ����·������URL����ȡ�ļ���.
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static string GetFileNameByPath(string filePath)
		{
			if (String.IsNullOrEmpty(filePath))
			{
                throw new ArgumentException("�ļ�������Ϊ��", "filePath");
			}
		
			int idx = filePath.LastIndexOf('\\');
			if (idx == -1)
			{
				return "";
			}

			return filePath.Substring(idx + 1);
		}

		/// <summary>
		/// ����URL��ȡ�ļ���.
		/// </summary>
		/// <param name="fileUrl"></param>
		/// <returns></returns>
		public static string GetFileNameByUrl(string fileUrl)
		{
			if (String.IsNullOrEmpty(fileUrl))
			{
				throw new ArgumentException("�ļ�url����Ϊ��", "fileUrl");
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
        /// ��ȡ�ļ����ƣ��������ļ���׺����.
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
