using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Botwave.Web;
using Botwave.Commons;
using Botwave.Extension.IBatisNet;
using Botwave.Report;
using Botwave.Report.Common;

namespace Botwave.XQP.Commons
{
    /// <summary>
    /// XQP2 辅助类.
    /// </summary>
    public static class XQPHelper
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(XQPHelper));
        /// <summary>
        /// 流程公告实体类型.
        /// </summary>
        public static readonly string EntityType_WorkflowNotice = "workflow_notice";

        /// <summary>
        /// 推送流程列表委托
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="args"></param>
        /// <param name="type"></param>
        private delegate void InvokeService(string url, string method, object[] args, string type);
        /// <summary>
        /// 流程公告弹出页面的路径.
        /// </summary>
        public  static readonly string Url_PopupNotice = WebUtils.GetAppPath() + "apps/xqp2/pages/notices/PopupNotice.aspx?noticeId=";

        /// <summary>
        /// 验证用户是否有访问指定流程资源的权限.
        /// </summary>
        /// <param name="userResources"></param>
        /// <param name="requiredResourceId"></param>
        /// <returns></returns>
        public static bool VerifyAccessResource(IDictionary<string, string> userResources, string requiredResourceId)
        {
            if (string.IsNullOrEmpty(requiredResourceId))
                return true;
            if (userResources == null || userResources.Count == 0)
                return false;

            // 比较资源是否以流程别名结束.是，则表示有访问权限
            foreach (string resource in userResources.Keys)
            {
                if (resource.Equals(requiredResourceId, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        public static void ExportExcel(System.Web.UI.Control provider, string fileName)
        {
            fileName = System.Web.HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8);

            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.Buffer = true;
            System.Web.HttpContext.Current.Response.Charset = "GB2312";
            System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName + ".xls");
            System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
            System.Web.HttpContext.Current.Response.ContentType = "application/ms-excel";

            System.Globalization.CultureInfo myCItrad = new System.Globalization.CultureInfo("ZH-CN", true);
            System.IO.StringWriter oStringWriter = new System.IO.StringWriter(myCItrad);
            System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
            
            provider.RenderControl(oHtmlTextWriter);

            provider = null;
            
            System.Web.HttpContext.Current.Response.Write(oStringWriter.ToString());
            System.Web.HttpContext.Current.Response.Buffer = false;
            System.Web.HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 获取报表列表（所有未删除的）.
        /// </summary>
        /// <param name="creator"></param>
        /// <returns></returns>
        public static IList<ReportEntity> GetReportList(string creator)
        {
            string sql = string.Format("select * from bwrpt_BaseInfo where IsEnabled=1 AND Creator='{0}' order by CreatedTime desc", creator);
            DataSet ds = Botwave.Commons.SqlHelper.ExecuteDataset(Botwave.Commons.SqlHelper.ConnectionString, CommandType.Text, sql);
            IList<ReportEntity> reports = new List<ReportEntity>();
            ReportEntity report = null;
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    report = new ReportEntity();
                    report.Id = DbUtils.ToInt32(row["ID"], 0);
                    report.Name = DbUtils.ToString(row["Name"], "NoName");
                    report.Creator = DbUtils.ToString(row["Creator"], "");
                    report.Extends = DbUtils.ToString(row["Extends"], "");
                    report.ReportSql = DbUtils.ToString(row["ReportSql"], "");
                    report.Remark = DbUtils.ToString(row["Remark"], "");
                    report.TableName = DbUtils.ToString(row["TableName"], "");
                    report.SourceType = DbUtils.ToInt32(row["SourceType"]);
                    report.IsEnabled = DbUtils.ToBoolean(row["IsEnabled"]);
                    report.CreatedTime = (DateTime)row["CreatedTime"];

                    reports.Add(report);
                }
            }

            return reports;
        }

        public static DataTable GetAllDepartments()
        {
            string sql = @"SELECT DpId, DpName, ParentDpId, DpFullName, DpLevel, DeptOrderNo, IsTmpDp, Type, 
                                            CreatedTime, LastModTime, Creator, LastModifier 
                                   FROM vw_bw_Depts_Detail WHERE [Type] =1
                                   ORDER BY OUOrder, DpId";

            return IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        /// <summary>
        /// 禁用用户已有的授权.
        /// </summary>
        /// <param name="fromUserName"></param>
        /// <returns></returns>
        public static int UpdateAuthorizationDisable(string fromUserName)
        {
            string sql = @"update bw_Authorizations set Enabled=0 
                                        where Enabled=1 and FromUserId in(select userid from bw_Users where UserName = '{0}')";
            return IBatisDbHelper.ExecuteNonQuery(CommandType.Text, string.Format(sql, fromUserName));
        }

        /// <summary>
        /// 推送流程列表方法
        /// </summary>
        public static void PushWorkflowList()
        {
            log.Info("Push WorkflowList Begin...");
            try
            {
                string sql = string.Format(@"SELECT WorkflowId,'{0}/ssoproxy.ashx?' as topurl, 'apps/xqp2/pages/workflows/workflowIndex.aspx?wid='+convert(nvarchar(50),WorkflowId) as requrl, bw.WorkflowName, Owner, Enabled, IsCurrent, Version, Creator, Remark,
          LastModifier, CreatedTime, LastModTime, IsDeleted, WorkflowAlias,depts,manager
      FROM vw_bwwf_Workflows_Detail bw left join xqp_workflowsettings s on bw.workflowname=s.workflowname
      WHERE (IsDeleted = 0) and (Enabled = 1) AND (IsCurrent = 1) order by WorkflowAlias asc", System.Configuration.ConfigurationManager.AppSettings["__EF_URL__"]);
                DataTable dt = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
                StringBuilder sb = new StringBuilder();
                sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?><result><sysname>流程易系统</sysname>");
                int index = 0;
                foreach (DataRow dw in dt.Rows)
                {
                    sb.Append("<item>");
                    sb.AppendFormat("<workflowname>{0}</workflowname>", dw["WorkflowName"]);
                    sb.AppendFormat("<remark>{0}</remark>", dw["WorkflowName"]);
                    sb.AppendFormat("<url>{0}</url>", DbUtils.ToString(dw["topurl"]) + System.Web.HttpUtility.UrlEncode(DbUtils.ToString(dw["requrl"])));
                    sb.AppendFormat("<dpnames>{0}</dpnames>", dw["depts"]);
                    sb.AppendFormat("<manager>{0}</manager>", dw["manager"]);
                    sb.AppendFormat("<sort>{0}</sort>", index);
                    sb.Append("</item>");
                    index++;
                }
                sb.Append("</result>");
                string[] arg = new string[1];
                //前三个为鉴权参数，最后为返回的xml
                arg[0] = sb.ToString();
                InvokeService iv = new InvokeService(InvokeWeb);
                iv.BeginInvoke(System.Configuration.ConfigurationManager.AppSettings["__ZH_URL__"], "PushWorkflowList", arg, "", null, null);
            }
            catch (Exception ex)
            {
                log.Error("Push WorkflowList Error..." + ex.ToString());
            }
        }
        /// <summary>
        /// 推送流程列表
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="args"></param>
        /// <param name="type"></param>
        private static void InvokeWeb(string url, string method, object[] args, string type)
        {
            try
            {
                object result = Botwave.XQP.Service.ExternalProxies.WebServicesHelper.InvokeWebService(url, method, args, type);
                log.Info("调用外部接口推送流程列表返回结果：" + Botwave.Commons.DbUtils.ToString(result));
            }
            catch (Exception ex)
            {
                log.Info("调用外部接口推送流程列表返回错误：" + ex.ToString());
            }
        }

        public static string md5(string data)
        {
            byte[] b = System.Text.Encoding.Default.GetBytes(data);
            b = new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
            {
                ret += b[i].ToString("x").PadLeft(2, '0');
            }
            return ret;
        }//end method

        /// <summary> 
        /// 将字符串使用base64算法解密 
        /// </summary> 
        /// <param name="code_type">编码类型</param> 
        /// <param name="code">已用base64算法加密的字符串</param> 
        /// <returns>解密后的字符串</returns> 
        public static string DecodeBase64(string code_type, string code)
        {
            string decode = "";
            
            try
            {
                byte[] bytes = Convert.FromBase64String(code);  //将2进制编码转换为8位无符号整数数组. 
                decode = System.Text.Encoding.GetEncoding(code_type).GetString(bytes);  //将指定字节数组中的一个字节序列解码为一个字符串。 
            }
            catch
            {
                decode = code;
            }
            return decode;
        }

        /// <summary> 
        /// 将字符串使用base64算法解密 
        /// </summary> 
        /// <param name="code_type">编码类型</param> 
        /// <param name="code">已用base64算法加密的字符串</param> 
        /// <returns>解密后的字符串</returns> 
        public static string EncodeBase64(string code_type, string str)
        {
            string encode = "";

            try
            {
                byte[] bytes = System.Text.Encoding.GetEncoding(code_type).GetBytes(str);//将字符串编码为字节数组
                  encode=Convert.ToBase64String(bytes);  //将指定字节数组中的一个字节序列编码码为一个字符串。 
            }
            catch
            {
                encode = str;
            }
            return encode;
        }

        /// <summary>
        /// 格式化当前操作人字符串.
        /// </summary>
        /// <param name="currentActors"></param>
        /// <returns></returns>
        public static string FormatActors(string currentActors)
        {
            if (string.IsNullOrEmpty(currentActors))
                return string.Empty;
            StringBuilder builder = new StringBuilder();
            string[] actors = currentActors.Split(',', '，');
            foreach (string item in actors)
            {
                int index = item.LastIndexOf('/');
                builder.AppendFormat(",{0}", (index > -1 && index < item.Length - 2) ? item.Substring(index + 1) : item);
            }
            if (builder.Length > 0)
                builder = builder.Remove(0, 1);
            return builder.ToString();
        }
    }
}
