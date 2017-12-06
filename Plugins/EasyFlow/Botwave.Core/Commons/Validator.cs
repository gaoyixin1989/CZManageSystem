using System;
using System.Text.RegularExpressions;

namespace Botwave.Commons
{
	/// <summary>
	/// Validator 的摘要说明。
	/// </summary>
	public static class Validator
	{
		/// <summary>
		/// 验证URL正则式
		/// </summary>
		public static readonly string REGEX_URL_STR = @"^http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$";
        
		/// <summary>
		/// 验证Email正则式
		/// </summary>
		public static readonly string REGEX_EMAIL_STR = @"^\w+((-\w+)|(\.\w+))*\@\w+((\.|-)\w+)*\.\w+$";

		/// <summary>
		/// 验证中华人民共和国身份证号码正则式
		/// </summary>
		public static readonly string REGEX_CNIDCARD_STR = @"^\d{15}(\d{2}[\d|X])?$";

		/// <summary>
		/// 验证中华人民共和国邮政编码正则式
		/// </summary>
		public static readonly string REGEX_CNPOSTCODE_STR = @"^\d{6}$";

		/// <summary>
		/// 验证中华人民共和国电话号码正则式
		/// </summary>
		public static readonly string REGEX_CNPHONE_STR = @"^(\(\d{3,4}\)|\d{3,4}-)?\d{7,8}$";

		/// <summary>
		/// 验证手机号码正则式
		/// </summary>
		public static readonly string REGEX_MOBILE_STR = @"^((\(\d{3}\))|(\d{3}\-))?((13\d{9})|(159\d{8}))$";

		/// <summary>
		/// 验证数字
		/// </summary>
		public static readonly string REGEX_NUMERIC_STR = @"^[+|-]?\d+[.]?\d*$";

		/// <summary>
		/// 验证整数（不判断是否溢出）
		/// </summary>
		public static readonly string REGEX_INTERGER_STR = @"^[+|-]?\d+$";

		/// <summary>
		/// 验证日期
		/// </summary>
		public static readonly string REGEX_DATE_STR = @"^\s*((\d{4})|(\d{2}))([-./])(\d{1,2})\4(\d{1,2})\s*$";//@"^\s*((\d{4})|(\d{2}))([-./])(\d{1,2})\4(\d{1,2})\s*$";@"^^((\d{4})|(\d{2}))([-./])(\d{1,2})([-./])(\d{1,2})$";

        /// <summary>
        /// 验证日期(或者时间).
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
		/// 验证Email
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
		/// 验证手机号
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
		/// 验证URL
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
		/// 验证中华人民共和国身份证号码
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
		/// 验证中华人民共和国邮政编码
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
		/// 验证中华人民共和国电话号码
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
		/// 验证数字
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
		/// 验证整数
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
		/// 验证日期
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
		/// 验证是否日期/时间
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
