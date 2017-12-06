using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ServiceLibrary.Domain
{
    [XmlType("APPLICATION")]
    public class Application
    {
        [XmlElement("APPID")]
        public string appID { get; set; }
        [XmlElement("WEBSERVICEPWD")]
        public string webServicePwd { get; set; }
        [XmlElement("ERRMESSAGE")]
        public ErrMessage errMessage { get; set; }
    }
    
    [XmlType("ERRMESSAGE")]
    public class ErrMessage
    {
        [XmlElement("ERRCODE")]
        public string errCode { get; set; }
        [XmlElement("ERRDESC")]
        public string errDesc { get; set; }
    }

    [XmlType("return")]
    public class ReturnCode
    {
        public string code { get; set; }
    }

}
