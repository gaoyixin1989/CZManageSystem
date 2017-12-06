using System;
using System.Collections.Generic;
using System.Text;

namespace EasyFlowWeb
{
    public class FilterKeyWord
    {
        /// <summary>
        /// 验证参数是否存在关键字  
        /// </summary>
        /// <param name="KeyWord"></param>
        /// <returns></returns>
        public static int Verification(string KeyWord)
        {
            int ret = 1;
            if (string.IsNullOrEmpty(KeyWord))
            {
                return ret;
            }
            else
            {
                if (KeyWord.IndexOf("|") > 0 || KeyWord.IndexOf("&") > 0 || KeyWord.IndexOf(";") > 0 ||
                    KeyWord.IndexOf("$") > 0 || KeyWord.IndexOf("%") > 0
                    || KeyWord.IndexOf("@") > 0 || KeyWord.IndexOf("'") > 0 || KeyWord.IndexOf("<") > 0 ||
                    KeyWord.IndexOf(">") > 0 || KeyWord.IndexOf("(") > 0 || KeyWord.IndexOf("(") > 0 ||
                    KeyWord.IndexOf(")") > 0 ||
                    KeyWord.IndexOf("\'") > 0 || KeyWord.IndexOf("\"") > 0 || KeyWord.IndexOf(",") > 0)
                {
                    ret = -1;
                }
                return ret;
            }
        }

        public static string ReplaceKey(string KeyWord)
        {
            if (string.IsNullOrEmpty(KeyWord))
            {
                return "";
            }
            else
            {
                KeyWord.Replace("$", "");
                KeyWord.Replace("<", "");
                KeyWord.Replace(">", "");
                KeyWord.Replace("%", "");
                KeyWord.Replace("'", "");
                KeyWord.Replace(";", "");
                KeyWord.Replace("(", "");
                KeyWord.Replace(")", "");
                KeyWord.Replace("&", "");
                KeyWord.Replace("+", "");
                KeyWord.Replace("|", "");
                KeyWord.Replace(@"\", "");
                KeyWord.Replace("@", "");
                KeyWord.Replace(@"""", "");
                return KeyWord;
            }
        }
    }
}