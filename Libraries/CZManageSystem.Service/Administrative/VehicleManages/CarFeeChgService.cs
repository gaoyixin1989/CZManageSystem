using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.Administrative.VehicleManages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.Administrative
{
    public class CarFeeChgService : BaseService<CarFeeChg>, ICarFeeChgService
    {
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="count">返回记录总数</param>
        /// <param name="objs">条件数组</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        //public override IEnumerable<dynamic> GetForPaging(out int count, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        //{
            
        //    var source = objs == null ? this._entityStore.Table.OrderBy(c => c.CarFeeChgId) : this._entityStore.Table.OrderBy(c => c.CarFeeChgId).Where(ExpressionFactory(objs));
        //    PagedList<CarFeeChg> pageList = new PagedList<CarFeeChg>(source, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count);
        //    count = pageList.TotalCount;
        //    return pageList.Select(u => new
        //    {
        //        u.CarFeeChgId,
        //        u.CarId,
        //        u.CorpId,
        //        u.CorpName,
        //        u.EatFee,
        //        u.EditorId,
        //        u.EditTime,
        //        u.FixFee,
        //        u.LiveFee,
        //        u.OilCount,
        //        u.OilFee,
        //        u.OilPrice,
        //        u.OtherFee,
        //        u.PayTime,
        //        u.Person,
        //        u.Remark,
        //        u.RoadLast,
        //        u.RoadCount,
        //        u.RoadFee,
        //        u.RoadThis,
        //        u.TotalFee

        //    });
        //}
    }
}
