using System;

namespace Botwave.Commons
{
	/// <summary>
	/// 数学辅助类.
	/// </summary>
	public static class MathUtils
	{
        /// <summary>
        /// 与 0 比较.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
		public static bool EqualsZero(double d)
		{
			return (Math.Round(d) == 0);
		}
	}
}
