using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Xml;
using System.Web;
using Botwave.Workflow.Extension.WorkflowMap;

namespace Botwave.Workflow.Extension.Configuration
{
    /// <summary>
    /// WorkflowMap 配置节点处理器.
    /// </summary>
    public class WorkflowMapSectionHandler : ConfigurationElement, IConfigurationSectionHandler
    {
        private static HttpContext context;

        #region IConfigurationSectionHandler 成员

        /// <summary>
        /// IConfigurationSectionHandler 的创建配置方法.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="configContext"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public object Create(object parent, object configContext, XmlNode section)
        {
            InitMaps(section);
            return section;
        }

        #endregion

        /// <summary>
        /// 初始化配置节点.
        /// </summary>
        public static void Initialize()
        {
            context = HttpContext.Current;
            object section = ConfigurationManager.GetSection(WorkflowMapSchema.Section_WorkflowMap);
            if(section == null)
                section = ConfigurationManager.GetSection("botwave/workflowMap");
            if (section == null)
                section = ConfigurationManager.GetSection("workflowMap");
        }

        #region private methods

        /// <summary>
        /// 初始化图象数据.
        /// </summary>
        /// <param name="section"></param>
        private static void InitMaps(XmlNode section)
        {
            if (section == null)
                return;
            XmlAttributeCollection attributes = section.Attributes;

            #region section attributes

            string attributeValue = string.Empty;
            if (attributes.Count > 0)
            {
                foreach (XmlAttribute attr in attributes)
                {
                    switch (attr.Name)
                    { 
                        case WorkflowMapSchema.Attribute_Alpha:
                            attributeValue = attr.Value.Trim();
                            if (!string.IsNullOrEmpty(attributeValue))
                                WorkflowMapManager.ColorAlpha = ToInt32(attributeValue);
                            break;
                        case WorkflowMapSchema.Attribute_SelectedColor:
                            attributeValue = attr.Value.Trim();
                            if (!string.IsNullOrEmpty(attributeValue))
                                WorkflowMapManager.ColorName = attributeValue;
                            break;
                        case WorkflowMapSchema.Attribute_CachePath:
                            attributeValue = attr.Value.Trim();
                            if (!string.IsNullOrEmpty(attributeValue))
                            {
                                WorkflowMapManager.ImageCachePath = attributeValue;
                            }
                            break;
                        default: 
                            break;
                    }
                }
            }
            #endregion

            #region workflows child nodes
            foreach (XmlNode workflowNode in section.ChildNodes)
            {
                if (workflowNode.Name != WorkflowMapSchema.Element_Workflow)
                    continue;
                WorkflowMapManager.WorkflowItem workflow = GetWorkflow(workflowNode);
                int activityIndex = 0;
                foreach (XmlNode activityNode in workflowNode.ChildNodes)
                {
                    if (activityNode.Name != WorkflowMapSchema.Element_Activity)
                        continue;

                    WorkflowMapManager.ActivityItem activity = GetActivity(activityNode);
                    string activityName = activity.Name;
                    if (!workflow.Activities.ContainsKey(activityName))
                    {
                        activity.Index = activityIndex;
                        workflow.Activities.Add(activityName, activity);
                        activityIndex++;
                    }
                }
                string workflowName = workflow.Name;
                if (!WorkflowMapManager.Maps.ContainsKey(workflowName))
                {
                    WorkflowMapManager.Maps.Add(workflowName, workflow);
                }
            }
            #endregion
        }

        /// <summary>
        /// 获取 WorkflowItem 对象.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private static WorkflowMapManager.WorkflowItem GetWorkflow(XmlNode node)
        {
            if (node == null)
                return null;
            XmlAttributeCollection attributes = node.Attributes;
            if (attributes.Count < 2 || attributes[WorkflowMapSchema.Attribute_Name] == null
                || attributes[WorkflowMapSchema.Attribute_Path] == null)
                return null;
            string workflowName = string.Empty;
            string path = string.Empty;
            foreach (XmlAttribute attr in attributes)
            {
                string attributeName = attr.Name;
                switch (attributeName)
                {
                    case WorkflowMapSchema.Attribute_Name:
                        workflowName = attr.Value.Trim();
                        break;
                    case WorkflowMapSchema.Attribute_Path:
                        path = attr.Value.Trim();
                        if (!string.IsNullOrEmpty(path))
                        {
                            path = context.Server.MapPath(path);
                        }
                        break;
                    default:
                        break;
                }
            }

            if (string.IsNullOrEmpty(workflowName) || string.IsNullOrEmpty(path))
                return null;

            workflowName = workflowName.ToUpper();
            WorkflowMapManager.WorkflowItem item = new WorkflowMapManager.WorkflowItem();
            item.Name = workflowName;
            item.Path = path;

            return item;
        }

