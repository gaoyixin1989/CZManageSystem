using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Botwave.Commons;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow.Domain;
using Botwave.XQP.Domain;

namespace Botwave.XQP.Designer
{
    /// <summary>
    /// 流程活动对象.
    /// </summary>
    [Serializable]
    //public class WorkflowActivity : Botwave.Workflow.Domain.ActivityDefinition
    public class WorkflowActivity : CZActivityDefinition
    {
        private int _x  = 0;
        private int _y = 0;
        private bool _selected = false;
        private string _prevActivity;
        private string _nextActivity;

        /// <summary>
        /// 在设计画布上的 X 坐标位置.
        /// </summary>
        public int X
        {
            get { return _x; }
            set { _x = value; }
        }

        /// <summary>
        /// 在设计画布上的 Y 坐标位置.
        /// </summary>
        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }

        /// <summary>
        /// 在设计画布上是否被选中.
        /// </summary>
        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; }
        }

        public string PrevActivity
        {
            get { return _prevActivity; }
            set { _prevActivity = value; }
        }

        public string NextActivity
        {
            get { return _nextActivity; }
            set { _nextActivity = value; }
        }

        public void LoadData(XmlNode activityNode)
        {
            foreach (XmlAttribute attribute in activityNode.Attributes)
            {
                string attrName = attribute.Name;
                string attrValue = attribute.Value;
                switch (attrName)
                {
                    case "name":
                        this.ActivityName = DbUtils.ToString(attrValue);
                        break;
                    case "x":
                        this._x = DbUtils.ToInt32(attrValue);
                        break;
                    case "y":
                        this._y = DbUtils.ToInt32(attrValue);
                        break;
                    case "selected":
                        this._selected = (string.IsNullOrEmpty(attrValue) || attrValue != "true" ? false : true);
                        break;
                    default:
                        break;
                }
            }
        }

        public XmlNode CreateNode(XmlDocument doc)
        {
            int state = this.State;
            XmlElement activity = null;
            XmlAttribute attribute = null;
            XmlElement childNode = null;

            if (state == 0)
            {
                activity = doc.CreateElement("start-activity");
            }
            else if (state == 2)
            {
                activity = doc.CreateElement("end-activity");
            }
            else if(state == 1)
            {
                activity = doc.CreateElement("activity");
            }

            if (activity == null)
                return null;

            attribute = doc.CreateAttribute("name");
            attribute.Value = this.ActivityName;
            activity.Attributes.Append(attribute);

            if (!string.IsNullOrEmpty(_prevActivity))
            {
                attribute = doc.CreateAttribute("prevActivity");
                attribute.Value = this._prevActivity;
                activity.Attributes.Append(attribute);
            }
            if (!string.IsNullOrEmpty(_nextActivity))
            {
                attribute = doc.CreateAttribute("nextActivity");
                attribute.Value = this._nextActivity;
                activity.Attributes.Append(attribute);
            }
            if (!string.IsNullOrEmpty(this.SplitCondition))
            {
                attribute = doc.CreateAttribute("splitCondition");
                attribute.Value = this.SplitCondition;
                activity.Attributes.Append(attribute);
            }
            if (!string.IsNullOrEmpty(this.JoinCondition))
            {
                attribute = doc.CreateAttribute("joinCondition");
                attribute.Value = this.SplitCondition;
                activity.Attributes.Append(attribute);
            }
            if (!string.IsNullOrEmpty(this.CountersignedCondition))
            {
                attribute = doc.CreateAttribute("countersignedCondition");
                attribute.Value = this.CountersignedCondition;
                activity.Attributes.Append(attribute);
            }
            if (!string.IsNullOrEmpty(this.ExecutionHandler))
            {
                attribute = doc.CreateAttribute("executionHandler");
                attribute.Value = this.ExecutionHandler;
                activity.Attributes.Append(attribute);
            }
            if (!string.IsNullOrEmpty(this.PostHandler))
            {
                attribute = doc.CreateAttribute("postHandler");
                attribute.Value = this.PostHandler;
                activity.Attributes.Append(attribute);
            }
            if (!string.IsNullOrEmpty(this.DecisionType))
            {
                attribute = doc.CreateAttribute("decisionType");
                attribute.Value = this.DecisionType;
                activity.Attributes.Append(attribute);
            }
            if (!string.IsNullOrEmpty(this.DecisionParser))
            {
                attribute = doc.CreateAttribute("decisionParser");
                attribute.Value = this.DecisionParser;
                activity.Attributes.Append(attribute);
            }
            if (!string.IsNullOrEmpty(this.RejectOption))
            {
                attribute = doc.CreateAttribute("rejectOption");
                attribute.Value = this.RejectOption;
                activity.Attributes.Append(attribute);
            }

            attribute = doc.CreateAttribute("x");
            attribute.Value = this._x.ToString();
            activity.Attributes.Append(attribute);

            attribute = doc.CreateAttribute("y");
            attribute.Value = this._y.ToString();
            activity.Attributes.Append(attribute);

            if (this._selected)
            {
                attribute = doc.CreateAttribute("selected");
                attribute.Value = "true";
                activity.Attributes.Append(attribute);
            }

            if (!string.IsNullOrEmpty(this.CommandRules))
            {
                childNode = doc.CreateElement("commandRules");
                childNode.AppendChild(doc.CreateCDataSection(this.CommandRules));
                activity.AppendChild(childNode);
            }

            if (!IsEmptyAllocator(this))
            {
                childNode = CreateAllocatorNode(doc, this, "taskAllocator");
                activity.AppendChild(childNode);
            }

            return activity;
        }

        /// <summary>
        /// 任务分派节点.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="value"></param>
        /// <param name="elementName">taskAllocator/assignmentAllocator.</param>
        /// <returns></returns>
        public static XmlElement CreateAllocatorNode(XmlDocument doc, AllocatorOption value, string elementName)
        {
            XmlElement allocator = doc.CreateElement(elementName);
            XmlAttribute attribute  = null;

            attribute = doc.CreateAttribute("resource");
            attribute.Value = value.AllocatorResource;
            allocator.Attributes.Append(attribute);

            attribute = doc.CreateAttribute("users");
            attribute.Value = value.AllocatorUsers;
            allocator.Attributes.Append(attribute);

            attribute = doc.CreateAttribute("extAllocators");
            attribute.Value = value.ExtendAllocators;
            allocator.Attributes.Append(attribute);

            attribute = doc.CreateAttribute("extAllocatorArgs");
            attribute.Value = value.ExtendAllocatorArgs;
            allocator.Attributes.Append(attribute);

            attribute = doc.CreateAttribute("default");
            attribute.Value = value.DefaultAllocator;
            allocator.Attributes.Append(attribute);

            return allocator;
        }

        /// <summary>
        /// 是否属于空对象.
        /// </summary>
        /// <param name="allocator"></param>
        /// <returns></returns>
        public static bool IsEmptyAllocator(AllocatorOption allocator)
        {
            return (allocator == null || (string.IsNullOrEmpty(allocator.AllocatorResource)
                && string.IsNullOrEmpty(allocator.AllocatorUsers)
                && string.IsNullOrEmpty(allocator.ExtendAllocators)
                && string.IsNullOrEmpty(allocator.ExtendAllocatorArgs)));
        }

        /// <summary>
        /// 更新坐标位置.
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static void UpdatePosition(Guid activityId, int x, int y)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("ActivityId", activityId);
            parameters.Add("X", x);
            parameters.Add("Y", y);

            IBatisMapper.Insert("bwwf_Designer_Activities_Insert", parameters);
        }

        public static IList<WorkflowActivity> GetActivities(Guid workflowId)
        {
            return IBatisMapper.Select<WorkflowActivity>("bwwf_Designer_Activities_Select_By_WorkflowId", workflowId);
        }

        public static IDictionary<Guid, AllocatorOption> GetAssignments(Guid workflowId)
        {
            return IBatisMapper.Mapper.QueryForDictionary<Guid, AllocatorOption>("bwwf_Designer_Assignments_Select_By_WorkflowId", workflowId, "ActivityId");
        }
    }
}
