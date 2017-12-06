using CZManageSystem.Data.Domain.Composite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

namespace CZManageSystem.Service.Composite
{
    public interface IOGSMStaticService : IBaseService<OGSMBasestationMonthStatic>
    {
        IList<object> GetForPagingByCondition
            (out int count, int pageIndex = 0, int pageSize = int.MaxValue, OGSMBasestationMonthStaticQueryBuilder obj = null);
        IList<object> GetBasestationYearForPagingByCondition
            (out int count, int pageIndex = 0, int pageSize = int.MaxValue, OGSMBasestationMonthStaticQueryBuilder obj = null);
        IList<object> GetGroupMonthForPagingByCondition
            (out int count, int pageIndex = 0, int pageSize = int.MaxValue, OGSMGroupMonthStaticQueryBuilder obj = null);

        IList<object> GetGroupYearForPagingByCondition
            (out int count, int pageIndex = 0, int pageSize = int.MaxValue, OGSMGroupMonthStaticQueryBuilder obj = null);

        IList<object> GetGroupBasestationForPagingByCondition
            (out int count, int pageIndex = 0, int pageSize = int.MaxValue);

        IList<object> GetBasestationChangeForPagingByCondition
            (out int count, int pageIndex = 0, int pageSize = int.MaxValue, OGSMBasestationChangeQueryBuilder obj = null);
        IList<object> GetContractWarningForPagingByCondition
            (out int count, int pageIndex = 0, int pageSize = int.MaxValue, OGSMContractWarningQueryBuilder obj = null);

        IList<object> GetBasestationNoPaymentWarningForPagingByCondition
            (out int count, int pageIndex = 0, int pageSize = int.MaxValue, OGSMNoPaymentWarningQueryBuilder obj = null);
    }
}
