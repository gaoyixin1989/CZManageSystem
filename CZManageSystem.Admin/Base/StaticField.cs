using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Admin.Base
{
    #region ImportFileType
    public class ImportFileType
    {
        public const string Question = "Question";
        public const string OGSMBase = "OGSMBase";
        public const string OGSMMonth = "OGSMMonth";
        public const string Sporadic = "Sporadic";
        public const string OGSMInfo = "OGSMInfo";
        public const string OGSMPInfo = "OGSMPInfo";
        public const string BoardroomInfo = "BoardroomInfo";
        public const string Proj = "Proj";
        public const string Asset = "Asset";
        public const string Equip = "Equip";
        public const string OGSMElectricity = "OGSMElectricity";
        public const string Consumable = "Consumable";
        public const string ConsumableInput = "ConsumableInput";
        public const string BasestationMonth = "BasestationMonth";
        public const string BasestationYear = "BasestationYear";
        public const string GroupMonth = "GroupMonth";
        public const string GroupYear = "GroupYear";
        public const string Stock = "Stock";
        public const string BasestationChange = "BasestationChange";
        public const string ContractWarning = "ContractWarning";
        public const string NoPaymentWarning = "NoPaymentWarning";
        public const string Electricity = "Electricity";
        public const string InnerVoices = "InnerVoices";
        public const string BoardroomInfoApply = "BoardroomInfoApply";
        public const string CarInfo = "CarInfo";
        public const string CarsReport = "CarsReport";
        public const string DriverFiles = "DriverFiles";
        public const string CarFeeFix = "CarFeeFix";
        public const string CarFeeRent = "CarFeeRent";
        public const string ConsumingDetail = "ConsumingDetail";
        public const string ScrapDetail = "ScrapDetail";
        public const string CancellingDetail = "CancellingDetail";
        public const string BirthControlInfo = "BirthControlInfo";
        public const string BirthControlChildrenInfo = "BirthControlChildrenInfo";
        public const string BirthControlRoster = "BirthControlRoster";
        public const string Market = "Market";//优惠方案
        public const string FixedIncome = "FixedIncome";//导入月固定收入
        public const string BirthControlChildren = "BirthControlChildren";
        public const string BirthControlSurvey = "BirthControlSurvey";
        public const string BirthControlSpouseGE = "BirthControlSpouseGE";
        public const string BirthControlGE = "BirthControlGE";
        public const string BirthControlSingleChildren = "BirthControlSingleChildren";
        public const string HRLzUserInfo = "HRLzUserInfo";
        public const string MonthInfo = "MonthInfo";
        public const string YearInfo = "YearInfo";
        public const string FixedIncomeHistory = "FixedIncomeHistory";//历史工资单
        public const string InvestContractPay = "InvestContractPay";
        public const string InvestMaterials = "InvestMaterials";//物资采购//物资采购导入
        public const string TransferPay = "TransferPay";
        public const string InvestProject = "InvestProject";//投资项目 
        public const string InvestContract = "InvestContract";//合同信息 
        public const string InvestContract_ModifyUser = "InvestContract_ModifyUser";//合同主办人修改导入
        public const string TempEstimate = "TempEstimate";//暂估管理
        public const string CourseIntegral = "CourseIntegral";//
        public const string TeachingIntegral = "TeachingIntegral";//
        public const string IntegralStatic = "IntegralStatic";//
        public const string Estimate = "Estimate";//暂估查询
        public const string InvestAgoEstimate = "InvestAgoEstimate";//历史项目暂估
        public const string OverTimeStatic = "OverTimeStatic";//
        public const string AgoEstimateApplyDetail = "AgoEstimateApplyDetail";//历史项目暂估申请明细
        public const string Annualleave = "Annualleave";//
        public const string MarketOrder_Market = "MarketOrder_Market";//营销方案维护 
        public const string MarketOrder_Number = "MarketOrder_Number";//号码段维护 
        public const string MarketOrder_Product = "MarketOrder_Product";//商品维护 
        public const string UserBalance = "UserBalance";//余额变动明细
        public const string OrderMealUserBaseinfo = "OrderMealUserBaseinfo";//用户与餐卡对应关系
        public const string Holidays = "Holidays";//假日定义 
        public const string Summarizing = "Summarizing";//考勤汇总表头汇总部分
        public const string SummarizingList = "SummarizingList";//考勤汇总列表
        public const string MarketOrder_OrderApply_YX = "MarketOrder_OrderApply_YX";//营销订单工单 

        public const string Detail = "Detail";//
        public const string SubscribeDetail = "SubscribeDetail";//
        public const string Static = "Static";// 
        public const string ComebackDept = "ComebackDept";// 
        public const string ComebackType = "ComebackType";// 
        public const string ComebackChild = "ComebackChild";
        public const string CarFeeYear = "CarFeeYear";//年审费用
        public const string Attendance = "Attendance";//考勤查询 
        public const string Abnormal = "Abnormal";//异常查询

        public const string StockAsset = "StockAsset";//固定资产编码

    }
    #endregion
    #region ExportTempPath
    public class ExportTempPath
    {
        public static readonly string spplicationBase = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        public static readonly string BoardroomInfo = spplicationBase + @"Template\Boardroom\Download.xls";//会议室资料
        public static readonly string OGSMBase = spplicationBase + @"Template\OGSM\OGSMExport.xls";// 
        public static readonly string Proj = spplicationBase + @"Template\Proj\Download.xls";//投资项目管理
        public static readonly string Asset = spplicationBase + @"Template\Stock\AssetDownload.xls";//设备资产管理
        public static readonly string Equip = spplicationBase + @"Template\Equip\Download.xls";//设备信息管理
        public static readonly string Consumable = spplicationBase + @"Template\Consumable\Download.xls";//耗材数据维护

        public static readonly string OGSMMonth = spplicationBase + @"Template\OGSM\OGSMMonthExport.xls";// 
        public static readonly string OGSMInfo = spplicationBase + @"Template\OGSM\OGSMInfoExport.xls";// 
        public static readonly string BasestationMonth = spplicationBase + @"Template\OGSM\BasestationMonthExport.xls";// 
        public static readonly string BasestationYear = spplicationBase + @"Template\OGSM\BasestationYearExport.xls";// 
        public static readonly string Question = spplicationBase + @"Template\Vote\Download.xls";//题目
        public static readonly string Question_ = spplicationBase + @"Template\Vote\Download_.xls";//题目_临时
        public static readonly string GroupMonth = spplicationBase + @"Template\OGSM\GroupMonthExport.xls";// 
        public static readonly string GroupYear = spplicationBase + @"Template\OGSM\GroupYearExport.xls";// 

        public static readonly string BasestationChange = spplicationBase + @"Template\OGSM\BasestationChangeExport.xls";// 
        public static readonly string ContractWarning = spplicationBase + @"Template\OGSM\ContractWarningExport.xls";// 
        public static readonly string NoPaymentWarning = spplicationBase + @"Template\OGSM\NoPaymentWarningExport.xls";// 
        public static readonly string Electricity = spplicationBase + @"Template\OGSM\ElectricityExport.xls";// 
        public static readonly string InnerVoices = spplicationBase + @"Template\InnerVoices\Download.xlsx";//投资项目管理
        public static readonly string BoardroomInfoApply = spplicationBase + @"Template\Boardroom\BoardroomInfoApply.xlsx";//会议室满意度报表
        public static readonly string CarInfo = spplicationBase + @"Template\CarInfo\CarInfoDownload.xlsx";//车辆信息
        public static readonly string CarsReport = spplicationBase + @"Template\CarInfo\CarsReportDownload.xlsx";//用车申请报表

        public static readonly string DriverFiles = spplicationBase + @"Template\DriverFiles\Download.xls";//司机档案
        public static readonly string CarFeeFix = spplicationBase + @"Template\CarFeeFix\Download.xls";//固定费用
        public static readonly string CarFeeRent = spplicationBase + @"Template\CarFeeRent\Download.xls";//租赁费用
        public static readonly string ConsumingDetail = spplicationBase + @"Template\Consumable\ConsumingDetailDownload.xlsx";//领用统计
        public static readonly string ScrapDetail = spplicationBase + @"Template\Consumable\ScrapDetailDownload.xlsx";//报废统计
        public static readonly string CancellingDetail = spplicationBase + @"Template\Consumable\CancellingDetailDownload.xlsx";//退库统计
        public static readonly string BirthControlInfo = spplicationBase + @"Template\BirthControlInfo\Download.xls";// 
        public static readonly string BirthControlRoster = spplicationBase + @"Template\BirthControlRoster\Download.xls";// 
        public static readonly string Market = spplicationBase + @"Template\MarketPlan\Market.xlsx";//优惠方案 
        public static readonly string BirthControlChildren = spplicationBase + @"Template\BirthControlChildren\Download.xls";//
        public static readonly string BirthControlSingleChildren = spplicationBase + @"Template\BirthControlSingleChildren\Download.xls";// 
        public static readonly string BirthControlGE = spplicationBase + @"Template\BirthControlGE\Download.xls";// 
        public static readonly string BirthControlSpouseGE = spplicationBase + @"Template\BirthControlSpouseGE\Download.xls";// 
        public static readonly string BirthControlSurvey = spplicationBase + @"Template\BirthControlSurvey\Download.xls";// 
        public static readonly string FixedIncome = spplicationBase + @"Template\FixedIncome\Download.xls";//导入月固定收入 
        public static readonly string FixedIncome_ = spplicationBase + @"Template\FixedIncome\Download_.xls";//导入月固定收入_临时

        public static readonly string HRLzUserInfo = spplicationBase + @"Template\HRLzUserInfo\Download.xls";//导入劳资员工信息
        public static readonly string MonthInfo = spplicationBase + @"Template\WelfareManage\MonthInfoDownload.xlsx";//
        public static readonly string YearInfo = spplicationBase + @"Template\WelfareManage\YearInfoDownload.xlsx";//
        public static readonly string FixedIncomeHistory = spplicationBase + @"Template\FixedIncomeHistory\Download.xls";//
        public static readonly string InvestContractPay = spplicationBase + @"Template\Invest\ContractPayDownload.xlsx";//
        public static readonly string TransferPay = spplicationBase + @"Template\Invest\TransferPayDownload.xlsx";//
        public static readonly string InvestMaterials = spplicationBase + @"Template\InvestMaterials\Download.xls";//物资采购
        public static readonly string InvestProject = spplicationBase + @"Template\Invest\Download_InvestProject.xls";//投资项目
        public static readonly string InvestProjectQuery = spplicationBase + @"Template\Invest\Download_InvestProjectQuery.xls";//投资项目查询
        public static readonly string InvestContract = spplicationBase + @"Template\Invest\Download_InvestContract.xls";//合同信息
        public static readonly string TempEstimate = spplicationBase + @"Template\Invest\InvestTempEstimateDownload.xlsx";//暂估管理
        public static readonly string CourseIntegral = spplicationBase + @"Template\CourseIntegral\Download.xls";//
        public static readonly string TeachingIntegral = spplicationBase + @"Template\TeachingIntegral\Download.xls";//
        public static readonly string IntegralStatic = spplicationBase + @"Template\IntegralStatic\Download.xls";//
        public static readonly string Estimate = spplicationBase + @"Template\Invest\InvestEstimateDownload.xlsx";//暂估查询
        public static readonly string InvestContractPayQueryPage = spplicationBase + @"Template\Invest\Download_InvestContractPayQueryPage.xls";//已付款查询
        public static readonly string Download_InvestTransferPayQueryPage = spplicationBase + @"Template\Invest\Download_InvestTransferPayQueryPage.xls";//已转资查询
        public static readonly string InvestProjectContractQuery = spplicationBase + @"Template\Invest\Download_InvestProjectContractQuery.xls";//项目合同查询
        public static readonly string InvestAgoEstimate = spplicationBase + @"Template\Invest\Download_InvestAgoEstimate.xls";//历史项目暂估信息

        public static readonly string OverTimeStatic = spplicationBase + @"Template\OverTimeStatic\Download.xls";
        public static readonly string Annualleave = spplicationBase + @"Template\Annualleave\Download.xls";
        public static readonly string UserBalance = spplicationBase + @"Template\UserBalance\Download.xls";
        public static readonly string OrderMealUserBaseinfo = spplicationBase + @"Template\OrderMealUserBaseinfo\Download.xls";
        public static readonly string Holidays = spplicationBase + @"Template\Holidays\Download.xls";
        public static readonly string Summarizing = spplicationBase + @"Template\Summarizing\Download.xlsx";//考勤汇总
        public static readonly string Detail = spplicationBase + @"Template\OrderMeal\DetailDownload.xls";
        public static readonly string Static = spplicationBase + @"Template\OrderMeal\StaticDownload.xls";
        public static readonly string SubscribeDetail = spplicationBase + @"Template\OrderMeal\SubscribeDetailDownload.xls";
        public static readonly string ComebackDept = spplicationBase + @"Template\Comeback\DeptDownload.xls";
        public static readonly string ComebackType = spplicationBase + @"Template\Comeback\TypeDownload.xls";
        public static readonly string ComebackChild = spplicationBase + @"Template\Comeback\ChildDownload.xls";
        public static readonly string CarFeeYear = spplicationBase + @"Template\CarFeeYear\Download.xls"; //年审费用
        public static readonly string Attendance = spplicationBase + @"Template\Attendance\Download.xlsx";//考勤查询
        public static readonly string Abnormal = spplicationBase + @"Template\Abnormal\Download.xlsx";//异常查询
    }
    #endregion
    #region SaveName
    public class SaveName
    {
        public static readonly string spplicationBase = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Template\Save\";
        public static readonly string BoardroomInfo = "会议室资料";//
        public static readonly string OGSMBase = "基站基础数据";//
        public static readonly string OGSMMonth = "基站月度数据";//
        public static readonly string OGSMInfo = "基站电量缴费明细";//
        public static readonly string Proj = "投资项目管理";
        public static readonly string Asset = "设备资产管理";
        public static readonly string Equip = "设备信息管理";
        public static readonly string Consumable = "耗材数据维护";
        public static readonly string Question = "题目";
        public static readonly string BasestationMonth = "基站月度电费情况统计";
        public static readonly string BasestationYear = "基站年度电费情况统计";
        public static readonly string GroupMonth = "分公司月度电费情况统计";
        public static readonly string GroupYear = "分公司年度电费情况统计";

        public static readonly string BasestationChange = "基站变化趋势分析";
        public static readonly string ContractWarning = "合同到期预警统计";
        public static readonly string NoPaymentWarning = "基站无缴费预警统计";
        public static readonly string Electricity = "基站电量分表";
        public static readonly string InnerVoices = "心声汇总";
        public static readonly string BoardroomInfoApply = "会议室满意度报表";
        public static readonly string CarInfo = "车辆信息";
        public static readonly string CarsReport = "用车申请报表";
        public static readonly string DriverFiles = "司机档案信息";
        public static readonly string CarFeeFix = "固定费用";
        public static readonly string CarFeeRent = "租赁费用";
        public static readonly string ConsumingDetail = "领用统计";
        public static readonly string ScrapDetail = "报废统计";
        public static readonly string CancellingDetail = "退库统计";
        public static readonly string BirthControlInfo = "计划生育信息";
        public static readonly string BirthControlRoster = "计划生育花名册信息";
        public static readonly string Market = "优惠方案可办理数统计";

        public static readonly string BirthControlChildren = "计划生育子女信息";
        public static readonly string BirthControlSurvey = "计划生育调查函名单";
        public static readonly string BirthControlSpouseGE = "计划生育员工配偶妇检";
        public static readonly string BirthControlGE = "计划生育员工妇检";
        public static readonly string BirthControlSingleChildren = "计划生育独生子女信息";
        public static readonly string FixedIncome = "月固定收入";
        public static readonly string HRLzUserInfo = "劳资管理员工信息";
        public static readonly string MonthInfo = "月福利";
        public static readonly string YearInfo = "年福利";
        public static readonly string FixedIncomeHistory = "历史工资单";
        public static readonly string InvestContractPay = "合同已付金额";
        public static readonly string TransferPay = "已转资合同";
        public static readonly string InvestMaterials = "物资采购";
        public static readonly string InvestProject = "投资项目";
        public static readonly string InvestContractPayQueryPage = "已付款查询";
        public static readonly string InvestContract = "合同信息";
        public static readonly string TempEstimate = "暂估管理";
        public static readonly string CourseIntegral = "培训积分明细";
        public static readonly string TeachingIntegral = "授课积分明细";
        public static readonly string Estimate = "暂估查询";
        public static readonly string IntegralStatic = "积分统计";
        public static readonly string InvestProjectQuery = "投资项目查询";
        public static readonly string InvestTransferPayQueryPage = "已转资查询";
        public static readonly string InvestProjectContractQuery = "项目合同查询";
        public static readonly string InvestAgoEstimate = "历史项目暂估信息";
        public static readonly string OverTimeStatic = "加班情况汇总表";
        public static readonly string Annualleave = "年休假统计";
        public static readonly string UserBalance = "余额变动明细";
        public static readonly string OrderMealUserBaseinfo = "用户与餐卡对应关系";
        public static readonly string Holidays = "假日定义";
        public static readonly string Detail = "订餐明细统计";//
        public static readonly string SubscribeDetail = "预约订餐明细统计";//
        public static readonly string Static = "订餐汇总";// 
        public static readonly string ComebackDept = "归口部门";
        public static readonly string ComebackType = "归口类型";
        public static readonly string ComebackChild = "归口小项";
        public static readonly string CarFeeYear = "年审费用";
        public static readonly string Summarizing = "考勤汇总";
        public static readonly string Attendance = "考勤查询";
        public static readonly string Abnormal = "异常查询";
    }

    #endregion
    public class DataDic
    {
        public static readonly string CorpName = "所属单位";
        public static readonly string Leader = "口头申请领导";
        public static readonly string UserType = "人员库类型";
    }
    public class Level
    {
        public static readonly string ServiceDepartment = "服务厅人力资源管理员";
        public static readonly string AdministrativeOffice = "科室人力资源管理员";
        public static readonly string Department = "部门人力资源管理员";
        public static readonly string HR = "人力资源管理员";
    }

    public class Admin
    {
        public const string UserName = "admin";

    }
}
