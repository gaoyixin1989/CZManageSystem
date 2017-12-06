using Aspose.Cells;
using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.HumanResources.Employees;
using CZManageSystem.Service.SysManger;
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
    public class HRLzUserInfoService : BaseService<HRLzUserInfo>, IHRLzUserInfoService
    {

        ISysUserService sysUserService = new SysUserService();
        static string _userName = "";
        bool isInsert = false;
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
                var source = objs == null ? this._entityStore.Table.OrderBy(c => c.EmployeeId) : this._entityStore.Table.OrderBy(c => c.EmployeeId).Where(ExpressionFactory(objs));
                PagedList<HRLzUserInfo> pageList = new PagedList<HRLzUserInfo>();
                var pageListResult = pageList.QueryPagedList(source.Where(u => u.Users != null), pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count);
                count = pageList.TotalCount;
                return pageListResult.Select(s => new
                {
                    s.EmployeeId,
                    s.Gears,
                    s.LastModFier,
                    s.LastModTime,
                    s.PositionRank,
                    s.Remark,
                    s.SetIntoTheRanks,
                    s.Tantile,
                    s.UserId,
                    EmployerName = s.Users.RealName
                }).ToList();

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public dynamic Import(Stream fileStream, string userName)
        {
            try
            {
                bool IsSuccess = false;
                int countUpdate = 0, countInsert = 0;
                int row = 2;
                string error = "";
                var dataTable = ExcelToDatatable(fileStream);
                if (dataTable.Rows.Count < 1)
                    return new { IsSuccess = false, Message = "读取到Excel记录为0！" };
                List<HRLzUserInfo> listUpdate = new List<HRLzUserInfo>();
                List<HRLzUserInfo> listInsert = new List<HRLzUserInfo>();
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
                    if (isInsert)
                        listInsert.Add(model);
                    else
                        listUpdate.Add(model);
                    isInsert = false;
                    if (listUpdate.Count == 100) //足够100的
                    {
                        if (this.UpdateByList(listUpdate))
                        {
                            countUpdate += listUpdate.Count;
                            listUpdate.Clear();
                            IsSuccess = true;
                        };
                    }
                    if (listInsert.Count == 100) //足够100的
                    {
                        if (this.InsertByList(listInsert))
                        {
                            countInsert += listInsert.Count;
                            listInsert.Clear();
                            IsSuccess = true;
                        };
                    }
                }
                if (listUpdate.Count > 0)//不足100的
                {
                    if (this.UpdateByList(listUpdate))
                    {
                        countUpdate += listUpdate.Count;
                        listUpdate.Clear();
                        IsSuccess = true;
                    };
                }
                if (listInsert.Count > 0)//不足100的
                {
                    if (this.InsertByList(listInsert))
                    {
                        countInsert += listInsert.Count;
                        listInsert.Clear();
                        IsSuccess = true;
                    };
                }
                int falCount = dataTable.Rows.Count - countUpdate - countInsert;
                var result = new
                {
                    IsSuccess = IsSuccess,
                    Message = "成功导入" + countInsert + "条，成功更新" + countUpdate + "条，失败" + falCount + "条。其他提示：" + error
                };
                return result;
            }
            catch (Exception ex)
            {
                var result = new { IsSuccess = false, Message = "文件内容错误！" + ex.Message };
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
        HRLzUserInfo GetModel(DataRow dataRow, out List<string> tip)
        {
            var model = dataRow.ItemArray;
            tip = new List<string>();
            var _temp = dataRow.ItemArray; 
            if (string.IsNullOrEmpty(_temp[0]?.ToString().Trim()))
            {
                tip.Add("员工编号不能为空");
                return null;
            }
            string  EmployeeId = _temp[0]?.ToString().Trim();
           var temp = new HRLzUserInfoService().FindById(EmployeeId);//查找劳资员工表中是否存在
            if (temp == null)
            {
                var user = sysUserService.FindByFeldName(f => f.EmployeeId == EmployeeId);//查找用户表中是否存在
                if (user == null)
                {
                    tip.Add("用户表中不存在该员工编号");
                    return null;
                }
                temp = new HRLzUserInfo();
                isInsert = true;
            } 
            if (string.IsNullOrEmpty(_temp[1]?.ToString().Trim()))
                tip.Add("职位职级不能为空");

            else
                temp.PositionRank = _temp[1]?.ToString().Trim();

            if (string.IsNullOrEmpty(_temp[2]?.ToString().Trim()))
                tip.Add("套入职级不能为空");
            else
                temp.SetIntoTheRanks = _temp[2]?.ToString().Trim();
            int intTantile = 0;
            if (string.IsNullOrEmpty(_temp[3]?.ToString().Trim()))
                tip.Add("分位值不能为空");
            else if (!int.TryParse(_temp[3]?.ToString().Trim(), out intTantile))
                tip.Add("分位值必须为数值");
            else
                temp.Tantile = intTantile;


            if (string.IsNullOrEmpty(_temp[4]?.ToString().Trim()))
                tip.Add("档位不能为空");
            else
                temp.Gears = _temp[4]?.ToString().Trim();


            temp.SetIntoTheRanks = _temp[5]?.ToString().Trim();
            temp.LastModTime = DateTime.Now;
            temp.LastModFier = _userName;
            return temp;
        }

    }
}
