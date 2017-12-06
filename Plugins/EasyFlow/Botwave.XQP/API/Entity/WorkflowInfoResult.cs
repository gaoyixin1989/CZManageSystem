using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace Botwave.XQP.API.Entity
{
    [Serializable]
    public class WorkflowInfoResult:ApiResult
    {
        public Activity[] Activitys { get; set; }

        public Activity[] NextActivityies { get; set; }

        public FiledInfo[] Fields { get; set; }
    }
}
