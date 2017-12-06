using CZManageSystem.Data;
using CZManageSystem.Data.Domain.Administrative.BirthControl;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CZManageSystem.Data;
namespace CZManageSystem.Service.Administrative.BirthControl
{
    public class BirthControlConfigService : BaseService<BirthControlConfig>, IBirthControlConfigService
    {
        static Users _user;
        static List<DataDictionary> listDic = new List<DataDictionary>();
    }
}
