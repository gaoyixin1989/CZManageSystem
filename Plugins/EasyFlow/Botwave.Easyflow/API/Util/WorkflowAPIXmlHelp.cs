namespace Botwave.Easyflow.API.Util
{
    using Botwave.Easyflow.API;
    using System;
    using System.Reflection;
    using System.Xml.Linq;

    public class WorkflowAPIXmlHelp
    {
        private XAttribute _appAuth = new XAttribute("AppAuth", "");
        private XAttribute _errorMsg = new XAttribute("ErrorMsg", "");
        private string _pageCount = "100";
        private string _pageIndex = "0";
        private XElement _Root;
        private XAttribute _success = new XAttribute("Success", "");
        private XElement _xContent;
        private XDocument _xDom = new XDocument();

        public WorkflowAPIXmlHelp()
        {
            this._Root = new XElement("Result", new object[] { "", this._appAuth, this._success, this._errorMsg });
            this._xDom.Add(this._Root);
        }

        private string GetAttr(XAttribute attr, XElement Node)
        {
            if (attr == null)
            {
                XElement element = Node.Element("workflow");
                if (element == null)
                {
                    return Node.Value;
                }
                return element.ToString();
            }
            return attr.Value;
        }

        public T GetEntity<T>(string ObjectXML) where T: new()
        {
            T local = new T();
            Type type = typeof(T);
            XElement element = XElement.Parse(ObjectXML);
            XAttribute attribute = element.Attribute("action");
            XAttribute attribute2 = element.Attribute("username");
            XAttribute attribute3 = element.Attribute("keypassword");
            XAttribute attribute4 = element.Attribute("pageindex");
            XAttribute attribute5 = element.Attribute("pagecount");
            if ((attribute2 == null) || string.IsNullOrEmpty(attribute2.Value))
            {
                throw new WorkflowAPIException(1);
            }
            if ((attribute3 == null) || string.IsNullOrEmpty(attribute3.Value))
            {
                throw new WorkflowAPIException(2);
            }
            PropertyInfo property = type.GetProperty("Action");
            PropertyInfo info2 = type.GetProperty("UserName");
            PropertyInfo info3 = type.GetProperty("KeyPassword");
            PropertyInfo info4 = type.GetProperty("PageIndex");
            PropertyInfo info5 = type.GetProperty("PageCount");
            property.SetValue(local, (attribute == null) ? string.Empty : attribute.Value.ToLower(), null);
            info2.SetValue(local, (attribute2 == null) ? string.Empty : attribute2.Value, null);
            info3.SetValue(local, (attribute3 == null) ? string.Empty : attribute3.Value, null);
            if (type.UnderlyingSystemType.Name != "ManageEntity")
            {
                info4.SetValue(local, (attribute4 == null) ? string.Empty : attribute4.Value, null);
                if (!((attribute5 == null) || string.IsNullOrEmpty(attribute5.Value)))
                {
                    info5.SetValue(local, attribute5.Value, null);
                }
                else
                {
                    info4.SetValue(local, this._pageIndex, null);
                    info5.SetValue(local, this._pageCount, null);
                }
            }
            PropertyInfo[] properties = type.GetProperties();
            XElement objA = element.Element("parameter");
            if (!object.Equals(objA, null))
            {
                foreach (PropertyInfo info6 in properties)
                {
                    string str = info6.Name.ToString();
                    foreach (XElement element3 in objA.Elements("item"))
                    {
                        string str2 = element3.Attribute("name").Value;
                        if (string.IsNullOrEmpty(str2))
                        {
                            break;
                        }
                        if (str.ToUpper() == str2.ToUpper())
                        {
                            string attr = this.GetAttr(element3.Attribute("value"), element3);
                            info6.SetValue(local, attr, null);
                            break;
                        }
                    }
                }
            }
            return local;
        }

        public override string ToString()
        {
            string str = string.Empty;
            if (object.Equals(this._xDom, null))
            {
                return str;
            }
            if (this._xContent != null)
            {
                this._Root.Add(this.XContent);
            }
            return this._xDom.ToString();
        }

        public string AppAuth
        {
            get
            {
                return this._appAuth.Value;
            }
            set
            {
                if (this._appAuth == null)
                {
                    this._appAuth = new XAttribute("AppAuth", "");
                }
                this._appAuth.Value = value;
            }
        }

        public string ErrorMsg
        {
            get
            {
                return this._errorMsg.Value;
            }
            set
            {
                if (this._errorMsg == null)
                {
                    this._errorMsg = new XAttribute("ErrorMsg", "");
                }
                this._errorMsg.Value = value;
            }
        }

        public string Success
        {
            get
            {
                return this._success.Value;
            }
            set
            {
                if (this._success == null)
                {
                    this._success = new XAttribute("Success", "");
                }
                this._success.Value = value;
            }
        }

        public XElement XContent
        {
            get
            {
                return this._xContent;
            }
            set
            {
                this._xContent = value;
            }
        }
    }
}

