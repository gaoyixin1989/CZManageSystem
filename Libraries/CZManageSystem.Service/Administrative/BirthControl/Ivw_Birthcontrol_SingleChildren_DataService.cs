﻿using CZManageSystem.Data.Domain.Administrative.BirthControl;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

namespace CZManageSystem.Service.Administrative.BirthControl
{
    public interface IVW_Birthcontrol_SingleChildren_DataService : IBaseService<VW_Birthcontrol_SingleChildren_Data>
    {
        IList<VW_Birthcontrol_SingleChildren_Data> GetForPagingByCondition(Users user, out int count, BirthControlGEBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
