﻿using CZManageSystem.Data.Domain.Administrative.VehicleManages;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

namespace CZManageSystem.Service.Administrative
{
    public interface IDriverFilesService:IBaseService<CarDriverInfo>
    {
        dynamic ImportDriverFiles(Stream fileStream, Users user);
    }
}
