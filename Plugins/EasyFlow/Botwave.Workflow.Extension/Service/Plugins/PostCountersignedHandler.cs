using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Plugin;

namespace Botwave.Workflow.Extension.Service.Plugins
{
    /// <summary>
    /// 会签的后续处理器实现类.
    /// </summary>
    public class PostCountersignedHandler : IPostCountersignedHandler
    {
        private log4net.ILog log = log4net.LogManager.GetLogger(typeof(PostCountersignedHandler));

        #region IPostCountersignedHandler 成员

        private IPostCountersignedHandler next = null;

        /// <summary>
        /// 下一后续处理器对象.
        /// </summary>
        public IPostCountersignedHandler Next
        {
            get { return next; }
            set { next = value; }
        }

        /// <summary>
        /// 执行.
        /// </summary>
        /// <param name="countersigned"></param>
        public void Execute(Countersigned countersigned)
        {
            log.Debug(countersigned);
        }

        #endregion
    }
}
