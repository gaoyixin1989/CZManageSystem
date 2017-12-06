using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using ZManageSystem.Core;

namespace CZManageSystem.Service.SysManger
{
    public interface ITracking_WorkflowService : IBaseService<Tracking_Workflow>
    {
        IList<string> GetCurrentActivityNames(Guid guid);
    }
}