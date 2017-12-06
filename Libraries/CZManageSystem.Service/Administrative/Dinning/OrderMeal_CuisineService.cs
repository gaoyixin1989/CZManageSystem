using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.Administrative.Dinning;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.Administrative.Dinning
{
    public class OrderMeal_CuisineService : BaseService<OrderMeal_Cuisine>, IOrderMeal_CuisineService
    {
        private readonly IRepository<OrderMeal_DinningRoom> _dinningroom = new EfRepository<OrderMeal_DinningRoom>();
        private readonly IRepository<DataDictionary> _datadictionary = new EfRepository<DataDictionary>();
        public IList<OrderMeal_Cuisine> GetForPagingByCondition(out int count, OrderMeal_CuisineQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {

            var curData = from hrvc in this._entityStore.Table
                          join bwu in this._dinningroom.Table
                          on hrvc.DinningRoomID equals bwu.Id
                          select new
                          {
                              hrvc.Id,
                              hrvc.DinningRoomID,
                              hrvc.CuisineName,
                              hrvc.CuisineType,
                              bwu.DinningRoomName
                          };

            if (objs.CuisineType != null && objs.CuisineType != "")
                curData = curData.Where(u => u.CuisineType.Contains(objs.CuisineType));
            if (objs.CuisineName != null && objs.CuisineName != "")
                curData = curData.Where(u => u.CuisineName.Contains(objs.CuisineName));
            if (objs.DinningRoomName != null && objs.DinningRoomName != "")
                curData = curData.Where(u => u.DinningRoomName.Contains(objs.DinningRoomName));
            if (objs.DinningRoomID != null && objs.DinningRoomID != "")
            {
                Guid _tmp = Guid.Parse(objs.DinningRoomID);
                curData = curData.Where(u => u.DinningRoomID == _tmp);
            }
                
            var list = curData.OrderByDescending(p => p.Id).Skip(pageIndex * pageSize).Take(pageSize).AsEnumerable().Select(x => new OrderMeal_Cuisine()
            {
                Id = x.Id,
                CuisineName = x.CuisineName,
                CuisineType = x.CuisineType,
                DinningRoomID = x.DinningRoomID
            });
            count = curData.Count();
            return list.ToList();
        }

        public IList<OrderMeal_CuisineTreeData> GetDictNameGroup(Guid DinningRoomId)

        {
            List<OrderMeal_CuisineTreeData> resultList = new List<OrderMeal_CuisineTreeData>();
            
            var list = this._entityStore.Table;
            if (DinningRoomId != Guid.Empty)
                list = list.Where(u => u.DinningRoomID == DinningRoomId);
            var dd = this._datadictionary.Table;
            foreach (var item in list)

            {
                var _tmpdd = dd.Where(u => u.DDValue == item.CuisineType).Select(u => u.DDId).ToList();
                
                OrderMeal_CuisineTreeData _tmp = new OrderMeal_CuisineTreeData();

                if (_tmpdd.Count > 0)
                {
                    _tmp.ParentId = _tmpdd[0].ToString();
                }
                else
                {
                    _tmp.ParentId = "00000000-0000-0000-0000-000000000000";
                }

                _tmp.CuisineType = item.CuisineType;

                _tmp.CuisineName = item.CuisineName;

                _tmp.Id = item.Id;

                resultList.Add(_tmp);

            }

            return resultList;

        }
    }
}
