using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

namespace CZManageSystem.Service.WelfareManage
{
    public interface IPersonalWelfareManageYearInfoService: IBaseService<PersonalWelfareManageYearInfo>
    {
        dynamic Import(Stream fileStream, Users user);
    }
}
