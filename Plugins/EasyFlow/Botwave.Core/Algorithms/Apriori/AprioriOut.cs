using System;
using System.Collections;

namespace Botwave.Algorithms.Apriori
{
	/// <summary>
	/// AprioriOut ��ժҪ˵����
	/// </summary>
	public abstract class AprioriOut
	{
        /// <summary>
        /// �����¼���.
        /// </summary>
        /// <param name="record"></param>
        public abstract void SaveResult(IList record);
	}
}
