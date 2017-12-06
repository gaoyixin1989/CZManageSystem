using Aspose.Cells;
using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.CollaborationCenter.Invest;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.CollaborationCenter.Invest
{
    public class InvestAgoEstimateApplyDetailService : BaseService<InvestAgoEstimateApplyDetail>, IInvestAgoEstimateApplyDetailService
    {
        static Users _user;
        //导入功能
        public dynamic Import(Stream fileStream, Users user, Guid applyID)
        {
            try
            {

                bool IsSuccess = false;
                int count = 0;
                _user = user;
                string error = "";
                int row = 2;
                var dataTable = ExcelToDatatable(fileStream);
                if (dataTable.Rows.Count < 1)
                    return new { IsSuccess = false, Message = "读取到Excel记录为0！" };

                List<InvestAgoEstimateApplyDetail> list = new List<InvestAgoEstimateApplyDetail>();
                List<InvestAgoEstimateApplyDetail> Questions = new List<InvestAgoEstimateApplyDetail>();

                foreach (DataRow item in dataTable.Rows)
                {
                    row++;
                    List<string> tip = new List<string>();
                    var model = GetModel(item, out tip, applyID);
                    if (model == null)
                    {
                        error += (string.IsNullOrEmpty(error) ? "" : "<br/>") + "第" + row + "行:" + string.Join(";", tip);
                        continue;
                    }
                    list.Add(model);
                    if (list.Count >= 100) //足够100的
                    {
                        this.InsertByList(list);

                        Questions.AddRange(list);
                        count += list.Count;
                        list.Clear();
                        IsSuccess = true;

                    }
                }
                if (list.Count > 0)//不足100的
                {
                    this.InsertByList(list);

                    Questions.AddRange(list);
                    count += list.Count;
                    list.Clear();
                    IsSuccess = true;
                }
                int falCount = dataTable.Rows.Count - count;
                //result.Message = "导入成功";
                var result = new
                {
                    IsSuccess = IsSuccess,
                    Message = "成功导入" + count + "条，失败" + falCount + "条。其他提示：<br/>" + error
                    //, Questions = Questions
                };
                return result;
            }
            catch (Exception ex)
            {
                return new { IsSuccess = false, Message = "文件内容错误！" };
            }
        }

        //将excel文件的stream转化为DataTable
        private DataTable ExcelToDatatable(Stream fileStream)
        {
            try
            {
                Workbook book = new Workbook(fileStream);
                Worksheet sheet = book.Worksheets[0];
                Cells cells = sheet.Cells;
                DataTable dt = new DataTable("Workbook");
                return cells.ExportDataTableAsString(1, 0, cells.MaxDataRow, cells.MaxDataColumn + 1, true);

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private InvestAgoEstimateApplyDetail GetModel(DataRow dataRow, out List<string> tip, Guid applyID)
        {
            DateTime tempDateTime = new DateTime();
            Decimal tempDecimal = 0;
            string tempStr = "";
            List<string> tempListStr = new List<string>();
            tip = new List<string>();
            var _temp = dataRow.ItemArray;
            InvestAgoEstimateApplyDetail temp = new InvestAgoEstimateApplyDetail();

            temp.ID = Guid.NewGuid();
            temp.ApplyID = applyID;
            temp.Year = DateTime.Now.Year;
            temp.Month = DateTime.Now.Month;

            if (string.IsNullOrEmpty(_temp[0]?.ToString().Trim()))//项目名称
                tip.Add("项目名称不能为空");
            else
                temp.ProjectName = _temp[0]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[1]?.ToString().Trim()))//项目编号
                tip.Add("项目编号不能为空");
            else
                temp.ProjectID = _temp[1]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[2]?.ToString().Trim()))//合同名称
                tip.Add("合同名称不能为空");
            else
                temp.ContractName = _temp[2]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[3]?.ToString().Trim()))//合同编号
                tip.Add("合同编号不能为空");
            else
                temp.ContractID = _temp[3]?.ToString().Trim();
            if (!string.IsNullOrEmpty(_temp[4]?.ToString().Trim()))//供应商
                temp.Supply = _temp[4]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[5]?.ToString().Trim()))//合同总金额
                tip.Add("合同总金额不能为空");
            else
            {
                if (!Decimal.TryParse(_temp[5]?.ToString().Trim(), out tempDecimal))
                    tip.Add("合同总金额为小数类型");
                else
                    temp.SignTotal = tempDecimal;
            }
            if (string.IsNullOrEmpty(_temp[6]?.ToString().Trim()))//合同实际金额
                tip.Add("合同实际金额不能为空");
            else
            {
                if (!Decimal.TryParse(_temp[6]?.ToString().Trim(), out tempDecimal))
                    tip.Add("合同实际金额为小数类型");
                else
                    temp.PayTotal = tempDecimal;
            }
            if (!string.IsNullOrEmpty(_temp[7]?.ToString().Trim()))//所属专业
                temp.Supply = _temp[7]?.ToString().Trim();

            if (!string.IsNullOrEmpty(_temp[8]?.ToString().Trim()))//科目
                temp.Course = _temp[8]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[9]?.ToString().Trim().Replace("%", "")))//项目形象进度
                tip.Add("项目形象进度不能为空");
            else
            {
                if (!Decimal.TryParse(_temp[9]?.ToString().Trim().Replace("%", ""), out tempDecimal))
                    tip.Add("项目形象进度为百分数");
                else
                {
                    temp.Rate = tempDecimal;
                    if (_temp[9]?.ToString().Trim().IndexOf("%") < 0)
                        temp.Rate = temp.Rate * 100;
                }
            }
            if (string.IsNullOrEmpty(_temp[10]?.ToString().Trim()))//已付款金额
                tip.Add("已付款金额不能为空");
            else
            {
                if (!Decimal.TryParse(_temp[10]?.ToString().Trim(), out tempDecimal))
                    tip.Add("已付款金额为小数类型");
                else
                    temp.Pay = tempDecimal;
            }
            if (string.IsNullOrEmpty(_temp[11]?.ToString().Trim()))//暂估金额
                tip.Add("暂估金额不能为空");
            else
            {
                if (!Decimal.TryParse(_temp[11]?.ToString().Trim(), out tempDecimal))
                    tip.Add("暂估金额为小数类型");
                else
                    temp.NotPay = tempDecimal;
            }


            if (tip.Count > 0)
                return null;
            return temp;
        }
    }
}
