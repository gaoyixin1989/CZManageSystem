using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Botwave.Workflow.Practices.CZMCC
{
    /// <summary>
    /// 类：封装 getuserinforeq 请求报文中的 svccont 节点
    /// N.B.根据规范，在 getuserinforeq 请求报文中 svccont 节点只有一个子节点 token
    /// </summary>
    public class SvcCont
    {
        public string token = "650013e2a1dc11496fcbbdb5a0d8b57f";
    }

    /// <summary>
    /// 类：封装 getusrinforeq 请求的数据报文
    /// 根据规范或者报文样例自定义
    /// </summary>
    [XmlRoot("ecity")] /*指定在 XML 文档中相应根节点名称*/
    public class GetUserInfoReqestDgrm
    {
        [XmlElement("msgname")] /* 指定在 XML文档中相应节点名称，必须与规范文档保持一致 */ 
        public string msgName = "getuserinforeq";

        [XmlElement("msgversion")]
        public string msgVersion = "1.0.0";

        [XmlElement("msgsender")]
        public string msgSender = "ecclient"; /* 单点登录报文统为取值：ecclient */

        [XmlElement("transactionid")] 
        public string transId;

        [XmlElement("timestamp")] 
        public string timeStamp;

        [XmlElement("svccont")] /* 指定在 XML文档中相应节点名称 */
        public SvcCont svcContent;

        public GetUserInfoReqestDgrm()
        {
            /* 初始化类成员变量 */
            this.svcContent = new SvcCont();
        }
    }

    public class GetUserInfo
    {
        /// <summary>
        /// 辅助函数：用于生成 time-stamp 字符串
        /// </summary>
        /// <returns></returns>
        private string generateTimeStamp()
        {
           return DateTime.Now.ToString("yyyyMMddHHmmss");
        }
        /// <summary>
        /// 辅助函数：根据规范创建应用范围内唯一的事务ID字符串
        /// </summary>
        /// <returns></returns>
        private string generateTransId()
        {
            /* 在发起方唯一标识一个交易的流水号，系统内唯一，18位唯一流水号
             * 组成方式： 平台标识2位＋8位日期＋8位唯一数，每天从00000001开始，如AD2011090200000001
            */
            string strPrefix = "CM";
            string strDate = DateTime.Now.ToString("yyyyMMdd");
            string strUnique = DateTime.Now.TimeOfDay.TotalMilliseconds.ToString();

            return strPrefix + strDate + strUnique.Substring(0, 8);
        }

        /// <summary>
        /// 函数：用于向企业彩云平台发送 getuserinforeq 报文    
        /// </summary>
        /// <param name="userToken"> 用户token 串</param>
        public string SendGetUserInfoRequest(string userToken)
        {
            GetUserInfoReqestDgrm gui = new GetUserInfoReqestDgrm();
            gui.transId = generateTransId();     /* 生成本次请求事务 id */
            gui.timeStamp = generateTimeStamp(); /* 生成本次请求的时间戳  */
            gui.svcContent.token = userToken;    /* 用户 token , 从客户端的请求链接/报文中获取 */

            /* 把报文对象序列化成为一个 XML 字符串*/
            XmlSerializer xmlSlr = new XmlSerializer(typeof(GetUserInfoReqestDgrm));        
            StringWriter strWriter = new StringWriter();
            xmlSlr.Serialize(strWriter, gui);
            string strPostData = strWriter.ToString(); 
#if DEBUG
            /* 将报文打印到文件，以便分析问题*/
            //FileStream fs = new FileStream(System.Web.HttpContext.Current.Server.MapPath("Data/sso_login_dgm.xml"), FileMode.Create);
            //TextWriter txtWriter = new StreamWriter(fs, Encoding.UTF8);
            //xmlSlr.Serialize(txtWriter, gui);
            //txtWriter.Close();
            //fs.Close();
#endif
            /* 将报文发送到企业彩云平台接口 */
            //string srv_url = "http://120.197.89.115:8095/tuip/portalAgent"; /* 测试环境 *//*正式 http://120.197.89.78:8087/tuip/portalAgent*/
            string srv_url = System.Configuration.ConfigurationManager.AppSettings["__QYCY__"];
            WebClient wc   = new WebClient();
            wc.Encoding    = Encoding.UTF8;    /* N.B. 企业彩云平台服务接口统一采用 UTF-8 编码 */

            string strResp = wc.UploadString(srv_url, strPostData);

            Debug.Write(strResp);
#if DEBUG
            /* 将报文打印到文件，以便分析问题*/
            //fs = new FileStream(System.Web.HttpContext.Current.Server.MapPath("Data/sso_login_rsp.xml"), FileMode.Create);
            //txtWriter = new StreamWriter(fs);
            
            //txtWriter.Write(strResp);
            //txtWriter.Close();
            //fs.Close();
#endif
            return strResp;
        }
    }
}