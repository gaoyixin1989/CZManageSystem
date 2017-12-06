using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.CollaborationCenter.Invest;
using CZManageSystem.Service.SysManger;
using ServiceLibrary.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary
{
    public class InvestMonthEstimateApplyService : ServiceJob
    {
        private const int tryMaxTime = 3;//数据推送最高尝试次数
        private string dataSource = DataIdToInt.DataSourceType.ToDo;
        IInvestTempEstimateService _investTempEstimateService = new InvestTempEstimateService();
        IInvestMonthEstimateApplyService _investMonthEstimateApplyService = new CZManageSystem.Service.CollaborationCenter.Invest.InvestMonthEstimateApplyService();
        IInvestMonthEstimateApplySubListService _investMonthEstimateApplySubListService = new InvestMonthEstimateApplySubListService();
        ITracking_WorkflowService _tracking_WorkflowService = new Tracking_WorkflowService();
        IInvestProjectService _projectService = new InvestProjectService();//投资项目信息
        IInvestContractService _contractService = new InvestContractService();//合同信息
        public override bool Execute()
        {
            #region 查询当前服务策略信息
            string sTemp = "";
            if (!SetStrategyInfo(out sTemp))
            {
                sMessage = sTemp;
                return false;
            }
            #endregion

            List<Tracking_Todo> list = new List<Tracking_Todo>();//待推送的待办信息
            #region 获取需要推送的信息 
            try
            {
                GetMonthApply();
            }
            catch (Exception ex)
            {
                //sMessage = "查询待推送待办信息失败";
                LogRecord.WriteLog(string.Format("每月暂估填报流程申请推送失败,其他提示：{0}", strCurStrategyInfo), LogResult.fail);
                AddStrategyLog(string.Format("每月暂估填报流程申请推送失败,其他提示：{0}", strCurStrategyInfo), false);
                return false;
            }
            #endregion

            SystemResult result = new SystemResult();
            // result = SendingData(list);

            SaveStrategyLog();
            //修改待阅为已阅，具体操作未知
            if (!result.IsSuccess)
            {
                sMessage = "每月暂估填报流程申请推送失败：" + result.Message;
                return false;
            }
            else
            {
                sMessage = "每月暂估填报流程申请推送成功";
                return true;
            }

        }
        public void GetMonthApply()
        {

            List<InvestMonthEstimateApply> Monthlist = new List<InvestMonthEstimateApply>();
            InvestMonthEstimateApply monthdate = new InvestMonthEstimateApply();
            //一个暂估负责人（原先合同主办人，最先是项目经理），一条待办工单
            var Euserlist = _investTempEstimateService.List().Where(s => s.Status == "正常").GroupBy(s => s.EstimateUserID).ToList();
            List<InvestMonthEstimateApplySubList> Detaillist = new List<InvestMonthEstimateApplySubList>();
            InvestMonthEstimateApplySubList detail = new InvestMonthEstimateApplySubList();
            int sYear = DateTime.Today.Year;
            int sMonth = DateTime.Today.Month;

            var list = _investTempEstimateService.List().Where(s => s.Status == "正常").ToList();
            foreach (var el in Euserlist)
            {
                var applyid = Guid.NewGuid();
                list = list.Where(s => s.EstimateUserID == el.Key).ToList();//根据暂估负责人查找明细
                if (list.Count > 0)
                {

                    foreach (var item in list)
                    {
                        if (!CheckRepeat(applyid, item.ProjectID, item.ContractID))
                        {
                            strCurStrategyInfo = "改项目编号和合同编号的组合已经被占用";
                            LogRecord.WriteLog(string.Format("每月暂估填报流程申请推送失败,其他提示：{0}", strCurStrategyInfo), LogResult.fail);
                            return ;
                        }
                        #region
                        detail = new InvestMonthEstimateApplySubList();
                        detail.ID = Guid.NewGuid();
                        detail.ApplyID = applyid;
                        detail.Month = sMonth;
                        detail.Year = sYear;
                        detail.BackRate = item.BackRate;
                        detail.ContractID = item.ContractID;
                        detail.ProjectName = GetProjectName(item.ProjectID);
                        detail.ContractName = GetContractName(item.ProjectID, item.ContractID);
                        detail.Course = item.Course;
                        // detail.IsUpdate = 
                        detail.ManagerID = item.ManagerID;
                        detail.Month = sMonth;
                        detail.NotPay = item.NotPay;
                        detail.Pay = item.Pay;
                        detail.PayTotal = item.PayTotal;
                        detail.ProjectID = item.ProjectID;
                        detail.Rate = item.Rate;
                        detail.SignTotal = item.SignTotal;
                        detail.Study = item.Study;
                        detail.Supply = item.Supply;
                        #endregion
                        Detaillist.Add(detail);                      
                        monthdate.Applicant = item.EstimateUserID.ToString();
                        monthdate.ApplyDpCode = item.ManagerObj.DpId;
                        monthdate.Mobile = item.ManagerObj.Mobile;
                       
                    }

                }
                monthdate.ApplyID = applyid;
                monthdate.ApplyTime = DateTime.Now;
                monthdate.Title = sYear + sMonth.ToString("00") + "月份暂估填报";//待办标题  
                Monthlist.Add(monthdate);

            }
            var sussess = _investMonthEstimateApplySubListService.InsertByList(Detaillist);
            //生成明细表，再生成代办信息
            if (sussess)
            {
                int intSuccrss; int intError;
                SumitData(Monthlist, out intSuccrss, out intError);

            }

        }
        /// <summary>
        /// 检查是否重复
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="ProjectID"></param>
        /// <param name="ContractID"></param>
        /// <returns></returns>
        public bool CheckRepeat(Guid ID, string ProjectID, string ContractID)
        {
            var dd = _investMonthEstimateApplySubListService.List().Where(u => u.ID != ID && u.ProjectID == ProjectID && u.ContractID == ContractID).ToList();
            if (dd.Count > 0)
                return false;
            else
                return true;
        }
        /// <summary>
        /// 每月暂估填报流程
        /// </summary>
        /// <param name="ID">ID</param>
        /// <returns></returns>
        public void SumitData(List<InvestMonthEstimateApply> listData, out int intSuccrss, out int intError)
        {
            intSuccrss = 0;
            intError = 0;
            foreach (var curData in listData)
            {
                bool isSuccess = true;
                InvestMonthEstimateApply CurApplyData = new InvestMonthEstimateApply();
                if (!curData.WorkflowInstanceId.HasValue)
                {//第一次提交
                    string objectXML = "<Root action=\"Start\" username=\"{0}\" keypassword=\"123\">"
                               + "<parameter>"
                                   + "<item name=\"workflowId\" value=\"{1}\"/>"
                                   + "<item name=\"workflowTitle\" value=\"{2}\"/>"
                                   + "<item name=\"workflowProperties\">"
                                       + "<workflow secrecy=\"0\" urgency=\"0\" importance=\"0\" expectFinishedTime=\"{6}\">"
                                           + "<fields>"
                                               + "<item name=\"F1\" value=\"{3}\"></item>"
                                           + "</fields>"
                                           + "<nextactivities>"
                                               + "<item name=\"{4}\" actors=\"{5}\"/>"
                                           + "</nextactivities>"
                                       + "</workflow>"
                                   + "</item>"
                               + "</parameter>"
                           + "</Root>";
                    objectXML = string.Format(objectXML, "admin", "每月暂估填报流程", curData.Title, curData.ApplyID.ToString(), "项目主管审批", "admin", DateTime.Now.AddYears(100).ToString("yyyy-MM-dd"));

                    string[] args = new string[4];
                    args[0] = ConfigData.Workflow_SystemID;
                    args[1] = ConfigData.Workflow_SystemAcount;
                    args[2] = ConfigData.Workflow_SystemPwd;
                    args[3] = objectXML;
                    string resultXml = WebServicesHelper.InvokeWebService(ConfigData.Workflow_SystemUrl, ConfigData.WorkflowType_ManageWorkflow, args).ToString();

                    System.Xml.XmlDocument xdoc = new System.Xml.XmlDocument();
                    xdoc.LoadXml(resultXml);
                    System.Xml.XmlNode resutNode = xdoc.SelectSingleNode("Result");
                    string success = resutNode.Attributes["Success"].Value;
                    string errmsg = resutNode.Attributes["ErrorMsg"].Value;
                    var strWorkflowInstanceId = resutNode.SelectNodes("item/start/item")[0].Attributes["value"].Value;

                    var workFlow = _tracking_WorkflowService.FindById(Guid.Parse(strWorkflowInstanceId));

                    CurApplyData = curData;
                    CurApplyData.Status = "1";

                    CurApplyData.Series = workFlow.SheetId;
                    CurApplyData.WorkflowInstanceId = Guid.Parse(strWorkflowInstanceId);

                    var succ = _investMonthEstimateApplyService.Insert(CurApplyData);
                    if (succ)
                    {
                        LogRecord.WriteLog(string.Format("每月暂估填报流程申请推送成功"), LogResult.success);
                        AddStrategyLog(string.Format("每月暂估填报流程申请推送成功"), true);
                    }
                    else
                    {
                        LogRecord.WriteLog(string.Format("每月暂估填报流程申请推送失败,其他提示：{0}", errmsg), LogResult.fail);
                        AddStrategyLog(string.Format("每月暂估填报流程申请推送失败,其他提示：{0}", errmsg), false);
                    }

                }
            }
        }


        string GetProjectName(string ProjectID)
        {
            var model = _projectService.FindByFeldName(t => t.ProjectID == ProjectID);
            if (model == null)
                return "";
            return model.ProjectName;
        }
        string GetContractName(string ProjectID, string ContractID)
        {
            var model = _contractService.FindByFeldName(t => t.ProjectID == ProjectID && t.ContractID == ContractID);
            if (model == null)
                return "";
            return model.ContractName;
        }
    }
}
