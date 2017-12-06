using Aspose.Cells;
using CZManageSystem.Core;
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
    public class CarInfoService : BaseService<CarInfo>, ICarInfoService
    {
        ISysDeptmentService _sysDeptmentService = new SysDeptmentService();
        IDriverFilesService _driverFilesService = new DriverFilesService();
        static Users _user;
        static List<DataDictionary> listDic = new List<DataDictionary>();
        IDataDictionaryService _dataDictionaryService = new DataDictionaryService();
        /// <summary>
        /// 批量导入
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public dynamic ImportCarInfo(Stream fileStream, Users user)
        {
            try
            {
                bool IsSuccess = false;
                int count = 0;
                int row = 1;
                string error = "";
                _user = user;
                string DDNames = "所属单位";
                listDic = _dataDictionaryService.List().Where(w => DDNames.Contains(w.DDName)).ToList() as List<DataDictionary>;
                var dataTable = ExcelToDatatable(fileStream);
                if (dataTable.Rows.Count < 1)
                    return new { IsSuccess = false, Message = "读取到Excel记录为0！" };

                List<CarInfo> list = new List<CarInfo>();
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
                return cells.ExportDataTableAsString(0, 0, cells.MaxDataRow + 1, cells.MaxDataColumn + 1,true );

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        CarInfo GetModel(DataRow dataRow, out List<string> tip)
        {
            tip = new List<string>();
            var _temp = dataRow.ItemArray;
            CarInfo temp = new CarInfo();
            if (string.IsNullOrEmpty(_temp[0]?.ToString().Trim()))
                tip.Add("所属单位不能为空");
            else
            { 
                var Corp = listDic.Find(f => f.DDText == _temp[0]?.ToString().Trim());
                if (Corp == null)
                    tip.Add("所属单位不存在");
                else
                    temp.CorpId =int.Parse(Corp.DDValue);
            }
            if (string.IsNullOrEmpty(_temp[1]?.ToString().Trim()))
                tip.Add("车辆编号不能为空");
            else
                temp.SN = _temp[1]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[2]?.ToString().Trim()))
                tip.Add("车牌号不能为空");
            else
                temp.LicensePlateNum = _temp[2]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[3]?.ToString().Trim()))
                tip.Add("车辆品牌不能为空");
            else
                temp.CarBrand = _temp[3]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[4]?.ToString().Trim()))
                tip.Add("车辆型号不能为空");
            else
                temp.CarModel = _temp[4]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[5]?.ToString().Trim()))
                tip.Add("车辆类型不能为空");
            else
                temp.CarType = _temp[5]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[6]?.ToString().Trim()))
                tip.Add("吨位/人数不能为空");
            else
                temp.CarTonnage = _temp[6]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[7]?.ToString().Trim()))
                tip.Add("管理部门不能为空");
            else
                temp.DeptName = _temp[7]?.ToString().Trim();
            DateTime dt = DateTime.Now;
            if (string.IsNullOrEmpty(_temp[8]?.ToString().Trim()))
                tip.Add("购买日期不能为空");
            else if (!DateTime.TryParse(_temp[8]?.ToString().Trim(), out dt))
            {
                tip.Add("购买日期为时间类型");
            }
            else
                temp.BuyDate = dt;
            if (string.IsNullOrEmpty(_temp[9]?.ToString().Trim()))
                tip.Add("购买价不能为空");
            else
                temp.CarPrice = _temp[9]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[10]?.ToString().Trim()))
                tip.Add("折旧年限不能为空");
            else
                temp.CarLimit = _temp[10]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[11]?.ToString().Trim()))
                tip.Add("每月折旧不能为空");
            else
                temp.Depre = _temp[11]?.ToString().Trim();

            //DateTime RentTime1 = DateTime.Now;
            //if (string.IsNullOrEmpty(_temp[12]?.ToString().Trim()))
            //    tip.Add("租赁开始时间不能为空");
            //else if (!DateTime.TryParse(_temp[12]?.ToString().Trim(), out RentTime1))
            //{
            //    tip.Add("租赁开始时间为时间类型");
            //}
            //else
            //    temp.RentTime1 = RentTime1;
            //DateTime RentTime2 = DateTime.Now;
            //if (string.IsNullOrEmpty(_temp[13]?.ToString().Trim()))
            //    tip.Add("租赁结束时间不能为空");
            //else if (!DateTime.TryParse(_temp[13]?.ToString().Trim(), out RentTime2))
            //{
            //    tip.Add("租赁结束时间时间类型");
            //}
            //else
            //    temp.RentTime2 = RentTime2;
            //DateTime PolicyTime1 = DateTime.Now;
            //if (string.IsNullOrEmpty(_temp[14]?.ToString().Trim()))
            //    tip.Add("保险开始时间不能为空");
            //else if (!DateTime.TryParse(_temp[14]?.ToString().Trim(), out PolicyTime1))
            //{
            //    tip.Add("保险开始时间为时间类型");
            //}
            //else
            //    temp.PolicyTime1 = PolicyTime1;
            //DateTime PolicyTime2 = DateTime.Now;
            //if (string.IsNullOrEmpty(_temp[15]?.ToString().Trim()))
            //    tip.Add("保险结束时间不能为空");
            //else if (!DateTime.TryParse(_temp[15]?.ToString().Trim(), out PolicyTime2))
            //{
            //    tip.Add("保险结束时间为时间类型");
            //}
            //else
            //    temp.PolicyTime2 = PolicyTime2;
            //DateTime AnnualTime1 = DateTime.Now;
            //if (string.IsNullOrEmpty(_temp[16]?.ToString().Trim()))
            //    tip.Add("年审开始时间不能为空");
            //else if (!DateTime.TryParse(_temp[16]?.ToString().Trim(), out AnnualTime1))
            //{
            //    tip.Add("年审开始时间为时间类型");
            //}
            //else
            //    temp.AnnualTime1 = AnnualTime1;
            //DateTime AnnualTime2 = DateTime.Now;
            //if (string.IsNullOrEmpty(_temp[17]?.ToString().Trim()))
            //    tip.Add("年审结束时间不能为空");
            //else if (!DateTime.TryParse(_temp[17]?.ToString().Trim(), out AnnualTime2))
            //{
            //    tip.Add("年审结束时间为时间类型");
            //}
            //else
            //    temp.AnnualTime2 = AnnualTime2;

            if (string.IsNullOrEmpty(_temp[18]?.ToString().Trim()))
                tip.Add("驾驶员不能为空");
            else
            {
                string name = _temp[18].ToString().Trim();
                var drive = _driverFilesService.FindByFeldName(d=>d.Name== name);
                if (drive == null)
                    tip.Add("该驾驶员不存在");
                else
                    temp.DriverId = drive.DriverId;
            }
           
            if (string.IsNullOrEmpty(_temp[19]?.ToString().Trim()))
                tip.Add("状态不能为空");
            else
            {
                int sta=0;
                if (_temp[19]?.ToString().Trim()== "空闲")
                {
                    sta = 0;
                }
                else if (_temp[19]?.ToString().Trim() == "出车中")
                {
                    sta = 1;
                }
                else if (_temp[19]?.ToString().Trim() == "送修")
                {
                    sta = 2;
                }
                else if (_temp[19]?.ToString().Trim() == "保养")
                {
                    sta = 3;
                }
                else if (_temp[19]?.ToString().Trim() == "停用")
                {
                    sta = 4;
                }
                else
                {
                    tip.Add("状态为空闲,出车中,送修,包养,停用。");
                }
                temp.Status = sta;
            }
            if (tip.Count > 0)
                return null;
            temp.EditTime = DateTime.Now;
            temp.EditorId = _user.UserId;
            temp.CarId = Guid.NewGuid();
            return temp;
        }
    }
}
