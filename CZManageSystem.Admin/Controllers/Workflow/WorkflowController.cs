using CZManageSystem.Admin.Base;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace CZManageSystem.Admin.Controllers.Workflow
{
    public class WorkflowController : BaseController
    {
        ITracking_Activities_CompletedService _consumableService = new Tracking_Activities_CompletedService();
        // GET: Workflow
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 根据流程实例ID获取已经完成的步骤信息
        /// </summary>
        /// <param name="WorkflowInstanceId">流程实例ID</param>
        /// <returns></returns>
        public ActionResult GetActivitiesCompletedByID(Guid? WorkflowInstanceId)
        {
            WorkflowInstanceId = WorkflowInstanceId ?? Guid.Parse("00000000-0000-0000-0000-000000000000");
            int count = 0;
            var queryCondition = new
            {
                WorkflowInstanceId = WorkflowInstanceId
            };
            var listData = _consumableService.GetForPaging(out count, queryCondition).Select(u => (Tracking_Activities_Completed)u).ToList();
            listData = listData.OrderBy(u => u.FinishedTime).ToList();
            List<object> listResult = new List<object>();
            foreach (var item in listData)
            {
                listResult.Add(new
                {
                    ActivityName = item.Activities.ActivityName,
                    ActorDescription = item.ActorDescription,
                    FinishedTime = item.FinishedTime,
                    Reason = item.Reason,
                    Command = item.Command,
                    CommandText= GetCommandText(item.Command)
                });
            }

            return Json(new { items = listResult, count = count });

        }

        private string GetCommandText(string value) {
            string result = "";
            switch (value)
            {
                case "approve": result = "通过"; break;
                case "reject": result = "退回"; break;
                case "close_activities": result = "作废"; break;
                default:break;
            }
            return result;
        }

        /// <summary>
        /// 获取流程提交后的下一个步骤和执行人信息
        /// </summary>
        /// <returns></returns>
        public ActionResult getWFStepAfterSumbit(string workflowName)
        {
            List<object> listResult = new List<object>();
            string resultXml = CommonFunction.SearchWorkflow_infolist(workflowName,this.WorkContext.CurrentUser.UserName);
            Dictionary<string, string> dictOperator = CommonFunction.GetActivitiesInfo(resultXml);
            List<ActivitiesProfile> listCCclass = CommonFunction.GetActivitiesProfileInfo(resultXml);
            foreach (var dict in dictOperator)
            {
                #region 操作人信息
                List<object> listUser = new List<object>();
                if (!string.IsNullOrEmpty(dict.Value))
                {
                    foreach (var a in dict.Value.Split(','))
                    {
                        if (!string.IsNullOrEmpty(a))
                        {
                            listUser.Add(new
                            {
                                UserName = a.Split('/')[1],
                                RealName = a.Split('/')[0]
                            });
                        }
                    }
                }
                #endregion

                #region 抄送人信息
                List<object> listCC = new List<object>();
                string strReviewType = "None";//抄送类别:None-不抄送，CheckBox-复选框，Classic-用户自己选择抄送人
                int intReviewActorCount = -1;
                bool boolReviewValidateType = false;
                ActivitiesProfile curProfile = listCCclass.Where(u => u.Name == dict.Key).FirstOrDefault();
                if (curProfile != null)
                {
                    strReviewType = curProfile.ReviewType;
                    intReviewActorCount = curProfile.ReviewActorCount;
                    boolReviewValidateType = curProfile.ReviewValidateType;
                    if (!string.IsNullOrEmpty(curProfile.Actors))
                    {
                        foreach (var a in curProfile.Actors.Split(','))
                        {
                            if (!string.IsNullOrEmpty(a))
                            {
                                listCC.Add(new
                                {
                                    UserName = a.Split('/')[1],
                                    RealName = a.Split('/')[0]
                                });
                            }
                        }
                    }
                }
                #endregion

                listResult.Add(new
                {
                    activityName = dict.Key,
                    users = listUser,
                    CC_ReviewType = strReviewType,//抄送类别:None-不抄送，CheckBox-复选框，Classic-用户自己选择抄送人
                    CC_ReviewActorCount = intReviewActorCount,//抄送人数限制
                    CC_ReviewValidateType= boolReviewValidateType,//抄送选项是否默认全选
                    listCC = listCC
                });
            }

            return Json(new { items = listResult }, JsonRequestBehavior.AllowGet);
        }


    }
}