using Aspose.Cells;
using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.CollaborationCenter.Invest;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 物资采购
/// </summary>
namespace CZManageSystem.Service.CollaborationCenter.Invest
{
    public class InvestMaterialsService : BaseService<InvestMaterials>, IInvestMaterialsService
    {

        /// 分页
        /// </summary>
        /// <param name="count">返回记录总数</param>
        /// <param name="objs">条件数组</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public override IEnumerable<dynamic> GetForPaging(out int count, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var source = objs == null ? this._entityStore.Table.OrderByDescending(c => c.OrderCreateTime) : this._entityStore.Table.OrderByDescending(c => c.OrderCreateTime).Where(ExpressionFactory(objs));
            PagedList<InvestMaterials> pageList = new PagedList<InvestMaterials>(source, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count);
            count = pageList.TotalCount;
            return pageList;
        }
        public dynamic Import(Stream fileStream)
        {
            try
            {
                bool IsSuccess = false;
                int count = 0;
                int row = 2;
                string error = "";
                var dataTable = ExcelToDatatable(fileStream);
                if (dataTable.Rows.Count < 1)
                    return new { IsSuccess = false, Message = "读取到Excel记录为0！" };

                List<InvestMaterials> list = new List<InvestMaterials>(); 
                foreach (DataRow item in dataTable.Rows)
                {
                    row++;
                    var model = GetModel(item);
                    var valid = new ViewModelValidator(model);
                   
                    if (!valid.IsValid())
                    {
                        error += "第" + row + "行:" + valid.ValidationErrorsToString +"；";
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
        InvestMaterials GetModel(DataRow dataRow)
        {
            var model = dataRow.ItemArray; 
            var _temp = dataRow.ItemArray;
            InvestMaterials temp = new InvestMaterials() { ID = Guid.NewGuid() }; 
            //项目编号 
            temp.ProjectID = _temp[0]?.ToString().Trim();

            //项目名称  
            temp.ProjectName = _temp[1]?.ToString().Trim();

            //订单编号 
            temp.OrderID = _temp[2]?.ToString().Trim();

            //订单说明 
            temp.OrderDesc = _temp[3]?.ToString().Trim();

            //订单录入公司 
            temp.OrderInCompany = _temp[4]?.ToString().Trim();

            //审核状态(批准) 
            temp.AuditStatus = _temp[5]?.ToString().Trim();

            //订单录入金额
            decimal dec = 0;
            if (decimal.TryParse(_temp[6]?.ToString().Trim(), out dec))

                temp.OrderInPay = dec;
            //订单接收公司 
            temp.OrderOutCompany = _temp[7]?.ToString().Trim();

            //订单接收金额
            dec = 0;
            if (decimal.TryParse(_temp[8]?.ToString().Trim(), out dec))
                temp.OrderOutSum = dec;

            //订单创建时间
            DateTime dt = DateTime.Now;
            if (DateTime.TryParse(_temp[9]?.ToString().Trim(), out dt))
                temp.OrderCreateTime = dt;

            //合同编号 
            temp.ContractID = _temp[10]?.ToString().Trim();

            //合同名称 
            temp.ContractName = _temp[11]?.ToString().Trim();

            //外围系统合同编号 
            temp.OutContractID = _temp[12]?.ToString().Trim();

            //订单标题 
            temp.OrderTitle = _temp[13]?.ToString().Trim();

            //订单备注 
            temp.OrderNote = _temp[14]?.ToString().Trim();

            //供应商 
            temp.Apply = _temp[15]?.ToString().Trim();

            //订单接收百分比 SUM
            dec = 0;
            if (decimal.TryParse(_temp[16]?.ToString().Trim(), out dec))
                temp.OrderOutRate = dec;

            //未接收设备（元）
            dec = 0;
            if (decimal.TryParse(_temp[17]?.ToString().Trim(), out dec))
                temp.OrderUnReceived = dec;

            return temp;
        }

    }
}
