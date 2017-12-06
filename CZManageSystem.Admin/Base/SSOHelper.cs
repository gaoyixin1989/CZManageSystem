using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Newtonsoft.Json;
using CZManageSystem.Admin.Models;
using CZManageSystem.Service.SysManger;
using CZManageSystem.Core;

namespace CZManageSystem.Admin.Base
{
    /// <summary>
    /// portal单点登录验证类
    /// </summary>
    public class SSOHelper
    {
        private static string SSOLoginUrl = "";
        private static string SystemID ="";
        private static string SysAccount = "";
        private static string SysPassword = "";
        private static void Init()
        {
            try
            {
                SSOLoginUrl = ConfigurationManager.AppSettings["SSOLoginUrl"].ToString();
                SystemID = ConfigurationManager.AppSettings["__SystemID__"].ToString();
                SysAccount = ConfigurationManager.AppSettings["__SysAccount__"].ToString();
                SysPassword = ConfigurationManager.AppSettings["__SysPassword__"].ToString();
            }
            catch (Exception ex)
            {
                LogRecord.WriteLog(ex.ToString(), "Portal登录参数初始");
            }

        }
        public static AuthResult SSOLogin(string Account, string Password)
        {
            
            AuthResult result = new AuthResult() { authResult = false };
            try
            {
                Init();
                Object[] args = new Object[4];
                args[0] = Account; //"AQIC5wM2LY4SfcxOZ%2F6UCaTQbXux%2BfsKdXIkPNFNk4y%40AAJTSQACNTI%3D%23";//linweichao
                args[1] = Password;
                args[2] = SystemID; //系统名称(该名称为统一信息平台LDAP服务器中与接入应用系统相对应的角色名称。该角色名称由统一信息平台提供给应用系统。)
                args[3] = 1;//验证类型(1＝LDAP静态密码认证、2＝Safeword动态密码认证)

                Object obj = WebServicesHelper.InvokeWebService(SSOLoginUrl, "UIPService", "CommonLogin", args);
                if(obj!=null)
                    result = JsonConvert.DeserializeObject<AuthResult>(JsonConvert.SerializeObject(obj));
                else
                {
                    LogRecord.WriteLog("登录验证返回null", "Portal登录失败");
                }

            }
            catch (Exception ex)
            {
                result.authMsg = ex.Message;
                LogRecord.WriteLog(ex.ToString(),"Portal登录失败");
            }
            return result;

        }
        public  static AuthResult CheckLogin(string token)
        {
            AuthResult result = new AuthResult() { authResult = false };
            try
            {
                Init();
                string[] args = new string[2];
                args[0] = token; //"AQIC5wM2LY4SfcxOZ%2F6UCaTQbXux%2BfsKdXIkPNFNk4y%40AAJTSQACNTI%3D%23";//linweichao
                args[1] = SystemID;

                Object obj = WebServicesHelper.InvokeWebService(SSOLoginUrl, "UIPService", "ValidateToken", args);
                 result = JsonConvert.DeserializeObject<AuthResult>(JsonConvert.SerializeObject(obj));
                
            }
            catch (Exception ex)
            {
                 result.authMsg=ex.Message;
                LogRecord.WriteLog( ex.ToString(),"Portal验证失败");
            }
            return result;
        }

    }



}