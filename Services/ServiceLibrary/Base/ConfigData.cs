using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary.Base
{
    public class ConfigData
    {
        #region 流程易中配置系统信息
        /// <summary>
        /// 流程易_系统ID
        /// </summary>
        public static string Workflow_SystemID
        {
            get
            {
                return ConfigurationManager.AppSettings["Workflow_SystemID"];
            }
        }

        /// <summary>
        /// 流程易_系统名称
        /// </summary>
        public static string Workflow_SystemAcount
        {
            get
            {
                return ConfigurationManager.AppSettings["Workflow_SystemAcount"];
            }
        }

        /// <summary>
        /// 流程易_系统密码
        /// </summary>
        public static string Workflow_SystemPwd
        {
            get
            {
                return ConfigurationManager.AppSettings["Workflow_SystemPwd"];
            }
        }

        /// <summary>
        /// 流程易_接口地址
        /// </summary>
        public static string Workflow_SystemUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["Workflow_SystemUrl"];
            }
        }

        /// <summary>
        /// 流程易_接口查询类别方法名
        /// </summary>
        public static string WorkflowType_SearchWorkflow
        {
            get
            {
                return ConfigurationManager.AppSettings["WorkflowType_SearchWorkflow"];
            }
        }
        /// <summary>
        /// 流程易_接口处理类别方法名
        /// </summary>
        public static string WorkflowType_ManageWorkflow
        {
            get
            {
                return ConfigurationManager.AppSettings["WorkflowType_ManageWorkflow"];
            }
        }
        /// <summary>
        /// 流程易_“会议室申请”流程名
        /// </summary>
        public static string WorkflowApply_BoardroomApply
        {
            get
            {
                return ConfigurationManager.AppSettings["WorkflowApply_BoardroomApply"];
            }
        }

        #endregion

        #region UIM系统信息，用于同步组织和用户信息
        /// <summary>
        /// UIM系统_接口地址
        /// </summary>
        public static string UIMinfo_url
        {
            get
            {
                return ConfigurationManager.AppSettings["UIMinfo_url"];
            }
        }

        /// <summary>
        /// UIM系统_类名称
        /// </summary>
        public static string UIMinfo_classname
        {
            get
            {
                return ConfigurationManager.AppSettings["UIMinfo_classname"];
            }
        }

        /// <summary>
        /// UIM系统_方法名
        /// </summary>
        public static string UIMinfo_methodname
        {
            get
            {
                return ConfigurationManager.AppSettings["UIMinfo_methodname"];
            }
        }

        /// <summary>
        /// UIM系统_应用系统id
        /// </summary>
        public static string UIMinfo_appid
        {
            get
            {
                return ConfigurationManager.AppSettings["UIMinfo_appid"];
            }
        }

        /// <summary>
        /// UIM系统_服务密码
        /// </summary>
        public static string UIMinfo_webservicepwd
        {
            get
            {
                return ConfigurationManager.AppSettings["UIMinfo_webservicepwd"];
            }
        }

        /// <summary>
        /// UIM系统-同步组织LOGID
        /// </summary>
        public static decimal UIMinfo_lastOrgLogId
        {
            get
            {
                decimal temp = 0;
                decimal.TryParse(ConfigurationManager.AppSettings["UIMinfo_lastOrgLogId"], out temp);
                return temp;
            }
            set
            {
                updateOrAddAppSetting("UIMinfo_lastOrgLogId", value.ToString());
            }
        }

        /// <summary>
        /// UIM系统-同步用户LOGID
        /// </summary>
        public static decimal UIMinfo_lastUserLogId
        {
            get
            {
                decimal temp = 0;
                decimal.TryParse(ConfigurationManager.AppSettings["UIMinfo_lastUserLogId"], out temp);
                return temp;
            }
            set
            {
                updateOrAddAppSetting("UIMinfo_lastUserLogId", value.ToString());
            }
        }
        #endregion

        #region portal系统信息，用于推送待办待阅
        /// <summary>
        ///portal系统待办待阅_推送地址
        /// </summary>
        public static string Portal_PendingUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["Portal_PendingUrl"];
            }
        }
        /// <summary>
        /// portal系统待办待阅_调用类名
        /// </summary>
        public static string Portal_PendingClassName
        {
            get
            {
                return ConfigurationManager.AppSettings["Portal_PendingClassName"];
            }
        }
        /// <summary>
        /// portal系统待办待阅_系统ID
        /// </summary>
        public static string Portal_SystemId
        {
            get
            {
                return ConfigurationManager.AppSettings["Portal_SystemId"];
            }
        }
        /// <summary>
        /// portal系统待办待阅_用户名
        /// </summary>
        public static string Portal_UserName
        {
            get
            {
                return ConfigurationManager.AppSettings["Portal_UserName"];
            }
        }
        /// <summary>
        /// portal系统待办待阅_密码
        /// </summary>
        public static string Portal_PassWord
        {
            get
            {
                return ConfigurationManager.AppSettings["Portal_PassWord"];
            }
        }
        /// <summary>
        /// portal系统待办待阅处理地址域名和端口部分
        /// </summary>
        public static string Portal_DealUrlAuthority
        {
            get
            {
                return ConfigurationManager.AppSettings["Portal_DealUrlAuthority"];
            }
        }
        #endregion

        #region 通知发送服务相关参数
        /// <summary>
        ///通知发送_失败尝试次数
        ///配置小于等于0时，默认3次
        /// </summary>
        public static int Notify_TryTime
        {
            get
            {
                int temp = 0;
                int.TryParse(ConfigurationManager.AppSettings["Notify_TryTime"], out temp);
                if (temp <= 0) temp = 3;//默认3次
                return temp;
            }
        }
        /// <summary>
        ///通知发送_通知有效时间范围（小时）
        ///即在有效时间范围内的信息才需要发送通知
        ///配置小于等于0时，默认24小时
        /// </summary>
        public static int Notify_HourRange
        {
            get
            {
                int temp = 0;
                int.TryParse(ConfigurationManager.AppSettings["Notify_HourRange"], out temp);
                if (temp <= 0) temp = 24;//默认24小时
                return temp;
            }
        }
        #endregion

        #region 短信平台相关参数
        /// <summary>
        ///短信平台_数据库连接字符串
        /// </summary>
        public static string SMS_Connection
        {
            get
            {
                return ConfigurationManager.AppSettings["SMS_Connection"];
            }
        }
        /// <summary>
        ///短信平台_发送端口
        /// </summary>
        public static string SMS_Port
        {
            get
            {
                return ConfigurationManager.AppSettings["SMS_Port"];
            }
        }
        /// <summary>
        ///短信平台_服务类型
        /// </summary>
        public static string SMS_Type
        {
            get
            {
                return ConfigurationManager.AppSettings["SMS_Type"];
            }
        }
        /// <summary>
        ///短信平台_是否需要状态报告
        /// </summary>
        public static string SMS_State
        {
            get
            {
                return ConfigurationManager.AppSettings["SMS_State"];
            }
        }
        /// <summary>
        ///短信平台_发送短信调用的存储过程
        /// </summary>
        public static string SMS_ProName
        {
            get
            {
                return ConfigurationManager.AppSettings["SMS_ProName"];
            }
        }


        public static string Card
        {
            get
            {
                return ConfigurationManager.AppSettings["Card"];
            }
        }
        #endregion

        /// <summary>
        /// 更新或添加配置文件中的AppSettings节点
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void updateOrAddAppSetting(string key, string value)
        {
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (ConfigurationManager.AppSettings[key] == null)
                cfa.AppSettings.Settings.Add(key, value);
            else
                cfa.AppSettings.Settings[key].Value = value;

            cfa.Save();
            ConfigurationManager.RefreshSection("appSettings");// 刷新命名节，在下次检索它时将从磁盘重新读取它。记住应用程序要刷新节点
        }


        #region 特殊营销方案

        public static string ImportDir
        {
            get
            {
                return ConfigurationManager.AppSettings["ImportDir"];
            }
        }
        public static string MRKTPLANDetail_FTPDiscount
        {
            get
            {
                return ConfigurationManager.AppSettings["MRKTPLANDetail_FTPDiscount"];
            }
        }
        public static string MRKTPLAN_FTPDiscount
        {
            get
            {
                return ConfigurationManager.AppSettings["MRKTPLAN_FTPDiscount"];
            }
        }
        public static string RemoteHost_FTPDiscount
        {
            get
            {
              return  ConfigurationManager.AppSettings["RemoteHost_FTPDiscount"];
            }
        }

        public static int RemotePort_FTPDiscount
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["RemotePort_FTPDiscount"]);
            }
        }
        public static string RemoteUser_FTPDiscount
        {
            get
            {
                return ConfigurationManager.AppSettings["RemoteUser_FTPDiscount"];
            }
        }

        public static string RemotePass_FTPDiscount
        {
            get
            {
                return ConfigurationManager.AppSettings["RemotePass_FTPDiscount"];
            }
        }
        public static string LocalPath_FTPDiscount
        {
            get
            {
                return ConfigurationManager.AppSettings["LocalPath_FTPDiscount"];
            }
        }
        #endregion

        #region 综合平台数据库
        /// <summary>
        /// 综合平台数据库
        /// </summary>
        public static string CZMS_Connection
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;
            }
        }
        #endregion

        #region 考勤数据同步相关参数
        /// <summary>
        /// 一卡通考勤数据库连接字符串
        /// </summary>
        public static string YKT_Connect
        {
            get
            {
                return ConfigurationManager.AppSettings["YKT_Connect"];
            }
        }
        /// <summary>
        /// 指纹考勤数据库连接字符串
        /// </summary>
        public static string SyncHrData_Connect
        {
            get
            {
                return ConfigurationManager.AppSettings["SyncHrData_Connect"];
            }
        }
        /// <summary>
        /// RFSIM考勤数据库连接字符串
        /// </summary>
        public static string RFSIMData_Connect
        {
            get
            {
                return ConfigurationManager.AppSettings["RFSIMData_Connect"];
            }
        }
        /// <summary>
        /// 通宝卡考勤数据库连接字符串
        /// </summary>
        public static string TBKData_Connect
        {
            get
            {
                return ConfigurationManager.AppSettings["TBKData_Connect"];
            }
        }
        /// <summary>
        /// 一次获取的考勤数据量
        /// </summary>
        public static int KQ_GetCount
        {
            get
            {
                int temp = 0;
                int.TryParse(ConfigurationManager.AppSettings["KQ_GetCount"], out temp);
                if (temp <= 0) temp = 10000;//默认10000
                return temp;
            }
        }
        /// <summary>
        /// 是否同步指纹考勤数据
        /// </summary>
        public static bool IsGet_HrData
        {
            get
            {
                if (ConfigurationManager.AppSettings["IsGet_HrData"] == "1")
                    return true;
                else
                    return false;
            }
        }
        #endregion
    }
}
