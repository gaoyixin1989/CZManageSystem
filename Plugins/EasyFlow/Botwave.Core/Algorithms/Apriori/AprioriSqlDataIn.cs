using System;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Text;
using System.Collections;
using Botwave.Commons;

namespace Botwave.Algorithms.Apriori
{
	/// <summary>
	/// AprioriSqlDataIn 的摘要说明。
	/// </summary>
	public class AprioriSqlDataIn : AprioriIn
	{
        //属性自动生成器：string DataTableName,string DataTableCodingFieldName,string DataTableItemFieldName,string ItemTableName,int MaxRecordCount,int CountX,int CountXY,DataCache QueryResultDataCache

        #region properties

        private string _dataTableName;
        private string _dataTableCodingFieldName;
        private string _dataTableItemFieldName;
        private string _itemTableName;
        private string _itemKeyFieldName;
        private string _itemTextFieldName;
        private int _maxRecordCount;
        private int _countXY;
        private int _countX;
        private DataCache _queryResultDataCache;

        /// <summary>
        /// 交易日志表名.
        /// </summary>
        public override string DataTableName
        {
            get { return _dataTableName;}
            set { _dataTableName = value;}
        }

        /// <summary>
        /// 交易日志中表示交易号的字段名称.
        /// </summary>
        public override string DataTableCodingFieldName
        {
            get { return _dataTableCodingFieldName;}
            set { _dataTableCodingFieldName = value;}
        }

        /// <summary>
        /// 交易日志中表示项集字段的名称.
        /// </summary>
        public override string DataTableItemFieldName
        {
            get { return _dataTableItemFieldName;}
            set { _dataTableItemFieldName = value;}
        }

        /// <summary>
        /// 项集表名称.
        /// </summary>
        public override string ItemTableName
        {
            get { return _itemTableName;}
            set { _itemTableName = value;}
        }

        internal override int MaxRecordCount
        {
            get { return _maxRecordCount;}
            set { _maxRecordCount = value;}
        }

        internal override int CountXY
        {
            get { return _countXY;}
            set { _countXY = value;}
        }

        internal override int CountX
        {
            get { return _countX; }
            set { _countX = value; }
        }

        /// <summary>
        /// 查询结果数据缓存对象.
        /// </summary>
        public override DataCache QueryResultDataCache
        {
            get { return _queryResultDataCache;}
            set { _queryResultDataCache = value;}
        }
        #endregion

        /// <summary>
        /// 数据源人口.
        /// </summary>
        /// <param name="dataTableName">交易日志表名</param>
        /// <param name="dataTableCodingFieldName">交易日志中表示交易号的字段名称</param>
        /// <param name="dataTableItemFieldName">交易日志中表示项集字段的名称</param>
        /// <param name="itemTableName">项集表名称</param>
        /// <param name="itemKeyFieldName">项集表中表示唯一标识的字段名称</param>
        /// <param name="itemTextFieldName">项集表中表示项具体名称的字段名称</param>
		public AprioriSqlDataIn(string dataTableName,string dataTableCodingFieldName,string dataTableItemFieldName,string itemTableName,string itemKeyFieldName,string itemTextFieldName) 
		{
			_dataTableName = dataTableName;
			_itemTableName = itemTableName;
            _dataTableCodingFieldName = dataTableCodingFieldName;
            _dataTableItemFieldName = dataTableItemFieldName;
            _itemKeyFieldName = itemKeyFieldName;
            _itemTextFieldName = itemTextFieldName;

			//新建查询结果缓存数据对象
            _queryResultDataCache = new DataCache();
            this._maxRecordCount = SetMaxRecordCount();
		}

		/// <summary>
        /// 获取所有的项集合.
		/// </summary>
        /// <returns>GoodItemRecord对象集合 IList</returns>
        public override IList GetAllItems()
		{
			IList ds = new ArrayList();
            GoodItemRecord record = null; 
			string sql = string.Format("Select {0},{1} From {2}",this._itemKeyFieldName,this._itemTextFieldName, this.ItemTableName);
			using(DataTable dt = SqlHelper.ExecuteDataset(CommandType.Text,sql).Tables[0])
			{
				foreach(DataRow dr in dt.Rows)
				{
					record = new GoodItemRecord();
                    record.ItemID = DbUtils.ToInt32(dr[this._itemKeyFieldName], 0);
                    record.ItemName = DbUtils.ToString(dr[this._itemTextFieldName], "");
					ds.Add(record);
				}
			}

			return ds;
		}

