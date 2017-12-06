using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Botwave.Commons.Threading
{
    /// <summary>
    /// ͬ������������.
    /// </summary>
    public static class SyncCallerHost
    {
        /// <summary>
        /// ����ָ�����ýӿ�.
        /// </summary>
        /// <param name="syncCaller"></param>
        public static void Run(ISyncCaller syncCaller)
        {
            if (null != syncCaller)
            {
                ThreadStart start = new ThreadStart(syncCaller.Call);
                Thread t = new Thread(start);
                t.Start(); 
            }            
        }
    }
}
