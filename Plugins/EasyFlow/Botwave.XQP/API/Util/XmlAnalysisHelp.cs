using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using Botwave.XQP.API.Entity;
using System.Xml.Linq;
using System.Data;
using Botwave.Easyflow.API;
using Botwave.Easyflow.API.Entity;

namespace Botwave.XQP.API.Util
{
    public static class XmlAnalysisHelp
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(XmlAnalysisHelp));

        /// <summary>
        /// GUID验证
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Guid? ToGuid(string value)
        {
            if (string.IsNullOrEmpty(value))
                return (Guid?)null;
            try
            {
                Guid result = new Guid(value);
                return result;
            }
            catch
            {
                return (Guid?)null;
            }
        }

        /// <summary>
        /// 时间转换
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDatetime(string value)
        {
            string ret_val = string.Empty;
            if (string.IsNullOrEmpty(value))
                return ret_val;

            try { ret_val = DateTime.Parse(value).ToString("yyyy-MM-dd HH:mm:ss"); }
            catch { }
            return ret_val;
        }

        /// <summary>
        /// 处理方法解析xml
        /// </summary>
        /// <param name="ObjectXML"></param>
        /// <returns></returns>
        public static WorkflowDetail AnalysisXml(string ObjectXML)
        {
            WorkflowDetail Detail = new WorkflowDetail();
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            XElement workflow = XElement.Parse(ObjectXML);//将string解析为XML

            #region 基本属性

            XAttribute Secrecy = null;
            XAttribute Urgency = null;
            XAttribute Importance = null;
            XAttribute ExpectFinishedTime = null;
            //XAttribute EAurl = workflow.Attribute("expectFinishedTime");//EA单特殊需要
            foreach (XAttribute attr in workflow.Attributes())
            {
                if (attr.Name.ToString().ToLower() == "secrecy")
                    Secrecy = attr;
                else if (attr.Name.ToString().ToLower() == "urgency")
                    Urgency = attr;
                else if (attr.Name.ToString().ToLower() == "importance")
                    Importance = attr;
                else if (attr.Name.ToString().ToLower() == "expectfinishedtime")
                    ExpectFinishedTime = attr;
            }
            Detail.Secrecy = Secrecy == null ? 0 : int.Parse(Secrecy.Value);
            Detail.Urgency = Urgency == null ? 1 : int.Parse(Urgency.Value);
            Detail.Importance = Importance == null ? 0 : int.Parse(Importance.Value);
            Detail.ExpectFinishedTime = ExpectFinishedTime == null ? string.Empty : ExpectFinishedTime.Value;
            //Detail.EAurl = EAurl == null ? string.Empty : EAurl.Value;

            #endregion

            XElement Fields = new XElement("fields");
            XElement Attachments = new XElement("attachments"); ;
            XElement NextActivities = new XElement("nextactivities"); ;
            foreach (XElement node in workflow.Elements())
            {
                if (node.Name.ToString().ToLower().Equals("fields"))
                    Fields.Add(node.Elements());
                else if (node.Name.ToString().ToLower().Equals("attachments"))
                    Attachments.Add(node.Elements());
                else if (node.Name.ToString().ToLower().Equals("nextactivities"))
                    NextActivities.Add(node.Elements());

            }
            #region 表单属性

            //XElement Fields = workflow.Element("fields");
            if (Fields != null)
            {
                IEnumerable<XElement> FieldsNodes = Fields.Elements("item");//fields下的item集合
                List<Field> fArray = new List<Field>();
                int i = 0;
                foreach (XElement fieldsNode in FieldsNodes)
                {
                    Field f = new Field();
                    f.Key = fieldsNode.Attribute("name").Value;
                    XAttribute itemValue = fieldsNode.Attribute("value");
                    f.Value = itemValue == null ? fieldsNode.Value : itemValue.Value;
                    fArray.Add(f);
                    i++;
                }
                Detail.Fields = fArray.ToArray();
            }

            #endregion

            #region 附件属性

            //XElement Attachments = workflow.Element("attachments");
            if (Attachments != null)
            {
                IEnumerable<XElement> AttachmentsNodes = Attachments.Elements("item");//fields下的item集合
                List<Attachment> Array = new List<Attachment>();
                int i = 0;
                foreach (XElement AttachmentNode in AttachmentsNodes)
                {
                    Attachment a = new Attachment();
                    a.Name = AttachmentNode.Attribute("name").Value;
                    a.Creator = AttachmentNode.Attribute("creator").Value;
                    a.Url = AttachmentNode.Attribute("url").Value;
                    a.CreatedTime = AttachmentNode.Attribute("createdtime").Value;
                    Array.Add(a);
                    i++;
                }
                Detail.Attachments = Array.ToArray();
            }

            #endregion

            #region 处理属性
            
            //XElement NextActivities = workflow.Element("nextactivities");
            if (NextActivities != null)
            {
                IEnumerable<XElement> NextActivitiesNodes = NextActivities.Elements("item");//fields下的item集合
                //Activity[] nArray = new Activity[NextActivitiesNodes.Count()];
                List<Activity> nArray = new List<Activity>();
                int i = 0;
                foreach (XElement NextActivitiesNode in NextActivitiesNodes)
                {
                    Activity a = new Activity();
                    a.Name = NextActivitiesNode.Attribute("name").Value;
                    a.Actors = NextActivitiesNode.Attribute("actors").Value.Split(',');
                    nArray.Add(a);
                    i++;
                }
                Detail.NextActivities = nArray.ToArray();
            }

            #endregion

            return Detail;
        }

        public static Attachment[] AttachmentXml(string ObjectXML)
        {
            Attachment[] Array = null;
            XElement workflow = XElement.Parse(ObjectXML);//将string解析为XML
            XElement Attachments = workflow.Element("attachments");
            List<Attachment> arrlist = new List<Attachment>();
            if (Attachments != null)
            {
                IEnumerable<XElement> AttachmentsNodes = Attachments.Elements("item");//attachments下的item集合
                
                //Array = new Attachment[AttachmentsNodes];
                int i = 0;
                foreach (XElement AttachmentNode in AttachmentsNodes)
                {
                    Attachment a = new Attachment();
                    a.Name = AttachmentNode.Attribute("name").Value;
                    a.Creator = AttachmentNode.Attribute("creator").Value;
                    a.Url = AttachmentNode.Attribute("url").Value;
                    a.CreatedTime = AttachmentNode.Attribute("createdtime").Value;
                    //Array[i] = a;
                    arrlist.Add(a);
                    i++;
                }
                //Array = arrlist.ToArray();
            }
            return arrlist.ToArray();
        }

        /// <summary>
        /// 将datatable转换为xml格式的字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="search"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public static string GetXmlContent(DataTable dt, SearchEntity search, int? recordCount)
        {
            XElement xContent = null; XAttribute xRecordCount = null, xName = null;

            if (!string.IsNullOrEmpty(search.Action))
            {
                xName = new XAttribute("Name", search.Action);
            }

            if (recordCount != 0)
            {
                xRecordCount = new XAttribute("recordCount", recordCount);
            }

            xContent = new XElement("item", xName, xRecordCount);

            try
            {
                ExpandHelper expand = new ExpandHelper();
                xContent = expand.GetExpandSearchXML(dt, search, xContent);
            }
            catch(Exception ex)
            {
                log.Error(ex);
                throw new WorkflowAPIException(5);
            }
            return xContent == null ? string.Empty : xContent.ToString();
        }

        /// <summary>
        /// 将datatable转换为xml格式的字符串
        /// </summary>
        /// <param name="strResult"></param>
        /// <param name="manage"></param>
        /// <returns></returns>
        public static string GetXmlContent(string strResult, ManageEntity manage)
        {
            XElement xContent = null; XAttribute  xName = null;

            if (!string.IsNullOrEmpty(manage.Action))
            {
                xName = new XAttribute("Name", manage.Action);
            }

            xContent = new XElement("item", xName);

            try
            {
                ExpandHelper expand = new ExpandHelper();
                xContent = expand.GetExpandManageXML(strResult, manage, xContent);
            }
            catch (Exception ex)
            {
                string mm = ex.Message;
                log.Error(ex.Message);
                throw new WorkflowAPIException(5);
            }
            return xContent == null ? string.Empty : xContent.ToString();
        }

        /// <summary>
        /// 将datatable转换为xml格式的字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="search"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public static string GetXmlContent(DataTable dt, SearchEntity search, IDictionary<string, object> formVariables, int? recordCount)
        {
            XElement xContent = null; XAttribute xRecordCount = null, xName = null;

            if (!string.IsNullOrEmpty(search.Action))
            {
                xName = new XAttribute("Name", search.Action);
            }

            if (recordCount != 0)
            {
                xRecordCount = new XAttribute("recordCount", recordCount);
            }

            xContent = new XElement("item", xName, xRecordCount);

            try
            {
                ExpandHelper expand = new ExpandHelper();
                xContent = expand.GetExpandSearchXML(dt, search, xContent, formVariables);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new WorkflowAPIException(5);
            }
            return xContent == null ? string.Empty : xContent.ToString();
        }
    }
}
