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
/// 历史项目暂估
/// </summary>
namespace CZManageSystem.Service.CollaborationCenter.Invest
{
    public class InvestAgoEstimateService : BaseService<InvestAgoEstimate>, IInvestAgoEstimateService
    {
        public IList<InvestAgoEstimate> GetForPaging(out int count, AgoEstimateQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
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
        private IQueryable<InvestAgoEstimate> GetQueryTable(AgoEstimateQueryBuilder obj = null)
        {
            var curTable = this._entityStore.Table;
            if (obj != null)
            {
                AgoEstimateQueryBuilder obj2 = (AgoEstimateQueryBuilder)CloneObject(obj);
                if (!string.IsNullOrEmpty(obj.Dept_Text))
                {//主办部门
                    curTable.Where(u => u.ManagerObj != null && u.ManagerObj.Dept != null && u.ManagerObj.Dept.DpFullName.Contains(obj.Dept_Text));
                    obj2.Dept_Text = string.Empty;
                }

                var exp = ExpressionFactory(obj2);
                if (exp != null)
                    curTable = curTable.Where(exp);

            }

            return curTable;
        }

        static Users _user;
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

                List<InvestAgoEstimate> list = new List<InvestAgoEstimate>();
                List<InvestAgoEstimate> Questions = new List<InvestAgoEstimate>();

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
                    list.Add(model);
                    if (list.Count >= 100) //足够100的
                    {
                        var list1 = list.Where(u => u.ID != Guid.Empty).ToList();
                        var list2 = list.Where(u => u.ID == Guid.Empty).ToList();
                        foreach (var itemList2 in list2)
                            itemList2.ID = Guid.NewGuid();

                        this.UpdateByList(list1);
                        this.InsertByList(list2);

                        List<InvestAgoEstimateLog> logList = new List<InvestAgoEstimateLog>();
                        foreach (var item2 in list1)
                        {
                            logList.Add(new InvestAgoEstimateLog()
                            {
                                OpName = user.RealName,
                                OpType = "导入修改",
                                OpTime = DateTime.Now,
                                ProjectID = item2.ProjectID,
                                ContractID = item2.ContractID,
                                PayTotal = item2.PayTotal,
                                Pay = item2.Pay,
                                NotPay = item2.NotPay,
                                Rate = item2.Rate
                            });
                        }
                        foreach (var item2 in list2)
                        {
                            logList.Add(new InvestAgoEstimateLog()
                            {
                                OpName = user.RealName,
                                OpType = "导入插入",
                                OpTime = DateTime.Now,
                                ProjectID = item2.ProjectID,
                                ContractID = item2.ContractID,
                                PayTotal = item2.PayTotal,
                                Pay = item2.Pay,
                                NotPay = item2.NotPay,
                                Rate = item2.Rate
                            });
                        }
                        new InvestAgoEstimateLogService().InsertByList(logList);

                        Questions.AddRange(list1);
                        Questions.AddRange(list2);
                        count += list1.Count + list2.Count;
                        list.Clear();
                        IsSuccess = true;

                    }
                }
                if (list.Count > 0)//不足100的
                {
                    var list1 = list.Where(u => u.ID != Guid.Empty).ToList();
                    var list2 = list.Where(u => u.ID == Guid.Empty).ToList();
                    foreach (var itemList2 in list2)
                        itemList2.ID = Guid.NewGuid();

                    this.UpdateByList(list1);
                    this.InsertByList(list2);

                    List<InvestAgoEstimateLog> logList = new List<InvestAgoEstimateLog>();
                    foreach (var item in list1)
                    {
                        logList.Add(new InvestAgoEstimateLog()
                        {
                            OpName = user.RealName,
                            OpType = "导入修改",
                            OpTime = DateTime.Now,
                            ProjectID = item.ProjectID,
                            ContractID = item.ContractID,
                            PayTotal = item.PayTotal,
                            Pay = item.Pay,
                            NotPay = item.NotPay,
                            Rate = item.Rate
                        });
                    }
                    foreach (var item in list2)
                    {
                        logList.Add(new InvestAgoEstimateLog()
                        {
                            OpName = user.RealName,
                            OpType = "导入插入",
                            OpTime = DateTime.Now,
                            ProjectID = item.ProjectID,
                            ContractID = item.ContractID,
                            PayTotal = item.PayTotal,
                            Pay = item.Pay,
                            NotPay = item.NotPay,
                            Rate = item.Rate
                        });
                    }
                    new InvestAgoEstimateLogService().InsertByList(logList);
                    Questions.AddRange(list1);
                    Questions.AddRange(list2);
                    count += list1.Count + list2.Count;
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

        private InvestAgoEstimate GetModel(DataRow dataRow, out List<string> tip)
        {
            DateTime tempDateTime = new DateTime();
            Decimal tempDecimal = 0;
            string tempStr = "";
            List<string> tempListStr = new List<string>();
            tip = new List<string>();
            var _temp = dataRow.ItemArray;
            InvestAgoEstimate temp = new InvestAgoEstimate();

            string str1 = null; //项目编号
            string str2 = null;//合同编号
            if (!string.IsNullOrEmpty(_temp[2]?.ToString().Trim()))//项目编号
                str1 = _temp[2]?.ToString().Trim();
            if (!string.IsNullOrEmpty(_temp[4]?.ToString().Trim()))//合同编号
                str2 = _temp[4]?.ToString().Trim();
            if (!string.IsNullOrEmpty(str1) && !string.IsNullOrEmpty(str2))
            {
                temp = this.FindByFeldName(u => u.ProjectID == str1 && u.ContractID == str2);
                if (temp == null || temp.ID == Guid.Empty)
                {
                    temp = new InvestAgoEstimate();
                    temp.Year = DateTime.Now.Year;
                    temp.Month = DateTime.Now.Month;
                }
            }

            if (!string.IsNullOrEmpty(_temp[1]?.ToString().Trim()))//项目名称
                temp.ProjectName = _temp[1]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[2]?.ToString().Trim()))//项目编号
                tip.Add("项目编号不能为空");
            else
                temp.ProjectID = _temp[2]?.ToString().Trim();
            if (!string.IsNullOrEmpty(_temp[3]?.ToString().Trim()))//合同名称
                temp.ContractName = _temp[3]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[4]?.ToString().Trim()))//合同编号
                tip.Add("合同编号不能为空");
            else
                temp.ContractID = _temp[4]?.ToString().Trim();
            if (!string.IsNullOrEmpty(_temp[5]?.ToString().Trim()))//供应商
                temp.Supply = _temp[5]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[6]?.ToString().Trim()))//合同总金额
                tip.Add("合同总金额不能为空");
            else
            {
                if (!Decimal.TryParse(_temp[6]?.ToString().Trim(), out tempDecimal))
                    tip.Add("合同总金额为小数类型");
                else
                    temp.SignTotal = tempDecimal;
            }
            if (string.IsNullOrEmpty(_temp[7]?.ToString().Trim()))//合同实际金额
                tip.Add("合同实际金额不能为空");
            else
            {
                if (!Decimal.TryParse(_temp[7]?.ToString().Trim(), out tempDecimal))
                    tip.Add("合同实际金额为小数类型");
                else
                    temp.PayTotal = tempDecimal;
            }
            if (string.IsNullOrEmpty(_temp[8]?.ToString().Trim().Replace("%", "")))//项目形象进度
                tip.Add("项目形象进度不能为空");
            else
            {
                if (!Decimal.TryParse(_temp[8]?.ToString().Trim().Replace("%", ""), out tempDecimal))
                    tip.Add("项目形象进度为百分数");
                else
                {
                    temp.Rate = tempDecimal;
                    if (_temp[8]?.ToString().Trim().IndexOf("%") < 0)
                        temp.Rate = temp.Rate * 100;
                }
            }
            if (string.IsNullOrEmpty(_temp[9]?.ToString().Trim()))//已付款金额
                tip.Add("已付款金额不能为空");
            else
            {
                if (!Decimal.TryParse(_temp[9]?.ToString().Trim(), out tempDecimal))
                    tip.Add("已付款金额为小数类型");
                else
                    temp.Pay = tempDecimal;
            }
            if (string.IsNullOrEmpty(_temp[10]?.ToString().Trim()))//暂估金额
                tip.Add("暂估金额不能为空");
            else
            {
                if (!Decimal.TryParse(_temp[10]?.ToString().Trim(), out tempDecimal))
                    tip.Add("暂估金额为小数类型");
                else
                    temp.NotPay = tempDecimal;
            }
            if (!string.IsNullOrEmpty(_temp[11]?.ToString().Trim()))//科目
                temp.Course = _temp[11]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[12].ToString().Trim()))
                tip.Add("主办人不能为空");
            else
            {
                tempListStr = _temp[12].ToString().Trim().Split(new char[] { ',', '，' }).ToList();
                var usersList = new SysUserService().List().Where(u => tempListStr.Contains(u.RealName)).Select(u => u.UserId).ToList();
                if (usersList.Count <= 0)
                    tip.Add("主办人不存在");
                else
                    temp.ManagerID = usersList[0];
            }



            if (tip.Count > 0)
                return null;
            return temp;
        }


    }
}
