using Aspose.Cells;
using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.ITSupport;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.ITSupport
{
    public partial class StockAssetService:BaseService<StockAsset>, IStockAssetService
    {
        private readonly IRepository<Stock> _stock;
        private readonly IRepository<StockAsset> _StockAsset;
        public StockAssetService()
        {
            this._stock = new EfRepository<Stock>();
            this._StockAsset = new EfRepository<StockAsset>();
        }
        public  IEnumerable<dynamic> GetStockAssetbyid(out int count, int pageIndex, int pageSize, int stockid)
        {
            var query = from sto in this._StockAsset.Table
                        join st in this._stock.Table
                        on sto.StockId equals st.Id
                        select new
                        {
                            Id = sto.Id,
                            EquipInfo = st.EquipInfo,
                            EquipClass = st.EquipClass,
                            AssetSn = sto.AssetSn,
                            State = sto.State,
                            StockId = sto.StockId
                        };
            var pageQuery = query.Where(a => a.StockId == stockid).OrderBy(a=>a.Id).Skip(pageSize * pageIndex <= 0 ? 0 : (pageIndex - 1)).Take(pageSize).ToList();
             count = pageQuery.Count();
            return pageQuery;
        }

        static Users _user;
        public dynamic Import(Stream fileStream, Users user, int id)
        {
            try
            {
                bool IsSuccess = false;
                int count = 0;
                string error = "";
                int row = 2;
                _user = user;
                var dataTable = ExcelToDatatable(fileStream);
                if (dataTable.Rows.Count < 1)
                    return false;

                List<StockAsset> list = new List<StockAsset>();
                List<string> tip = new List<string>();
                foreach (DataRow item in dataTable.Rows)
                {
                    row++;
                    var model = GetModel(item, id, out tip);
                    if (model == null)
                    {
                        error += "第" + row + "行:" + string.Join(";", tip);
                        continue;
                    }
                    list.Add(model);
                    if (list.Count == 100) //足够100的
                    {
                        if (this.InsertByList(list))
                        {
                            count += list.Count;
                            list.Clear();
                            IsSuccess = true;
                        };
                    }
                }
                if (list.Count > 0)//不足100的
                {
                    if (this.InsertByList(list))
                    {
                        count += list.Count;
                        list.Clear();
                        IsSuccess = true;
                    };
                }
                int falCount = dataTable.Rows.Count - count;
                var result = new { IsSuccess = IsSuccess, Message = "成功导入" + count + "条，失败" + falCount + "条" };
                return result;
            }
            catch (Exception ex)
            {
                var result = new { IsSuccess = false, Message = "文件内容错误！" };
                return result;
            }
        }

        DataTable ExcelToDatatable(Stream fileStream)
        {
            try
            {
                Workbook book = new Workbook(fileStream);
                Worksheet sheet = book.Worksheets[0];
                Cells cells = sheet.Cells;
                return cells.ExportDataTableAsString(1, 1, cells.MaxDataRow, cells.MaxDataColumn + 1, true);

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        StockAsset GetModel(DataRow dataRow, int id, out List<string> tip)
        {
            tip = new List<string>();
            var _temp = dataRow.ItemArray;
            StockAsset temp = new StockAsset();
            temp.StockId = id;
            //temp.Id = Guid.NewGuid();
            if (string.IsNullOrEmpty(_temp[0]?.ToString().Trim()))
                tip.Add("固定资产编码不能为空");
            else
                temp.AssetSn = _temp[0]?.ToString().Trim();
                temp.State = 0;
            if (tip.Count > 0)
                return null;
            return temp;
        }
    }
}
