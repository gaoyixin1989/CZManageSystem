using CZManageSystem.Data.Domain.ITSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

namespace CZManageSystem.Service.ITSupport
{
    public interface  IEquipApplyService : IBaseService<EquipApp>
    {
        IEnumerable<dynamic> GetApplyList(out int count, string ApplyTitle = null, int pageIndex = 0, int pageSize = int.MaxValue);
        IEnumerable<dynamic> GetAssetSn(string EquipClass);

        IEnumerable<dynamic> GetApplyDetail(Guid EquipClass);
    }
}
