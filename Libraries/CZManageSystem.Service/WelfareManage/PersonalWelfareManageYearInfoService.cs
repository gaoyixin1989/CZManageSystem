using Aspose.Cells;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.WelfareManage
{
   public class PersonalWelfareManageYearInfoService : BaseService<PersonalWelfareManageYearInfo>, IPersonalWelfareManageYearInfoService
    {
        static Users _user;

        /// <summary>
        /// 批量导入
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public dynamic Import(Stream fileStream, Users user)
        {
            try
            {
                bool IsSuccess = false;
                int count = 0;
                int row = 2;
                string error = "";
                _user = user;
                var dataTable = ExcelToDatatable(fileStream);
                if (dataTable.Rows.Count < 1)
                    return new { IsSuccess = false, Message = "读取到Excel记录为0！" };

                List<PersonalWelfareManageYearInfo> list = new List<PersonalWelfareManageYearInfo>();
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

        PersonalWelfareManageYearInfo GetModel(DataRow dataRow, out List<string> tip)
        {
            tip = new List<string>();
            var _temp = dataRow.ItemArray;
            PersonalWelfareManageYearInfo temp = new PersonalWelfareManageYearInfo();
            if (string.IsNullOrEmpty(_temp[0]?.ToString().Trim()))
                tip.Add("员工编号不能为空");
            else
                temp.Employee = _temp[0]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[1]?.ToString().Trim()))
                tip.Add("员工姓名不能为空");
            else
                temp.EmployeeName = _temp[1]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[2]?.ToString().Trim()))
                tip.Add("年度不能为空");
            else
                temp.CYear = _temp[2]?.ToString().Trim();
          
            Decimal TotalAmount = 0;
            if (string.IsNullOrEmpty(_temp[3]?.ToString().Trim()))
                tip.Add("个人福利总额度不能为空");
            else if (!Decimal.TryParse(_temp[3]?.ToString().Trim(), out TotalAmount))
                tip.Add("数量为小数类型");
            else
                temp.WelfareYearTotalAmount = TotalAmount;

            //temp.Editor = _user.UserName;
            temp.CreateTime = DateTime.Now;
            temp.EditTime = DateTime.Now;
            temp.YID = Guid.NewGuid();
            if (tip.Count > 0)
                return null;
            return temp;
        }
    }
}
