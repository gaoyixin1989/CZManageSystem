using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Botwave.Commons.Threading
{
    /// <summary>
    /// 同步调用宿主类.
    /// </summary>
    public static class SyncCallerHost
    {
        /// <summary>
        /// 运行指定调用接口.
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
