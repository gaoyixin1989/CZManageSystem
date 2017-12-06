using Aspose.Cells;
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
    public class ConsumableService : BaseService<Consumable>, IConsumableService
    {
        static Users _user;

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="count">返回记录总数</param>
        /// <param name="objs">条件数组</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public IList<Consumable> GetForPaging(out int count, ConsumableQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var curTable = GetQueryTable(objs);
            count = curTable.Count();
            var list = curTable.OrderByDescending(u => "").Skip(pageIndex * pageSize).Take(pageSize);
            return list.ToList();

        }

        /// <summary>
        /// 编辑查询条件
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        private IQueryable<Consumable> GetQueryTable(ConsumableQueryBuilder obj = null)
        {
            var curTable = this._entityStore.Table;
            if (obj != null)
            {
                ConsumableQueryBuilder obj2 = (ConsumableQueryBuilder)CloneObject(obj);
                if (obj.hasStock.HasValue)
                {//是否存在库存量
                    if (obj.hasStock.Value == true)
                        curTable = curTable.Where(u => u.Amount > 0);
                    else
                        curTable = curTable.Where(u => u.Amount < 1);
                    obj2.hasStock = null;
                }


                var exp = ExpressionFactory(obj2);
                if (exp != null)
                    curTable = curTable.Where(exp);

            }

            return curTable;
        }



        /// <summary>
        /// 批量导入
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public dynamic ImportConsumable(Stream fileStream, Users user)
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

                List<Consumable> list = new List<Consumable>();
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

        Consumable GetModel(DataRow dataRow, out List<string> tip)
        {
            tip = new List<string>();
            var _temp = dataRow.ItemArray;
            Consumable temp = new Consumable();
            if (string.IsNullOrEmpty(_temp[0]?.ToString().Trim()))
                tip.Add("序号不能为空");

            if (string.IsNullOrEmpty(_temp[1]?.ToString().Trim()))
                tip.Add("耗材类别不能为空");
            else
                temp.Type = _temp[1]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[2]?.ToString().Trim()))
                tip.Add("耗材型号不能为空");
            else
                temp.Model = _temp[2]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[3]?.ToString().Trim()))
                tip.Add("耗材名称不能为空");
            else
                temp.Name = _temp[3]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[4]?.ToString().Trim()))
                tip.Add("适用设备不能为空");
            else
                temp.Equipment = _temp[4]?.ToString().Trim();

            if (string.IsNullOrEmpty(_temp[5]?.ToString().Trim()))
                tip.Add("耗材品牌不能为空");
            else
                temp.Trademark = _temp[5]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[6]?.ToString().Trim()))
                tip.Add("单位不能为空");
            else
                temp.Unit = _temp[6]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[7]?.ToString().Trim()))
                tip.Add("是否低值不能为空");
            if (_temp[7].ToString() == "否")
            {
                temp.IsValue = "0";
            }
            if (_temp[7].ToString() == "是")
            {
                temp.IsValue = "1";
            }
            temp.Remark = _temp[8]?.ToString().Trim();
            temp.Amount = 0;//耗材当前拥有量
            temp.IsDelete = "0";//删除状态
            if (tip.Count > 0)
                return null;
            return temp;
        }



        /// <summary>
        /// 耗材导入
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public dynamic ImportConsumableInput(Stream fileStream, Users user)
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

                List<Consumable> list = new List<Consumable>();
                List<string> tip = new List<string>();
                foreach (DataRow item in dataTable.Rows)
                {
                    row++;
                    var model = GetModelInput(item, out tip);
                    if (model == null)
                    {
                        error += "第" + row + "行:" + string.Join(";", tip);
                        continue;
                    }
                    list.Add(model);
                    if (list.Count == 100) //足够100的
                    {
                        foreach (var item1 in list)
                        {
                            if (addConsumableInput(item1))
                            {
                                IsSuccess = true;
                                count++;
                            }
                        }
                        list.Clear();
                    }
                }
                if (list.Count > 0)//不足100的
                {
                    foreach (var item1 in list)
                    {
                        if (addConsumableInput(item1))
                        {
                            IsSuccess = true;
                            count++;
                        }
                    }
                    list.Clear();
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
        /// <summary>
        /// 增加耗材信息
        /// </summary>
        /// <param name="obj"></param>
        private bool addConsumableInput(Consumable obj)
        {
            bool isSuccess = false;
            Consumable temp = this.FindByFeldName(u => u.Type == obj.Type
              && u.Model == obj.Model
              && u.Equipment == obj.Equipment
              && u.Trademark == obj.Trademark
              && u.Unit == obj.Unit
              && u.IsValue == obj.IsValue);
            if (temp != null && temp.ID != 0)
            {
                temp.Amount += obj.Amount;
                isSuccess = this.Update(temp);
            }
            else
            {
                obj.IsDelete = "0";
                isSuccess = this.Insert(obj);
            }
            return isSuccess;
        }

        /// <summary>
        /// 耗材导入
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="tip"></param>
        /// <returns></returns>
        Consumable GetModelInput(DataRow dataRow, out List<string> tip)
        {
            tip = new List<string>();
            var _temp = dataRow.ItemArray;
            Consumable temp = new Consumable();

            if (string.IsNullOrEmpty(_temp[1]?.ToString().Trim()))
                tip.Add("耗材类别不能为空");
            else
                temp.Type = _temp[1]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[2]?.ToString().Trim()))
                tip.Add("耗材型号不能为空");
            else
                temp.Model = _temp[2]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[3]?.ToString().Trim()))
                tip.Add("耗材名称不能为空");
            else
                temp.Name = _temp[3]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[4]?.ToString().Trim()))
                tip.Add("适用设备不能为空");
            else
                temp.Equipment = _temp[4]?.ToString().Trim();

            if (string.IsNullOrEmpty(_temp[5]?.ToString().Trim()))
                tip.Add("耗材品牌不能为空");
            else
                temp.Trademark = _temp[5]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[6]?.ToString().Trim()))
                tip.Add("单位不能为空");
            else
                temp.Unit = _temp[6]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[7]?.ToString().Trim()))
                tip.Add("是否低值不能为空");
            if (_temp[7].ToString() == "否")
            {
                temp.IsValue = "0";
            }
            if (_temp[7].ToString() == "是")
            {
                temp.IsValue = "1";
            }

            if (string.IsNullOrEmpty(_temp[8]?.ToString().Trim()))
                tip.Add("数量不能为空");
            else
            {
                int count = 0;
                if (int.TryParse(_temp[8]?.ToString().Trim(), out count))
                {
                    temp.Amount = count;
                }
                else
                    tip.Add("数量不能转换为整形");
            }

            if (tip.Count > 0)
                return null;
            return temp;
        }
    }
}
