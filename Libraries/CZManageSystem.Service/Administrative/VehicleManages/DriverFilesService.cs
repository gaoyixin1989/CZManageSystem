using Aspose.Cells;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.Administrative.VehicleManages;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.Administrative
{
    public class DriverFilesService : BaseService<CarDriverInfo>, IDriverFilesService
    {
        static Users _user;
        ISysDeptmentService _sysDeptmentService = new SysDeptmentService();
        ISysUserService _sysUserService = new SysUserService();
        /// <summary>
        /// 批量导入
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public dynamic ImportDriverFiles(Stream fileStream, Users user)
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

                List<CarDriverInfo> list = new List<CarDriverInfo>();
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

        CarDriverInfo GetModel(DataRow dataRow, out List<string> tip)
        {
            tip = new List<string>();
            var _temp = dataRow.ItemArray;
            CarDriverInfo temp = new CarDriverInfo();
           
                temp.DriverId = Guid.NewGuid();
            if (string.IsNullOrEmpty(_temp[0]?.ToString().Trim()))
                tip.Add("编辑人不能为空");
            else
            //temp.EditorId = _temp[1]?.ToString().Trim();
            {
                var depts = _temp[0].ToString().Trim();
                var deptsList = _sysUserService.List().Where(w => depts.Contains(w.RealName)).Select(s => s.UserId);
                if (deptsList.Count() < 0)
                    tip.Add("管理人不存在");
                else
                    temp.EditorId =new Guid(string.Join(",", deptsList));
            }
                temp.EditTime = DateTime.Now;
            if (string.IsNullOrEmpty(_temp[1]?.ToString().Trim()))
                tip.Add("所属单位不能为空");
            else
            //temp.CorpId = _temp[3]?.ToString().Trim();
            {
                var depts = _temp[1].ToString().Trim();
                List<DataDictionary> CorpList = GetDictListByDDName("所属单位");
                var deptsList = CorpList.Where(u => u.DDText == depts).FirstOrDefault();
                if (deptsList==null)
                    tip.Add("所属单位不存在");
                else
                    temp.CorpId = int.Parse(string.Join(",", deptsList.DDValue));
            }
            if (string.IsNullOrEmpty(_temp[2]?.ToString().Trim()))
                tip.Add("司机编号不能为空");
            else
                temp.SN = _temp[2]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[3]?.ToString().Trim()))
                tip.Add("司机姓名不能为空");
            else
                temp.Name = _temp[3]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[4]?.ToString().Trim()))
                tip.Add("手机号不能为空");
            else
                temp.Mobile = _temp[4]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[5]?.ToString().Trim()))
                tip.Add("部门名称不能为空");
            else
                temp.DeptName = _temp[5]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[6]?.ToString().Trim()))
                tip.Add("开始驾驶时间不能为空");
            else
                temp.CarAge = Convert.ToDateTime(_temp[6]?.ToString().Trim());
            if (string.IsNullOrEmpty(_temp[7]?.ToString().Trim()))
                tip.Add("生日不能为空");
            else
                temp.Birthday = Convert.ToDateTime(_temp[7]?.ToString().Trim());
            //temp.Remark = _temp[8]?.ToString().Trim();
            if (tip.Count > 0)
                return null;
            return temp;
        }
        public List<DataDictionary> GetDictListByDDName(string DDName)
        {
            IDataDictionaryService _dataDictionaryService = new DataDictionaryService();
            var modelList = new List<DataDictionary>();
            int count = 0;
            if (!string.IsNullOrEmpty(DDName))
            {
                modelList = _dataDictionaryService.QueryDataByPage(out count, 0, int.MaxValue, DDName, null).ToList();
                modelList = modelList.Where(u => (u.EnableFlag ?? false)).ToList();
            }
            return modelList;
        }
    }
}
