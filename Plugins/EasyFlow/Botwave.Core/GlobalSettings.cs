using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Botwave
{
    /// <summary>
    /// 全局配置类.
    /// </summary>
    public class GlobalSettings : ConfigurationSection
    {
        private static GlobalSettings instance = null;

        private const string pluginPath = "plugins/easyflow/";
        /// <summary>
        /// 实例对象.
        /// </summary>
        public static GlobalSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (typeof(GlobalSettings))
                    {
                        if (instance == null)
                        {
                            instance = ConfigurationManager.GetSection("botwave/global") as GlobalSettings;
                        }
                    }
                }
                return instance;
            }
        }

        #region properties

        /// <summary>
        /// 是否允许界面主题.
        /// </summary>
        [ConfigurationProperty("allowTheme", DefaultValue = false)]
        public bool AllowTheme
        {
            get { return (bool)this["allowTheme"]; }
            set { this["allowTheme"] = value; }
        }

        /// <summary>
        /// 界面主题的健值.
        /// </summary>
        [ConfigurationProperty("themeKey", DefaultValue = "{BA8DACF9-5DDB-43b7-B545-AC7A0E53F954}")]
        public string ThemeKey
        {
            get { return (string)this["themeKey"]; }
            set { this["themeKey"] = value; }
        }

        /// <summary>
        /// 系统消息的健值.
        /// </summary>
        [ConfigurationProperty("messageKey", DefaultValue = "{DD6DB1B2-F06F-4351-9B14-D187450A094B}")]
        public string MessageKey
        {
            get { return (string)this["messageKey"]; }
            set { this["messageKey"] = value; }
        }

        /// <summary>
        /// 默认工作主页.
        /// </summary>
        [ConfigurationProperty("defaultPage", DefaultValue = pluginPath + "main/default.aspx")]
        public string DefaultPage
        {
            get { return (string)this["defaultPage"]; }
            set { this["defaultPage"] = value; }
        }


        /// <summary>
        /// 临时目录.
        /// </summary>
        [ConfigurationProperty("temporaryDir", DefaultValue = pluginPath + "App_Data/Temp/")]
        public string TemporaryDir
        {
            get { return (string)this["temporaryDir"]; }
            set { this["temporaryDir"] = value; }
        }

        /// <summary>
        /// 文件上传路径.
        /// </summary>
        [ConfigurationProperty("uploadpath", DefaultValue = pluginPath + "App_Data/Temp/")]
        public string UploadPath
        {
            get { return (string)this["uploadpath"]; }
            set { this["uploadpath"] = value; }
        }

        /// <summary>
        /// 操作成功的提示信息.
        /// </summary>
        [ConfigurationProperty("successMessage", DefaultValue = "操作成功.")]
        public string SuccessMessage
        {
            get { return (string)this["successMessage"]; }
            set { this["successMessage"] = value; }
        }

        /// <summary>
        /// 操作失败的提示信息.
        /// </summary>
        [ConfigurationProperty("failMessage", DefaultValue = "操作错误.")]
        public string FailMessage
        {
            get { return (string)this["failMessage"]; }
            set { this["failMessage"] = value; }
        }

        /// <summary>
        /// 没有对应权限的提示信息.
        /// </summary>
        [ConfigurationProperty("noPermissionMessage", DefaultValue = "您没有该操作权限.")]
        public string NoPermissionMessage
        {
            get { return (string)this["noPermissionMessage"]; }
            set { this["noPermissionMessage"] = value; }
        }

        /// <summary>
        /// 格式化异常提示信息.
        /// </summary>
        [ConfigurationProperty("formatExceptionMessage", DefaultValue = "格式错误.")]
        public string FormatExceptionMessage
        {
            get { return (string)this["formatExceptionMessage"]; }
            set { this["formatExceptionMessage"] = value; }
        }

        /// <summary>
        /// 参数异常提示信息.
        /// </summary>
        [ConfigurationProperty("argumentExceptionMessage", DefaultValue = "参数错误.")]
        public string ArgumentExceptionMessage
        {
            get { return (string)this["argumentExceptionMessage"]; }
            set { this["argumentExceptionMessage"] = value; }
        }

        /// <summary>
        /// 删除操作前的确认信息.
        /// </summary>
        [ConfigurationProperty("confirmBeforeDeleteMessage", DefaultValue = "确定要删除吗?")]
        public string ConfirmBeforeDeleteMessage
        {
            get { return (string)this["confirmBeforeDeleteMessage"]; }
            set { this["confirmBeforeDeleteMessage"] = value; }
        }

        /// <summary>
        /// 系统访问址.
        /// </summary>
        [ConfigurationProperty("address")]
        public string Address
        {
            get { return (string)this["address"]; }
            set { this["address"] = value; }
        }

        /// <summary>
        /// 系统签名，可用于发送短信/邮件等.
        /// </summary>
        [ConfigurationProperty("signature", DefaultValue = "广州博汇数码科技有限公司")]
        public string Signature
        {
            get { return (string)this["signature"]; }
            set { this["signature"] = value; }
        }

        /// <summary>
        /// 应用程序是否处于调试模式，此参数对应用程序级别的异常处理有影响.
        /// </summary>
        [ConfigurationProperty("isDebugMode", DefaultValue = false)]
        public bool IsDebugMode
        {
            get { return (bool)this["isDebugMode"]; }
            set { this["isDebugMode"] = value; }
        }

        /// <summary>
        /// 密钥
        /// </summary>
        [ConfigurationProperty("encryptKey", DefaultValue = "{A090CB24-AF38-4544-92F8-A5B9F1A36ABD}")]
        public string EncryptKey
        {
            get { return (string)this["encryptKey"]; }
            set { this["encryptKey"] = value; }
        }
        #endregion
    }
}
