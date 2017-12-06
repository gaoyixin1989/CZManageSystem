using System;
using System.Text.RegularExpressions;

namespace Botwave.Commons
{
	/// <summary>
	/// Validator ��ժҪ˵����
	/// </summary>
	public static class Validator
	{
		/// <summary>
		/// ��֤URL����ʽ
		/// </summary>
		public static readonly string REGEX_URL_STR = @"^http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$";
        
		/// <summary>
		/// ��֤Email����ʽ
		/// </summary>
		public static readonly string REGEX_EMAIL_STR = @"^\w+((-\w+)|(\.\w+))*\@\w+((\.|-)\w+)*\.\w+$";

		/// <summary>
		/// ��֤�л����񹲺͹����֤��������ʽ
		/// </summary>
		public static readonly string REGEX_CNIDCARD_STR = @"^\d{15}(\d{2}[\d|X])?$";

		/// <summary>
		/// ��֤�л����񹲺͹�������������ʽ
		/// </summary>
		public static readonly string REGEX_CNPOSTCODE_STR = @"^\d{6}$";

		/// <summary>
		/// ��֤�л����񹲺͹��绰��������ʽ
		/// </summary>
		public static readonly string REGEX_CNPHONE_STR = @"^(\(\d{3,4}\)|\d{3,4}-)?\d{7,8}$";

		/// <summary>
		/// ��֤�ֻ���������ʽ
		/// </summary>
		public static readonly string REGEX_MOBILE_STR = @"^((\(\d{3}\))|(\d{3}\-))?((13\d{9})|(159\d{8}))$";

		/// <summary>
		/// ��֤����
		/// </summary>
		public static readonly string REGEX_NUMERIC_STR = @"^[+|-]?\d+[.]?\d*$";

		/// <summary>
		/// ��֤���������ж��Ƿ������
		/// </summary>
		public static readonly string REGEX_INTERGER_STR = @"^[+|-]?\d+$";

		/// <summary>
		/// ��֤����
		/// </summary>
		public static readonly string REGEX_DATE_STR = @"^\s*((\d{4})|(\d{2}))([-./])(\d{1,2})\4(\d{1,2})\s*$";//@"^\s*((\d{4})|(\d{2}))([-./])(\d{1,2})\4(\d{1,2})\s*$";@"^^((\d{4})|(\d{2}))([-./])(\d{1,2})([-./])(\d{1,2})$";

        /// <summary>
        /// ��֤����(����ʱ��).
        /// </summary>
        public static readonly string REGEX_TIME_STR = @"^\s*((\d{4})|(\d{2}))([-./])(\d{1,2})([-./])(\d{1,2})\s*(\s(((0?[0-9])|([1-2][0-9]))\:([0-5]?[0-9])((\s)|(\:([0-5]?[0-9])))))?$";

 
		private static Regex REGEXP_URL = new Regex(REGEX_URL_STR);     
		private static Regex REGEXP_EMAIL = new Regex(REGEX_EMAIL_STR);     
		private static Regex REGEXP_CNIDCARD = new Regex(REGEX_CNIDCARD_STR);     
		private static Regex REGEXP_CNPOSTCODE = new Regex(REGEX_CNPOSTCODE_STR);     
		private static Regex REGEXP_CNPHONE = new Regex(REGEX_CNPHONE_STR);     
		private static Regex REGEXP_MOBILE = new Regex(REGEX_MOBILE_STR);     
		private static Regex REGEXP_NUMERIC = new Regex(REGEX_NUMERIC_STR);    
		private static Regex REGEXP_INTERGER = new Regex(REGEX_INTERGER_STR);    
		private static Regex REGEXP_DATE = new Regex(REGEX_DATE_STR);


		/// <summary>
		/// ��֤Email
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static bool IsEmail(string input)
		{
			if (String.IsNullOrEmpty(input))
			{
				return false;
			}

			return REGEXP_EMAIL.IsMatch(input);
		}

		/// <summary>
		/// ��֤�ֻ���
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static bool IsMobile(string input)
		{
			if (String.IsNullOrEmpty(input))
			{
				return false;
			}

			return REGEXP_MOBILE.IsMatch(input);
		}

		
		/// <summary>
		/// ��֤URL
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static bool IsURL(string input)
		{ 
			if (String.IsNullOrEmpty(input))
			{
				return false;
			}

			return REGEXP_URL.IsMatch(input);
		}

		/// <summary>
		/// ��֤�л����񹲺͹����֤����
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static bool IsCnIdCard(string input)
		{
			if (String.IsNullOrEmpty(input))
			{
				return false;
			}

			return REGEXP_CNIDCARD.IsMatch(input);
		}

		/// <summary>
		/// ��֤�л����񹲺͹���������
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static bool IsCnPostcode(string input)
		{          
			if (String.IsNullOrEmpty(input))
			{
				return false;
			}

			return REGEXP_CNPOSTCODE.IsMatch(input);
		}

		/// <summary>
		/// ��֤�л����񹲺͹��绰����
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static bool IsCnPhone(string input)
		{       
			if (String.IsNullOrEmpty(input))
			{
				return false;
			}

			return REGEXP_CNPHONE.IsMatch(input);
		}

		/// <summary>
		/// ��֤����
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static bool IsNumeric(string input)
		{         
			if (String.IsNullOrEmpty(input))
			{
				return false;
			}

			return REGEXP_NUMERIC.IsMatch(input);
		}

		/// <summary>
		/// ��֤����
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static bool IsInteger(string input)
		{          
			if (String.IsNullOrEmpty(input))
			{
				return false;
			}

			return REGEXP_INTERGER.IsMatch(input);
		}

		/// <summary>
		/// ��֤����
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static bool IsDate(string input)
		{
			if (String.IsNullOrEmpty(input))
			{
				return false;
			}

			return REGEXP_DATE.IsMatch(input);
		}

		/// <summary>
		/// ��֤�Ƿ�����/ʱ��
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static bool IsDateTime(string input)
		{
			if (String.IsNullOrEmpty(input))
			{
				return false;
			}

            DateTime dt = DateTime.MinValue;
            DateTime.TryParse(input, out dt);
            return (dt != DateTime.MinValue);
		}
	}
}
