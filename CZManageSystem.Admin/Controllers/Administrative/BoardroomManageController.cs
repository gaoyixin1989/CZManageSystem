using Aspose.Cells;
using CZManageSystem.Admin.Base;
using CZManageSystem.Admin.Models;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.Administrative;
using CZManageSystem.Data.Domain.Composite;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.Administrative;
using CZManageSystem.Service.Composite;
using CZManageSystem.Service.SysManger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.Composite
{
    public class BoardroomManageController : BaseController
    {

        IBoardroomInfoService _boardroomInfoService = new BoardroomInfoService();
        IBoardroomApplyService _boardroomApplyService = new BoardroomApplyService();
        ITracking_WorkflowService _tracking_WorkflowService = new Tracking_WorkflowService();
        IScheduleService _scheduleService = new ScheduleService();//行程安排

        #region 会议室资料信息
        #region 视图
        /// <summary>
        /// 会议室资料信息列表
        /// </summary>
        /// <returns></returns>
        public ActionResult BoardroomInfo()
        {
            return View();
        }
        /// <summary>
        /// 会议室资料信息编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult BoardroomInfoEdit(int? BoardroomID)
        {
            ViewData["BoardroomID"] = BoardroomID;
            if (BoardroomID == null)
                ViewBag.Title = "会议室资料新增";
            else
                ViewBag.Title = "会议室资料编辑";
            return View();
        }

        public ActionResult BoardroomInfoLook(int? BoardroomID)
        {
            ViewData["BoardroomID"] = BoardroomID;
            ViewBag.Title = "会议室资料查看";
            return View();
        }
        #endregion
        #region 查询和增删改
        /// <summary>
        /// 根据ID获取会议室资料信息数据
        /// </summary>
        /// <param name="BoardroomID"></param>
        /// <returns></returns>
        public ActionResult GetBoardroomInfoDataByID(int BoardroomID)
        {
            List<DataDictionary> CorpList = GetDictListByDDName(DataDic.CorpName);
            var obj = _boardroomInfoService.FindById(BoardroomID);
            return Json(new
            {
                obj.BoardroomID,
                obj.Editor,
                EditTime = obj.EditTime.HasValue ? obj.EditTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                obj.CorpID,
                CorpID_text = getDictText(CorpList, (obj.CorpID ?? -1).ToString()),
                //obj.Code,
                obj.Name,
                obj.Address,
                obj.MaxMan,
                obj.Equip,
                obj.EquipOther,
                //obj.Purpose,
                //obj.BookMode,
                obj.ManagerUnit,
                ManagerUnit_text = CommonFunction.getDeptNamesByIDs(obj.ManagerUnit),
                obj.ManagerPerson,
                ManagerPerson_text = CommonFunction.getUserRealNamesByIDs(obj.ManagerPerson),
                obj.State,
                obj.Remark,
                //StartTime = obj.StartTime.HasValue ? obj.StartTime.Value.ToString("yyyy-MM-dd HH:mm") : "",
                //EndTime = obj.EndTime.HasValue ? obj.EndTime.Value.ToString("yyyy-MM-dd HH:mm") : "",
                obj.Field00,
                obj.Field01,
                obj.Field02
            });
        }


        /// <summary>
        /// 下载会议室资料信息数据
        /// </summary> 
        /// <param name="queryBuilder">参数</param>
        /// <returns></returns>
        public ActionResult Download(string queryBuilder = null)
        {
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<BoardroomInfoQueryBuilder>(queryBuilder);
                int pageIndex = 0;
                int pageSize = int.MaxValue;
                List<DataDictionary> CorpList = GetDictListByDDName(DataDic.CorpName);

                int count = 0;
                var modelList = _boardroomInfoService.GetForPaging(out count, QueryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (BoardroomInfo)u).ToList();

                var listResult = modelList.Select(item => new
                {
                    item.BoardroomID,
                    item.Editor,
                    EditTime = item.EditTime.HasValue ? item.EditTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                    item.CorpID,
                    CorpID_text = getDictText(CorpList, (item.CorpID ?? -1).ToString()),
                    //item.Code,
                    item.Name,
                    item.Address,
                    item.MaxMan,
                    item.Equip,
                    item.EquipOther,
                    //item.Purpose,
                    //item.BookMode,
                    item.ManagerUnit,
                    ManagerUnit_text = CommonFunction.getDeptNamesByIDs(item.ManagerUnit),
                    item.ManagerPerson,
                    ManagerPerson_text = CommonFunction.getUserRealNamesByIDs(item.ManagerPerson),
                    item.State,
                    item.Remark,
                    //StartTime = item.StartTime.HasValue ? item.StartTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                    //EndTime = item.EndTime.HasValue ? item.EndTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : ""

                }).ToList<object>();


                if (listResult.Count < 1)
                    return View("../Export/Message");

                #region Excel部分
                //生成文件名
                var fileToSaveName = SaveName.BoardroomInfo + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.BoardroomInfo);
                //设置集合变量
                designer.SetDataSource(ImportFileType.BoardroomInfo, listResult);
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
        /// 获取会议室资料信息数据列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数据量</param>
        /// <returns></returns>
        public ActionResult GetBoardroomInfoListData(int pageIndex = 1, int pageSize = 5, BoardroomInfoQueryBuilder queryBuilder = null)
        {
            List<DataDictionary> CorpList = GetDictListByDDName(DataDic.CorpName);

            int count = 0;
            var modelList = _boardroomInfoService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (BoardroomInfo)u).ToList();
            List<object> listResult = new List<object>();
            foreach (var item in modelList)
            {
                listResult.Add(new
                {
                    item.BoardroomID,
                    item.Editor,
                    EditTime = item.EditTime.HasValue ? item.EditTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                    item.CorpID,
                    CorpID_text = getDictText(CorpList, (item.CorpID ?? -1).ToString()),
                    //item.Code,
                    item.Name,
                    item.Address,
                    item.MaxMan,
                    item.Equip,
                    item.EquipOther,
                    //item.Purpose,
                    //item.BookMode,
                    item.ManagerUnit,
                    ManagerUnit_text = CommonFunction.getDeptNamesByIDs(item.ManagerUnit),
                    item.ManagerPerson,
                    ManagerPerson_text = CommonFunction.getUserRealNamesByIDs(item.ManagerPerson),
                    item.State,
                    item.Remark,
                    //StartTime = item.StartTime.HasValue ? item.StartTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                    //EndTime = item.EndTime.HasValue ? item.EndTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                    item.Field00,
                    item.Field01,
                    item.Field02
                });
            }

            return Json(new { items = listResult, count = count });
        }

        /// <summary>
        /// 获取会议室资料信息数据--不完全版
        /// </summary>
        public ActionResult GetBoardroomInfoListData_short()
        {
            int count = 0;
            var modelList = _boardroomInfoService.GetForPaging(out count, null).Select(u => (BoardroomInfo)u).ToList();
            List<object> listResult = new List<object>();
            //下面的可以用这种写法   var  listResult = modelList.Select(a=>new { a.BoardroomID,a.Name });
            foreach (var item in modelList)
            {
                listResult.Add(new
                {
                    item.BoardroomID,
                    item.Name,
                    item.State
                });
            }

            return Json(new { items = listResult, count = count });
        }

        /// <summary>
        /// 保存会议室资料信息数据
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        public ActionResult SaveBoardroomInfo(BoardroomInfo dataObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过

            if (!dataObj.CorpID.HasValue)
                tip = "所属单位不能为空";
            //else if (dataObj.Code == null || string.IsNullOrEmpty(dataObj.Code.Trim()))//且不能重复
            //    tip = "会议室编号不能为空";
            else if (dataObj.Name == null || string.IsNullOrEmpty(dataObj.Name.Trim()))
                tip = "会议室名称不能为空";
            else if (dataObj.Address == null || string.IsNullOrEmpty(dataObj.Address.Trim()))
                tip = "会议室地点不能为空";
            else if ((dataObj.MaxMan ?? 0) < 1)
                tip = "最大人数为正整数";
            else if (dataObj.ManagerUnit == null || string.IsNullOrEmpty(dataObj.ManagerUnit.Trim()))
                tip = "管理单位不能为空";
            else if (dataObj.ManagerPerson == null || string.IsNullOrEmpty(dataObj.ManagerPerson.Trim()))
                tip = "管理人不能为空";
            else
            {
                isValid = true;
                dataObj.State = string.IsNullOrEmpty(dataObj.State) ? "在用" : dataObj.State;
            }

            //相同“字典名称”，不能存在相同的值
            //var objs = _boardroomInfoService.List().Where(u => u.Code == dataObj.Code && u.BoardroomID != dataObj.BoardroomID);
            //if (objs.Count() > 0)
            //{
            //    isValid = false;
            //    tip = "该会议室编号已经被占用";
            //}

            if (!isValid)
            {
                result.IsSuccess = false;
                result.Message = tip;
                return Json(result);
            }
            #endregion

            if (dataObj.BoardroomID == 0)//新增
            {
                dataObj.Editor = this.WorkContext.CurrentUser.UserId.ToString();
                dataObj.EditTime = DateTime.Now;
                result.IsSuccess = _boardroomInfoService.Insert(dataObj);
            }
            else
            {//编辑
                result.IsSuccess = _boardroomInfoService.Update(dataObj);
            }
            if (!result.IsSuccess)
            {
                result.Message = "保存失败";
            }
            return Json(result);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult DeleteBoardroomInfo(int[] ids)
        {
            bool isSuccess = false;
            int successCount = 0;
            var objs = _boardroomInfoService.List().Where(u => ids.Contains(u.BoardroomID)).ToList();
            if (objs.Count > 0)
            {
                isSuccess = _boardroomInfoService.DeleteByList(objs);
                successCount = isSuccess ? objs.Count() : 0;
            }
            return Json(new
            {
                isSuccess = successCount > 0 ? true : false,
                successCount = successCount
            });
        }
        #endregion
        #endregion

        #region 会议室申请
        #region 视图
        /// <summary>
        /// 会议室申请信息——日历
        /// </summary>
        public ActionResult BoardroomApply_Calendar(string type)
        {
            if (type != "manager")
                type = "user";
            ViewData["type"] = type;
            return View();
        }
        /// <summary>
        /// 会议室申请信息——列表
        /// </summary>
        public ActionResult BoardroomApply_List()
        {
            return View();
        }
        /// <summary>
        /// 我的会议室申请列表
        /// </summary>
        public ActionResult BoardroomApply_MyList()
        {
            return View();
        }
        /// <summary>
        /// 会议室申请信息编辑
        /// </summary>
        public ActionResult BoardroomApplyEdit(int? id, int? BoardroomID, string strDate, string type = "look")
        {
            ViewData["workflowName"] = FlowInstance.WorkflowType.BoardroomApply;
            ViewData["type"] = type;
            ViewData["id"] = id;
            if (id == null)
            { //新增
                ViewBag.Title = "会议室申请新增";
                ViewData["BoardroomID"] = BoardroomID;
                DateTime temp = new DateTime();
                if (!DateTime.TryParse(strDate, out temp))
                {
                    temp = DateTime.Now.AddDays(1);
                }
                ViewData["strDate"] = temp.ToString("yyyy-MM-dd");
            }
            else
            { //编辑
                ViewBag.Title = "会议室申请编辑";
            }
            return View();
        }
        /// <summary>
        /// 我的会议室申请——查看页
        /// </summary>
        public ActionResult BoardroomApply_ForWF(string type, string step, int? id)
        {
            ViewData["type"] = type;
            ViewData["step"] = step;
            ViewData["id"] = id;
            return View();
        }

        /// <summary>
        /// 我的会议室申请使用满意度报表
        /// </summary>
        public ActionResult BoardroomApply_JudgeReport()
        {
            return View();
        }
        #endregion
        #region 查询和增删改
        /// <summary>
        /// 根据ID获取会议室申请信息数据
        /// </summary>
        /// <param name="id"></param>
        public ActionResult GetBoardroomApplyDataByID(int id)
        {
            var obj = _boardroomApplyService.FindById(id);
            var room = _boardroomInfoService.FindById(obj.Room);
            return Json(new
            {
                item = obj,
                att = new
                {
                    AppPerson_text = CommonFunction.getUserRealNamesByIDs(obj.AppPerson),
                    AppTime = obj.AppTime.HasValue ? obj.AppTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                    AppDept_text = CommonFunction.getDeptNamesByIDs(obj.AppDept),
                    JoinPeople_text = CommonFunction.getUserRealNamesByIDs(obj.JoinPeople),
                    MeetingDate = obj.MeetingDate.Value.ToString("yyyy-MM-dd ") + obj.StartTime,
                    EndDate = obj.EndDate.Value.ToString("yyyy-MM-dd ") + obj.EndTime,
                    EndDate_Real = obj.EndDate_Real.Value.ToString("yyyy-MM-dd HH:mm"),
                    BookTime = obj.BookTime.HasValue ? obj.BookTime.Value.ToString("yyyy-MM-dd HH:mm") : "",
                    WelcoomeTime = obj.WelcoomeTime.HasValue ? obj.WelcoomeTime.Value.ToString("yyyy-MM-dd HH:mm") : "",
                    RoomName = room?.Name
                }
            });
        }

        /// <summary>
        /// 获取会议室申请信息数据列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数据量</param>
        public ActionResult GetBoardroomApplyListData(int pageIndex = 1, int pageSize = 5, BoardroomApplyQueryBuilder queryBuilder = null, bool isManager = false)
        {
            //string mm = QueryFlowState("");
            if (!isManager)
                queryBuilder.AppPerson = new string[] { this.WorkContext.CurrentUser.UserId.ToString() };
            else
            {//管理员，不查看未提交的信息
                queryBuilder.State_without = (new List<int>() { 0 }).ToArray();
            }
            List<DataDictionary> CorpList = GetDictListByDDName(DataDic.CorpName);
            //待修改
            int count = 0;
            var modelList = _boardroomApplyService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).ToList();
            List<object> listResult = new List<object>();
            foreach (var item in modelList)
            {
                var curRoom = _boardroomInfoService.FindById(item.Room);
                Tracking_Workflow workFlow = new Tracking_Workflow();
                if (!string.IsNullOrEmpty(item.WorkflowInstanceId))
                {
                    workFlow = _tracking_WorkflowService.FindById(Guid.Parse(item.WorkflowInstanceId));
                }

                listResult.Add(new
                {
                    item.ID,
                    item.WorkflowInstanceId,
                    item.ApplyTitle,
                    //item.CorpID,
                    //CorpID_text = getDictText(CorpList, (item.CorpID ?? -1).ToString()),
                    EndDate = item.EndDate.HasValue ? item.EndDate.Value.ToString("yyyy-MM-dd") : "",
                    EndDate_Real = item.EndDate_Real,
                    item.Code,
                    item.State,
                    item.StateRemark,
                    //item.AppDept,
                    //AppDept_text = getDeptNamesByIDs(item.AppDept),
                    item.AppPerson,
                    AppPerson_text = CommonFunction.getUserRealNamesByIDs(item.AppPerson),
                    item.AppTime,
                    item.ContactMobile,
                    item.Room,
                    RoomInfo = new
                    {
                        curRoom?.Name,
                        curRoom?.Address
                    },
                    //item.JoinNum,
                    //item.JoinPeople,
                    //JoinPeople_text = CommonFunction.getUserRealNamesByIDs(item.JoinPeople),
                    MeetingDate = item.MeetingDate.HasValue ? item.MeetingDate.Value.ToString("yyyy-MM-dd") : "",
                    item.StartTime,
                    item.EndTime,
                    //item.AwokeTime,
                    //item.UseFor,
                    //item.Fugle,
                    //item.NeedEquip,
                    //item.OtherEquip,
                    //item.BannerContent,
                    //item.BannerLength,
                    //item.BannerWidth,
                    //item.BannerMode,
                    //item.WelcomeContent,
                    //item.WelcoomeTime,
                    //item.WelcoomeSect,
                    //item.BookTime,
                    //item.Remark,
                    //item.Field00,
                    //item.Field01,
                    //item.Field02,
                    //item.Editor,
                    //Editor_text = CommonFunction.getUserRealNamesByIDs(item.Editor),
                    item.JudgeServiceQuality,
                    item.JudgeEnvironmental,
                    item.JudgeOtherSuggest,
                    item.JudgeState,
                    item.ISTVMeeting,
                    Tracking_Workflow = workFlow,
                    WF_StateText = CommonFunction.GetFlowStateText(workFlow),
                    WF_CurActivityName = _tracking_WorkflowService.GetCurrentActivityNames(workFlow.WorkflowInstanceId).FirstOrDefault()
                });
            }

            return Json(new { items = listResult, count = count });
        }
        public ActionResult GetBoardroomApplyDataForCalendar(int pageIndex = 1, int pageSize = int.MaxValue, BoardroomApplyQueryBuilder queryBuilder = null, bool isManager = false)
        {
            //string mm = QueryFlowState("");
            if (!isManager)
                queryBuilder.AppPerson = new string[] { this.WorkContext.CurrentUser.UserId.ToString() };
            else
            {//管理员，不查看未提交的信息
                queryBuilder.State_without = (new List<int>() { 0 }).ToArray();
            }
            List<DataDictionary> CorpList = GetDictListByDDName(DataDic.CorpName);
            //待修改
            int count = 0;
            var models = _boardroomApplyService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);
            models = models.Where(u => !string.IsNullOrEmpty(u.WorkflowInstanceId));
            var modelList = models.ToList();
            List<object> listResult = new List<object>();
            foreach (var item in modelList)
            {
                var curRoom = _boardroomInfoService.FindById(item.Room);
                Tracking_Workflow workFlow = new Tracking_Workflow(); ;
                if (!string.IsNullOrEmpty(item.WorkflowInstanceId))
                {
                    workFlow = _tracking_WorkflowService.FindById(Guid.Parse(item.WorkflowInstanceId));
                }
                if (workFlow == null || workFlow.State == 99)
                    continue;

                listResult.Add(new
                {
                    item.ID,
                    //item.WorkflowInstanceId,
                    item.ApplyTitle,
                    //item.CorpID,
                    //CorpID_text = getDictText(CorpList, (item.CorpID ?? -1).ToString()),
                    EndDate = item.EndDate.HasValue ? item.EndDate.Value.ToString("yyyy-MM-dd") : "",
                    EndDate_Real = item.EndDate_Real,
                    //item.Code,
                    item.State,
                    item.StateRemark,
                    //item.AppDept,
                    //AppDept_text = getDeptNamesByIDs(item.AppDept),
                    item.AppPerson,
                    AppPerson_text = CommonFunction.getUserRealNamesByIDs(item.AppPerson),
                    item.AppTime,
                    //item.ContactMobile,
                    item.Room,
                    RoomInfo = new
                    {
                        curRoom?.Name,
                        curRoom?.Address
                    },
                    //item.JoinNum,
                    //item.JoinPeople,
                    //JoinPeople_text = CommonFunction.getUserRealNamesByIDs(item.JoinPeople),
                    MeetingDate = item.MeetingDate.HasValue ? item.MeetingDate.Value.ToString("yyyy-MM-dd") : "",
                    item.StartTime,
                    item.EndTime,
                    //item.AwokeTime,
                    //item.UseFor,
                    //item.Fugle,
                    //item.NeedEquip,
                    //item.OtherEquip,
                    //item.BannerContent,
                    //item.BannerLength,
                    //item.BannerWidth,
                    //item.BannerMode,
                    //item.WelcomeContent,
                    //item.WelcoomeTime,
                    //item.WelcoomeSect,
                    //item.BookTime,
                    //item.Remark,
                    //item.Field00,
                    //item.Field01,
                    //item.Field02,
                    //item.Editor,
                    //Editor_text = CommonFunction.getUserRealNamesByIDs(item.Editor),
                    item.JudgeServiceQuality,
                    item.JudgeEnvironmental,
                    item.JudgeOtherSuggest,
                    item.JudgeState,
                    item.ISTVMeeting,
                    Tracking_Workflow = workFlow,
                    WF_StateText = CommonFunction.GetFlowStateText(workFlow),
                    WF_CurActivityName = _tracking_WorkflowService.GetCurrentActivityNames(workFlow.WorkflowInstanceId).FirstOrDefault()
                });
            }

            return Json(new { items = listResult, count = listResult.Count });
        }


        /// <summary>
        /// 保存会议室申请信息数据
        /// </summary>
        public ActionResult SaveBoardroomApply(BoardroomApply dataObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过

            if ((dataObj.Room ?? 0) == 0)
                tip = "会议室不能为空";
            if (string.IsNullOrEmpty(dataObj.AppPerson))
                tip = "申请人不能为空";
            else if (string.IsNullOrEmpty(dataObj.ApplyTitle))
                tip = "会议主题不能为空";
            else if (string.IsNullOrEmpty(dataObj.Code))
                tip = "流程单号不能为空";
            else if (string.IsNullOrEmpty(dataObj.JoinNum))
                tip = "与会人数不能为空";
            else if (!dataObj.MeetingDate.HasValue)
                tip = "会议开始时间不能为空";
            else if (!dataObj.EndDate.HasValue)
                tip = "会议结束时间不能为空";
            else if (!dataObj.BookTime.HasValue)
                tip = "订会时间不能为空";
            else if (string.IsNullOrEmpty(dataObj.Fugle))
                tip = "领导参加情况不能为空";
            else
            {
                isValid = true;
                dataObj.StartTime = dataObj.MeetingDate.Value.ToString("HH:mm");//开始时间
                dataObj.MeetingDate = dataObj.MeetingDate.Value.Date;//会议开始日期
                dataObj.EndDate_Real = dataObj.EndDate;//会议的真实结束时间(默认为申请时的结束时间)
                dataObj.EndTime = dataObj.EndDate.Value.ToString("HH:mm");//结束时间
                dataObj.EndDate = dataObj.EndDate.Value.Date;//会议结束日期
                dataObj.ISTVMeeting = dataObj.ISTVMeeting ?? false; //是否电视会议
                dataObj.CorpID = _boardroomInfoService.FindById(dataObj.Room).CorpID;//所属单位
                dataObj.Fugle_pro = dataObj.Fugle_pro ?? false; //是否有省公司领导参加

            }
            if (!checkRoomApply(dataObj))
            {
                isValid = false;
                tip = "当前会议室申请时间段已经被占用，请更换会议室或时间段。";
            }
            else if (_boardroomInfoService.FindById(dataObj.Room)?.State=="停用")
            {
                isValid = false;
                tip = "当前会议室已经停用。";
            }
            if (!isValid)
            {
                result.IsSuccess = false;
                result.Message = tip;
                return Json(result);
            }
            #endregion

            dataObj.Editor = this.WorkContext.CurrentUser.UserId.ToString();
            if (dataObj.ID == 0)//新增
            {
                dataObj.AppTime = DateTime.Now;
                dataObj.State = 0;
                dataObj.JudgeState = 0;

                result.IsSuccess = _boardroomApplyService.Insert(dataObj);
            }
            else
            {//编辑
                result.IsSuccess = _boardroomApplyService.Update(dataObj);
            }
            if (!result.IsSuccess)
            {
                result.Message = "保存失败";
            }
            else
            {
                result.Message = dataObj.ID.ToString();
            }
            return Json(result);
        }

        /// <summary>
        /// 保存评价
        /// </summary>
        public ActionResult SaveBoardroomApply_Judge(BoardroomApply dataObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            BoardroomApply curData = _boardroomApplyService.FindById(dataObj.ID);
            if (curData == null || curData.ID == 0)
            {
                result.IsSuccess = false;
                result.Message = "该数据不存在";
                return Json(result);
            }

            curData.JudgeEnvironmental = dataObj.JudgeEnvironmental;
            curData.JudgeOtherSuggest = dataObj.JudgeOtherSuggest;
            curData.JudgeServiceQuality = dataObj.JudgeServiceQuality;
            curData.JudgeState = 1;
            curData.JudgeTime = DateTime.Now;
            curData.Editor = this.WorkContext.CurrentUser.UserId.ToString();
            result.IsSuccess = _boardroomApplyService.Update(curData);
            if (!result.IsSuccess)
            {
                result.Message = "保存评价失败";
            }
            return Json(result);
        }



        /// <summary>
        /// 提交会议室申请
        /// </summary>
        /// <param name="id">数据ID</param>
        /// <param name="nextActivity">下一步骤名称</param>
        /// <param name="nextActors">下一步执行人</param>
        /// <param name="nextCC">下一步抄送人</param>
        /// <returns></returns>
        public ActionResult SumbitBoardroomApply(int id, string nextActivity = null, string nextActors = null, string nextCC = null)
        {
            BoardroomApply curData = _boardroomApplyService.FindById(id);
            SystemResult result = new SystemResult() { IsSuccess = false };
            if (curData == null || curData.ID == 0)
            {
                result.IsSuccess = false;
                result.Message = "该数据不存在";
                return Json(result);
            }
            result = BoardroomApply_Sumbit(curData, nextActivity, nextActors);
            if (result.IsSuccess && !string.IsNullOrEmpty(nextCC))
            {//提交成功后抄送
                curData = _boardroomApplyService.FindById(curData.ID);
                CommonFunction.PendingData(Guid.Parse(curData.WorkflowInstanceId), nextCC);//抄送
            }
            return Json(result);
        }



        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult DeleteBoardroomApply(int[] ids)
        {
            bool isSuccess = false;
            int successCount = 0;
            var objs = _boardroomApplyService.List().Where(u => ids.Contains(u.ID)).ToList();
            if (objs.Count > 0)
            {
                isSuccess = _boardroomApplyService.DeleteByList(objs);
                successCount = isSuccess ? objs.Count() : 0;
            }
            return Json(new
            {
                isSuccess = successCount > 0 ? true : false,
                successCount = successCount
            });
        }

        /// <summary>
        /// 取消操作——普通用户
        /// </summary>
        public ActionResult CancelBoardroomApply_user(int[] ids, string reason)
        {
            bool isSuccess = false;
            var objs = _boardroomApplyService.List().Where(u => ids.Contains(u.ID)).ToList();

            int successCount = 0;
            int errorCount = 0;

            if (objs.Count > 0)
            {
                SystemResult sysR = new SystemResult();
                foreach (var item in objs)
                {
                    item.State = -1;
                    item.StateRemark = "申请人取消";
                    if (!string.IsNullOrEmpty(reason)) item.StateRemark += ":" + reason;
                    item.Editor = this.WorkContext.CurrentUser.UserId.ToString();
                    sysR = BoardroomApply_Cancel(item);
                    if (sysR.IsSuccess)
                    {
                        _boardroomApplyService.Update(item);
                        CancelScheduleAfterBoardroomApplyCancel(item);
                        successCount++;
                    }
                    else
                        errorCount++;
                }
            }
            return Json(new
            {
                isSuccess = successCount > 0 ? true : false
            });
        }

        /// <summary>
        /// 取消会议室申请单添加的流程安排
        /// </summary>
        /// <param name="item"></param>
        private void CancelScheduleAfterBoardroomApplyCancel(BoardroomApply applyData)
        {
            BoardroomInfo roomInfo = _boardroomInfoService.FindByFeldName(u => u.BoardroomID == (applyData.Room ?? 0));
            DateTime scheduleTime = DateTime.Parse(applyData.MeetingDate.Value.ToString("yyyy-MM-dd") + " " + applyData.StartTime + ":00");
            string strContentLike1 = string.Format("参加会议“{0}”", applyData.ApplyTitle);
            string strContentLike2 = string.Format("地点：{0}——{1}",
               roomInfo.Address,
               roomInfo.Name);


            //_scheduleService
            List<Guid> userIDs = new List<Guid>();
            if (!string.IsNullOrEmpty(applyData.JoinPeople))
            {
                foreach (var str in applyData.JoinPeople.Split(','))
                {
                    userIDs.Add(Guid.Parse(str));
                }
            }

            if (userIDs.Count > 0)
            {
                List<Schedule> listSchedule = _scheduleService.List().Where(u => u.Time == scheduleTime && userIDs.Contains(u.UserId) && (u.Content.Contains(strContentLike1) || u.Content.Contains(strContentLike2))).ToList();
                if (listSchedule.Count > 0)
                    _scheduleService.DeleteByList(listSchedule);
            }

        }

        /// <summary>
        /// 取消操作——管理者
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="reason">取消原因</param>
        /// <returns></returns>
        public ActionResult CancelBoardroomApply_manager(int[] ids, string reason)
        {
            int successCount = 0;
            int errorCount = 0;
            bool isSuccess = false;
            var objs = _boardroomApplyService.List().Where(u => ids.Contains(u.ID)).ToList();
            if (objs.Count > 0)
            {
                SystemResult sysR = new SystemResult();
                foreach (var item in objs)
                {
                    item.State = -1;
                    item.StateRemark = "管理员取消";
                    if (!string.IsNullOrEmpty(reason)) item.StateRemark += ":" + reason;
                    item.Editor = this.WorkContext.CurrentUser.UserId.ToString();
                    sysR = BoardroomApply_Cancel(item); if (sysR.IsSuccess)
                    {
                        _boardroomApplyService.Update(item);
                        CancelScheduleAfterBoardroomApplyCancel(item);
                        successCount++;
                    }
                    else
                        errorCount++;
                }
            }
            return Json(new
            {
                isSuccess = successCount > 0 ? true : false
            });
        }

        /// <summary>
        /// 更新会议结束时间
        /// </summary>
        public ActionResult UpdateMeetingEndTime(int id, DateTime newtime)
        {
            var obj = _boardroomApplyService.FindById(id);
            obj.EndDate_Real = newtime;
            bool isSuccess = _boardroomApplyService.Update(obj);
            return Json(new
            {
                isSuccess = isSuccess
            });
        }
        #endregion
        #endregion

        #region 其他方法
        /// <summary>
        /// 生成新的申请信息
        /// </summary>
        public ActionResult getNewApplyInfo()
        {
            object AppInfo = new object();
            if (this.WorkContext.CurrentUser != null)
            {
                AppInfo = new
                {
                    AppPerson = this.WorkContext.CurrentUser.UserId.ToString(),
                    AppPerson_text = this.WorkContext.CurrentUser.RealName,
                    AppTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    AppDept = this.WorkContext.CurrentUser.DpId,
                    AppDept_text = CommonFunction.getDeptNamesByIDs(this.WorkContext.CurrentUser.DpId),
                    ContactMobile = this.WorkContext.CurrentUser.Mobile,
                    ApplyTitle = this.WorkContext.CurrentUser.RealName + "的会议室预定申请(" + DateTime.Now.ToString("yyyy-MM-dd") + ")",//会议主题
                    Code = "流程单号待生成"
                };
            }
            return Json(AppInfo);
        }

        /// <summary>
        /// 获取字典名称text
        /// </summary>
        /// <param name="CorpList"></param>
        /// <param name="DDValue"></param>
        private string getDictText(List<DataDictionary> CorpList, string DDValue)
        {
            string strResult = "";
            var temp = CorpList.Where(u => u.DDValue == DDValue).FirstOrDefault();
            if (temp != null)
                strResult = temp.DDText;
            return strResult;
        }

        /// <summary>
        /// 查询该申请所在时间会议室是否已经可用
        /// 即该会议室在该时间段内是否已经有其他申请提交了,有则不可用，返回false
        /// </summary>
        /// <param name="applyData"></param>
        /// <returns></returns>
        private bool checkRoomApply(BoardroomApply applyData)
        {
            BoardroomApplyQueryBuilder queryBuilder = new BoardroomApplyQueryBuilder()
            {
                Room = new int[] { applyData.Room ?? 0 },
                State = new int[] { 1 },
                MeetingTime_start = Convert.ToDateTime(applyData.MeetingDate.Value.ToString("yyyy-MM-dd " + applyData.StartTime + ":00")),
                MeetingTime_end = Convert.ToDateTime(applyData.EndDate_Real.Value.ToString("yyyy-MM-dd HH:mm:ss"))
            };
            int count = 0;
            var modelList = _boardroomApplyService.GetForPaging(out count, queryBuilder).Where(u => u.ID != applyData.ID);
            var wfList = modelList.Select(u => u.WorkflowInstanceId).ToList();//获取其中的流程示例ID

            int[] intState = { 1, 2 };
            Tracking_Workflow workFlow;
            foreach (string curId in wfList)
            {
                workFlow = _tracking_WorkflowService.FindById(Guid.Parse(curId));
                if (workFlow != null && intState.Contains(workFlow.State))
                    return false;
            }

            return true;
        }
        /// <summary>
        /// 下载满意度报表
        /// </summary>
        /// <param name="queryBuilder"></param>
        /// <returns></returns>
        public ActionResult DownloadboardroomApply(string queryBuilder = null)
        {
            string path = "";
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<BoardroomApplyQueryBuilder>(queryBuilder);
                int pageIndex = 0;
                int pageSize = int.MaxValue;

                int count = 0;
                var modelList = _boardroomApplyService.GetForPaging(out count, QueryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize).ToList();
                List<object> listResult = new List<object>();
                foreach (var item in modelList)
                {
                    var curRoom = _boardroomInfoService.FindById(item.Room);
                    Tracking_Workflow workFlow = new Tracking_Workflow();
                    if (!string.IsNullOrEmpty(item.WorkflowInstanceId))
                    {
                        workFlow = _tracking_WorkflowService.FindById(Guid.Parse(item.WorkflowInstanceId));
                    }

                    listResult.Add(new
                    {
                        item.ID,
                        item.WorkflowInstanceId,
                        item.ApplyTitle,
                        EndDate = item.EndDate.HasValue ? item.EndDate.Value.ToString("yyyy-MM-dd") : "",
                        EndDate_Real = item.EndDate_Real,
                        //item.Code,
                        item.State,
                        item.StateRemark,
                        item.AppPerson,
                        AppPerson_text = CommonFunction.getUserRealNamesByIDs(item.AppPerson),
                        AppTime = item.AppTime.HasValue ? item.AppTime.Value.ToString("yyyy-MM-dd") : "",
                        item.ContactMobile,
                        item.Room,
                        curRoom?.Name,
                        MeetingDate = item.MeetingDate.HasValue ? item.MeetingDate.Value.ToString("yyyy-MM-dd") : "",
                        item.StartTime,
                        item.EndTime,
                        item.JudgeServiceQuality,
                        item.JudgeEnvironmental,
                        item.JudgeOtherSuggest,
                        item.JudgeState,
                        item.ISTVMeeting,
                        Tracking_Workflow = workFlow,
                        WF_StateText = CommonFunction.GetFlowStateText(workFlow),
                        WF_CurActivityName = _tracking_WorkflowService.GetCurrentActivityNames(workFlow.WorkflowInstanceId).FirstOrDefault()
                    });
                }

                if (listResult.Count <1)
                    return View("../Export/Message");
                #region Excel部分
                //生成文件名
                var fileToSaveName = SaveName.BoardroomInfoApply + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.BoardroomInfoApply);
                //设置集合变量
                designer.SetDataSource(ImportFileType.BoardroomInfoApply, listResult);
                //根据数据源处理生成报表内容
                designer.Process();
                //designer.Save(path, FileFormatType.Excel2003);
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

        #region 流程方法
        /// <summary>
        /// 提交会议室申请
        /// </summary>
        public SystemResult BoardroomApply_Sumbit(BoardroomApply curData, string nextActivity, string nextActors)
        {
            //nextActivity = "审核";
            //nextActors = "admin";//caiwencheng
            if (string.IsNullOrEmpty(nextActivity) || string.IsNullOrEmpty(nextActors))
            {
                CommonFunction.GetFirstOperatorInfoAfterSumbit(FlowInstance.WorkflowType.BoardroomApply, this.WorkContext.CurrentUser.UserName, out nextActivity, out nextActors);

            }

            SystemResult result = new SystemResult() { IsSuccess = false };
            if (curData == null || curData.ID == 0)
            {
                result.IsSuccess = false;
                result.Message = "该数据不存在";
                return result;
            }
            string objectXML = "";

            if (string.IsNullOrEmpty(curData.WorkflowInstanceId))
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
                objectXML = string.Format(objectXML, this.WorkContext.CurrentUser.UserName, FlowInstance.WorkflowType.BoardroomApply, curData.ApplyTitle, curData.ID, nextActivity, nextActors, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd"));

            }
            else
            {//退回提交
                ITracking_TodoService tempService = new Tracking_TodoService();
                Tracking_Todo tempActivity = new Tracking_Todo();
                Guid guid = new Guid();
                Guid.TryParse(curData.WorkflowInstanceId, out guid);
                tempActivity = new Tracking_TodoService().Entitys.Where(u => u.WorkflowInstanceId == guid).FirstOrDefault();
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
                objectXML = string.Format(objectXML, this.WorkContext.CurrentUser.UserName, tempActivity.ActivityInstanceId.ToString(), curData.ApplyTitle, curData.ID, nextActivity, nextActors, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd"));

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
                if (string.IsNullOrEmpty(curData.WorkflowInstanceId))
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
                    curData.WorkflowInstanceId = strWorkflowInstanceId;
                    curData.Code = workFlow.SheetId;
                    curData.AppTime = DateTime.Now;

                    result.IsSuccess = _boardroomApplyService.Update(curData);
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

        /// <summary>
        /// 取消会议室申请工单
        /// </summary>
        /// <param name="curData"></param>
        /// <returns></returns>
        public SystemResult BoardroomApply_Cancel(BoardroomApply curData)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };

            ITracking_TodoService tempService = new Tracking_TodoService();
            Tracking_Todo tempActivity = new Tracking_Todo();
            Guid guid = new Guid();
            Guid.TryParse(curData.WorkflowInstanceId, out guid);
            tempActivity = new Tracking_TodoService().Entitys.Where(u => u.WorkflowInstanceId == guid).FirstOrDefault();
            string objectXML = "<Root action=\"execute\" username=\"{0}\" keypassword=\"123\">"
                            + "<parameter>"
                                + "<item name=\"activityInstanceId\" value=\"{1}\"/>"
                                + "<item name=\"command\" value=\"cancel\"/>"
                                + "<item name=\"workflowProperties\">"
                                    + "<workflow secrecy=\"0\" urgency=\"0\" importance=\"0\" expectFinishedTime=\"{3}\">"
                                    //+ "<nextactivities>"
                                    //    + "<item name=\"{3}\" actors=\"{4}\"/>"
                                    //+ "</nextactivities>"
                                    + "</workflow>"
                                + "</item>"
                                + "<item name = \"Content\" value = \"{2}\" />"
                               + "</parameter>"
                        + "</Root>";
            objectXML = string.Format(objectXML, this.WorkContext.CurrentUser.UserName, tempActivity.ActivityInstanceId.ToString(), curData.StateRemark, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd"));// , nextActivity, nextActors,     curData.ApplyTitle, curData.ID

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
            int intSuccess = 0;
            int.TryParse(success, out intSuccess);
            if (intSuccess > 0)
            {
                result.IsSuccess = true;
            }
            else
            {
                result.IsSuccess = false;
                result.Message = errmsg;
            }
            return result;
        }

        #endregion
    }
}