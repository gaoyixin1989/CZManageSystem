using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.Composite;
using CZManageSystem.Service.Composite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CZManageSystem.Service.ITSupport;
using CZManageSystem.Data.Domain.ITSupport;
using CZManageSystem.Service.SysManger;
using CZManageSystem.Admin.Base;
using CZManageSystem.Data.Domain.SysManger;
using Newtonsoft.Json;
using Aspose.Cells;

namespace CZManageSystem.Admin.Controllers.ITSupport
{
    public class ConsumableController : BaseController
    {
        IConsumableService _consumableService = new ConsumableService();
        IConsumable_InputListService _inputListService = new Consumable_InputListService();
        IConsumable_InputDetailService _inputDetailService = new Consumable_InputDetailService();
        IConsumable_CancellingService _cancellingService = new Consumable_CancellingService();
        IConsumable_CancellingDetailService _cancellingDetailService = new Consumable_CancellingDetailService();
        IConsumable_ConsumingService _consumableConsumingService = new Consumable_ConsumingService();
        IConsumable_ConsumingDetailService _consumable_ConsumingDetailService = new Consumable_ConsumingDetailService();
        ILibraryStatisticsService _libraryStatisticsService = new LibraryStatisticsService();
        IScrapDetailService _scrapDetailService = new ScrapDetailService();
        ICancellingDetailService _cancellingDetail = new CancellingDetailService();
        IConsumingDetailService _consumingDetailService = new ConsumingDetailService();

        IConsumable_LevellingService _levellingService = new Consumable_LevellingService();//耗材调平申请表
        IConsumable_LevellingDetailService _levellingDetailService = new Consumable_LevellingDetailService();//耗材调平申请明细表
        IConsumable_ScrapService _consumable_ScrapService = new Consumable_ScrapService();
        IConsumable_ScrapDetailService _consumable_ScrapDetailService = new Consumable_ScrapDetailService();
        //耗材零星
        IConsumable_SporadicService _consumable_SporadicService = new Consumable_SporadicService();
        IConsumable_SporadicDetailService _consumable_SporadicDetailService = new Consumable_SporadicDetailService();

        IConsumable_MakeupService _makeupService = new Consumable_MakeupService();//耗材补录归档
        IConsumable_MakeupDetailService _makeupDetailService = new Consumable_MakeupDetailService();//耗材补录归档明细

        ITracking_WorkflowService _tracking_WorkflowService = new Tracking_WorkflowService();

        // GET: Consumable
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ConsumableIndex()
        {
            return View();
        }
        public ActionResult RepertoryIndex()
        {
            return View();
        }
        public ActionResult StorageIndex()
        {
            return View();
        }
        public ActionResult ConsumableEdit(int? id)
        {
            ViewData["Id"] = id;
            if (id == null)
                ViewBag.Title = "耗材新增";
            else
                ViewBag.Title = "耗材编辑";
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="selected">选中的耗材ID</param>
        /// <param name="hasStock">是否存在库存量</param>
        /// <returns></returns>
        public ActionResult ConWinInfo(string selected,bool? hasStock=null)
        {
            ViewData["selected"] = selected;
            ViewData["hasStock"] = hasStock;
            return View();
        }
        public ActionResult ConsumingApply(Guid? Id, string type)
        {
            ViewData["workflowName"] = FlowInstance.WorkflowType.ConsumingApply;
            ViewData["Id"] = Id;
            ViewData["type"] = type;
            if (Id == null)
                ViewBag.Title = "耗材领用申请";
            else
                ViewBag.Title = "耗材领用申请编辑";
            return View();
        }

        public ActionResult ConsumingList()
        {
            return View();
        }
        public ActionResult ConsumingView(Guid? Id)
        {
            ViewData["Id"] = Id;
            return View();
        }
        public ActionResult StorageEdit(int? ID)
        {
            ViewData["ID"] = ID;
            return View();
        }

        public ActionResult ScrapDetail()
        {
            return View();
        }
        public ActionResult CancellingDetail()
        {
            return View();
        }
        public ActionResult ConsumingDetail()
        {
            return View();
        }
        #region 领用申请


        #region 获取信息下一个步骤和执行人信息

        /// <summary>
        /// 获取申请人信息
        /// </summary>
        /// <returns></returns>
        public ActionResult getNewApplyInfo()
        {
            object AppInfo = new object();
            if (this.WorkContext.CurrentUser != null)
            {
                AppInfo = new
                {
                    Applylicant = this.WorkContext.CurrentUser.UserId,
                    ApplyName = this.WorkContext.CurrentUser.RealName,
                    ApplyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    ApplyDpCode = this.WorkContext.CurrentUser.Dept.DpId,
                    DpName = this.WorkContext.CurrentUser.Dept.DpName,
                    Mobile = this.WorkContext.CurrentUser.Mobile,
                    // Series = "流程单号待生成",
                    Title = this.WorkContext.CurrentUser.RealName + "的领用申请(" + DateTime.Now.ToString("yyyy-MM-dd") + ")"
                };
            }
            return Json(AppInfo);
        }
        #endregion
        public ActionResult GetListData_ConsumingDetail(int pageIndex = 1, int pageSize = 5, ConsumableQueryBuilder queryBuilder = null)
        {

            int count = 0;
            var modelList = _consumingDetailService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);

            return Json(new { items = modelList, count = count });
        }
        /// <summary>
        /// 领用申请列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="queryBuilder"></param>
        /// <param name="withDelData"></param>
        /// <returns></returns>
        public ActionResult GetListData_Consuming(int pageIndex = 1, int pageSize = 5, ConsumingListQueryBuilder queryBuilder = null, bool withDelData = false)
        {
            int count = 0;
            var modelList = _consumableConsumingService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);

            return Json(new { items = modelList, count = count });
        }
        /// <summary>
        /// 删除领用申请
        /// </summary>
        /// <param name="ConsumingIDs">领用申请ID</param>
        /// <returns></returns>
        public ActionResult Delete_Consuming(Guid?[] ConsumingIDs)
        {
            int allCount = 0;
            int successCount = 0;
            string strMsg = "";

            var ListDatas = _consumableConsumingService.List().Where(u => ConsumingIDs.Contains(u.ID)).ToList();//领用申请信息
            allCount = ListDatas.Count;
            ListDatas = ListDatas.Where(u => u.State != 1).ToList();

            if (allCount > ListDatas.Count)
            {
                strMsg = "其中存在已经提交的申请信息，不能删除";
            }
            else if (ListDatas.Count > 0)
            {
                foreach (var item in ListDatas)
                {
                    if (_consumableConsumingService.Delete(item))
                    {
                        successCount++;
                        var detailData = _consumable_ConsumingDetailService.List().Where(u => u.ApplyID == item.ID).ToList();
                        _consumable_ConsumingDetailService.DeleteByList(detailData);
                    }
                }
            }
            else
            {
                strMsg = "删除失败";
            }

            return Json(new
            {
                isSuccess = successCount > 0 ? true : false,
                successCount = successCount,
                messsage = strMsg
            });
        }
        public ActionResult ConsumableGetDetailByApplyID(Guid Id)
        {
            var modelList = _consumable_ConsumingDetailService.List().Where(u => u.ApplyID == Id).ToList();

            object DetailInfo = new object();
            List<object> det = new List<object>();
            foreach (var item in modelList)
            {
                var detail = _consumableService.List().Where(t => t.ID == item.ConsumableID).ToList();
                if (detail != null && detail.Count > 0)
                {
                    DetailInfo = new
                    {
                        ID = detail[0].ID,
                        Type = detail[0].Type,
                        Model = detail[0].Model,
                        Name = detail[0].Name,
                        IsValue = detail[0].IsValue,
                        Unit = detail[0].Unit,
                        Equipment = detail[0].Equipment,
                        Trademark = detail[0].Trademark,
                        count = item.ClaimsNumber
                    };
                    det.Add(DetailInfo);
                }
            }

            return Json(new { items = det });
        }
        public ActionResult ConsumableGetListByIds(int[] ids)
        {
            var modelList = _consumableService.List().Where(u => ids.Contains(u.ID)).ToList();
            return Json(new { items = modelList });

        }
        /// <summary>
        /// 保存领用申请
        /// </summary>
        /// <param name="ConsumingList">领用申请</param>
        /// <param name="Details">领用明细</param>
        /// <returns></returns>
        public ActionResult Save_Consuming(Consumable_Consuming ConsumingList, List<Consumable_ConsumingDetail> Details = null)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过
            if (ConsumingList.Title == null || string.IsNullOrEmpty(ConsumingList.Title.Trim()))
                tip = "标题不能为空";
            else
            {
                isValid = true;
                ConsumingList.Title = ConsumingList.Title.Trim();
            }

            if (!isValid)
            {
                result.IsSuccess = false;
                result.Message = tip;
                return Json(result);
            }
            #endregion
            ConsumingList.Applicant = this.WorkContext.CurrentUser.UserId.ToString();
            ConsumingList.ApplyTime = DateTime.Now;
            ConsumingList.ApplyDpCode = this.WorkContext.CurrentUser.Dept.DpId;
            ConsumingList.State = 0;
            if (ConsumingList.ID == null || ConsumingList.ID.ToString() == "00000000-0000-0000-0000-000000000000")
            {//新增
                ConsumingList.ID = Guid.NewGuid();
                result.IsSuccess = _consumableConsumingService.Insert(ConsumingList);
            }
            else
            {//编辑
                result.IsSuccess = _consumableConsumingService.Update(ConsumingList);
            }
            result.Message = ConsumingList.ID.ToString();
            //领用申请保存成功，则继续保存更新领用申请单明细
            if (result.IsSuccess)
            {
                // int count = 0;
                //先删除原来的明细数据
                var oldData = _consumable_ConsumingDetailService.List().Where(u => u.ApplyID == ConsumingList.ID).ToList();
                _consumable_ConsumingDetailService.DeleteByList(oldData);
                if (Details != null && Details.Count > 0)
                {

                    foreach (var item in Details)
                    {
                        item.ApplyID = ConsumingList.ID;
                        //item.ConsumableID=
                        item.ID = Guid.NewGuid();
                    }

                    result.IsSuccess = _consumable_ConsumingDetailService.InsertByList(Details);
                    if (!result.IsSuccess)
                    {
                        result.Message = "保存领用申请耗材明细信息失败";
                    }
                }
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "保存领用申请信息失败";
            }

