using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.CollaborationCenter.Invest;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.CollaborationCenter.Invest
{
    public class InvestEstimateService : BaseService<InvestEstimate>, IInvestEstimateService
    {
        //IInvestContractService _contractService = new InvestContractService();//合同信息

        public IEnumerable < dynamic> GetForPaging_(out int count, ref dynamic total, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            try
            {
                var source = objs == null ? this._entityStore.Table.OrderByDescending(c => c.ProjectID) : this._entityStore.Table.OrderByDescending(c => c.ProjectID).Where(ExpressionFactory(objs));
                PagedList<InvestEstimate> pageList = new PagedList<InvestEstimate>(source, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count);
                count = pageList.TotalCount;

                decimal? TaxCount = 0;
                var modelList = pageList.ToList().Select(u => new
                {
                    u.Month,
                    u.Year,
                    u.ID,
                    u.ProjectID,
                    u.ContractID,
                    u.ProjectName,
                    u.ContractName,
                    u.Supply,
                    u.SignTotal,
                    u.PayTotal,
                    u.Study,
                    ManagerName = u.ManagerID == null ? "" : u.ManagerObj?.RealName,
                    Dpfullname = u.EstimateUserID == null ? "" : u.UserObj?.Dept?.DpName,
                    u.Course,
                    u.BackRate,
                    u.Rate,
                    u.Pay,
                    u.NotPay,
                    Tax = GetTax(u.ProjectID, u.ContractID, ref TaxCount)
                });
                total = new
                {
                    SignTotalCount = pageList.Sum(u => u.SignTotal ?? 0),//合同金额
                    PayTotalCount = pageList.Sum(u => u.PayTotal ?? 0),//实际合同金额
                    TaxCount = TaxCount,//合同税金额
                    NotPayCount = pageList.Sum(u => u.NotPay ?? 0),//暂估金额
                    PayCount = pageList.Sum(u => u.Pay ?? 0)//已付款金额

                };
                return  modelList ;

            }
            catch (Exception ex)
            {

                throw;
            }

            #region MyRegion
            //var query = this.List().ToList().Select(u => new
            //{
            //    u.Month,
            //    u.Year,
            //    u.ID,
            //    u.ProjectID,
            //    u.ContractID,
            //    u.ProjectName,
            //    u.ContractName,
            //    u.Supply,
            //    u.SignTotal,
            //    u.PayTotal,
            //    u.Study,
            //    ManagerName = u.ManagerID == null ? "" : u.ManagerObj?.RealName,
            //    Dpfullname = u.EstimateUserID == null ? "" : u.UserObj?.Dept?.DpName,
            //    u.Course,
            //    u.BackRate,
            //    u.Rate,
            //    u.Pay,
            //    u.NotPay,
            //    Tax = GetTax(u.ProjectID, u.ContractID)
            //});
            //count = query.Count();
            //#region 查询条件
            //if (objs.ProjectID != null)
            //    query = query.Where(a => a.ProjectID.Contains(objs.ProjectID));
            //if (objs.ProjectName != null)
            //    query = query.Where(a => a.ProjectName.Contains(objs.ProjectName));
            //if (objs.ContractID != null)
            //    query = query.Where(a => a.ContractID.Contains(objs.ContractID));
            //if (objs.ContractName != null)
            //    query = query.Where(a => a.ContractName.Contains(objs.ContractName));
            //if (objs.Course != null)
            //    query = query.Where(a => a.Course.Contains(objs.Course));
            //if (objs.Study != null)
            //    query = query.Where(a => a.Study.Contains(objs.Study));
            //if (objs.Supply != null)
            //    query = query.Where(a => a.Supply.Contains(objs.Supply));
            //if (objs.Year != null)
            //    query = query.Where(a => a.Year.ToString() == objs.Year);
            //if (objs.Month != null)
            //    query = query.Where(a => a.Month.ToString() == objs.Month);
            //if (objs.Dpfullname != null)
            //    query = query.Where(a => a.Dpfullname.Contains(objs.Dpfullname));
            //if (objs.ManagerName != null)
            //    query = query.Where(a => a.ManagerName.Contains(objs.ManagerName));
            //if (objs.BackRate_start != null)
            //{
            //    var BackRate_start = Convert.ToDecimal(objs.BackRate_start);
            //    query = query.Where(a => a.BackRate >= BackRate_start);
            //}
            //if (objs.BackRate_end != null)
            //{
            //    var BackRate_end = Convert.ToDecimal(objs.BackRate_end);
            //    query = query.Where(a => a.BackRate <= BackRate_end);
            //}
            //if (objs.NotPay_start != null)
            //{
            //    var NotPay_start = Convert.ToDecimal(objs.NotPay_start);
            //    query = query.Where(a => a.NotPay >= NotPay_start);
            //}
            //if (objs.NotPay_end != null)
            //{
            //    var NotPay_end = Convert.ToDecimal(objs.NotPay_end);
            //    query = query.Where(a => a.NotPay <= NotPay_end);
            //}
            //if (objs.Pay_start != null)
            //{
            //    var Pay_start = Convert.ToDecimal(objs.Pay_start);
            //    query = query.Where(a => a.Pay >= Pay_start);
            //}
            //if (objs.Pay_end != null)
            //{
            //    var Pay_end = Convert.ToDecimal(objs.Pay_end);
            //    query = query.Where(a => a.Pay <= Pay_end);
            //}
            //if (objs.PayTotal_start != null)
            //{
            //    var PayTotal_start = Convert.ToDecimal(objs.PayTotal_start);
            //    query = query.Where(a => a.PayTotal >= PayTotal_start);
            //}
            //if (objs.PayTotal_end != null)
            //{
            //    var PayTotal_end = Convert.ToDecimal(objs.PayTotal_end);
            //    query = query.Where(a => a.PayTotal <= PayTotal_end);
            //}
            //if (objs.Rate_start != null)
            //{
            //    var Rate_start = Convert.ToDecimal(objs.Rate_start);
            //    query = query.Where(a => a.Rate >= Rate_start);
            //}
            //if (objs.Rate_end != null)
            //{
            //    var Rate_end = Convert.ToDecimal(objs.Rate_end);
            //    query = query.Where(a => a.Rate <= Rate_end);
            //}
            //if (objs.SignTotal_start != null)
            //{
            //    var SignTotal_start = Convert.ToDecimal(objs.SignTotal_start);
            //    query = query.Where(a => a.SignTotal >= SignTotal_start);
            //}
            //if (objs.SignTotal_end != null)
            //{
            //    var SignTotal_end = Convert.ToDecimal(objs.SignTotal_end);
            //    query = query.Where(a => a.SignTotal <= SignTotal_end);
            //}
            //if (objs.Tax_start != null)
            //{
            //    var Tax_start = Convert.ToDecimal(objs.Tax_start);
            //    query = query.Where(a => a.Tax >= Tax_start);
            //}
            //if (objs.PayTotal_end != null)
            //{
            //    var PayTotal_end = Convert.ToDecimal(objs.PayTotal_end);
            //    query = query.Where(a => a.PayTotal <= PayTotal_end);
            //}
            //#endregion

            //return query.OrderByDescending(a => a.ID).Skip(pageIndex * pageSize).Take(pageSize).ToList(); 
            #endregion

        }
        private  decimal? GetTax(string ProjectID, string ContractID, ref decimal? TaxCount)
        {
            
            var model = new EfRepository<InvestContract>().FindByFeldName(t => t.ProjectID == ProjectID && t.ContractID == ContractID);
            if (model == null)
                return 0;
            TaxCount += model.Tax;
            return model.Tax;
        }
    }
}
