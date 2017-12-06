using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.HumanResources.Employees;
using System;
using System.Collections.Generic;
using System.Data; 
using System.Linq; 

namespace CZManageSystem.Service.HumanResources.Employees
{
    public class GdPayVService : BaseService<GdPayV>, IGdPayVService
    {
        static SystemContext dbContext = new SystemContext("SqlConnectionHR");
        public GdPayVService() : base(dbContext)
        { }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="count">返回记录总数</param>
        /// <param name="objs">条件数组</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public override IEnumerable<dynamic> GetForPaging(out int count, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            try
            {
                var source = objs == null ? this._entityStore.Table.OrderByDescending(c => c.billcyc) : this._entityStore.Table.OrderByDescending(c => c.billcyc).Where(ExpressionFactory(objs));
                PagedList<GdPayV> pageList = new PagedList<GdPayV>(source, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count);
                count = pageList.TotalCount;
                return pageList.Select(s => new
                {
                    EmployerId = s.员工编号,
                    DeptName = s.部门,
                    Billcyc = s.billcyc,
                    DataType = s.DataType,
                    DeptId = s.deptid,
                    Pid = s.pid,
                    ValueStr = s.value_str,
                    FixedIncomeProject = s.固定收入项目,
                    TypeOf = s.所属类型,
                    Income = s.收入,
                    PayId = s.收入编号,
                    UpdateTime = s.更新时间,
                    AccountingCycle = s.账务周期,
                    EmployerName = s.姓名,

                });
            }
            catch (Exception ex)
            {

                throw;
            }
        }
   
    }
}
