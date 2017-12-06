using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace Botwave.XQP.API.Enums
{
    /// <summary>
    /// 处理命令的集合
    /// </summary>
    public enum ManageCommandEnum
    {        
        /// <summary>
        /// 评论工单
        /// </summary>
        comment,
        /// <summary>
        /// 发起一个新的需求单工单
        /// </summary>
        start,
        /// <summary>
        /// 传入表单数据以处理需求单(approve：通过审核。reject：退回工单。cancel：取消工单)
        /// </summary>
        execute,
        /// <summary>
        /// 部署流程
        /// </summary>
        deploy,
        /// <summary>
        /// 保存工单
        /// </summary>
        save,
        /// <summary>
        /// 转交工单
        /// </summary>
        assign
    }
}
