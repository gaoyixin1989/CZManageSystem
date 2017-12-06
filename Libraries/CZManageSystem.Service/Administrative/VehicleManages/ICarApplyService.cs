
using CZManageSystem.Data.Domain.Administrative.VehicleManages;
using System.Collections.Generic;
using ZManageSystem.Core;

namespace CZManageSystem.Service.Administrative
{
    public interface ICarApplyService : IBaseService<CarApply>
    {
        IEnumerable<dynamic> GetCarEvaluation(out int count, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}