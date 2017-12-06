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

namespace CZManageSystem.Service.Administrative.VehicleManages
{
    public class CarFeeFixService : BaseService<CarFeeFix>, ICarFeeFixService
    {
        static Users _user;
        ISysDeptmentService _sysDeptmentService = new SysDeptmentService();
        ISysUserService _sysUserService = new SysUserService();
        ICarInfoService _carInfoService = new CarInfoService();
        static List<CarInfo> listDic = new List<CarInfo>();
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
                listDic = _carInfoService.List().ToList();
                var dataTable = ExcelToDatatable(fileStream);
                if (dataTable.Rows.Count < 1)
                    return new { IsSuccess = false, Message = "读取到Excel记录为0！" };

                List<CarFeeFix> list = new List<CarFeeFix>();
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

        CarFeeFix GetModel(DataRow dataRow, out List<string> tip)
        {
            tip = new List<string>();
            var _temp = dataRow.ItemArray;
            CarFeeFix temp = new CarFeeFix();

            temp.CarFeeFixId = Guid.NewGuid();
            temp.EditTime = DateTime.Now;
            if (string.IsNullOrEmpty(_temp[0]?.ToString().Trim()))
                tip.Add("所属单位不能为空");
            else
            //temp.CorpId = _temp[3]?.ToString().Trim();
            {
                var depts = _temp[0].ToString().Trim();
                List<DataDictionary> CorpList = GetDictListByDDName("所属单位");
                var deptsList = CorpList.Where(u => u.DDText == depts).FirstOrDefault();
                if (deptsList == null)
                    tip.Add("所属单位不存在");
                else
                    temp.CorpId = int.Parse(string.Join(",", deptsList.DDValue));
            }
            if (string.IsNullOrEmpty(_temp[1]?.ToString().Trim()))
                tip.Add("车牌号不能为空");
            else
            {
                var Corp = listDic.Where(f => f.LicensePlateNum == _temp[1]?.ToString().Trim()).FirstOrDefault();
                if (Corp == null)
                    tip.Add("所属单位不存在");
                else
                    temp.CarId = Corp.CarId;
            }
            if (string.IsNullOrEmpty(_temp[2]?.ToString().Trim()))
                tip.Add("缴费日期不能为空");
            else
                temp.PayTime = Convert.ToDateTime(_temp[2]?.ToString().Trim());
            if (string.IsNullOrEmpty(_temp[3]?.ToString().Trim()))
                tip.Add("保险费不能为空");
            else
                temp.FolicyFee = decimal.Parse(_temp[3]?.ToString().Trim());
            if (string.IsNullOrEmpty(_temp[4]?.ToString().Trim()))
                tip.Add("车船税不能为空");
            else
                temp.TaxFee = decimal.Parse(_temp[4]?.ToString().Trim());
            if (string.IsNullOrEmpty(_temp[5]?.ToString().Trim()))
                tip.Add("公里基金不能为空");
            else
                temp.RoadFee = decimal.Parse(_temp[5]?.ToString().Trim());
            if (string.IsNullOrEmpty(_temp[6]?.ToString().Trim()))
                tip.Add("其它费不能为空");
            else
                temp.OtherFee = decimal.Parse(_temp[6]?.ToString().Trim());
            if (string.IsNullOrEmpty(_temp[7]?.ToString().Trim()))
                tip.Add("小计不能为空");
            else
                temp.TotalFee = decimal.Parse(_temp[7]?.ToString().Trim());
            if (string.IsNullOrEmpty(_temp[8]?.ToString().Trim()))
                tip.Add("计费开始不能为空");
            else
                temp.StartTime = Convert.ToDateTime(_temp[8]?.ToString().Trim());
            if (string.IsNullOrEmpty(_temp[9]?.ToString().Trim()))
                tip.Add("计费结束不能为空");
            else
                temp.EndTime = Convert.ToDateTime(_temp[9]?.ToString().Trim());
            if (string.IsNullOrEmpty(_temp[10]?.ToString().Trim()))
                tip.Add("经手人不能为空");
            else
                temp.Person =_temp[10]?.ToString().Trim();
           
            temp.Remark = _temp[11]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[12]?.ToString().Trim()))
                tip.Add("编辑人不能为空");
            else
            //temp.EditorId = _temp[1]?.ToString().Trim();
            {
                var depts = _temp[12].ToString().Trim();
                var deptsList = _sysUserService.List().Where(w => depts.Contains(w.RealName)).Select(s => s.UserId);
                if (deptsList.Count() < 0)
                    tip.Add("管理人不存在");
                else
                    temp.EditorId = new Guid(string.Join(",", deptsList));
            }
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
