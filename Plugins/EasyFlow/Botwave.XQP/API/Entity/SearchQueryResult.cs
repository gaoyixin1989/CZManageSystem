using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.XQP.API.Entity
{
    [Serializable]
    public class SearchQueryResult:ApiResult
    {
        public SearchWorkflowList[] WorkflowList { get; set; }

        public Activity[] Activity { get; set; }
    }


    public class SearchWorkflowList
    {
        public string WorkflowFullName { get; set; }

        public string WorkflowName { get; set; }
    }
}
