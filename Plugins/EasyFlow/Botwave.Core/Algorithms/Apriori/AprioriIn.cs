using System;
using System.Collections;

namespace Botwave.Algorithms.Apriori
{
	/// <summary>
	/// AprioriIn 的摘要说明。
	/// </summary>
	public abstract class AprioriIn
    {
        #region properties

        /// <summary>
        /// 交易日志表名.
        /// </summary>
        public abstract string DataTableName
		{
			get;
			set;
		}

        /// <summary>
        /// 交易日志中表示交易号的字段名称.
        /// </summary>
        public abstract string DataTableCodingFieldName
        {
            get;
            set;
        }

        /// <summary>
        /// 交易日志中表示项集字段的名称.
        /// </summary>
        public abstract string DataTableItemFieldName
        {
            get;
            set;
        }

        /// <summary>
        /// 项集表名称.
        /// </summary>
        public abstract string ItemTableName
		{
			get;
			set;
		}

        /// <summary>
        /// 查询结果的数据缓存器.
        /// </summary>
        public abstract DataCache QueryResultDataCache
        {
            get;
            set;
        }

        /// <summary>
        /// 最大记录数.
        /// </summary>
        internal abstract int MaxRecordCount
		{
			get;
			set;
		}

        internal abstract int CountX
        {
            get;
            set;
        }

        internal abstract int CountXY
        {
            get;
            set;
        }
        #endregion

        #region methods

        /// <summary>
        /// 获取所有的项集合.
        /// </summary>
        /// <returns></returns>
        public abstract IList GetAllItems();

        /// <summary>
        /// 设置最大记录数.
        /// </summary>
        /// <returns></returns>
        public abstract int SetMaxRecordCount();

        /// <summary>
        /// 在所有的交易记录中进行了某些物品交易的记录数.
        /// </summary>
        /// <param name="itemIDs"></param>
        /// <returns></returns>
        public abstract int GetTheItemBarginedCount(string itemIDs);

        /// <summary>
        /// 在所有的交易记录中进行了某个物品交易的记录数.
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public abstract int GetTheItemBarginedCount(int itemID);

        /// <summary>
        /// 获取支持度.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        internal abstract double GetSupport(string from, string to);

        /// <summary>
        /// 获取可信度.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        internal abstract double GetConfidence(string from, string to);

        #endregion
	}
}
