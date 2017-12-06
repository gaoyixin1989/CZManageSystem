using Aspose.Cells;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace CZManageSystem.Admin.Base
{
    public class CommonFunction
    {
        #region GetSerialNo获取申请工单序列号
        public static string GetSerialNo(string sListNumMode)
        {
            string sResult = "";
            if (!string.IsNullOrEmpty(sListNumMode))
            {
                var mm = new EfRepository<string>().Execute<string>(string.Format("exec Sp_WorkFlow_GetSerialNo @sPrefix='{0}'", sListNumMode)).ToList();
                sResult = mm[0].ToString();
            }
            return sResult;
        }
        #endregion

        /// <summary>
        /// 根据ids获取用户名称信息
        /// </summary>
        public static string getUserRealNamesByIDs(string ids)
        {
            string strResult = "";
            if (!string.IsNullOrEmpty(ids))
            {
                ISysUserService _userService = new SysUserService();
                string[] arrID = ids.Split(',');
                var mm = _userService.List().Where(u => arrID.Contains(u.UserId.ToString())).Select(u => u.RealName).ToList();
                strResult = string.Join(",", mm);
            }
            return strResult;
        }
        /// <summary>
        /// 根据ids获取部门名称信息
        /// </summary>
        public static string getDeptNamesByIDs(string ids)
        {
            string strResult = "";
            if (!string.IsNullOrEmpty(ids))
            {
                ISysDeptmentService _sysDeptmentService = new SysDeptmentService();
                string[] arrID = ids.Split(',');
                var mm = _sysDeptmentService.List().Where(u => arrID.Contains(u.DpId)).Select(u => u.DpName).ToList();
                strResult = string.Join(",", mm);
            }
            return strResult;
        }

        /// <summary>
        /// 根据流程实例ID获取已经完成的步骤信息
        /// </summary>
        /// <param name="WorkflowInstanceId">流程实例ID</param>
        /// <returns></returns>
        public static List<Tracking_Activities_Completed> GetActivitiesCompletedByID(Guid? WorkflowInstanceId)
        {
            WorkflowInstanceId = WorkflowInstanceId ?? Guid.Parse("00000000-0000-0000-0000-000000000000");
            int count = 0;
            var queryCondition = new
            {
                WorkflowInstanceId = WorkflowInstanceId
            };
            var listData = new Tracking_Activities_CompletedService().GetForPaging(out count, queryCondition).Select(u => (Tracking_Activities_Completed)u).ToList();
            listData = listData.OrderBy(u => u.FinishedTime).ToList();

            return listData;

        }

        public static string GetActivitieResultText(string value)
        {
            string result = "";
            switch (value)
            {
                case "approve": result = "通过"; break;
                case "reject": result = "退回"; break;
                case "close_activities": result = "作废"; break;
                default: break;
            }
            return result;
        }

        /// <summary>
        /// 获取流程状态名称说明
        /// </summary>
        public static string GetFlowStateText(Tracking_Workflow workflow)
        {
            string strResult = "未提交";
            if (workflow != null)
            {
                switch (workflow.State)
                {
                    case 1: strResult = "已提交"; break;
                    case 2: strResult = "已完成"; break;
                    case 99: strResult = "已取消"; break;
                    default: break;
                }
            }
            return strResult;
        }
        /// <summary>
        /// 从xml中解析出下一步的操作人信息
        /// </summary>
        /// <param name="resultXml">xml结果</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetActivitiesInfo(string resultXml)
        {
            Dictionary<string, string> dictResult = new Dictionary<string, string>();

            XmlDocument doc = new XmlDocument();
            XElement element = XElement.Parse(resultXml);//将string解析为XML
            XElement firstItem = element.Element("item");
            if (firstItem != null)
            {
                IEnumerable<XElement> ActivityNodes = firstItem.Elements("Activity");
                if (ActivityNodes != null)
                {
                    foreach (XElement curActivity in ActivityNodes)
                    {
                        string activityName = "";
                        string sctorUsers = "";
                        List<object> listUser = new List<object>();
                        IEnumerable<XElement> items = curActivity.Elements("item");
                        foreach (XElement curItem in items)
                        {
                            string temp1 = curItem.Attribute("name").Value.ToLower();
                            switch (temp1)
                            {
                                case "name": activityName = curItem.Attribute("value").Value; break;
                                case "actors": sctorUsers = curItem.Attribute("value").Value; break;
                                default: break;
                            }
                        }

                        if (!dictResult.ContainsKey(activityName))
                            dictResult.Add(activityName, sctorUsers);

                    }
                }
            }
            return dictResult;
        }

        /// <summary>
        /// 从xml中解析出下一步的抄送人人信息
        /// </summary>
        /// <param name="resultXml">xml结果</param>
        /// <returns></returns>
        public static List<ActivitiesProfile> GetActivitiesProfileInfo(string resultXml)
        {
            List<ActivitiesProfile> listResult = new List<ActivitiesProfile>();

            XmlDocument doc = new XmlDocument();
            XElement element = XElement.Parse(resultXml);//将string解析为XML
            XElement firstItem = element.Element("item");
            if (firstItem != null)
            {
                IEnumerable<XElement> ActivityNodes = firstItem.Elements("ActivitiesProfile");
                if (ActivityNodes != null)
                {
                    foreach (XElement curActivity in ActivityNodes)
                    {
                        ActivitiesProfile curValue = new ActivitiesProfile();
                        List<object> listUser = new List<object>();
                        IEnumerable<XElement> items = curActivity.Elements("item");
                        foreach (XElement curItem in items)
                        {
                            string temp1 = curItem.Attribute("name").Value.ToLower();
                            switch (temp1)
                            {
                                case "name": curValue.Name = curItem.Attribute("value").Value; break;
                                case "actors": curValue.Actors = curItem.Attribute("value").Value; break;
                                case "reviewtype": curValue.ReviewType = curItem.Attribute("value").Value; break;
                                case "reviewvalidatetype": curValue.ReviewValidateType = (curItem.Attribute("value").Value.ToLower() == "true"); break;
                                case "reviewactorcount":
                                    {
                                        int intTemp = curValue.ReviewActorCount;
                                        int.TryParse(curItem.Attribute("value").Value, out intTemp);
                                        curValue.ReviewActorCount = intTemp;
                                        break;
                                    }
                                default: break;
                            }
                        }
                        listResult.Add(curValue);

                    }
                }
            }

            return listResult;
        }

        //调用接口查询信息
        public static string SearchWorkflow_infolist(string workflowName, string curUserName, string activityInstanceId = null)
        {
            if (string.IsNullOrEmpty(curUserName))
                curUserName = "admin";
            string objectXML = "<Root action =\"infolist\" username =\"{2}\" keypassword =\"7e357adc-1b28-4753-9718-f8b23d3db974\" pageindex =\"0\" pagecount =\"10\">"
                                + "<parameter>"
                                    + "<item name =\"ActivityInstanceId\" value =\"{1}\"/>"
                                    + "<item name =\"WorkflowAlias\" value =\"\"/>"
                                    + "<item name =\"Workflows\" value =\"{0}\"/>"
                                + "</parameter>"
                             + "</Root>";
            objectXML = string.Format(objectXML, workflowName, activityInstanceId, curUserName);//, "d756ecab-6883-4737-b817-055e94221d55"
            string[] args = new string[4];
            args[0] = FlowInstance.Workflow_SystemID;
            args[1] = FlowInstance.Workflow_SystemAcount;
            args[2] = FlowInstance.Workflow_SystemPwd;
            args[3] = objectXML;
            string resultXml = WebServicesHelper.InvokeWebService(FlowInstance.Workflow_SystemUrl, FlowInstance.MethodName.SearchWorkflow, args).ToString();
            return resultXml;
        }

        /// <summary>
        /// 获取某流程提交后的第一个可选操作步骤和操作人
        /// </summary>
        /// <param name="workflowName">流程名称</param>
        /// <param name="nextActivity">步骤名称</param>
        /// <param name="nextActors">操作人</param>
        public static void GetFirstOperatorInfoAfterSumbit(string workflowName, string curUserName, out string nextActivity, out string nextActors)
        {
            nextActivity = "";
            nextActors = "";
            string strXml = SearchWorkflow_infolist(workflowName, curUserName);
            Dictionary<string, string> dictOperator = GetActivitiesInfo(strXml);
            if (dictOperator.Count > 0)
            {
                nextActivity = dictOperator.Keys.First();
                nextActors = dictOperator.Values.First();
            }
        }

        /// <summary>
        /// 抄送待阅工单
        /// </summary>
        /// <param name="senderActivityInstanceId">前一个节点实例ID</param>
        /// <param name="message">信息内容</param>
        /// <param name="sender">发送人</param>
        /// <param name="workflowId">流程ID</param>
        /// <param name="workflowTitle">工单标题</param>
        /// <param name="activityId">当前步骤流程ID</param>
        /// <param name="activityInstanceId">当前步骤实例ID</param>
        /// <param name="actors">接受人</param>
        public static bool PendingWF(Guid senderActivityInstanceId, string message, string sender, Guid workflowId
            , string workflowTitle, Guid activityId, Guid activityInstanceId, IList<string> actors)
        {
            Assembly assembly = null;
            assembly = Assembly.Load("Botwave.XQP");
            Type tx = assembly.GetType("Botwave.XQP.Domain.ToReview");

            Type[] tp = new Type[8];
            tp[0] = typeof(Guid);
            tp[1] = typeof(string);
            tp[2] = typeof(string);
            tp[3] = typeof(Guid);
            tp[4] = typeof(string);
            tp[5] = typeof(Guid);
            tp[6] = typeof(Guid);
            tp[7] = typeof(IList<string>);

            bool re = (bool)tx.InvokeMember("OnPendingReview", BindingFlags.InvokeMethod, null, null
                , new object[] {
                                senderActivityInstanceId,
                                    message,
                                    sender,
                                    workflowId,
                                    workflowTitle,
                                    activityId,
                                    activityInstanceId,
                                    actors
                                });
            return re;

        }

        /// <summary>
        /// 流程提交后抄送
        /// </summary>
        /// <param name="data"></param>
        public static void PendingData(Guid WorkflowInstanceId, string nextCC)
        {
            List<string> listActor = new List<string>();
            foreach (var dd in nextCC.Split(','))
                listActor.Add(dd);

            Tracking_Todo todoActivity = new Tracking_TodoService().FindByFeldName(u => u.WorkflowInstanceId == WorkflowInstanceId);
            Tracking_Activities_Completed completedActivity = new Tracking_Activities_CompletedService().List()
                .Where(u => u.WorkflowInstanceId == WorkflowInstanceId).OrderByDescending(u => u.FinishedTime).FirstOrDefault();
            Workflows wf = new WorkflowsService().FindByFeldName(u => u.IsCurrent == true && u.WorkflowName == todoActivity.WorkflowName);
            PendingWF(
                       completedActivity.ActivityInstanceId,
                       "您接收到待阅信息“" + todoActivity.Title + "”，请查看",
                       completedActivity.Actor,
                       wf.WorkflowId,
                       todoActivity.Title,
                       todoActivity.ActivityId,
                       todoActivity.ActivityInstanceId,
                       listActor
                );
        }



        //判断是否为手机号码
        public static bool isMobilePhone(string value)
        {
            //电信手机号正则表达式
            string dianxin = @"^1[3578][01379]\d{8}$";
            Regex dReg = new Regex(dianxin);
            //移动手机号正则表达式
            string yidong = @"^(134[012345678]\d{7}|1[34578][012356789]\d{8})$";
            Regex yReg = new Regex(yidong);
            //联通手机号正则表达式
            string liantong = @"^1[34578][01256]\d{8}$";
            Regex lReg = new Regex(liantong);

            value = value.Trim();
            if (dReg.IsMatch(value) || yReg.IsMatch(value) || lReg.IsMatch(value))
                return true;
            else
                return false;

        }

        /// <summary>
        /// 发送手机短信
        /// </summary>
        /// <param name="listMobileto">接收手机号码</param>
        /// <param name="content">短信内容</param>
        /// <returns></returns>
        public static bool SendSms(List<string> listMobileto, string content)
        {
            string SMS_Contention = System.Configuration.ConfigurationManager.AppSettings["SMS_Contention"];//短信平台_数据库连接字符串
            string SMS_ProName = System.Configuration.ConfigurationManager.AppSettings["SMS_ProName"];//短信平台_发送短信调用的存储过程
            string SMS_Port = System.Configuration.ConfigurationManager.AppSettings["SMS_Port"];//短信平台_发送端口
            string SMS_Type = System.Configuration.ConfigurationManager.AppSettings["SMS_Type"];//短信平台_服务类型
            string SMS_State = System.Configuration.ConfigurationManager.AppSettings["SMS_State"];//短信平台_是否需要状态报告

            SmsDAO.init(SMS_Contention);//初始化短信系统数据库连接字符串
            bool result = false;
            foreach(string mobileto in listMobileto)
            {
                if (!string.IsNullOrEmpty(mobileto))
                {
                    int temperrcode = SmsDAO.sqlExec(string.Format("Exec {0} '{1}','{2}','{3}','{4}','{5}','','{6}'"
                        , SMS_ProName
                        , mobileto
                        , SMS_Port
                        , mobileto
                        , content
                        , SMS_Type
                        , SMS_State));
                    if (temperrcode <= 0)
                        result = true;
                }

            }            

            return result;
        }
        /// <summary>
        /// 获取部门、室，如果是某营销中心或者服营厅的  只获取其部门，分公司也算一个部门
        /// </summary>
        /// <param name="dpid"></param>
        /// <returns></returns>
        public static List<string> GetDepartMent(string dpid)
        {
            List<string> listResult = new List<string>();
            if (!string.IsNullOrEmpty(dpid))
            {
                IUum_OrganizationinfoService _sysDeptmentService = new Uum_OrganizationinfoService();
                var _obj = _sysDeptmentService.FindById(dpid);
                if(_obj!=null)
                {
                    if (_obj.department.Contains("营销中心") || _obj.department.Contains("服营厅"))
                    {
                        listResult.Add(_obj.branch);
                        listResult.Add("");
                    }
                    else if (string.IsNullOrEmpty(_obj.department) || _obj.department == "")
                    {
                        listResult.Add(_obj.branch);
                        listResult.Add("");
                    }
                    else
                    {
                        listResult.Add(_obj.department);
                        listResult.Add(_obj.userGroup);
                    }
                }
                else
                {
                    listResult.Add("");
                    listResult.Add("");
                }
                
            }
            return listResult;
        }
    }
}

#region 该文件需要的其他自定义类
/// <summary>
/// 流程抄送信息类
/// </summary>
public class ActivitiesProfile
{
    public string Name = "";//节点名称
    public string Actors = "";//抄送人信息
    public string ReviewType = "";//类别：None-不支持抄送，Classic-旧有方式的抄送(即用户自由选择)，CheckBox-复选框方式的抄送
    public bool ReviewValidateType = false;//抄送人是否默认全选
    public int ReviewActorCount = -1;//人数限制，-1代表不限制
}
#endregion
#region 公共转换类 
public static class CommonConvert
{

    public static string DateConveryyyyMM(string date)
    {
        if (date.Length != 6)
            return "";
        DateTime dt = DateTime.ParseExact(date, "yyyyMM", System.Globalization.CultureInfo.InvariantCulture);
        return dt.ToString("yyyy年M月");
    }
    public static string DateTimeToString(DateTime? date, string dateFormat = "yyyy-MM-dd")
    {
        if (date == null)
            return "";
        DateTime dt = Convert.ToDateTime(date);
        return dt.ToString(dateFormat);
    }
}
#endregion