using CZManageSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using WebService_WF.Base;
using WebService_WF.Domain;
using WebService_WF.ExecuteFuns;

namespace WebService_WF
{
    /// <summary>
    /// WebService_WF 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class WebService_WF : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "HelloWorld";
        }

        [WebMethod(Description = "流程执行步骤后调用服务")]
        public string SearchWorkflow(string str1, string str2, string str3, string objectXml)
        {
            try
            {
                WorkflowDetail detail = BaseFun.AnalysisXml(objectXml);
                LogRecord.WriteLog(string.Format("流程“{0}(受理号：{1})”执行步骤“{2}”后调用服务", detail.WorkflowName, detail.SheetId, detail.PreActivities[0].Name), LogResult.normal);

                if (detail.WorkflowName == WfSectionGroup.ConsumableCancelling["WorkFlowName"].ToString())
                {//耗材退库申请
                    new ConsumableCancelling_Execute(detail);
                }
                else if (detail.WorkflowName == WfSectionGroup.ConsumableApply["WorkFlowName"].ToString())
                {//耗材领用申请
                    new ConsumableConsuming_Execute(detail);
                }
                else if (detail.WorkflowName == WfSectionGroup.ConsumableLevelling["WorkFlowName"].ToString())
                {//耗材调平申请
                    new ConsumableLevelling_Execute(detail);
                }
                else if (detail.WorkflowName == WfSectionGroup.ConsumableScrap["WorkFlowName"].ToString())
                {//耗材报废申请 
                    new Consumable_Scrap_Execute(detail);
                }
                else if (detail.WorkflowName == WfSectionGroup.ConsumableMakeup["WorkFlowName"].ToString())
                {//耗材补录归档
                    new ConsumableMakeup_Execute(detail);
                }
                else if (detail.WorkflowName == WfSectionGroup.EquipApply["WorkFlowName"].ToString())
                {//办公设备申请
                    new EquipApply_Execute(detail);
                }
                else if (detail.WorkflowName == WfSectionGroup.BoardroomApply["WorkFlowName"].ToString())
                {//会议室申请
                    new BoardroomApply_Execute(detail);
                }
                else if (detail.WorkflowName == WfSectionGroup.InvestMonthEstimateApply["WorkFlowName"].ToString())
                {//会议室申请
                    new InvestMonthEstimateApply_Execute(detail);
                }
                else if (detail.WorkflowName == WfSectionGroup.InvestAgoEstimateApply["WorkFlowName"].ToString())
                {//历史项目暂估申请
                    new InvestAgoEstimateApply_Execute(detail);
                }
                else if (detail.WorkflowName == WfSectionGroup.VacationApply["WorkFlowName"].ToString())
                {//休假申请
                    new VacationApply_Execute(detail);
                }
                else if (detail.WorkflowName == WfSectionGroup.ReVacationApply["WorkFlowName"].ToString())
                {//异常休假申请
                    new ReVacationApply_Execute(detail);
                }
                else if (detail.WorkflowName == WfSectionGroup.VacationCloseApply["WorkFlowName"].ToString())
                {//销假申请
                    new VacationCloseApply_Execute(detail);
                }
                else if (detail.WorkflowName == WfSectionGroup.MarketOrder_OrderApply_YX["WorkFlowName"].ToString())
                {//营销订单流程
                    new MarketOrder_OrderApply_YX_Execute(detail);
                }
                else
                {
                    LogRecord.WriteLog(string.Format("流程“{0}(受理号：{1})”执行步骤“{2}”后调用服务失败：{3}", detail.WorkflowName, detail.SheetId, detail.PreActivities[0].Name,"没有对应的配置信息"), LogResult.error);
                }
            }
            catch (Exception ex)
            {
                LogRecord.WriteLog(string.Format("流程执行步骤后调用服务失败：{0}", ex.Message), LogResult.error);
            }
            return "";
        }
    }
}
