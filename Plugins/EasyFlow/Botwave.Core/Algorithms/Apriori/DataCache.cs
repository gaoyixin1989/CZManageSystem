using System;
using System.Collections;

namespace Botwave.Algorithms.Apriori
{
	/// <summary>
	/// ���ݻ����ࡣ
	/// </summary>
	public sealed class DataCache
	{
		private Hashtable ht;

        /// <summary>
        /// ���췽��.
        /// </summary>
        public DataCache() { ht = new Hashtable(); }

        /// <summary>
        /// �򻺴���б����Ӧ��ֵ.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values"></param>
		public void AddCache(string key,string values)
		{
			if(!ht.ContainsKey(key))
				ht.Add(key,values);
		}


        /// <summary>
        /// ���һ�������Ƿ����ֵ.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
		public int GetValue(string key)
		{
			int ret = -1;

			string[] arr = key.Split(",".ToCharArray());
			int flag = 0;
			IDictionaryEnumerator myEnumerator = ht.GetEnumerator();
			while(myEnumerator.MoveNext() && ret == -1)
			{
				flag = 0;
				for(int i=0;i<arr.Length;i++)
					if(StrUtils.IsMsgInString(myEnumerator.Key.ToString(),arr[i],",".ToCharArray())) flag++;

				int len = myEnumerator.Key.ToString().Split(",".ToCharArray()).Length;
				if(flag == arr.Length && arr.Length == len) ret = Convert.ToInt32(myEnumerator.Value);
			}

			return ret;
		}

        /// <summary>
        /// ��ջ�������.
        /// </summary>
        public void ClearCache()
        {
            ht.Clear();
        }
    }
}
