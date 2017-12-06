using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Admin.Models
{
    public class UserQueryBuilder
    {
        //public List<string> DpIdList { get; set; }
        public List<string> DpId { get; set; }
        public string UserName { get; set; }
        public string RealName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public int Status { get; set; }
    }

    public class NoticeQueryBuilder
    {
        public string Title { get; set; }
        public DateTime? Createdtime_Start { get; set; }
        public DateTime? Createdtime_End { get; set; }
    }

    public class DataDicQueryBuilder{
        public string  DDName { get; set; }
        public string searchDDName { get; set; }
    }
    public class SysVersionQueryBuilder
    {
        public string Version { get; set; }
    }
    public class SysLinkQueryBuilder
    {
        public string LinkName { get; set; }
    }

    public class ResourcesQueryBuilder
    {
        public string ResourceId { get; set; }

        public string Alias { get; set; }
    }
    public class IAMS_PendingJobQueryBuilder
    {

    }
    public class IAMS_PendingMsgQueryBuilder
    {

    }
    public class EmployeesQueryBuilder
    {
        public string DpId { get; set; }

        public string name { get; set; }

        public string employerid { get; set; }
        public string billcyc_start { get; set; }
        public string billcyc_end { get; set; }
    }
    public class EquipQueryBuilder
    {
        public string EquipClass { get; set; }
    }
    public class ProjQueryBuilder
    {
        public string ProjSn { get; set; }
        public string ProjName { get; set; }
    }

    public  class CarEvaluationQueryBuilder
    {
        public int? CorpId { get; set; }
    }
    
    public class DriverFilesQueryBuilder
    {
        public int? CorpId { get; set; }

        public string Name { get; set; }
    }

    public class CarFeeFixQueryBuilder
    {
        public int? CorpId { get; set; }

    }

    

    //耗材入库
    public class ConsumableInputListQueryBuilder
    {
        public string Title { get; set; }
        public int? State { get; set; }

        public DateTime? CreateTime_start { get; set; }//申请时间
        public DateTime? CreateTime_end { get; set; }

        public string Code { get; set; }

        public string Operator_Text { get; set; }

        public Guid? Operator { get; set; }
    }
    public class ConsumingListQueryBuilder
    {
        public string Title { get; set; }
        public int? State { get; set; }
    }
    public class ConsumableScrapQueryBuilder
    {
        public string Title { get; set; }
        public int? State { get; set; }
    }
    public class ConsumableSporadicQueryBuilder
    {
        public string Title { get; set; }
        public int? State { get; set; }
    }

    //耗材退库
    public class ConsumableCancellingQueryBuilder
    {
        public string Title { get; set; }
        public int? State { get; set; }
        public string AppPerson { get; set; }
    }

    //耗材调平
    public class ConsumableLevellingQueryBuilder
    {
        public string Title { get; set; }
        public int? State { get; set; }
        public Guid? AppPerson { get; set; }
    }

    //耗材补录归档
    public class ConsumableMakeupQueryBuilder
    {
        public string Title { get; set; }
        public int? State { get; set; }
        public string UsePerson { get; set; }
    }
    
    
    
    
    

    
    
    
    public class CarInfoQueryBuilder
    {
        public string LicensePlateNum { get; set; }

    }
    public class CarFeeYearQueryBuilder
    {
        public string CorpName { get; set; }

    }
    public class CarDispatchQueryBuilder
    {
        public int? CorpId { get; set; }
    }

    public class MarketPlanQueryBuilder
    {
        
        public string Tel { get; set; }
        public string Coding { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
    }


    public class FixedIncomeQueryBuilder
    {
        public string TypeOf { get; set; }
        public DateTime? AppTime_start { get; set; }
        public DateTime? AppTime_end { get; set; }
        public string FixedIncomeProject { get; set; }
        public string CodeOrName { get; set; }
        public string DeptName { get; set; }
        public List<string> PositionRank { get; set; }
        public List<string> Gears { get; set; }
        public Nullable<int> Tantile1 { get; set; }
        public Nullable<int> Tantile2 { get; set; }


    }

    public class FixedIncomeHistoryQueryBuilder
    {
        public string EmployerId { get; set; }

        public string EmployerName { get; set; }

    }

    public class BirthControlLogQueryBuilder
    {
        public string EmployerId { get; set; }
        public string OpType { get; set; }

    }


    public class BirthControlGEQueryBuilder
    {
        public string EmployerId { get; set; }

    }
    public class FixedIncomeToList
    {
        public string EmployerId { get; set; }
        public string AccountingCycle { get; set; }
        public string EmployerName { get; set; }
        public string DeptName { get; set; }
        public string FixedIncomeProject { get; set; }

        public string Value { get; set; }

    }

    public class QueryHRLzUserInfoBuilder
    {
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
    }
    public class PersonalWelfareManageMonthInfoQueryBuilder
    {
        public string Employee { get; set; }
        public string EmployeeName { get; set; }
        public string CYearAndMonth { get; set; }
        public string WelfarePackage { get; set; }

    }
    public class PersonalWelfareManageYearInfoQueryBuilder
    {
        public string Employee { get; set; }
        public string EmployeeName { get; set; }
        public string CYear { get; set; }

    }
    public class PersonalWelfareInfoQueryBuilder
    {
        public string Year { get; set; }

    }

    /// <summary>
    /// 物资采购查询条件
    /// </summary>
    public class InvestMaterialsQueryBuilder
    {
        /// <summary>
        /// 项目编号
        /// </summary>
        public string ProjectID { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 合同编号
        /// </summary>
        public string ContractID { get; set; }
        /// <summary>
        /// 合同名称
        /// </summary>
        public string ContractName { get; set; }
        /// <summary>
        /// 订单说明
        /// </summary>
        public string OrderDesc { get; set; }
        /// <summary>
        /// 从订单录入金额SUM
        /// </summary>
        public Nullable<decimal> OrderInPay_Start { get; set; }
        /// <summary>
        /// 至订单录入金额SUM
        /// </summary>
        public Nullable<decimal> OrderInPay_End { get; set; }
        /// <summary>
        /// 从订单接收金额SUM
        /// </summary>
        public Nullable<decimal> OrderOutSum_Start { get; set; }
        /// <summary>
        /// 至订单接收金额SUM
        /// </summary>
        public Nullable<decimal> OrderOutSum_End { get; set; }
        /// <summary>
        /// 从未接收设备(元)
        /// </summary>
        public Nullable<decimal> OrderUnReceived_Start { get; set; }
        /// <summary>
        /// 至未接收设备(元)
        /// </summary>
        public Nullable<decimal> OrderUnReceived_End { get; set; }
        /// <summary>
        /// 供应商
        /// </summary>
        public string Apply { get; set; }
        /// 订单编号
        /// </summary> 
        public string OrderID { get; set; }


    }


    //已转资导入
    public class InvestTransferPayQueryBuilder
    {
        public string ProjectID { get; set; }//项目编号
        public string ContractID { get; set; }//合同编号
        public string IsTransfer { get; set; }//是否已转资
        public decimal? TransferPay_start { get; set; }//转资金额
        public decimal? TransferPay_end { get; set; }
    }


    //暂估管理
    public class InvestTempEstimateQueryBuilder
    {
        public string ProjectID { get; set; }//项目编号
        public string ProjectName { get; set; }//项目名称
        public string ContractID { get; set; }//合同编号
        public string ContractName { get; set; }//合同名称
    }


    public class AttendanceQueryBuilder
    {
        public string UserIds { get; set; }
        public string DeptIds { get; set; }
        public DateTime? SkTime_Start { get; set; }
        public DateTime? SkTime_End { get; set; }

    }

    public class CheckAttendanceQueryBuilder
    {
        public DateTime? AtDate_Start { get; set; }
        public DateTime? AtDate_End { get; set; }

    }
    //public class AttendanceListQueryBuilder
    //{
    //    public string EmployeeId { get; set; }
    //    public string IpOn { get; set; }
    //    public string IpOff { get; set; }
    //    public DateTime? AtDate_Start { get; set; }
    //    public DateTime? AtDate_End { get; set; }
    //    public List<Guid> UserId { get; set; }
    //    public List<string> DpId { get; set; }
    //    public List<string> RealName { get; set; }
    //    public List<string> DpName { get; set; }

    //}
    public class CorrectApplyQueryBuilder
    {
        public string Title { get; set; }
        public int? State { get; set; }
    }

    /// <summary>
    /// 年度投资金额查询
    /// </summary>
    public class ProjectYearTotalQueryQueryBuilder
    {
        public int? Year { get; set; }//年度
        public string ProjectID { get; set; }//项目编号
        public string ProjectName { get; set; }//项目名称
        public decimal? YearTotal_start { get; set; }//投资金额
        public decimal? YearTotal_end { get; set; }//投资金额
    }

    public class VacationConfigQueryBuilder
    {
        public string VacationName { get; set; }

    }
    public class HolidayQueryBuilder
    {
        public int? YearDate { get; set; }
        public string EmployeeId { get; set; }
        public string DpId { get; set; }
        public Guid? UserId { get; set; }

    }

    public class CarsReportBuilder
    {
        public DateTime? StartTime { get; set; }
        public DateTime? TimeOut { get; set; }
        public int ApplyType { get; set; }

    }

    //假日定义查询
    public class AttendanceHolidayQueryBuilder
    {
        public string HolidayName { get; set; }

    }


    public class SummarizingQueryBuilder
    {
        public string EmployeeId { get; set; }
        public string UserIds { get; set; }
        public string DeptIds { get; set; }
        public int?  Year { get; set; }
        public int?  Month { get; set; }
        public int? UserType { get; set; }

    }
    public class ComebackApplyQueryBuilder
    {
        public string Title { get; set; }
        public int? Status { get; set; }

    }

    public class MarketOrder_MarketQueryBuilder
    {
        public string Market { get; set; }//名称
        public string Order { get; set; }//序号
        public bool? isJK { get; set; }//是否家宽业务
    }

    public class KnowledgeQueryBuilder
    {
        public string Title { get; set; }
        public DateTime? Createdtime_Start { get; set; }
        public DateTime? Createdtime_End { get; set; }
    }

}

