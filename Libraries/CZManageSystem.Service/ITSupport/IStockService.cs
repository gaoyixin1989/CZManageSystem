using CZManageSystem.Data;
using CZManageSystem.Data.Domain.ITSupport;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

namespace CZManageSystem.Service.ITSupport
{
    public interface IStockService : IBaseService<Stock>
    {
        IEnumerable<dynamic> GetForPaging(out int count, StockQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
        IEnumerable<dynamic> EquipStockNum(out int count, EquipAppQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);

        IEnumerable<dynamic> Outmatinfo(out int count, List<Stock> stock, OutstockQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
        IEnumerable<dynamic> InStockinfo(out int count, List<Stock> stock, StockQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
        /// <summary>
        /// 设备资产管理
        /// </summary>
        /// <param name="count"></param>
        /// <param name="objs"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IEnumerable<dynamic> EquipAsset(out int count, EquipAssetQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);

        dynamic ImportStock(Stream fileStream, Users user);

    }
}
