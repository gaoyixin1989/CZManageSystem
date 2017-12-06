using System;
using System.Collections;

namespace Botwave.Algorithms.Apriori
{
	/// <summary>
	/// AprioriIn ��ժҪ˵����
	/// </summary>
	public abstract class AprioriIn
    {
        #region properties

        /// <summary>
        /// ������־����.
        /// </summary>
        public abstract string DataTableName
		{
			get;
			set;
		}

        /// <summary>
        /// ������־�б�ʾ���׺ŵ��ֶ�����.
        /// </summary>
        public abstract string DataTableCodingFieldName
        {
            get;
            set;
        }

        /// <summary>
        /// ������־�б�ʾ��ֶε�����.
        /// </summary>
        public abstract string DataTableItemFieldName
        {
            get;
            set;
        }

        /// <summary>
        /// �������.
        /// </summary>
        public abstract string ItemTableName
		{
			get;
			set;
		}

        /// <summary>
        /// ��ѯ��������ݻ�����.
        /// </summary>
        public abstract DataCache QueryResultDataCache
        {
            get;
            set;
        }

        /// <summary>
        /// ����¼��.
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
        /// ��ȡ���е����.
        /// </summary>
        /// <returns></returns>
        public abstract IList GetAllItems();

        /// <summary>
        /// ��������¼��.
        /// </summary>
        /// <returns></returns>
        public abstract int SetMaxRecordCount();

        /// <summary>
        /// �����еĽ��׼�¼�н�����ĳЩ��Ʒ���׵ļ�¼��.
        /// </summary>
        /// <param name="itemIDs"></param>
        /// <returns></returns>
        public abstract int GetTheItemBarginedCount(string itemIDs);

        /// <summary>
        /// �����еĽ��׼�¼�н�����ĳ����Ʒ���׵ļ�¼��.
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public abstract int GetTheItemBarginedCount(int itemID);

        /// <summary>
        /// ��ȡ֧�ֶ�.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        internal abstract double GetSupport(string from, string to);

        /// <summary>
        /// ��ȡ���Ŷ�.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        internal abstract double GetConfidence(string from, string to);

        #endregion
	}
}
