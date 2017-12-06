using System;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace Botwave.Commons
{
	/// <summary>
	/// ���ݱ���֤���ࡣ
	/// </summary>
	public static class DataTableValidator
	{
        /// <summary>
        /// ������֤����ö��.
        /// </summary>
		public enum ValidateType
		{
            /// <summary>
            /// �κ�ֵ.
            /// </summary>
			Any = 0,	
            /// <summary>
            /// ����ʽ.
            /// </summary>
			Regex = 1,
            /// <summary>
            /// �ı�����.
            /// </summary>
			Length = 2,
            /// <summary>
            /// ����.
            /// </summary>
			Sequence = 3,
            /// <summary>
            /// ����.
            /// </summary>
			Int = 4,	
            /// <summary>
            /// С��.
            /// </summary>
			Number = 5,
            /// <summary>
            /// ����.
            /// </summary>
			Date = 6
		}

        /// <summary>
        /// ��ȡָ�����Ƶ���֤������.
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
		/// �������ı�����.
		/// </summary>
		/// <param name="dtInput"></param>
		/// <param name="rules">e.g. rules={{"����","�����ֵ","��֤����","���ʽ","��Сֵ","���ֵ","������Ϣ"},{"colName","false","regex","[a|b]","","","ֻ������a��b"},{"colName","false","length","","1","6","�����ַ������ȱ���С�ڵ���6�Ҵ��ڵ���1"}}</param>
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
					if (allowEmpty)	//����Ϊ��ֵ
					{
						if (input.Length != 0)	//���벻Ϊ����ָ����ʽ��飬��������һ��
						{
							isValid = CheckInput(input, validateType, expression, minVal, maxVal);
						}
					}
					else	//������Ϊ��ֵ
					{
						if (input.Length == 0)//����Ϊ��
						{
                            isValid = false;
						}
						else		//���벻Ϊ����ָ����ʽ���
						{
							isValid = CheckInput(input, validateType, expression, minVal, maxVal);
						}
					}

					if (!isValid)
					{                        
						containErrorItems = true;
                        sb.Append("��[");
                        sb.Append(i + 1);
                        sb.Append("]�У�[");
                        sb.Append(colName);
                        sb.Append("]��ʽ���󣬴�����Ϣ��[");
						sb.Append(errorInfo);
                        sb.Append("]��������ֵ��[");
						sb.Append(input);
                        sb.Append("]��");
                        sb.Append("<br />");
						//sb.Append(Environment.NewLine);
					}
				}
			}

			errorMsg = sb.ToString();
			return !containErrorItems;
		}

        /// <summary>
        /// ������������.
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
				case ValidateType.Regex:	//����ʽ��֤
					if (!Regex.IsMatch(input, expression))
					{
						isValid = false;
					}
					break;
				case ValidateType.Length:	//�ı�������֤
					iMinVal = (minVal.Length == 0 ? 0 : int.Parse(minVal));
					iMaxVal = (maxVal.Length == 0 ? 0 : int.Parse(maxVal));

					if ((iMinVal != 0 && input.Length < iMinVal)
						|| (iMaxVal != 0 && input.Length > iMaxVal))
					{
						isValid = false;
					}
					break;
				case ValidateType.Sequence:	//������֤
					if (expression.Length != 0)	//ָ��������
					{
						isValid = StringUtils.IsInSequence(expression, ',', input);
					}
					break;
				case ValidateType.Int:	//������֤
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
				case ValidateType.Number:	//��ֵ��֤
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
				case ValidateType.Date:	//������֤
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
