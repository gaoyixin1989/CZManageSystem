using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Botwave.XQP.API.Proxies
{
    public class InterfaceManagerHelper
    {
        public static readonly InterfaceManagerHelper Default = new InterfaceManagerHelper();


        private string __SystemID__ = ConfigurationManager.AppSettings["__SystemID__"];
        private string __SysAccount__ = ConfigurationManager.AppSettings["__SysAccount__"];
        private string __SysPassword__ = ConfigurationManager.AppSettings["__SysAccount__"];
        private static string InterfaceManagerUrl = ConfigurationManager.AppSettings["__InterfaceManagerUrl__"];
        private static string InterfaceManagerID = ConfigurationManager.AppSettings["__InterfaceManagerID__"];

        private InterfaceManager _interfaceManager;

        public InterfaceManagerHelper()
        {
            _interfaceManager = new InterfaceManager(InterfaceManagerUrl);            
        }

        /// <summary>
        /// 验证调用接口方是否有权限调用
        /// </summary>
        /// <param name="interfaceId">接入综合应用平台的接口标识号，由综合应用平台管理员给出</param>
        /// <param name="transferSystemId">调用接口方的综合应用平台分配的系统标识号</param>
        /// <param name="transferSysAccount">调用接口方的综合应用平台的账号，由综合应用平台管理员给出</param>
        /// <param name="transferSysPassword">调用接口方的综合应用平台的密码，由综合应用平台管理员给出</param>
        /// <returns></returns>
        public InterfaceActionResult ValiateInterface(string transferSystemId, string transferSysAccount, string transferSysPassword)
        {
            InterfaceActionResult var = new InterfaceActionResult();
            ValidateInfaceResult vifr = _interfaceManager.ValiateInterface(__SystemID__, __SysAccount__, __SysPassword__, InterfaceManagerID, transferSystemId, transferSysAccount, transferSysPassword);
            var.AppAuth = vifr.AppAuth;
            var.ReturnMessage = vifr.ReturnMessage;
            var.ReturnValue = vifr.ReturnValue;
            return var;
        }
    }

    /// <summary>
    /// 调用接口操作结束
    /// AppAuth:0: 操作成功
    ///         -95: 接口已被禁用
    ///         -96：没有权限使用接口
    ///         -97：接口没有注册到系统
    ///         -100：系统产生未知错误
    ///         -1：调用接口失败
    /// ReturnMessage:返回错误信息
    /// ReturnValue:操作是否成功
    /// </summary>
    [Serializable]
    public partial class InterfaceActionResult
    {

        private int appAuthField;

        private bool returnValueField;

        private string returnMessageField;

        /// <remarks/>
        public int AppAuth
        {
            get
            {
                return this.appAuthField;
            }
            set
            {
                this.appAuthField = value;
            }
        }

        /// <remarks/>
        public bool ReturnValue
        {
            get
            {
                return this.returnValueField;
            }
            set
            {
                this.returnValueField = value;
            }
        }

        /// <remarks/>
        public string ReturnMessage
        {
            get
            {
                return this.returnMessageField;
            }
            set
            {
                this.returnMessageField = value;
            }
        }
    }
}
