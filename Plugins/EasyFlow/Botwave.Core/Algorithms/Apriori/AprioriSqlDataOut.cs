using System;
using System.Collections;
using System.Data.SqlClient;
using System.Data;
using Botwave.Commons;

namespace Botwave.Algorithms.Apriori
{
	/// <summary>
	/// AprioriSqlDataOut 的摘要说明。
	/// </summary>
	public class AprioriSqlDataOut : AprioriOut
	{
		private string _resultTableName;
        private LibConst.OperateType operType;

		private string ResultTableName
		{
			get { return _resultTableName;}
			set { _resultTableName = value;}
		}

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="resultTableName"></param>
        /// <param name="type"></param>
        public AprioriSqlDataOut(string resultTableName, LibConst.OperateType type)
		{
			_resultTableName = resultTableName;
            operType = type;
		}

        /// <summary>
        /// 保存结果记录.
        /// </summary>
        /// <param name="record"></param>
		public override void SaveResult(IList record)
		{
            if (operType == LibConst.OperateType.Insert) TruncateTableData();

			string sql = "";
			ResultRecord result = null;
			for(int i=0;i<record.Count;i++)
			{
				result = record[i] as ResultRecord;
				if(result != null)
				{
					sql = "If Exists(Select 1 From {0} Where [From]='{1}' and [To]='{2}') Update {3} Set Support={4},Confidence={5} Where [From]='{6}' and [To]='{7}'";
                    sql += " Else Insert Into {8}([From],[To],Support,Confidence,FromCount,FromToCount) Values('{9}','{10}',{11},{12},{13},{14})";
					sql = string.Format(sql,this.ResultTableName,result.From,result.To,this.ResultTableName,result.Support,result.Confidence,result.From,result.To,this.ResultTableName,result.From,result.To,result.Support,result.Confidence,result.FromCount,result.FromToCount);
					SqlHelper.ExecuteNonQuery(CommandType.Text,sql);
				}
			}
		}

        private void TruncateTableData()
        {
            string sql = string.Format("Truncate Table {0}",this.ResultTableName);
            SqlHelper.ExecuteNonQuery(CommandType.Text, sql);
        }
	}
}
