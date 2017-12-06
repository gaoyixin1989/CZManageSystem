using System;
using System.Collections;
using System.IO;
using System.Web;

using Botwave.Commons;

namespace Botwave.Web
{
	/// <summary>
	/// UpDownLoadUtils ��ժҪ˵����
	/// </summary>
	public sealed class UpDownLoadUtils
	{
		/// <summary>
		/// �ϴ�����ļ�.
		/// </summary>
		/// <param name="saveDirectory"></param>
		/// <param name="controlNames"></param>
		/// <param name="isGenUniqueFileName"></param>
		/// <returns></returns>
		public static string UploadFiles(string saveDirectory,string[] controlNames,bool isGenUniqueFileName)
		{
			HttpContext ctx = HttpContext.Current;

            if (ctx.Request.Files.Count >= 1)
            {
                saveDirectory = ctx.Server.MapPath(saveDirectory);

                if (!System.IO.Directory.Exists(saveDirectory))
                {
                    System.IO.Directory.CreateDirectory(saveDirectory);
                }

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                const int MAX_TRY_COUNT = 5;	//�����ͬ���ļ�����ʱ��������Դ���

                foreach (string ctlName in controlNames)
                {
                    HttpPostedFile uploadFile = ctx.Request.Files[ctlName];
                    if (CheckPostedFile(uploadFile))
                    {
                        if (uploadFile.ContentLength == 0)
                        {
                            return "NoContent";
                        }
                        else
                        {
                            continue;
                        }
                    }

                    string fileName;

                    if (isGenUniqueFileName)
                        fileName = FileUtils.GetUniqueFileName(uploadFile.FileName);
                    else
                        fileName = Path.GetFileNameWithoutExtension(uploadFile.FileName)
                            + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(uploadFile.FileName);

                    string filePath = saveDirectory + "\\" + fileName;
                    int i = 1;
                    while (System.IO.File.Exists(filePath) && i <= MAX_TRY_COUNT)
                    {
                        fileName = FileUtils.GetUniqueFileName(uploadFile.FileName);
                        filePath = saveDirectory + "\\" + fileName;
                        i++;
                    }

                    uploadFile.SaveAs(filePath);
                    sb.Append(fileName);
                    sb.Append(";");
                }

                return sb.ToString();
            }
            else
            {
                return "";
            }
		}

        /// <summary>
        /// ����Ƿ�����ļ�.
        /// </summary>
        /// <param name="uploadFile"></param>
        /// <returns></returns>
        private static bool CheckPostedFile(HttpPostedFile uploadFile)
        {
            return null == uploadFile || uploadFile.FileName.Trim().Length > 0 || uploadFile.ContentLength == 0;
        }

        /// <summary>
        /// ɾ���ļ�.
        /// </summary>
        /// <param name="saveDirectory"></param>
        /// <param name="uploadFile"></param>
        /// <returns></returns>
		public static string UploadFile(string saveDirectory, HttpPostedFile uploadFile)
		{
			string fileName = "";

			HttpContext ctx = HttpContext.Current;
			saveDirectory=ctx.Server.MapPath(saveDirectory);

			if (!System.IO.Directory.Exists(saveDirectory))
			{
				System.IO.Directory.CreateDirectory(saveDirectory);
			}

            if (CheckPostedFile(uploadFile))
			{				
				const int MAX_TRY_COUNT = 5;	//�����ͬ���ļ�����ʱ��������Դ���

				fileName = FileUtils.GetUniqueFileName(uploadFile.FileName);							
				string filePath = saveDirectory + "\\" + fileName;
				int i = 1;	
				while (System.IO.File.Exists(filePath) && i <= MAX_TRY_COUNT)
				{
					fileName = FileUtils.GetUniqueFileName(uploadFile.FileName);
					filePath = saveDirectory + "\\" + fileName;
					i++;
				}

				uploadFile.SaveAs(filePath);
			}

			return fileName;
		}

		/// <summary>
		/// �ϴ��ļ�.
		/// </summary>
		/// <param name="saveDirectory"></param>
		/// <param name="controlName"></param>
		/// <returns></returns>
		public static string UploadFile(string saveDirectory, string controlName)
		{
			HttpContext ctx = HttpContext.Current;
			HttpPostedFile uploadFile = ctx.Request.Files[controlName];
			return UploadFile(saveDirectory, uploadFile);
		}

		/// <summary>
		/// �����ļ�.
		/// </summary>
		/// <param name="srcURL"></param>
		/// <param name="destFileName"></param>
		public static void DownLoadFile(string srcURL, string destFileName)
		{
			System.Net.WebClient wc = new System.Net.WebClient();
			wc.DownloadFile(srcURL, destFileName);
		}

		/// <summary>
		/// �����ļ�.
		/// </summary>
		/// <param name="srcFileName">Դ�ļ���</param>
		/// <param name="sDisplayFileName">�ڿͻ��˱�����ļ���</param>
		/// <param name="mustDeleteFile">�Ƿ�ɾ��Դ�ļ�</param>
		public static void ExportFile(string srcFileName, string sDisplayFileName, bool mustDeleteFile)
		{
            HttpResponse res = HttpContext.Current.Response;
			HttpContext ctx = HttpContext.Current;

			if (!File.Exists(srcFileName))
			{
                res.Write("�����ļ������ڣ��������Ա��ϵ��");
                res.End();
				return;
			}

            res.Buffer = true;
            res.Clear();

			//read the file
            //FileStream fs = new FileStream(srcFileName, FileMode.Open, FileAccess.Read);
            //BinaryReader outStream = new BinaryReader(fs);

			string sContentType = GetContentType(Path.GetExtension(sDisplayFileName));

			//send the headers to the users browser 
            res.ContentType = sContentType;
            res.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(sDisplayFileName));
            //res.AddHeader("Content-Length", fs.Length.ToString());
            res.TransmitFile(srcFileName);
            res.Charset = "GBK";

			//output the file to the browser 
            //res.BinaryWrite(outStream.ReadBytes((int)fs.Length));
            res.Flush();
            res.Clear();
			
			//tidy up 
            //outStream.Close();
            //outStream = null;

            //fs.Close();

			if (mustDeleteFile)
			{
				File.Delete(srcFileName);
			}

            res.End();
		}

		/// <summary>
		/// ���ļ��ĺ�׺����ȡ�ļ���ContentType.
		/// </summary>
		/// <param name="fileExtension"></param>
		/// <returns></returns>
		public static string GetContentType(string fileExtension)
		{
			string sContentType;

			switch (fileExtension.ToLower())
			{
				case ".asf":
					sContentType = "video/x-ms-asf";
					break;
				case ".avi":
					sContentType = "video/avi";
					break;
				case ".doc":
					sContentType = "application/msword";
					break;
				case ".zip":
					sContentType = "application/zip";
					break;
				case ".xls":
					sContentType = "application/vnd.ms-excel";
					break;
				case ".gif":
					sContentType = "image/gif";
					break;
				case ".jpg":
				case ".jpeg":
					sContentType = "image/jpeg";
					break;
				case ".wav":
					sContentType = "audio/wav";
					break;
				case ".mp3":
					sContentType = "audio/mpeg3";
					break;
				case ".mpg":
				case ".mpeg":
					sContentType = "video/mpeg";
					break;
				case ".rtf":
					sContentType = "application/rtf";
					break;
				case ".htm":
				case ".html":
					sContentType = "text/html";
					break;
				case ".txt":
					sContentType = "text/plain";
					break;
				default:
					sContentType = "application/octet-stream";
					break;
			}

			return sContentType;
		}

	}
}

