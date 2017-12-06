using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Commons
{
    /// <summary>
    /// ���ϸ�����.
    /// </summary>
    public static class CollectionUtils
    {
        /// <summary>
        /// ��string��IListת��Ϊstring[].
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string[] IList2StringArray(IList<string> list)
        {
            int count = list.Count;
            string[] arr = new string[count];
            for (int i = 0; i < count; i++)
            {
                arr[i] = list[i];
            }
            return arr;
        }

        /// <summary>
        /// ��string��IListת��Ϊ�Զ��ŷָ���string.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string IList2String(IList<string> list)
        {
            return IList2String(list, ",");
        }

        /// <summary>
        /// ��string��IListת��Ϊ��splitter�ָ���string.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="splitter"></param>
        /// <returns></returns>
        public static string IList2String(IList<string> list, string splitter)
        {
            StringBuilder sb = new StringBuilder();
            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                sb.Append(list[i]);
                sb.Append(splitter);
            }
            if (count > 0)
            {
                sb.Length = sb.Length - splitter.Length;
            }
            return sb.ToString();
        }
    }
}