            return Json(result);
        }
        /// <summary>
        /// 提交领用申请
        /// </summary>
        /// <param name="ApplyID">申请ID</param>
        /// <returns></returns>
        public ActionResult Sumbit_Consuming(Guid ApplyID, string nextActivity, string nextActors, string nextCC = null)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            var curData = _consumableConsumingService.FindById(ApplyID);
            if (curData == null || curData.ID.ToString() == "00000000-0000-0000-0000-000000000000")
            {
                result.IsSuccess = false;
                result.Message = "该领用申请信息不存在";
                return Json(result);
            }

            Dictionary<string, string> dictField = new Dictionary<string, string>();
            dictField.Add("F1", curData.ID.ToString());
            var workFlowResult = WorkFlowHelper.ActionFlow(WorkContext.CurrentUser.UserName, curData.Title, dictField, nextActivity, nextActors, FlowInstance.WorkflowType.ConsumingApply);
            if (workFlowResult.Success > 0)
            {
                curData.WorkflowInstanceId = workFlowResult.WorkFlow.WorkflowInstanceId;
                curData.Series = workFlowResult.WorkFlow.SheetId;
                curData.State = 1;
                if (_consumableConsumingService.Update(curData))
                {
                    result.IsSuccess = true;
                }
            }

            if (result.IsSuccess && !string.IsNullOrEmpty(nextCC))
            {//提交成功后抄送
                curData = _consumableConsumingService.FindById(ApplyID);
                CommonFunction.PendingData(curData.WorkflowInstanceId.Value, nextCC);//抄送
            }

            return Json(result);
        }

        #endregion
        public ActionResult GetListData_CancellingDetails(int pageIndex = 1, int pageSize = 5, ConsumableQueryBuilder queryBuilder = null)
        {

            int count = 0;
            var modelList = _cancellingDetail.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);

            return Json(new { items = modelList, count = count });
        }



        public ActionResult GetListData_ScrapDetail(int pageIndex = 1, int pageSize = 5, ConsumableQueryBuilder queryBuilder = null)
        {

            int count = 0;
            var modelList = _scrapDetailService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);

            return Json(new { items = modelList, count = count });
        }

        public ActionResult GetListData_Storage(int ID)
        {
            int count = 0;
            var list = _libraryStatisticsService.List().Where(u => u.ID == ID).ToList();
            return Json(new { items = list, count = count });

        }

        public ActionResult ConsumableGetByID(Guid Id)
        {
            var modelList = _consumableConsumingService.FindById(Id);
            List<object> listResult = new List<object>();
            listResult.Add(new
            {
                modelList.ID,
                modelList.WorkflowInstanceId,
                modelList.Series,
                modelList.ApplyTime,
                DpName = CommonFunction.getDeptNamesByIDs(modelList.ApplyDpCode),
                ApplyName = CommonFunction.getUserRealNamesByIDs(modelList.Applicant),
                modelList.Mobile,
                modelList.Title,
                modelList.Content,
                modelList.State
            });

            return Json(new { items = listResult });
        }

        /// <summary>
        /// 下载数据
        /// </summary> 
        /// <param name="queryBuilder">参数</param>
        /// <returns></returns>
        public ActionResult Download(string queryBuilder = null)
        {
            string path = "";
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<ConsumableQueryBuilder>(queryBuilder);
                int pageIndex = 0;
                int pageSize = int.MaxValue;

                int count = 0;
                var modelList = _consumableService.GetForPaging(out count, QueryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize) as List<Consumable>;
                var list = modelList.Select(s => new
                {
                    s.Amount,
                    s.Equipment,
                    s.ID,
                    s.IsDelete,
                    IsValue = s?.IsValue == "1" ? "是" : "否",
                    s.Model,
                    s.Name,
                    s.Remark,
                    s.Trademark,
                    s.Type,
                    s.Unit

                }).ToList<object>();
                if (list.Count < 1)
                    return View("../Export/Message");
                #region Excel部分
                //生成文件名
                var fileToSaveName = SaveName.Consumable + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.Consumable);
                //设置集合变量
                designer.SetDataSource(ImportFileType.Consumable, list);
                //根据数据源处理生成报表内容
                designer.Process();
                var response = GetResponse(fileToSaveName);
                designer.Save(Url.Content(fileToSaveName), SaveType.OpenInExcel, FileFormatType.Excel2003, response);
                response.Flush();
                response.Close();
                designer = null;
                response.End();

                #endregion

            }
            catch (Exception ex)
            {

                throw;
            }
            return null;
        }

        /// <summary>
        /// 获取基础数据维护列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="queryBuilder"></param>
        /// <returns></returns>
        public ActionResult ConsumableGetListData(int pageIndex = 1, int pageSize = 5, ConsumableQueryBuilder queryBuilder = null, bool withDelData = false)
        {
            //if (queryBuilder.StartCreateTime != null)
            //    queryBuilder.StartCreateTime = queryBuilder.StartCreateTime.Value.Date;
            //if (queryBuilder.EndCreateTime != null)
            //    queryBuilder.EndCreateTime = queryBuilder.EndCreateTime.Value.AddDays(1).Date.AddSeconds(-1);
            if (!withDelData)
                queryBuilder.IsDelete = "0";
            int count = 0;
            var modelList = _consumableService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);

            return Json(new { items = modelList, count = count });
        }
        /// <summary>
        /// 新增和修改数据
        /// </summary>
        /// <param name="Cons"></param>
        /// <returns></returns>
        public ActionResult ConsumableSave(Consumable Cons)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };

            if (Cons.ID == 0)
            {
                Cons.IsDelete = "0";
                result.IsSuccess = _consumableService.Insert(Cons);
            }
            else
            {
                Cons.IsDelete = "0";
                result.IsSuccess = _consumableService.Update(Cons);
            }
            return Json(result);
        }
        /// <summary>
        /// 根据id查询数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ConsumableGetDataByID(int id)
        {
            var item = _consumableService.FindById(id);
            return Json(new
            {
                ID = item.ID,
                Type = item.Type,
                Model = item.Model,
                Name = item.Name,
                Equipment = item.Equipment,
                Unit = item.Unit,
                Trademark = item.Trademark,
                IsValue = item.IsValue,
                Amount = item.Amount,
                Remark = item.Remark
            });
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult ConsumableDelete(int[] ids)
        {


            foreach (int id in ids)
            {
                var obj = _consumableService.FindById(id);
                if (obj != null)
                {
                    obj.IsDelete = "1";
                    _consumableService.Update(obj);
                }
            }
            return Json(new { status = 0, message = "成功" });
        }

        #region 入库管理
        //耗材入库单信息页面
        public ActionResult InputList()
        {
            return View();
        }
        //耗材入库单的明细页面
        public ActionResult InputListEdit(int? InputListID, string type = "look")
        {
            Consumable_InputList inputList = _inputListService.FindById(InputListID ?? 0);
            if (inputList != null && inputList.State == 1)
                type = "look";
            ViewData["type"] = type;
            ViewData["InputListID"] = InputListID;
            ViewData["RealName"] = this.WorkContext.CurrentUser.RealName;
            ViewData["Mobile"] = this.WorkContext.CurrentUser.Mobile;
            ViewData["NewTitle"] = this.WorkContext.CurrentUser.RealName + DateTime.Now.ToString("(yyyy-MM-dd HH:mm:ss)") + "入库";
            return View();
        }

        /// <summary>
        /// 获取入库单信息数据列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数据量</param>
        /// <param name="queryBuilder">查询条件</param>
        /// <returns></returns>
        public ActionResult GetListData_InputList(int pageIndex = 1, int pageSize = 5, ConsumableInputListQueryBuilder queryBuilder = null, bool isManager = false)
        {
            if (!isManager)
            {
                queryBuilder.Operator = this.WorkContext.CurrentUser.UserId;
            }
            int count = 0;
            var modelList = _inputListService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (Consumable_InputList)u).ToList();

            List<object> listResult = new List<object>();
            foreach (var item in modelList)
            {
                listResult.Add(new
                {
                    item.ID,
                    item.Title,
                    item.Code,
                    item.CreateTime,
                    item.InputTime,
                    item.Operator,
                    Operator_Text = CommonFunction.getUserRealNamesByIDs(item.Operator.HasValue ? item.Operator.ToString() : ""),
                    item.Remark,
                    item.State,
                    item.SumbitUser,
                    SumbitUser_Text = CommonFunction.getUserRealNamesByIDs(item.SumbitUser.HasValue ? item.SumbitUser.ToString() : "")
                });
            }

            return Json(new { items = listResult, count = count }, JsonRequestBehavior.AllowGet);

        }


        public ActionResult GetListData_InputLists(int pageIndex = 1, int pageSize = 5, ConsumableInputListQueryBuilder queryBuilder = null)
        {
            int count = 0;
            queryBuilder.State = 1;
            var modelList = _inputListService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (Consumable_InputList)u).ToList();

            List<object> listResult = new List<object>();
            foreach (var item in modelList)
            {
                listResult.Add(new
                {
                    item.ID,
                    item.Title,
                    item.Code,
                    item.CreateTime,
                    item.InputTime,
                    item.Operator,
                    Operator_Text = CommonFunction.getUserRealNamesByIDs(item.Operator.HasValue ? item.Operator.ToString() : ""),
                    item.Remark,
                    item.State,
                    item.SumbitUser,
                    SumbitUser_Text = CommonFunction.getUserRealNamesByIDs(item.SumbitUser.HasValue ? item.SumbitUser.ToString() : "")
                });
            }

            return Json(new { items = listResult, count = count }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 根据ID获取入库单信息数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetDataByID_InputList(int id)
        {
            var inputData = _inputListService.FindById(id);
            int count = 0;
            List<Consumable_InputDetail> detailDatas = new List<Consumable_InputDetail>();
            Users sumbitUser = new Users();
            if (inputData != null)
            {
                detailDatas = _inputDetailService.List().Where(u => u.InputListID == inputData.ID).ToList();
                sumbitUser = new SysUserService().FindById(inputData.SumbitUser);
            }
            return Json(new
            {
                inputData = inputData,
                detailDatas = detailDatas,
                sumbitUser = sumbitUser == null ? null : new { RealName = sumbitUser.RealName, Mobile = sumbitUser.Mobile }
            }
                , JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取入库单明细数据列表
        /// </summary>
        /// <param name="InputListID">入库单ID</param>
        /// <param name="OnlyInput">是否只输出入库的信息</param>
        /// <returns></returns>
        public ActionResult GetListData_InputDetail(int InputListID = 0, bool OnlyInput = true)
        {
            int count = 0;
            //获取入库明细数量
            var detailList = _inputDetailService.GetForPaging(out count, new { InputListID = InputListID }).Select(u => (Consumable_InputDetail)u).ToList();
            List<int> detail_ConsumableList = detailList.Select(u => u.ConsumableID.Value).ToList();

            //获取耗材信息数据
            List<Consumable> modelList = new List<Consumable>();
            if (OnlyInput)
            {
                modelList = _consumableService.List().Where(u => detail_ConsumableList.Contains(u.ID)).ToList();
            }
            else
            {
                modelList = _consumableService.List().ToList();
            }
            ////去掉没有入库明细且已经被删除的信息
            //modelList = modelList.Where(u => u.IsDelete == "0" || detail_ConsumableList.Contains(u.ID)).ToList();

            List<object> listResult = new List<object>();
            foreach (var item in modelList)
            {
                int countTemp = 0;
                countTemp = detailList.Where(u => u.ConsumableID == item.ID).FirstOrDefault()?.Amount ?? 0;
                listResult.Add(new
                {
                    item.ID,
                    item.Type,
                    item.Name,
                    item.Trademark,
                    item.Model,
                    item.Equipment,
                    item.Unit,
                    item.IsValue,
                    Count = countTemp
                });
            }
            return Json(new { items = listResult, count = count });
        }

        /// <summary>
        /// 保存入库单
        /// </summary>
        /// <param name="inputList">入库单信息</param>
        /// <param name="inputDetails">入库单明细</param>
        /// <returns></returns>
        public ActionResult Save_InputList(Consumable_InputList inputList, List<Consumable_Amount> inputDetails = null)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过
            if (inputList.Title == null || string.IsNullOrEmpty(inputList.Title.Trim()))
                tip = "标题不能为空";
            else
            {
                isValid = true;
                inputList.Title = inputList.Title.Trim();
            }

            if (!isValid)
            {
                result.IsSuccess = false;
                result.Message = tip;
                return Json(result);
            }
            #endregion

            if (inputList.ID == 0)
            {//新增
                inputList.CreateTime = DateTime.Now;
                inputList.Operator = this.WorkContext.CurrentUser.UserId;
                inputList.State = 0;
                result.IsSuccess = _inputListService.Insert(inputList);
            }
            else
            {//编辑
                inputList.Operator = this.WorkContext.CurrentUser.UserId;
                result.IsSuccess = _inputListService.Update(inputList);
            }

            //入库单信息保存成功，则继续保存更新入库单明细
            if (result.IsSuccess)
            {
                int count = 0;
                //先删除原来的明细数据
                var oldData = _inputDetailService.GetForPaging(out count, new { InputListID = inputList.ID }).Select(u => (Consumable_InputDetail)u).ToList();
                _inputDetailService.DeleteByList(oldData);
                if (inputDetails != null && inputDetails.Count > 0)
                {
                    List<Consumable_InputDetail> listDetail = new List<Consumable_InputDetail>();
                    foreach (var item in inputDetails)
                    {
                        Consumable_InputDetail tempData = new Consumable_InputDetail();
                        tempData.InputListID = inputList.ID;
                        tempData.ConsumableID = item.ConsumableID;
                        tempData.Amount = item.Amount;
                        listDetail.Add(tempData);
                    }
                    if (listDetail.Count > 0)
                        result.IsSuccess = _inputDetailService.InsertByList(listDetail);
                    else
                        result.IsSuccess = true;
                    if (!result.IsSuccess)
                    {
                        result.Message = "保存入库单入库耗材明细信息失败";
                    }
                }
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "保存入库单信息失败";
            }

            if (result.IsSuccess)
                result.Message = inputList.ID.ToString();
            return Json(result);
        }
        /// <summary>
        /// 提交入库单(入库)
        /// </summary>
        /// <param name="inputListID">入库单ID</param>
        /// <returns></returns>
        public ActionResult Sumbit_InputList(int inputListID)
        {
            SystemResult result = new SystemResult() { IsSuccess = false, Message = "" };

            var curData = _inputListService.FindById(inputListID);
            if (curData == null || curData.ID == 0)
            {
                result.IsSuccess = false;
                result.Message = "该入库单信息不存在";
                return Json(result);
            }

            int count = 0;
            //获取当前入库单的明细
            var detailData = _inputDetailService.GetForPaging(out count, new { InputListID = curData.ID }).Select(u => (Consumable_InputDetail)u).ToList();
            int detailDataAmount = 0;
            detailDataAmount = detailData.Sum(u => u.Amount ?? 0);
            if (detailDataAmount > 0)
            {
                curData.State = 1;
                curData.InputTime = DateTime.Now;
                curData.Code = CommonFunction.GetSerialNo("Ware[Year][Month]-"); //"待定";
                curData.SumbitUser = this.WorkContext.CurrentUser.UserId;
                result.IsSuccess = _inputListService.Update(curData);
                if (!result.IsSuccess)
                {
                    result.Message = "入库单提交失败";
                }
                else
                {
                    //修改库存数量
                    List<Consumable> consumableList = new List<Consumable>();
                    foreach (var tempDetail in detailData)
                    {
                        Consumable tempData = _consumableService.FindById(tempDetail.ConsumableID);
                        tempData.Amount += tempDetail.Amount ?? 0;
                        consumableList.Add(tempData);
                    }
                    if (!_consumableService.UpdateByList(consumableList))
                    {//修改库存数量失败
                        result.IsSuccess = false;
                        result.Message = "入库时修改库存数量失败";
                        curData.State = 0;
                        _inputListService.Update(curData);
                    }
                }
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "入库耗材数量为空";
            }
            return Json(result);
        }
        /// <summary>
        /// 删除入库单
        /// </summary>
        /// <param name="InputListID">入库单ID</param>
        /// <returns></returns>
        public ActionResult Delete_InputList(int InputListID)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            var curData = _inputListService.FindById(InputListID);
            if (curData == null || curData.ID == 0)
            {
                result.IsSuccess = false;
                result.Message = "该入库单信息不存在";
                return Json(result);
            }
            if (curData.State == 1)
            {
                result.IsSuccess = false;
                result.Message = "该入库单已经提交，不允许删除";
                return Json(result);
            }

            result.IsSuccess = _inputListService.Delete(curData);
            if (result.IsSuccess)
            {
                int count = 0;
                var detailData = _inputDetailService.GetForPaging(out count, new { InputListID = curData.ID }).Select(u => (Consumable_InputDetail)u).ToList();
                _inputDetailService.DeleteByList(detailData);
            }
            else
            {
                result.Message = "入库单删除失败";
            }

            return Json(result);
        }

        /// <summary>
        /// 删除入库单
        /// </summary>
        /// <param name="InputListID">入库单ID</param>
        /// <returns></returns>
        public ActionResult Delete_InputLists(int[] InputListIDs)
        {
            bool isSuccess = false;
            int allCount = 0;
            int successCount = 0;
            string strMsg = "";

            var inputListDatas = _inputListService.List().Where(u => InputListIDs.Contains(u.ID)).ToList();//入库单信息
            allCount = inputListDatas.Count;
            inputListDatas = inputListDatas.Where(u => (u.State ?? 0) != 1).ToList();

            if (allCount > inputListDatas.Count)
            {
                strMsg = "其中存在已经提交的入库单信息，不能删除";
            }
            else if (inputListDatas.Count > 0)
            {
                foreach (var item in inputListDatas)
                {
                    if (_inputListService.Delete(item))
                    {
                        successCount++;
                        var detailData = _inputDetailService.List().Where(u => u.InputListID == item.ID).ToList();
                        _inputDetailService.DeleteByList(detailData);
                    }
                }
            }
            else
            {
                strMsg = "删除失败";
            }

            return Json(new
            {
                isSuccess = successCount > 0 ? true : false,
                successCount = successCount,
                messsage = strMsg
            });

        }
        #endregion

        #region 退库管理
        //耗材退库列表页
        public ActionResult CancellingIndex()
        {
            return View();
        }
        //耗材退库编辑页
        public ActionResult CancellingEdit(Guid? CancellingID, string type = "look")
        {
            ViewData["workflowName"] = FlowInstance.WorkflowType.ConsumingCancelling;
            ViewData["type"] = type;
            ViewData["CancellingID"] = CancellingID.HasValue ? CancellingID.ToString() : null;
            ViewData["RealName"] = this.WorkContext.CurrentUser.RealName;
            ViewData["DeptName"] = this.WorkContext.CurrentUser.Dept.DpName;
            ViewData["Mobile"] = this.WorkContext.CurrentUser.Mobile;
            ViewData["NewTitle"] = this.WorkContext.CurrentUser.RealName + "的耗材退库申请" + DateTime.Now.ToString("(yyyy-MM-dd)");
            return View();
        }
        //耗材退库信息——流程中使用
        public ActionResult CancellingInfo_WF(Guid? CancellingID)
        {
            ViewData["CancellingID"] = CancellingID.HasValue ? CancellingID.ToString() : null;
            return View();
        }

        /// <summary>
        /// 获取退库单信息数据列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数据量</param>
        /// <param name="queryBuilder">查询条件</param>
        /// <returns></returns>
        public ActionResult GetListData_CancellingList(int pageIndex = 1, int pageSize = 5, ConsumableCancellingQueryBuilder queryBuilder = null, bool isManager = false)
        {
            if (!isManager)
            {
                queryBuilder.AppPerson = this.WorkContext.CurrentUser.UserId.ToString();
            }
            int count = 0;
            var modelList = _cancellingService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (Consumable_Cancelling)u).ToList();

            List<object> listResult = new List<object>();
            foreach (var item in modelList)
            {
                listResult.Add(new
                {
                    item.ID,
                    item.WorkflowInstanceId,
                    item.Series,
                    item.ApplyTime,
                    item.AppDept,
                    AppDept_Text = CommonFunction.getDeptNamesByIDs(item.AppDept),
                    item.AppPerson,
                    AppPerson_Text = CommonFunction.getUserRealNamesByIDs(item.AppPerson),
                    item.Mobile,
                    item.Title,
                    item.Content,
                    item.State,
                    WF_StateText = CommonFunction.GetFlowStateText(item.Tracking_Workflow),
                    WF_CurActivityName = _tracking_WorkflowService.GetCurrentActivityNames(item.WorkflowInstanceId ?? Guid.Parse("00000000-0000-0000-0000-000000000000")).FirstOrDefault()
                });
            }

            return Json(new { items = listResult, count = count }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 删除退库单
        /// </summary>
        /// <param name="InputListID">退库单ID</param>
        /// <returns></returns>
        public ActionResult Delete_Cancellings(Guid[] IDs)
        {
            bool isSuccess = false;
            int allCount = 0;
            int successCount = 0;
            string strMsg = "";

            var cancellingDatas = _cancellingService.List().Where(u => IDs.Contains(u.ID)).ToList();//退库单信息
            allCount = cancellingDatas.Count;

            if (cancellingDatas.Where(u => u.State == 1).Count() > 0)
            {
                strMsg = "其中存在已提交的记录，不能删除";
            }
            else if (cancellingDatas.Count > 0)
            {
                if (_cancellingService.DeleteByList(cancellingDatas))
                {
                    successCount = cancellingDatas.Count;
                    foreach (var item in cancellingDatas)
                    {
                        var temp = _cancellingDetailService.List().Where(u => u.ApplyID == item.ID).ToList();
                        _cancellingDetailService.DeleteByList(temp);
                    }
                }
            }
            else
            {
                strMsg = "删除失败";
            }

            return Json(new
            {
                isSuccess = successCount > 0 ? true : false,
                successCount = successCount,
                messsage = strMsg
            });

        }

        /// <summary>
        /// 保存退库单
        /// </summary>
        /// <param name="inputList">退库单信息</param>
        /// <param name="inputDetails">退库单明细</param>
        /// <returns></returns>
        public ActionResult Save_Cancelling(Consumable_Cancelling dataObj, List<Consumable_Amount> details = null)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过
            if (dataObj.Title == null || string.IsNullOrEmpty(dataObj.Title.Trim()))
                tip = "标题不能为空";
            else if (dataObj.Content == null || string.IsNullOrEmpty(dataObj.Content.Trim()))
                tip = "退库原因不能为空";
            else
            {
                isValid = true;
                dataObj.Title = dataObj.Title.Trim();
                dataObj.Content = dataObj.Content.Trim();
            }

            if (!isValid)
            {
                result.IsSuccess = false;
                result.Message = tip;
                return Json(result);
            }
            #endregion

            if (dataObj.ID == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {//新增
                dataObj.ID = Guid.NewGuid();
                dataObj.State = 0;
                dataObj.AppPerson = this.WorkContext.CurrentUser.UserId.ToString();
                dataObj.AppDept = this.WorkContext.CurrentUser.DpId;
                dataObj.Mobile = this.WorkContext.CurrentUser.Mobile;
                result.IsSuccess = _cancellingService.Insert(dataObj);
            }
            else
            {//编辑
                result.IsSuccess = _cancellingService.Update(dataObj);
            }

            //退库单信息保存成功，则继续保存更新退库单明细
            if (result.IsSuccess)
            {
                int count = 0;
                //先删除原来的明细数据
                var oldData = _cancellingDetailService.GetForPaging(out count, new { ApplyID = dataObj.ID }).Select(u => (Consumable_CancellingDetail)u).ToList();
                _cancellingDetailService.DeleteByList(oldData);
                if (details != null && details.Count > 0)
                {
                    List<Consumable_CancellingDetail> listDetail = new List<Consumable_CancellingDetail>();
                    foreach (var item in details)
                    {
                        Consumable_CancellingDetail tempData = new Consumable_CancellingDetail();
                        tempData.ID = Guid.NewGuid();
                        tempData.ApplyID = dataObj.ID;
                        tempData.ConsumableID = item.ConsumableID;
                        tempData.CancelNumber = item.Amount;
                        listDetail.Add(tempData);
                    }
                    if (listDetail.Count > 0)
                        result.IsSuccess = _cancellingDetailService.InsertByList(listDetail);
                    else
                        result.IsSuccess = true;
                    if (!result.IsSuccess)
                    {
                        result.Message = "保存退库单耗材明细信息失败";
                    }
                }
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "保存退库单信息失败";
            }

            if (result.IsSuccess)
                result.Message = dataObj.ID.ToString();
            return Json(result);
        }

        /// <summary>
        /// 根据ID获取退库单信息数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetDataByID_Cancelling(Guid? id)
        {
            id = id ?? Guid.Parse("00000000-0000-0000-0000-000000000000");
            var cancellingData = _cancellingService.FindById(id);
            int count = 0;
            List<Consumable_CancellingDetail> detailDatas = new List<Consumable_CancellingDetail>();
            Users sumbitUser = new Users();
            if (cancellingData != null)
            {
                detailDatas = _cancellingDetailService.List().Where(u => u.ApplyID == cancellingData.ID).ToList();
                Guid userID = Guid.Parse("00000000-0000-0000-0000-000000000000");
                if (Guid.TryParse(cancellingData.AppPerson, out userID))
                    sumbitUser = new SysUserService().FindById(userID);
            }
            return Json(new
            {
                cancellingData = cancellingData,
                detailDatas = detailDatas,
                sumbitUser = sumbitUser == null ? null : new { RealName = sumbitUser.RealName, Mobile = sumbitUser.Mobile, DeptName = sumbitUser.Dept.DpFullName }
            }
                , JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取退库单明细数据列表
        /// </summary>
        /// <param name="CancellingID">退库单ID</param>
        /// <param name="OnlyInput">是否只输出退库的信息</param>
        /// <returns></returns>
        public ActionResult GetListData_CancellingDetail(Guid? CancellingID, bool OnlyInput = true)
        {
            CancellingID = CancellingID ?? Guid.Parse("00000000-0000-0000-0000-000000000000");
            int count = 0;
            //获取退库明细数量
            var detailList = _cancellingDetailService.GetForPaging(out count, new { ApplyID = CancellingID }).Select(u => (Consumable_CancellingDetail)u).ToList();
            List<int> detail_ConsumableList = detailList.Select(u => u.ConsumableID.Value).ToList();

            //获取耗材信息数据
            List<Consumable> modelList = new List<Consumable>();
            if (OnlyInput)
            {
                modelList = _consumableService.List().Where(u => detail_ConsumableList.Contains(u.ID)).ToList();
            }
            else
            {
                modelList = _consumableService.List().ToList();
            }

            List<object> listResult = new List<object>();
            foreach (var item in modelList)
            {
                int countTemp = 0;
                countTemp = detailList.Where(u => u.ConsumableID == item.ID).FirstOrDefault()?.CancelNumber ?? 0;
                listResult.Add(new
                {
                    item.ID,
                    item.Type,
                    item.Name,
                    item.Trademark,
                    item.Model,
                    item.Equipment,
                    item.Unit,
                    item.IsValue,
                    Count = countTemp
                });
            }
            return Json(new { items = listResult, count = count });
        }

        public ActionResult CheckConsumableAmount_ForCancelling(Guid? id)
        {
            SystemResult result = new SystemResult() { IsSuccess = false, Message = "" };
            id = id ?? Guid.Parse("00000000-0000-0000-0000-000000000000");
            var curData = _cancellingService.FindById(id);
            if (curData == null || curData.ID == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                result.IsSuccess = false;
                result.Message = "该退库申请单信息不存在";
                return Json(result);
            }
            int count = 0;
            //获取当前退库单的明细
            var detailData = _cancellingDetailService.GetForPaging(out count, new { ApplyID = curData.ID }).Select(u => (Consumable_CancellingDetail)u).ToList();
            int detailDataAmount = detailData.Sum(u => u.CancelNumber ?? 0);
            if (detailDataAmount <= 0)
            {
                result.IsSuccess = false;
                result.Message = "该退库单中的退库总量小于1，请正确填写信息";
                return Json(result);
            }
            List<string> overConsumable = new List<string>();
            foreach (var item in detailData)
            {
                var curConsumable = _consumableService.FindById(item.ConsumableID);
                if (curConsumable.Amount < (item.CancelNumber ?? 0))//退库申请数量大于库存量
                {
                    overConsumable.Add(curConsumable.Name);
                }
            }
            if (overConsumable.Count > 0)
            {
                result.IsSuccess = false;
                result.Message = string.Format("该退库单中“{0}”的退库数量超出库存量", string.Join(",", overConsumable));
                return Json(result);
            }
            result.IsSuccess = true;
            result.Message = "";
            return Json(result);
        }

        /// <summary>
        /// 提交退库单(发起流程工单)
        /// </summary>
        /// <param name="CancellingID">退库单ID</param>
        /// <returns></returns>
        public ActionResult Sumbit_Cancelling(Guid cancellingID, string nextActivity = null, string nextActors = null, string nextCC = null)
        {
            SystemResult result = new SystemResult() { IsSuccess = false, Message = "" };

            var curData = _cancellingService.FindById(cancellingID);
            if (curData == null || curData.ID == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                result.IsSuccess = false;
                result.Message = "该退库申请单信息不存在";
                return Json(result);
            }

            int count = 0;
            //获取当前退库单的明细
            var detailData = _cancellingDetailService.GetForPaging(out count, new { ApplyID = curData.ID }).Select(u => (Consumable_CancellingDetail)u).ToList();
            int detailDataAmount = detailData.Sum(u => u.CancelNumber ?? 0);
            if (detailDataAmount <= 0)
            {
                result.IsSuccess = false;
                result.Message = "该退库单中的退库总量小于1，请正确填写信息";
                return Json(result);
            }

            #region 判断退库数量是否大于库存量
            List<string> overConsumable = new List<string>();
            foreach (var item in detailData)
            {
                var curConsumable = _consumableService.FindById(item.ConsumableID);
                if (curConsumable.Amount < (item.CancelNumber ?? 0))//退库申请数量大于库存量
                {
                    overConsumable.Add(curConsumable.Name);
                }
            }
            if (overConsumable.Count > 0)
            {
                result.IsSuccess = false;
                result.Message = string.Format("该退库单中“{0}”的退库数量超出库存量", string.Join(",", overConsumable));
                return Json(result);
            }
            #endregion

            result = Sumbit_Cancelling_WF(curData, nextActivity, nextActors);

            if (result.IsSuccess && !string.IsNullOrEmpty(nextCC))
            {//提交成功后抄送
                curData = _cancellingService.FindById(cancellingID);
                CommonFunction.PendingData(curData.WorkflowInstanceId.Value, nextCC);//抄送
            }

            return Json(result);
        }

        #region 耗材退库申请的流程处理
        /// <summary>
        /// 提交退库申请
        /// </summary>
        public SystemResult Sumbit_Cancelling_WF(Consumable_Cancelling curData, string nextActivity, string nextActors)
        {
            if (string.IsNullOrEmpty(nextActivity) || string.IsNullOrEmpty(nextActors))
            {
                CommonFunction.GetFirstOperatorInfoAfterSumbit(FlowInstance.WorkflowType.ConsumingCancelling, this.WorkContext.CurrentUser.UserName, out nextActivity, out nextActors);
            }

            SystemResult result = new SystemResult() { IsSuccess = false };
            string objectXML = "";

            if (!curData.WorkflowInstanceId.HasValue)
            {//第一次提交
                objectXML = "<Root action=\"Start\" username=\"{0}\" keypassword=\"123\">"
                                + "<parameter>"
                                    + "<item name=\"workflowId\" value=\"{1}\"/>"
                                    + "<item name=\"workflowTitle\" value=\"{2}\"/>"
                                    + "<item name=\"workflowProperties\">"
                                        + "<workflow secrecy=\"0\" urgency=\"0\" importance=\"0\" expectFinishedTime=\"{6}\">"
                                            + "<fields>"
                                                + "<item name=\"F1\" value=\"{3}\"></item>"
                                            + "</fields>"
                                            + "<nextactivities>"
                                                + "<item name=\"{4}\" actors=\"{5}\"/>"
                                            + "</nextactivities>"
                                        + "</workflow>"
                                    + "</item>"
                                + "</parameter>"
                            + "</Root>";
                objectXML = string.Format(objectXML, this.WorkContext.CurrentUser.UserName, FlowInstance.WorkflowType.ConsumingCancelling, curData.Title, curData.ID.ToString(), nextActivity, nextActors, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd"));

            }
            else
            {//退回提交
                ITracking_TodoService tempService = new Tracking_TodoService();
                Tracking_Todo tempActivity = new Tracking_Todo();
                Guid guid = curData.WorkflowInstanceId.Value;
                tempActivity = new Tracking_TodoService().Entitys.Where(u => u.WorkflowInstanceId == guid).FirstOrDefault();//当前节点实例
                objectXML = "<Root action=\"execute\" username=\"{0}\" keypassword=\"123\">"
                                + "<parameter>"
                                    + "<item name=\"activityInstanceId\" value=\"{1}\"/>"
                                    + "<item name=\"command\" value=\"Approve\"/>"
                                    + "<item name=\"workflowTitle\" value=\"{2}\"/>"
                                    + "<item name=\"workflowProperties\">"
                                        + "<workflow secrecy=\"0\" urgency=\"0\" importance=\"0\" expectFinishedTime=\"{6}\">"
                                            + "<fields>"
                                                + "<item name=\"F1\" value=\"{3}\"></item>"
                                            + "</fields>"
                                            + "<nextactivities>"
                                                + "<item name=\"{4}\" actors=\"{5}\"/>"
                                            + "</nextactivities>"
                                        + "</workflow>"
                                    + "</item>"
                                + "</parameter>"
                            + "</Root>";
                objectXML = string.Format(objectXML, this.WorkContext.CurrentUser.UserName, tempActivity.ActivityInstanceId.ToString(), curData.Title, curData.ID.ToString(), nextActivity, nextActors, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd"));

            }

            string[] args = new string[4];
            args[0] = FlowInstance.Workflow_SystemID;
            args[1] = FlowInstance.Workflow_SystemAcount;
            args[2] = FlowInstance.Workflow_SystemPwd;
            args[3] = objectXML;

            string resultXml = WebServicesHelper.InvokeWebService(FlowInstance.Workflow_SystemUrl, FlowInstance.MethodName.ManageWorkflow, args).ToString();
            System.Xml.XmlDocument xdoc = new System.Xml.XmlDocument();
            xdoc.LoadXml(resultXml);
            System.Xml.XmlNode resutNode = xdoc.SelectSingleNode("Result");
            string success = resutNode.Attributes["Success"].Value;
            string errmsg = resutNode.Attributes["ErrorMsg"].Value;
            string strWorkflowInstanceId = "";
            string strActivityinstanceId = "";
            int intSuccess = 0;
            int.TryParse(success, out intSuccess);
            if (intSuccess > 0)
            {
                if (!curData.WorkflowInstanceId.HasValue)
                {//第一次提交
                    System.Xml.XmlNodeList xmlList = xdoc.SelectNodes("Result/item/start/item");
                    for (int i = 0; i < xmlList.Count; i++)
                    {
                        switch (xmlList[i].Attributes["name"].Value)
                        {
                            case "WorkflowInstanceId": strWorkflowInstanceId = xmlList[i].Attributes["value"].Value; break;
                            case "ActivityinstanceId": strActivityinstanceId = xmlList[i].Attributes["value"].Value; break;
                            default: break;
                        }
                    }

                    var workFlow = _tracking_WorkflowService.FindById(Guid.Parse(strWorkflowInstanceId));
                    curData.State = 1;
                    curData.WorkflowInstanceId = Guid.Parse(strWorkflowInstanceId);
                    curData.Series = workFlow.SheetId;
                    curData.ApplyTime = DateTime.Now;

                    result.IsSuccess = _cancellingService.Update(curData);
                }
                else
                {//退回提交
                    result.IsSuccess = true;
                }
            }
            else
            {
                result.IsSuccess = false;
                result.Message = errmsg;
            }
            return result;
        }
        #endregion

        #endregion

        #region 调平管理
        //耗材调平列表页
        public ActionResult LevellingIndex()
        {
            return View();
        }
        //耗材调平编辑页
        public ActionResult LevellingEdit(Guid? LevellingID, string type = "look")
        {
            ViewData["workflowName"] = FlowInstance.WorkflowType.ConsumingLevelling;
            ViewData["type"] = type;
            ViewData["LevellingID"] = LevellingID.HasValue ? LevellingID.ToString() : null;
            ViewData["RealName"] = this.WorkContext.CurrentUser.RealName;
            ViewData["DeptName"] = this.WorkContext.CurrentUser.Dept.DpName;
            ViewData["Mobile"] = this.WorkContext.CurrentUser.Mobile;
            ViewData["NewTitle"] = this.WorkContext.CurrentUser.RealName + "的耗材调平申请" + DateTime.Now.ToString("(yyyy-MM-dd)");
            return View();
        }
        //耗材调平信息——流程中使用
        public ActionResult LevellingInfo_WF(Guid? LevellingID)
        {
            ViewData["LevellingID"] = LevellingID.HasValue ? LevellingID.ToString() : null;
            return View();
        }

        /// <summary>
        /// 获取调平单信息数据列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数据量</param>
        /// <param name="queryBuilder">查询条件</param>
        /// <returns></returns>
        public ActionResult GetListData_LevellingList(int pageIndex = 1, int pageSize = 5, ConsumableLevellingQueryBuilder queryBuilder = null, bool isManager = false)
        {
            if (!isManager)
            {
                queryBuilder.AppPerson = this.WorkContext.CurrentUser.UserId;
            }
            int count = 0;
            var modelList = _levellingService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (Consumable_Levelling)u).ToList();

            List<object> listResult = new List<object>();
            foreach (var item in modelList)
            {
                listResult.Add(new
                {
                    item.ID,
                    item.WorkflowInstanceId,
                    item.Series,
                    item.ApplyTime,
                    item.AppDept,
                    AppDept_Text = CommonFunction.getDeptNamesByIDs(item.AppDept),
                    item.AppPerson,
                    AppPerson_Text = CommonFunction.getUserRealNamesByIDs(item.AppPerson?.ToString()),
                    item.Mobile,
                    item.Title,
                    item.Content,
                    item.State,
                    WF_StateText = CommonFunction.GetFlowStateText(item.Tracking_Workflow),
                    WF_CurActivityName = _tracking_WorkflowService.GetCurrentActivityNames(item.WorkflowInstanceId ?? Guid.Parse("00000000-0000-0000-0000-000000000000")).FirstOrDefault()
                });
            }

            return Json(new { items = listResult, count = count }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 删除调平单
        /// </summary>
        /// <param name="IDs">退库单ID</param>
        /// <returns></returns>
        public ActionResult Delete_Levellings(Guid[] IDs)
        {
            bool isSuccess = false;
            int allCount = 0;
            int successCount = 0;
            string strMsg = "";

            var levellingDatas = _levellingService.List().Where(u => IDs.Contains(u.ID)).ToList();//调平单信息
            allCount = levellingDatas.Count;

            if (levellingDatas.Where(u => u.State == 1).Count() > 0)
            {
                strMsg = "其中存在已提交的记录，不能删除";
            }
            else if (levellingDatas.Count > 0)
            {
                if (_levellingService.DeleteByList(levellingDatas))
                {
                    successCount = levellingDatas.Count;
                    foreach (var item in levellingDatas)
                    {
                        var temp = _levellingDetailService.List().Where(u => u.ApplyID == item.ID).ToList();
                        _levellingDetailService.DeleteByList(temp);
                    }
                }
            }
            else
            {
                strMsg = "删除失败";
            }

            return Json(new
            {
                isSuccess = successCount > 0 ? true : false,
                successCount = successCount,
                messsage = strMsg
            });

        }

        /// <summary>
        /// 保存调平单
        /// </summary>
        /// <param name="dataObj">调平单信息</param>
        /// <param name="details">调平单明细</param>
        /// <returns></returns>
        public ActionResult Save_Levelling(Consumable_Levelling dataObj, List<Consumable_Amount> details = null)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过
            if (dataObj.Title == null || string.IsNullOrEmpty(dataObj.Title.Trim()))
                tip = "标题不能为空";
            else if (dataObj.Content == null || string.IsNullOrEmpty(dataObj.Content.Trim()))
                tip = "调平原因不能为空";
            else
            {
                isValid = true;
                dataObj.Title = dataObj.Title.Trim();
                dataObj.Content = dataObj.Content.Trim();
            }

            if (!isValid)
            {
                result.IsSuccess = false;
                result.Message = tip;
                return Json(result);
            }
            #endregion

            if (dataObj.ID == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {//新增
                dataObj.ID = Guid.NewGuid();
                dataObj.State = 0;
                dataObj.AppPerson = this.WorkContext.CurrentUser.UserId;
                dataObj.AppDept = this.WorkContext.CurrentUser.DpId;
                dataObj.Mobile = this.WorkContext.CurrentUser.Mobile;
                result.IsSuccess = _levellingService.Insert(dataObj);
            }
            else
            {//编辑
                result.IsSuccess = _levellingService.Update(dataObj);
            }

            //调平单信息保存成功，则继续保存更新调平单明细
            if (result.IsSuccess)
            {
                int count = 0;
                //先删除原来的明细数据
                var oldData = _levellingDetailService.GetForPaging(out count, new { ApplyID = dataObj.ID }).Select(u => (Consumable_LevellingDetail)u).ToList();
                _levellingDetailService.DeleteByList(oldData);
                if (details != null && details.Count > 0)
                {
                    List<Consumable_LevellingDetail> listDetail = new List<Consumable_LevellingDetail>();
                    foreach (var item in details)
                    {
                        Consumable_LevellingDetail tempData = new Consumable_LevellingDetail();
                        tempData.ID = Guid.NewGuid();
                        tempData.ApplyID = dataObj.ID;
                        tempData.ConsumableID = item.ConsumableID;
                        tempData.Amount = item.Amount;
                        listDetail.Add(tempData);
                    }
                    if (listDetail.Count > 0)
                        result.IsSuccess = _levellingDetailService.InsertByList(listDetail);
                    else
                        result.IsSuccess = true;
                    if (!result.IsSuccess)
                    {
                        result.Message = "保存调平单耗材明细信息失败";
                    }
                }
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "保存调平单信息失败";
            }

            if (result.IsSuccess)
                result.Message = dataObj.ID.ToString();
            return Json(result);
        }

        /// <summary>
        /// 根据ID获取调平单信息数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetDataByID_Levelling(Guid? id)
        {
            id = id ?? Guid.Parse("00000000-0000-0000-0000-000000000000");
            var levellingData = _levellingService.FindById(id);
            int count = 0;
            List<Consumable_LevellingDetail> detailDatas = new List<Consumable_LevellingDetail>();
            Users sumbitUser = new Users();
            if (levellingData != null)
            {
                detailDatas = _levellingDetailService.List().Where(u => u.ApplyID == levellingData.ID).ToList();
                Guid userID = levellingData.AppPerson ?? Guid.Parse("00000000-0000-0000-0000-000000000000");
                sumbitUser = new SysUserService().FindById(userID);
            }
            return Json(new
            {
                levellingData = levellingData,
                detailDatas = detailDatas,
                sumbitUser = sumbitUser == null ? null : new { RealName = sumbitUser.RealName, Mobile = sumbitUser.Mobile, DeptName = sumbitUser.Dept.DpFullName }
            }
                , JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取调平单明细数据列表
        /// </summary>
        /// <param name="LevellingID">退库单ID</param>
        /// <param name="OnlyInput">是否只输出退库的信息</param>
        /// <returns></returns>
        public ActionResult GetListData_LevellingDetail(Guid? LevellingID, bool OnlyInput = true)
        {
            LevellingID = LevellingID ?? Guid.Parse("00000000-0000-0000-0000-000000000000");
            int count = 0;
            //获取调平明细数量
            var detailList = _levellingDetailService.GetForPaging(out count, new { ApplyID = LevellingID }).Select(u => (Consumable_LevellingDetail)u).ToList();
            List<int> detail_ConsumableList = detailList.Select(u => u.ConsumableID.Value).ToList();

            //获取耗材信息数据
            List<Consumable> modelList = new List<Consumable>();
            if (OnlyInput)
            {
                modelList = _consumableService.List().Where(u => detail_ConsumableList.Contains(u.ID)).ToList();
            }
            else
            {
                modelList = _consumableService.List().ToList();
            }

            List<object> listResult = new List<object>();
            foreach (var item in modelList)
            {
                int countTemp = 0;
                countTemp = detailList.Where(u => u.ConsumableID == item.ID).FirstOrDefault()?.Amount ?? 0;
                listResult.Add(new
                {
                    item.ID,
                    item.Type,
                    item.Name,
                    item.Trademark,
                    item.Model,
                    item.Equipment,
                    item.Unit,
                    item.IsValue,
                    Count = countTemp
                });
            }
            return Json(new { items = listResult, count = count });
        }

        /// <summary>
        /// 提交调平单(发起流程工单)
        /// </summary>
        /// <param name="LevellingID">退库单ID</param>
        /// <returns></returns>
        public ActionResult Sumbit_Levelling(Guid levellingID, string nextActivity = null, string nextActors = null, string nextCC = null)
        {
            SystemResult result = new SystemResult() { IsSuccess = false, Message = "" };

            var curData = _levellingService.FindById(levellingID);
            if (curData == null || curData.ID == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                result.IsSuccess = false;
                result.Message = "该调平申请单信息不存在";
                return Json(result);
            }

            int count = 0;
            //获取当前调平单的明细
            var detailData = _levellingDetailService.GetForPaging(out count, new { ApplyID = curData.ID }).Select(u => (Consumable_LevellingDetail)u).ToList();

            if (detailData.Count <= 0)
            {
                result.IsSuccess = false;
                result.Message = "该调平单中的耗材明细为空，请正确填写信息";
                return Json(result);
            }

            result = Sumbit_Levelling_WF(curData, nextActivity, nextActors);
            if (result.IsSuccess && !string.IsNullOrEmpty(nextCC))
            {//提交成功后抄送
                curData = _levellingService.FindById(curData.ID);
                CommonFunction.PendingData(curData.WorkflowInstanceId.Value, nextCC);//抄送
            }

            return Json(result);
        }

        #region 耗材调平申请的流程处理
        /// <summary>
        /// 提交调平申请
        /// </summary>
        public SystemResult Sumbit_Levelling_WF(Consumable_Levelling curData, string nextActivity, string nextActors)
        {
            if (string.IsNullOrEmpty(nextActivity) || string.IsNullOrEmpty(nextActors))
            {
                CommonFunction.GetFirstOperatorInfoAfterSumbit(FlowInstance.WorkflowType.ConsumingLevelling, this.WorkContext.CurrentUser.UserName, out nextActivity, out nextActors);
            }

            SystemResult result = new SystemResult() { IsSuccess = false };
            string objectXML = "";

            if (!curData.WorkflowInstanceId.HasValue)
            {//第一次提交
                objectXML = "<Root action=\"Start\" username=\"{0}\" keypassword=\"123\">"
                                + "<parameter>"
                                    + "<item name=\"workflowId\" value=\"{1}\"/>"
                                    + "<item name=\"workflowTitle\" value=\"{2}\"/>"
                                    + "<item name=\"workflowProperties\">"
                                        + "<workflow secrecy=\"0\" urgency=\"0\" importance=\"0\" expectFinishedTime=\"{6}\">"
                                            + "<fields>"
                                                + "<item name=\"F1\" value=\"{3}\"></item>"
                                            + "</fields>"
                                            + "<nextactivities>"
                                                + "<item name=\"{4}\" actors=\"{5}\"/>"
                                            + "</nextactivities>"
                                        + "</workflow>"
                                    + "</item>"
                                + "</parameter>"
                            + "</Root>";
                objectXML = string.Format(objectXML, this.WorkContext.CurrentUser.UserName, FlowInstance.WorkflowType.ConsumingLevelling, curData.Title, curData.ID.ToString(), nextActivity, nextActors, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd"));

            }
            else
            {//退回提交
                ITracking_TodoService tempService = new Tracking_TodoService();
                Tracking_Todo tempActivity = new Tracking_Todo();
                Guid guid = curData.WorkflowInstanceId.Value;
                tempActivity = new Tracking_TodoService().Entitys.Where(u => u.WorkflowInstanceId == guid).FirstOrDefault();//当前节点实例
                objectXML = "<Root action=\"execute\" username=\"{0}\" keypassword=\"123\">"
                                + "<parameter>"
                                    + "<item name=\"activityInstanceId\" value=\"{1}\"/>"
                                    + "<item name=\"command\" value=\"Approve\"/>"
                                    + "<item name=\"workflowTitle\" value=\"{2}\"/>"
                                    + "<item name=\"workflowProperties\">"
                                        + "<workflow secrecy=\"0\" urgency=\"0\" importance=\"0\" expectFinishedTime=\"{6}\">"
                                            + "<fields>"
                                                + "<item name=\"F1\" value=\"{3}\"></item>"
                                            + "</fields>"
                                            + "<nextactivities>"
                                                + "<item name=\"{4}\" actors=\"{5}\"/>"
                                            + "</nextactivities>"
                                        + "</workflow>"
                                    + "</item>"
                                + "</parameter>"
                            + "</Root>";
                objectXML = string.Format(objectXML, this.WorkContext.CurrentUser.UserName, tempActivity.ActivityInstanceId.ToString(), curData.Title, curData.ID.ToString(), nextActivity, nextActors, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd"));

            }

            string[] args = new string[4];
            args[0] = FlowInstance.Workflow_SystemID;
            args[1] = FlowInstance.Workflow_SystemAcount;
            args[2] = FlowInstance.Workflow_SystemPwd;
            args[3] = objectXML;

            string resultXml = WebServicesHelper.InvokeWebService(FlowInstance.Workflow_SystemUrl, FlowInstance.MethodName.ManageWorkflow, args).ToString();
            System.Xml.XmlDocument xdoc = new System.Xml.XmlDocument();
            xdoc.LoadXml(resultXml);
            System.Xml.XmlNode resutNode = xdoc.SelectSingleNode("Result");
            string success = resutNode.Attributes["Success"].Value;
            string errmsg = resutNode.Attributes["ErrorMsg"].Value;
            string strWorkflowInstanceId = "";
            string strActivityinstanceId = "";
            int intSuccess = 0;
            int.TryParse(success, out intSuccess);
            if (intSuccess > 0)
            {
                if (!curData.WorkflowInstanceId.HasValue)
                {//第一次提交
                    System.Xml.XmlNodeList xmlList = xdoc.SelectNodes("Result/item/start/item");
                    for (int i = 0; i < xmlList.Count; i++)
                    {
                        switch (xmlList[i].Attributes["name"].Value)
                        {
                            case "WorkflowInstanceId": strWorkflowInstanceId = xmlList[i].Attributes["value"].Value; break;
                            case "ActivityinstanceId": strActivityinstanceId = xmlList[i].Attributes["value"].Value; break;
                            default: break;
                        }
                    }

                    var workFlow = _tracking_WorkflowService.FindById(Guid.Parse(strWorkflowInstanceId));
                    curData.State = 1;
                    curData.WorkflowInstanceId = Guid.Parse(strWorkflowInstanceId);
                    curData.Series = workFlow.SheetId;
                    curData.ApplyTime = DateTime.Now;

                    result.IsSuccess = _levellingService.Update(curData);
                }
                else
                {//退回提交
                    result.IsSuccess = true;
                }
            }
            else
            {
                result.IsSuccess = false;
                result.Message = errmsg;
            }

            return result;
        }
        #endregion

        #endregion

        #region 报废申请
        public ActionResult Consumable_ScrapList()
        {
            return View();
        }
        public ActionResult Consumable_ScrapView(Guid? Id)
        {
            ViewData["Id"] = Id;
            return View();
        }
        public ActionResult getApplyInfo()
        {
            object AppInfo = new object();
            if (this.WorkContext.CurrentUser != null)
            {
                AppInfo = new
                {
                    Applylicant = this.WorkContext.CurrentUser.UserId,
                    ApplyName = this.WorkContext.CurrentUser.RealName,
                    ApplyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    ApplyDpCode = this.WorkContext.CurrentUser.Dept.DpId,
                    DpName = this.WorkContext.CurrentUser.Dept.DpName,
                    Mobile = this.WorkContext.CurrentUser.Mobile,
                    // Series = "流程单号待生成",
                    Title = this.WorkContext.CurrentUser.RealName + "的报废申请(" + DateTime.Now.ToString("yyyy-MM-dd") + ")"
                };
            }
            return Json(AppInfo);
        }
        public ActionResult Consumable_ScrapApply(Guid? Id, string type = "look")
        {
            ViewData["workflowName"] = FlowInstance.WorkflowType.ConsumableScrap;
            ViewData["type"] = type;
            ViewData["Id"] = Id.HasValue ? Id.ToString() : null;
            ViewData["NewTitle"] = this.WorkContext.CurrentUser.RealName + "的报废退库申请" + DateTime.Now.ToString("(yyyy-MM-dd)");
            return View();
        }
        /// <summary>
        /// 报废申请列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="queryBuilder"></param>
        /// <param name="withDelData"></param>
        /// <returns></returns>
        public ActionResult GetListData_Consumable_Scrap(int pageIndex = 1, int pageSize = 5, ConsumableScrapQueryBuilder queryBuilder = null, bool withDelData = false)
        {
            int count = 0;
            var modelList = _consumable_ScrapService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);

            return Json(new { items = modelList, count = count });
        }
        /// <summary>
        /// 删除报废申请
        /// </summary>
        /// <param name="ConsumingIDs">报废申请ID</param>
        /// <returns></returns>
        public ActionResult Delete_Consumable_Scrap(Guid?[] ScrapIDs)
        {
            int allCount = 0;
            int successCount = 0;
            string strMsg = "";

            var ListDatas = _consumable_ScrapService.List().Where(u => ScrapIDs.Contains(u.ID)).ToList();//领用申请信息
            allCount = ListDatas.Count;
            ListDatas = ListDatas.Where(u => u.State != 1).ToList();

            if (allCount > ListDatas.Count)
            {
                strMsg = "其中存在已经提交的申请信息，不能删除";
            }
            else if (ListDatas.Count > 0)
            {
                foreach (var item in ListDatas)
                {
                    if (_consumable_ScrapService.Delete(item))
                    {
                        successCount++;
                        var detailData = _consumable_ScrapDetailService.List().Where(u => u.ApplyID == item.ID.ToString()).ToList();
                        _consumable_ScrapDetailService.DeleteByList(detailData);
                    }
                }
            }
            else
            {
                strMsg = "删除失败";
            }

            return Json(new
            {
                isSuccess = successCount > 0 ? true : false,
                successCount = successCount,
                messsage = strMsg
            });
        }
        /// <summary>
        /// 获取明细
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult Consumable_ScrapGetDetailByApplyID(Guid Id)
        {
            var modelList = _consumable_ScrapDetailService.List().Where(u => u.ApplyID == Id.ToString()).ToList();

            object DetailInfo = new object();
            List<object> det = new List<object>();
            foreach (var item in modelList)
            {
                var detail = _consumableService.List().Where(t => t.ID == item.ConsumableID).ToList();
                if (detail != null && detail.Count > 0)
                {
                    DetailInfo = new
                    {
                        ID = detail[0].ID,
                        Type = detail[0].Type,
                        Model = detail[0].Model,
                        Name = detail[0].Name,
                        IsValue = detail[0].IsValue,
                        Unit = detail[0].Unit,
                        Equipment = detail[0].Equipment,
                        Trademark = detail[0].Trademark,
                        count = item.ScrapNumber
                    };
                    det.Add(DetailInfo);
                }
            }

            return Json(new { items = det });
        }

        public ActionResult Consumable_ScrapGetByID(Guid Id)
        {
            var modelList = _consumable_ScrapService.FindById(Id);
            List<object> listResult = new List<object>();
            listResult.Add(new
            {
                modelList.ID,
                modelList.WorkflowInstanceId,
                modelList.Series,
                modelList.ApplyTime,
                DpName = CommonFunction.getDeptNamesByIDs(modelList.ApplyDpCode),
                ApplyName = CommonFunction.getUserRealNamesByIDs(modelList.Applicant),
                modelList.Mobile,
                modelList.Title,
                modelList.Content,
                modelList.State
            });

            return Json(new { items = listResult });
        }

        public ActionResult Save_Consumable_Scrap(Consumable_Scrap dataObj, List<Consumable_ScrapDetail> Details = null)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过
            if (dataObj.Title == null || string.IsNullOrEmpty(dataObj.Title.Trim()))
                tip = "标题不能为空";
            else if (dataObj.Content == null || string.IsNullOrEmpty(dataObj.Content.Trim()))
                tip = "报废原因不能为空";
            else
            {
                isValid = true;
                dataObj.Title = dataObj.Title.Trim();
                dataObj.Content = dataObj.Content.Trim();
            }

            if (!isValid)
            {
                result.IsSuccess = false;
                result.Message = tip;
                return Json(result);
            }
            #endregion

            if (dataObj.ID == null || dataObj.ID == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {//新增
                dataObj.ID = Guid.NewGuid();
                dataObj.State = 0;
                dataObj.Applicant = this.WorkContext.CurrentUser.UserId.ToString();
                dataObj.ApplyDpCode = this.WorkContext.CurrentUser.DpId;
                dataObj.Mobile = this.WorkContext.CurrentUser.Mobile;
                result.IsSuccess = _consumable_ScrapService.Insert(dataObj);
            }
            else
            {//编辑
                result.IsSuccess = _consumable_ScrapService.Update(dataObj);
            }

            //报废单信息保存成功，则继续保存更新退库单明细
            if (result.IsSuccess)
            {
                int count = 0;
                //先删除原来的明细数据
                var oldData = _consumable_ScrapDetailService.List().Where(u => u.ApplyID == dataObj.ID.ToString()).ToList();
                _consumable_ScrapDetailService.DeleteByList(oldData);
                if (Details != null && Details.Count > 0)
                {
                    List<Consumable_ScrapDetail> listDetail = new List<Consumable_ScrapDetail>();
                    foreach (var item in Details)
                    {
                        Consumable_ScrapDetail tempData = new Consumable_ScrapDetail();
                        tempData.ID = Guid.NewGuid();
                        tempData.ApplyID = dataObj.ID.ToString();
                        tempData.ConsumableID = item.ConsumableID;
                        tempData.ScrapNumber = item.ScrapNumber;
                        listDetail.Add(tempData);
                    }
                    if (listDetail.Count > 0)
                        result.IsSuccess = _consumable_ScrapDetailService.InsertByList(listDetail);
                    else
                        result.IsSuccess = true;
                    if (!result.IsSuccess)
                    {
                        result.Message = "保存报废耗材明细信息失败";
                    }
                }
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "保存报废单信息失败";
            }

            if (result.IsSuccess)
                result.Message = dataObj.ID.ToString();
            return Json(result);
        }

        /// <summary>
        /// 报废单(发起流程工单)
        /// </summary>
        /// <param name="CancellingID">退库单ID</param>
        /// <returns></returns>
        public ActionResult Sumbit_Consumable_Scrap(Guid ScrapID, string nextActivity = null, string nextActors = null, string nextCC = null)
        {
            SystemResult result = new SystemResult() { IsSuccess = false, Message = "" };

            var curData = _consumable_ScrapService.FindById(ScrapID);
            if (curData == null || curData.ID == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                result.IsSuccess = false;
                result.Message = "该报废申请单信息不存在";
                return Json(result);
            }

            int count = 0;
            //获取当前报废单的明细

            var detailData = _consumable_ScrapDetailService.List().Where(u => u.ApplyID == curData.ID.ToString()).ToList();
            int detailDataAmount = detailData.Sum(u => u.ScrapNumber ?? 0);
            if (detailDataAmount <= 0)
            {
                result.IsSuccess = false;
                result.Message = "该报废单中的报废总量小于1，请正确填写信息";
                return Json(result);
            }

            #region 判断退库数量是否大于库存量
            List<string> overConsumable = new List<string>();
            foreach (var item in detailData)
            {
                var curConsumable = _consumableService.FindById(item.ConsumableID);
                if (curConsumable.Amount < (item.ScrapNumber ?? 0))//报废申请数量大于库存量
                {
                    overConsumable.Add(curConsumable.Name);
                }
            }
            if (overConsumable.Count > 0)
            {
                result.IsSuccess = false;
                result.Message = string.Format("该报废单中“{0}”的报废数量超出库存量", string.Join(",", overConsumable));
                return Json(result);
            }
            #endregion

            result = Sumbit_Scrap_WF(curData, nextActivity, nextActors);
            if (result.IsSuccess && !string.IsNullOrEmpty(nextCC))
            {//提交成功后抄送
                curData = _consumable_ScrapService.FindById(ScrapID);
                CommonFunction.PendingData(curData.WorkflowInstanceId.Value, nextCC);//抄送
            }
            return Json(result);
        }

        #region 耗材报废申请的流程处理
        /// <summary>
        /// 提交报废申请
        /// </summary>
        public SystemResult Sumbit_Scrap_WF(Consumable_Scrap curData, string nextActivity, string nextActors)
        {
            if (string.IsNullOrEmpty(nextActivity) || string.IsNullOrEmpty(nextActors))
            {
                CommonFunction.GetFirstOperatorInfoAfterSumbit(FlowInstance.WorkflowType.ConsumingCancelling, this.WorkContext.CurrentUser.UserName, out nextActivity, out nextActors);
            }

            SystemResult result = new SystemResult() { IsSuccess = false };
            string objectXML = "";

            if (!curData.WorkflowInstanceId.HasValue)
            {//第一次提交
                objectXML = "<Root action=\"Start\" username=\"{0}\" keypassword=\"123\">"
                                + "<parameter>"
                                    + "<item name=\"workflowId\" value=\"{1}\"/>"
                                    + "<item name=\"workflowTitle\" value=\"{2}\"/>"
                                    + "<item name=\"workflowProperties\">"
                                        + "<workflow secrecy=\"0\" urgency=\"0\" importance=\"0\" expectFinishedTime=\"{6}\">"
                                            + "<fields>"
                                                + "<item name=\"F1\" value=\"{3}\"></item>"
                                            + "</fields>"
                                            + "<nextactivities>"
                                                + "<item name=\"{4}\" actors=\"{5}\"/>"
                                            + "</nextactivities>"
                                        + "</workflow>"
                                    + "</item>"
                                + "</parameter>"
                            + "</Root>";
                objectXML = string.Format(objectXML, this.WorkContext.CurrentUser.UserName, FlowInstance.WorkflowType.ConsumableScrap, curData.Title, curData.ID.ToString(), nextActivity, nextActors, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd"));

            }
            else
            {//退回提交
                ITracking_TodoService tempService = new Tracking_TodoService();
                Tracking_Todo tempActivity = new Tracking_Todo();
                Guid guid = curData.WorkflowInstanceId.Value;
                tempActivity = new Tracking_TodoService().Entitys.Where(u => u.WorkflowInstanceId == guid).FirstOrDefault();//当前节点实例
                objectXML = "<Root action=\"execute\" username=\"{0}\" keypassword=\"123\">"
                                + "<parameter>"
                                    + "<item name=\"activityInstanceId\" value=\"{1}\"/>"
                                    + "<item name=\"command\" value=\"Approve\"/>"
                                    + "<item name=\"workflowTitle\" value=\"{2}\"/>"
                                    + "<item name=\"workflowProperties\">"
                                        + "<workflow secrecy=\"0\" urgency=\"0\" importance=\"0\" expectFinishedTime=\"{6}\">"
                                            + "<fields>"
                                                + "<item name=\"F1\" value=\"{3}\"></item>"
                                            + "</fields>"
                                            + "<nextactivities>"
                                                + "<item name=\"{4}\" actors=\"{5}\"/>"
                                            + "</nextactivities>"
                                        + "</workflow>"
                                    + "</item>"
                                + "</parameter>"
                            + "</Root>";
                objectXML = string.Format(objectXML, this.WorkContext.CurrentUser.UserName, tempActivity.ActivityInstanceId.ToString(), curData.Title, curData.ID.ToString(), nextActivity, nextActors, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd"));

            }

            string[] args = new string[4];
            args[0] = FlowInstance.Workflow_SystemID;
            args[1] = FlowInstance.Workflow_SystemAcount;
            args[2] = FlowInstance.Workflow_SystemPwd;
            args[3] = objectXML;

            string resultXml = WebServicesHelper.InvokeWebService(FlowInstance.Workflow_SystemUrl, FlowInstance.MethodName.ManageWorkflow, args).ToString();
            System.Xml.XmlDocument xdoc = new System.Xml.XmlDocument();
            xdoc.LoadXml(resultXml);
            System.Xml.XmlNode resutNode = xdoc.SelectSingleNode("Result");
            string success = resutNode.Attributes["Success"].Value;
            string errmsg = resutNode.Attributes["ErrorMsg"].Value;
            string strWorkflowInstanceId = "";
            string strActivityinstanceId = "";
            int intSuccess = 0;
            int.TryParse(success, out intSuccess);
            if (intSuccess > 0)
            {
                if (!curData.WorkflowInstanceId.HasValue)
                {//第一次提交
                    System.Xml.XmlNodeList xmlList = xdoc.SelectNodes("Result/item/start/item");
                    for (int i = 0; i < xmlList.Count; i++)
                    {
                        switch (xmlList[i].Attributes["name"].Value)
                        {
                            case "WorkflowInstanceId": strWorkflowInstanceId = xmlList[i].Attributes["value"].Value; break;
                            case "ActivityinstanceId": strActivityinstanceId = xmlList[i].Attributes["value"].Value; break;
                            default: break;
                        }
                    }

                    var workFlow = _tracking_WorkflowService.FindById(Guid.Parse(strWorkflowInstanceId));
                    curData.State = 1;
                    curData.WorkflowInstanceId = Guid.Parse(strWorkflowInstanceId);
                    curData.Series = workFlow.SheetId;
                    curData.ApplyTime = DateTime.Now;

                    result.IsSuccess = _consumable_ScrapService.Update(curData);
                }
                else
                {//退回提交
                    result.IsSuccess = true;
                }
            }
            else
            {
                result.IsSuccess = false;
                result.Message = errmsg;
            }
            return result;
        }
        #endregion
        #endregion

        #region 零星申请
        public ActionResult Consumable_SporadicList()
        {
            return View();
        }
        public ActionResult Consumable_SporadicView(Guid? Id)
        {
            ViewData["Id"] = Id;
            return View();
        }
        public ActionResult Consumable_SporadicApply(Guid? Id, string type = "look")
        {
            ViewData["workflowName"] = FlowInstance.WorkflowType.ConsumableSporadic;
            ViewData["type"] = type;
            ViewData["Id"] = Id.HasValue ? Id.ToString() : null;
            ViewData["NewTitle"] = this.WorkContext.CurrentUser.RealName + "的报废退库申请" + DateTime.Now.ToString("(yyyy-MM-dd)");
            return View();
        }
        public ActionResult getSporadicApplyInfo()
        {
            object AppInfo = new object();
            if (this.WorkContext.CurrentUser != null)
            {
                AppInfo = new
                {
                    Applylicant = this.WorkContext.CurrentUser.UserId,
                    ApplyName = this.WorkContext.CurrentUser.RealName,
                    ApplyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    ApplyDpCode = this.WorkContext.CurrentUser.Dept.DpId,
                    DpName = this.WorkContext.CurrentUser.Dept.DpName,
                    Mobile = this.WorkContext.CurrentUser.Mobile,
                    // Series = "流程单号待生成",
                    Title = this.WorkContext.CurrentUser.RealName + "的零星申请(" + DateTime.Now.ToString("yyyy-MM-dd") + ")"
                };
            }
            return Json(AppInfo);
        }
        /// <summary>
        /// 零星申请列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="queryBuilder"></param>
        /// <param name="withDelData"></param>
        /// <returns></returns>
        public ActionResult GetListData_Consumable_Sporadic(int pageIndex = 1, int pageSize = 5, ConsumableSporadicQueryBuilder queryBuilder = null, bool withDelData = false)
        {
            int count = 0;
            var modelList = _consumable_SporadicService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);

            return Json(new { items = modelList, count = count });
        }
        /// <summary>
        /// 删除零星申请
        /// </summary>
        /// <param name="ConsumingIDs">报废申请ID</param>
        /// <returns></returns>
        public ActionResult Delete_Consumable_Sporadic(Guid?[] IDs)
        {
            int allCount = 0;
            int successCount = 0;
            string strMsg = "";

            var ListDatas = _consumable_SporadicService.List().Where(u => IDs.Contains(u.ID)).ToList();//领用申请信息
            allCount = ListDatas.Count;
            ListDatas = ListDatas.Where(u => u.State != 1).ToList();

            if (allCount > ListDatas.Count)
            {
                strMsg = "其中存在已经提交的申请信息，不能删除";
            }
            else if (ListDatas.Count > 0)
            {
                foreach (var item in ListDatas)
                {
                    if (_consumable_SporadicService.Delete(item))
                    {
                        successCount++;
                        var detailData = _consumable_SporadicDetailService.List().Where(u => u.ApplyID == item.ID).ToList();
                        _consumable_SporadicDetailService.DeleteByList(detailData);
                    }
                }
            }
            else
            {
                strMsg = "删除失败";
            }

            return Json(new
            {
                isSuccess = successCount > 0 ? true : false,
                successCount = successCount,
                messsage = strMsg
            });
        }
        /// <summary>
        /// 获取明细
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult Consumable_SporadicGetDetailByApplyID(Guid Id)
        {
            var modelList = _consumable_SporadicDetailService.List().Where(u => u.ApplyID == Id).ToList();
            return Json(new { items = modelList });
        }

        public ActionResult Consumable_SporadicGetByID(Guid Id)
        {
            var modelList = _consumable_SporadicService.FindById(Id);
            List<object> listResult = new List<object>();
            listResult.Add(new
            {
                modelList.ID,
                modelList.WorkflowInstanceId,
                modelList.Series,
                modelList.ApplyTime,
                DpName = CommonFunction.getDeptNamesByIDs(modelList.ApplyDpCode),
                ApplyName = CommonFunction.getUserRealNamesByIDs(modelList.Applicant),
                modelList.Mobile,
                modelList.Title,
                modelList.Content,
                modelList.State
            });

            return Json(new { items = listResult });
        }
        public ActionResult SporadicDetaildeleteByID(Guid Id)
        {
            var detail = _consumable_SporadicDetailService.FindById(Id);
            var isSuccess = _consumable_SporadicDetailService.Delete(detail);
            return Json(new { IsSuccess = isSuccess });
        }
        public ActionResult Save_Consumable_Sporadic(Consumable_Sporadic dataObj, List<Consumable_SporadicDetail> Details = null)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过
            if (dataObj.Title == null || string.IsNullOrEmpty(dataObj.Title.Trim()))
                tip = "标题不能为空";
            else if (dataObj.Content == null || string.IsNullOrEmpty(dataObj.Content.Trim()))
                tip = "原因不能为空";
            else
            {
                isValid = true;
                dataObj.Title = dataObj.Title.Trim();
                dataObj.Content = dataObj.Content.Trim();
            }

            if (!isValid)
            {
                result.IsSuccess = false;
                result.Message = tip;
                return Json(result);
            }
            #endregion

            if (dataObj.ID == null || dataObj.ID == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {//新增
                dataObj.ID = Guid.NewGuid();
                dataObj.State = 0;
                dataObj.Applicant = this.WorkContext.CurrentUser.UserId.ToString();
                dataObj.ApplyDpCode = this.WorkContext.CurrentUser.DpId;
                dataObj.Mobile = this.WorkContext.CurrentUser.Mobile;
                result.IsSuccess = _consumable_SporadicService.Insert(dataObj);
            }
            else
            {//编辑
                result.IsSuccess = _consumable_SporadicService.Update(dataObj);
            }
            if (result.IsSuccess)
                result.Message = dataObj.ID.ToString();
            return Json(result);
        }

        /// <summary>
        /// 零星耗材(发起流程工单)
        /// </summary>
        /// <param name="SporadicID">零星耗材ID</param>
        /// <returns></returns>
        public ActionResult Sumbit_Consumable_Sporadic(Guid SporadicID, string nextActivity = null, string nextActors = null, string nextCC = null)
        {
            SystemResult result = new SystemResult() { IsSuccess = false, Message = "" };

            var curData = _consumable_SporadicService.FindById(SporadicID);
            if (curData == null || curData.ID == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                result.IsSuccess = false;
                result.Message = "该零星耗材申请单信息不存在";
                return Json(result);
            }

            int count = 0;
            //获取当前报废单的明细

            var detailData = _consumable_SporadicDetailService.List().Where(u => u.ApplyID == curData.ID).ToList();
            int detailDataAmount = detailData.Sum(u => u.ApplyCount ?? 0);
            if (detailDataAmount <= 0)
            {
                result.IsSuccess = false;
                result.Message = "该报零星耗材中的总量小于1，请正确填写信息";
                return Json(result);
            }
            result = Sumbit_Sporadic_WF(curData, nextActivity, nextActors);
            if (result.IsSuccess && !string.IsNullOrEmpty(nextCC))
            {//提交成功后抄送
                curData = _consumable_SporadicService.FindById(SporadicID);
                CommonFunction.PendingData(curData.WorkflowInstanceId.Value, nextCC);//抄送
            }
            return Json(result);
        }

        #region 耗材零星申请的流程处理
        /// <summary>
        /// 提交报废申请
        /// </summary>
        public SystemResult Sumbit_Sporadic_WF(Consumable_Sporadic curData, string nextActivity, string nextActors)
        {
            if (string.IsNullOrEmpty(nextActivity) || string.IsNullOrEmpty(nextActors))
            {
                CommonFunction.GetFirstOperatorInfoAfterSumbit(FlowInstance.WorkflowType.ConsumingCancelling, this.WorkContext.CurrentUser.UserName, out nextActivity, out nextActors);
            }

            SystemResult result = new SystemResult() { IsSuccess = false };
            string objectXML = "";

            if (!curData.WorkflowInstanceId.HasValue)
            {//第一次提交
                objectXML = "<Root action=\"Start\" username=\"{0}\" keypassword=\"123\">"
                                + "<parameter>"
                                    + "<item name=\"workflowId\" value=\"{1}\"/>"
                                    + "<item name=\"workflowTitle\" value=\"{2}\"/>"
                                    + "<item name=\"workflowProperties\">"
                                        + "<workflow secrecy=\"0\" urgency=\"0\" importance=\"0\" expectFinishedTime=\"{6}\">"
                                            + "<fields>"
                                                + "<item name=\"F1\" value=\"{3}\"></item>"
                                            + "</fields>"
                                            + "<nextactivities>"
                                                + "<item name=\"{4}\" actors=\"{5}\"/>"
                                            + "</nextactivities>"
                                        + "</workflow>"
                                    + "</item>"
                                + "</parameter>"
                            + "</Root>";
                objectXML = string.Format(objectXML, this.WorkContext.CurrentUser.UserName, FlowInstance.WorkflowType.ConsumableSporadic, curData.Title, curData.ID.ToString(), nextActivity, nextActors, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd"));

            }
            else
            {//退回提交
                ITracking_TodoService tempService = new Tracking_TodoService();
                Tracking_Todo tempActivity = new Tracking_Todo();
                Guid guid = curData.WorkflowInstanceId.Value;
                tempActivity = new Tracking_TodoService().Entitys.Where(u => u.WorkflowInstanceId == guid).FirstOrDefault();//当前节点实例
                objectXML = "<Root action=\"execute\" username=\"{0}\" keypassword=\"123\">"
                                + "<parameter>"
                                    + "<item name=\"activityInstanceId\" value=\"{1}\"/>"
                                    + "<item name=\"command\" value=\"Approve\"/>"
                                    + "<item name=\"workflowTitle\" value=\"{2}\"/>"
                                    + "<item name=\"workflowProperties\">"
                                        + "<workflow secrecy=\"0\" urgency=\"0\" importance=\"0\" expectFinishedTime=\"{6}\">"
                                            + "<fields>"
                                                + "<item name=\"F1\" value=\"{3}\"></item>"
                                            + "</fields>"
                                            + "<nextactivities>"
                                                + "<item name=\"{4}\" actors=\"{5}\"/>"
                                            + "</nextactivities>"
                                        + "</workflow>"
                                    + "</item>"
                                + "</parameter>"
                            + "</Root>";
                objectXML = string.Format(objectXML, this.WorkContext.CurrentUser.UserName, tempActivity.ActivityInstanceId.ToString(), curData.Title, curData.ID.ToString(), nextActivity, nextActors, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd"));

            }

            string[] args = new string[4];
            args[0] = FlowInstance.Workflow_SystemID;
            args[1] = FlowInstance.Workflow_SystemAcount;
            args[2] = FlowInstance.Workflow_SystemPwd;
            args[3] = objectXML;

            string resultXml = WebServicesHelper.InvokeWebService(FlowInstance.Workflow_SystemUrl, FlowInstance.MethodName.ManageWorkflow, args).ToString();
            System.Xml.XmlDocument xdoc = new System.Xml.XmlDocument();
            xdoc.LoadXml(resultXml);
            System.Xml.XmlNode resutNode = xdoc.SelectSingleNode("Result");
            string success = resutNode.Attributes["Success"].Value;
            string errmsg = resutNode.Attributes["ErrorMsg"].Value;
            string strWorkflowInstanceId = "";
            string strActivityinstanceId = "";
            int intSuccess = 0;
            int.TryParse(success, out intSuccess);
            if (intSuccess > 0)
            {
                if (!curData.WorkflowInstanceId.HasValue)
                {//第一次提交
                    System.Xml.XmlNodeList xmlList = xdoc.SelectNodes("Result/item/start/item");
                    for (int i = 0; i < xmlList.Count; i++)
                    {
                        switch (xmlList[i].Attributes["name"].Value)
                        {
                            case "WorkflowInstanceId": strWorkflowInstanceId = xmlList[i].Attributes["value"].Value; break;
                            case "ActivityinstanceId": strActivityinstanceId = xmlList[i].Attributes["value"].Value; break;
                            default: break;
                        }
                    }

                    var workFlow = _tracking_WorkflowService.FindById(Guid.Parse(strWorkflowInstanceId));
                    curData.State = 1;
                    curData.WorkflowInstanceId = Guid.Parse(strWorkflowInstanceId);
                    curData.Series = workFlow.SheetId;
                    curData.ApplyTime = DateTime.Now;

                    result.IsSuccess = _consumable_SporadicService.Update(curData);
                }
                else
                {//退回提交
                    result.IsSuccess = true;
                }
            }
            else
            {
                result.IsSuccess = false;
                result.Message = errmsg;
            }
            return result;
        }
        #endregion
        #endregion




        #region 补录归档——用于先用耗材再补提工单
        //耗材补录归档列表页
        public ActionResult MakeupIndex()
        {
            return View();
        }
        //耗材补录归档编辑页
        public ActionResult MakeupEdit(Guid? MakeupID, string type = "look")
        {
            ViewData["workflowName"] = FlowInstance.WorkflowType.ConsumingMakeup;
            ViewData["type"] = type;
            ViewData["MakeupID"] = MakeupID.HasValue ? MakeupID.ToString() : null;
            ViewData["RealName"] = this.WorkContext.CurrentUser.RealName;
            ViewData["DeptName"] = this.WorkContext.CurrentUser.Dept.DpName;
            ViewData["Mobile"] = this.WorkContext.CurrentUser.Mobile;
            ViewData["NewTitle"] = this.WorkContext.CurrentUser.RealName + "的耗材补录归档申请" + DateTime.Now.ToString("(yyyy-MM-dd)");
            return View();
        }

        //耗材补录归档信息——流程中使用
        public ActionResult MakeupInfo_WF(Guid? MakeupID)
        {
            ViewData["MakeupID"] = MakeupID.HasValue ? MakeupID.ToString() : null;
            return View();
        }

        /// <summary>
        /// 获取补录归档单信息数据列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数据量</param>
        /// <param name="queryBuilder">查询条件</param>
        /// <returns></returns>
        public ActionResult GetListData_MakeupList(int pageIndex = 1, int pageSize = 5, ConsumableMakeupQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var modelList = _makeupService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (Consumable_Makeup)u).ToList();

            List<object> listResult = new List<object>();
            foreach (var item in modelList)
            {
                listResult.Add(new
                {
                    item.ID,
                    item.WorkflowInstanceId,
                    item.Series,
                    item.AppTime,
                    item.AppDept,
                    //AppDept_Text = CommonFunction.getDeptNamesByIDs(item.AppDept),
                    AppDept_Text = item.DeptsForApp?.DpFullName,
                    item.AppPerson,
                    //AppPerson_Text = CommonFunction.getUserRealNamesByIDs(item.AppPerson.HasValue ? item.AppPerson.ToString() : null),
                    AppPerson_Text = item.UsersForApp?.RealName,
                    item.Mobile,
                    item.Title,
                    item.Content,
                    item.State,
                    item.UsePerson,
                    UsePerson_Text = item.UsersForUse?.RealName,
                    WF_StateText = CommonFunction.GetFlowStateText(item.Tracking_Workflow),
                    WF_CurActivityName = _tracking_WorkflowService.GetCurrentActivityNames(item.WorkflowInstanceId ?? Guid.Parse("00000000-0000-0000-0000-000000000000")).FirstOrDefault()
                });
            }

            return Json(new { items = listResult, count = count }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 删除补录归档单
        /// </summary>
        /// <param name="InputListID">补录归档单ID</param>
        /// <returns></returns>
        public ActionResult Delete_Makeups(Guid[] IDs)
        {
            bool isSuccess = false;
            int allCount = 0;
            int successCount = 0;
            string strMsg = "";

            var makeupDatas = _makeupService.List().Where(u => IDs.Contains(u.ID)).ToList();//补录归档单信息
            allCount = makeupDatas.Count;

            if (makeupDatas.Where(u => u.State == 1).Count() > 0)
            {
                strMsg = "其中存在已提交的记录，不能删除";
            }
            else if (makeupDatas.Count > 0)
            {
                if (_makeupService.DeleteByList(makeupDatas))
                {
                    successCount = makeupDatas.Count;
                    foreach (var item in makeupDatas)
                    {
                        var temp = _makeupDetailService.List().Where(u => u.ApplyID == item.ID).ToList();
                        _makeupDetailService.DeleteByList(temp);
                    }
                }
            }
            else
            {
                strMsg = "删除失败";
            }

            return Json(new
            {
                isSuccess = successCount > 0 ? true : false,
                successCount = successCount,
                messsage = strMsg
            });

        }

        /// <summary>
        /// 保存补录归档单
        /// </summary>
        /// <param name="inputList">补录归档单信息</param>
        /// <param name="inputDetails">补录归档单明细</param>
        /// <returns></returns>
        public ActionResult Save_Makeup(Consumable_Makeup dataObj, List<Consumable_Amount> details = null)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过
            if (dataObj.Title == null || string.IsNullOrEmpty(dataObj.Title.Trim()))
                tip = "标题不能为空";
            else if (dataObj.UsePerson.HasValue == false || dataObj.UsePerson.Value == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                tip = "耗材使用人不能为空";
            else if (dataObj.Content == null || string.IsNullOrEmpty(dataObj.Content.Trim()))
                tip = "补录归档原因不能为空";
            else
            {
                isValid = true;
                dataObj.Title = dataObj.Title.Trim();
                dataObj.Content = dataObj.Content.Trim();
            }

            if (!isValid)
            {
                result.IsSuccess = false;
                result.Message = tip;
                return Json(result);
            }
            #endregion

            dataObj.AppTime = DateTime.Now;
            if (dataObj.ID == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {//新增
                dataObj.ID = Guid.NewGuid();
                dataObj.State = 0;
                dataObj.AppPerson = this.WorkContext.CurrentUser.UserId;
                dataObj.AppDept = this.WorkContext.CurrentUser.DpId;
                dataObj.Mobile = this.WorkContext.CurrentUser.Mobile;
                result.IsSuccess = _makeupService.Insert(dataObj);
            }
            else
            {//编辑
                result.IsSuccess = _makeupService.Update(dataObj);
            }

            //补录归档单信息保存成功，则继续保存更新补录归档单明细
            if (result.IsSuccess)
            {
                int count = 0;
                //先删除原来的明细数据
                var oldData = _makeupDetailService.GetForPaging(out count, new { ApplyID = dataObj.ID }).Select(u => (Consumable_MakeupDetail)u).ToList();
                _makeupDetailService.DeleteByList(oldData);
                if (details != null && details.Count > 0)
                {
                    List<Consumable_MakeupDetail> listDetail = new List<Consumable_MakeupDetail>();
                    foreach (var item in details)
                    {
                        Consumable_MakeupDetail tempData = new Consumable_MakeupDetail();
                        tempData.ID = Guid.NewGuid();
                        tempData.ApplyID = dataObj.ID;
                        tempData.ConsumableID = item.ConsumableID;
                        tempData.Amount = item.Amount;
                        listDetail.Add(tempData);
                    }
                    if (listDetail.Count > 0)
                        result.IsSuccess = _makeupDetailService.InsertByList(listDetail);
                    else
                        result.IsSuccess = true;
                    if (!result.IsSuccess)
                    {
                        result.Message = "保存补录归档单耗材明细信息失败";
                    }
                }
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "保存补录归档单信息失败";
            }

            if (result.IsSuccess)
                result.Message = dataObj.ID.ToString();
            return Json(result);
        }

        /// <summary>
        /// 根据ID获取补录归档单信息数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetDataByID_Makeup(Guid? id)
        {
            id = id ?? Guid.Parse("00000000-0000-0000-0000-000000000000");
            var makeupData = _makeupService.FindById(id);
            int count = 0;
            List<Consumable_MakeupDetail> detailDatas = new List<Consumable_MakeupDetail>();
            Users sumbitUser = new Users();
            if (makeupData != null)
            {
                detailDatas = _makeupDetailService.List().Where(u => u.ApplyID == makeupData.ID).ToList();
                sumbitUser = makeupData.UsersForApp;
            }
            return Json(new
            {
                makeupData = new
                {
                    makeupData.ID,
                    makeupData.WorkflowInstanceId,
                    makeupData.Series,
                    makeupData.AppTime,
                    makeupData.AppDept,
                    AppDept_Text = makeupData.DeptsForApp?.DpFullName,
                    makeupData.AppPerson,
                    AppPerson_Text = makeupData.UsersForApp?.RealName,
                    makeupData.Mobile,
                    makeupData.Title,
                    makeupData.Content,
                    makeupData.State,
                    makeupData.UsePerson,
                    UsePerson_Text = makeupData.UsersForUse?.RealName
                },
                detailDatas = detailDatas,
                sumbitUser = sumbitUser == null ? null : new { RealName = sumbitUser.RealName, Mobile = sumbitUser.Mobile, DeptName = sumbitUser.Dept.DpFullName },
                useUser = makeupData.UsersForUse == null ? null : new { ID = makeupData.UsersForUse.UserId, RealName = makeupData.UsersForUse.RealName }
            }
                , JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取补录归档单明细数据列表
        /// </summary>
        /// <param name="MakeupID">补录归档单ID</param>
        /// <param name="OnlyInput">是否只输出补录归档的信息</param>
        /// <returns></returns>
        public ActionResult GetListData_MakeupDetail(Guid? MakeupID, bool OnlyInput = true)
        {
            MakeupID = MakeupID ?? Guid.Parse("00000000-0000-0000-0000-000000000000");
            int count = 0;
            //获取补录归档明细数量
            var detailList = _makeupDetailService.GetForPaging(out count, new { ApplyID = MakeupID }).Select(u => (Consumable_MakeupDetail)u).ToList();
            List<int> detail_ConsumableList = detailList.Select(u => u.ConsumableID.Value).ToList();

            //获取耗材信息数据
            List<Consumable> modelList = new List<Consumable>();
            if (OnlyInput)
            {
                modelList = _consumableService.List().Where(u => detail_ConsumableList.Contains(u.ID)).ToList();
            }
            else
            {
                modelList = _consumableService.List().ToList();
            }

            List<object> listResult = new List<object>();
            foreach (var item in modelList)
            {
                int countTemp = 0;
                countTemp = detailList.Where(u => u.ConsumableID == item.ID).FirstOrDefault()?.Amount ?? 0;
                listResult.Add(new
                {
                    item.ID,
                    item.Type,
                    item.Name,
                    item.Trademark,
                    item.Model,
                    item.Equipment,
                    item.Unit,
                    item.IsValue,
                    Count = countTemp
                });
            }
            return Json(new { items = listResult, count = count });
        }

        /// <summary>
        /// 检查数据是否符合提交要求
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CheckConsumableAmount_ForMakeup(Guid? id)
        {
            SystemResult result = new SystemResult() { IsSuccess = false, Message = "" };
            id = id ?? Guid.Parse("00000000-0000-0000-0000-000000000000");
            var curData = _makeupService.FindById(id);
            if (curData == null || curData.ID == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                result.IsSuccess = false;
                result.Message = "该补录归档申请单信息不存在";
                return Json(result);
            }
            int count = 0;
            //获取当前补录归档单的明细
            var detailData = _makeupDetailService.GetForPaging(out count, new { ApplyID = curData.ID }).Select(u => (Consumable_MakeupDetail)u).ToList();
            int detailDataAmount = detailData.Sum(u => u.Amount ?? 0);
            if (detailDataAmount <= 0)
            {
                result.IsSuccess = false;
                result.Message = "该补录归档单中的补录归档总量小于1，请正确填写信息";
                return Json(result);
            }
            List<string> overConsumable = new List<string>();
            foreach (var item in detailData)
            {
                var curConsumable = _consumableService.FindById(item.ConsumableID);
                if (curConsumable.Amount < (item.Amount ?? 0))//补录归档申请数量大于库存量
                {
                    overConsumable.Add(curConsumable.Name);
                }
            }
            if (overConsumable.Count > 0)
            {
                result.IsSuccess = false;
                result.Message = string.Format("该补录归档单中“{0}”的补录归档数量超出库存量", string.Join(",", overConsumable));
                return Json(result);
            }
            result.IsSuccess = true;
            result.Message = "";
            return Json(result);
        }

        /// <summary>
        /// 提交补录归档单(发起流程工单)
        /// 注：该流程提交给发起人，抄送给耗材使用人
        /// </summary>
        /// <param name="MakeupID">补录归档单ID</param>
        /// <returns></returns>
        public ActionResult Sumbit_Makeup(Guid makeupID, string nextActivity = null, string nextActors = null, string nextCC = null)
        {
            nextActors = this.WorkContext.CurrentUser.UserName;
            SystemResult result = new SystemResult() { IsSuccess = false, Message = "" };

            var curData = _makeupService.FindById(makeupID);
            if (curData == null || curData.ID == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                result.IsSuccess = false;
                result.Message = "该补录归档申请单信息不存在";
                return Json(result);
            }

            int count = 0;
            //获取当前补录归档单的明细
            var detailData = _makeupDetailService.GetForPaging(out count, new { ApplyID = curData.ID }).Select(u => (Consumable_MakeupDetail)u).ToList();
            int detailDataAmount = detailData.Sum(u => u.Amount ?? 0);
            if (detailDataAmount <= 0)
            {
                result.IsSuccess = false;
                result.Message = "该补录归档单中的补录归档总量小于1，请正确填写信息";
                return Json(result);
            }

            #region 判断补录归档数量是否大于库存量
            List<string> overConsumable = new List<string>();
            foreach (var item in detailData)
            {
                var curConsumable = _consumableService.FindById(item.ConsumableID);
                if (curConsumable.Amount < (item.Amount ?? 0))//补录归档申请数量大于库存量
                {
                    overConsumable.Add(curConsumable.Name);
                }
            }
            if (overConsumable.Count > 0)
            {
                result.IsSuccess = false;
                result.Message = string.Format("该补录归档单中“{0}”的补录归档数量超出库存量", string.Join(",", overConsumable));
                return Json(result);
            }
            #endregion

            result = Sumbit_Makeup_WF(curData, nextActivity, nextActors);
            if (result.IsSuccess)
            {//提交成功后抄送使用人
                curData = _makeupService.FindById(curData.ID);
                CommonFunction.PendingData(curData.WorkflowInstanceId.Value, curData.UsersForUse.UserName);//补录归档提交后抄送使用人
            }
            return Json(result);
        }

        #region 耗材补录归档申请的流程处理
        /// <summary>
        /// 提交补录归档申请
        /// </summary>
        public SystemResult Sumbit_Makeup_WF(Consumable_Makeup curData, string nextActivity, string nextActors)
        {
            if (string.IsNullOrEmpty(nextActivity) || string.IsNullOrEmpty(nextActors))
            {
                CommonFunction.GetFirstOperatorInfoAfterSumbit(FlowInstance.WorkflowType.ConsumingMakeup, this.WorkContext.CurrentUser.UserName, out nextActivity, out nextActors);
            }

            SystemResult result = new SystemResult() { IsSuccess = false };
            string objectXML = "";

            if (!curData.WorkflowInstanceId.HasValue)
            {//第一次提交
                objectXML = "<Root action=\"Start\" username=\"{0}\" keypassword=\"123\">"
                                + "<parameter>"
                                    + "<item name=\"workflowId\" value=\"{1}\"/>"
                                    + "<item name=\"workflowTitle\" value=\"{2}\"/>"
                                    + "<item name=\"workflowProperties\">"
                                        + "<workflow secrecy=\"0\" urgency=\"0\" importance=\"0\" expectFinishedTime=\"{6}\">"
                                            + "<fields>"
                                                + "<item name=\"F1\" value=\"{3}\"></item>"
                                            + "</fields>"
                                            + "<nextactivities>"
                                                + "<item name=\"{4}\" actors=\"{5}\"/>"
                                            + "</nextactivities>"
                                        + "</workflow>"
                                    + "</item>"
                                + "</parameter>"
                            + "</Root>";
                objectXML = string.Format(objectXML, this.WorkContext.CurrentUser.UserName, FlowInstance.WorkflowType.ConsumingMakeup, curData.Title, curData.ID.ToString(), nextActivity, nextActors, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd"));

            }
            else
            {//退回提交
                ITracking_TodoService tempService = new Tracking_TodoService();
                Tracking_Todo tempActivity = new Tracking_Todo();
                Guid guid = curData.WorkflowInstanceId.Value;
                tempActivity = new Tracking_TodoService().Entitys.Where(u => u.WorkflowInstanceId == guid).FirstOrDefault();//当前节点实例
                objectXML = "<Root action=\"execute\" username=\"{0}\" keypassword=\"123\">"
                                + "<parameter>"
                                    + "<item name=\"activityInstanceId\" value=\"{1}\"/>"
                                    + "<item name=\"command\" value=\"Approve\"/>"
                                    + "<item name=\"workflowTitle\" value=\"{2}\"/>"
                                    + "<item name=\"workflowProperties\">"
                                        + "<workflow secrecy=\"0\" urgency=\"0\" importance=\"0\" expectFinishedTime=\"{6}\">"
                                            + "<fields>"
                                                + "<item name=\"F1\" value=\"{3}\"></item>"
                                            + "</fields>"
                                            + "<nextactivities>"
                                                + "<item name=\"{4}\" actors=\"{5}\"/>"
                                            + "</nextactivities>"
                                        + "</workflow>"
                                    + "</item>"
                                + "</parameter>"
                            + "</Root>";
                objectXML = string.Format(objectXML, this.WorkContext.CurrentUser.UserName, tempActivity.ActivityInstanceId.ToString(), curData.Title, curData.ID.ToString(), nextActivity, nextActors, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd"));

            }

            string[] args = new string[4];
            args[0] = FlowInstance.Workflow_SystemID;
            args[1] = FlowInstance.Workflow_SystemAcount;
            args[2] = FlowInstance.Workflow_SystemPwd;
            args[3] = objectXML;

            string resultXml = WebServicesHelper.InvokeWebService(FlowInstance.Workflow_SystemUrl, FlowInstance.MethodName.ManageWorkflow, args).ToString();
            System.Xml.XmlDocument xdoc = new System.Xml.XmlDocument();
            xdoc.LoadXml(resultXml);
            System.Xml.XmlNode resutNode = xdoc.SelectSingleNode("Result");
            string success = resutNode.Attributes["Success"].Value;
            string errmsg = resutNode.Attributes["ErrorMsg"].Value;
            string strWorkflowInstanceId = "";
            string strActivityinstanceId = "";
            int intSuccess = 0;
            int.TryParse(success, out intSuccess);
            if (intSuccess > 0)
            {
                if (!curData.WorkflowInstanceId.HasValue)
                {//第一次提交
                    System.Xml.XmlNodeList xmlList = xdoc.SelectNodes("Result/item/start/item");
                    for (int i = 0; i < xmlList.Count; i++)
                    {
                        switch (xmlList[i].Attributes["name"].Value)
                        {
                            case "WorkflowInstanceId": strWorkflowInstanceId = xmlList[i].Attributes["value"].Value; break;
                            case "ActivityinstanceId": strActivityinstanceId = xmlList[i].Attributes["value"].Value; break;
                            default: break;
                        }
                    }

                    var workFlow = _tracking_WorkflowService.FindById(Guid.Parse(strWorkflowInstanceId));
                    curData.State = 1;
                    curData.WorkflowInstanceId = Guid.Parse(strWorkflowInstanceId);
                    curData.Series = workFlow.SheetId;
                    curData.AppTime = DateTime.Now;

                    result.IsSuccess = _makeupService.Update(curData);
                }
                else
                {//退回提交
                    result.IsSuccess = true;
                }
            }
            else
            {
                result.IsSuccess = false;
                result.Message = errmsg;
            }
            return result;
        }
        #endregion
        #endregion

        #region 导出
        /// <summary>
        /// 导出领用统计
        /// </summary> 
        /// <param name="queryBuilder">参数</param>
        /// <returns></returns>
        public ActionResult ConsumingDetailDownload(string queryBuilder = null)
        {
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<ConsumableQueryBuilder>(queryBuilder);
                int pageIndex = 0;
                int pageSize = int.MaxValue;
                int count = 0;
                var modelList = _consumingDetailService.GetForPaging(out count, QueryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).ToList();


                if (modelList.Count < 1)
                    return View("../Export/Message");

                #region Excel部分
                //生成文件名
                var fileToSaveName = SaveName.ConsumingDetail + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.ConsumingDetail);
                //设置集合变量
                designer.SetDataSource(ImportFileType.ConsumingDetail, modelList);
                //根据数据源处理生成报表内容
                designer.Process();
                var response = GetResponse(fileToSaveName);
                designer.Save(Url.Content(fileToSaveName), SaveType.OpenInExcel, FileFormatType.Excel2003, response);
                response.Flush();
                response.Close();
                designer = null;
                response.End();

                #endregion

            }
            catch (Exception ex)
            {

                throw;
            }
            return null;
        }
       /// <summary>
       /// 报废 导出
       /// </summary>
       /// <param name="queryBuilder"></param>
       /// <returns></returns>
        public ActionResult ScrapDetailDownload(string queryBuilder = null)
        {
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<ConsumableQueryBuilder>(queryBuilder);
                int pageIndex = 0;
                int pageSize = int.MaxValue;
                int count = 0;
                var modelList = _scrapDetailService.GetForPaging(out count, QueryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).ToList();


                if (modelList.Count < 1)
                    return View("../Export/Message");

                #region Excel部分
                //生成文件名
                var fileToSaveName = SaveName.ScrapDetail + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.ScrapDetail);
                //设置集合变量
                designer.SetDataSource(ImportFileType.ScrapDetail, modelList);
                //根据数据源处理生成报表内容
                designer.Process();
                var response = GetResponse(fileToSaveName);
                designer.Save(Url.Content(fileToSaveName), SaveType.OpenInExcel, FileFormatType.Excel2003, response);
                response.Flush();
                response.Close();
                designer = null;
                response.End();

                #endregion

            }
            catch (Exception ex)
            {

                throw;
            }
            return null;
        }
        /// <summary>
        /// 退库 导出
        /// </summary>
        /// <param name="queryBuilder"></param>
        /// <returns></returns>
        public ActionResult CancellingDetailDownload(string queryBuilder = null)
        {
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<ConsumableQueryBuilder>(queryBuilder);
                int pageIndex = 0;
                int pageSize = int.MaxValue;
                int count = 0;
                var modelList = _cancellingDetail.GetForPaging(out count, QueryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).ToList();


                if (modelList.Count < 1)
                    return View("../Export/Message");

                #region Excel部分
                //生成文件名
                var fileToSaveName = SaveName.CancellingDetail + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.CancellingDetail);
                //设置集合变量
                designer.SetDataSource(ImportFileType.CancellingDetail, modelList);
                //根据数据源处理生成报表内容
                designer.Process();
                var response = GetResponse(fileToSaveName);
                designer.Save(Url.Content(fileToSaveName), SaveType.OpenInExcel, FileFormatType.Excel2003, response);
                response.Flush();
                response.Close();
                designer = null;
                response.End();

                #endregion

            }
            catch (Exception ex)
            {

                throw;
            }
            return null;
        }
        #endregion

        public class Consumable_Amount
        {
            public int ConsumableID { get; set; }
            public int Amount { get; set; }
        }

    }
}