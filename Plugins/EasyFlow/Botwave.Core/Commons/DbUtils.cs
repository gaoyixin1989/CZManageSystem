using System;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace Botwave.Commons
{
	/// <summary>
    /// 数据库辅助类。
	/// </summary>
	public static class DbUtils
	{
        private static Regex m_DangerSQLRegex = new Regex("['|;|(|)]", RegexOptions.Compiled);

        /// <summary>
        /// 危险 SQL 字符正则表达式.
        /// </summary>
        public static Regex DangerSQLRegex
        {
            get { return m_DangerSQLRegex; }
        }

		#region convert DB object to other type

        /// <summary>
        /// 转换指定对象为布尔值.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
		public static bool ToBoolean(object value)
		{
			return ToBoolean(value, false);
		}

        /// <summary>
        /// 转换指定对象为布尔值.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
		public static bool ToBoolean(object value, bool defaultValue)
		{
			return (null == value || value == DBNull.Value) ? defaultValue : Convert.ToBoolean(value);
		}

        /// <summary>
        /// 转换指定对象为字符串.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
		public static string ToString(object value)
		{
			return ToString(value, String.Empty);
		}

        /// <summary>
        /// 转换指定对象为字符串.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
		public static string ToString(object value, string defaultValue)
		{
			return (null == value || value == DBNull.Value) ? defaultValue : value.ToString();
        }

        /// <summary>
        /// 转换指定对象为整型.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
		public static int ToInt32(object value)
		{
			return ToInt32(value, 0);
        }

        /// <summary>
        /// 转换指定对象为整型.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
		public static int ToInt32(object value, int defaultValue)
		{
            return (null == value || value == DBNull.Value || value.ToString().Length == 0) ? defaultValue : Convert.ToInt32(value);
		}

        /// <summary>
        /// 转换指定对象为 single 类型.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
		public static float ToSingle(object value)
		{
			return ToSingle(value, 0);
		}

        /// <summary>
        /// 转换指定对象为 single 类型.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
		public static float ToSingle(object value, int defaultValue)
		{
            return (null == value || value == DBNull.Value || value.ToString().Length == 0) ? defaultValue : Convert.ToSingle(value);
		}

        /// <summary>
        /// 转换指定对象为 double 类型.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToDouble(object value)
        {
            return ToSingle(value, 0);
        }

        /// <summary>
        /// 转换指定对象为 double 类型.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double ToDouble(object value, int defaultValue)
        {
            return (null == value || value == DBNull.Value || value.ToString().Length == 0) ? defaultValue : Convert.ToDouble(value);
        }

        /// <summary>
        /// 转换为时间类型.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime? ToDateTime(object value)
        {
            if (value == null || value == DBNull.Value)
                return null;
            DateTime output;
            if (DateTime.TryParse(value.ToString(), out output))
                return output;
            return null;
        }

        /// <summary>
        /// 转化为 Guid 类型.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Guid? ToGuid(object value)
        {
            if (value == null || value == DBNull.Value)
                return null;
            try { return new Guid(value.ToString()); }
            catch { }
            return null;
        }
		#endregion
        
		/// <summary>
		/// 过滤 SQL 语句.
		/// </summary>
		/// <param name="sql"></param>
		/// <returns></returns>
		public static string FilterSQL(string sql)
		{
			if (String.IsNullOrEmpty(sql))
			{
				return String.Empty;
			}
            return m_DangerSQLRegex.Replace(sql, String.Empty);		
		}
		
        /// <summary>
        /// 复制指定数据表数据.
        /// </summary>
        /// <param name="dtSrc"></param>
        /// <param name="dtDest"></param>
        /// <param name="srcFields"></param>
        /// <param name="destFields"></param>
        /// <param name="regexs"></param>
        /// <param name="dtEx"></param>
        /// <returns></returns>
		public static bool Copy(DataTable dtSrc, DataTable dtDest, string[] srcFields, string[] destFields, string[] regexs, out DataTable dtEx)
		{
			if (null == srcFields 
				|| null == destFields
				|| null == regexs
				|| srcFields.Length != destFields.Length
				|| srcFields.Length != regexs.Length)
			{
				throw new ArgumentException("invalid srcFields/destFields/regexs");
			}

			int cols = srcFields.Length;
			bool[] arrCheck = new bool[cols];
			Regex[] arrRegex = new Regex[cols];

			for (int i=0; i<arrCheck.Length; i++)
			{
				if (null == regexs[i] || regexs[i].Length == 0)
				{
					arrCheck[i] = false;
				}
				else
				{
					arrCheck[i] = true;
					arrRegex[i] = new Regex(regexs[i], RegexOptions.Compiled);
				}
			}

			dtEx = dtSrc.Clone();
			int rows = dtSrc.Rows.Count;
			bool isValid = true;
			for (int i=0; i<rows; i++)
			{
				DataRow drSrc = dtSrc.Rows[i];
				DataRow newRow = dtDest.NewRow();
				for (int j=0; j<cols; j++)
				{
					string cellTest = drSrc[srcFields[j]].ToString();
					if (arrCheck[i])
					{
						if (!arrRegex[i].IsMatch(cellTest))
						{
							isValid = false;
							DataRow exRow = dtEx.NewRow();
							Copy(drSrc, exRow);
							dtEx.Rows.Add(exRow);
							continue;
						}
					}
					newRow[destFields[j]] = drSrc[srcFields[j]];
				}

				if (isValid)
				{
					dtDest.Rows.Add(newRow);
				}
			}

			return true;
		}

        /// <summary>
        /// 复制指定数据表数据.
        /// </summary>
        /// <param name="dtSrc"></param>
        /// <param name="dtDest"></param>
        /// <param name="srcFields"></param>
        /// <param name="destFields"></param>
		public static void Copy(DataTable dtSrc, DataTable dtDest, string[] srcFields, string[] destFields)
		{
			if (null == srcFields 
				|| null == destFields
				|| srcFields.Length != destFields.Length)
			{
				throw new ArgumentException("invalid srcFields/destFields");
			}

			int rows = dtSrc.Rows.Count;
			int cols = srcFields.Length;
			for (int i=0; i<rows; i++)
			{
				DataRow drSrc = dtSrc.Rows[i];
				DataRow newRow = dtDest.NewRow();
				for (int j=0; j<cols; j++)
				{
					newRow[destFields[j]] = drSrc[srcFields[j]];
				}
				dtDest.Rows.Add(newRow);
			}
		}

        /// <summary>
        /// 复制指定数据表数据.
        /// </summary>
        /// <param name="dtSrc"></param>
        /// <param name="dtDest"></param>
        public static void Copy(DataTable dtSrc, DataTable dtDest)
        {
            DataColumnCollection columns = dtSrc.Columns;
            for (int i = 0, rowcount = dtSrc.Rows.Count; i < rowcount; i++)
            {
                DataRow drSrc = dtSrc.Rows[i];
                DataRow newRow = dtDest.NewRow();
                for (int j = 0, colcount = columns.Count; j < colcount; j++)
                {
                    newRow[j] = drSrc[j];
                }
                dtDest.Rows.Add(newRow);
            }            
        }		

		/// <summary>
		/// Copy行数据，要求表结构相同.
		/// </summary>
		/// <param name="drSrc"></param>
		/// <param name="drDest"></param>
		private static void Copy(DataRow drSrc, DataRow drDest)
		{
			DataColumnCollection columns = drSrc.Table.Columns;
			for (int i=0, count=columns.Count; i<count; i++)
			{
				drDest[i] = drSrc[i];
			}
		}
	}
}
