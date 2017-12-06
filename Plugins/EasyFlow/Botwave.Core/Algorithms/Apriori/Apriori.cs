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
	/// Apriori 的摘要说明。
	/// </summary>
	public sealed class Apriori : IApriori
	{
        //属性自动生成器：double MinSupport,double MinConfidence,int MinLength,int MaxLength,AprioriIn In,AprioriOut Out

        #region properties

        private double _minSupport;
		private double _minConfidence;
		private int _minLength;
		private int _maxLength;
		private AprioriIn _in;
		private AprioriOut _out;

		private double MinSupport
		{
			get { return _minSupport;}
			set { _minSupport = value;}
		}

		private double MinConfidence
		{
			get { return _minConfidence;}
			set { _minConfidence = value;}
		}

		private int MinLength
		{
			get { return _minLength;}
			set { _minLength = value;}
		}

		private int MaxLength
		{
			get { return _maxLength;}
			set { _maxLength = value;}
		}

		private AprioriIn In
		{
			get { return _in;}
			set { _in = value;}
		}

		private AprioriOut Out
		{
			get { return _out;}
			set { _out = value;}
        }
        #endregion

        /// <summary>
        /// AprioriSet 算法对象.
        /// </summary>
        /// <param name="inObj"></param>
        /// <param name="outObj"></param>
        /// <param name="minSupport">支持度</param>
        /// <param name="minConfidence">可信度</param>
		public Apriori(AprioriIn inObj,AprioriOut outObj,double minSupport,double minConfidence)
            :this(inObj,outObj,minSupport,minConfidence,1,1000){}

        /// <summary>
        /// AprioriSet算法对象
        /// </summary>
        /// <param name="inObj"></param>
        /// <param name="outObj"></param>
        /// <param name="minSupport">支持度</param>
        /// <param name="minConfidence">可信度</param>
        /// <param name="maxLength">最大的项与项关联的个数</param>
        public Apriori(AprioriIn inObj, AprioriOut outObj, double minSupport, double minConfidence, int maxLength)
            : this(inObj, outObj, minSupport, minConfidence, 1, maxLength){}

        /// <summary>
        /// AprioriSet 算法对象.
        /// </summary>
        /// <param name="inObj"></param>
        /// <param name="outObj"></param>
        /// <param name="minSupport">支持度</param>
        /// <param name="minConfidence">可信度</param>
        /// <param name="minLength">最小的项与项关联的个数</param>
        /// <param name="maxLength">最大的项与项关联的个数</param>
		public Apriori(AprioriIn inObj,AprioriOut outObj,double minSupport,double minConfidence,int minLength,int maxLength)
		{
			_in = inObj;
			_out = outObj;
            _minSupport = minSupport;
            //如果支持度为整数，则将它转换为小数表示
			if(DbUtils.ToInt32(minSupport,0) > 1) _minSupport = minSupport/inObj.MaxRecordCount;
			_minConfidence = minConfidence;
			_minLength = minLength;
			_maxLength = maxLength;
		}

        /// <summary>
        /// 开始分析计算.
        /// </summary>
		public void Create()
		{
			GenerateFrequence();

            //将缓存中的数据清空
            In.QueryResultDataCache.ClearCache();
		}
		
        /// <summary>
        /// 生成最大频繁集.
        /// </summary>
        private void GenerateFrequence() 
		{
			//获取所有的项集
			IList dsItem = In.GetAllItems();

			//满足最小支持度和信任度的结果集
			IList dsResult = new ArrayList();

			//剪枝后的项集
			IList pruneSet = null;

			//生成L1
			GenerateFrequenceL1(dsItem,ref dsResult,ref pruneSet);

			//循环递规调用，生成满足最小计数的最大频繁集
			if(pruneSet != null) RecursiveDump(2,ref dsResult,pruneSet);

			Out.SaveResult(dsResult);
		}

		/// <summary>
		/// 生成频繁集L1，同时进行剪枝并返回新项集.
		/// </summary>
		/// <param name="allItemDS"></param>
		/// <param name="resultDS"></param>
        /// <param name="newItemSet"></param>
		private void GenerateFrequenceL1(IList allItemDS,ref IList resultDS,ref IList newItemSet)
		{
			GoodItemRecord goodItem;
			ResultRecord result;

			for(int i=0;i<allItemDS.Count;i++)
			{
				goodItem = allItemDS[i] as GoodItemRecord;
				result = new ResultRecord();
				result.From = goodItem.ItemID.ToString();
				result.To = "";
				result.Support = In.GetSupport(result.From,result.To);
				result.Confidence = 1.0;
                result.FromCount = In.CountXY;
                result.FromToCount = In.CountXY;
				if(result.Support>=this.MinSupport && result.Confidence>=this.MinConfidence) 
				{
					resultDS.Add(result);
					if(newItemSet == null) newItemSet = new ArrayList();
					newItemSet.Add(new FrequenceSet(result.From));

                    //保存一次结果
                    Out.SaveResult(resultDS);
				}
			}
		}

        /// <summary>
        /// 循环递规调用，生成满足最小计数的最大频繁集.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="dsResult"></param>
        /// <param name="pruneSet"></param>
		private void RecursiveDump(int level,ref IList dsResult,IList pruneSet)
		{
			FrequenceSet fs;
			ResultRecord result;
			string frequenceStr,from,to;
			string[] arr;
			bool flag;

			//自连接后项集
			IList newPruneSet = FrequenceSet.SelfConntion(pruneSet);
			//新剪枝后项集
			pruneSet = new ArrayList();

			//由L1生成Ln
			for(int i=0;i<newPruneSet.Count;i++)
			{
				fs = newPruneSet[i] as FrequenceSet;
				frequenceStr = fs.SetName;
				arr = frequenceStr.Split(",".ToCharArray());
				flag = false;
				//对项集值（158）进行分解（1->58，5->18，8->15）并计算支持度和可信度
				for(int j=0;j<arr.Length;j++)
				{
					from = arr[j];
					to = frequenceStr;
					StrUtils.RemoveMsgInString(ref to,from,",".ToCharArray());

					result = new ResultRecord();
					result.From = from;
					result.To = to;
					result.Support = In.GetSupport(result.From,result.To);
					result.Confidence = In.GetConfidence(result.From,result.To);
                    result.FromCount = In.CountX;
                    result.FromToCount = In.CountXY;
                    if (result.Support >= this.MinSupport && result.Confidence >= this.MinConfidence) 
					{
						dsResult.Add(result);
						flag = true;

                        //保存一次结果
                        Out.SaveResult(dsResult);
					}
				}
				if(flag) pruneSet.Add(new FrequenceSet(fs.SetName));
			}

			//设置跳出循环的条件
			if(newPruneSet.Count > 1&& level >= this.MinLength && level < this.MaxLength) RecursiveDump(level+1,ref dsResult,pruneSet);
		}
	}
}
