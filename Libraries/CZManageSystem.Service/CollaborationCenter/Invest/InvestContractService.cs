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

/// <summary>
/// 合同信息
/// </summary>
namespace CZManageSystem.Service.CollaborationCenter.Invest
{
    public class InvestContractService : BaseService<InvestContract>, IInvestContractService
    {
        static Users _user;
        public IList<InvestContract> GetForPaging(out int count, InvestContractQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
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
        private IQueryable<InvestContract> GetQueryTable(InvestContractQueryBuilder obj = null)
        {
            var curTable = this._entityStore.Table;
            if (obj != null)
            {
                InvestContractQueryBuilder obj2 = (InvestContractQueryBuilder)CloneObject(obj);
                if (!string.IsNullOrEmpty(obj.DpCode_Text))
                {//主办部门
                    var DpCodeList = new Service.SysManger.SysDeptmentService().List().Where(u => u.DpName.Contains(obj.DpCode_Text)).Select(u => u.DpId).ToList();
                    curTable = curTable.Where(u => DpCodeList.Contains(u.DpCode));
                    obj2.DpCode_Text = string.Empty;
                }
                if (!string.IsNullOrEmpty(obj.User_Text))
                {//主办人
                    var UserIDList = new Service.SysManger.SysUserService().List().Where(u => u.RealName.Contains(obj.User_Text)).Select(u => u.UserId).ToList();
                    curTable = curTable.Where(u => UserIDList.Contains(u.UserID.Value));
                    obj2.User_Text = string.Empty;
                }
                if (!string.IsNullOrEmpty(obj.ProjectName))
                {//项目名称
                    var projectIdList = new InvestProjectService().List().Where(u => u.ProjectName.Contains(obj.ProjectName)).Select(u => u.ProjectID).ToList();
                    curTable = curTable.Where(u => projectIdList.Contains(u.ProjectID));
                    obj2.ProjectName = null;
                }

                var exp = ExpressionFactory(obj2);
                if (exp != null)
                    curTable = curTable.Where(exp);

            }

            return curTable;
        }

        //导入功能
        public dynamic Import(Stream fileStream, Users user)
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

                List<InvestContract> list = new List<InvestContract>();
                List<InvestContract> Questions = new List<InvestContract>();
                List <InvestTempEstimate> templist = new List<InvestTempEstimate>();

                foreach (DataRow item in dataTable.Rows)
                {
                    row++;
                    List<string> tip = new List<string>();
                    var model = GetModel(item, out tip);
                    if (model == null)
                    {
                        error += (string.IsNullOrEmpty(error) ? "" : "<br/>") + "第" + row + "行:" + string.Join(";", tip);
                        continue;
                    }
                    if (list.Where(u => u.ContractID == model.ContractID && u.ProjectID == model.ProjectID).Count() > 0)
                    {
                        error += (string.IsNullOrEmpty(error) ? "" : "<br/>") + "第" + row + "行:合同编号+项目编号已经存在，不可重复";
                        continue;
                    }
                    list.Add(model);
                    InvestTempEstimate temp= GetTemp(model);
                    templist.Add(temp);
                    if (list.Count == 100) //足够100的
                    {
                        if (this.InsertByList(list) &&new InvestTempEstimateService().InsertByList(templist))
                        {
                            Questions.AddRange(list);
                            count += list.Count;
                            list.Clear();
                            IsSuccess = true;
                        };
                    }
                }
                if (list.Count > 0)//不足100的
                {
                    if (this.InsertByList(list) && new InvestTempEstimateService().InsertByList(templist))
                    {
                        Questions.AddRange(list);
                        count += list.Count;
                        list.Clear();
                        IsSuccess = true;
                    };
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

        private InvestTempEstimate GetTemp(InvestContract dataObj)
            {
            InvestTempEstimate temp = new InvestTempEstimate();
            temp.ID = Guid.NewGuid();
            temp.ContractID = dataObj.ContractID;
            temp.ProjectID = dataObj.ProjectID;
            temp.SignTotal = dataObj.SignTotal;
            temp.Supply = dataObj.Supply;
            temp.PayTotal = dataObj.PayTotal;
            var es = new EfRepository<InvestEstimate>().Table.Where(s => s.Year == DateTime.Today.Year && s.Month == DateTime.Today.Month && s.ContractID == dataObj.ContractID && s.ProjectID == dataObj.ProjectID).FirstOrDefault();
            temp.BackRate = es == null ? 0 : es.BackRate;
            var cc = new EfRepository<InvestContractPay>().Table.Where(s => s.ContractID == dataObj.ContractID && s.ProjectID == dataObj.ProjectID).FirstOrDefault();
            temp.Pay = cc == null ? 0 : cc.Pay;
            temp.NotPay = 0;
            temp.Rate = 0;

            return temp;
        }
        //合同主办人修改导入
        public dynamic Import_ModifyUser(Stream fileStream, Users user)
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

                List<InvestContract> list = new List<InvestContract>();
                List<InvestContract> Questions = new List<InvestContract>();

                foreach (DataRow item in dataTable.Rows)
                {
                    row++;
                    var _temp = item.ItemArray;
                    List<string> tip = new List<string>();

                    string strProjectID = "";//项目编号
                    string strContractID = "";//合同编号
                    Guid? guidUserID = null;//主办人ID

                    #region 获取数据并赋值
                    if (string.IsNullOrEmpty(_temp[0]?.ToString().Trim()))
                        tip.Add("合同编号不能为空");
                    else
                        strContractID = _temp[0]?.ToString().Trim();
                    if (string.IsNullOrEmpty(_temp[2]?.ToString().Trim()))
                        tip.Add("项目编号不能为空");
                    else
                        strProjectID = _temp[2]?.ToString().Trim();
                    if (string.IsNullOrEmpty(_temp[1].ToString().Trim()))
                        tip.Add("主办人不能为空");
                    else
                    {
                        List<string> tempListStr = _temp[1].ToString().Trim().Split(new char[] { ',', '，' }).ToList();
                        var usersList = new SysUserService().List().Where(u => tempListStr.Contains(u.RealName)).Select(u => u.UserId).ToList();
                        if (usersList.Count <= 0)
                            tip.Add("主办人不存在");
                        else
                            guidUserID = usersList[0];
                    }
                    #endregion

                    if (tip.Count == 0)
                    {
                        var oldDatas = this.List().Where(u => u.ContractID == strContractID && u.ProjectID == strProjectID).ToList();
                        if (oldDatas.Count > 0)
                        {
                            foreach (var item2 in oldDatas)
                                item2.UserID = guidUserID;
                            if (this.UpdateByList(oldDatas))
                                count++;
                            else
                                tip.Add("更新数据失败");
                        }
                        else
                        {
                            tip.Add("没有对应相同项目编号和合同编号的数据");
                        }
                    }
                    if (tip.Count > 0)
                    {
                        error += (string.IsNullOrEmpty(error) ? "" : "<br/>") + "第" + row + "行:" + string.Join(";", tip);
                        continue;
                    }

                }

                int falCount = dataTable.Rows.Count - count;
                //result.Message = "导入成功";
                var result = new
                {
                    IsSuccess = count > 0,
                    Message = "成功导入" + count + "条，失败" + falCount + "条。其他提示：<br/>" + error
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

        private InvestContract GetModel(DataRow dataRow, out List<string> tip)
        {
            DateTime tempDateTime = new DateTime();
            Decimal tempDecimal = 0;
            string tempStr = "";
            List<string> tempListStr = new List<string>();
            tip = new List<string>();
            var _temp = dataRow.ItemArray;
            InvestContract temp = new InvestContract();
            if (!string.IsNullOrEmpty(_temp[0]?.ToString().Trim()))//合同流水号
                temp.ContractSeries = _temp[0]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[1]?.ToString().Trim()))
                tip.Add("合同编号不能为空");
            else
                temp.ContractID = _temp[1]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[2]?.ToString().Trim()))
                tip.Add("合同名称不能为空");
            else
                temp.ContractName = _temp[2]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[3]?.ToString().Trim()))
                tip.Add("主办部门不能为空");
            else
            {
                tempStr = _temp[3].ToString().Trim();
                var deptsList = new SysDeptmentService().List().Where(w => w.DpName == tempStr).Select(s => s.DpId).ToList();
                if (deptsList.Count <= 0)
                    tip.Add("主办部门不存在");
                else
                    temp.DpCode = deptsList[0];
            }
            if (string.IsNullOrEmpty(_temp[4].ToString().Trim()))
                tip.Add("主办人不能为空");
            else
            {
                tempListStr = _temp[4].ToString().Trim().Split(new char[] { ',', '，' }).ToList();
                var usersList = new SysUserService().List().Where(u => tempListStr.Contains(u.RealName)).Select(u => u.UserId).ToList();
                if (usersList.Count <= 0)
                    tip.Add("主办人不存在");
                else
                    temp.UserID = usersList[0];
            }
            if (!string.IsNullOrEmpty(_temp[5]?.ToString().Trim()))//实际合同金额
            {
                if (!Decimal.TryParse(_temp[5]?.ToString().Trim(), out tempDecimal))
                    tip.Add("实际合同金额为小数类型");
                else
                    temp.PayTotal = tempDecimal;
            }
            if (!string.IsNullOrEmpty(_temp[6]?.ToString().Trim()))//币种
                temp.Currency = _temp[6]?.ToString().Trim();
            if (!string.IsNullOrEmpty(_temp[7]?.ToString().Trim()))//合同状态
                temp.ContractState = _temp[7]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[8]?.ToString().Trim()))//合同属性
                temp.Attribute = _temp[8]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[9]?.ToString().Trim()))
                tip.Add("项目编号不能为空");
            else
                temp.ProjectID = _temp[9]?.ToString().Trim();
            //if (string.IsNullOrEmpty(_temp[10]?.ToString().Trim()))
            //    tip.Add("项目名称不能为空");
            //else
            //    temp.ProjectName = _temp[10]?.ToString().Trim();
            if (!string.IsNullOrEmpty(_temp[11]?.ToString().Trim()))//项目金额
            {
                if (!Decimal.TryParse(_temp[11]?.ToString().Trim(), out tempDecimal))
                    tip.Add("项目金额为小数类型");
                else
                    temp.ProjectTotal = tempDecimal;
            }
            if (!string.IsNullOrEmpty(_temp[12]?.ToString().Trim()))//审批开始时间
            {
                if (DateTime.TryParse(_temp[12]?.ToString().Trim(), out tempDateTime))
                    temp.ApproveStartTime = tempDateTime;
                else
                    tip.Add("审批开始时间格式不对");
            }
            if (!string.IsNullOrEmpty(_temp[13]?.ToString().Trim()))//审批结束时间
            {
                if (DateTime.TryParse(_temp[13]?.ToString().Trim(), out tempDateTime))
                    temp.ApproveEndTime = tempDateTime;
                else
                    tip.Add("审批结束时间格式不对");
            }
            if (!string.IsNullOrEmpty(_temp[14]?.ToString().Trim()))//合同档案号
                temp.ContractFilesNum = _temp[14]?.ToString().Trim();
            if (!string.IsNullOrEmpty(_temp[15]?.ToString().Trim()))//印花税率
                temp.StampTaxrate = _temp[15]?.ToString().Trim();
            if (!string.IsNullOrEmpty(_temp[16]?.ToString().Trim()))//印花税金
                temp.Stamptax = _temp[16]?.ToString().Trim();
            if (!string.IsNullOrEmpty(_temp[17]?.ToString().Trim()))//合同对方
                temp.ContractOpposition = _temp[17]?.ToString().Trim();
            if (!string.IsNullOrEmpty(_temp[18]?.ToString().Trim()))//需求部门
            {
                tempStr = _temp[18].ToString().Trim();
                var deptsList = new SysDeptmentService().List().Where(w => w.DpName == tempStr).Select(s => s.DpId).ToList();
                if (deptsList.Count <= 0)
                    tip.Add("需求部门不存在");
                else
                    temp.RequestDp = string.Join(",", deptsList);
            }
            if (!string.IsNullOrEmpty(_temp[19]?.ToString().Trim()))//相关部门
            {
                tempStr = _temp[19].ToString().Trim();
                var deptsList = new SysDeptmentService().List().Where(w => w.DpName == tempStr).Select(s => s.DpId).ToList();
                if (deptsList.Count <= 0)
                    tip.Add("相关部门不存在");
                else
                    temp.RelevantDp = string.Join(",", deptsList);
            }
            if (!string.IsNullOrEmpty(_temp[20]?.ToString().Trim()))//项目开展原因
                temp.ProjectCause = _temp[20]?.ToString().Trim();
            if (!string.IsNullOrEmpty(_temp[21]?.ToString().Trim()))//合同类型
                temp.ContractType = _temp[21]?.ToString().Trim();
            if (!string.IsNullOrEmpty(_temp[22]?.ToString().Trim()))//合同对方来源
                temp.ContractOppositionFrom = _temp[22]?.ToString().Trim();
            if (!string.IsNullOrEmpty(_temp[23]?.ToString().Trim()))//合同对方选择方式
                temp.ContractOppositionType = _temp[23]?.ToString().Trim();
            if (!string.IsNullOrEmpty(_temp[24]?.ToString().Trim()))//采购方式
                temp.Purchase = _temp[24]?.ToString().Trim();
            if (!string.IsNullOrEmpty(_temp[25]?.ToString().Trim()))//付款方式
                temp.PayType = _temp[25]?.ToString().Trim();
            if (!string.IsNullOrEmpty(_temp[26]?.ToString().Trim()))//付款说明
                temp.PayRemark = _temp[26]?.ToString().Trim();
            if (!string.IsNullOrEmpty(_temp[27]?.ToString().Trim()))//合同有效区间起始
            {
                if (DateTime.TryParse(_temp[27]?.ToString().Trim(), out tempDateTime))
                    temp.ContractStartTime = tempDateTime;
                else
                    tip.Add("合同有效区间起始不对");
            }
            if (!string.IsNullOrEmpty(_temp[28]?.ToString().Trim()))//合同有效区间终止
            {
                if (DateTime.TryParse(_temp[28]?.ToString().Trim(), out tempDateTime))
                    temp.ContractEndTime = tempDateTime;
                else
                    tip.Add("合同有效区间终止格式不对");
            }
            if (!string.IsNullOrEmpty(_temp[29]?.ToString().Trim()))//框架合同
                temp.IsFrameContract = _temp[29]?.ToString().Trim();
            if (!string.IsNullOrEmpty(_temp[30]?.ToString().Trim()))//起草时间
            {
                if (DateTime.TryParse(_temp[30]?.ToString().Trim(), out tempDateTime))
                    temp.DraftTime = tempDateTime;
                else
                    tip.Add("起草时间格式不对");
            }
            if (string.IsNullOrEmpty(_temp[31]?.ToString().Trim()))
                tip.Add("是否MIS单类不能为空");
            else
                temp.IsMIS = _temp[31]?.ToString().Trim() == "是" ? "1" : "0";
            if (string.IsNullOrEmpty(_temp[32]?.ToString().Trim()))
                tip.Add("合同税金不能为空");
            else if (!Decimal.TryParse(_temp[32]?.ToString().Trim(), out tempDecimal))
                tip.Add("合同税金为小数类型");
            else
                temp.Tax = tempDecimal;
            if (string.IsNullOrEmpty(_temp[33]?.ToString().Trim()))
                tip.Add("供应商不能为空");
            else
                temp.Supply = _temp[33]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[34]?.ToString().Trim()))
                tip.Add("签订时间不能为空");
            {
                if (DateTime.TryParse(_temp[34]?.ToString().Trim(), out tempDateTime))
                    temp.SignTime = tempDateTime;
                else
                    tip.Add("合同有效区间终止格式不对");
            }
            if (string.IsNullOrEmpty(_temp[35]?.ToString().Trim()))
                tip.Add("合同不含税金额不能为空");
            {
                if (!Decimal.TryParse(_temp[35]?.ToString().Trim(), out tempDecimal))
                    tip.Add("合同不含税金额为小数类型");
                else
                    temp.SignTotal = tempDecimal;
            }
            if (string.IsNullOrEmpty(_temp[36]?.ToString().Trim()))
                tip.Add("合同含税金额不能为空");
            {
                if (!Decimal.TryParse(_temp[36]?.ToString().Trim(), out tempDecimal))
                    tip.Add("合同含税金额为小数类型");
                else
                    temp.SignTotalTax = tempDecimal;
            }
            if (!string.IsNullOrEmpty(_temp[37]?.ToString().Trim()))//已签署项目总额
            {
                if (!Decimal.TryParse(_temp[37]?.ToString().Trim(), out tempDecimal))
                    tip.Add("已签署项目总额为小数类型");
                else
                    temp.ProjectAllTotal = tempDecimal;
            }
            if (string.IsNullOrEmpty(_temp[38]?.ToString().Trim()))
                tip.Add("合同总金额不能为空");
            else
            {
                if (!Decimal.TryParse(_temp[38]?.ToString().Trim(), out tempDecimal))
                    tip.Add("合同总金额为小数类型");
                else
                    temp.AllTotal = tempDecimal;
            }



            var dd = this.List().Where(u => u.ProjectID == temp.ProjectID && u.ContractID == temp.ContractID).Count();
            if (dd > 0)
            {
                tip.Add("合同编号+项目编号已经存在，不可重复");
            }


            temp.ID = Guid.NewGuid();
            temp.ImportTime = DateTime.Now;
            temp.IsDel = "0";
            if (tip.Count > 0)
                return null;
            return temp;
        }

     
    }
}
