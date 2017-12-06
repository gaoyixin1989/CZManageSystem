using CZManageSystem.Data;
using CZManageSystem.Data.Domain.Administrative.BirthControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.Administrative.BirthControl
{
    public class BirthControlApplyService : BaseService<BirthControlApply>, IBirthControlApplyService
    {
        public IList<BirthControlApply> GetProcessingList()
        {
            var query = this._entityStore.Table.Where(a => a.State == 1);
            List<BirthControlApply> List = new List<BirthControlApply>(query);
            return List;
        }
    }
}
