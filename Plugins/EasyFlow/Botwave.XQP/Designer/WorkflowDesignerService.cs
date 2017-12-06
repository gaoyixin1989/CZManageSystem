using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using System.Linq;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.XQP.Domain;
using Newtonsoft.Json;
//using LitJson;
using Botwave.Workflow.Routing.Domain;
using Botwave.Workflow.Routing.Implements;
using Botwave.XQP.Service;
using Botwave.Workflow.Extension.Domain;
using Botwave.Workflow.Extension.Util;
using Botwave.Workflow.Extension.Service;

namespace Botwave.XQP.Designer
{
    /// <summary>
    /// 流程可视化设计数据保存服务类.
    /// </summary>
    [WebService(Namespace = "http://www.botwave.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService()]//如果不添加它，Json访问WebService会出错
    public class WorkflowDesignerService : System.Web.Services.WebService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(WorkflowDesignerService));

        private static IActivityDefinitionService activityDefinitionService;
        private static IDeployService deployService;
        private static IActivityRulesService activityRulesService;
        private static IWorkflowRoleService workflowRoleService;
        private static IWorkflowResourceService workflowResourceService;

        static WorkflowDesignerService()
        {
            activityDefinitionService = Spring.Context.Support.WebApplicationContext.Current["activityDefinitionService"] as IActivityDefinitionService;
            deployService = Spring.Context.Support.WebApplicationContext.Current["deployService"] as IDeployService;
            activityRulesService = Spring.Context.Support.WebApplicationContext.Current["activityRulesService"] as IActivityRulesService;
            workflowRoleService = Spring.Context.Support.WebApplicationContext.Current["workflowRoleService"] as IWorkflowRoleService;
            workflowResourceService = Spring.Context.Support.WebApplicationContext.Current["workflowResourceService"] as IWorkflowResourceService;
        }

        public WorkflowDesignerService()
        {
            //如果使用设计的组件，请取消注释以下行 
            //InitializeComponent(); 
        }

        /// <summary>
        /// 保存流程可视化设计的 XML 数据.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        [WebMethod(Description = "保存流程可视化设计的 XML 数据.")]
        public string SaveWorkflowXml(string key, string xmlData)
        {
            string creator = Botwave.Security.LoginHelper.UserName;
            string retrunValue = "false";
            if (string.IsNullOrEmpty(xmlData))
            {
                log.ErrorFormat("XML Data is Null Or Empty.[key:{0}]", key);
                return retrunValue;
            }
            else
            {
                Botwave.XQP.Commons.LogWriter.Write(creator, "部署流程" + key, xmlData);
                log.DebugFormat("[workflowId]{0}\r\n[data]{1}", key, xmlData);
            }
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlData);

                WorkflowComponent profile = new WorkflowComponent();
                profile.LoadWorkflowData(doc.DocumentElement);  // 更新流程设置 <workflow />
                IDictionary<string, WorkflowActivity> profileActivities = new Dictionary<string, WorkflowActivity>(StringComparer.CurrentCultureIgnoreCase);

                for (int i = 0; i < doc.DocumentElement.ChildNodes.Count; i++)
                {
                    XmlNode childNode = doc.DocumentElement.ChildNodes[i];
                    string nodeName = childNode.Name;
                    switch (nodeName)
                    {
                        case "creationControl":
                            profile.LoadCreationData(childNode);   // 更新流程发单控制
                            doc.DocumentElement.RemoveChild(childNode);
                            i--;
                            break;
                        case "smsNotify":
                            profile.SmsNotifyFormat = childNode.InnerText;
                            doc.DocumentElement.RemoveChild(childNode);
                            i--;
                            break;
                        case "emailNotify":
                            profile.EmailNotifyFormat = childNode.InnerText;
                            doc.DocumentElement.RemoveChild(childNode);
                            i--;
                            break;
                        case "statSMSNotify":
                            profile.StatSmsNodifyFormat = childNode.InnerText;
                            doc.DocumentElement.RemoveChild(childNode);
                            i--;
                            break;
                        case "statEmailNotify":
                            profile.StatEmailNodifyFormat = childNode.InnerText;
                            doc.DocumentElement.RemoveChild(childNode);
                            i--;
                            break;
                        case "activity":
                        case "start-activity":
                        case "end-activity":
                            WorkflowActivity activity = new WorkflowActivity();
                            activity.LoadData(childNode);
                            if (!profileActivities.ContainsKey(activity.ActivityName))
                            {
                                profileActivities.Add(activity.ActivityName, activity);
                            }
                            break;
                        default:
                            break;
                    }
                }

                using (StringReader reader = new StringReader(doc.OuterXml))
                {
                    using (XmlReader xmlReader = XmlReader.Create(reader))
                    {
                        DeployActionResult result = deployService.DeployWorkflow(xmlReader, creator);
                        if (xmlReader.ReadState != ReadState.Closed)
                            xmlReader.Close();

                        Botwave.XQP.Commons.LogWriter.Write(creator, "部署返回结果", "WorkflowId:{0} Result:{1}  Message:{2}", result.WorkflowId, result.Success, result.Message);
                        Guid workflowId = result.WorkflowId;
                        key = workflowId.ToString();
                        if (result.Success)
                        {
                            retrunValue = key;
                            // 设置更新流程活动坐标位置.
                            if (activityDefinitionService != null)
                            {
                                IList<ActivityDefinition> activities = activityDefinitionService.GetActivitiesByWorkflowId(workflowId);
                                if (activities != null && activities.Count > 0)
                                {
                                    foreach (ActivityDefinition activity in activities)
                                    {
                                        string name = activity.ActivityName;
                                        if (profileActivities.ContainsKey(name))
                                        {
                                            WorkflowActivity designerActivity = profileActivities[name];
                                            WorkflowActivity.UpdatePosition(activity.ActivityId, designerActivity.X, designerActivity.Y);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    reader.Close();
                }

                profile.UpdateData();
            }
            catch (Exception ex)
            {
                Botwave.XQP.Commons.LogWriter.Write(creator, ex);
                log.Error(ex);
                return retrunValue;
            }
            return retrunValue;
        }

        /// <summary>
        /// 保存流程可视化设计的 XML 数据.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        [WebMethod(Description = "保存流程可视化设计的 JSON 数据.")]
        public string SaveWorkflowJson(string jsonData)
        {
            string retrunValue = "false";
            string creator = Botwave.Security.LoginHelper.UserName;
            if (string.IsNullOrEmpty(jsonData))
            {
                log.ErrorFormat("JSON Data is Null Or Empty.");
                return retrunValue;
            }
            string key = "";
            string xmlData = string.Empty;
            Botwave.Workflow.ActionResult acitonResult;
            DeployActionResult result = new DeployActionResult(); ;
            try
            {
                Botwave.Workflow.Extension.Util.ResourceHelper.Resources_WorkflowCommons = new string[] { 
            "流程协作", "提单","报表统计","查看保密单","高级查询","流程管理" };
                ProcessData processData = JsonConvert.DeserializeObject<ProcessData>(jsonData);
                //ProcessData processData =LitJson.JsonMapper.ToObject<ProcessData>(jsonData);
                WorkflowSetting setting = processData.Setting;
                WorkflowComponent workflow = processData.Profile;
                XmlDocument doc = new XmlDocument();
                XmlNode root = workflow.CreateNode(doc);
                IList<list> activityList = processData.List;
                foreach (list l in activityList)
                {
                    WorkflowActivity activityDefinition = l.Activity;
                    AllocatorOption assignment = l.Assignment;

                    string prevActivityNames = "";
                    foreach (string a in activityDefinition.PrevActivityNames)
                    {
                        prevActivityNames += "," + a;
                    }
                    prevActivityNames = prevActivityNames.Length > 1 ? prevActivityNames.Substring(1) : prevActivityNames;
                    string nextActivityNames = "";
                    foreach (string a in activityDefinition.NextActivityNames)
                    {
                        nextActivityNames += "," + a;
                    }
                    nextActivityNames = nextActivityNames.Length > 1 ? nextActivityNames.Substring(1) : nextActivityNames;
                    activityDefinition.PrevActivity = prevActivityNames;
                    activityDefinition.NextActivity = nextActivityNames;
                    XmlNode activityNode = activityDefinition.CreateNode(doc);
                    XmlElement assignmentNode = WorkflowActivity.CreateAllocatorNode(doc, assignment, "assignmentAllocator");
                    activityNode.AppendChild(assignmentNode);
                    root.AppendChild(activityNode);
                }
                Botwave.XQP.Commons.LogWriter.Write(creator, "部署流程" + key, xmlData);
                log.DebugFormat("[workflowId]{0}\r\n[data]{1}", key, xmlData);

                WorkflowComponent profile = new WorkflowComponent();
                profile.LoadWorkflowData(root);  // 更新流程设置 <workflow />
                IDictionary<string, WorkflowActivity> profileActivities = new Dictionary<string, WorkflowActivity>(StringComparer.CurrentCultureIgnoreCase);

                for (int i = 0; i < root.ChildNodes.Count; i++)
                {
                    XmlNode childNode = root.ChildNodes[i];
                    string nodeName = childNode.Name;
                    switch (nodeName)
                    {
                        case "creationControl":
                            profile.LoadCreationData(childNode);   // 更新流程发单控制
                            root.RemoveChild(childNode);
                            i--;
                            break;
                        case "smsNotify":
                            profile.SmsNotifyFormat = childNode.InnerText;
                            root.RemoveChild(childNode);
                            i--;
                            break;
                        case "emailNotify":
                            profile.EmailNotifyFormat = childNode.InnerText;
                            root.RemoveChild(childNode);
                            i--;
                            break;
                        case "statSMSNotify":
                            profile.StatSmsNodifyFormat = childNode.InnerText;
                            root.RemoveChild(childNode);
                            i--;
                            break;
                        case "statEmailNotify":
                            profile.StatEmailNodifyFormat = childNode.InnerText;
                            root.RemoveChild(childNode);
                            i--;
                            break;
                        case "remark":
                            profile.Remark = childNode.InnerText;
                            break;
                        case "activity":
                        case "start-activity":
                        case "end-activity":
                            WorkflowActivity activity = new WorkflowActivity();
                            activity.LoadData(childNode);
                            if (!profileActivities.ContainsKey(activity.ActivityName))
                            {
                                profileActivities.Add(activity.ActivityName, activity);
                            }
                            break;
                        default:
                            break;
                    }
                }
                StringReader reader = new StringReader(root.OuterXml);
                XmlReader xmlReader = XmlReader.Create(reader);

                acitonResult = deployService.CheckWorkflow(xmlReader);
                if (!acitonResult.Success)
                {
                    if (xmlReader.ReadState != ReadState.Closed)
                        xmlReader.Close();
                    reader.Close();
                    Botwave.XQP.Commons.LogWriter.Write(creator, "部署返回结果", "WorkflowId:{0} Result:{1}  Message:{2}", result.WorkflowId, acitonResult.Success, acitonResult.Message);
                    retrunValue = acitonResult.Message;
                    return "{\"result\":\"error\",\"info\":\"" + retrunValue + "\"}";
                }
                //using (StringReader reader = new StringReader(root.OuterXml))
                //{
                reader = new StringReader(root.OuterXml);
                using (xmlReader = XmlReader.Create(reader))
                {
                    result = deployService.DeployWorkflow(xmlReader, creator);
                    if (xmlReader.ReadState != ReadState.Closed)
                        xmlReader.Close();

                    Botwave.XQP.Commons.LogWriter.Write(creator, "部署返回结果", "WorkflowId:{0} Result:{1}  Message:{2}", result.WorkflowId, result.Success, result.Message);
                    Guid workflowId = result.WorkflowId;
                    key = workflowId.ToString();
                    if (result.Success)
                    {
                        retrunValue = key;
                        // 设置更新流程活动坐标位置.
                        if (activityDefinitionService != null)
                        {
                            WorkflowActivitySetting(profileActivities,activityList,workflowId,creator);
                        }
                        SetProfile(profile,processData.ManagerIds,creator);
                    }
                    //}
                    reader.Close();
                }
            }

            catch (Exception ex)
            {
                Botwave.XQP.Commons.LogWriter.Write(creator, ex);
                log.Error(ex);
                //return retrunValue;
                return "{\"result\":\"error\",\"info\":\"" + ex.Message + "\"}";
            }
            log.InfoFormat("[workflowId]{0}\r\n[data]{1}", key, xmlData);
            if (result.Success)
                return "{\"result\":\"success\",\"info\":\"" + retrunValue + "\"}";
            else
                return "{\"result\":\"error\",\"info\":\"" + retrunValue + "\"}";
        }

        /// <summary>
        /// 设置更新流程活动坐标位置与规则库
        /// </summary>
        /// <param name="profileActivities"></param>
        /// <param name="activityList"></param>
        /// <param name="workflowId"></param>
        /// <param name="creator"></param>
        private void WorkflowActivitySetting(IDictionary<string, WorkflowActivity> profileActivities, IList<list> activityList, Guid workflowId,string creator)
        {
            try
            {
                log.Info("设置更新流程活动坐标位置与规则库." + workflowId);
                Botwave.XQP.Commons.LogWriter.Write(creator, "设置更新流程活动坐标位置与规则库." + workflowId, "");

                IList<ActivityDefinition> activities = activityDefinitionService.GetActivitiesByWorkflowId(workflowId);
                if (activities != null && activities.Count > 0)
                {
                    foreach (ActivityDefinition activityDefinition in activities)
                    {
                        string name = activityDefinition.ActivityName;
                        if (profileActivities.ContainsKey(name))
                        {
                            WorkflowActivity designerActivity = profileActivities[name];
                            WorkflowActivity.UpdatePosition(activityDefinition.ActivityId, designerActivity.X, designerActivity.Y);
                            CZActivityDefinition.UpdateWorkflowActivityPrint(designerActivity);
                        }
                        string rulesJson = activityList.Where(a => a.Process_name == name).First().Rules;
                        if (!string.IsNullOrEmpty(rulesJson))
                        {
                            rulesJson = Botwave.XQP.Commons.XQPHelper.DecodeBase64("utf-8", rulesJson);
                            IList<RulesDetail> rules = JsonConvert.DeserializeObject<List<RulesDetail>>(rulesJson);
                            //IList<RulesDetail> rules = JsonMapper.ToObject<List<RulesDetail>>(rulesJson);
                            foreach (RulesDetail rulesDetail in rules)
                            {
                                rulesDetail.Ruleid = Guid.NewGuid();
                                rulesDetail.Workflowid = workflowId;
                                rulesDetail.StepType = 1;
                                rulesDetail.ParentRuleId = Guid.Empty;
                                rulesDetail.Status = 1;
                                rulesDetail.Creator = creator;
                                rulesDetail.Createdtime = DateTime.Now;
                                rulesDetail.LastModifier = creator;
                                rulesDetail.LastModtime = DateTime.Now;
                                if (activityRulesService.ExistActivityRules(rulesDetail) > -1)
                                {
                                    activityRulesService.ActivityRulesDetailUpdateByActName(rulesDetail);
                                }
                                else
                                {
                                    activityRulesService.ActivityRulesDetailInsert(rulesDetail);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("设置更新流程活动坐标位置与规则库." + workflowId+"出错："+ex.ToString());
                Botwave.XQP.Commons.LogWriter.Write(creator, ex);
            }
        }

        /// <summary>
        /// 更新流程基本信息设置
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="ManagerIds"></param>
        /// <param name="creator"></param>
        private void SetProfile(WorkflowComponent profile, string ManagerIds,string creator)
        {
            try
            {
                log.Info("更新流程基本信息设置." + profile.WorkflowName);
                Botwave.XQP.Commons.LogWriter.Write(creator, "设置更新流程活动坐标位置与规则库." + profile.WorkflowName, "");
                profile.UpdateData();
                //从角色中加载流程管理员
                string resourceid = "";
                Botwave.Security.Domain.ResourceInfo manager = workflowRoleService.GetResourceInfoByName(profile.WorkflowName + "-流程管理");
                if (manager != null)
                    resourceid = manager.ResourceId;
                else
                {
                    resourceid = manager.ResourceId + "0005";
                    //manager = workflowRoleService.GetResourceInfoByName(profile.WorkflowName);
                    ResourceProperty item = new ResourceProperty(resourceid, manager.ResourceId, Botwave.Workflow.Extension.Util.ResourceHelper.Resources_WorkflowCommons[5], profile.WorkflowName);
                    item.Type = ResourceHelper.ResourceType_Common;
                    item.SortIndex = 7;
                    IList<ResourceProperty> newResources = new List<ResourceProperty>();
                    newResources.Add(item);
                    workflowResourceService.InsertResources(newResources);
                }
                IList<Guid> userIds = new List<Guid>();
                if (!string.IsNullOrEmpty(ManagerIds))
                {
                    string[] ids = ManagerIds.Replace("'", "").Split(',');

                    foreach (string id in ids)
                    {
                        userIds.Add(new Guid(id));
                    }

                }
                string result = workflowRoleService.InsertWorkflowManager(resourceid, userIds, creator);
                Botwave.XQP.Commons.LogWriter.Write("配置流程管理员", "配置流程管理员部署返回结果" + result);
            }
            catch (Exception ex)
            {
                log.Error("更新流程基本信息设置.出错："+ex.ToString());
                Botwave.XQP.Commons.LogWriter.Write(creator, ex);
            }
        }
    }
}
