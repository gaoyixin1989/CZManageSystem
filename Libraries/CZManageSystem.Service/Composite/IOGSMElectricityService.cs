using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CZManageSystem.Core;
using CZManageSystem.Data.Domain.Composite;
using System.Linq.Expressions;
using ZManageSystem.Core;
using System.IO;
using CZManageSystem.Data.Domain.SysManger;

namespace CZManageSystem.Service.Composite
{
    public interface IOGSMElectricityService:IBaseService<OGSMElectricity>
    {
        IList<OGSMElectricity> GetForPagingByCondition(out int count, int pageIndex = 0, int pageSize = int.MaxValue, OGSMElectricityQueryBuilder obj = null);
        IList<OGSMElectricity> GetForPagingIByCondition(OGSMInfoQueryBuilder USR_NBR = null);
        dynamic ImportOGSMBase(Stream fileStream, Users user);
    }
}
