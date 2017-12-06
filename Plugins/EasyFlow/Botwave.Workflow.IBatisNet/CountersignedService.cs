using System;
using System.Collections.Generic;
using System.Text;

using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Extension.IBatisNet;

namespace Botwave.Workflow.IBatisNet
{
    public class CountersignedService : AbstractCountersignedService
    {
        #region ICountersignedService Members

        public override IList<TodoInfo> GetTodoList(Guid activityInstanceId)
        {
            return IBatisMapper.Select<TodoInfo>("bwwf_Countersigned_Select_Todo_By_ActivityInstanceId", activityInstanceId);
        }

        public override IList<Countersigned> GetCountersignedList(Guid activityInstanceId)
        {
            return IBatisMapper.Select<Countersigned>("bwwf_Countersigned_Select_By_ActivityInstanceId", activityInstanceId);
        }

        #endregion

        protected override void DoSign(Countersigned countersigned)
        {
            IBatisMapper.Insert("bwwf_Countersigned_Insert", countersigned);
        }
    }
}
