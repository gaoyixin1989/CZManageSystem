using CZManageSystem.Admin.Base;
using CZManageSystem.Admin.Models;
using CZManageSystem.Service.Administrative;
using CZManageSystem.Service.Administrative.BirthControl;
using CZManageSystem.Service.Administrative.Dinning;
using CZManageSystem.Service.Administrative.VehicleManages;
using CZManageSystem.Service.CollaborationCenter.Invest;
using CZManageSystem.Service.CollaborationCenter.MarketOrder;
using CZManageSystem.Service.Composite;
using CZManageSystem.Service.HumanResources.AnnualLeave;
using CZManageSystem.Service.HumanResources.Employees;
using CZManageSystem.Service.HumanResources.Integral;
using CZManageSystem.Service.ITSupport;
using CZManageSystem.Service.MarketPlan;
using CZManageSystem.Service.WelfareManage;
using CZManageSystem.Service.HumanResources.Attendance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CZManageSystem.Service.OperatingFloor.ComeBack;

namespace CZManageSystem.Admin.Controllers.Uploadify
{
    public class ImportController : BaseController
    {
        #region Field
        static string _saveName { get; set; }
        static string _serverPath { get; set; }
        static readonly string tempPath = "~/Template/";
        IVoteQuestionTempService voteQuestionTempService = new VoteQuestionTempService();
        IOGSMService OGSMTempService = new OGSMService();
        IOGSMMonthService OGSMMonthTempService = new OGSMMonthService();
        IOGSMInfoService OGSMInfoTempService = new OGSMInfoService();
        IOGSMElectricityService OGSMElectricityTempService = new OGSMElectricityService();
        IBoardroomInfoService BoardroomInfoService = new BoardroomInfoService();
        IConsumable_SporadicDetailService _consumable_SporadicDetailService = new Consumable_SporadicDetailService();
        IProjService _sysProjService = new ProjService();
        IEquipService _sysEquipService = new EquipService();
        IStockService _sysStockService = new StockService();
        IConsumableService consumableService = new ConsumableService();
        ICarInfoService _carInfoService = new CarInfoService();
        IDriverFilesService _driverFilesService = new DriverFilesService();
        ICarFeeFixService _carFeeFixService = new CarFeeFixService();
        IBirthControlInfoService _birthcontrolinfoService = new BirthControlInfoService();
        IBirthControlChildrenInfoService _birthcontrolchildreninfoService = new BirthControlChildrenInfoService();
        IBirthControlRosterService _BirthControlRsterService = new BirthControlRsterService();
        IUcs_MarketPlan2Service _ucs_MarketPlan2Service = new Ucs_MarketPlan2Service();
        IGdPayService gdPayService = new GdPayService();
        IHRLzUserInfoService hrLzUserInfoService = new HRLzUserInfoService();
        IPersonalWelfareManageMonthInfoService _personalWelfareManageMonthInfoService = new PersonalWelfareManageMonthInfoService();
        IPersonalWelfareManageYearInfoService _personalWelfareManageYearInfoService = new PersonalWelfareManageYearInfoService();
        IInvestContractPayService _investContractPayService = new InvestContractPayService();
        InvestTransferPayService _investTransferPayService = new InvestTransferPayService();
        IInvestMaterialsService investMaterialsService = new InvestMaterialsService();
        IHRVacationCoursesService _hrvacationcourseservice = new HRVacationCoursesService();
        IHRVacationTeachingService _hrvactiontechingservice = new HRVacationTeachingService();
        IHRAnnualLeaveService _hrannualleaveservice = new HRAnnualLeaveService();
        IOrderMeal_UserBaseinfoService _userbaseinfoservice = new OrderMeal_UserBaseinfoService();
        IHRHolidaysService hrHolidaysService = new HRHolidaysService();
        #endregion
        // GET: Uploadify
        public ActionResult Index(string type = null, string data = null)
        {
            ViewData["type"] = type; //类型
            ViewData["data"] = data; //参数
            Set(type);
            ViewData["saveName"] = _saveName;
            return View();
        }
        public ActionResult ImportFiles(string type = null)
        {
            dynamic result = new SystemResult() { IsSuccess = false, Message = "导入失败" };

            if (Request.Files.Count <= 0)
                return Json(result);
            HttpPostedFileBase file = Request.Files[0];
            #region 分支处理
            switch (type)
            {
                case ImportFileType.Question:
                    result = voteQuestionTempService.ImportQuestion(file.InputStream, WorkContext.CurrentUser);
                    break;
                case ImportFileType.OGSMBase:
                    result = OGSMTempService.ImportOGSMBase(file.InputStream, WorkContext.CurrentUser);
                    break;
                case ImportFileType.OGSMMonth:
                    result = OGSMMonthTempService.ImportOGSMMonth(file.InputStream, WorkContext.CurrentUser);
                    break;
                case ImportFileType.Sporadic:
                    {
                        var data = Request.Form["data"];
                        result = _consumable_SporadicDetailService.ImportSporadicDetail(file.InputStream, WorkContext.CurrentUser, Guid.Parse(data));
                    }
                    break;
                case ImportFileType.OGSMPInfo:
                    result = OGSMInfoTempService.ImportOGSMPInfo(file.InputStream, WorkContext.CurrentUser);
                    break;
                case ImportFileType.OGSMInfo:
                    result = OGSMInfoTempService.ImportOGSMInfo(file.InputStream, WorkContext.CurrentUser);
                    break;
                case ImportFileType.BoardroomInfo:
                    result = BoardroomInfoService.ImportBoardroomInfo(file.InputStream, WorkContext.CurrentUser);
                    break;
                case ImportFileType.Proj:
                    result = _sysProjService.ImportProj(file.InputStream, WorkContext.CurrentUser);
                    break;
                case ImportFileType.Equip:
                    result = _sysEquipService.ImportEquip(file.InputStream, WorkContext.CurrentUser);
                    break;
                case ImportFileType.OGSMElectricity:
                    result = OGSMElectricityTempService.ImportOGSMBase(file.InputStream, WorkContext.CurrentUser);
                    break;
                case ImportFileType.Consumable:
                    result = consumableService.ImportConsumable(file.InputStream, WorkContext.CurrentUser);
                    break;
                case ImportFileType.ConsumableInput:
                    result = consumableService.ImportConsumableInput(file.InputStream, WorkContext.CurrentUser);
                    break;
                case ImportFileType.Stock:
                    result = _sysStockService.ImportStock(file.InputStream, WorkContext.CurrentUser);
                    break;
                case ImportFileType.CarInfo:
                    result = _carInfoService.ImportCarInfo(file.InputStream, WorkContext.CurrentUser);
                    break;
                case ImportFileType.DriverFiles:
                    result = _driverFilesService.ImportDriverFiles(file.InputStream, WorkContext.CurrentUser);
                    break;
                case ImportFileType.CarFeeFix:
                    result = _carFeeFixService.ImportDriverFiles(file.InputStream, WorkContext.CurrentUser);
                    break;
                case ImportFileType.BirthControlInfo:
                    result = _birthcontrolinfoService.ImportBirthControlInfo(file.InputStream, WorkContext.CurrentUser);
                    break;
                case ImportFileType.BirthControlChildrenInfo:
                    result = _birthcontrolchildreninfoService.ImportBirthControlChildrenInfo(file.InputStream, WorkContext.CurrentUser);
                    break;
                case ImportFileType.BirthControlRoster:
                    result = _BirthControlRsterService.ImportBirthControlRosterInfo(file.InputStream, WorkContext.CurrentUser);
                    break;
                case ImportFileType.Market:
                    result = _ucs_MarketPlan2Service.ImportDelUcs_MarketPlan2(file.InputStream, WorkContext.CurrentUser);
                    break;
                case ImportFileType.FixedIncome:
                    result = gdPayService.Import(file.InputStream);
                    break;
                case ImportFileType.HRLzUserInfo:
                    result = hrLzUserInfoService.Import(file.InputStream, WorkContext.CurrentUser.RealName);
                    break;
                case ImportFileType.MonthInfo:
                    result = _personalWelfareManageMonthInfoService.Import(file.InputStream, WorkContext.CurrentUser);
                    break;
                case ImportFileType.YearInfo:
                    result = _personalWelfareManageYearInfoService.Import(file.InputStream, WorkContext.CurrentUser);
                    break;
                case ImportFileType.InvestContractPay:
                    result = _investContractPayService.Import(file.InputStream, WorkContext.CurrentUser);
                    break;
                case ImportFileType.TransferPay:
                    result = _investTransferPayService.Import(file.InputStream, WorkContext.CurrentUser);
                    break;
                case ImportFileType.InvestProject:
                    result = new InvestProjectService().Import(file.InputStream, WorkContext.CurrentUser);
                    break;
                case ImportFileType.InvestMaterials:
                    result = investMaterialsService.Import(file.InputStream);
                    break;
                case ImportFileType.InvestContract:
                    result = new InvestContractService().Import(file.InputStream, WorkContext.CurrentUser);
                    break;
                case ImportFileType.InvestContract_ModifyUser:
                    result = new InvestContractService().Import_ModifyUser(file.InputStream, WorkContext.CurrentUser);
                    break;
                case ImportFileType.TempEstimate:
                    result = new InvestTempEstimateService().Import(file.InputStream, WorkContext.CurrentUser);
                    break;
                case ImportFileType.CourseIntegral:
                    result = _hrvacationcourseservice.ImportVacationCourses(file.FileName, file.InputStream, WorkContext.CurrentUser);
                    break;
                case ImportFileType.TeachingIntegral:
                    result = _hrvactiontechingservice.ImportVacationCourses(file.FileName, file.InputStream, WorkContext.CurrentUser);
                    break;
                case ImportFileType.InvestAgoEstimate://历史项目暂估
                    result = new InvestAgoEstimateService().Import(file.InputStream, WorkContext.CurrentUser);
                    break;
                case ImportFileType.AgoEstimateApplyDetail://历史项目暂估申请明细
                    {
                        var data = Request.Form["data"];
                        result = new InvestAgoEstimateApplyDetailService().Import(file.InputStream, WorkContext.CurrentUser, Guid.Parse(data));
                    }
                    break;
                case ImportFileType.Annualleave:
                    result = _hrannualleaveservice.ImportHRAnnualLeave(file.FileName, file.InputStream, WorkContext.CurrentUser);
                    break;
                case ImportFileType.MarketOrder_Market://营销方案维护
                    result = new MarketOrder_MarketService().Import(file.InputStream, WorkContext.CurrentUser);
                    break;
                case ImportFileType.MarketOrder_Number://号码段维护
                    result = new MarketOrder_NumberService().Import(file.InputStream, WorkContext.CurrentUser);
                    break;
                case ImportFileType.MarketOrder_Product://商品维护
                    result = new MarketOrder_ProductService().Import(file.InputStream, WorkContext.CurrentUser);
                    break;
                case ImportFileType.OrderMealUserBaseinfo:
                    result = _userbaseinfoservice.ImportOrderMealUserBaseinfo(file.FileName, file.InputStream);
                    break;
                case ImportFileType.Holidays:
                    result = hrHolidaysService.Import(WorkContext .CurrentUser , file.InputStream);
                    break;
                case ImportFileType.MarketOrder_OrderApply_YX://商品维护
                    result = new MarketOrder_OrderApplyService().Import(file.InputStream, WorkContext.CurrentUser);
                    break;
                case ImportFileType.ComebackDept://
                    result = new ComebackDeptService().Import(WorkContext.CurrentUser , file.InputStream);
                    break;
                case ImportFileType.ComebackType://
                    result = new ComebackTypeService().Import(WorkContext.CurrentUser, file.InputStream);
                    break;
                case ImportFileType.ComebackChild://
                    result = new ComebackChildService().Import(WorkContext.CurrentUser, file.InputStream);
                    break; 
                case ImportFileType.CarFeeYear://年审费用
                    result = new CarFeeYearService().Import(WorkContext.CurrentUser, file.InputStream);
                    break;
                case ImportFileType.StockAsset:
                    {
                        var data = Request.Form["data"];
                        result = new StockAssetService().Import(file.InputStream, WorkContext.CurrentUser, int.Parse(data));
                    }
                    break;
                default:
                    break;
            }
            #endregion
            return Json(result);
        }
        /// <summary>
        /// 下载
        /// </summary>
        /// <returns></returns>
        public ActionResult Download(string type)
        {
            Set(type);
            var path = Server.MapPath(_serverPath);//服务器中的文件路径
            return File(path, "application/vnd.ms-excel	application/x-excel", Url.Content(_saveName));
        }
        #region 方法
        void Set(string type)
        {
            switch (type)//分支处理。
            {
                case ImportFileType.Question:
                    _serverPath = tempPath + "Vote/QuestionTemp.xls";//路径
                    _saveName = "题目模板.xls";//下载保存时默认名 
                    break;
                case ImportFileType.OGSMBase:
                    _serverPath = tempPath + "OGSM/OGSM.xls";//路径
                    _saveName = "基站基础数据模板.xls";//下载保存时默认名 
                    break;
                case ImportFileType.OGSMMonth:
                    _serverPath = tempPath + "OGSM/OGSMMonth.xls";//路径
                    _saveName = "基站月度数据模板.xls";//下载保存时默认名 
                    break;
                case ImportFileType.OGSMPInfo:
                    _serverPath = tempPath + "OGSM/OGSMPInfo.xls";//路径
                    _saveName = "基站电量私电缴费明细模板.xls";//下载保存时默认名 
                    break;
                case ImportFileType.OGSMInfo:
                    _serverPath = tempPath + "OGSM/OGSMInfo.xls";//路径
                    _saveName = "基站电量缴费明细模板.xls";//下载保存时默认名 
                    break;
                case ImportFileType.BoardroomInfo:
                    _serverPath = tempPath + "Boardroom/BoardroomInfo.xls";//路径
                    _saveName = "会议室资料.xls";//下载保存时默认名 
                    break;
                case ImportFileType.OGSMElectricity:
                    _serverPath = tempPath + "OGSM/OGSMElectricity.xls";//路径
                    _saveName = "基站分表模板.xls";//下载保存时默认名 
                    break;
                case ImportFileType.Proj:
                    _serverPath = tempPath + "Proj/ProjList.xlsx";//路径
                    _saveName = "投资项目管理.xlsx";//下载保存时默认名 
                    break;
                case ImportFileType.Equip:
                    _serverPath = tempPath + "Equip/EquipList.xlsx";//路径
                    _saveName = "设备信息管理.xlsx";//下载保存时默认名 
                    break;
                case ImportFileType.Sporadic:
                    _serverPath = tempPath + "Sporadic/SporadicList.xlsx";//路径
                    _saveName = "零星耗材.xlsx";//下载保存时默认名 
                    break;
                case ImportFileType.Consumable:
                    _serverPath = tempPath + "Consumable/Consumable.xls";//路径
                    _saveName = "基础数据维护.xls";//下载保存时默认名 
                    break;
                case ImportFileType.ConsumableInput:
                    _serverPath = tempPath + "Consumable/ConsumableInput.xls";//路径
                    _saveName = "耗材导入模板.xls";//下载保存时默认名 
                    break;
                case ImportFileType.Stock:
                    _serverPath = tempPath + "Stock/StockList.xlsx";//路径
                    _saveName = "设备入库.xlsx";//下载保存时默认名 
                    break;
                case ImportFileType.CarInfo:
                    _serverPath = tempPath + "CarInfo/CarInfoTemp.xlsx";//路径
                    _saveName = "车辆信息.xlsx";//下载保存时默认名 
                    break;
                case ImportFileType.DriverFiles:
                    _serverPath = tempPath + "DriverFiles/DriverFiles.xls";//路径
                    _saveName = "司机档案信息.xls";//下载保存时默认名 
                    break;
                case ImportFileType.CarFeeFix:
                    _serverPath = tempPath + "CarFeeFix/CarFeeFix.xlsx";//路径
                    _saveName = "固定费用.xlsx";//下载保存时默认名 
                    break;
                case ImportFileType.BirthControlInfo:
                    _serverPath = tempPath + "BirthControlInfo/BirthControlInfo.xls";//路径
                    _saveName = "计划生育信息.xls";//下载保存时默认名 
                    break;
                case ImportFileType.BirthControlChildrenInfo:
                    _serverPath = tempPath + "BirthControlInfo/BirthControlChildrenInfo.xls";//路径
                    _saveName = "计划生育子女信息.xls";//下载保存时默认名 
                    break;
                case ImportFileType.BirthControlRoster:
                    _serverPath = tempPath + "BirthControlRoster/BirthControlRoster.xls";//路径
                    _saveName = "计划生育花名册信息.xls";//下载保存时默认名 
                    break;
                case ImportFileType.Market:
                    _serverPath = tempPath + "MarketPlan/MarketDel.xlsx";//路径
                    _saveName = "导入删除优惠方案.xls";//下载保存时默认名 
                    break;
                case ImportFileType.FixedIncome:
                    _serverPath = tempPath + "FixedIncome/FixedIncome.xls";//路径
                    _saveName = "月固定收入导入模板.xls";//下载保存时默认名 
                    break;
                case ImportFileType.HRLzUserInfo:
                    _serverPath = tempPath + "HRLzUserInfo/HRLzUserInfo.xls";//路径
                    _saveName = "劳资管理员工信息导入模板.xls";//下载保存时默认名 
                    break;
                case ImportFileType.MonthInfo:
                    _serverPath = tempPath + "WelfareManage/MonthInfo.xls";//路径
                    _saveName = "月福利模板.xls";//下载保存时默认名 
                    break;
                case ImportFileType.YearInfo:
                    _serverPath = tempPath + "WelfareManage/YearInfo.xls";//路径
                    _saveName = "年福利模板.xls";//下载保存时默认名 
                    break;
                case ImportFileType.InvestContractPay:
                    _serverPath = tempPath + "Invest/ContractPayInfo.xls";//路径
                    _saveName = "已付款导入模板.xls";//下载保存时默认名 
                    break;
                case ImportFileType.TransferPay:
                    _serverPath = tempPath + "Invest/TransferPayInfo.xls";//路径
                    _saveName = "已转资导入模板.xls";//下载保存时默认名 
                    break;
                case ImportFileType.InvestProject:
                    _serverPath = tempPath + "Invest/InvestProjectInfo.xls";//路径
                    _saveName = "投资项目导入模板.xls";//下载保存时默认名 
                    break;
                case ImportFileType.InvestMaterials:
                    _serverPath = tempPath + "InvestMaterials/InvestMaterials.xls";//路径
                    _saveName = "物资采购导入导入模板.xls";//下载保存时默认名 
                    break;
                case ImportFileType.InvestContract:
                    _serverPath = tempPath + "Invest/InvestContractInfo.xls";//路径
                    _saveName = "合同信息导入模板.xls";//下载保存时默认名 
                    break;
                case ImportFileType.InvestContract_ModifyUser:
                    _serverPath = tempPath + "Invest/InvestContract_ModifyUser.xls";//路径
                    _saveName = "合同主办人修改导入模板.xls";//下载保存时默认名 
                    break;
                case ImportFileType.TempEstimate:
                    _serverPath = tempPath + "Invest/InvestTempEstimateInfo.xls";//路径
                    _saveName = "暂估导入模板.xls";//下载保存时默认名 
                    break;
                case ImportFileType.CourseIntegral:
                    _serverPath = tempPath + "CourseIntegral/Training_schedule.xls";//路径
                    _saveName = "培训积分导入模板.xls";//下载保存时默认名 
                    break;
                case ImportFileType.TeachingIntegral:
                    _serverPath = tempPath + "TeachingIntegral/Instruction_list_template.xls";//路径
                    _saveName = "授课积分导入模板.xls";//下载保存时默认名 
                    break;
                case ImportFileType.InvestAgoEstimate:
                    _serverPath = tempPath + "Invest/InvestAgoEstimateInfo.xls";//路径
                    _saveName = "历史项目暂估导入模板.xls";//下载保存时默认名 
                    break;
                case ImportFileType.AgoEstimateApplyDetail://历史项目暂估申请明细
                    _serverPath = tempPath + "Invest/AgoEstimateApplyDetail.xls";//路径
                    _saveName = "历史项目暂估申请明细导入模板.xls";//下载保存时默认名 
                    break;
                case ImportFileType.Annualleave:
                    _serverPath = tempPath + "AnnualLeave/Annual_leave_template.xls";//路径
                    _saveName = "年休假导入模板.xls";//下载保存时默认名 
                    break;
                case ImportFileType.MarketOrder_Market:
                    _serverPath = tempPath + "MarketOrder/MarketOrder_MarketInfo.xls";//路径
                    _saveName = "营销方案维护导入模板.xls";//下载保存时默认名 
                    break;
                case ImportFileType.MarketOrder_Number:
                    _serverPath = tempPath + "MarketOrder/MarketOrder_NumberInfo.xls";//路径
                    _saveName = "号码段维护导入模板.xls";//下载保存时默认名 
                    break;
                case ImportFileType.MarketOrder_Product:
                    _serverPath = tempPath + "MarketOrder/MarketOrder_ProductInfo.xls";//路径
                    _saveName = "商品维护导入模板.xls";//下载保存时默认名 
                    break;
                case ImportFileType.OrderMealUserBaseinfo:
                    _serverPath = tempPath + "OrderMealUserBaseinfo/OrderMealUserBaseinfo.xls";//路径
                    _saveName = "食堂用户导入模板.xls";//下载保存时默认名 
                    break;
                case ImportFileType.Holidays :
                    _serverPath = tempPath + "Holidays/Holidays.xls";//路径
                    _saveName = "假日定义导入模板.xls";//下载保存时默认名 
                    break;
                case ImportFileType.MarketOrder_OrderApply_YX:
                    _serverPath = tempPath + "MarketOrder/MarketOrder_OrderApply_YX.xls";//路径
                    _saveName = "营销订单导入模板.xls";//下载保存时默认名 
                    break;
                case ImportFileType.ComebackDept:
                    _serverPath = tempPath + "Comeback/DeptInfo.xls";//路径
                    _saveName = "部门预算管理导入模板.xls";//下载保存时默认名 
                    break;
                case ImportFileType.ComebackType:
                    _serverPath = tempPath + "Comeback/TypeInfo.xls";//路径
                    _saveName = "成本分配导入模板.xls";//下载保存时默认名 
                    break;
                case ImportFileType.ComebackChild:
                    _serverPath = tempPath + "Comeback/ChildInfo.xls";//路径
                    _saveName = "归口小项导入模板.xls";//下载保存时默认名 
                    break;
                case ImportFileType.StockAsset:
                    _serverPath = tempPath + "Stock/StockAsset.xls";//路径
                    _saveName = "固定资产编码导入模板.xls";//下载保存时默认名 
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}