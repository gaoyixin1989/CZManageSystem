using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;

namespace Botwave.Commons
{
	/// <summary>
	/// 字符串辅助类.
	/// </summary>
	public static class StringUtils
	{
		/// <summary>
		/// 计算中文字符串长度.
		/// </summary>
		/// <param name="cnstr"></param>
		/// <returns></returns>
		public static int CnLength(string cnstr)
		{
			byte[] bs = Encoding.ASCII.GetBytes(cnstr);
		
			int len = 0;  									//len为字符串之实际长度
			for (int i=0; i<=bs.Length-1; i++)
			{
				if (bs[i] == 63)							//判断是否为汉字或全脚符号
				{
					len++;
				}
				len++;
			}

			return len;
		}

        /// <summary>
        /// 累加字符串.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="times"></param>
        /// <returns></returns>
		public static string Repeat(string str, int times)
		{
			StringBuilder sb = new StringBuilder(str.Length * times);
			for (int i=0; i<times; i++)
			{
				sb.Append(str);
			}
			return sb.ToString();
        } 

        /// <summary>
        /// 验证input是否在以separator分隔的序列sequence中.
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="separator"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsInSequence(string sequence, char separator, string input)
		{
			string[] ss = sequence.Split(separator);
			bool isInSequence = false;
			for (int i=0, count=ss.Length; i<count; i++)
			{
				if (input == ss[i])
				{
					isInSequence = true;
					break;
				}
			}
			return isInSequence;
		}

		/// <summary>
		/// 拆分字符串.
		/// </summary>
		/// <param name="s"></param>
		/// <param name="separator"></param>
		/// <returns></returns>
		public static string[] Split(string s,string separator)
		{
			return System.Text.RegularExpressions.Regex.Split(s, separator);
		}

        /// <summary>
        /// 截断并补齐字符串.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="maxlength"></param>
        /// <param name="placeholder"></param>
        /// <returns></returns>
        public static string Truncate(string s, int maxlength, string placeholder)
        {
            if (String.IsNullOrEmpty(s) || s.Length <= maxlength)
            {
                return s;
            }

            return s.Substring(0, maxlength) + placeholder;
        }

        /// <summary>
        /// 截断并补齐字符串(中文)
        /// </summary>
        /// <param name="s"></param>
        /// <param name="maxLength"></param>
        /// <param name="placeholder"></param>
        /// <returns></returns>
        public static string TruncateCnStr(string s, int maxLength, string placeholder)
        {
            byte[] bs = Encoding.ASCII.GetBytes(s);

            int cnLength = 0;
            int extraCount = 0;//额外的/多余的字符数.

            for (int i = 0; i <= bs.Length - 1; i++)
            {
                if (bs[i] == 63)//判断是否为汉字或全脚符号.
                {
                    cnLength++;
                }
                cnLength++;

                if (cnLength > maxLength)
                {
                    extraCount++;
                }
            }

            if (cnLength >= maxLength)
            {
                return s.Substring(0, s.Length - extraCount) + placeholder;
            }
            return s;
        }

        #region Join

        /// <summary>
        /// 合并字符串列表为一个字符串.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string Join(IList<string> list)
        {
            return Join(list, String.Empty, String.Empty, String.Empty);
        }

        /// <summary>
        /// 合并字符串.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="perfix"></param>
        /// <param name="postfix"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string Join(IList<string> list, string perfix, string postfix, string separator)
        {
            StringBuilder sb = new StringBuilder();
            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                sb.Append(perfix);
                sb.Append(list[i]);
                sb.Append(postfix);
                sb.Append(separator);
            }
            if (count > 0)
            {
                sb.Length -= separator.Length;
            }
            return sb.ToString();
        }

