using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Plugin;
using Botwave.Workflow.Extension.Util;

namespace Botwave.Workflow.Extension.Service.Plugins
{
    /// <summary>
    /// 流程活动(步骤)执行的后续处理器实现类.
    /// </summary>
    public class PostActivityExecutionHandler : IPostActivityExecutionHandler
    {
        #region IPostActivityExecutionHandler 成员

        private IPostActivityExecutionHandler next = null;

        /// <summary>
        /// 下一后续处理器对象.
        /// </summary>
        public IPostActivityExecutionHandler Next
        {
            get { return next; }
            set { this.next = value; }
        }

        #endregion

        #region IActivityExecutionHandler 成员

        /// <summary>
        /// 执行处理.
        /// </summary>
        /// <param name="context"></param>
        public void Execute(ActivityExecutionContext context)
        {

        }

        #endregion
    }
}
