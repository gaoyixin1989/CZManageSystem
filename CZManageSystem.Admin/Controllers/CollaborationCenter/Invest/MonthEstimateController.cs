using CZManageSystem.Admin.Base;
using CZManageSystem.Service.CollaborationCenter.Invest;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.CollaborationCenter.Invest
{
    public class MonthEstimateController : BaseController
    {
        ITracking_WorkflowService _tracking_WorkflowService = new Tracking_WorkflowService();
        // GET: MonthEstimate

        public ActionResult ApplyView(Guid? ID)
        {
            ViewData["ID"] = ID;
            return View();
        }
        IInvestMonthEstimateApplyService _investMonthEstimateApplyService = new CZManageSystem.Service.CollaborationCenter.Invest.InvestMonthEstimateApplyService();
        IInvestMonthEstimateApplySubListService _investMonthEstimateApplySubListService = new InvestMonthEstimateApplySubListService();
        public ActionResult GetListByApplyID(Guid ID)
        {
            var model = _investMonthEstimateApplySubListService.List().Where(u => u.ApplyID == ID).ToList();
            var modelList = model.ToList().Select(u => new
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
                u.Course,
                u.BackRate,
                u.Rate,
                u.Pay,
                u.NotPay,
            });
           

            return Json(new { items = modelList });
        }

        public ActionResult GetByID(Guid ID)
        {
            var modelList = _investMonthEstimateApplyService.FindById(ID);
            List<object> listResult = new List<object>();
            listResult.Add(new
            {
                modelList.ApplyID,
                modelList.WorkflowInstanceId,
                modelList.Series,
                modelList.ApplyTime,
                DpName = CommonFunction.getDeptNamesByIDs(modelList.ApplyDpCode),
                ApplyName = CommonFunction.getUserRealNamesByIDs(modelList.Applicant),
                modelList.Mobile,
                modelList.Title,
                modelList.Status
            });

            return Json(new { items = listResult });
        }

    }
}