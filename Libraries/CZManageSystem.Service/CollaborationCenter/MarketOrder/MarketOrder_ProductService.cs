using Aspose.Cells;
using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 营销订单-商品维护
/// </summary>
namespace CZManageSystem.Service.CollaborationCenter.MarketOrder
{
    public class MarketOrder_ProductService : BaseService<MarketOrder_Product>, IMarketOrder_ProductService
    {
        IMarketOrder_EndTypeService _endTypeService = new MarketOrder_EndTypeService();
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="count">返回记录总数</param>
        /// <param name="objs">条件数组</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public override IEnumerable<dynamic> GetForPaging(out int count, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            //var curTable = GetQueryTable(objs);
            var curTable = this._entityStore.Table;
            if (objs != null)
            {
                var exp = ExpressionFactory(objs);
                curTable = curTable.Where(exp);
            }

            return new PagedList<MarketOrder_Product>(curTable.OrderBy(c => c.ProductID), pageIndex <= 0 ? 0 : pageIndex, pageSize, out count);
        }

        static Users _user;
        //导入功能
        public dynamic Import(Stream fileStream, Users user)
        {
            try
            {
                bool IsSuccess = false;
                int count = 0;
                _user = user;
                string error = "";
                int row = 2;
                var dataTable = ExcelToDatatable(fileStream);
                if (dataTable.Rows.Count < 1)
                    return new { IsSuccess = false, Message = "读取到Excel记录为0！" };

                List<MarketOrder_Product> list = new List<MarketOrder_Product>();
                List<MarketOrder_Product> Questions = new List<MarketOrder_Product>();

                foreach (DataRow item in dataTable.Rows)
                {
                    row++;
                    List<string> tip = new List<string>();
                    var model = GetModel(item, out tip);
                    if (model == null)
                    {
                        error += (string.IsNullOrEmpty(error) ? "" : "<br/>") + "第" + row + "行:" + string.Join(";", tip);
                        continue;
                    }

                    if (IsUsed_ProductID(model) || list.Where(u => u.ProductID == model.ProductID).Count() > 0)
                    {
                        error += (string.IsNullOrEmpty(error) ? "" : "<br/>") + "第" + row + "行:商品序号已经被占用";
                        continue;
                    }

                    list.Add(model);
                    if (list.Count == 100) //足够100的
                    {
                        if (this.InsertByList(list))
                        {
                            Questions.AddRange(list);
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
                        Questions.AddRange(list);
                        count += list.Count;
                        list.Clear();
                        IsSuccess = true;
                    };
                }
                int falCount = dataTable.Rows.Count - count;
                //result.Message = "导入成功";
                var result = new
                {
                    IsSuccess = IsSuccess,
                    Message = "成功导入" + count + "条，失败" + falCount + "条。其他提示：<br/>" + error
                    //, Questions = Questions
                };
                return result;
            }
            catch (Exception ex)
            {
                return new { IsSuccess = false, Message = "文件内容错误！" };
            }
        }

        //将excel文件的stream转化为DataTable
        private DataTable ExcelToDatatable(Stream fileStream)
        {
            try
            {
                Workbook book = new Workbook(fileStream);
                Worksheet sheet = book.Worksheets[0];
                Cells cells = sheet.Cells;
                DataTable dt = new DataTable("Workbook");
                return cells.ExportDataTableAsString(1, 0, cells.MaxDataRow, cells.MaxDataColumn + 1, true);

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private MarketOrder_Product GetModel(DataRow dataRow, out List<string> tip)
        {
            List<string> tempListStr = new List<string>();
            tip = new List<string>();
            var _temp = dataRow.ItemArray;
            MarketOrder_Product temp = new MarketOrder_Product();
            if (string.IsNullOrEmpty(_temp[0]?.ToString().Trim()))
                tip.Add("商品序号不能为空");
            else
                temp.ProductID = _temp[0]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[1]?.ToString().Trim()))
                tip.Add("商品机型不能为空");
            else
            {
                var strName = _temp[1]?.ToString().Trim();
                var obj = _endTypeService.FindByFeldName(u => u.EndType == strName);
                if (obj != null && obj.ID != Guid.Empty)
                    temp.ProductTypeID = obj.ID;
                else
                    tip.Add("终端机型中没有对应的该商品机型");
            }
            if (!string.IsNullOrEmpty(_temp[2]?.ToString().Trim()))
                temp.Remark = _temp[2]?.ToString().Trim();

            temp.ID = Guid.NewGuid();
            if (tip.Count > 0)
                return null;
            return temp;
        }

        /// <summary>
        /// 商品编号是否已经被使用
        /// </summary>
        /// <param name="dataObj"></param>
        /// <returns></returns>
        private bool IsUsed_ProductID(MarketOrder_Product dataObj)
        {
            var temp = this.List().Where(u => u.ProductID == dataObj.ProductID && u.ID != dataObj.ID).Count();
            if (temp > 0)
                return true;
            else
                return false;
        }

    }
}
