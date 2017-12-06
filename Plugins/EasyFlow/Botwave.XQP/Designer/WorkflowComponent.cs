using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow.Domain;
using Botwave.Commons;

namespace Botwave.XQP.Designer
{
    /// <summary>
    /// 流程设计组件对象.
    /// </summary>
    [Serializable]
    public class WorkflowComponent : Botwave.XQP.Domain.WorkflowProfile
    {
        private log4net.ILog log = log4net.LogManager.GetLogger(typeof(WorkflowComponent));

        #region properties

        private string _owner = "admin";
        private string _workflowAlias;
        private string _aliasImage;
        private string _remark;

        public string Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }

        public string WorkflowAlias
        {
            get { return _workflowAlias; }
            set { _workflowAlias = value; }
        }

        public string AliasImage
        {
            get
            {
                if (string.IsNullOrEmpty(_aliasImage) && !string.IsNullOrEmpty(_workflowAlias))
                    _aliasImage = string.Format("flow_{0}.gif", _workflowAlias.ToLower());
                return _aliasImage;
            }
            set { _aliasImage = value; }
        }

        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        #endregion

        /// <summary>
        /// 加载流程数据.
        /// </summary>
        /// <param name="workflowNode"></param>
        public void LoadWorkflowData(XmlNode workflowNode)
        {
            foreach (XmlAttribute attribute in workflowNode.Attributes)
            {
                string attrName = attribute.Name;
                string attrValue = attribute.Value;
                switch (attrName)
                {
                    case "name":
                        this.WorkflowName = attrValue;
                        break;
                    case "alias":
                        this.WorkflowAlias = attrValue;
                        break;
                    case "basicFields":
                        this.BasicFields = attrValue;
                        break;
                    case "printAndExp":
                        this.PrintAndExp = DbUtils.ToInt32(attrValue);
                        break;
                    case "printAmount":
                        this.PrintAmount = DbUtils.ToInt32(attrValue);
                        break;
                    case "isMobile":
                        this.IsMobile = DbUtils.ToBoolean(attrValue);
                        break;
                    case "depts":
                        this.Depts = attrValue;
                        break;
                    case "manager":
                        this.Manager = attrValue;
                        break;
                    case "workflowInstanceTitle":
                        this.WorkflowInstanceTitle = attrValue;
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 加载发单控制数据.
        /// </summary>
        /// <param name="creationControl"></param>
        public void LoadCreationData(XmlNode creationControl)
        {
            foreach (XmlAttribute attribute in creationControl.Attributes)
            {
                string attrName = attribute.Name;
                string attrValue = attribute.Value;
                switch(attrName)
                {
                    case "type":
                        this.CreationControlType = ToCreationControlType(attrValue);
                        break;
                    case "minNotifyTaskCount":
                        this.MinNotifyTaskCount = ToInt32(attrValue);
                        break;
                    case "maxCreationInMonth":
                        this.MaxCreationInMonth = ToInt32(attrValue);
                        break;
                    case "maxCreationInWeek":
                        this.MaxCreationInWeek = ToInt32(attrValue);
                        break;
                    case "maxCreationUndone":
                        this.MaxCreationUndone = ToInt32(attrValue);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 创建 XML 节点对象.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public XmlNode CreateNode(XmlDocument doc)
        {
            XmlElement workflow = doc.CreateElement("workflow");
            XmlAttribute attribute = null;
            XmlElement childNode = null;

            attribute = doc.CreateAttribute("name");
            attribute.Value = this.WorkflowName;
            workflow.Attributes.Append(attribute);

            attribute = doc.CreateAttribute("owner");
            attribute.Value = this._owner;
            workflow.Attributes.Append(attribute);

            if (!string.IsNullOrEmpty(this._workflowAlias))
            {
                attribute = doc.CreateAttribute("alias");
                attribute.Value = this._workflowAlias;
                workflow.Attributes.Append(attribute);
            }

            if (!string.IsNullOrEmpty(this.BasicFields))
            {
                attribute = doc.CreateAttribute("basicFields");
                attribute.Value = this.BasicFields;
                workflow.Attributes.Append(attribute);
            }

            attribute = doc.CreateAttribute("printAndExp");
            attribute.Value = this.PrintAndExp.ToString();
            workflow.Attributes.Append(attribute);

            attribute = doc.CreateAttribute("printAmount");
            attribute.Value = this.PrintAmount.ToString();
            workflow.Attributes.Append(attribute);

            attribute = doc.CreateAttribute("isMobile");
            attribute.Value = this.IsMobile.ToString();
            workflow.Attributes.Append(attribute);

            attribute = doc.CreateAttribute("depts");
            attribute.Value = this.Depts;
            workflow.Attributes.Append(attribute);

            attribute = doc.CreateAttribute("manager");
            attribute.Value = this.Manager;
            workflow.Attributes.Append(attribute);

            attribute = doc.CreateAttribute("workflowInstanceTitle");
            attribute.Value = this.WorkflowInstanceTitle;
            workflow.Attributes.Append(attribute);

            if (!string.IsNullOrEmpty(this._remark))
            {
                childNode = doc.CreateElement("remark");
                childNode.AppendChild(doc.CreateCDataSection(this._remark));
                workflow.AppendChild(childNode);
            }

            // 发单控制.
            childNode = doc.CreateElement("creationControl");

            attribute = doc.CreateAttribute("type");
            attribute.Value = this.CreationControlType;
            childNode.Attributes.Append(attribute);

            attribute = doc.CreateAttribute("minNotifyTaskCount");
            attribute.Value = this.MinNotifyTaskCount.ToString();
            childNode.Attributes.Append(attribute);

            attribute = doc.CreateAttribute("maxCreationInMonth");
            attribute.Value = this.MaxCreationInMonth.ToString();
            childNode.Attributes.Append(attribute);

            attribute = doc.CreateAttribute("maxCreationInWeek");
            attribute.Value = this.MaxCreationInWeek.ToString();
            childNode.Attributes.Append(attribute);

            attribute = doc.CreateAttribute("maxCreationUndone");
            attribute.Value = this.MaxCreationUndone.ToString();
            childNode.Attributes.Append(attribute);

            workflow.AppendChild(childNode);

            childNode = doc.CreateElement("smsNotify");
            childNode.AppendChild(doc.CreateCDataSection(this.SmsNotifyFormat));
            workflow.AppendChild(childNode);

            childNode = doc.CreateElement("emailNotify");
            childNode.AppendChild(doc.CreateCDataSection(this.EmailNotifyFormat));
            workflow.AppendChild(childNode);

            childNode = doc.CreateElement("statSMSNotify");
            childNode.AppendChild(doc.CreateCDataSection(this.StatSmsNodifyFormat));
            workflow.AppendChild(childNode);

            childNode = doc.CreateElement("statEmailNotify");
            childNode.AppendChild(doc.CreateCDataSection(this.StatEmailNodifyFormat));
            workflow.AppendChild(childNode);

            return workflow;
        }

        public static string ToCreationControlType(string value)
        {
            if (value != null)
                value = value.Trim().ToLower();
            if (value != "room" || value != "dept")
                return string.Empty;
            return value;
        }

        public static int ToInt32(string value)
        {
            if (string.IsNullOrEmpty(value))
                return -1;
            int result = -1;
            if (int.TryParse(value, out result))
                return result;
            return -1;
        }

        /// <summary>
        /// 更新流程设置.
        /// </summary>
        /// <returns></returns>
        public bool UpdateData()
        {
            if (string.IsNullOrEmpty(this.WorkflowName))
                return false;

            try
            {
                // 更新流程设置(bwwf_WorkflowSettings).
                WorkflowSetting setting = new WorkflowSetting();
                setting.WorkflowName = this.WorkflowName;
                setting.WorkflowAlias = this.WorkflowAlias;
                setting.AliasImage = this.AliasImage;
                setting.BasicFields = this.BasicFields;
                setting.TaskNotifyMinCount = this.MinNotifyTaskCount;
                setting.UndoneMaxCount = this.MaxCreationUndone;

                UpdateWorkflowSettings(setting);

                this.Insert(); // 更新流程设置(xqp_WorkflowSettings).
                log.Info("workflow designer update settings.");
                Botwave.XQP.Commons.LogWriter.Write("更新流程设置", this.WorkflowName);
            }
            catch (Exception ex)
            {
                Botwave.XQP.Commons.LogWriter.Write(ex);
                log.Error(ex);
                return false;
            }
            return true;
        }

        public static void UpdateWorkflowSettings(WorkflowSetting setting)
        {
            object results = IBatisMapper.Mapper.QueryForObject("bwwf_WorkflowSettings_Select_IsExists", setting.WorkflowName);
            if (results == null || string.IsNullOrEmpty(results.ToString()))
            {
                IBatisMapper.Insert("bwwf_WorkflowSettings_Insert", setting); // 新增
            }
            else
            {
                IBatisMapper.Update("bwwf_WorkflowSettings_Update", setting);    // 更新
            }
        }

        public static WorkflowComponent GetWorkflow(Guid workflowId)
        {
            return IBatisMapper.Load<WorkflowComponent>("bwwf_Designer_Workflow_Select_By_WorkflowId", workflowId);
        }
    }
}
