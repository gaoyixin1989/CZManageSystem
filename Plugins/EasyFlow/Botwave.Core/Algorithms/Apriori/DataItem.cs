using System;
using System.Collections;

namespace Botwave.Algorithms.Apriori
{
    /// <summary>
    /// ��Ʒ��.
    /// </summary>
	public sealed class GoodItemRecord
	{
		//�����Զ���������int ItemID,string ItemName

		private int _itemID;
		private string _itemName;

        /// <summary>
        /// ���.
        /// </summary>
		public int ItemID
		{
			get { return _itemID;}
			set { _itemID = value;}
		}

        /// <summary>
        /// ����.
        /// </summary>
		public string ItemName
		{
			get { return _itemName;}
			set { _itemName = value;}
		}
	}

    /// <summary>
    /// �����¼��.
    /// </summary>
    public sealed class ResultRecord
	{
        //�����Զ���������string From,string To,double Support,double Confidence,int FromCount,int FromToCount

        #region properties

        private string _from;
        private string _to;
        private double _support;
        private double _confidence;
        private int _fromCount;
        private int _fromToCount;

        /// <summary>
        /// ����������.
        /// </summary>
        public string From
        {
            get { return _from;}
            set { _from = value;}
        }

        /// <summary>
        /// ���շ�������.
        /// </summary>
        public string To
        {
            get { return _to;}
            set { _to = value;}
        }

        /// <summary>
        /// ֧�ֶ�.
        /// </summary>
        public double Support
        {
            get { return _support;}
            set { _support = value;}
        }

        /// <summary>
        /// ���Ŷ�.
        /// </summary>
        public double Confidence
        {
            get { return _confidence;}
            set { _confidence = value;}
        }

        /// <summary>
        /// ���������������.
        /// </summary>
        public int FromCount
        {
            get { return _fromCount;}
            set { _fromCount = value;}
        }

        /// <summary>
        /// ���շ������������.
        /// </summary>
        public int FromToCount
        {
            get { return _fromToCount;}
            set { _fromToCount = value;}
        }
        #endregion
    }

    /// <summary>
    /// Ƶ�ȼ�����.
    /// </summary>
    public sealed class FrequenceSet
	{		
		//�����Զ���������string SetName,string From,string To

		private string _setName;

        /// <summary>
        /// ��������.
        /// </summary>
		public string SetName
		{
			get { return _setName;}
			set { _setName = value;}
        }

        /// <summary>
        /// ���췽��.
        /// </summary>
        public FrequenceSet() { }

        /// <summary>
        /// ���췽��.
        /// </summary>
        /// <param name="setName"></param>
		public FrequenceSet(string setName)
		{
			this._setName = setName;
		}

        /// <summary>
        /// ��������.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
		public static IList SelfConntion(IList record)
		{
			IList newSet = new ArrayList();
			FrequenceSet fs,fs1,fs2;

			for(int i=0;i<record.Count;i++)
			{
				fs1 = record[i] as FrequenceSet;
				for(int j=i+1;j<record.Count;j++)
				{
					fs2 = record[j] as FrequenceSet;
					fs = new FrequenceSet();
					fs.SetName = StrUtils.DistinctMsg(fs1.SetName + "," + fs2.SetName,",".ToCharArray());
					if(CheckLength(record,fs.SetName))
						if(!IsHaveIn(newSet,fs.SetName))
							if(IsContain(record,fs.SetName))
								newSet.Add(fs);
				}
			}
			return newSet;
		}

		/// <summary>
		/// ����ӵ�ֵ�Ƿ��Ѿ�����.
		/// </summary>
		/// <param name="ds"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		private static bool IsHaveIn(IList ds,string msg)
		{
			bool bl = false;
			string setName = "";
			string[] arr;
			int flag = 0;

			for(int i=0;i<ds.Count;i++)
			{
				flag = 0;
				setName = (ds[i] as FrequenceSet).SetName;
				arr = msg.Split(",".ToCharArray());
				for(int j=0;j<arr.Length;j++)
					if(StrUtils.IsMsgInString(setName,arr[j],",".ToCharArray())) flag++;
				if(flag == arr.Length) bl = true;
			}
			return bl;
		}

		/// <summary>
		/// ����ӵ�ֵ���·ֽ���Ƿ��ܹ���ԭ������ȫ������.
		/// </summary>
		/// <param name="oldRecord"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		private static bool IsContain(IList oldRecord,string msg)
		{
			bool bl = true;
			string[] arr = msg.Split(",".ToCharArray());
			string tempStr = "";

			for(int i=0;i<arr.Length;i++)
			{
				tempStr = msg;
				StrUtils.RemoveMsgInString(ref tempStr,arr[i],",".ToCharArray());
				if(!IsHaveIn(oldRecord,tempStr)) bl = false;
			}

			return bl;
		}

		/// <summary>
		/// ��鳤��.
		/// </summary>
		/// <param name="oldRecord"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		private static bool CheckLength(IList oldRecord,string msg)
		{
			bool bl = false;
			int len = msg.Split(",".ToCharArray()).Length;
			int len2 = (oldRecord[0] as FrequenceSet).SetName.Split(",".ToCharArray()).Length + 1;
			if(len == len2) bl = true;

			return bl;
		}
	}
}
