using System;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace Botwave.Commons
{
	/// <summary>
	/// 数据表验证器类。
	/// </summary>
	public static class DataTableValidator
	{
        /// <summary>
        /// 数据验证类型枚举.
        /// </summary>
		public enum ValidateType
		{
            /// <summary>
            /// 任何值.
            /// </summary>
			Any = 0,	
            /// <summary>
            /// 正则式.
            /// </summary>
			Regex = 1,
            /// <summary>
            /// 文本长度.
            /// </summary>
			Length = 2,
            /// <summary>
            /// 序列.
            /// </summary>
			Sequence = 3,
            /// <summary>
            /// 整数.
            /// </summary>
			Int = 4,	
            /// <summary>
            /// 小数.
            /// </summary>
			Number = 5,
            /// <summary>
            /// 日期.
            /// </summary>
			Date = 6
		}

        /// <summary>
        /// 获取指定名称的验证器类型.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
		public static ValidateType GetValidateType(string input)
		{
			input = (null == input) ? "" : input.ToLower();
			ValidateType validType = ValidateType.Any;
			switch (input)
			{
				case "regex":
					validType = ValidateType.Regex;
					break;
				case "length":
					validType = ValidateType.Length;
					break;
				case "sequence":
					validType = ValidateType.Sequence;
					break;
				case "int":
					validType = ValidateType.Int;
					break;
				case "number":
					validType = ValidateType.Number;
					break;
				case "date":
					validType = ValidateType.Date;
					break;
				default:
					break;
			}
			return validType;
		}

		/// <summary>
		/// 检查输入的表数据.
		/// </summary>
		/// <param name="dtInput"></param>
		/// <param name="rules">e.g. rules={{"列名","允许空值","验证类型","表达式","最小值","最大值","错误信息"},{"colName","false","regex","[a|b]","","","只能输入a或b"},{"colName","false","length","","1","6","输入字符串长度必须小于等于6且大于等于1"}}</param>
		/// <param name="errorMsg"></param>
		/// <returns></returns>
		public static bool CheckData(DataTable dtInput, string[,] rules, out string errorMsg)
		{
			StringBuilder sb = new StringBuilder();
			
			bool containErrorItems = false;
			for (int i=0, dataCount=dtInput.Rows.Count; i<dataCount; i++)
			{
				DataRow dr = dtInput.Rows[i];
				for (int j=0, ruleCount = rules.GetLength(0); j<ruleCount; j++)
				{
					string colName = rules[j,0];
					bool allowEmpty = (0 == String.Compare(rules[j,1], "true", true));
					ValidateType validateType = GetValidateType(rules[j,2]);
					string expression = rules[j,3];
					string minVal = rules[j,4];
					string maxVal = rules[j,5];
					string errorInfo = rules[j,6];

					bool isValid = true;
					string input = (null == dr[colName] || dr[colName] == DBNull.Value) ? "" : dr[colName].ToString().Trim();
					if (allowEmpty)	//允许为空值
					{
						if (input.Length != 0)	//输入不为空则按指定格式检查，否则检查下一个
						{
							isValid = CheckInput(input, validateType, expression, minVal, maxVal);
						}
					}
					else	//不允许为空值
					{
						if (input.Length == 0)//输入为空
						{
                            isValid = false;
						}
						else		//输入不为空则按指定格式检查
						{
							isValid = CheckInput(input, validateType, expression, minVal, maxVal);
						}
					}

					if (!isValid)
					{                        
						containErrorItems = true;
                        sb.Append("第[");
                        sb.Append(i + 1);
                        sb.Append("]行，[");
                        sb.Append(colName);
                        sb.Append("]格式错误，错误信息：[");
						sb.Append(errorInfo);
                        sb.Append("]，现输入值：[");
						sb.Append(input);
                        sb.Append("]；");
                        sb.Append("<br />");
						//sb.Append(Environment.NewLine);
					}
				}
			}

			errorMsg = sb.ToString();
			return !containErrorItems;
		}

        /// <summary>
        /// 检查输入的数据.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="validateType"></param>
        /// <param name="expression"></param>
        /// <param name="minVal"></param>
        /// <param name="maxVal"></param>
        /// <returns></returns>
		public static bool CheckInput(string input, ValidateType validateType, string expression, string minVal, string maxVal)
		{
			bool isValid = true;
			int iMinVal = 0;
			int iMaxVal = 0;

			switch (validateType)
			{
				case ValidateType.Regex:	//正则式验证
					if (!Regex.IsMatch(input, expression))
					{
						isValid = false;
					}
					break;
				case ValidateType.Length:	//文本长度验证
					iMinVal = (minVal.Length == 0 ? 0 : int.Parse(minVal));
					iMaxVal = (maxVal.Length == 0 ? 0 : int.Parse(maxVal));

					if ((iMinVal != 0 && input.Length < iMinVal)
						|| (iMaxVal != 0 && input.Length > iMaxVal))
					{
						isValid = false;
					}
					break;
				case ValidateType.Sequence:	//序列验证
					if (expression.Length != 0)	//指定了序列
					{
						isValid = StringUtils.IsInSequence(expression, ',', input);
					}
					break;
				case ValidateType.Int:	//整数验证
					if (Validator.IsInteger(input))
					{
						int iInput = int.Parse(input);
						iMinVal = (minVal.Length == 0 ? Int32.MinValue : int.Parse(minVal));
						iMaxVal = (maxVal.Length == 0 ? Int32.MaxValue : int.Parse(maxVal));
						if (iInput < iMinVal || iInput > iMaxVal)
						{
							isValid = false;
						}
					}
					else
					{
						isValid = false;
					}
					break;
				case ValidateType.Number:	//数值验证
					if (Validator.IsNumeric(input))
					{
						double dInput = double.Parse(input);
						double dMinVal = (minVal.Length == 0 ? Double.MinValue : double.Parse(minVal));
						double dMaxVal = (maxVal.Length == 0 ? Double.MaxValue : double.Parse(maxVal));
						if (dInput < dMinVal || dInput > dMaxVal)
						{
							isValid = false;
						}
					}
					else
					{
						isValid = false;
					}
					break;
				case ValidateType.Date:	//日期验证
                    DateTime dtInput;
                    if (DateTime.TryParse(input, out dtInput))
                    {
                        DateTime dtMinVal = (minVal.Length == 0 ? DateTime.MinValue : DateTime.Parse(minVal));
                        DateTime dtMaxVal = (maxVal.Length == 0 ? DateTime.MaxValue : DateTime.Parse(maxVal));
                        if (dtInput < dtMinVal || dtInput > dtMaxVal)
                        {
                            isValid = false;
                        }
                    }
					else
					{
						isValid = false;
					}
					break;
				default:
					break;
			}

			return isValid;
		}
	}
}
