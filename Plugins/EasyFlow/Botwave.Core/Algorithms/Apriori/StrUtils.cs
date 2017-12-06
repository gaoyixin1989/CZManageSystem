using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Algorithms.Apriori
{
    /// <summary>
    /// 字符串辅助类.
    /// </summary>
    public class StrUtils
    {
        /// <summary>
        /// 以指定分隔符在指定字符串中移除指定信息字符串.
        /// </summary>
        /// <param name="oldString"></param>
        /// <param name="removeMsg"></param>
        /// <param name="splitChar"></param>
        public static void RemoveMsgInString(ref string oldString, string removeMsg, char[] splitChar)
        {
            string[] strArray = oldString.Split(splitChar);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < strArray.Length; i++)
            {
                if ((strArray[i] != removeMsg) && (strArray[i].Trim() != ""))
                {
                    builder.Append("," + strArray[i]);
                }
            }
            if (builder.Length > 0)
            {
                oldString = builder.Remove(0, 1).ToString();
            }
            else
            {
                oldString = "";
            }
        }

        /// <summary>
        /// 以指定分隔符检查指定信息字符串是否包含在指定字符串中.
        /// </summary>
        /// <param name="oldString"></param>
        /// <param name="chekMsg"></param>
        /// <param name="splitChar"></param>
        /// <returns></returns>
        public static bool IsMsgInString(string oldString, string chekMsg, char[] splitChar)
        {
            bool flag = false;
            string[] strArray = oldString.Split(splitChar);
            new StringBuilder();
            for (int i = 0; i < strArray.Length; i++)
            {
                if ((strArray[i] == chekMsg) && (strArray[i].Trim() != ""))
                {
                    flag = true;
                }
            }
            return flag;
        }

        /// <summary>
        /// 以指定分隔符去除掉字符串中重复部分.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="splitChar"></param>
        /// <returns></returns>
        public static string DistinctMsg(string msg, char[] splitChar)
        {
            StringBuilder builder = new StringBuilder();
            string[] strArray = msg.Split(splitChar);
            for (int i = 0; i < strArray.Length; i++)
            {
                if (i == 0)
                {
                    builder.Append(strArray[i]);
                }
                else if (!IsMsgInString(builder.ToString(), strArray[i], splitChar))
                {
                    builder.Append("," + strArray[i]);
                }
            }
            return builder.ToString();
        }
    }
}