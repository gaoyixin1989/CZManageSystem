using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService_WF.Domain
{
    /// <summary>
    /// 流程步骤定义属性
    /// </summary>
    [Serializable]
    public class Activity
    {
        /// <summary>  
        /// 步骤定义名称 
        /// </summary> 
        public string Name { get; set; }

        /// <summary>  
        /// 步骤定义处理人数组 
        /// </summary> 
        public string Actors { get; set; }

        /// <summary>
        /// 当前实例步骤ID
        /// </summary>
        public string ActivityInstanceId { get; set; }

        /// <summary>
        /// 流程节点ID
        /// </summary>
        public string ActivityId { get; set; }

        public string OperateType { get; set; }

        /// <summary>
        /// 工单处理命令
        /// </summary>
        public string Command { get; set; }

    }
}