using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml.Linq;
using WebService_WF.Domain;

namespace WebService_WF.Base
{
    public class BaseFun
    {
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
            XElement item = workflow.Element("item");
            XElement detail = item.Element("detail");

            #region 基本属性

            IDictionary<string, object> baseDict = new Dictionary<string, object>();
            IEnumerable<XElement> details = detail.Elements("item");
            foreach (XElement node in details)
            {
                if (!baseDict.ContainsKey(node.Attribute("name").Value))
                    baseDict.Add(node.Attribute("name").Value, node.Attribute("value").Value);
            }
            Type t = typeof(WorkflowDetail);
            Detail = (WorkflowDetail)Activator.CreateInstance(t);// new Student()
            //7.3根据类型 获得 该类型的 所有属性定义
            PropertyInfo[] properties = t.GetProperties();
            //7.4遍历属性数组
            SetProperty(baseDict, Detail);
            #endregion

            IEnumerable<XElement> Fields = detail.Elements("Field");
            IEnumerable<XElement> Attachments = detail.Elements("Attachment");
            IEnumerable<XElement> PreActivities = detail.Elements("PreActivity"); ;
            IEnumerable<XElement> Activities = detail.Elements("Activitys"); ;
            IEnumerable<XElement> NextActivities = detail.Elements("NextActivitys"); ;

            #region 表单属性
            List<Field> listField = new List<Field>();
            if (Fields != null && Fields.Count() > 0)
            {
                foreach (XElement curField in Fields)
                {
                    Field f = new Field();
                    IEnumerable<XElement> FieldsNodes = curField.Elements("item");//field下的item集合
                    foreach (XElement curItem in FieldsNodes)
                    {
                        string temp = curItem.Attribute("name").Value.ToLower();
                        switch (temp)
                        {
                            case "fkey": f.Key = curItem.Attribute("value").Value; break;
                            case "name": f.Name = curItem.Attribute("value").Value; break;
                            case "fvalue": f.Value = curItem.Attribute("value").Value; break;
                            default: break;
                        }
                    }
                    listField.Add(f);
                }
            }
            Detail.Fields = listField;
            #endregion

            #region 附件属性
            List<Attachment> listAttachment = new List<Attachment>();
            if (Attachments != null && Attachments.Count() > 0)
            {
                foreach (XElement curAttachment in Attachments)
                {
                    Attachment f = new Attachment();
                    IEnumerable<XElement> Nodes = curAttachment.Elements("item");//field下的item集合
                    foreach (XElement curItem in Nodes)
                    {
                        string temp = curItem.Attribute("name").Value.ToLower();
                        switch (temp)
                        {
                            case "name": f.Name = curItem.Attribute("value").Value; break;
                            case "creator": f.Creator = curItem.Attribute("value").Value; break;
                            case "url": f.Url = curItem.Attribute("value").Value; break;
                            case "createdtime": f.CreatedTime = curItem.Attribute("value").Value; break;
                            default: break;
                        }
                    }
                    listAttachment.Add(f);
                }
            }
            Detail.Attachments = listAttachment;
            #endregion

            #region 处理属性-next步骤
            List<Activity> listNextActivitie = new List<Activity>();
            if (NextActivities != null && NextActivities.Count() > 0)
            {
                foreach (XElement curActivity in NextActivities)
                {
                    Activity f = new Activity();
                    IEnumerable<XElement> Nodes = curActivity.Elements("item");//field下的item集合
                    foreach (XElement curItem in Nodes)
                    {
                        string temp = curItem.Attribute("name").Value.ToLower();
                        switch (temp)
                        {
                            case "activityinstanceid": f.ActivityInstanceId = curItem.Attribute("value").Value; break;
                            case "activityid": f.ActivityId = curItem.Attribute("value").Value; break;
                            case "name": f.Name = curItem.Attribute("value").Value; break;
                            case "actors": f.Actors = curItem.Attribute("value").Value; break;
                            case "operatetype": f.OperateType = curItem.Attribute("value").Value; break;
                            case "command": f.Command = curItem.Attribute("value").Value; break;
                            default: break;
                        }
                    }
                    listNextActivitie.Add(f);
                }
            }
            Detail.NextActivities = listNextActivitie;
            #endregion
            
