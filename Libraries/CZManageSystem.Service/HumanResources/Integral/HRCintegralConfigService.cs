using CZManageSystem.Data;
using CZManageSystem.Data.Domain.HumanResources.Integral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.HumanResources.Integral
{
    public class HRCintegralConfigService : BaseService<HRCintegralConfig>, IHRCintegralConfigService
    {
        public IList<HRCintegralConfig> JudgeConfig(double value)
        {
            var curData = this._entityStore.Table;
            var list = curData.Where(u => u.Maxdays > value).Where(u => u.Mindays <= value);            
            return list.ToList();
        }
    }
}
