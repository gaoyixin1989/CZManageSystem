using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Extension.IBatisNet;

namespace Botwave.Workflow.Extension.Domain
{
    /// <summary>
    /// 流程步骤操作人提示信息类.
    /// </summary>
    [Serializable]
    public class TooltipInfo : ActorDetail
    {
        #region gets / sets
        
        private int workingCount;

        /// <summary>
        /// 用户待办工单数.
        /// </summary>
        public int WorkingCount
        {
            get { return workingCount; }
            set { workingCount = value; }
        }
        #endregion  
    }
}
