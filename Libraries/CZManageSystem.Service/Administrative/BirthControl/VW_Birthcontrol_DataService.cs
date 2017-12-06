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
    public class VW_Birthcontrol_DataService : BaseService<VW_Birthcontrol_Data>, IVW_Birthcontrol_DataService
    {
        static Users _user;
        public IList<VW_Birthcontrol_Data> GetForPagingByCondition(Users user,out int count, BirthControlInfoBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var curData = GetQueryTable(objs); ;
            _user = user;
            var mm = new EfRepository<string>().Execute<string>(string.Format("exec BirthControl_GetUser @UserName='{0}'", _user.UserName)).ToList();

            //var mm3 = new EfRepository<BirthControlStatic>().ExecuteResT<BirthControlStatic>(string.Format("exec BirthControl_GetStaticData '','','chenwanlin',''", _user.UserName));
            //List<object> resultList = new List<object>();
            //foreach (var x in mm3)
            //{
            //    resultList.Add(new
            //    {
            //        x.OfficialStaffCount
            //    });
            //}
            curData = curData.Where(u => mm.Contains(u.UserName.ToString()));
            count = curData.Count();
            var list = curData.OrderBy(p => "").Skip(pageIndex * pageSize).Take(pageSize);           
            return list.ToList();
        }
        /// <summary>
        /// 编辑查询条件
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        private IQueryable<VW_Birthcontrol_Data> GetQueryTable(BirthControlInfoBuilder obj = null)
        {
            var curTable = this._entityStore.Table;
            if (obj != null)
            {
                BirthControlInfoBuilder obj2 = (BirthControlInfoBuilder)CloneObject(obj);
                if (obj.DpId != null && obj.DpId.Count > 0)
                {//状态
                    curTable = curTable.Where(u => obj.DpId.Contains(u.DpId));
                    obj2.DpId = null;
                }

                var exp = ExpressionFactory(obj2);
                if (exp != null)
                    curTable = curTable.Where(exp);

            }

            return curTable;
        }


    }
    
}
