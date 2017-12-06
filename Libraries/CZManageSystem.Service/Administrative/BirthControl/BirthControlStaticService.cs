using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.Administrative.BirthControl;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.Administrative.BirthControl
{
    public class BirthControlStaticService : BaseService<BirthControlStatic>, IBirthControlStaticService
    {
        static Users _user;
        public IList<object> GetForPagingByCondition(Users user, string DpId = null, string StartTime = null, string EndTime = null)
        {
            _user = user;          
            var mm3 = new EfRepository<BirthControlStatic>().ExecuteResT<BirthControlStatic>(string.Format("exec BirthControl_GetStaticData '{0}','{1}','{2}','{3}'",StartTime,EndTime,_user.UserName,DpId));
            List<object> resultList = new List<object>();
            foreach (var x in mm3)
            {
                resultList.Add(x);
            }
            return resultList;
        }
    }
}
