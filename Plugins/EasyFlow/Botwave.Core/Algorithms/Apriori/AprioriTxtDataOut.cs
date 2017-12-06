using System;
using System.IO;
using System.Text;
using System.Collections;
using Botwave.Commons;


namespace Botwave.Algorithms.Apriori
{
	/// <summary>
	/// AprioriTxtDataOut 的摘要说明。
	/// </summary>
	public class AprioriTxtDataOut : AprioriOut
	{
		private string _fileUrl;
		private FileStream fs;

        /// <summary>
        /// 文件 URL.
        /// </summary>
		private string FileUrl
		{
			get { return _fileUrl;}
			set { _fileUrl = value;}
		}

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="fileUrl"></param>
		public AprioriTxtDataOut(string fileUrl)
		{
			this._fileUrl = fileUrl;
		}

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="fileStream"></param>
		public AprioriTxtDataOut(FileStream fileStream)
		{
			fs = fileStream;
		}

        /// <summary>
        /// 保存结果记录.
        /// </summary>
        /// <param name="record"></param>
		public override void SaveResult(IList record)
		{
            if (DbUtils.ToString(this.FileUrl, "") == "")
				Save(fs,record);
			else
			{
				FileStream stream = new FileStream(this.FileUrl,FileMode.Create,FileAccess.Write,FileShare.Read);
				Save(stream,record);
				stream.Close();
			}
		}

        /// <summary>
        /// 保存结果.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="record"></param>
		private void Save(FileStream stream,IList record)
		{
			StringBuilder sb = new StringBuilder();
			StreamWriter sw = new StreamWriter(stream,Encoding.UTF8);

			ResultRecord result;
			for(int i=0;i<record.Count;i++)
			{
				sb.Remove(0,sb.Length);
				result = record[i] as ResultRecord;
                sb.Append(string.Format("规则{0}：从{1}到{2}，支持度：{3}，可信度：{4}，[{5}]总数：{6}，[{7}]总数：{8}", i, result.From, ((result.To == "") ? result.From : result.To), result.Support, result.Confidence, result.From, result.FromCount, ((result.To == "") ? result.From : result.To), result.FromToCount));
				sw.WriteLine(sb.ToString());
			}
			sw.Close();
		}
	}
}
