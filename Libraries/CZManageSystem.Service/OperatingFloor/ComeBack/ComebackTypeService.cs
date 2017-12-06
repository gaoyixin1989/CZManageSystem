using Aspose.Cells;
using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.OperatingFloor.ComeBack
{
   public  class ComebackTypeService : BaseService<ComebackType>, IComebackTypeService
    {
        public Users _user { get; set; }
        public IEnumerable<dynamic> GetForPaging(out int count, ComebackTypeQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var curTable = this._entityStore.Table;
            if (objs.YearStart != null)
                curTable = curTable.Where(a => a.ComebackSource.Year >= objs.YearStart);
            if (objs.YearEnd != null)
            {
                curTable = curTable.Where(a => a.ComebackSource.Year <= objs.YearEnd);
            }
            if (objs.BudgetDept != null)
                curTable = curTable.Where(a => a.BudgetDept.Contains(objs.BudgetDept));
            return new PagedList<ComebackType>(curTable.OrderByDescending(c => c.ID), pageIndex <= 0 ? 0 : pageIndex, pageSize, out count).Select(s => new
            {
                s.ID,
                s.BudgetDept,
                s.Amount,
                s.Remark,
                s.ComebackSource.Name,
                s.ComebackSource.Year,
                RemainAmount = s.Amount == null ? 0 : s.Amount - this._entityStore.Execute<decimal>(" select  [dbo].[getComebackTypeAmount]('" + s.ID + "')").First()
            });
        }

        public IEnumerable<dynamic> CheckChildRemain(Guid PID)
        {
            var typeremain = this.List().Where(s => s.PID == PID).ToList().Select(s => new {
                Year= s.ComebackSource.Year,
                s.Amount,
                RemainAmount = s.Amount == null ? 0 : s.Amount - this._entityStore.Execute<decimal>(" select  [dbo].[getComebackTypeAmount]('" + s.ID + "')").First()

            });
            return typeremain;


        }

        public dynamic Import(Users user, Stream fileStream)
        {
            try
            {
                _user = user;
                bool IsSuccess = false;
                int count = 0;
                int row = 2;
                string error = "";
                List<string> tip = new List<string>();
                var dataTable = ExcelToDatatable(fileStream);
                if (dataTable.Rows.Count < 1)
                    return new { IsSuccess = false, Message = "读取到Excel记录为0！" };

                List<ComebackType> list = new List<ComebackType>();
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
                var result = new
                {
                    IsSuccess = IsSuccess,
                    Message = "成功导入" + count + "条，失败" + falCount + "条。其他提示：" + error
                };
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
                return cells.ExportDataTableAsString(1, 0, cells.MaxDataRow, cells.MaxDataColumn + 1, true);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        ComebackType GetModel(DataRow dataRow, out List<string> tip)
        {
            tip = new List<string>();
            var _temp = dataRow.ItemArray;
            ComebackType temp = new ComebackType();
            if (string.IsNullOrEmpty(_temp[1]?.ToString().Trim()))
                tip.Add("年不能为空");
            if (string.IsNullOrEmpty(_temp[2]?.ToString().Trim()))
                tip.Add("归口项目不能为空");
            else
            {
                var year = _temp[1].ToString().Trim();
                var dept=_temp[2].ToString().Trim();
                var tep = new ComebackSourceService().List().Where(s => s.Year.ToString() == year && s.BudgetDept == dept).Select(s => s.ID).ToList();
                if (tep.Count==0)
                    tip.Add("归口项目不存在");
                else
                 temp.PID =tep[0];
            }
            if (string.IsNullOrEmpty(_temp[3]?.ToString().Trim()))
                tip.Add("部门不能为空");
            else
                temp.BudgetDept = _temp[3].ToString().Trim();

            temp.Amount = Convert.ToDecimal(_temp[4]?.ToString().Trim());
            temp.Remark = _temp[5]?.ToString().Trim();

            temp.ID = Guid.NewGuid();
            if (tip.Count > 0)
                return null;
            return temp;
        }

    }
}
