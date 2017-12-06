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
	/// Apriori ��ժҪ˵����
	/// </summary>
	public sealed class Apriori : IApriori
	{
        //�����Զ���������double MinSupport,double MinConfidence,int MinLength,int MaxLength,AprioriIn In,AprioriOut Out

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
        /// AprioriSet �㷨����.
        /// </summary>
        /// <param name="inObj"></param>
        /// <param name="outObj"></param>
        /// <param name="minSupport">֧�ֶ�</param>
        /// <param name="minConfidence">���Ŷ�</param>
		public Apriori(AprioriIn inObj,AprioriOut outObj,double minSupport,double minConfidence)
            :this(inObj,outObj,minSupport,minConfidence,1,1000){}

        /// <summary>
        /// AprioriSet�㷨����
        /// </summary>
        /// <param name="inObj"></param>
        /// <param name="outObj"></param>
        /// <param name="minSupport">֧�ֶ�</param>
        /// <param name="minConfidence">���Ŷ�</param>
        /// <param name="maxLength">��������������ĸ���</param>
        public Apriori(AprioriIn inObj, AprioriOut outObj, double minSupport, double minConfidence, int maxLength)
            : this(inObj, outObj, minSupport, minConfidence, 1, maxLength){}

        /// <summary>
        /// AprioriSet �㷨����.
        /// </summary>
        /// <param name="inObj"></param>
        /// <param name="outObj"></param>
        /// <param name="minSupport">֧�ֶ�</param>
        /// <param name="minConfidence">���Ŷ�</param>
        /// <param name="minLength">��С������������ĸ���</param>
        /// <param name="maxLength">��������������ĸ���</param>
		public Apriori(AprioriIn inObj,AprioriOut outObj,double minSupport,double minConfidence,int minLength,int maxLength)
		{
			_in = inObj;
			_out = outObj;
            _minSupport = minSupport;
            //���֧�ֶ�Ϊ����������ת��ΪС����ʾ
			if(DbUtils.ToInt32(minSupport,0) > 1) _minSupport = minSupport/inObj.MaxRecordCount;
			_minConfidence = minConfidence;
			_minLength = minLength;
			_maxLength = maxLength;
		}

        /// <summary>
        /// ��ʼ��������.
        /// </summary>
		public void Create()
		{
			GenerateFrequence();

            //�������е��������
            In.QueryResultDataCache.ClearCache();
		}
		
        /// <summary>
        /// �������Ƶ����.
        /// </summary>
        private void GenerateFrequence() 
		{
			//��ȡ���е��
			IList dsItem = In.GetAllItems();

			//������С֧�ֶȺ����ζȵĽ����
			IList dsResult = new ArrayList();

			//��֦����
			IList pruneSet = null;

			//����L1
			GenerateFrequenceL1(dsItem,ref dsResult,ref pruneSet);

			//ѭ���ݹ���ã�����������С���������Ƶ����
			if(pruneSet != null) RecursiveDump(2,ref dsResult,pruneSet);

			Out.SaveResult(dsResult);
		}

		/// <summary>
		/// ����Ƶ����L1��ͬʱ���м�֦���������.
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

                    //����һ�ν��
                    Out.SaveResult(resultDS);
				}
			}
		}

        /// <summary>
        /// ѭ���ݹ���ã�����������С���������Ƶ����.
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

			//�����Ӻ��
			IList newPruneSet = FrequenceSet.SelfConntion(pruneSet);
			//�¼�֦���
			pruneSet = new ArrayList();

			//��L1����Ln
			for(int i=0;i<newPruneSet.Count;i++)
			{
				fs = newPruneSet[i] as FrequenceSet;
				frequenceStr = fs.SetName;
				arr = frequenceStr.Split(",".ToCharArray());
				flag = false;
				//���ֵ��158�����зֽ⣨1->58��5->18��8->15��������֧�ֶȺͿ��Ŷ�
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

                        //����һ�ν��
                        Out.SaveResult(dsResult);
					}
				}
				if(flag) pruneSet.Add(new FrequenceSet(fs.SetName));
			}

			//��������ѭ��������
			if(newPruneSet.Count > 1&& level >= this.MinLength && level < this.MaxLength) RecursiveDump(level+1,ref dsResult,pruneSet);
		}
	}
}
