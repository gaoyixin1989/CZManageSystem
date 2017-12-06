using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.HumanResources.Employees;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace CZManageSystem.Service.HumanResources.Employees
{
    public class VWHRSumService : BaseService<VWHRSum>, IVWHRSumService
    {
        static SystemContext dbContext = new SystemContext("SqlConnectionHR");
        IUum_UserinfoService uum_UserinfoService = new Uum_UserinfoService();
        ISysUserService sysUserService = new SysUserService();
        ISysDeptmentService sysDeptmentService = new SysDeptmentService();
        public VWHRSumService() : base(dbContext)
        { }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="count">返回记录总数</param>
        /// <param name="objs">条件数组</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        //public override IEnumerable<dynamic> GetForPaging(out int count, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        //{

        //    var source = objs == null ? this._entityStore.Table.OrderBy(c => c.employerid) : this._entityStore.Table.OrderBy(c => c.employerid).Where(ExpressionFactory(objs));
        //    PagedList<VWHRSum> pageList = new PagedList<VWHRSum>(source, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count, true);
        //    count = pageList.TotalCount;
        //    return pageList.Select(item => new
        //    {
        //        item.name,
        //        item.employerid,
        //        DpName = GetDeptNmae(item.employerid),
        //        item.billcyc,
        //        revenue = item.总收入,
        //        salary = item.实发,
        //        deducted = item.应扣除,
        //        item.sjhm,
        //        item.acctno,
        //        item.updatetime
        //    }).ToList();
        //}
        public IEnumerable<dynamic> GetListForPaging(out int count, string employeeid, string employeename, string dpName, string billcyc_start, string billcyc_end, int pageIndex = 0, int pageSize = int.MaxValue)
        {

            try
            {
                var source = this._entityStore.Table.OrderBy(c => c.employerid).AsQueryable();

                if (!string.IsNullOrEmpty(employeeid))
                {
                    source = source.Where(c => c.employerid.Contains(employeeid) == true).AsQueryable();

                }
                if (!string.IsNullOrEmpty(employeename))
                {
                    source = source.Where(c => c.name.Contains(employeename) == true).AsQueryable();
                }
                if (!string.IsNullOrEmpty(billcyc_start))
                {
                    billcyc_start = billcyc_start.Replace("半年", "06.5").Replace("年终", "13");
                    Double billcyc = Convert.ToDouble(billcyc_start);
                    source = source.Where(c => c.billcyc >= billcyc).AsQueryable();
                }
                if (!string.IsNullOrEmpty(billcyc_end))
                {
                    billcyc_end = billcyc_end.Replace("半年", "06.5").Replace("年终", "13");
                    Double billcyc = Convert.ToDouble(billcyc_end);
                    source = source.Where(c => c.billcyc <= billcyc).AsQueryable();
                }


                PagedList<VWHRSum> pageList = new PagedList<VWHRSum>(source.OrderByDescending(o => o.billcyc), pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count, true);
                count = pageList.TotalCount;

                return pageList.ToList().Select(item => new
                {
                    //         

                    item.name,
                    item.employerid,
                    DpName = GetDeptNmae(item.employerid),
                    billcyc = GetBillcyc(item.billcyc),
                    revenue = item.总收入,
                    salary = item.实发,
                    deducted = item.应扣除,
                    item.sjhm,
                    item.acctno,
                    item.updatetime
                });
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        string GetDeptNmae(string employerid)
        {

            var model = uum_UserinfoService.FindByFeldName(f => f.employee == employerid);
            if (model == null)
                return "";
            var user = sysUserService.FindByFeldName(f => f.UserName == model.userID);
            //if (user == null)
            //    return "";
            //var Dept = sysDeptmentService.FindByFeldName(f => f.DpId == user.DpId);
            return user?.Dept?.DpName;
        }
        public string GetBillcyc(double? m)
        {
            if (m == null)
                return null;
            string mStr = m.ToString();
            if (mStr.IndexOf(".") > 1)
                return mStr.Substring(0, 4) + "半年";
            else if (mStr.LastIndexOf("13") > 2)
            {
                return mStr.Substring(0, 4) + "年终";
            }
            return mStr;
        }
    }
}
