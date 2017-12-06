using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Domain.Composite
{
    public partial class OGSMBasestationMonthStatic
    {
        public Nullable<int> PAY_MON { get; set; }
        public string Group_Name { get; set; }
        public string BaseStation { get; set; }
        public Nullable<decimal> CHG_CNT { get; set; }
        public Nullable<decimal> Avg_CHG { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> AvgMoney { get; set; }
        public Nullable<decimal> Prev_Amount { get; set; }
        public string Rate { get; set; }
        public Nullable<decimal> CMShare { get; set; }
        public Nullable<decimal> CUShare { get; set; }
        public Nullable<decimal> CTShare { get; set; }
    }
    public class OGSMContractWarningQueryBuilder
    {
        public string USR_NBR { get; set; }
        public string Group_Name { get; set; }
        public string BaseStation { get; set; }
        public string PropertyRight { get; set; }
        public string PowerType { get; set; }
        public string WarningSituation { get; set; }
    }
    public class OGSMNoPaymentWarningQueryBuilder
    {
        public string USR_NBR { get; set; }
        public string Group_Name { get; set; }
        public string BaseStation { get; set; }
        public string PropertyRight { get; set; }
        public string PowerType { get; set; }
        public string PaymentSituation { get; set; }
    }
    public class OGSMBasestationChangeQueryBuilder
    {
        public string USR_NBR { get; set; }
        public string Group_Name { get; set; }
        public string BaseStation { get; set; }
        public string PropertyRight { get; set; }
        public string PowerType { get; set; }
        public string PAY_MON_Start { get; set; }
        public string PAY_MON_End { get; set; }
    }
    public class OGSMGroupMonthStaticQueryBuilder
    {
        public string Group_Name { get; set; }
        public string BaseStation { get; set; }
        public string PowerType { get; set; }
        public string PAY_MON_Start { get; set; }
        public string PAY_MON_End { get; set; }
    }
    public class OGSMBasestationMonthStaticQueryBuilder
    {
        public string Group_Name { get; set; }
        public string BaseStation { get; set; }
        public string PowerType { get; set; }
        public string PAY_MON_Start { get; set; }
        public string PAY_MON_End { get; set; }
    }
    public partial class OGSMBasestationYearStatic
    {
        public Nullable<int> Year { get; set; }
        public string Group_Name { get; set; }
        public string BaseStation { get; set; }
        public Nullable<decimal> CHG_CNT { get; set; }
        public Nullable<decimal> Avg_CHG { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> AvgMoney { get; set; }
        public Nullable<decimal> CMShare { get; set; }
        public Nullable<decimal> CUShare { get; set; }
        public Nullable<decimal> CTShare { get; set; }
    }
    public partial class OGSMGroupMonthStatic
    {
        public Nullable<int> PAY_MON { get; set; }
        public string Group_Name { get; set; }
        public Nullable<decimal> CHG_CNT { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> AvgAmount { get; set; }
        public Nullable<decimal> CMShare { get; set; }
        public Nullable<decimal> CUShare { get; set; }
        public Nullable<decimal> CTShare { get; set; }
        public Nullable<decimal> CMPropertyRightMoney { get; set; }
        public Nullable<decimal> CUPropertyRightMoney { get; set; }
        public Nullable<decimal> CTPropertyRightMoney { get; set; }
    }


    public partial class OGSMGroupYearStatic
    {
        public Nullable<int> Year { get; set; }
        public string Group_Name { get; set; }
        public Nullable<decimal> CHG_CNT { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> AvgAmount { get; set; }
        public Nullable<decimal> CMShare { get; set; }
        public Nullable<decimal> CUShare { get; set; }
        public Nullable<decimal> CTShare { get; set; }
        public Nullable<decimal> CMPropertyRightMoney { get; set; }
        public Nullable<decimal> CUPropertyRightMoney { get; set; }
        public Nullable<decimal> CTPropertyRightMoney { get; set; }
    }

    public partial class OGSMGroupBasestationStatic
    {
        public string Group_Name { get; set; }
        public string CommonCnt { get; set; }
        public string PrivateCnt { get; set; }
        public string BasestationCnt { get; set; }
        public string PrivatePercent { get; set; }
    }

    public partial class OGSMBasestationChangeStatic
    {
        public Nullable<int> PAY_MON { get; set; }
        public string Group_Name { get; set; }
        public string Town { get; set; }
        public string USR_NBR { get; set; }
        public string BaseStation { get; set; }
        public string PowerStation { get; set; }
        public string PowerType { get; set; }
        public string PropertyRight { get; set; }
        public Nullable<decimal> Money { get; set; }
        public Nullable<decimal> PrevMoney { get; set; }
        public Nullable<decimal> PrevYearMonth { get; set; }
        public string ChainChanges { get; set; }
        public string YearBasis { get; set; }
    }
    public partial class OGSMContractWarningStatic
    {
        public string Town { get; set; }
        public string Group_Name { get; set; }
        public string USR_NBR { get; set; }
        public string BaseStation { get; set; }
        public string PowerType { get; set; }
        public string PropertyRight { get; set; }

        public Nullable<System.DateTime> ContractStartTime { get; set; }
        public Nullable<System.DateTime> ContractEndTime { get; set; }
        public string PowerStation { get; set; }
        public string WarningSituation { get; set; }
    }

    public partial class OGSMBasestationNoPaymentStatic
    {
        public Nullable<int> PAY_MON { get; set; }
        public string Town { get; set; }
        public string Group_Name { get; set; }
        public string USR_NBR { get; set; }
        public string BaseStation { get; set; }
        public string PowerType { get; set; }
        public string PropertyRight { get; set; }
        public string PowerStation { get; set; }
    }
}
