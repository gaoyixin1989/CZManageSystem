using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Text;
using System.Xml;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Plugin;

namespace Botwave.Workflow.Service
{
    /// <summary>
    /// 流程部署服务的抽象类.
    /// </summary>
    public abstract class AbstarctDeployService : IDeployService
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(AbstarctDeployService));

        #region 字段

        /// <summary>
        /// 不进行权限控制的分派资源前缀.使得系统找不到该资源.
        /// </summary>
        protected static readonly string PrefixDisableResource = "#NONE#_";

        /// <summary>
        /// 流程部署文件的注释内容.
        /// </summary>
        protected static readonly string FlowComment = @"# 流程名称与所有人是必填的，以名称作为标识，如果存在流程名称重复，则新增一个版本并作为当前版本;
# remark：表示流程的备注内容，并且只能有一个 remark 节点;
# 所有流程都必须有且只有一个开始步骤(start-activity)与结束步骤(end-activity)，需要有一个以上(包含一个)的中间步骤(activity);
# 开始步骤(start-activity)的名称(name)可以为空(此时默认为初始化)，结束步骤(end-activity)的名称也可以为空(此时默认为完成);
# prevActivity:表示上一步骤；nextActivity：表示下一步骤。两个属性的值是相应的步骤名称(多个名称之间以','或者'，'隔开);
# 中间步骤(activity)必须有名称(Name)，以及上一步骤(prevActivity)和下一步骤(nextActivity);
# 开始步骤(start-activity)的下一步骤(nextActivity)不能为空，结束步骤(end-activity)的上一步骤(prevActivity)不能为空;
# 在一个流程之中的活动名称必须唯一;
# joinCondition、splitCondition、countersignedCondition分别是合并条件、分支条件、会签条件
# commandRules：表示对应的步骤(activity)的命令规则，主要用于流程自动处理;
# 一个步骤(activity)中只能有一个 commandRules 节点。
# taskAllocator 任务分配配置节点.
# extAllocators 各任务分派实例以分号隔开；冒号后面为其参数，各参数之间以逗号分隔.如, superior:arg1,arg2;processor:1
# decisionType指分支选择类型，分为manual(手动)与auto（自动）两种，默认为手动
# rejectOption指拒绝/退回时的选择,initial退回起始/提单状态,previous退回上一步,none不允许退回,还可以是特定的步骤名称";

        #endregion

        #region service interfaces

        private IWorkflowDefinitionService workflowDefinitionService;
        private IActivityDefinitionService activityDefinitionService;
        private IResourceTranslator resourceTranslator;
        private IPreCommitDeployHandler preCommitDeployHandler;
        private IPostDeployHandler postDeployHandler;

        /// <summary>
        /// 流程定义服务.
        /// </summary>
        public IWorkflowDefinitionService WorkflowDefinitionService
        {
            get { return workflowDefinitionService; }
            set { workflowDefinitionService = value; }
        }

        /// <summary>
        /// 流程步骤定义服务.
        /// </summary>
        public IActivityDefinitionService ActivityDefinitionService
        {
            get { return activityDefinitionService; }
            set { activityDefinitionService = value; }
        }

        /// <summary>
        /// 权限资源转换服务.
        /// </summary>
        public IResourceTranslator ResourceTranslator
        {
            get { return resourceTranslator; }
            set { resourceTranslator = value; }
        }

        /// <summary>
        /// 提交部署数据的前续处理.
        /// </summary>
        public IPreCommitDeployHandler PreCommitDeployHandler
        {
            get { return preCommitDeployHandler; }
            set { preCommitDeployHandler = value; }
        }

        /// <summary>
        /// 部署的后续处理.
        /// </summary>
        public IPostDeployHandler PostDeployHandler
        {
            get { return postDeployHandler; }
            set { postDeployHandler = value; }
        }

        #endregion

        #region IDeployService 成员

        /// <summary>
        /// 检查流程 XML 文档格式.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public virtual ActionResult CheckWorkflow(XmlReader reader)
        {
            ActionResult result = new ActionResult();
            result.Success = true;
            string strName = "";

            if (!CheckChildNodes(reader, ref strName))
            {
                result.Success = false;
                result.Message = "配置文件中的子节点配置有误!请检查配置文件!";
                return result;
            }
            if (CheckNodeCount("start-activity", strName) != 1)
            {
                result.Success = false;
                result.Message = "所有流程都必须有且只有一个开始活动 start-activity!";
                return result;
            }
            if (CheckNodeCount("activity", strName) < 1)
            {
                result.Success = false; result.Message = "所有流程都必须有一个以上(包含一个)的中间活动 activity!";
                return result;
            }
            if (CheckNodeCount("end-activity", strName) != 1)
            {
                result.Success = false;
                result.Message = "所有流程都必须有且只有一个结束活动 end-activity!";
                return result;
            }
            return result;
        }

        /// <summary>
        /// 部署指定流程文档定义.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="creator"></param>
        /// <returns></returns>
        public virtual DeployActionResult DeployWorkflow(XmlReader reader, string creator)
        {
            DeployActionResult result = new DeployActionResult();
            IList<DeployActivityDefinition> deployActivities = new List<DeployActivityDefinition>();
            IDictionary<string, Guid> deployActivityDict = new Dictionary<string, Guid>();
            int sortIndex = 1;

            WorkflowDefinition workflow = new WorkflowDefinition();
            Guid workflowId = Guid.NewGuid();
            try
            {
                ResourceNameHandler resHandler = null;
                if (resourceTranslator != null)
                    resHandler = new ResourceNameHandler(resourceTranslator.Name2Alias);
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        switch (reader.Name)
                        {
                            case "workflow":
                                workflow = ReadWorkflowDefinition(reader, creator, workflowId);
                                break;
                            case "remark":
                                workflow.Remark = reader.ReadString();
                                break;
                            case "activity":
                            case "start-activity":
                            case "end-activity":
                                DeployActivityDefinition definition = ReadActivityDefinition(reader, workflowId, sortIndex, resHandler);
                                if (definition != null)
                                {
                                    string activityName = definition.ActivityName;
                                    if (!string.IsNullOrEmpty(activityName) && !deployActivityDict.ContainsKey(activityName))
                                    {
                                        deployActivities.Add(definition);
                                        deployActivityDict.Add(activityName, definition.ActivityId);
                                        sortIndex++;
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                        reader.MoveToElement();
                    }
                }

                #region 重新设置:前一活动和下一活动集合

                foreach (DeployActivityDefinition activity in deployActivities)
                {
                    result = ResetDeplyActivitySets(activity.PrevActivitySets, deployActivityDict);
                    if (result.Success == false)
                        return result;

                    result = ResetDeplyActivitySets(activity.NextActivitySets, deployActivityDict);
                    if (result.Success == false)
                        return result;
                }

                #endregion

                this.PreCommitDeployProcess(workflow, deployActivities);

                //流程名称存在重名，则更新重名定义
                WorkflowDefinition laterWorkflow = this.GetCurrentWorkflow(workflow.WorkflowName);
                if (laterWorkflow != null)
                {
                    this.UpdateCurrent(laterWorkflow.WorkflowId);
                }
                result = this.InsertWorkflow(workflow, deployActivities);

                this.PostDeployProcess(laterWorkflow, workflow, deployActivities);
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                result.Success = true;
            }
            finally
            {
                reader.Close();
            }
            result.WorkflowId = workflowId;
            return result;
        }

        /// <summary>
        /// 导出指定流程定义.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        public virtual ActionResult ExportWorkflow(XmlWriter writer, Guid workflowId)
        {
            ActionResult result = new ActionResult();
            WorkflowDefinition workflow = new WorkflowDefinition();
            workflow = workflowDefinitionService.GetWorkflowDefinition(workflowId);
            IList<ActivityDefinition> activities = activityDefinitionService.GetSortedActivitiesByWorkflowId(workflowId);
            IDictionary<Guid, AllocatorOption> assigmentDict = GetAssignmentAllocators(workflowId);

            try
            {
                WriteWorkflow(writer, workflow, activities, assigmentDict);
                result.Success = true;
            }
            catch
            {
                result.Success = false;
                result.Message = "导出失败!";
            }
            return result;
        }

        /// <summary>
        /// 删除指定流程.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        public virtual ActionResult DeleteWorkflow(Guid workflowId)
        {
            ActionResult result = new ActionResult();
            try
            {
                ExecuteDelete(workflowId);
                result.Success = true;
            }
            catch
            {
                result.Success = false;
                result.Message = "删除出错!";
            }
            return result;
        }

        #endregion

        #region 检查部署文档
        /// <summary>
        /// 检查指定 XML 节点字符串中的指定节点名称的节点数.
        /// </summary>
        /// <param name="nodeName">指定节点名称.</param>
        /// <param name="nodeString">指定 XML 字符串.</param>
        /// <returns></returns>
        protected static int CheckNodeCount(string nodeName, string nodeString)
        {
            int count = 0, startIndex = 0;
            while (-1 != (startIndex = nodeString.IndexOf("[" + nodeName + "]", startIndex)))
            {
                startIndex += nodeName.Length;
                count++;
            }
            return count;
        }

        /// <summary>
        /// 检查指定 XmlReader 对象的子节点是否满足格式.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="nodeString"></param>
        /// <returns></returns>
        protected static bool CheckChildNodes(XmlReader reader, ref string nodeString)
        {
            string nodeName = "";
            string activityName = "";
            bool flag = false;
            try
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        nodeName = reader.Name; // 节点名称.
                        nodeString += string.Format("[{0}]", nodeName);// 节点名称集合字符串: [workflow][start-activity][activity][end-activity].

                        if (nodeName == "workflow")
                        {
                            // 如果节点是流程 workflow，必须有属性 name，且值不能为空;
                            string name = reader.GetAttribute("name");
                            // 必须存在流程名称或活动名称属性.
                            if (string.IsNullOrEmpty(name) || name.Length == 0)
                                return false;
                            string owner = reader.GetAttribute("owner");
                            if (owner != null && owner.Length == 0)
                                return false;
                        }
                        else
                        {
                            // start-activity, end-activity, activity 节点的处理
                            if (nodeName.EndsWith("activity"))
                            {
                                string name = reader.GetAttribute("name");
                                // 必须存在流程名称或活动名称属性.
                                if (string.IsNullOrEmpty(name) || name.Trim().Length == 0)
                                    return false;
                                name = name.Trim();
                                // 在一个流程之中的活动名称必须唯一，即检查是否存在多个同名活动名称
                                activityName += string.Format("[{0}]", name);
                                if (CheckNodeCount(name, activityName) > 1)
                                    return false;

                                string prevActivity = reader.GetAttribute("prevActivity");
                                string nextActivity = reader.GetAttribute("nextActivity");

                                if (nodeName == "start-activity")
                                {
                                    // 如果节点是起始活动 start-activity，必须有属性 name 以及下一活动 nextActivity 属性，且都不能为空;
                                    if (string.IsNullOrEmpty(nextActivity))
                                        return false;
                                }
                                else if (nodeName == "end-activity")
                                {
                                    // 如果节点是起始活动 end-activity，必须有属性 name 以及下一活动 prevActivity 属性，且都不能为空;
                                    if (string.IsNullOrEmpty(prevActivity))
                                        return false;
                                }
                                else if (nodeName == "activity")
                                {
                                    // 如果节点是中间活动 activity，必须有属性 name 与下一活动 prevActivity 属性 以及下一活动 nextActivity 属性，且都不为空
                                    if (string.IsNullOrEmpty(prevActivity) || string.IsNullOrEmpty(nextActivity))
                                        return false;
                                }
                            }
                        }
                        flag = true;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                flag = false;
            }
            finally
            {
                reader.Close();
            }
            return flag;
        }

        #endregion

        #region 导入

        /// <summary>
        /// 获取流程定义.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="creator"></param>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        protected virtual WorkflowDefinition ReadWorkflowDefinition(XmlReader reader, string creator, Guid workflowId)
        {
            WorkflowDefinition workflow = new WorkflowDefinition();
            for (int i = 0; i < reader.AttributeCount; i++)
            {
                reader.MoveToAttribute(i);
                switch (reader.Name)
                {
                    case "name":
                        workflow.WorkflowName = reader.Value;
                        break;
                    case "owner":
                        workflow.Owner = reader.Value;
                        break;
                    default:
                        break;
                }
            }
            if (string.IsNullOrEmpty(workflow.Owner))
                workflow.Owner = creator;
            workflow.WorkflowId = workflowId;
            workflow.Creator = creator;
            workflow.LastModifier = creator;
            workflow.IsCurrent = true;
            workflow.Enabled = true;
            workflow.Version = 1;

            //流程名称存在重名，则新增一版本并置为当前版本
            WorkflowDefinition laterWorkflow = GetCurrentWorkflow(workflow.WorkflowName);
            if (laterWorkflow != null)
            {
                workflow.Version = laterWorkflow.Version + 1;
            }
            return workflow;
        }

        /// <summary>
        /// 获取流程活动(步骤)定义.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="workflowId"></param>
        /// <param name="sortOrder"></param>
        /// <param name="resHandler"></param>
        /// <returns></returns>
        protected virtual DeployActivityDefinition ReadActivityDefinition(XmlReader reader, Guid workflowId, int sortOrder, ResourceNameHandler resHandler)
        {
            Guid activityId = Guid.NewGuid();
            DeployActivityDefinition item = new DeployActivityDefinition();
            item.ActivityId = activityId;
            item.WorkflowId = workflowId;
            item.SortOrder = sortOrder;

            string parentName = ToLower(reader.Name);
            int attributeCount = reader.AttributeCount;

            #region 读取属性("activity", "start-activity","end-activity")

            for (int i = 0; i < attributeCount; i++)
            {
                string tempValue = null;
                reader.MoveToAttribute(i);
                switch (reader.Name)
                {
                    case "name":
                        if (reader.Value.Length == 0)
                        {
                            if (parentName == "start-activity")
                                item.ActivityName = "草稿";
                            else if (parentName == "end-activity")
                                item.ActivityName = "完成";
                            else
                                throw new ArgumentException("流程活动名称不能为空.");
                        }
                        else
                        {
                            item.ActivityName = reader.Value;
                        }
                        break;
                    case "prevActivity":
                        string prevActivityNames = reader.Value.Trim();
                        item.PrevActivityNames = prevActivityNames;
                        if (string.IsNullOrEmpty(prevActivityNames))
                        {
                            item.PrevActivitySetId = Guid.Empty;
                        }
                        else
                        {
                            Guid setId = Guid.NewGuid();
                            item.PrevActivitySetId = setId;
                            item.PrevActivitySets = GetDeplyActivitySets(prevActivityNames, setId);
                        }
                        break;
                    case "nextActivity":
                        string nextActivityNames = reader.Value.Trim();
                        item.NextActivityNames = nextActivityNames;
                        if (string.IsNullOrEmpty(nextActivityNames))
                        {
                            item.NextActivitySetId = Guid.Empty;
                        }
                        else
                        {
                            Guid setId = Guid.NewGuid();
                            item.NextActivitySetId = setId;
                            item.NextActivitySets = GetDeplyActivitySets(nextActivityNames, setId);
                        }
                        break;
                    case "joinCondition":
                        tempValue = reader.Value;
                        if (string.IsNullOrEmpty(tempValue))
                            tempValue = tempValue.ToLower();
                        item.JoinCondition = tempValue;
                        break;
                    case "splitCondition":
                        tempValue = reader.Value;
                        if (string.IsNullOrEmpty(tempValue))
                            tempValue = tempValue.ToLower();
                        item.SplitCondition = tempValue;
                        break;
                    case "countersignedCondition":
                        tempValue = reader.Value;
                        if (string.IsNullOrEmpty(tempValue))
                            tempValue = tempValue.ToLower();
                        item.CountersignedCondition = tempValue;
                        break;
                    case "commandRules":
                        item.CommandRules = reader.Value;
                        break;
                    case "executionHandler":
                        item.ExecutionHandler = reader.Value;
                        break;
                    case "postHandler":
                        item.PostHandler = reader.Value;
                        break;
                    case "decisionType":
                        item.DecisionType = reader.Value;
                        break;
                    case "decisionParser":
                        item.DecisionParser = reader.Value;
                        break;
                    case "rejectOption":
                        item.RejectOption = reader.Value;
                        break;
                    default:
                        break;
                }
            }
            #endregion

            // 设置空转交分派设置对象
            AllocatorOption assignmentAllocator = new AllocatorOption();
            assignmentAllocator.ActivityId = activityId;
            item.AssignmentAllocator = assignmentAllocator;

            #region 读取子节点("activity", "start-activity")

            if (parentName == "activity" || parentName == "start-activity")
            {
                reader.MoveToElement();
                using (XmlReader childReader = reader.ReadSubtree())
                {
                    if (childReader != null)
                    {
                        while (childReader.Read())
                        {
                            if (childReader.NodeType == XmlNodeType.Element)
                            {
                                if (childReader.Name == "commandRules")
                                {
                                    // 命令规则
                                    string rules = childReader.ReadString();
                                    rules = rules.Trim(' ', '\r', '\n', '\t'); // 去除空格
                                    item.CommandRules = rules;
                                }
                                else if (childReader.Name == "taskAllocator")
                                {
                                    // 任务的指派
                                    ReadAllocator(reader, item, resHandler);
                                }
                                else if (childReader.Name == "assignmentAllocator")
                                {
                                    // 转交任务的指派
                                    #region assignmentAllocator

                                    assignmentAllocator = new AllocatorOption();
                                    if (ReadAllocator(reader, assignmentAllocator, resHandler)) // assignmentAllocator 不为空时.
                                    {
                                        assignmentAllocator.ActivityId = activityId;
                                        item.AssignmentAllocator = assignmentAllocator;
                                    }
                                    #endregion
                                }
                                childReader.MoveToElement();
                            }
                        }
                    }
                }
            }

            #endregion

            if (parentName == "start-activity")
                item.State = 0;
            else if (parentName == "end-activity")
                item.State = 2;
            else
                item.State = 1;

            return item;
        }

        /// <summary>
        /// 获取任务分派信息定义.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="output"></param>
        /// <param name="resHandler"></param>
        /// <returns></returns>
        protected static bool ReadAllocator(XmlReader reader, AllocatorOption output, ResourceNameHandler resHandler)
        {
            int attributeCount = reader.AttributeCount;
            if (attributeCount == 0)
                return false;
            for (int i = 0; i < attributeCount; i++)
            {
                string tempValue = string.Empty;
                reader.MoveToAttribute(i);
                switch (reader.Name)
                {
                    case "resource":
                        tempValue = reader.Value;
                        if (!string.IsNullOrEmpty(tempValue) && resHandler != null)
                        {
                            tempValue = resHandler.Invoke(tempValue);
                            if (string.IsNullOrEmpty(tempValue))
                                tempValue = reader.Value;
                        }
                        output.AllocatorResource = ToUpper(tempValue);
                        break;
                    case "users":
                        output.AllocatorUsers = ToLower(reader.Value);
                        break;
                    case "extAllocators":
                        output.ExtendAllocators = ToLower(reader.Value);
                        break;
                    case "extAllocatorArgs":
                        output.ExtendAllocatorArgs = ToLower(reader.Value);
                        break;
                    case "default":
                        string defaultAllocator = ToLower(reader.Value);
                        if (!string.IsNullOrEmpty(defaultAllocator) && defaultAllocator.IndexOf(':') > -1)
                        {
                            defaultAllocator = defaultAllocator.Remove(defaultAllocator.IndexOf(':'));
                        }
                        output.DefaultAllocator = defaultAllocator;
                        break;
                    default:
                        break;
                }
            }
            return true;
        }

        /// <summary>
        /// 获取指定的流程活动(步骤)集合列表.
        /// </summary>
        /// <param name="activitySetNames"></param>
        /// <param name="activitySetId"></param>
        /// <returns></returns>
        protected static IList<DeployActivitySet> GetDeplyActivitySets(string activitySetNames, Guid activitySetId)
        {
            IList<DeployActivitySet> deployActivitySets = new List<DeployActivitySet>();
            if (string.IsNullOrEmpty(activitySetNames))
                return deployActivitySets;

            string[] nameArray = activitySetNames.Split(new string[] { ",", "，" }, StringSplitOptions.RemoveEmptyEntries);
            // 集合流程活动名称字典.
            IDictionary<string, string> deployActivityNames = new Dictionary<string, string>();
            foreach (string name in nameArray)
            {
                string tempActivityName = name;
                tempActivityName = tempActivityName.Trim();
                if (string.IsNullOrEmpty(tempActivityName))
                    continue;
                if (!deployActivityNames.ContainsKey(tempActivityName))
                {
                    deployActivityNames.Add(tempActivityName, null); // 添加到流程集合流程活动名称字典中.

                    DeployActivitySet deploySet = new DeployActivitySet();
                    deploySet.SetId = activitySetId;
                    deploySet.ActivityName = tempActivityName;
                    deployActivitySets.Add(deploySet);
                }
            }
            return deployActivitySets;
        }

        /// <summary>
        /// 重新设置活动集合的活动 ID.
        /// </summary>
        /// <param name="activitySets"></param>
        /// <param name="deployActivityDict"></param>
        /// <returns></returns>
        protected static DeployActionResult ResetDeplyActivitySets(IList<DeployActivitySet> activitySets, IDictionary<string, Guid> deployActivityDict)
        {
            DeployActionResult result = new DeployActionResult();
            foreach (DeployActivitySet deploySet in activitySets)
            {
                string activityName = deploySet.ActivityName;
                if (deployActivityDict.ContainsKey(activityName))
                {
                    deploySet.ActivityId = deployActivityDict[activityName];
                }
                else
                {
                    //  未知的上一个属性定义.
                    result.Message = string.Format("有未知的上一个流程活动名称[{0}].", activityName);
                    result.Success = false;
                    return result;
                }
            }
            result.Message = "重新设置成功!";
            result.Success = true;
            return result;
        }

        #endregion

        #region 导出

        /// <summary>
        /// 输出流程.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="item"></param>
        /// <param name="activities"></param>
        /// <param name="assignmentAllocators"></param>
        protected virtual void WriteWorkflow(XmlWriter writer, WorkflowDefinition item, IList<ActivityDefinition> activities, IDictionary<Guid, AllocatorOption> assignmentAllocators)
        {
            writer.WriteStartDocument();
            writer.WriteComment(FlowComment);

            // 输出流程节点.
            writer.WriteStartElement("workflow");
            writer.WriteAttributeString("name", item.WorkflowName);
            writer.WriteAttributeString("owner", item.Owner);

            writer.WriteStartElement("remark");
            writer.WriteCData(item.Remark);
            writer.WriteEndElement();

            // 权限资源处理器.
            ResourceAliasHandler resHandler = null;
            if (resourceTranslator != null)
                resHandler = new ResourceAliasHandler(resourceTranslator.Name2Alias);
            // 流程步骤名称字典.
            IDictionary<Guid, string> dictActivityName = ToAcitvityNameDictionary(activities);

            // 输出流程步骤(活动).
            foreach (ActivityDefinition activity in activities)
            {
                Guid activityId = activity.ActivityId;
                int state = activity.State;
                string prevActivityString = string.Empty;
                string nextActivityString = string.Empty;
                if (state == 0)
                {
                    nextActivityString = GetActivityNames(dictActivityName, activity.NextActivitySetId);
                    WriteStartActivity(writer, activity, nextActivityString);
                }
                else if (state == 1)
                {
                    prevActivityString = GetActivityNames(dictActivityName, activity.PrevActivitySetId);
                    nextActivityString = GetActivityNames(dictActivityName, activity.NextActivitySetId);
                    WriteActivity(writer, activity, assignmentAllocators, resHandler, prevActivityString, nextActivityString);
                }
                else if (state == 2)
                {
                    prevActivityString = GetActivityNames(dictActivityName, activity.PrevActivitySetId);
                    WriteEndActivity(writer, activity, prevActivityString);
                }
                else
                {
                    // 未知状态.
                }
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
        }

        /// <summary>
        /// 输出流促步骤(活动).
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="item"></param>
        /// <param name="assignmentAllocators"></param>
        /// <param name="resHandler"></param>
        /// <param name="prevActivityString"></param>
        /// <param name="nextActivityString"></param>
        protected virtual void WriteActivity(XmlWriter writer, ActivityDefinition item, IDictionary<Guid, AllocatorOption> assignmentAllocators, ResourceAliasHandler resHandler, string prevActivityString, string nextActivityString)
        {
            Guid activityId = item.ActivityId;

            writer.WriteStartElement("activity", "");
            writer.WriteAttributeString("name", item.ActivityName);
            writer.WriteAttributeString("prevActivity", prevActivityString);
            writer.WriteAttributeString("nextActivity", nextActivityString);
            writer.WriteAttributeString("joinCondition", item.JoinCondition);
            writer.WriteAttributeString("splitCondition", item.SplitCondition);
            writer.WriteAttributeString("countersignedCondition", item.CountersignedCondition);
            writer.WriteAttributeString("executionHandler", item.ExecutionHandler);
            writer.WriteAttributeString("postHandler", item.PostHandler);
            writer.WriteAttributeString("decisionType", item.DecisionType);
            writer.WriteAttributeString("decisionParser", item.DecisionParser);
            writer.WriteAttributeString("rejectOption", item.RejectOption);

            writer.WriteStartElement("commandRules");
            writer.WriteCData(item.CommandRules);
            writer.WriteEndElement();

            // taskAllocator
            WriteAllocator(writer, item, "taskAllocator", resHandler);

            // assignmentAllocator
            if (assignmentAllocators.ContainsKey(activityId))
            {
                WriteAllocator(writer, assignmentAllocators[activityId], "assignmentAllocator", resHandler);
            }
            else
            {
                WriteAllocator(writer, "assignmentAllocator");
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// 输出流程的起始步骤(活动).
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="item"></param>
        /// <param name="nextActivityString"></param>
        protected virtual void WriteStartActivity(XmlWriter writer, ActivityDefinition item, string nextActivityString)
        {
            writer.WriteStartElement("start-activity", "");
            writer.WriteAttributeString("name", item.ActivityName);
            writer.WriteAttributeString("nextActivity", nextActivityString);
            writer.WriteAttributeString("splitCondition", item.SplitCondition);
            writer.WriteAttributeString("executionHandler", item.ExecutionHandler);
            writer.WriteAttributeString("postHandler", item.PostHandler);
            writer.WriteAttributeString("decisionType", item.DecisionType);
            writer.WriteAttributeString("decisionParser", item.DecisionParser);

            writer.WriteStartElement("commandRules");
            writer.WriteCData(item.CommandRules);
            writer.WriteEndElement();

            writer.WriteEndElement();
        }

        /// <summary>
        /// 输出流程的结束步骤(活动).
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="item"></param>
        /// <param name="prevActivityString"></param>
        protected virtual void WriteEndActivity(XmlWriter writer, ActivityDefinition item, string prevActivityString)
        {
            writer.WriteStartElement("end-activity", "");
            writer.WriteAttributeString("name", item.ActivityName);
            writer.WriteAttributeString("prevActivity", prevActivityString);
            writer.WriteEndElement();
        }

        /// <summary>
        /// 导出空的任务分派器到 XML 输出流中.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="nodeName"></param>
        protected virtual void WriteAllocator(XmlWriter writer, string nodeName)
        {
            writer.WriteStartElement(nodeName);
            writer.WriteAttributeString("resource", "");
            writer.WriteAttributeString("users", "");
            writer.WriteAttributeString("extAllocators", "");
            writer.WriteAttributeString("extAllocatorArgs", "");
            writer.WriteAttributeString("default", "");
            writer.WriteEndElement();
        }

        /// <summary>
        /// 导出任务分派器到 XML 输出流中.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="item"></param>
        /// <param name="nodeName"></param>
        /// <param name="resHandler"></param>
        protected virtual void WriteAllocator(XmlWriter writer, AllocatorOption item, string nodeName, ResourceAliasHandler resHandler)
        {
            writer.WriteStartElement(nodeName);
            string resource = item.AllocatorResource;
            if (!string.IsNullOrEmpty(resource))
            {
                if (resource.StartsWith(PrefixDisableResource, StringComparison.OrdinalIgnoreCase))
                {
                    // 不启用权限控制.
                    resource = "";
                }
                else if (resHandler != null)
                {
                    resource = resHandler.Invoke(resource);
                    if (string.IsNullOrEmpty(resource))
                        resource = item.AllocatorResource;
                }
            }
            writer.WriteAttributeString("resource", ToUpper(resource));
            writer.WriteAttributeString("users", ToLower(item.AllocatorUsers));
            writer.WriteAttributeString("extAllocators", ToLower(item.ExtendAllocators));
            writer.WriteAttributeString("extAllocatorArgs", ToLower(item.ExtendAllocatorArgs));
            writer.WriteAttributeString("default", ToLower(item.DefaultAllocator));
            writer.WriteEndElement();
        }

        /// <summary>
        /// 获取活动名称字符串（以","隔开）.
        /// </summary>
        /// <param name="dictActivityName"></param>
        /// <param name="activitySetId"></param>
        /// <returns></returns>
        protected string GetActivityNames(IDictionary<Guid, string> dictActivityName, Guid activitySetId)
        {
            if (activitySetId == Guid.Empty)
                return string.Empty;

            IList<Guid> activityIds = this.GetAcitivtyIdsBySetId(activitySetId);
            if (activityIds == null || activityIds.Count == 0)
                return string.Empty;

            return GetActivityNames(dictActivityName, activityIds);
        }

        /// <summary>
        /// 获取活动名称字符串（以","隔开）.
        /// </summary>
        /// <param name="dictActivityName"></param>
        /// <param name="activityIds"></param>
        /// <returns></returns>
        protected static string GetActivityNames(IDictionary<Guid, string> dictActivityName, IList<Guid> activityIds)
        {
            StringBuilder nameBuilder = new StringBuilder();
            foreach (Guid activityId in activityIds)
            {
                if (dictActivityName.ContainsKey(activityId))
                {
                    nameBuilder.AppendFormat(",{0}", dictActivityName[activityId]);
                }
            }
            if (nameBuilder.Length > 1)
                nameBuilder.Remove(0, 1);
            return nameBuilder.ToString();
        }

        #endregion

        #region 其他辅助方法

        /// <summary>
        /// 将指定流程步骤定义列表转换为相应的流程名称字典(key : ActivityId, value: ActivityName).
        /// </summary>
        /// <param name="activities"></param>
        /// <returns></returns>
        protected static IDictionary<Guid, string> ToAcitvityNameDictionary(IList<ActivityDefinition> activities)
        {
            IDictionary<Guid, string> dict = new Dictionary<Guid, string>();
            if (activities == null || activities.Count == 0)
                return dict;
            foreach (ActivityDefinition item in activities)
            {
                if (!dict.ContainsKey(item.ActivityId))
                    dict.Add(item.ActivityId, item.ActivityName);
            }
            return dict;
        }

        /// <summary>
        /// 获取小写字符串.
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        protected static string ToLower(string inputString)
        {
            if (!string.IsNullOrEmpty(inputString))
                inputString = inputString.ToLower();
            return inputString;
        }

        /// <summary>
        /// 获取大写字符串.
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        protected static string ToUpper(string inputString)
        {
            if (!string.IsNullOrEmpty(inputString))
                inputString = inputString.ToUpper();
            return inputString;
        }

        /// <summary>
        /// 清除字符串首尾的特殊空白字符.
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        protected static string TrimWhitespace(string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
                return string.Empty;
            return inputString.Trim('\r', '\n', '\t', ' ');  // 去除空白
        }

        #endregion

        #region 流程部署前续处理

        /// <summary>
        /// 流程部署的前续处理，即提交流程数据到数据库前的处理.
        /// </summary>
        /// <param name="newWorkflow"></param>
        /// <param name="newActivities"></param>
        protected virtual void PreCommitDeployProcess(WorkflowDefinition newWorkflow, ICollection<DeployActivityDefinition> newActivities)
        {
            if (null != preCommitDeployHandler)
            {
                preCommitDeployHandler.Execute(newWorkflow, newActivities);
                IPreCommitDeployHandler next = preCommitDeployHandler.Next;
                while (null != next)
                {
                    next.Execute(newWorkflow, newActivities);
                    next = next.Next;
                }
            }
        }

        #endregion

        #region 流程配置后续处理：关联表单与流程定义

        /// <summary>
        /// 流程配置后续处理：关联表单与流程定义.
        /// </summary>
        /// <param name="oldWorkflow">原流程定义对象.</param>
        /// <param name="newWorkflow">新流程定义对象.</param>
        /// <param name="newActivities">新流程活动列表.</param>
        protected virtual void PostDeployProcess(WorkflowDefinition oldWorkflow, WorkflowDefinition newWorkflow, ICollection<DeployActivityDefinition> newActivities)
        {
            if (null != postDeployHandler)
            {
                postDeployHandler.Execute(oldWorkflow, newWorkflow, newActivities);
                IPostDeployHandler next = postDeployHandler.Next;
                while (null != next)
                {
                    next.Execute(oldWorkflow, newWorkflow, newActivities);
                    next = next.Next;
                }
            }
        }
        #endregion

        #region 抽象方法.

        /// <summary>
        /// 执行删除流程.
        /// </summary>
        /// <param name="workflowId">流程定义编号.</param>
        /// <returns></returns>
        protected abstract int ExecuteDelete(Guid workflowId);

        /// <summary>
        /// 获取指定流程编号的各流程步骤转交的任务分派设置字典(key:ActivityId).
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        protected abstract IDictionary<Guid, AllocatorOption> GetAssignmentAllocators(Guid workflowId);

        /// <summary>
        /// 获取指定的步骤集合编号的流程步骤编号列表.
        /// </summary>
        /// <param name="activitySetId"></param>
        /// <returns></returns>
        protected abstract IList<Guid> GetAcitivtyIdsBySetId(Guid activitySetId);

        /// <summary>
        /// 获取指定流程名称的当前版本信息.
        /// </summary>
        /// <param name="workflowName"></param>
        /// <returns></returns>
        protected abstract WorkflowDefinition GetCurrentWorkflow(string workflowName);

        /// <summary>
        /// 更新当前版本.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        protected abstract int UpdateCurrent(Guid workflowId);

        /// <summary>
        /// 插入流程数据以及流程步骤列表数据到数据库.
        /// </summary>
        /// <param name="workflow"></param>
        /// <param name="activities"></param>
        /// <returns></returns>
        protected abstract DeployActionResult InsertWorkflow(WorkflowDefinition workflow, IList<DeployActivityDefinition> activities);

        #endregion

        #region 委托

        /// <summary>
        /// 获取权限资源别名的资源处理器委托.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected delegate string ResourceAliasHandler(string id);

        /// <summary>
        /// 获取权限资源名称的资源处理器委托.
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        protected delegate string ResourceNameHandler(string alias);

        #endregion

        #region XML 架构

        /// <summary>
        /// 部署 XML 架构类.
        /// </summary>
        protected static class DeploySchmas
        {
            /// <summary>
            /// Workflow 元素名称.
            /// </summary>
            public const string Element_Workflow = "workflow";
        }

        #endregion
    }
}
