using CZManageSystem.Core;
using CZManageSystem.Data.Domain.Administrative;
using CZManageSystem.Data.Domain.Composite;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.Administrative;
using CZManageSystem.Service.Composite;
using CZManageSystem.Service.SysManger;
using ServiceLibrary.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary
{
    /// <summary>
    /// 会议室使用结束后5天内没有评论的需要自动好评
    /// </summary>
    public class BoardroomApplyAutoJudge : ServiceJob
    {
        IBoardroomApplyService _boardroomApplyService = new BoardroomApplyService();
        ITracking_WorkflowService _tracking_WorkflowService = new Tracking_WorkflowService();
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

            //查询获取需要自动好评的信息
            List<BoardroomApply> listData = GetApply_needjudge();

            int intSuccrss = 0, intError = 0;
            //对查询到的数据进行自动好评（包含流程自动评论下一步）
            AutoJudgeData(listData, out intSuccrss, out intError);

            sMessage = string.Format("成功{0}条,失败{1}条", intSuccrss, intError);
            return true;
        }

        /// <summary>
        /// 查询获取需要自动好评的信息
        /// </summary>
        public List<BoardroomApply> GetApply_needjudge()
        {
            List<BoardroomApply> listResult = new List<BoardroomApply>();
            BoardroomApplyQueryBuilder queryBuilder = new BoardroomApplyQueryBuilder();
            queryBuilder.State = (new List<int>() { 1 }).ToArray();//已经提交
            queryBuilder.JudgeState = (new List<int>() { 0 }).ToArray();//未评价
            queryBuilder.EndDate_Real_end = DateTime.Now.AddDays(-5);//5天前结束会议

            //先从会议室申请表获取符合条件的数据
            int count = 0;
            var listData = _boardroomApplyService.GetForPaging(out count, queryBuilder).ToList();
            List<int> listID = new List<int>();
            //再筛选处于评论步骤的数据
            foreach (var curData in listData)
            {
                if (!string.IsNullOrEmpty(curData.WorkflowInstanceId))
                {
                    string curActivityName = _tracking_WorkflowService.GetCurrentActivityNames(Guid.Parse(curData.WorkflowInstanceId)).FirstOrDefault();
                    if (curActivityName == "评论" || curActivityName == "完成")
                        listID.Add(curData.ID);
                }
            }
            if (listID.Count > 0)
            {
                listResult = listData.Where(u => listID.Contains(u.ID)).ToList();
            }

            return listResult;
        }

        public void AutoJudgeData(List<BoardroomApply> listData, out int intSuccrss, out int intError)
        {
            intSuccrss = 0;
            intError = 0;
            foreach (var curData in listData)
            {
                bool isSuccess = true;

                ITracking_TodoService tempService = new Tracking_TodoService();
                Tracking_Todo tempActivity = new Tracking_Todo();
                Guid guid = new Guid();
                Guid.TryParse(curData.WorkflowInstanceId, out guid);
                tempActivity = new Tracking_TodoService().Entitys.Where(u => u.WorkflowInstanceId == guid).FirstOrDefault();

                #region 执行下一步流程
                if (tempActivity != null && tempActivity.ActivityInstanceId != Guid.Parse("00000000-0000-0000-0000-000000000000"))
                {
                    string objectXML = "<Root action=\"execute\" username=\"{0}\" keypassword=\"123\">"
                            + "<parameter>"
                                + "<item name=\"activityInstanceId\" value=\"{1}\"/>"
                                + "<item name=\"command\" value=\"cancel\"/>"
                                + "<item name=\"workflowProperties\">"
                                    + "<workflow secrecy=\"0\" urgency=\"0\" importance=\"0\" expectFinishedTime=\"{3}\">"
                                    + "</workflow>"
                                + "</item>"
                                + "<item name = \"Content\" value = \"{2}\" />"
                               + "</parameter>"
                        + "</Root>";
                    objectXML = string.Format(objectXML, "admin", tempActivity.ActivityInstanceId.ToString(), "系统自动评论", DateTime.Now.AddYears(100).ToString("yyyy-MM-dd"));// , nextActivity, nextActors,     curData.ApplyTitle, curData.ID

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
                    int intSuccess = 0;
                    int.TryParse(success, out intSuccess);

                    if (intSuccess > 0)
                    {
                        LogRecord.WriteLog(string.Format("会议室申请ID为{0}的待办信息执行评论成功", curData.ID), LogResult.success);
                        AddStrategyLog(string.Format("会议室申请ID为{0}的待办信息执行评论成功", curData.ID), true);
                    }
                    else
                    {
                        isSuccess = false;
                        LogRecord.WriteLog(string.Format("会议室申请ID为{0}的待办信息执行评论成功", curData.ID), LogResult.fail);
                        AddStrategyLog(string.Format("会议室申请ID为{0}的待办信息执行评论成功", curData.ID), false);
                    }
                }
                else
                {
                    LogRecord.WriteLog(string.Format("会议室申请ID为{0}的信息“{1}”没有找到对应的待办信息", curData.ID, curData.ApplyTitle), LogResult.fail);
                    AddStrategyLog(string.Format("会议室申请ID为{0}的信息“{1}”没有找到对应的待办信息", curData.ID, curData.ApplyTitle), false);
                }
                #endregion

                if (isSuccess)
                {
                    curData.JudgeServiceQuality = "好";
                    curData.JudgeEnvironmental = "好";
                    curData.JudgeState = 2;
                    curData.JudgeTime = DateTime.Now;
                    isSuccess = _boardroomApplyService.Update(curData);
                    if (isSuccess)
                    {
                        LogRecord.WriteLog(string.Format("会议室申请ID为{0}的信息自动评论成功", curData.ID), LogResult.success);
                        AddStrategyLog(string.Format("会议室申请ID为{0}的信息自动评论成功", curData.ID), true);
                    }
                    else
                    {
                        LogRecord.WriteLog(string.Format("会议室申请ID为{0}的信息评论内容保存失败", curData.ID), LogResult.fail);
                        AddStrategyLog(string.Format("会议室申请ID为{0}的信息评论内容保存失败", curData.ID), false);
                    }
                }

                if (isSuccess)
                    intSuccrss++;
                else
                    intError++;

            }


        }

    }

}
