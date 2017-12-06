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
	/// AprioriSqlDataIn ��ժҪ˵����
	/// </summary>
	public class AprioriSqlDataIn : AprioriIn
	{
        //�����Զ���������string DataTableName,string DataTableCodingFieldName,string DataTableItemFieldName,string ItemTableName,int MaxRecordCount,int CountX,int CountXY,DataCache QueryResultDataCache

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
        /// ������־����.
        /// </summary>
        public override string DataTableName
        {
            get { return _dataTableName;}
            set { _dataTableName = value;}
        }

        /// <summary>
        /// ������־�б�ʾ���׺ŵ��ֶ�����.
        /// </summary>
        public override string DataTableCodingFieldName
        {
            get { return _dataTableCodingFieldName;}
            set { _dataTableCodingFieldName = value;}
        }

        /// <summary>
        /// ������־�б�ʾ��ֶε�����.
        /// </summary>
        public override string DataTableItemFieldName
        {
            get { return _dataTableItemFieldName;}
            set { _dataTableItemFieldName = value;}
        }

        /// <summary>
        /// �������.
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
        /// ��ѯ������ݻ������.
        /// </summary>
        public override DataCache QueryResultDataCache
        {
            get { return _queryResultDataCache;}
            set { _queryResultDataCache = value;}
        }
        #endregion

        /// <summary>
        /// ����Դ�˿�.
        /// </summary>
        /// <param name="dataTableName">������־����</param>
        /// <param name="dataTableCodingFieldName">������־�б�ʾ���׺ŵ��ֶ�����</param>
        /// <param name="dataTableItemFieldName">������־�б�ʾ��ֶε�����</param>
        /// <param name="itemTableName">�������</param>
        /// <param name="itemKeyFieldName">����б�ʾΨһ��ʶ���ֶ�����</param>
        /// <param name="itemTextFieldName">����б�ʾ��������Ƶ��ֶ�����</param>
		public AprioriSqlDataIn(string dataTableName,string dataTableCodingFieldName,string dataTableItemFieldName,string itemTableName,string itemKeyFieldName,string itemTextFieldName) 
		{
			_dataTableName = dataTableName;
			_itemTableName = itemTableName;
            _dataTableCodingFieldName = dataTableCodingFieldName;
            _dataTableItemFieldName = dataTableItemFieldName;
            _itemKeyFieldName = itemKeyFieldName;
            _itemTextFieldName = itemTextFieldName;

			//�½���ѯ����������ݶ���
            _queryResultDataCache = new DataCache();
            this._maxRecordCount = SetMaxRecordCount();
		}

		/// <summary>
        /// ��ȡ���е����.
		/// </summary>
        /// <returns>GoodItemRecord���󼯺� IList</returns>
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
		/// ��ȡ����Դ�е����н��׼�¼����.
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
		/// �����еĽ��׼�¼�н�����ĳЩ��Ʒ���׵ļ�¼��.
		/// </summary>
		/// <param name="itemIDs"></param>
		/// <returns></returns>
		public override int GetTheItemBarginedCount(string itemIDs)
		{
			//�����ڻ����в��Ҷ�Ӧֵ
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
				//���滺���ֵ
				//SaveCacheData(itemIDs,count);
                QueryResultDataCache.AddCache(itemIDs, count.ToString());
			}
			return count;
		}

		/// <summary>
		/// �����еĽ��׼�¼�н�����ĳ����Ʒ���׵ļ�¼��.
		/// </summary>
        /// <param name="itemID"></param>
		/// <returns></returns>
		public override int GetTheItemBarginedCount(int itemID)
		{
			//�����ڻ����в��Ҷ�Ӧֵ
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

				//���滺���ֵ
				//SaveCacheData(itemID.ToString(),count);
                QueryResultDataCache.AddCache(itemID.ToString(), count.ToString());
			}

			return count;
		}

		/// <summary>
		/// ��ȡ֧�ֶ�
		/// ���׼���ͬʱ���� XY �Ľ����������н�����֮��.
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
		/// ��ȡ���Ŷ�.
		/// ���׼���ͬʱ���� XY �Ľ���������� X �Ľ�����֮��.
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
