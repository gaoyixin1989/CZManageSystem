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
    public interface IStockAssetService : IBaseService<StockAsset>
    {
         IEnumerable<dynamic> GetStockAssetbyid(out int count,int pageIndex, int pageSize, int stockid);
        dynamic Import(Stream fileStream, Users user, int id);
    }
}
