using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.XQP.API.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowMenuGroupResult: ApiResult
    {
        /// <summary>
        /// 分组名称数组
        /// </summary>
        public MenuGroup[] GroupNames { get; set; }
    }

    public class MenuGroup
    {        /// <summary>
        /// 分组名称
        /// </summary>
        public string GroupName { get; set; }
    }
}
