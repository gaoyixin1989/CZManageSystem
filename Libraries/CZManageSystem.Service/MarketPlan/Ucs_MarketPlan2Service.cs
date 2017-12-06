using Aspose.Cells;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using CZManageSystem.Data.Domain.MarketPlan;

namespace CZManageSystem.Service.MarketPlan
{
    public class Ucs_MarketPlan2Service : BaseService<Ucs_MarketPlan2>, IUcs_MarketPlan2Service
    {
        static Users _user;
        IUcs_MarketPlanLogService _ucs_MarketPlanLogService = new Ucs_MarketPlanLogService();
        /// <summary>
        /// 批量导入
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public dynamic ImportDelUcs_MarketPlan2(Stream fileStream, Users user)
        {
            Ucs_MarketPlanLog log = new Ucs_MarketPlanLog();
            log.Id = Guid.NewGuid();
            try
            {
                bool IsSuccess = false;
                int count = 0;
                int row = 1;
                string error = "";
                _user = user;
                var dataTable = ExcelToDatatable(fileStream);
                if (dataTable.Rows.Count < 1)
                    return new { IsSuccess = false, Message = "读取到Excel记录为0！" };

                List<Ucs_MarketPlan2> list = new List<Ucs_MarketPlan2>();
                List<string> tip = new List<string>();
               
                foreach (DataRow item in dataTable.Rows)
                {
                    row++;
                    var model = GetModel(item, out tip);
                    if (model == null)
                    {
                        error += "第" + row + "行:" + string.Join(";", tip);
                        continue;
                    }
                    list.Add(model);                  
                    log.Name = model.Name;
                    log.Coding = model.Coding;
                    log.Creator = _user.UserName;
                    log.Creattime = DateTime.Now;
                    log.Department = _user.Dept.DpName;
                    if (list.Count == 100) //足够100的
                    {
                        if (this.DeleteByList(list))
                        {
                            count += list.Count;
                            list.Clear();
                            IsSuccess = true;
                        };
                    }
                }
                if (list.Count > 0)//不足100的
                {
                    if (this.DeleteByList(list))
                    {
                        count += list.Count;
                        list.Clear();
                        IsSuccess = true;
                    };
                }
                int falCount = dataTable.Rows.Count - count;
                var result = new
                {
                    IsSuccess = IsSuccess,
                    Message = "成功删除" + count + "条，失败" + falCount + "条。其他提示：" + error
                };
                log.Remark = result.Message;
                _ucs_MarketPlanLogService.Insert(log);
                return result;
            }
            catch (Exception ex)
            {
                var result = new { IsSuccess = false, Message = "文件内容错误！" };
                log.Remark = result.Message;
                _ucs_MarketPlanLogService.Insert(log);
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
                return cells.ExportDataTableAsString(0, 0, cells.MaxDataRow + 1, cells.MaxDataColumn + 1, true);

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        Ucs_MarketPlan2 GetModel(DataRow dataRow, out List<string> tip)
        {
            tip = new List<string>();
            var _temp = dataRow.ItemArray;
            Ucs_MarketPlan2 temp = new Ucs_MarketPlan2();
            if (string.IsNullOrEmpty(_temp[0]?.ToString().Trim()))
                tip.Add("营销方案编码不能为空");
            else
            {
                temp.Coding = _temp[0]?.ToString().Trim();
            }
            temp=this.FindByFeldName(t => t.Coding == temp.Coding);
            return temp;
        }
    }
}
