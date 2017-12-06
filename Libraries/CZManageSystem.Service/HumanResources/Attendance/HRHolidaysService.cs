using Aspose.Cells;
using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.HumanResources.Attendance;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace CZManageSystem.Service.HumanResources.Attendance
{
    public class HRHolidaysService : BaseService<HRHolidays>, IHRHolidaysService
    {
        public Users _user { get; set; }
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
                var source = objs == null ? this._entityStore.Table.OrderByDescending(c => c.EditTime) : this._entityStore.Table.OrderByDescending(c => c.EditTime).Where(ExpressionFactory(objs));
                PagedList<HRHolidays> pageList = new PagedList<HRHolidays>(source, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count);
                count = pageList.TotalCount;
                return pageList;
            }
            catch (Exception ex)
            {

                throw;
            }
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
                var dataTable = ExcelToDatatable(fileStream);
                if (dataTable.Rows.Count < 1)
                    return new { IsSuccess = false, Message = "读取到Excel记录为0！" };

                List<HRHolidays> list = new List<HRHolidays>();
                foreach (DataRow item in dataTable.Rows)
                {
                    row++;
                    var model = GetModel(item);
                    var valid = new ViewModelValidator(model);

                    if (!valid.IsValid())
                    {
                        error += "第" + row + "行:" + valid.ValidationErrorsToString + "；";
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
                return cells.ExportDataTableAsString(1, 0, cells.MaxDataRow, cells.MaxDataColumn + 1,true );
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        HRHolidays GetModel(DataRow dataRow)
        {
            var model = dataRow.ItemArray;
            var _temp = dataRow.ItemArray;
            HRHolidays temp = new HRHolidays() { ID = Guid.NewGuid() };
            //假日名称  
            temp.HolidayName = _temp[1]?.ToString().Trim();

            //假期类别 
            temp.HolidayClass = _temp[2]?.ToString().Trim();
            DateTime dt;
            int year;
            if (int.TryParse(_temp[3]?.ToString().Trim(), out year))
                //年度 
                temp.HolidayYear = new DateTime(year, 1, 1);

            if (DateTime.TryParse(_temp[4]?.ToString().Trim(), out dt))
                //开始时间 
                temp.StartTime = dt;
            if (DateTime.TryParse(_temp[5]?.ToString().Trim(), out dt))
                //结束时间 
                temp.EndTime = dt;
            temp.EditorId = _user.UserId;
            temp.Editor = _user.RealName;
            temp.EndTime = DateTime.Now;
            return temp;
        }
        #region 方法
        string GetDate(string date)
        {
            if (date.Length != 6)
                return "";
            DateTime dt = DateTime.ParseExact(date, "yyyyMM", System.Globalization.CultureInfo.InvariantCulture);
            return dt.ToString("yyyy年M月");
        }
        #endregion
    }
}
