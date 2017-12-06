using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.Composite;
using CZManageSystem.Service.Composite;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using CZManageSystem.Admin.Base;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Data.Domain.Administrative;
using Newtonsoft.Json;
using Aspose.Cells;

namespace CZManageSystem.Admin.Controllers.Composite
{

    public class InnerVoicesController : BaseController
    {

        INnerVoicesService _nnerVoicesService = new NnerVoicesService();
        ITracking_WorkflowService _tracking_WorkflowService = new Tracking_WorkflowService();

        // GET: InnerVoices
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SynopsisIndex()
        {
            return View();
        }

        public ActionResult Myheart()
        {
            return View();
        }

        public ActionResult Edit(int? id)
        {
            ViewData["workflowName"] = FlowInstance.WorkflowType.VoiceApply;
            ViewData["Id"] = id == null ? 0 : id;
            ViewBag.User = this.WorkContext.CurrentUser;

            return View();
        }
        public ActionResult View(int id)
        {
            ViewData["Id"] = id;
            ViewBag.User = this.WorkContext.CurrentUser;

            ViewBag.backUrl = this.Request.UrlReferrer;
            return View();
        }
        public ActionResult View_ForWF(string id)
        {
            ViewData["Id"] = id;
            ViewBag.User = this.WorkContext.CurrentUser;

            return View();
        }
        public ActionResult CheckData(int id)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            if (id > 0)
            {
                InnerVoices Voice = _nnerVoicesService.FindById(id);
                if (Voice.TrackingWorkflow != null)
                {
                    result.Message = "该基层心声已提交，不能进行修改操作！";
                    return Json(result);
                }
            }
            result.IsSuccess = true;
            return Json(result);
        }
        public ActionResult Save(InnerVoices Voice)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            Voice.Creator = this.WorkContext.CurrentUser.UserName;
            Voice.Creatorid = this.WorkContext.CurrentUser.UserId.ToString();
            Voice.Username = this.WorkContext.CurrentUser.UserName;
            Voice.DeptName = this.WorkContext.CurrentUser.Dept.DpName;
            Voice.Phone = this.WorkContext.CurrentUser.Mobile;
            if (Voice.Id == 0)
            {
                result.IsSuccess = _nnerVoicesService.Insert(Voice);
            }
            else
            {
                result.IsSuccess = _nnerVoicesService.Update(Voice);
            }
            result.data = Voice;
            return Json(result);
        }

        #region 流程方法
        /// <summary>
        /// 提交心声申请
        /// </summary>
        /// <param name="Voice"></param>
        /// <returns></returns>
        public ActionResult Submit(InnerVoices Voice, string nextActivity, string nextActors, string nextCC = null)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            try
            {
                if (string.IsNullOrEmpty(nextActivity) || string.IsNullOrEmpty(nextActors))
                {
                    CommonFunction.GetFirstOperatorInfoAfterSumbit(FlowInstance.WorkflowType.BoardroomApply, this.WorkContext.CurrentUser.UserName, out nextActivity, out nextActors);
                    if (!string.IsNullOrEmpty(nextActors))
                        nextActors = nextActors.Split(',')[0];
                }

                if (Voice == null || Voice.Id == 0)
                {
                    result.IsSuccess = false;
                    result.Message = "该数据不存在";
                    return Json(result);
                }
                string objectXML = "";
                string strUser = this.WorkContext.CurrentUser.UserName;
                if (string.IsNullOrEmpty(Voice.WorkflowInstanceId.ToString()))
                {//第一次提交
                    objectXML = "<Root action=\"Start\" username=\"{0}\" keypassword=\"123\">"
                                    + "<parameter>"
                                        + "<item name=\"workflowId\" value=\"{1}\"/>"
                                        + "<item name=\"workflowTitle\" value=\"{2}\"/>"
                                        + "<item name=\"workflowProperties\">"
                                            + "<workflow secrecy=\"0\" urgency=\"0\" importance=\"0\" expectFinishedTime=\"{6}\">"
                                                + "<fields>"
                                                    + "<item name=\"F1\" value=\"{3}\"></item>"
                                                    + "<item name=\"F2\" value=\"{7}\"></item>"
                                                + "</fields>"
                                                + "<nextactivities>"
                                                    + "<item name=\"{4}\" actors=\"{5}\"/>"
                                                + "</nextactivities>"
                                            + "</workflow>"
                                        + "</item>"
                                    + "</parameter>"
                                + "</Root>";
                    objectXML = string.Format(objectXML, strUser, FlowInstance.WorkflowType.VoiceApply, Voice.Applytitle, Voice.Id, nextActivity, nextActors, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd"), Voice.IsInfo);

                }
                else
                {//退回提交
                    ITracking_TodoService tempService = new Tracking_TodoService();
                    Tracking_Todo tempActivity = new Tracking_Todo();
                    tempActivity = new Tracking_TodoService().Entitys.Where(u => u.WorkflowInstanceId == Voice.WorkflowInstanceId).FirstOrDefault();
                    objectXML = "<Root action=\"execute\" username=\"{0}\" keypassword=\"123\">"
                                    + "<parameter>"
                                        + "<item name=\"activityInstanceId\" value=\"{1}\"/>"
                                        + "<item name=\"command\" value=\"Approve\"/>"
                                        + "<item name=\"workflowTitle\" value=\"{2}\"/>"
                                        + "<item name=\"workflowProperties\">"
                                            + "<workflow secrecy=\"0\" urgency=\"0\" importance=\"0\" expectFinishedTime=\"{6}\">"
                                                + "<fields>"
                                                    + "<item name=\"F1\" value=\"{3}\"></item>"
                                                    + "<item name=\"F2\" value=\"{7}\"></item>"
                                                + "</fields>"
                                                + "<nextactivities>"
                                                    + "<item name=\"{4}\" actors=\"{5}\"/>"
                                                + "</nextactivities>"
                                            + "</workflow>"
                                        + "</item>"
                                    + "</parameter>"
                                + "</Root>";
                    objectXML = string.Format(objectXML, strUser, tempActivity.ActivityInstanceId.ToString(), Voice.Applytitle, Voice.Id, nextActivity, nextActors, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd"), Voice.IsInfo);

                }

                string[] args = new string[4];
                args[0] = FlowInstance.Workflow_SystemID;
                args[1] = FlowInstance.Workflow_SystemAcount;
                args[2] = FlowInstance.Workflow_SystemPwd;
                args[3] = objectXML;

                string resultXml = WebServicesHelper.InvokeWebService(FlowInstance.Workflow_SystemUrl, FlowInstance.MethodName.ManageWorkflow, args).ToString();
                System.Xml.XmlDocument xdoc = new System.Xml.XmlDocument();
                xdoc.LoadXml(resultXml);
                System.Xml.XmlNode resutNode = xdoc.SelectSingleNode("Result");
                string success = resutNode.Attributes["Success"].Value;
                string errmsg = resutNode.Attributes["ErrorMsg"].Value;
                string strWorkflowInstanceId = "";
                string strActivityinstanceId = "";
                int intSuccess = 0;
                int.TryParse(success, out intSuccess);
                if (intSuccess > 0)
                {
                    if (string.IsNullOrEmpty(Voice.WorkflowInstanceId.ToString()))
                    {//第一次提交
                        System.Xml.XmlNodeList xmlList = xdoc.SelectNodes("Result/item/start/item");
                        for (int i = 0; i < xmlList.Count; i++)
                        {
                            switch (xmlList[i].Attributes["name"].Value)
                            {
                                case "WorkflowInstanceId": strWorkflowInstanceId = xmlList[i].Attributes["value"].Value; break;
                                case "ActivityinstanceId": strActivityinstanceId = xmlList[i].Attributes["value"].Value; break;
                                default: break;
                            }
                        }

                        var workFlow = _tracking_WorkflowService.FindById(Guid.Parse(strWorkflowInstanceId));

                        Voice.WorkflowInstanceId = Guid.Parse(strWorkflowInstanceId);
                        Voice.Applysn = workFlow.SheetId;///表单号

                        result.IsSuccess = _nnerVoicesService.Update(Voice);
                    }
                    else
                    {//退回提交
                        result.IsSuccess = true;
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = errmsg;
                }


            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            if (result.IsSuccess && !string.IsNullOrEmpty(nextCC))
            {//提交成功后抄送
                InnerVoices curData = _nnerVoicesService.FindById(Voice.Id);
                CommonFunction.PendingData(curData.WorkflowInstanceId.Value, nextCC);//抄送
            }

            return Json(result);
        }
        /// <summary>
        /// 取消基层心声申请工单
        /// </summary>
        /// <param name="curData"></param>
        /// <returns></returns>
        public ActionResult VoiceCancel(BoardroomApply curData)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };

            ITracking_TodoService tempService = new Tracking_TodoService();
            Tracking_Todo tempActivity = new Tracking_Todo();
            Guid guid = new Guid();
            Guid.TryParse(curData.WorkflowInstanceId, out guid);
            tempActivity = new Tracking_TodoService().Entitys.Where(u => u.WorkflowInstanceId == guid).FirstOrDefault();
            string objectXML = "<Root action=\"execute\" username=\"{0}\" keypassword=\"123\">"
                            + "<parameter>"
                                + "<item name=\"activityInstanceId\" value=\"{1}\"/>"
                                + "<item name=\"command\" value=\"cancel\"/>"
                                + "<item name=\"workflowProperties\">"
                                    + "<workflow secrecy=\"0\" urgency=\"0\" importance=\"0\" expectFinishedTime=\"{3}\">"
                                    //+ "<nextactivities>"
                                    //    + "<item name=\"{3}\" actors=\"{4}\"/>"
                                    //+ "</nextactivities>"
                                    + "</workflow>"
                                + "</item>"
                                + "<item name = \"Content\" value = \"{2}\" />"
                               + "</parameter>"
                        + "</Root>";
            objectXML = string.Format(objectXML, this.WorkContext.CurrentUser.UserName, tempActivity.ActivityInstanceId.ToString(), curData.StateRemark, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd"));// , nextActivity, nextActors,     curData.ApplyTitle, curData.ID

            string[] args = new string[4];
            args[0] = FlowInstance.Workflow_SystemID;
            args[1] = FlowInstance.Workflow_SystemAcount;
            args[2] = FlowInstance.Workflow_SystemPwd;
            args[3] = objectXML;

            string resultXml = WebServicesHelper.InvokeWebService(FlowInstance.Workflow_SystemUrl, FlowInstance.MethodName.ManageWorkflow, args).ToString();

            System.Xml.XmlDocument xdoc = new System.Xml.XmlDocument();
            xdoc.LoadXml(resultXml);
            System.Xml.XmlNode resutNode = xdoc.SelectSingleNode("Result");
            string success = resutNode.Attributes["Success"].Value;
            string errmsg = resutNode.Attributes["ErrorMsg"].Value;
            int intSuccess = 0;
            int.TryParse(success, out intSuccess);
            if (intSuccess > 0)
            {
                result.IsSuccess = true;
            }
            else
            {
                result.IsSuccess = false;
                result.Message = errmsg;
            }
            return Json(result);
        }
        #endregion
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, InnerVoicesQueryBuilder queryBuilder = null)
        {
            if (queryBuilder.CreateTime_Start != null)
                queryBuilder.CreateTime_Start = queryBuilder.CreateTime_Start.Value.Date;
            if (queryBuilder.CreateTime_End != null)
                queryBuilder.CreateTime_End = queryBuilder.CreateTime_End.Value.AddDays(1).Date.AddSeconds(-1);

            queryBuilder.IsNiming = "是";
            queryBuilder.isSumbit = true;
            int count = 0;

            var modelList = _nnerVoicesService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => new
            {
                u.Id,
                DeptName= u.IsInfo=="是"? u.DeptName:null,
                u.WorkflowInstanceId,
                u.TrackingWorkflow,
                u.Applytitle,
                u.Applysn,
                Creator = u.IsInfo == "是" ? u.Creator : null,
                Creatorid = u.IsInfo == "是" ? u.Creatorid : null,
                u.Themetype,
                u.Content,
                u.IsNiming,
                u.Attids,
                u.Remark,
                u.IsInfo,
                Username = u.IsInfo == "是" ? u.Username : null,
                Phone = u.IsInfo == "是" ? u.Phone : null,
                CreateTime = u.CreateTime.ToString()
            });
            

            return Json(new { items = modelList, count = count });
        }

        public ActionResult Delete(int[] ids)
        {
            bool isSuccess = false;
            int successCount = 0;
            List<InnerVoices> list = new List<InnerVoices>();
            foreach (int id in ids)
            {
                var obj = _nnerVoicesService.FindById(id);
                if (obj.TrackingWorkflow != null)
                {
                    SystemResult result = new SystemResult() { IsSuccess = false, Message = "该基层心声已提交，不能进行修改操作！" };
                    return Json(result);
                }
                list.Add(obj);
            }
            if (_nnerVoicesService.DeleteByList(list))
            {
                isSuccess = true;
                successCount = list.Count;
            }

            return Json(new
            {
                IsSuccess = isSuccess,
                successCount = successCount,
            });
        }

        public ActionResult GetDataByID(int id)
        {
            object model;
            // var item = _nnerVoicesService.FindById(id);
            if (id != 0)
            {
                var u = _nnerVoicesService.FindById(id);
                model = new
                {
                    u.Id,
                    u.WorkflowInstanceId,
                    u.Applytitle,
                    u.Applysn,
                    u.Creator,
                    u.Creatorid,
                    u.Themetype,
                    u.Content,
                    u.IsNiming,
                    u.Attids,
                    u.Remark,
                    CreateTime = u.CreateTime.HasValue ? u.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                    u.IsInfo,
                    u.Username,
                    u.DeptName,
                    u.Phone,
                    u.IsManager
                };
            }
            else
                model = new
                {

                    Applytitle = string.Format(WorkContext.CurrentUser.RealName + "的心声申请({0})", DateTime.Now.ToString("yyyy-MM-dd")),
                    Phone = WorkContext.CurrentUser.Mobile,
                    CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    Username = WorkContext.CurrentUser.RealName
                };
            return Json(new { Items = model });
            //return Json(new
            //{
            //    Id = item.Id,
            //    Applyid = item.WorkflowInstanceId,
            //    Applytitle = item.Applytitle,
            //    Applysn = item.Applysn,
            //    Creator = item.Creator,
            //    Creatorid = item.Creatorid,
            //    Themetype = item.Themetype,
            //    Content = item.Content,
            //    IsNiming = item.IsNiming,
            //    Attids = item.Attids,
            //    Remark = item.Remark,
            //    IsInfo = item.IsInfo,
            //    CreateTime = item.CreateTime.HasValue ? item.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
            //    Username = item.Username,
            //    DeptName = item.DeptName,
            //    Phone = item.Phone
            //});
        }
        public ActionResult GetDataByID_View(int id)
        {
            object model;
            if (id != 0)
            {
                var u = _nnerVoicesService.FindById(id);
                model = new
                {
                    u.Id,
                    u.WorkflowInstanceId,
                    u.Applytitle,
                    u.Applysn,
                    Creator = u.IsInfo == "是" ? u.Creator : null,
                    Creatorid = u.IsInfo == "是" ? u.Creatorid : null,
                    u.Themetype,
                    u.Content,
                    u.IsNiming,
                    u.Attids,
                    u.Remark,
                    CreateTime = u.CreateTime.HasValue ? u.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                    u.IsInfo,
                    Username = u.IsInfo == "是" ? u.Username : null,
                    DeptName = u.IsInfo == "是" ? u.DeptName : null,
                    Phone = u.IsInfo == "是" ? u.Phone : null,
                    u.IsManager
                };
            }
            else
                model = new
                {

                    Applytitle = string.Format(WorkContext.CurrentUser.RealName + "的心声申请({0})", DateTime.Now.ToString("yyyy-MM-dd")),
                    Phone = WorkContext.CurrentUser.Mobile,
                    CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    Username = WorkContext.CurrentUser.RealName
                };
            return Json(new { Items = model });
        }

        public ActionResult GetVoiceListByUserName(int pageIndex = 1, int pageSize = 5, InnerVoicesQueryBuilder queryBuilder = null)
        {

            if (queryBuilder.CreateTime_Start != null)
                queryBuilder.CreateTime_Start = queryBuilder.CreateTime_Start.Value.Date;
            if (queryBuilder.CreateTime_End != null)
                queryBuilder.CreateTime_End = queryBuilder.CreateTime_End.Value.AddDays(1).Date.AddSeconds(-1);
            int count = 0;
            queryBuilder.Creator = this.WorkContext.CurrentUser.UserName;
            var model = _nnerVoicesService.GetForPaging(out count, queryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize).Select(u => new
            {
                u.Id,
                u.DeptName,
                u.WorkflowInstanceId,
                u.TrackingWorkflow,
                u.Applytitle,
                u.Applysn,
                u.Creator,
                u.Creatorid,
                u.Themetype,
                u.Content,
                u.IsNiming,
                u.Attids,
                u.Remark,
                u.IsInfo,
                u.Username,
                u.Phone,
                CreateTime = u.CreateTime.ToString()
            });

            return Json(new { items = model, count = count });
        }

        public ActionResult Download(string queryBuilder = null)
        {
            string path = "";
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<InnerVoicesQueryBuilder>(queryBuilder);
                int pageIndex = 0;
                int pageSize = int.MaxValue;

                int count = 0;
                var modelList = _nnerVoicesService.GetForPaging(out count, QueryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize).Select(u => new
                {
                    u.Id,
                    DeptName = u.IsInfo == "是" ? u.DeptName : null,
                    u.WorkflowInstanceId,
                    u.TrackingWorkflow,
                    u.Applytitle,
                    u.Applysn,
                    Creator = u.IsInfo == "是" ? u.Creator : null,
                    Creatorid = u.IsInfo == "是" ? u.Creatorid : null,
                    u.Themetype,
                    u.Content,
                    u.IsNiming,
                    u.Attids,
                    u.Remark,
                    u.IsInfo,
                    Username = u.IsInfo == "是" ? u.Username : null,
                    Phone = u.IsInfo == "是" ? u.Phone : null,
                    CreateTime = u.CreateTime.ToString()
                }).ToList();
                if (modelList.Count <1)
                    return View("../Export/Message");
                #region Excel部分
                //生成文件名
                var fileToSaveName = SaveName.InnerVoices + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.InnerVoices);
                //设置集合变量
                designer.SetDataSource(ImportFileType.InnerVoices, modelList);
                //根据数据源处理生成报表内容
                designer.Process();
                //designer.Save(path, FileFormatType.Excel2003);
                var response = GetResponse(fileToSaveName);
                designer.Save(Url.Content(fileToSaveName), SaveType.OpenInExcel, FileFormatType.Excel2003, response);
                response.Flush();
                response.Close();
                designer = null;
                response.End();

                #endregion

            }
            catch (Exception ex)
            {

                throw;
            }
            return null;
        }
    }
}