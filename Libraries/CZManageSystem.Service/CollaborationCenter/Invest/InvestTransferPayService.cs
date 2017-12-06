using Aspose.Cells;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.CollaborationCenter.Invest;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 已转资合同表
/// </summary>
namespace CZManageSystem.Service.CollaborationCenter.Invest
{
    public class InvestTransferPayService : BaseService<InvestTransferPay>, IInvestTransferPayService
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

                List<InvestTransferPay> list = new List<InvestTransferPay>();
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
                    if (this.List().Where(u => u.ProjectID == model.ProjectID&&u.ContractID==model.ContractID).Count() > 0)
                    {
                        error += (string.IsNullOrEmpty(error) ? "" : "<br/>") + "第" + row + "行:项目编号和合同编号的组合已经被占用";
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
                return cells.ExportDataTableAsString(0, 0, cells.MaxDataRow + 1, cells.MaxDataColumn + 1, true);

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        InvestTransferPay GetModel(DataRow dataRow, out List<string> tip)
        {
            tip = new List<string>();
            var _temp = dataRow.ItemArray;
            InvestTransferPay temp = new InvestTransferPay();
            if (string.IsNullOrEmpty(_temp[0]?.ToString().Trim()))
                tip.Add("项目编号不能为空");
            else
                temp.ProjectID = _temp[0]?.ToString().Trim();
            //if (string.IsNullOrEmpty(_temp[1]?.ToString().Trim()))
            //    tip.Add("项目名称不能为空");
            //else
            //    temp.ProjectName = _temp[1]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[2]?.ToString().Trim()))
                tip.Add("合同编号不能为空");
            else
                temp.ContractID = _temp[2]?.ToString().Trim();

            //if (string.IsNullOrEmpty(_temp[3]?.ToString().Trim()))
            //    tip.Add("合同名称不能为空");
            //else
            //    temp.ContractName = _temp[3]?.ToString().Trim();

            if (string.IsNullOrEmpty(_temp[4]?.ToString().Trim()))
                tip.Add("是否已转资不能为空");
            else
                temp.IsTransfer = _temp[4]?.ToString().Trim();
            Decimal TransferPay = 0;
            if (string.IsNullOrEmpty(_temp[5]?.ToString().Trim()))
                tip.Add("已转资金额不能为空");
            else if (!Decimal.TryParse(_temp[5]?.ToString().Trim(), out TransferPay))
                tip.Add("已转资金额为小数类型");
            else
                temp.TransferPay = TransferPay;
            temp.EditTime = DateTime.Now;
            temp.EditorID = _user.UserId;
            temp.ID = Guid.NewGuid();

            if (!string.IsNullOrEmpty(temp.ProjectID) && !string.IsNullOrEmpty(temp.ContractID))
            {
                if (!CheckRepeat(temp.ID, temp.ProjectID, temp.ContractID))
                {
                    tip.Add("该项目编号和合同编号的组合已经存在");
                }
            }

            if (tip.Count > 0)
                return null;
            return temp;
        }

        /// <summary>
        /// 检查是否重复
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="ProjectID"></param>
        /// <param name="ContractID"></param>
        /// <returns></returns>
        public bool CheckRepeat(Guid ID, string ProjectID, string ContractID)
        {
            var dd = this.List().Where(u => u.ID != ID && u.ProjectID == ProjectID && u.ContractID == ContractID).ToList();
            if (dd.Count > 0)
                return false;
            else
                return true;
        }

    }
}
