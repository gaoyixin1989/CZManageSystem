using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.Administrative.VehicleManages;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.Administrative;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.Administrative
{
    public class CarEvaluationController : BaseController
    {
        ICarApplyService _carApplyService = new CarApplyService();
        // GET: CarEvaluation
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(Guid? Id)
        {
            ViewData["Id"] = Id;
            return View();
        }
      
        public ActionResult CarEvaluationGetListData(int pageIndex = 1, int pageSize = 5 , CarEvaluationQueryBuilder queryBuilder=null)
        {
            int count = 0;
            var modelList = _carApplyService.GetCarEvaluation(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);
            var items = modelList;
            return Json(new { items = items, count = count });
        }
        public ActionResult CarEvaluationDataByID(Guid id)
        {
            var u = _carApplyService.FindById(id);
            return Json(new
            {
                u.Allocator,
                u.ApplyTitle,
                u.AllotIntro,
                u.AllotTime,
                u.ApplyCant,
                u.ApplyId,
                u.Attach,
                u.BalCount,
                u.BalRemark,
                u.BalTime,
                u.BalTotal,
                u.BalUser,
                u.Boral,
                u.CarIds,
                u.CorpId,
                CreateTime = u.BalTime.HasValue ? u.BalTime.Value.ToString("yyyy-MM-dd") : "",
                u.DeptName,
                u.Destination1,
                u.Destination2,
                u.Destination3,
                u.Destination4,
                u.Destination5,
                u.Driver,
                u.EndTime,
                u.Field00,
                u.Field01,
                u.Field02,
                u.FinishTime,
                u.KmCount,
                u.KmNum1,
                u.KmNum2,
                u.Leader,
                u.Mobile,
                u.OpinGrade1,
                u.OpinGrade2,
                u.OpinGrade3,
                u.OpinGrade4,
                u.OpinGrade5,
                u.OpinGrade6,
                u.OpinGrade7,
                u.OpinRemark,
                u.OpinTime,
                u.OpinUser,
                u.PersonCount,
                u.Remark,
                u.Request,
                u.Road,
                u.SpecialReason,
                u.Starting,
                u.StartTime,
                u.TimeOut,
                u.UptTime,
                u.UptUser,
                u.UseType,
                u.WorkflowInstanceId

            });
        }

        public ActionResult CarEvaluationInfo(CarApply ca)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            if (ca.ApplyId != Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                ca.OpinUser= this.WorkContext.CurrentUser.UserName;
                ca.OpinTime = DateTime.Now;
                result.IsSuccess = _carApplyService.Update(ca);
            }
            if (!result.IsSuccess)
            {
                result.Message = "保存失败";
            }
            return Json(result);
        }


    }
}