        /// <summary>
        /// 合并字符串.
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="perfix"></param>
        /// <param name="postfix"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string Join(string[] arr, string perfix, string postfix, string separator)
        {
            if (null == arr) throw new ArgumentNullException("arr", "array should not be null");

            int count = arr.Length;
            if (count > 0)
            {
                perfix = (null == perfix) ? "" : perfix;
                postfix = (null == postfix) ? "" : postfix;
                separator = (null == separator) ? "" : separator;
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < count; i++)
                {
                    sb.Append(perfix);
                    sb.Append(arr[i]);
                    sb.Append(postfix);
                    sb.Append(separator);
                }
                sb.Length -= separator.Length;
                return sb.ToString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 合并字符串.
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string Join(string[] arr, string separator)
        {
            if (null == arr || arr.Length == 0) return String.Empty;

            if (arr.Length >= 10)
            {
                return JoinByStringBuilder(arr, separator);
            }
            else
            {
                return JoinByString(arr, separator);
            }
        }

        /// <summary>
        /// 合并字符串.
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string JoinByStringBuilder(string[] arr, string separator)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < arr.Length; i++)
            {
                sb.Append(arr[i]);
                sb.Append(separator);
            }
            sb.Length = sb.Length - separator.Length;
            return sb.ToString();
        }

        /// <summary>
        /// 合并字符串.
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string JoinByString(string[] arr, string separator)
        {
            string result = "";
            int len = arr.Length;
            for (int i = 0, icount = len - 2; i <= icount; i++)
            {
                result += arr[i] + separator;
            }
            result += arr[len - 1];
            return result;
        }

        #endregion

        #region Align

        /// <summary>
        /// 向左对齐.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="length"></param>
        /// <param name="placeholder"></param>
        /// <returns></returns>
        public static string AlignLeft(string s, int length, char placeholder)
        {
            return Align(s, length, placeholder, true);
        }

        /// <summary>
        /// 向右对齐.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="length"></param>
        /// <param name="placeholder"></param>
        /// <returns></returns>
        public static string AlignRight(string s, int length, char placeholder)
        {
            return Align(s, length, placeholder, false);
        }

        private static string Align(string s, int length, char placeholder, bool isAlignLeft)
        {
            if (null == s || length <= s.Length) return s;

            int lengthOfPlaceholders = length - s.Length;
            char[] cc = new char[lengthOfPlaceholders];
            for (int i = 0; i < lengthOfPlaceholders; i++)
            {
                cc[i] = placeholder;
            }

            string retVal = isAlignLeft ? (new string(cc) + s) : (s + new string(cc));
            return retVal;
        }

        #endregion

        /// <summary>
        /// 获得最后一个匹配项后的字符串.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string GetSplitAfterStr(string s, string separator)
        {
            if (String.IsNullOrEmpty(s))
            {
                return s;
            }
            return s.Substring(s.LastIndexOf(separator)+1);
        }

        /// <summary>
        /// 替换html中的特殊字符.
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string HtmlEncode(object inputString)
        {
            string retVal = inputString.ToString();
            retVal = retVal.Replace("&", "&amp;");
            retVal = retVal.Replace("\"", "&quot;");
            retVal = retVal.Replace("'", "&quot;");
            retVal = retVal.Replace("<", "&lt;");
            retVal = retVal.Replace(">", "&gt;");
            retVal = retVal.Replace(" ", "&nbsp;");
            retVal = retVal.Replace("  ", "&nbsp;&nbsp;");
            retVal = retVal.Replace("\t", "&nbsp;&nbsp;");
            retVal = retVal.Replace("\r\n", "<br/>");
            return retVal;
        }

        /// <summary>
        /// 恢复html中的特殊字符 .
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string HtmlDecode(object inputString)
        {
            string retVal = inputString.ToString();
            retVal = retVal.Replace("&amp;", "&");
            retVal = retVal.Replace("&quot;", "\"");
            retVal = retVal.Replace("&quot;", "'");
            retVal = retVal.Replace("&lt;", "<");
            retVal = retVal.Replace("&gt;", ">");
            retVal = retVal.Replace("&nbsp;", " ");
            retVal = retVal.Replace("&nbsp;&nbsp;", "  ");
            retVal = retVal.Replace("&nbsp;&nbsp;", "\t");
            retVal = retVal.Replace("<br/>", "\r\n");
            return retVal;
        }
    }
}