		/// <summary>
		/// 获取数据源中的所有交易记录总数.
		/// </summary>
		public override int SetMaxRecordCount()
		{
            string sql = string.Format("Select Count(Distinct {0}) From {1}",this.DataTableCodingFieldName,this.DataTableName);
            //this._maxRecordCount = StrUtils.ToInt32(SqlHelper.ExecuteScalar(CommandType.Text,sql),1);
            SqlConnection conn = new SqlConnection(SqlHelper.ConnectionString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.CommandTimeout = int.MaxValue;
            conn.Open();
            int count = DbUtils.ToInt32(cmd.ExecuteScalar(), 1);
            conn.Close();

            return count;
		}

		/// <summary>
		/// 在所有的交易记录中进行了某些物品交易的记录数.
		/// </summary>
		/// <param name="itemIDs"></param>
		/// <returns></returns>
		public override int GetTheItemBarginedCount(string itemIDs)
		{
			//首先在缓存中查找对应值
			//int count = IsExistsInCacheTable(itemIDs);
            int count = QueryResultDataCache.GetValue(itemIDs);
			
			if(count == -1)
			{
				int len = itemIDs.Split(",".ToCharArray()).Length;
                string sql = string.Format("Select Count(*) from (Select Distinct {0},{1} From {2})tb where {3} in ({4}) Group By {5} Having Count(*)>={6}", this.DataTableItemFieldName,this.DataTableCodingFieldName,this.DataTableName,this.DataTableItemFieldName, itemIDs,this.DataTableCodingFieldName, len);
                //count = SqlHelper.ExecuteDataset(CommandType.Text,sql).Tables[0].Rows.Count;
                SqlConnection conn = new SqlConnection(SqlHelper.ConnectionString);
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.CommandTimeout = int.MaxValue;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                conn.Open();
                da.Fill(ds);
                conn.Close();
                da.Dispose();
                cmd.Dispose();

                count = ds.Tables[0].Rows.Count;
				//保存缓存的值
				//SaveCacheData(itemIDs,count);
                QueryResultDataCache.AddCache(itemIDs, count.ToString());
			}
			return count;
		}

		/// <summary>
		/// 在所有的交易记录中进行了某个物品交易的记录数.
		/// </summary>
        /// <param name="itemID"></param>
		/// <returns></returns>
		public override int GetTheItemBarginedCount(int itemID)
		{
			//首先在缓存中查找对应值
			//int count = IsExistsInCacheTable(itemID.ToString());
            int count = QueryResultDataCache.GetValue(itemID.ToString());

			if(count == -1)
			{
				string sql = string.Format("Select Count(Distinct {0}) From {1} Where {2} = {3}",this.DataTableCodingFieldName,this.DataTableName,this.DataTableItemFieldName,itemID);
                //count = StrUtils.ToInt32(SqlHelper.ExecuteScalar(CommandType.Text,sql),0);
                SqlConnection conn = new SqlConnection(SqlHelper.ConnectionString);
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.CommandTimeout = int.MaxValue;
                conn.Open();
                count = DbUtils.ToInt32(cmd.ExecuteScalar(), 0);
                conn.Close();
                cmd.Dispose();
                conn.Dispose();

				//保存缓存的值
				//SaveCacheData(itemID.ToString(),count);
                QueryResultDataCache.AddCache(itemID.ToString(), count.ToString());
			}

			return count;
		}

		/// <summary>
		/// 获取支持度
		/// 交易集中同时包含 XY 的交易数与所有交易数之比.
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
        internal override double GetSupport(string from, string to)
		{
			if((to == "") && (from.Split(",".ToCharArray()).Length == 1))
				this.CountXY = GetTheItemBarginedCount(Convert.ToInt32(from));
			else
				this.CountXY = GetTheItemBarginedCount(from + "," + to);

			return (this.CountXY + 0.0)/this.MaxRecordCount;
		}

		/// <summary>
		/// 获取可信度.
		/// 交易集中同时包含 XY 的交易数与包含 X 的交易数之比.
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
        internal override double GetConfidence(string from, string to)
		{
			if(from.Split(",".ToCharArray()).Length > 1)
				this._countX = GetTheItemBarginedCount(from);
			else
                this._countX = GetTheItemBarginedCount(Convert.ToInt32(from));

			return (this.CountXY + 0.0)/this.CountX;
		}
	}
}
