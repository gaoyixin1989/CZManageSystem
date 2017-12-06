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
    public class ComebackChildService : BaseService<ComebackChild>, IComebackChildService
    {
        public Users _user { get; set; }
        public IEnumerable<dynamic> GetForPaging(out int count, ComebackChildQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var curTable = this._entityStore.Table;
            if (objs.YearStart != null)
                curTable = curTable.Where(a => a.Year >= objs.YearStart);
            if (objs.YearEnd != null)
            {
                curTable = curTable.Where(a => a.Year <= objs.YearEnd);
            }
            if (objs.Name != null)
                curTable = curTable.Where(a => a.Name.Contains(objs.Name));
            if (objs.BudgetDept != null)
                curTable = curTable.Where(a => a.ComebackType.BudgetDept.Contains(objs.BudgetDept));
            if (objs.ProName != null)
                curTable = curTable.Where(a => a.ComebackType.ComebackSource.Name.Contains(objs.ProName));

            return new PagedList<ComebackChild>(curTable.OrderByDescending(c => c.ID), pageIndex <= 0 ? 0 : pageIndex, pageSize, out count).Select(s => new
            {
                s.ID,
                s.Year,
                s.Amount,
                s.Remark,
                s.Name,
                BudgetDept= s.ComebackType.BudgetDept,
                ProName= s.ComebackType.ComebackSource.Name,
                RemainAmount = s.Amount == null ? 0 : s.Amount - this._entityStore.Execute<decimal>(" select  [dbo].[GetComebackChildAmount]('" + s.ID + "')").First()
            });
        }

        public IEnumerable<dynamic> GetReport(out int count, ComebackReporteQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var curTable = this._entityStore.Table;
            if (objs.YearStart != null)
                curTable = curTable.Where(a => a.ComebackType.ComebackSource.Year >= objs.YearStart);
            if (objs.YearEnd != null)
            {
                curTable = curTable.Where(a => a.ComebackType.ComebackSource.Year <= objs.YearEnd);
            }
            if (objs.BudgetDept != null)
                curTable = curTable.Where(a => a.ComebackType.BudgetDept.Contains(objs.BudgetDept));
            return new PagedList<ComebackChild>(curTable.OrderByDescending(c => new { c.ComebackType.ComebackSource.Year, c.ComebackType.ComebackSource.BudgetDept }), pageIndex <= 0 ? 0 : pageIndex, pageSize, out count).Select(s => new
            {
                SourceID = s.ComebackType.ComebackSource.ID,
                SourceBudgetDept = s.ComebackType.ComebackSource.BudgetDept,
                SourceAmount = s.ComebackType.ComebackSource.Amount,
                SourceName = s.ComebackType.ComebackSource.Name,
                SourceYear = s.ComebackType.ComebackSource.Year,
                SourceRemainAmount = s.ComebackType.ComebackSource.Amount == null ? 0 : s.ComebackType.ComebackSource.Amount - this._entityStore.Execute<decimal>(" select  [dbo].[getComebackSourceAmount]('" + s.ComebackType.ComebackSource.ID + "')").First(),
                TypeID=s.ComebackType.ID,
                TypePID= s.ComebackType.PID,
                TypeBudgetDept = s.ComebackType.BudgetDept,
                TypeAmount= s.ComebackType.Amount,
                TypeRemainAmount = s.ComebackType.Amount == null ? 0 : s.ComebackType.Amount - this._entityStore.Execute<decimal>(" select  [dbo].[getComebackTypeAmount]('" + s.ComebackType.ID + "')").First(),
                //ChildID=s.ID,
                //ChildName=s.Name,
                //ChildYear= s.Year,
                ChildPID = s.PID,
                ChildAmount = s.Amount,
                ChildRemainAmount = s.Amount == null ? 0 : s.Amount - this._entityStore.Execute<decimal>(" select  [dbo].[GetComebackChildAmount]('" + s.ID + "')").First()

            });
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

                List<ComebackChild> list = new List<ComebackChild>();
                foreach (DataRow item in dataTable.Rows)
                {
                    row++;
                    var model = GetModel(item, out tip);
                   
                    if (model==null )
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

        ComebackChild GetModel(DataRow dataRow, out List<string> tip)
        {
            tip = new List<string>();
            var _temp = dataRow.ItemArray;
            ComebackChild temp = new ComebackChild();
            if (string.IsNullOrEmpty(_temp[1]?.ToString().Trim()))
                tip.Add("年不能为空");
            else
                temp.Year =Convert.ToInt32( _temp[1].ToString().Trim());
            if (string.IsNullOrEmpty(_temp[2]?.ToString().Trim()))
                tip.Add("部门不能为空");
            if (string.IsNullOrEmpty(_temp[3]?.ToString().Trim()))
                tip.Add("归口项目不能为空");
            else
            {              
                var dept = _temp[2].ToString().Trim();
                var proj = _temp[3].ToString().Trim();
                var tep = new ComebackSourceService().List().Where(s => s.BudgetDept == dept&&s.Name==proj).Select(s => s.ID).ToList();
                var sourid = tep[0];
               var  type= new ComebackTypeService().List().Where(s => s.BudgetDept == dept && s.PID == sourid).Select(s => s.ID).ToList();
                if (type.Count == 0)
                    tip.Add("归口项目不存在");
                else
                    temp.PID = type[0];
            }
            if (string.IsNullOrEmpty(_temp[4]?.ToString().Trim()))
                tip.Add("归口小项不能为空");
            else
                temp.Name = _temp[4].ToString().Trim();

            temp.Amount = Convert.ToDecimal(_temp[5]?.ToString().Trim());
            temp.Remark = _temp[6]?.ToString().Trim();

            temp.ID = Guid.NewGuid();
            if (tip.Count > 0)
                return null;
            return temp;
        }
    }
}
