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
/// 合同已付金额表
/// </summary>
namespace CZManageSystem.Service.CollaborationCenter.Invest
{
    public class InvestContractPayService : BaseService<InvestContractPay>, IInvestContractPayService
    {
        static Users _user;

        public IList<InvestContractPay> GetForPaging(out int count, InvestContractPayQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var curTable = GetQueryTable(objs);
            count = curTable.Count();
            var list = curTable.OrderBy(p => "").Skip(pageIndex * pageSize).Take(pageSize);
            return list.ToList();
        }

        /// <summary>
        /// 编辑查询条件
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        private IQueryable<InvestContractPay> GetQueryTable(InvestContractPayQueryBuilder obj = null)
        {
            var curTable = this._entityStore.Table;
            if (obj != null)
            {
                InvestContractPayQueryBuilder obj2 = (InvestContractPayQueryBuilder)CloneObject(obj);
                
                if (!string.IsNullOrEmpty(obj.ProjectName))
                {//项目名称
                    var projectIdList = new InvestProjectService().List().Where(u => u.ProjectName.Contains(obj.ProjectName)).Select(u => u.ProjectID).ToList();
                    curTable = curTable.Where(u => projectIdList.Contains(u.ProjectID));
                    obj2.ProjectName = null;
                }
                if (!string.IsNullOrEmpty(obj.ContractName))
                {//合同名称
                    var contractIdList = new InvestContractService().List().Where(u => u.ContractName.Contains(obj.ContractName)).Select(u => u.ContractID).ToList();
                    curTable = curTable.Where(u => contractIdList.Contains(u.ContractID));
                    obj2.ContractName = null;
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

                List<InvestContractPay> list = new List<InvestContractPay>();
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
                return cells.ExportDataTableAsString(0, 0, cells.MaxDataRow+1, cells.MaxDataColumn + 1, true);

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        InvestContractPay GetModel(DataRow dataRow, out List<string> tip)
        {
            tip = new List<string>();
            var _temp = dataRow.ItemArray;
            InvestContractPay temp = new InvestContractPay();
            if (string.IsNullOrEmpty(_temp[0]?.ToString().Trim()))
                tip.Add("批不能为空");
            else
                temp.Batch = _temp[0]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[1]?.ToString().Trim()))
                tip.Add("日记不能分账录不能为空");
            else
                temp.DateAccount = _temp[1]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[2]?.ToString().Trim()))
                tip.Add("行说明不能为空");
            else
            {
                temp.RowNote = _temp[2]?.ToString().Trim();
                var v = this.FindByFeldName(s => s.RowNote == temp.RowNote);
                if (v!=null)
                    tip.Add("行说明不能重复");
                //else
                //   temp.RowNote = _temp[2]?.ToString().Trim();
             }

            if (string.IsNullOrEmpty(_temp[3]?.ToString().Trim()))
                tip.Add("项目编号不能为空");
            else
                temp.ProjectID = _temp[3]?.ToString().Trim();

            if (string.IsNullOrEmpty(_temp[4]?.ToString().Trim()))
                tip.Add("合同编号不能为空");
            else
                temp.ContractID = _temp[4]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[5]?.ToString().Trim()))
                tip.Add("供应商不能为空");
            else
                temp.Supply = _temp[5]?.ToString().Trim();
            Decimal Pay = 0;
            if (string.IsNullOrEmpty(_temp[6]?.ToString().Trim()))
                tip.Add("已付款金额不能为空");
            else if (!Decimal.TryParse(_temp[6]?.ToString().Trim(), out Pay))
                tip.Add("已付款金额为小数类型");
            else
                temp.Pay = Pay;
            temp.Time = DateTime.Now;
            temp.UserID = _user.UserId;
            temp.ID = Guid.NewGuid();
            if (tip.Count > 0)
                return null;
            return temp;
        }
    }
}
