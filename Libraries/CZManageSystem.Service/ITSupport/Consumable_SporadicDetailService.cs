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

namespace CZManageSystem.Service.ITSupport
{
    public class Consumable_SporadicDetailService : BaseService<Consumable_SporadicDetail>, IConsumable_SporadicDetailService
    {
        static Users _user;
        public dynamic  ImportSporadicDetail(Stream fileStream, Users user,Guid id)
        {
            try
            {
                bool IsSuccess = false;
                int count = 0;
                string error = "";
                int row = 1;
                _user = user;
                var dataTable = ExcelToDatatable(fileStream);
                if (dataTable.Rows.Count < 1)
                    return false;

                List<Consumable_SporadicDetail> list = new List<Consumable_SporadicDetail>();
                List<string> tip = new List<string>();
                foreach (DataRow item in dataTable.Rows)
                {
                    row++;
                     var model = GetModel(item,id, out tip);
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
                return cells.ExportDataTableAsString(0, 0, cells.MaxDataRow + 1, cells.MaxDataColumn + 1, true);

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        Consumable_SporadicDetail GetModel(DataRow dataRow, Guid id, out List<string> tip)
        {
            tip = new List<string>();
            var _temp = dataRow.ItemArray;
            Consumable_SporadicDetail temp = new Consumable_SporadicDetail();
            temp.ApplyID = id;
            temp.ID = Guid.NewGuid();
            if (string.IsNullOrEmpty(_temp[0]?.ToString().Trim()))
                tip.Add("零星明细不能为空");
            else
                temp.Relation = _temp[0]?.ToString().Trim();
            int ApplyCount = 1;
            if (string.IsNullOrEmpty(_temp[1]?.ToString().Trim()))
                tip.Add("数量不能为空");
            else if (!int.TryParse(_temp[1]?.ToString().Trim(), out ApplyCount))
            {
                tip.Add("数量为正整数");
            }
            else
                temp.ApplyCount = ApplyCount;
            Decimal Amount =1;
            if (string.IsNullOrEmpty(_temp[2]?.ToString().Trim()))
                tip.Add("金额不能为空");
            else if (!Decimal.TryParse(_temp[2]?.ToString().Trim(), out Amount))
            {
                tip.Add("金额为正整数或小数");
            }
            else
                temp.Amount = Amount;
            if (tip.Count > 0)
                return null;
            //temp.Relation = _temp[0].ToString();
            //temp.ApplyCount = Convert.ToInt32(_temp[1].ToString());
            // temp.Amount =Convert.ToDecimal( _temp[2].ToString());
            return temp;
        }
    }
}
