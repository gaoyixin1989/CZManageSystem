using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;

namespace Botwave.Commons
{
	/// <summary>
	/// �ַ���������.
	/// </summary>
	public static class StringUtils
	{
		/// <summary>
		/// ���������ַ�������.
		/// </summary>
		/// <param name="cnstr"></param>
		/// <returns></returns>
		public static int CnLength(string cnstr)
		{
			byte[] bs = Encoding.ASCII.GetBytes(cnstr);
		
			int len = 0;  									//lenΪ�ַ���֮ʵ�ʳ���
			for (int i=0; i<=bs.Length-1; i++)
			{
				if (bs[i] == 63)							//�ж��Ƿ�Ϊ���ֻ�ȫ�ŷ���
				{
					len++;
				}
				len++;
			}

			return len;
		}

        /// <summary>
        /// �ۼ��ַ���.
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
        /// ��֤input�Ƿ�����separator�ָ�������sequence��.
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
		/// ����ַ���.
		/// </summary>
		/// <param name="s"></param>
		/// <param name="separator"></param>
		/// <returns></returns>
		public static string[] Split(string s,string separator)
		{
			return System.Text.RegularExpressions.Regex.Split(s, separator);
		}

        /// <summary>
        /// �ضϲ������ַ���.
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
        /// �ضϲ������ַ���(����)
        /// </summary>
        /// <param name="s"></param>
        /// <param name="maxLength"></param>
        /// <param name="placeholder"></param>
        /// <returns></returns>
        public static string TruncateCnStr(string s, int maxLength, string placeholder)
        {
            byte[] bs = Encoding.ASCII.GetBytes(s);

            int cnLength = 0;
            int extraCount = 0;//�����/������ַ���.

            for (int i = 0; i <= bs.Length - 1; i++)
            {
                if (bs[i] == 63)//�ж��Ƿ�Ϊ���ֻ�ȫ�ŷ���.
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
        /// �ϲ��ַ����б�Ϊһ���ַ���.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string Join(IList<string> list)
        {
            return Join(list, String.Empty, String.Empty, String.Empty);
        }

        /// <summary>
        /// �ϲ��ַ���.
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
        /// �ϲ��ַ���.
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
        /// �ϲ��ַ���.
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
        /// �ϲ��ַ���.
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
        /// �ϲ��ַ���.
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
        /// �������.
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
        /// ���Ҷ���.
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
        /// ������һ��ƥ�������ַ���.
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
        /// �滻html�е������ַ�.
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
        /// �ָ�html�е������ַ� .
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
