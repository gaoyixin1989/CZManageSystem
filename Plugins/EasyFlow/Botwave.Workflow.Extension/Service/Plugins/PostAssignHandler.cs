using System;
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
    /// 任务指派的后续处理器实现类.
    /// </summary>
    public class PostAssignHandler : IPostAssignHandler
    {
        #region IPostAssignHandler 成员

        private IPostAssignHandler next = null;

        /// <summary>
        /// 下一后续处理器对象.
        /// </summary>
        public IPostAssignHandler Next
        {
            get { return next; }
            set { next = value; }
        }

        /// <summary>
        /// 执行处理.
        /// </summary>
        /// <param name="assignment"></param>
        public void Execute(Assignment assignment)
        {

        }

        #endregion
    }
}
