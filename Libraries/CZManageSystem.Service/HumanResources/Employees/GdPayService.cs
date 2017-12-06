using Aspose.Cells;
using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.HumanResources.Employees;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.HumanResources.Employees
{
    public class GdPayService : BaseService<GdPay>, IGdPayService
    {
        static SystemContext dbContext = new SystemContext("SqlConnectionHR");
        IGdPayIdService gdPayIdService = new GdPayIdService();
        static List<GdPayId> IncomeType;
        public GdPayService() : base(dbContext)
        { }
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
            try
            {
                var source = objs == null ? this._entityStore.Table.OrderBy(c => c.payid) : this._entityStore.Table.OrderBy(c => c.payid).Where(ExpressionFactory(objs));
                PagedList<GdPay> pageList = new PagedList<GdPay>(source, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count);
                count = pageList.TotalCount;
                return pageList.Select(s => new
                {
                    s.billcyc,
                    s.employerid,
                    s.je,
                    s.payid,
                    s.updatetime,
                    s.value_str
                });
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public dynamic Import(Stream fileStream)
        {
            try
            {
                bool IsSuccess = false;
                int count = 0;
                int row = 2;
                string error = "";
                var dataTable = ExcelToDatatable(fileStream);
                if (dataTable.Rows.Count < 1)
                    return new { IsSuccess = false, Message = "读取到Excel记录为0！" };
                IncomeType = GetIncomeTypeS();
                List<GdPay> list = new List<GdPay>();
                List<string> tip = new List<string>();
                foreach (DataRow item in dataTable.Rows)
                {
                    row++;
                    var model = GetModel(item, out tip);
                    if (model == null)
                    {
                        error += "第" + row + "行:" + string.Join("；", tip);
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
                IncomeType.Clear();
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
        GdPay GetModel(DataRow dataRow, out List<string> tip)
        {
            var model = dataRow.ItemArray;
            tip = new List<string>();
            var _temp = dataRow.ItemArray;
            GdPay temp = new GdPay();
            if (string.IsNullOrEmpty(_temp[0]?.ToString().Trim()))
                tip.Add("员工编号不能为空");
            else
                temp.employerid = _temp[0]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[1]?.ToString().Trim()))
                tip.Add("账务周期不能为空");

            int type = 1;
            if (string.IsNullOrEmpty(_temp[1]?.ToString().Trim()))
                tip.Add("资产类型不能为空");
            else if (!int.TryParse(_temp[1]?.ToString().Trim(), out type))
            {
                tip.Add("账务周期必须为正整数");
            }
            else
                temp.billcyc = type;
             
            if (string.IsNullOrEmpty(_temp[2]?.ToString().Trim()))
                tip.Add("固定收入类型不能为空");
            else
            {
                var find = IncomeType.Find(f => f.payname == _temp[2]?.ToString().Trim());
                if (find == null)
                    tip.Add("该固定收入类型不存在");
                else
                {
                    temp.payid = find.payid;
                    if (string.IsNullOrEmpty(_temp[3]?.ToString().Trim()))
                        tip.Add("收入不能为空");
                    else if (find.DataType == "num")
                    {
                        int num = 0;
                        if (!int.TryParse(_temp[3]?.ToString().Trim(), out type))
                        {
                            tip.Add("该收入类型必须为数字");
                        }
                        else
                            temp.je = num.ToString();
                    }
                    else
                        temp.value_str = _temp[3]?.ToString().Trim();
                }
            }
            if (tip.Count > 0)
                return null;
            var modelResult = this .FindByFeldName(f => f.payid == temp.payid && f.billcyc == temp.billcyc && f.employerid == temp.employerid);
            if (modelResult!=null )
            {
                tip.Add("该条记录已经存在！");
                return null;
            }
            temp.updatetime = DateTime.Now;
            return temp;
        }
        public List<GdPayId> GetIncomeTypeS()
        {
            var modelList = gdPayIdService.List().Where(w => w.pid > 0).ToList();
            return modelList;
        }
    }
}