        /// <summary>
        /// 获取 ActivityItem 对象.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private static WorkflowMapManager.ActivityItem GetActivity(XmlNode node)
        {
            if (node == null)
                return null;
            XmlAttributeCollection attributes = node.Attributes;
            if (attributes.Count < 5
                || attributes[WorkflowMapSchema.Attribute_Name] == null
                || attributes[WorkflowMapSchema.Attribute_Width] == null
                || attributes[WorkflowMapSchema.Attribute_Height] == null
                || attributes[WorkflowMapSchema.Attribute_X] == null
                || attributes[WorkflowMapSchema.Attribute_Y] == null)
                return null;

            WorkflowMapManager.ActivityItem item = new WorkflowMapManager.ActivityItem();
            string activityName = string.Empty;
            foreach (XmlAttribute attr in attributes)
            {
                string attributeName = attr.Name;
                switch (attributeName)
                {
                    case WorkflowMapSchema.Attribute_Name:
                        activityName = attr.Value.Trim();
                        break;
                    case WorkflowMapSchema.Attribute_X:
                        item.X = ToInt32(attr.Value);
                        break;
                    case WorkflowMapSchema.Attribute_Y:
                        item.Y = ToInt32(attr.Value);
                        break;
                    case WorkflowMapSchema.Attribute_Width:
                        item.Width = ToInt32(attr.Value);
                        break;
                    case WorkflowMapSchema.Attribute_Height:
                        item.Height = ToInt32(attr.Value);
                        break;
                    default:
                        break;
                }
            }

            if (string.IsNullOrEmpty(activityName))
                return null;

            activityName = activityName.ToUpper();
            item.Name = activityName;
            return item;
        }

        /// <summary>
        /// 将字符串型转化为整型.
        /// </summary>
        /// <param name="inputValue"></param>
        /// <returns></returns>
        private static int ToInt32(string inputValue)
        {
            if (string.IsNullOrEmpty(inputValue))
                return 0;
            try
            {
                return int.Parse(inputValue.Trim());
            }
            catch
            {
                return 0;
            }
        }

        #endregion

        #region WorkflowMap Schema Class

        /// <summary>
        /// WorkflowMap 架构类.
        /// </summary>
        public static class WorkflowMapSchema
        {
            /// <summary>
            /// WorkflowMap 配置节点名.
            /// </summary>
            public const string Section_WorkflowMap = "botwave.workflowMap";
            /// <summary>
            /// 流程节点名称.
            /// </summary>
            public const string Element_Workflow = "workflow";
            /// <summary>
            /// 流程步骤节点名称.
            /// </summary>
            public const string Element_Activity = "activity";
            /// <summary>
            /// 透明度属性.
            /// </summary>
            public const string Attribute_Alpha = "alpha";
            /// <summary>
            /// 被选中的颜色属性.
            /// </summary>
            public const string Attribute_SelectedColor = "selectedColor";
            /// <summary>
            /// 缓存路径属性.
            /// </summary>
            public const string Attribute_CachePath = "cachePath";
            /// <summary>
            /// 名称属性.
            /// </summary>
            public const string Attribute_Name = "name";
            /// <summary>
            /// 路径属性.
            /// </summary>
            public const string Attribute_Path = "path";
            /// <summary>
            /// 宽度属性.
            /// </summary>
            public const string Attribute_Width = "width";
            /// <summary>
            /// 高度属性.
            /// </summary>
            public const string Attribute_Height = "height";
            /// <summary>
            /// 起始坐标 X 位置属性.
            /// </summary>
            public const string Attribute_X = "x";
            /// <summary>
            /// 起始坐标 Y 位置属性.
            /// </summary>
            public const string Attribute_Y = "y";
        }

        #endregion
    }
}
