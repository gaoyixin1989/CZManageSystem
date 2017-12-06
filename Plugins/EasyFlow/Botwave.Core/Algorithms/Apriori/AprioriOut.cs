using System;
using System.Collections;

namespace Botwave.Algorithms.Apriori
{
	/// <summary>
	/// AprioriOut 的摘要说明。
	/// </summary>
	public abstract class AprioriOut
	{
        /// <summary>
        /// 保存记录结果.
        /// </summary>
        /// <param name="record"></param>
        public abstract void SaveResult(IList record);
	}
}
