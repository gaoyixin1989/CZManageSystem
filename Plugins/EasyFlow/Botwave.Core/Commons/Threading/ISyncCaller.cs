using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Commons.Threading
{
    /// <summary>
    /// 同步调用服务接口.
    /// </summary>
    public interface ISyncCaller
    {
        /// <summary>
        /// 执行调用.
        /// </summary>
        void Call();
    }
}
