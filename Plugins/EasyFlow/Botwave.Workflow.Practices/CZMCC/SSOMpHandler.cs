using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.SessionState;
using Botwave.Web;
using Botwave.Security;
using Botwave.Security.Service;
using Botwave.Workflow.Practices.CZMCC.Support;
using System.Xml.Linq;
using System.Data;

namespace Botwave.Workflow.Practices.CZMCC
{
    /// <summary>
    /// 潮州移动单点登录(Portal Login)处理类.
    /// </summary>
    public class SSOMpHandler : BaseSSOProxy
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(SSOMpHandler));
        private static readonly string flag = System.Configuration.ConfigurationManager.AppSettings["__QYCY_FLAG__"];
        static SSOMpHandler()
        {
            authService = new Botwave.Workflow.Practices.CZMCC.Support.DefaultAuthService();
        }

        /// <summary>
        /// 构造方法.
        /// </summary>
        public SSOMpHandler()
        {  }

        protected override string GetPortalAccount(HttpContext context)
        {
            string token = string.Empty;
            string eccode = string.Empty;
            try
            {
                token=context.Request.QueryString["token"];
                eccode = context.Request.QueryString["eccode"];
                log.InfoFormat("ReqUrl:{2}; eccode:{0}; Token:{1};",
                    eccode,
                   token,
                   context.Request.RawUrl);
            }
            catch (Exception ex)
            {
                log.ErrorFormat("display: {0}", ex.ToString());
            }

            try
            {
                if (md5(flag) != eccode)
                {
                    log.Error("应用标识不合法");
                    return null;
                }
                GetUserInfo info = new GetUserInfo();
                string xml = info.SendGetUserInfoRequest(token);
                log.Info(xml);
                XElement xe = XElement.Parse(xml);
                XElement item = xe.Element("result");
                XElement rc = item.Element("rspcode");
                XElement rcc = item.Element("rspdesc");
                if (rc.Value == "0") 
                {
                    XElement sc = xe.Element("svccont");
                    XElement u = sc.Element("userinfo");
                    string username = u.Element("username").Value;
                    string mobnum = u.Element("mobnum").Value;
                    string realname = u.Element("name").Value;
                    if (!string.IsNullOrEmpty(username))
                    {
                        if (username.Split(';').Length > 0)
                        {
                            log.Info("username字段异常，将进行截取...");
                            return username.Split(';')[0];
                        }
                    }
                    else//企业彩云返回数据异常，username的内容为空
                    {
                        //手机号在系统账号里面会有重复
                        object user = Botwave.Extension.IBatisNet.IBatisDbHelper.ExecuteScalar(CommandType.Text,string.Format("select username from bw_users where mobile='{0}' and realname = '{1}'",mobnum,realname));
                        username = Botwave.Commons.DbUtils.ToString(user);
                    }
                    return username;
                }
                return null;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }

        protected override void OnPostGetAccount(ref string portalAccount)
        {
            if (string.IsNullOrEmpty(portalAccount) && LoginHelper.IsLoginUser)
            {
                portalAccount = LoginHelper.UserName;
                log.DebugFormat("sso.current: {0}", portalAccount);
            }
        }

        protected override void OnGetAccountFail(HttpContext context)
        {
            log.Error("没有取到用户统一登录的相关数据.");
            authService.Logout(string.Empty);
            context.Response.Redirect(WebUtils.GetAppPath()+"apps/gmcc/pages/loginmobile.aspx");
            return;
        }

        protected override void OnGetAccountSuccess(HttpContext context, string account)
        {
            log.InfoFormat("OnGetAccountSuccess:{0}", account);
            //portal验证成功，应用本地系统逻辑。根据portal账号获取登录信息，再跳转到本地系统特定页面.
            if (authService.TrustedLogin(account) != LoginStatus.Success)
            {
                log.ErrorFormat("TrustedLogin fail:{0}", account);
                context.Response.Redirect(WebUtils.GetAppPath() + "apps/gmcc/pages/loginmobile.aspx");
                return;
            }
            else
            {
                log.DebugFormat("TrustedLogin success:{0}", account);
                //string queryString = context.Request.Url.Query;
                //if (queryString != null && queryString.Length > 0)
                //{
                //    queryString = queryString.Remove(0, 1);
                //    queryString = HttpUtility.UrlDecode(queryString);
                //    context.Response.Redirect(queryString);
                //}
                context.Response.Redirect(WebUtils.GetAppPath() + "contrib/mobile/pages/default.aspx");
            }
        }

        public string md5(string data)
        {
            byte[] b = System.Text.Encoding.Default.GetBytes(data);
            b = new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
            {
                ret += b[i].ToString("x").PadLeft(2, '0');
            }
            return ret;
        }
    }
}
