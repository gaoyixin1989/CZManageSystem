using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace Botwave.XQP.API.Entity
{
    [Serializable]
    public class WorkflowDefinitionResult : ApiResult
    {
        /// <summary>
        /// 流程定义xml
        /// </summary>
        public string ExportFlowStr { get; set; }
    }
}
