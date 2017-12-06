using CZManageSystem.Data.Domain.HumanResources.Integral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

namespace CZManageSystem.Service.HumanResources.Integral
{
    public interface ITIntegralConfigService : IBaseService<HRTIntegralConfig>
    {
        IList<HRTIntegralConfig> JudgeConfig(double value);
    }
}
