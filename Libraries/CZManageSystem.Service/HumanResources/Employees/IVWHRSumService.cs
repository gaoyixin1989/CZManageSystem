
using System.Collections.Generic;
using CZManageSystem.Data.Domain.HumanResources.Employees;
using ZManageSystem.Core;

namespace CZManageSystem.Service.HumanResources.Employees
{
    public interface IVWHRSumService : IBaseService<VWHRSum>
    {
        IEnumerable<dynamic> GetListForPaging(out int count, string employeeid, string employeename, string dpName, string billcyc_start, string billcyc_end, int pageIndex = 0, int pageSize = int.MaxValue);
        string GetBillcyc(double? m);
    }
}