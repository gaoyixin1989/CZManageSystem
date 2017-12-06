﻿using Aspose.Cells;
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
/// 营销订单-营销方案维护
/// </summary>
namespace CZManageSystem.Service.CollaborationCenter.MarketOrder
{
    public class MarketOrder_MarketService : BaseService<MarketOrder_Market>, IMarketOrder_MarketService
    {
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

            return new PagedList<MarketOrder_Market>(curTable.OrderBy(c => c.AbleTime).ThenBy(c=>c.DisableTime), pageIndex <= 0 ? 0 : pageIndex, pageSize, out count);
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

                List<MarketOrder_Market> list = new List<MarketOrder_Market>();
                List<MarketOrder_Market> Questions = new List<MarketOrder_Market>();

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

        private MarketOrder_Market GetModel(DataRow dataRow, out List<string> tip)
        {
            List<string> tempListStr = new List<string>();
            tip = new List<string>();
            var _temp = dataRow.ItemArray;
            MarketOrder_Market temp = new MarketOrder_Market();
            if (string.IsNullOrEmpty(_temp[0]?.ToString().Trim()))
                tip.Add("营销方案编号不能为空");
            else
                temp.Order = _temp[0]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[1]?.ToString().Trim()))
                tip.Add("营销方案名称不能为空");
            else
                temp.Market = _temp[1]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[2]?.ToString().Trim()))
                tip.Add("生效时间不能为空");
            else
            {
                DateTime tempDate = new DateTime();
                if (DateTime.TryParse(_temp[2]?.ToString().Trim(), out tempDate))
                    temp.AbleTime = tempDate;
                else
                    tip.Add("生效时间格式不正确");
            }
            if (string.IsNullOrEmpty(_temp[3]?.ToString().Trim()))
                tip.Add("失效时间不能为空");
            else
            {
                DateTime tempDate = new DateTime();
                if (DateTime.TryParse(_temp[3]?.ToString().Trim(), out tempDate))
                    temp.DisableTime = tempDate;
                else
                    tip.Add("失效时间格式不正确");
            }
            if (string.IsNullOrEmpty(_temp[4]?.ToString().Trim()))
                tip.Add("营销方案说明不能为空");
            else
                temp.Remark = _temp[4]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[5]?.ToString().Trim()))
                tip.Add("优惠费用不能为空");
            else
            {
                decimal tempValue = new decimal();
                if (decimal.TryParse(_temp[5]?.ToString().Trim(), out tempValue))
                    temp.PlanPay = tempValue;
                else
                    tip.Add("优惠费用格式不正确");
            }
            if (string.IsNullOrEmpty(_temp[6]?.ToString().Trim()))
                tip.Add("实收费用不能为空");
            else
            {
                decimal tempValue = new decimal();
                if (decimal.TryParse(_temp[6]?.ToString().Trim(), out tempValue))
                    temp.MustPay = tempValue;
                else
                    tip.Add("实收费用格式不正确");
            }
            if (string.IsNullOrEmpty(_temp[7]?.ToString().Trim()))
                tip.Add("是否家宽业务不能为空");
            else
            {
                if (_temp[7]?.ToString().Trim() == "是")
                    temp.isJK = true;
                else
                    temp.isJK = false;
            }
            
            temp.ID = Guid.NewGuid();
            if (tip.Count > 0)
                return null;
            return temp;
        }
    }
}
