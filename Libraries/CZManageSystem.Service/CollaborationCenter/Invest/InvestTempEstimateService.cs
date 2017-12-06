using Aspose.Cells;
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
    public class InvestTempEstimateService : BaseService<InvestTempEstimate>, IInvestTempEstimateService
    {
        static Users _user;
        IInvestContractPayService _investContractPayService = new InvestContractPayService();
        IInvestContractService _investContractService = new InvestContractService();

        public IList<InvestTempEstimate> GetForPaging(out int count, StopInvestTempEstimateQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
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
        private IQueryable<InvestTempEstimate> GetQueryTable(StopInvestTempEstimateQueryBuilder obj = null)
        {
            var curTable = this._entityStore.Table;
            if (obj != null)
            {
                StopInvestTempEstimateQueryBuilder obj2 = (StopInvestTempEstimateQueryBuilder)CloneObject(obj);

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
                if (!string.IsNullOrEmpty(obj.EstimateUserName))
                {//负责人
                    var ManagerIDList = new Service.SysManger.SysUserService().List().Where(u => u.RealName.Contains(obj.EstimateUserName)).Select(u => u.UserId).ToList();
                    curTable = curTable.Where(u => ManagerIDList.Contains(u.ManagerID.Value));
                    obj2.EstimateUserName = null;
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
                int row = 1;
                string error = "";
                _user = user;
                var dataTable = ExcelToDatatable(fileStream);
                if (dataTable.Rows.Count < 1)
                    return new { IsSuccess = false, Message = "读取到Excel记录为0！" };

                List<InvestTempEstimate> list = new List<InvestTempEstimate>();
                List<string> tip = new List<string>();
                string tipp = "";
                List<InvestContract> contr = new List<InvestContract>();
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
                   
                   var con= GetModelContract(model.ProjectID, model.ContractID,model.PayTotal, out tipp);
                    if (con == null)
                    {
                        error += "第" + row + "行:" + string.Join(";", tipp);
                        continue;
                    }
                    contr.Add(con);
                    if (list.Count == 100) //足够100的
                    {
                        if (this.UpdateByList(list))
                        {
                            count += list.Count;
                            list.Clear();
                            _investContractService.UpdateByList(contr);
                            contr.Clear();
                            IsSuccess = true;
                        };
                    }
                }
                if (list.Count > 0)//不足100的
                {
                    if (this.UpdateByList(list))
                    {
                        count += list.Count;
                        list.Clear();
                        _investContractService.UpdateByList(contr);
                        contr.Clear();
                        IsSuccess = true;
                    };
                }
                int falCount = dataTable.Rows.Count - count;
                var result = new
                {
                    IsSuccess = IsSuccess,
                    Message = "成功修改" + count + "条暂估数据，失败" + falCount + "条。其他提示：" + error
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

        InvestTempEstimate GetModel(DataRow dataRow, out List<string> tip)
        {
            tip = new List<string>();
            var _temp = dataRow.ItemArray;
            InvestTempEstimate temp = new InvestTempEstimate();
            if (string.IsNullOrEmpty(_temp[0]?.ToString().Trim()))
                tip.Add("项目编号不能为空");
            else
                temp.ProjectID = _temp[0]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[1]?.ToString().Trim()))
                tip.Add("合同编号不能为空");
            else
                temp.ContractID = _temp[1]?.ToString().Trim();
          
            Decimal PayTotal = 0;
            if (string.IsNullOrEmpty(_temp[2]?.ToString().Trim()))
                tip.Add("实际合同金额不能为空");
            else if (!Decimal.TryParse(_temp[2]?.ToString().Trim(), out PayTotal))
                tip.Add("实际合同款金额为小数类型");
            else
                temp.PayTotal = PayTotal;

            Decimal Rate = 0;
            if (string.IsNullOrEmpty(_temp[3]?.ToString().Trim()))
                tip.Add("项目形象进度不能为空");
            else if (!Decimal.TryParse(_temp[3]?.ToString().Trim(), out Rate))
                tip.Add("项目形象进度为小数类型");
            else
                temp.Rate = Rate;

          //var Pay=  _investContractPayService.FindByFeldName(s => s.ProjectID == temp.ProjectID && s.ContractID == temp.ContractID)?.Pay;
          //  if (Pay!=null)
          //      temp.NotPay= PayTotal * Rate / 100 - Pay;

            var list = this.List().Where(s => s.ProjectID == temp.ProjectID && s.ContractID == temp.ContractID).Select(s=>new{
                s.ID,
                s.BackRate,
                s.Course,
                s.IsLock,
                s.ManagerID ,
                s.Pay,
                s.NotPay ,
                s.SignTotal,
                s.Status ,
                s.StatusTime,
                s.Study ,
                s.Supply 
        }).ToList();
            if (list.Count==0)
                tip.Add("该数据不存在");
            else
            {
                temp.ID = list[0].ID;
                temp.Supply = list[0].Supply;
                temp.SignTotal = list[0].SignTotal;
                temp.Study = list[0].Study;
                temp.ManagerID = list[0].ManagerID;
                temp.Course = list[0].Course;
                temp.BackRate = list[0].BackRate;
                temp.Pay = list[0].Pay;
                temp.IsLock = list[0].IsLock;
               // temp.EstimateUserID = list[0].EstimateUserID;
                temp.Status = list[0].Status;
                temp.StatusTime = list[0].StatusTime;

                if(!string.IsNullOrEmpty(list[0].Pay.ToString()))
                temp.NotPay = PayTotal * Rate / 100 - list[0].Pay;


            }
               


            if (tip.Count > 0)
                return null;
            return temp;
        }

        InvestContract GetModelContract(string ProjectID,string ContractID,decimal ?PayTotal,out string tip)
        {
            tip = "";
            var contrlist = _investContractService.List().Where(s => s.ProjectID == ProjectID && s.ContractID == ContractID).ToList();
            if (contrlist.Count >= 1)
                contrlist[0].PayTotal = PayTotal;
            else
                tip = "该数据不存在合同信息中,无法同步合同信息";
            if (!string.IsNullOrEmpty(tip))
                return null;
            return contrlist[0];

        }
    }
}
