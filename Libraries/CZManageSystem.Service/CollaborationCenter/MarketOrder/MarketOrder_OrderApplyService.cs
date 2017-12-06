using Aspose.Cells;
using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 营销订单-营销订单工单
/// </summary>
namespace CZManageSystem.Service.CollaborationCenter.MarketOrder
{
    public class MarketOrder_OrderApplyService : BaseService<MarketOrder_OrderApply>, IMarketOrder_OrderApplyService
    {
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="count">返回记录总数</param>
        /// <param name="objs">条件数组</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public IList<MarketOrder_OrderApply> GetForPaging(out int count, OrderApplyQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var curTable = GetQueryTable(objs);
            count = curTable.Count();
            var list = curTable.OrderByDescending(u=>u.ApplyTime).Skip(pageIndex * pageSize).Take(pageSize);
            return list.ToList();
            
        }

        /// <summary>
        /// 编辑查询条件
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        private IQueryable<MarketOrder_OrderApply> GetQueryTable(OrderApplyQueryBuilder obj = null)
        {
            var curTable = this._entityStore.Table;
            if (obj != null)
            {
                OrderApplyQueryBuilder obj2 = (OrderApplyQueryBuilder)CloneObject(obj);
                if (obj.isJK.HasValue)
                {//是否家宽业务
                    curTable = curTable.Where(u => u.MarketObj != null && (u.MarketObj.isJK ?? false) == obj.isJK);
                    obj2.isJK = null;
                }
                if (obj.ListStatus != null && obj.ListStatus.Count > 0)
                {//流程状态
                    curTable = curTable.Where(u => obj.ListStatus.Contains(u.Status));
                    obj2.ListStatus = null;
                }
                if (obj.ListOrderStatus != null && obj.ListOrderStatus.Count > 0)
                {//受理单状态
                    curTable = curTable.Where(u => obj.ListOrderStatus.Contains(u.OrderStatus));
                    obj2.ListOrderStatus = null;
                }
                if (obj.ListEndTypeID != null && obj.ListEndTypeID.Count > 0)
                {//终端机型
                    curTable = curTable.Where(u => obj.ListEndTypeID.Contains(u.EndTypeID));
                    obj2.ListEndTypeID = null;
                }
                if (obj.ListAreaID != null && obj.ListAreaID.Count > 0)
                {//所属区域
                    curTable = curTable.Where(u => obj.ListAreaID.Contains(u.AreaID));
                    obj2.ListAreaID = null;
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

                List<MarketOrder_OrderApply> list = new List<MarketOrder_OrderApply>();
                List<MarketOrder_OrderApply> Questions = new List<MarketOrder_OrderApply>();

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

        private MarketOrder_OrderApply GetModel(DataRow dataRow, out List<string> tip)
        {
            IMarketOrder_MarketService _marketService = new MarketOrder_MarketService();//营销方案
            IMarketOrder_EndTypeService _endTypeService = new MarketOrder_EndTypeService();//终端机型
            IMarketOrder_SetmealService _setmealService = new MarketOrder_SetmealService();//基本套餐
            IMarketOrder_BusinessService _businessService = new MarketOrder_BusinessService();//捆绑业务


            List<string> tempListStr = new List<string>();
            tip = new List<string>();
            var _temp = dataRow.ItemArray;
            MarketOrder_OrderApply temp = new MarketOrder_OrderApply();
            if (string.IsNullOrEmpty(_temp[0]?.ToString().Trim()))
                tip.Add("标题不能为空");
            else
                temp.Title = _temp[0]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[1]?.ToString().Trim()))
                tip.Add("营销方案名称不能为空");
            else
            {
                string value = _temp[1]?.ToString().Trim();
                var obj1 = _marketService.FindByFeldName(u => u.Market == value);
                if (obj1 == null || obj1.ID == Guid.Empty)
                    tip.Add("系统中没有对应的营销方案");
                else
                    temp.MarketID = obj1.ID;
            }
            if (string.IsNullOrEmpty(_temp[2]?.ToString().Trim()))
                tip.Add("终端机型不能为空");
            else
            {
                string value = _temp[2]?.ToString().Trim();
                var obj1 = _endTypeService.FindByFeldName(u => u.EndType == value);
                if (obj1 == null || obj1.ID == Guid.Empty)
                    tip.Add("系统中没有对应的终端机型");
                else
                    temp.EndTypeID = obj1.ID;
            }
            if (string.IsNullOrEmpty(_temp[3]?.ToString().Trim()))
                tip.Add("目标客户不能为空");
            else
            {
                string value = _temp[3]?.ToString().Trim();
                if (!CommonFunction.isMobilePhone(value))
                    tip.Add("目标客户需要有效的手机号码");
                else
                    temp.CustomPhone = value;
            }
            if (string.IsNullOrEmpty(_temp[4]?.ToString().Trim()))
                tip.Add("客户名称不能为空");
            else
                temp.CustomName = _temp[4]?.ToString().Trim();
            if (!string.IsNullOrEmpty(_temp[5]?.ToString().Trim()))//客户身份证号
                temp.CustomPersonID = _temp[5]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[6]?.ToString().Trim()))//
                tip.Add("其他联系方式不能为空");
            else
                temp.CustomOther = _temp[6]?.ToString().Trim();
            if (string.IsNullOrEmpty(_temp[7]?.ToString().Trim()))//
                tip.Add("联系地址不能为空");
            else
                temp.CustomAddr = _temp[7]?.ToString().Trim();
            if (!string.IsNullOrEmpty(_temp[8]?.ToString().Trim()))//可用号码
                temp.UseNumber = _temp[8]?.ToString().Trim();
            if (!string.IsNullOrEmpty(_temp[9]?.ToString().Trim()))//SIM卡号
                temp.SIMNumber = _temp[9]?.ToString().Trim();
            if (!string.IsNullOrEmpty(_temp[10]?.ToString().Trim()))//IMEI码
                temp.IMEI = _temp[10]?.ToString().Trim();
            if (!string.IsNullOrEmpty(_temp[11]?.ToString().Trim()))//基本套餐
            {
                string value = _temp[11]?.ToString().Trim();
                var obj1 = _setmealService.FindByFeldName(u => u.Setmeal == value);
                if (obj1 == null || obj1.ID == Guid.Empty)
                    tip.Add("系统中没有对应的基本套餐");
                else
                    temp.SetmealID = obj1.ID;
            }
            if (!string.IsNullOrEmpty(_temp[12]?.ToString().Trim()))//捆绑业务
            {
                string value = _temp[12]?.ToString().Trim();
                var obj1 = _businessService.FindByFeldName(u => u.Business == value);
                if (obj1 == null || obj1.ID == Guid.Empty)
                    tip.Add("系统中没有对应的捆绑业务");
                else
                    temp.BusinessID = obj1.ID;
            }
            if (!string.IsNullOrEmpty(_temp[13]?.ToString().Trim()))//备注
                temp.Remark = _temp[13]?.ToString().Trim();
            if (!string.IsNullOrEmpty(_temp[14]?.ToString().Trim()))//项目编号
                temp.ProjectID = _temp[14]?.ToString().Trim();
            if (!string.IsNullOrEmpty(_temp[15]?.ToString().Trim()))//发送对象账号
                temp.SendTo = _temp[15]?.ToString().Trim();

            temp.ApplyID = Guid.NewGuid();
            temp.ApplyTime = DateTime.Now;
            temp.Applicant = _user.UserId;
            temp.MobilePh = _user.Mobile;
            temp.Status = "编辑";
            temp.OrderStatus = "草稿";
            if (tip.Count > 0)
                return null;
            return temp;
        }

    }
}
