using System;

namespace Botwave.Commons
{
	/// <summary>
	/// ��ѧ������.
	/// </summary>
	public static class MathUtils
	{
        /// <summary>
        /// �� 0 �Ƚ�.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
		public static bool EqualsZero(double d)
		{
			return (Math.Round(d) == 0);
		}
	}
}
