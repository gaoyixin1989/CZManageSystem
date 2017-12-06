using System;
using System.Collections;

namespace Botwave.Algorithms.Apriori
{
    /// <summary>
    /// 商品项.
    /// </summary>
	public sealed class GoodItemRecord
	{
		//属性自动生成器：int ItemID,string ItemName

		private int _itemID;
		private string _itemName;

        /// <summary>
        /// 编号.
        /// </summary>
		public int ItemID
		{
			get { return _itemID;}
			set { _itemID = value;}
		}

        /// <summary>
        /// 名称.
        /// </summary>
		public string ItemName
		{
			get { return _itemName;}
			set { _itemName = value;}
		}
	}

    /// <summary>
    /// 结果记录类.
    /// </summary>
    public sealed class ResultRecord
	{
        //属性自动生成器：string From,string To,double Support,double Confidence,int FromCount,int FromToCount

        #region properties

        private string _from;
        private string _to;
        private double _support;
        private double _confidence;
        private int _fromCount;
        private int _fromToCount;

        /// <summary>
        /// 主分析对象.
        /// </summary>
        public string From
        {
            get { return _from;}
            set { _from = value;}
        }

        /// <summary>
        /// 参照分析对象.
        /// </summary>
        public string To
        {
            get { return _to;}
            set { _to = value;}
        }

        /// <summary>
        /// 支持度.
        /// </summary>
        public double Support
        {
            get { return _support;}
            set { _support = value;}
        }

        /// <summary>
        /// 可信度.
        /// </summary>
        public double Confidence
        {
            get { return _confidence;}
            set { _confidence = value;}
        }

        /// <summary>
        /// 主分析对象的数量.
        /// </summary>
        public int FromCount
        {
            get { return _fromCount;}
            set { _fromCount = value;}
        }

        /// <summary>
        /// 参照分析对象的数量.
        /// </summary>
        public int FromToCount
        {
            get { return _fromToCount;}
            set { _fromToCount = value;}
        }
        #endregion
    }

    /// <summary>
    /// 频度集合类.
    /// </summary>
    public sealed class FrequenceSet
	{		
		//属性自动生成器：string SetName,string From,string To

		private string _setName;

        /// <summary>
        /// 集合名称.
        /// </summary>
		public string SetName
		{
			get { return _setName;}
			set { _setName = value;}
        }

        /// <summary>
        /// 构造方法.
        /// </summary>
        public FrequenceSet() { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="setName"></param>
		public FrequenceSet(string setName)
		{
			this._setName = setName;
		}

        /// <summary>
        /// 自身连接.
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
		/// 待添加的值是否已经存在.
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
		/// 待添加的值重新分解后，是否能够在原集合中全部存在.
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
		/// 检查长度.
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
