using System;
using System.Collections;

namespace Botwave.Algorithms.Apriori
{
	/// <summary>
	/// 数据缓存类。
	/// </summary>
	public sealed class DataCache
	{
		private Hashtable ht;

        /// <summary>
        /// 构造方法.
        /// </summary>
        public DataCache() { ht = new Hashtable(); }

        /// <summary>
        /// 向缓存表中保存对应的值.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values"></param>
		public void AddCache(string key,string values)
		{
			if(!ht.ContainsKey(key))
				ht.Add(key,values);
		}


        /// <summary>
        /// 查找缓存表中是否存在值.
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
        /// 清空缓存数据.
        /// </summary>
        public void ClearCache()
        {
            ht.Clear();
        }
    }
}