            #region 处理属性-pre步骤
            List<Activity> listPreActivitie = new List<Activity>();
            if (PreActivities != null && PreActivities.Count() > 0)
            {
                foreach (XElement curActivity in PreActivities)
                {
                    Activity f = new Activity();
                    IEnumerable<XElement> Nodes = curActivity.Elements("item");//field下的item集合
                    foreach (XElement curItem in Nodes)
                    {
                        string temp = curItem.Attribute("name").Value.ToLower();
                        switch (temp)
                        {
                            case "activityinstanceid": f.ActivityInstanceId = curItem.Attribute("value").Value; break;
                            case "activityid": f.ActivityId = curItem.Attribute("value").Value; break;
                            case "name": f.Name = curItem.Attribute("value").Value; break;
                            case "actors": f.Actors = curItem.Attribute("value").Value; break;
                            case "operatetype": f.OperateType = curItem.Attribute("value").Value; break;
                            case "command": f.Command = curItem.Attribute("value").Value; break;
                            default: break;
                        }
                    }
                    listPreActivitie.Add(f);
                }
            }
            Detail.PreActivities = listPreActivitie;
            #endregion
            
            #region 处理属性-当前步骤
            List<Activity> listActivitie = new List<Activity>();
            if (Activities != null && Activities.Count() > 0)
            {
                foreach (XElement curActivity in Activities)
                {
                    Activity f = new Activity();
                    IEnumerable<XElement> Nodes = curActivity.Elements("item");//field下的item集合
                    foreach (XElement curItem in Nodes)
                    {
                        string temp = curItem.Attribute("name").Value.ToLower();
                        switch (temp)
                        {
                            case "activityinstanceid": f.ActivityInstanceId = curItem.Attribute("value").Value; break;
                            case "activityid": f.ActivityId = curItem.Attribute("value").Value; break;
                            case "name": f.Name = curItem.Attribute("value").Value; break;
                            case "actors": f.Actors = curItem.Attribute("value").Value; break;
                            case "operatetype": f.OperateType = curItem.Attribute("value").Value; break;
                            case "command": f.Command = curItem.Attribute("value").Value; break;
                            default: break;
                        }
                    }
                    listActivitie.Add(f);
                }
            }
            Detail.Activities = listActivitie;
            #endregion

            return Detail;
        }

        public static void SetProperty(IDictionary<string, object> baseDict, object obj)
        {
            Type type = obj.GetType();
            PropertyInfo[] pro = type.GetProperties();
            foreach (PropertyInfo p in pro)
            {
                Type t = p.PropertyType;

                if (baseDict.ContainsKey(p.Name))
                {
                    //Object ob = Activator.CreateInstance(t);
                    object value = Convert.ChangeType(baseDict[p.Name], t);
                    p.SetValue(obj, value, null);
                }

            }
        }

        /// <summary>
        /// 根据属性名称，将第一个对象复制到第二个对象
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public static void AutoMapping<S, T>(S s, T t)
        {
            // get source PropertyInfos
            PropertyInfo[] pps = GetPropertyInfos(s.GetType());
            // get target type
            Type target = t.GetType();

            foreach (var pp in pps)
            {
                PropertyInfo targetPP = target.GetProperty(pp.Name);
                object value = pp.GetValue(s, null);

                if (targetPP != null && value != null)
                {
                    try { targetPP.SetValue(t, value, null); }
                    catch (Exception ex) { }

                }
            }
        }
        private static PropertyInfo[] GetPropertyInfos(Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        public static Users FindUserByUserName(string userName) {
            var temp=new SysUserService().FindByFeldName(u=>u.UserName==userName);
            return temp;
        }

    }
}