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
/// 投资项目信息
/// </summary>
namespace CZManageSystem.Service.CollaborationCenter.Invest
{
    public class InvestProjectService : BaseService<InvestProject>, IInvestProjectService
    {
        static Users _user;

        public IList<InvestProject> GetForPaging(out int count, InvestProjectQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
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
        private IQueryable<InvestProject> GetQueryTable(InvestProjectQueryBuilder obj = null)
        {
            var curTable = this._entityStore.Table;
            if (obj != null)
            {
                InvestProjectQueryBuilder obj2 = (InvestProjectQueryBuilder)CloneObject(obj);

                if (!string.IsNullOrEmpty(obj.DpCode_Text))
                {//部门
                    var DpCodeList = new Service.SysManger.SysDeptmentService().List().Where(u => u.DpName.Contains(obj.DpCode_Text)).Select(u => u.DpId).ToList();
                    curTable = curTable.Where(u => DpCodeList.Contains(u.DpCode));
                    obj2.DpCode_Text = null;
                }
                if (!string.IsNullOrEmpty(obj.ManagerID_Text))
                {//项目经理
                    var ManagerIDList = new Service.SysManger.SysUserService().List().Where(u => u.RealName.Contains(obj.ManagerID_Text)).Select(u => u.UserId).ToList();
                    curTable = curTable.Where(u => ManagerIDList.Contains(u.ManagerID.Value));
                    obj2.ManagerID_Text = null;
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

                List<InvestProject> list = new List<InvestProject>();
                List<InvestProject> Questions = new List<InvestProject>();

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
                    if (list.Where(u => u.ProjectID == model.ProjectID).Count() > 0)
                    {
                        error += (string.IsNullOrEmpty(error) ? "" : "<br/>") + "第" + row + "行:项目编号已经被占用";
                        continue;
                    }

                    list.Add(model);
                    if (list.Count == 100) //足够100的
                    {
                        if (this.InsertByList(list))
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
                    if (this.InsertByList(list))
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

        private InvestProject GetModel(DataRow dataRow, out List<string> tip)
        {
            Decimal tempDecimal = 0;
            string tempStr = "";
            List<string> tempListStr = new List<string>();
            tip = new List<string>();
            var _temp = dataRow.ItemArray;
            InvestProject temp = new InvestProject();
            if (string.IsNullOrEmpty(_temp[0]?.ToString().Trim()))
                tip.Add("计划任务书文号不能为空");
            else
                temp.TaskID = _temp[0]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[1]?.ToString().Trim()))
                tip.Add("项目编号不能为空");
            else
            {
                if (CheckProjectID(Guid.Empty, _temp[1]?.ToString().Trim()))
                    temp.ProjectID = _temp[1]?.ToString().Trim();
                else
                    tip.Add("项目编号已经被占用");
            }
            if (string.IsNullOrEmpty(_temp[2]?.ToString().Trim()))
                tip.Add("项目名称不能为空");
            else
                temp.ProjectName = _temp[2]?.ToString().Trim();
            if (!string.IsNullOrEmpty(_temp[3]?.ToString().Trim()))//起止年限
                temp.BeginEnd = _temp[3]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[4]?.ToString().Trim()))
                tip.Add("项目总投资不能为空");
            else if (!Decimal.TryParse(_temp[4]?.ToString().Trim(), out tempDecimal))
                tip.Add("项目总投资为小数类型");
            else
                temp.Total = tempDecimal;

            if (string.IsNullOrEmpty(_temp[5]?.ToString().Trim()))
                tip.Add("项目总投资不能为空");
            else if (!Decimal.TryParse(_temp[5]?.ToString().Trim(), out tempDecimal))
                tip.Add("项目总投资为小数类型");
            else
                temp.YearTotal = tempDecimal;
            if (string.IsNullOrEmpty(_temp[6]?.ToString().Trim()))
                tip.Add("年度建设内容不能为空");
            else
                temp.Content = _temp[6]?.ToString().Trim();
            if (!string.IsNullOrEmpty(_temp[7]?.ToString().Trim()))//要求完成时限
                temp.FinishDate = _temp[7]?.ToString().Trim();

            if (string.IsNullOrEmpty(_temp[8]?.ToString().Trim()))
                tip.Add("负责专业室不能为空");
            else
            {
                tempStr = _temp[8].ToString().Trim();
                var deptsList = new SysDeptmentService().List().Where(w => w.DpName == tempStr).Select(s => s.DpId).ToList();
                if (deptsList.Count <= 0)
                    tip.Add("负责专业室不存在");
                else
                    temp.DpCode = deptsList[0];
            }
            if (string.IsNullOrEmpty(_temp[9].ToString().Trim()))
                tip.Add("室负责人不能为空");
            else
            {
                tempListStr = _temp[9].ToString().Trim().Split(new char[] { ',', '，' }).ToList();
                var usersList = new SysUserService().List().Where(u => tempListStr.Contains(u.RealName)).Select(u => u.UserId).ToList();
                if (usersList.Count <= 0)
                    tip.Add("室负责人不存在");
                else
                    temp.UserID = usersList[0];
            }

            if (string.IsNullOrEmpty(_temp[10].ToString().Trim()))
                tip.Add("项目经理不能为空");
            else
            {
                tempListStr = _temp[10].ToString().Trim().Split(new char[] { ',', '，' }).ToList();
                var usersList = new SysUserService().List().Where(u => tempListStr.Contains(u.RealName)).Select(u => u.UserId).ToList();
                if (usersList.Count <= 0)
                    tip.Add("项目经理不存在");
                else
                    temp.ManagerID = usersList[0];
            }

            if (!string.IsNullOrEmpty(_temp[11]?.ToString().Trim()))//起止年限
            {
                int intYear = 0;
                if (int.TryParse(_temp[11]?.ToString().Trim(), out intYear))
                    temp.Year = intYear;
                else
                    temp.Year = DateTime.Now.Year;
            }
            else
                temp.Year = DateTime.Now.Year;
            temp.ID = Guid.NewGuid();
            if (tip.Count > 0)
                return null;
            return temp;
        }

        /// <summary>
        /// 检查项目编号
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        private bool CheckProjectID(Guid ID, string ProjectID)
        {
            var dd = this.List().Where(u => u.ID != ID && u.ProjectID == ProjectID).ToList();
            if (dd.Count > 0)
                return false;
            else
                return true;
        }

    }
}
