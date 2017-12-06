using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.CollaborationCenter.Payment;
using CZManageSystem.Service.CollaborationCenter.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.CollaborationCenter.Payment
{
    public class BasicDataSetController : BaseController
    {
        // GET: BasicDataSet
        #region Field
        IPaymentPayeeService paymentPayeeService = new PaymentPayeeService();
        IPaymentPayerService paymentPayerService = new PaymentPayerService();
        IPaymentHallService paymentHallService = new PaymentHallService();
        IPaymentCompanyService paymentCompanyService = new PaymentCompanyService();
        #endregion
        #region 服营厅设置
        public ActionResult HallIndex()
        {
            return View();
        }

        public ActionResult HallEdit(string id)
        {
            ViewData["id"] = id;
            ViewData["startId"] = WorkContext.CurrentUser.Dept.DpId;
            return View();
        } 
        public ActionResult GetPaymentPayeesDataByID(Guid id)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            var model = paymentPayeeService.FindById(id);
            if (model == null)
            {
                result.Message = "不存在当前收款人账号！";
                return Json(result);
            }
            result.IsSuccess = true;
            result.data = model;
            return Json(result);

        }
        public ActionResult GetDataByID(Guid id)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            var model = paymentHallService.FindById(id);
            if (model == null)
            {
                result.Message = "不存在当前服营厅！";
                return Json(result);
            }
            result.IsSuccess = true;
            result.data = new
            {
                model.HallID,
                model.HallName,
                //model.PaymentPayees,
                model?.Depts.DpFullName,
                model.DpId
            };
            return Json(result);

        }
        public ActionResult HallSave(HallViewModels hall)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            if (hall == null)
            {
                result.Message = "保存对象不能为空！";
                return Json(result);
            }

            if (hall.HallID == null || hall.HallID == new Guid())
            {
                var saveModel = new PaymentHall()
                {
                    HallID = Guid.NewGuid(),
                    HallName = hall.HallName,
                    DpId = hall.DpId
                };
                result.IsSuccess = paymentHallService.Insert(saveModel);
                result.data = saveModel.HallID;
                return Json(result);
            }
            var model = paymentHallService.FindById(hall.HallID);
            if (model == null)
            {
                result.Message = "不存在当前服营厅！";
                return Json(result);
            }
            if (model.HallName == hall.HallName)
            {
                result.Message = "服营厅名称已经存在！";
                return Json(result);
            }
            model.DpId = hall.DpId;
            model.HallName = hall.HallName;
            result.IsSuccess = paymentHallService.Update(model);
            result.data = model.HallID;
            return Json(result);

        }
        public ActionResult GetPaymentPayeesList(Guid hallID, int pageIndex = 1, int pageSize = 10)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };

            if (hallID == null || hallID == new Guid())
            {
                result.Message = "服营厅不存在！";
                return Json(result);
            }
            int count = 0;
            var list = paymentPayeeService.GetForPaging(out count, new { HallID = hallID }, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize);
            result.data = new { list, count };
            return Json(result);

        }
        public ActionResult PaymentPayeesSave(PaymentPayee paymentPayees)
        {
            try
            {
                SystemResult result = new SystemResult() { IsSuccess = false };
                if (paymentPayees == null)
                {
                    result.Message = "保存对象不能为空！";
                    return Json(result);
                }
                if (paymentPayees.HallID == null || paymentPayees.HallID == new Guid())
                {
                    result.Message = "请先保存服营厅！";
                    return Json(result);
                }
                if (paymentPayees.PayeeID == null || paymentPayees.PayeeID == new Guid())
                {
                    paymentPayees.PayeeID = Guid.NewGuid();
                    result.IsSuccess = paymentPayeeService.Insert(paymentPayees);
                    return Json(result);
                }
                var model = paymentPayeeService.FindById(paymentPayees.PayeeID);
                if (model == null)
                {
                    result.Message = "不存在当前服营厅！";
                    return Json(result);
                }
                /// <summary>
                /// 收款人帐号ID
                /// <summary>
                //model.PayeeID = paymentPayees.PayeeID;
                /// <summary>
                /// 收款人帐号
                /// <summary>
                model.Account = paymentPayees.Account;
                /// <summary>
                /// 收款人名称
                /// <summary>
                model.Name = paymentPayees.Name;
                /// <summary>
                /// 收款人所属分行代码
                /// <summary>
                model.BranchCode = paymentPayees.BranchCode;
                /// <summary>
                /// 开户行
                /// <summary>
                model.Branch = paymentPayees.Branch;
                /// <summary>
                /// 开户行名称
                /// <summary>
                model.OpenBank = paymentPayees.OpenBank;
                /// <summary>
                /// 所属银行名称
                /// <summary>
                model.Bank = paymentPayees.Bank;
                /// <summary>
                /// 所属银行代码
                /// <summary>
                model.BankCode = paymentPayees.BankCode;
                /// <summary>
                /// 属地代码
                /// <summary>
                model.AddressCode = paymentPayees.AddressCode;
                /// <summary>
                /// 区域代码
                /// <summary>
                model.AreaCode = paymentPayees.AreaCode;
                /// <summary>
                /// 服营厅ID
                /// <summary>
                model.HallID = paymentPayees.HallID;

                result.IsSuccess = paymentPayeeService.Update(model);
                return Json(result);
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public ActionResult GetHallListData(int pageIndex = 1, int pageSize = 10, string hallName = null)
        {

            int count = 0; //voteSelectedAnserService
            var modelList = paymentHallService.GetForPaging(out count, new { HallName = hallName }, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize);
            return Json(new { items = modelList, count = count });

        }


        public ActionResult PaymentPayeesDelete(Guid[] ids)
        {
            var models = paymentPayeeService.List().Where(f => ids.Contains(f.PayeeID)).ToList();
            SystemResult result = new SystemResult() { IsSuccess = false, data = new { successCount = models.Count } };
            if (models.Count <= 0)
            {
                result.Message = "该数据不存在！";
                return Json(result);
            }
            if (paymentPayeeService.DeleteByList(models))
                result.IsSuccess = true;
            return Json(result);
        }
        public ActionResult HallDelete(Guid[] ids)
        {
            var models = paymentHallService.List().Where(f => ids.Contains(f.HallID)).ToList();
            SystemResult result = new SystemResult() { IsSuccess = false, data = new { successCount = models.Count } };
            if (models.Count <= 0)
            {
                result.Message = "该数据不存在！";
                return Json(result);
            }
            if (paymentHallService.DeleteByList(models))
                result.IsSuccess = true;
            return Json(result);
        }
        #endregion

        #region 代垫公司设置
        public ActionResult CompanyIndex()
        {
            return View();
        }

        public ActionResult CompanyEdit(string id)
        {
            ViewData["id"] = id;
            ViewData["startId"] = WorkContext.CurrentUser.Dept.DpId;
            return View();
        }
        public ActionResult GetCompanyDataByID(Guid id)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            var model = paymentCompanyService.FindById(id);
            if (model == null)
            {
                result.Message = "不存在当前服营厅！";
                return Json(result);
            }
            result.IsSuccess = true;
            result.data = new
            {
                model.DpId ,
                model.DpName  
                //model.PaymentPayees, 
            };
            return Json(result);

        }
        public ActionResult CompanySave(CompanyViewModels viewModel)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            if (viewModel == null)
            {
                result.Message = "保存对象不能为空！";
                return Json(result);
            }

            if (viewModel.DpId  == null || viewModel.DpId == new Guid())
            {
                var saveModel = new PaymentCompany()
                {
                    DpId = Guid.NewGuid(),
                    DpName = viewModel.DpName 
                };
                result.IsSuccess = paymentCompanyService.Insert(saveModel);
                result.data = saveModel.DpId ;
                return Json(result);
            }
            var model = paymentCompanyService.FindById(viewModel.DpId);
            if (model == null)
            {
                result.Message = "不存在当前公司名称！";
                return Json(result);
            }
            if (model.DpName  == viewModel.DpName )
            {
                result.Message = "公司名称已经存在！";
                return Json(result);
            }
            model.DpId = viewModel.DpId;
            model.DpName  = viewModel.DpName;
            result.IsSuccess = paymentCompanyService.Update(model);
            result.data = model.DpId;
            return Json(result);

        }
        public ActionResult GetPayerDataByID(Guid id)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            var model = paymentPayeeService.FindById(id);
            if (model == null)
            {
                result.Message = "不存在当前收款人账号！";
                return Json(result);
            }
            result.IsSuccess = true;
            result.data = model;
            return Json(result);

        }
        public ActionResult GetCompanyListData(int pageIndex = 1, int pageSize = 10, string dpName = null)
        { 
            int count = 0; //voteSelectedAnserService
            var modelList = paymentCompanyService.GetForPaging(out count,null , pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize);
            return Json(new { items = modelList, count = count });

        }
        public ActionResult PayerDelete(Guid[] ids)
        {
            var models = paymentHallService.List().Where(f => ids.Contains(f.HallID)).ToList();
            SystemResult result = new SystemResult() { IsSuccess = false, data = new { successCount = models.Count } };
            if (models.Count <= 0)
            {
                result.Message = "该数据不存在！";
                return Json(result);
            }
            if (paymentHallService.DeleteByList(models))
                result.IsSuccess = true;
            return Json(result);
        }

        public ActionResult CompanyDelete(Guid[] ids)
        {
            var models = paymentHallService.List().Where(f => ids.Contains(f.HallID)).ToList();
            SystemResult result = new SystemResult() { IsSuccess = false, data = new { successCount = models.Count } };
            if (models.Count <= 0)
            {
                result.Message = "该数据不存在！";
                return Json(result);
            }
            if (paymentHallService.DeleteByList(models))
                result.IsSuccess = true;
            return Json(result);
        }

        public ActionResult GetPayerList(Guid companyID, int pageIndex = 1, int pageSize = 10)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };

            if (companyID == null || companyID == new Guid())
            {
                result.Message = "服营厅不存在！";
                return Json(result);
            }
            int count = 0;
            var list = paymentPayerService.GetForPaging(out count, new { CompanyID = companyID }, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize);
            result.data = new { list, count };
            return Json(result);

        }
        public ActionResult PayerSave(PaymentPayee paymentPayees)
        {
            try
            {
                SystemResult result = new SystemResult() { IsSuccess = false };
                if (paymentPayees == null)
                {
                    result.Message = "保存对象不能为空！";
                    return Json(result);
                }
                if (paymentPayees.HallID == null || paymentPayees.HallID == new Guid())
                {
                    result.Message = "请先保存服营厅！";
                    return Json(result);
                }
                if (paymentPayees.PayeeID == null || paymentPayees.PayeeID == new Guid())
                {
                    paymentPayees.PayeeID = Guid.NewGuid();
                    result.IsSuccess = paymentPayeeService.Insert(paymentPayees);
                    return Json(result);
                }
                var model = paymentPayeeService.FindById(paymentPayees.PayeeID);
                if (model == null)
                {
                    result.Message = "不存在当前服营厅！";
                    return Json(result);
                }
                /// <summary>
                /// 收款人帐号ID
                /// <summary>
                //model.PayeeID = paymentPayees.PayeeID;
                /// <summary>
                /// 收款人帐号
                /// <summary>
                model.Account = paymentPayees.Account;
                /// <summary>
                /// 收款人名称
                /// <summary>
                model.Name = paymentPayees.Name;
                /// <summary>
                /// 收款人所属分行代码
                /// <summary>
                model.BranchCode = paymentPayees.BranchCode;
                /// <summary>
                /// 开户行
                /// <summary>
                model.Branch = paymentPayees.Branch;
                /// <summary>
                /// 开户行名称
                /// <summary>
                model.OpenBank = paymentPayees.OpenBank;
                /// <summary>
                /// 所属银行名称
                /// <summary>
                model.Bank = paymentPayees.Bank;
                /// <summary>
                /// 所属银行代码
                /// <summary>
                model.BankCode = paymentPayees.BankCode;
                /// <summary>
                /// 属地代码
                /// <summary>
                model.AddressCode = paymentPayees.AddressCode;
                /// <summary>
                /// 区域代码
                /// <summary>
                model.AreaCode = paymentPayees.AreaCode;
                /// <summary>
                /// 服营厅ID
                /// <summary>
                model.HallID = paymentPayees.HallID;

                result.IsSuccess = paymentPayeeService.Update(model);
                return Json(result);
            }
            catch (Exception ex)
            {

                throw;
            }

        }
      
        #endregion
    }
}