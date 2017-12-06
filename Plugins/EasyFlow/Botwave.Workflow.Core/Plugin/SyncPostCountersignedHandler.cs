using System;
using System.Collections.Generic;
using System.Text;

using Botwave.Workflow.Domain;

namespace Botwave.Workflow.Plugin
{
    /// <summary>
    ///会签后续处理器的同步实现类.
    /// </summary>
    public class SyncPostCountersignedHandler : Botwave.Commons.Threading.ISyncCaller
    {
        private IPostCountersignedHandler postCountersignedHandler;
        private Countersigned countersigned;

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="postCountersignedHandler"></param>
        /// <param name="countersigned"></param>
        public SyncPostCountersignedHandler(IPostCountersignedHandler postCountersignedHandler,
            Countersigned countersigned)
        {
            this.postCountersignedHandler = postCountersignedHandler;
            this.countersigned = countersigned;
        }

        #region ISyncCaller Members

        /// <summary>
        /// 调用.
        /// </summary>
        public void Call()
        {
            if (null != postCountersignedHandler)
            {
                postCountersignedHandler.Execute(countersigned);
                IPostCountersignedHandler next = postCountersignedHandler.Next;
                while (null != next)
                {
                    next.Execute(countersigned);
                    next = next.Next;
                }
            }
        }

        #endregion
    }
}
