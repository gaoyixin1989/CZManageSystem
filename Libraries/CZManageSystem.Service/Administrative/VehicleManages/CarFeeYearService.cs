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
    public class CarFeeYearService : BaseService<CarFeeYear>, ICarFeeYearService
    {
        public Users _user { get; set; }
        IDataDictionaryService _dataDictionaryService = new DataDictionaryService();
        ICarInfoService carInfoService = new CarInfoService();
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

                List<CarFeeYear> list = new List<CarFeeYear>();
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
                return cells.ExportDataTableAsString(0, 0, cells.MaxDataRow, cells.MaxDataColumn + 1, true);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        CarFeeYear GetModel(DataRow dataRow)
        {
            var model = dataRow.ItemArray;
            var _temp = dataRow.ItemArray;
            CarFeeYear temp = new CarFeeYear() { CarFeeYearId = Guid.NewGuid() };
            //   所属单位
            var corpName = _temp[1]?.ToString().Trim();
            var corpModel = _dataDictionaryService.FindByFeldName(d => d.DDName == "所属单位" && d.DDText == corpName);
            if (corpModel != null)
                temp.CorpId = Convert.ToInt32(corpModel.DDValue);
            //  车辆ID 
            var licensePlateNum = _temp[2]?.ToString().Trim();
            temp.CarId = carInfoService.FindByFeldName(f => f.LicensePlateNum == licensePlateNum)?.CarId;
            //  使用单位
            temp.CorpName = _temp[3]?.ToString().Trim();
            //  缴费日期
            DateTime dt;
            if (DateTime.TryParse(_temp[4]?.ToString().Trim(), out dt))
                temp.PayTime = dt;
            decimal dec = 0;
            if (decimal.TryParse(_temp[5]?.ToString().Trim(), out dec))
                //  费用小计
                temp.TotalFee = dec;
            //经手人
            temp.Person = _temp[6]?.ToString().Trim();
            temp.EditorId = _user.UserId;
            //temp. = _user.RealName;
            temp.EditTime = DateTime.Now;
            return temp;
        }
    }
}
