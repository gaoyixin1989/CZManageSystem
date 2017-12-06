using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CZManageSystem.Admin.Models
{
    public class SystemResult
    {
        /// <summary>
        /// 成功与否
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string Message = "";
        /// <summary>
        /// 返回数据
        /// </summary>
        public object data { get; set; }

    }
    public class WorkFlowResult
    {
        public int Success { get; set; }
        public string Errmsg { get; set; }
        public Tracking_Workflow WorkFlow { get; set; }
    }
}