using CZManageSystem.Admin.Base;
using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.CollaborationCenter.SmsManager;
using CZManageSystem.Service.CollaborationCenter.SmsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

/// <summary>
/// 短信发送管理
/// </summary>
namespace CZManageSystem.Admin.Controllers.CollaborationCenter.SmsManager
{
    public class SendSmsController : BaseController
    {

        ISendSmsService _smsService = new SendSmsService();//短信发送
        IV_SendSmsCountService _smsCountService = new V_SendSmsCountService();//短信统计

        // GET: SendSms
        //短信查询
        public ActionResult Index()
        {
            return View();
        }
        //短信群发
        public ActionResult SmsSend(Guid? ID, string type = "look")
        {
            ViewData["ID"] = ID;
            ViewData["type"] = type;
            return View();
        }
        //短信日统计
        public ActionResult SmsStatistics()
        {
            return View();
        }

        /// <summary>
        /// 查询短信
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数据量</param>
        /// <param name="queryBuilder">查询条件</param>
        /// <returns></returns>
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, SendSmsQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var modelList = _smsService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (SendSms)u).ToList();

            List<object> listResult = new List<object>();
            foreach (var item in modelList)
            {
                listResult.Add(new
                {
                    item.ID,
                    item.Mobile,
                    item.Context,
                    item.Time,
                    item.Date,
                    item.Sender,
                    SenderName = item.SenderObj.RealName,
                    item.Dept,
                    DeptNapt = item.DeptObj.DpFullName,
                    item.Error,
                    item.Count,
                    item.ShowName
                });
            }

            return Json(new { items = listResult, count = count });

        }

        /// <summary>
        /// 短信日统计
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数据量</param>
        /// <param name="queryBuilder">查询条件</param>
        /// <returns></returns>
        public ActionResult GetListData_Statistics(int pageIndex = 1, int pageSize = 5, V_SendSmsCountQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var modelList = _smsCountService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (V_SendSmsCount)u).ToList();

            List<object> listResult = new List<object>();
            foreach (var item in modelList)
            {
                listResult.Add(new
                {
                    item.Date,
                    item.Count,
                    item.Sender,
                    item.SenderName,
                    item.Dept,
                    item.DeptFullName
                });
            }

            return Json(new { items = listResult, count = count });

        }


        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetDataByID(Guid id)
        {
            var item = _smsService.FindById(id);
            object Apply = new
            {
                item.ID,
                item.Mobile,
                item.Context,
                item.Time,
                item.Date,
                item.Sender,
                SenderName = item.SenderObj.RealName,
                item.Dept,
                DeptNapt = item.DeptObj.DpFullName,
                item.Error,
                item.Count,
                item.ShowName
            };
            return Json(Apply);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="dataObj"></param>
        /// <returns></returns>
        public ActionResult Send(SendSms dataObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过
            if (dataObj.Context == null || string.IsNullOrEmpty(dataObj.Context.Trim()))
                tip = "短信内容不能为空";
            else if (dataObj.Mobile == null || string.IsNullOrEmpty(dataObj.Mobile.Trim()))
                tip = "手机号码不能为空";
            else
            {
                List<string> listTemp = new List<string>();
                foreach (string str in dataObj.Mobile.Split(','))
                {
                    if (!string.IsNullOrEmpty(str.Trim()))
                    {
                        if (CommonFunction.isMobilePhone(str.Trim()))
                        {
                            listTemp.Add(str.Trim());
                        }
                        else
                        {
                            tip = string.Format("手机号码中“{0}”不是合法的手机号码", str);
                            break;
                        }
                    }
                }
                if (string.IsNullOrEmpty(tip))
                {
                    dataObj.Mobile = string.Join(",", listTemp);
                    if (dataObj.Mobile == null || string.IsNullOrEmpty(dataObj.Mobile.Trim()))
                        tip = "手机号码不能为空";
                }
            }

            if (string.IsNullOrEmpty(tip))
            {
                isValid = true;
                dataObj.Mobile = dataObj.Mobile.Trim();
                dataObj.Context = dataObj.Context.Trim();
            }

            if (!isValid)
            {
                result.IsSuccess = false;
                result.Message = tip;
                return Json(result);
            }
            #endregion

            dataObj.ID = Guid.NewGuid();
            dataObj.Sender = this.WorkContext.CurrentUser.UserId;
            dataObj.Dept = this.WorkContext.CurrentUser.DpId;
            dataObj.Time = DateTime.Now;
            dataObj.Date = DateTime.Now.Date;

            string strContext = dataObj.Context;
            if (dataObj.ShowName == "是")
                strContext = string.Format("[{0}]{1}",this.WorkContext.CurrentUser.RealName, strContext);

            dataObj.Error = !CommonFunction.SendSms(dataObj.Mobile.Split(',').ToList(), strContext);
            dataObj.Count = dataObj.Mobile.Split(',').Length;

            result.IsSuccess = _smsService.Insert(dataObj);

            if (!result.IsSuccess)
            {
                result.Message = "保存失败";
            }
            else if (dataObj.Error.Value)
            {
                result.IsSuccess = false;
                result.Message = "发送失败";
            }
            return Json(result);
        }


    }
}