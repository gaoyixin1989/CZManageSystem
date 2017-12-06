using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using Botwave.Entities;
using Botwave.Commons;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Extension.Domain;
using Botwave.Workflow.Extension.Service;
using Botwave.DynamicForm.Domain;
using Botwave.DynamicForm.Services;
using Ionic.Zip;

namespace Botwave.XQP.ImportExport
{
    public static class WorkflowExporter
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(WorkflowExporter));

        /// <summary>
        /// 是否抛出附件路径无效的异常错误.
        /// </summary>
        private static readonly bool IsThrowAttachmentInvalid= true;

        private static string AppUrl;

        private static IActivityService activityService = Spring.Context.Support.WebApplicationContext.Current["ActivityService"] as IActivityService;
        private static ITaskAssignService taskAssignService = Spring.Context.Support.WebApplicationContext.Current["taskAssignService"] as ITaskAssignService;
        private static IFormDefinitionService formDefinitionService = Spring.Context.Support.WebApplicationContext.Current["formDefinitionService"] as IFormDefinitionService;
        private static IFormInstanceService formInstanceService = Spring.Context.Support.WebApplicationContext.Current["formInstanceService"] as IFormInstanceService;
        private static IWorkflowUserService workflowUserService = Spring.Context.Support.WebApplicationContext.Current["workflowUserService"] as IWorkflowUserService;

        static WorkflowExporter()
        {
            AppUrl = Botwave.GlobalSettings.Instance.Address;
            if (!AppUrl.EndsWith("/"))
                AppUrl = AppUrl + "/";

            string value = System.Configuration.ConfigurationManager.AppSettings["__WorkflowExporter_ThrowAttachmentInvalid__"];
            if (!string.IsNullOrEmpty(value) && value.Equals("false", StringComparison.OrdinalIgnoreCase))
                IsThrowAttachmentInvalid = false;
        }

        private static bool OnDownload(string directory, DataTable attachments)
        {
            if (string.IsNullOrEmpty(directory) || attachments == null || attachments.Rows.Count == 0)
                return false;
            WebClient downloadClient = new WebClient();
            foreach (DataRow row in attachments.Rows)
            {
                string url = DbUtils.ToString(row["FileName"]);
                string title = DbUtils.ToString(row["title"])+ DbUtils.ToString(row["MimeType"]);

                string localPath = string.Format("{0}/{1}", directory, title);
                if (File.Exists(localPath))
                    continue;
                try
                {
                    downloadClient.DownloadFile(url, localPath);
                }
                catch (Exception ex)
                {
                    downloadClient.Dispose();
                    log.ErrorFormat("下载附件到：{0}，出错：{1}", localPath, ex);
                    if (IsThrowAttachmentInvalid)
                    {
                        throw new Exception(string.Format("附件下载路径无效：{0}", title));
                    }
                }
            }
            downloadClient.Dispose();
            return true;
        }

        private static bool OnCompress(string directory, string saveFileName)
        {
            if (!Directory.Exists(directory))
                return false;
            using (ZipFile zip = new ZipFile(Encoding.UTF8))
            {
                zip.AddDirectory(directory);
                zip.Save(saveFileName);
            }
            return true;
        }

        private static void OnDelete(string directory, string saveFileName)
        {
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, true);
            }
            if (File.Exists(saveFileName))
            {
                File.Delete(saveFileName);
            }
        }

        public static bool ExportZip(HttpContext context, Guid workflowInstanceId, string sheetID, string title)
        {
            if (context == null)
                return false;
            HttpResponse response = context.Response;
            if(string.IsNullOrEmpty(sheetID))
                sheetID = workflowInstanceId.ToString("N");

            try
            {
                // 目录内容.
                string directory = context.Server.MapPath(string.Format("~/App_Data/Temp/{0}", sheetID));
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                DataTable attachments;
                string name = HttpUtility.UrlEncode(title);
                string html = GetHtml(workflowInstanceId, name, out attachments);

                // 导出文档.
                using (StreamWriter writer = new StreamWriter(string.Format("{0}/{1}.doc", directory, title)))
                {
                    writer.Write(html);
                    writer.Close();
                }

                string path = directory + ".zip";

                // 下载附件.
                OnDownload(directory, attachments);

                // 压缩内容.
                OnCompress(directory, path);

                // 输出.
                response.AddHeader("Pragma", "public");
                response.AddHeader("Expires", "0");
                response.AddHeader("Cache-Control", "must-revalidate, post-check=0, pre-check=0");
                response.AddHeader("Content-Type", "application/force-download");
                response.AddHeader("Content-Type", "application/octet-stream");
                response.AddHeader("Content-Type", "application/download");
                response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.zip", name));
                response.AddHeader("Content-Transfer-Encoding", "binary");

                response.WriteFile(path);
                response.Flush();

                // 清除临时文件.
                OnDelete(directory, path);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                response.Write(string.Format("<script>alert('导出出错。{0}。请重试。');window.history.go(-1);</script>", ex.Message));
            }
            response.End();
            return true;
        }

        public static bool ExportWord(HttpResponse response, Guid workflowInstanceId, string title)
        {
            if (response == null)
                return false;

            title = HttpUtility.UrlEncode(title);
            response.Clear();
            try
            {
                DataTable attachments;
                string html = GetHtml(workflowInstanceId, title, out attachments);

                // 输出.
                response.AddHeader("Pragma", "public");
                response.AddHeader("Expires", "0");
                response.AddHeader("Cache-Control", "must-revalidate, post-check=0, pre-check=0");
                response.AddHeader("Content-Type", "application/force-download");
                response.AddHeader("Content-Type", "application/octet-stream");
                response.AddHeader("Content-Type", "application/download");
                response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.doc", title));
                response.AddHeader("Content-Transfer-Encoding", "binary");

                response.Write(html);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                response.Write(string.Format("<script>alert('导入出错。{0}。请重试。');window.history.go(-1);</script>", ex.Message));
            }
            response.End();
            return true;
        }

        private static string GetHtml(Guid workflowInstanceId, string title, out DataTable attachments)
        {
            attachments = null;
            if (workflowInstanceId == Guid.Empty)
                throw new ArgumentException("参数 workflowInstanceId 不能为空值");
            DataTable detail = GetWorkflowDetail(workflowInstanceId);
            if (detail == null || detail.Rows.Count == 0)
            {
                throw new Exception(string.Format("未找到导出工单：{0}，工单名称：{1}", workflowInstanceId, title));
            }
            DataRow detailRow = detail.Rows[0];
            Guid formDefinitionId = DbUtils.ToGuid(detailRow["FormDefinitionId"]).Value;
            FormDefinition formDefinition = formDefinitionService.GetFormDefinitionById(formDefinitionId);
            if (formDefinition == null)
            {
                throw new Exception(string.Format("未找到表单定义：{0}，工单名称：{1}", formDefinitionId, title));
            }
            string templateContent = formDefinition.TemplateContent;
            if (string.IsNullOrEmpty(templateContent))
            {
                throw new Exception(string.Format("未找到表单定义内容：{0}，定时内容为空。工单名称：{1}", formDefinitionId, title));
            }
            // 表单值字典.
            IList<FormItemInstance> formItems = formInstanceService.GetFormItemInstancesByFormInstanceId(workflowInstanceId, true);
            IDictionary<string, string> formValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            if (formItems == null)
            {
                throw new Exception(string.Format("未找到表单项的值：{0}，工单名称：{1}", workflowInstanceId, title));
            }
            else
            {
                foreach (FormItemInstance item in formItems)
                {
                    FormItemDefinition itemDefinition = item.Definition;
                    if (itemDefinition == null)
                        continue;
                    string key = itemDefinition.FName;
                    string value = itemDefinition.ItemDataType == FormItemDefinition.DataType.Decimal ?
                        item.DecimalValue.ToString() : (string.IsNullOrEmpty(item.TextValue) ? item.Value : item.TextValue);
                    value = HttpUtility.HtmlEncode(value);
                    if (!string.IsNullOrEmpty(value))
                        value = value.Replace("\r\n", "<br/>");
                    formValues[key] = value;
                }
            }

            StringBuilder result = new StringBuilder();
            result.AppendLine("<html>");
            result.AppendLine("<head>");
            result.AppendLine("<style type=\"text/css\">");
            result.AppendLine(@"table {margin:0px;border-collapse:collapse;border:solid 1px #000000;}
body ,table th, table td{font-family:Microsoft YaHei, Arial; font-size:9pt;font-weight:normal;}
table th, table td{border:solid 1px #000;}
table th{background-color:#F4F4F4; text-align:right;}");
            result.AppendLine("</style>");
            result.AppendLine("</head>");
            result.AppendLine("<body>");
            result.AppendLine("<div style=\"border-bottom:1px solid #C0CEDF;color:#09c;font-weight:bold; margin-bottom:8px;\">基本信息</div>");
            result.AppendLine("<table cellpadding=\"3\" cellspacing=\"0\" border=\"0\" style=\"margin:0;\" width=\"100%\">");
            result.AppendLine(BuildHeader(workflowInstanceId, detailRow));
            result.AppendLine("</table>");

            // 表单信息.
            result.AppendLine("<div style=\"border-bottom:1px solid #C0CEDF;color:#09c;font-weight:bold; margin-top:8px;margin-bottom:8px;\">详细信息</div>");
            result.AppendLine(BuildTemplate(workflowInstanceId, formDefinitionId, StringUtils.HtmlDecode(templateContent), formValues));

            // 附件信息.
            result.AppendLine("<div style=\"border-bottom:1px solid #C0CEDF;color:#09c;font-weight:bold; margin-top:8px;margin-bottom:8px;\">附件信息</div>");
            result.AppendLine("<table cellpadding=\"3\" cellspacing=\"0\" border=\"0\" style=\"margin:0;\" width=\"100%\">");
            result.AppendLine(BuildAttachments(workflowInstanceId, string.Empty, out attachments));
            result.AppendLine("</table>");

            // 处理信息.
            result.AppendLine("<div style=\"border-bottom:1px solid #C0CEDF;color:#09c;font-weight:bold; margin-top:8px;margin-bottom:8px;\">处理信息</div>");
            result.AppendLine("<table cellpadding=\"3\" cellspacing=\"0\" border=\"0\" style=\"margin:0;\" width=\"100%\">");
            result.AppendLine(BuildProcessHistory(workflowInstanceId));
            result.AppendLine("</table>");

            result.AppendLine("</body></html>");

            return result.ToString();
        }

        private static string BuildHeader(Guid workflowInstanceId, DataRow detail)
        {
            if (detail == null)
                return null;

            StringBuilder result = new StringBuilder();
            result.AppendFormat(@"<tr>
            <th width=""13%"">标   题:</th>
            <td colspan=""3"">{0}</td>
            <th width=""13%"">受理号:</th>
            <td width=""20%"">{1}</td>
        </tr>
        {2}
        <tr><th width=""13%"">发起人:</th><td width=""20%"">{3}</td>
            <th width=""13%"">联系电话:</th><td width=""20%"">{4}</td>
            <th width=""13%"">创建时间:</th><td width=""20%"">{5:yyyy-MM-dd HH:mm:ss}</td>
        </tr>{6}
        <tr><th width=""13%"">当前流程:</th><td colspan=""5"" style=""font-weight:bold"">{7}</td>
        </tr>", detail["工单标题"], detail["工单号"],
              GetWorkflowFields(detail),
             detail["发起人"], detail["联系电话"], detail["发起时间"],
             GetWorkflowCurrentActivities(workflowInstanceId, DbUtils.ToInt32(detail["状态"])),
            detail["WorkflowName"]);

            return result.ToString();
        }
        
        private static string BuildTemplate(Guid workflowInstanceId, Guid formDefinitionId, string templateContent, IDictionary<string, string> values)
        {
            if (string.IsNullOrEmpty(templateContent))
                return null;
            // 格式化内容.
            templateContent = templateContent.ToLower();
            //int index = templateContent.LastIndexOf("</table>");
            //if (index > 0)
            //    templateContent = templateContent.Substring(0, index + 8)
            string message;
            templateContent = Sgml.XHtmlHelper.ToXHtml("<div>" + templateContent + "</div>", out message);
            if (!string.IsNullOrEmpty(message))
            {
                log.ErrorFormat("转化表单：{0} 为 XHtml 出错：{1}", formDefinitionId, message);
            }
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(templateContent);

                // 移除所有 script 元素.
                XmlNodeList childs = doc.DocumentElement.SelectNodes("//script");
                foreach (XmlNode child in childs)
                {
                    child.ParentNode.RemoveChild(child);
                }

                // 所有展开/收起 div 节点.
                childs = doc.DocumentElement.SelectNodes("//div[@class=\"showcontrol\"]");
                foreach (XmlNode child in childs)
                {
                    XmlNode h4 = child.SelectSingleNode("h4");
                    child.RemoveAll();
                    child.Attributes.Append(CreateAttribute(doc, "style", "border-bottom:1px solid #C0CEDF;color:#09c;font-weight:bold; margin-top:8px;margin-bottom:8px;"));
                    child.InnerText = h4 == null ? string.Empty : h4.InnerText;
                }

                childs = doc.DocumentElement.SelectNodes("//table"); // 所有 table 元素.
                foreach (XmlNode child in childs)
                {
                    GetFormatTable(doc, child, values, false);
                }
                return doc.OuterXml;
            }
            catch (Exception ex)
            {
                log.ErrorFormat("转化表单：{0} Xml 读取 出错：{1}", formDefinitionId, ex);
                throw ex;
            }
        }

        private static string GetFormatTable(XmlDocument doc, XmlNode tableElement, IDictionary<string, string> values, bool isOuter)
        {
            if (tableElement == null)
                return null;
            // table
            tableElement.Attributes.RemoveNamedItem("class");
            tableElement.Attributes.RemoveNamedItem("style");
            tableElement.Attributes.Append(CreateAttribute(doc, "style", "word-break: break-all;"));
            if (tableElement.Attributes["width"] == null)
                tableElement.Attributes.Append(CreateAttribute(doc, "width", "100%"));

            // tr
            XmlNodeList rowNodes = tableElement.SelectNodes("//tr");
            foreach (XmlElement rowNode in rowNodes)
            {
                rowNode.RemoveAllAttributes();
            }

            // th
            XmlNodeList headNodes = tableElement.SelectNodes("//th");
            foreach (XmlElement headNode in headNodes)
            {
                headNode.RemoveAttribute("style");
                headNode.RemoveAttribute("width");
                headNode.Attributes.Append(CreateAttribute(doc, "width", "13%"));
            }

            // td
            XmlNodeList dataNodes = tableElement.SelectNodes("//td");
            foreach (XmlElement dataNode in dataNodes)
            {
                string dataValue = null;
                // 绑定表单项的值.
                if (dataNode.HasChildNodes)
                {
                    foreach (XmlNode child in dataNode.ChildNodes)
                    {
                        string childName = child.Name;
                        if (childName == "input" || childName == "select" || childName == "textarea")
                        {
                            string dataKey = child.Attributes["id"] == null ? null : child.Attributes["id"].Value;
                            dataKey = dataKey == null ? string.Empty : dataKey.Trim();
                            if (string.IsNullOrEmpty(dataKey))
                                continue;

                            int index = dataKey.LastIndexOf("_");
                            dataKey = index > 0 ? dataKey.Substring(index + 1) : dataKey;
                            dataValue = values.ContainsKey(dataKey) ? values[dataKey] : string.Empty;
                        }
                    }
                }
                if (dataValue != null)
                    dataNode.InnerXml = dataValue;
                else
                    dataNode.InnerXml = string.Empty ;
            }
            return (isOuter ? tableElement.OuterXml : tableElement.InnerXml);
        }

        private static DataTable GetWorkflowDetail(Guid workflowInstanceId)
        {
            string sql = string.Format(@"select WorkflowId, WorkflowInstanceId, FormDefinitionId,  WorkflowName, BasicFields, 工单标题, 工单号, 发起人, 联系电话, 发起时间, 期望完成时间, 保密级别, 紧急程度, 重要级别, 期望完成时间, 状态 
                        from vw_xqp_Tracking_Workflows_Detail where WorkflowInstanceId='{0}'", workflowInstanceId);

            return Botwave.Extension.IBatisNet.IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        private static string GetWorkflowFields(DataRow row)
        {
            string basicFields = DbUtils.ToString(row["BasicFields"]);
            if (string.IsNullOrEmpty(basicFields) || basicFields.Length < 4)
                basicFields = basicFields + "1111";
            // 字段数计算.
            int count = 0;
            int index = 0;
            while (index < 4)
            {
                if (basicFields[index] == '1')
                    count++;
                index++;
            }
            if (count == 0)
                return null;
            string[] names = new string[] { "期望完成时间", "保密设置", "紧急程度", "重要级别" };
            string[] values = new string[] { string.Format("{0:yyyy-MM-dd HH:mm:ss}", row["期望完成时间"]), DbUtils.ToString(row["保密级别"]), DbUtils.ToString(row["紧急程度"]), DbUtils.ToString(row["重要级别"]) };
            StringBuilder result = new StringBuilder();
            result.AppendLine("<tr>");
            switch (count)
            {
                case 1:
                    index = basicFields.IndexOf('1');
                    result.AppendFormat("<th width=\"13%\">{0}:</th><td colspan=\"5\">{1}</td>", names[index], values[index]);
                    break;
                case 2:
                    index = basicFields.IndexOf('1');
                    result.AppendFormat("<th width=\"13%\">{0}:</th><td width=\"20%\">{1}</td>", names[index], values[index]);
                    index = basicFields.IndexOf('1', index + 1);
                    result.AppendFormat("<th width=\"13%\">{0}:</th><td colspan=\"3\">{1}</td>", names[index], values[index]);
                    break;
                case 3:
                    index = basicFields.IndexOf('1');
                    result.AppendFormat("<th width=\"13%\">{0}:</th><td width=\"20%\">{1}</td>", names[index], values[index]);
                    index = basicFields.IndexOf('1', index + 1);
                    result.AppendFormat("<th width=\"13%\">{0}:</th><td width=\"20%\">{1}</td>", names[index], values[index]);
                    index = basicFields.IndexOf('1', index + 1);
                    result.AppendFormat("<th width=\"13%\">{0}:</th><td width=\"20%\">{1}</td>", names[index], values[index]);
                    break;
                case 4:
                    result.AppendFormat("<th width=\"13%\">{0}:</th><td width=\"20%\">{1}</td>", names[0], values[0]);
                    result.AppendFormat("<th width=\"13%\">{0}:</th><td colspan=\"3\">{1}</td>", names[1], values[1]);
                    result.AppendLine("</tr><tr>");
                    result.AppendFormat("<th width=\"13%\">{0}:</th><td width=\"20%\">{1}</td>", names[2], values[2]);
                    result.AppendFormat("<th width=\"13%\">{0}:</th><td colspan=\"3\">{1}</td>", names[3], values[3]);
                    break;
            }
            result.AppendLine("</tr>");
            return result.ToString();
        }

        private static string GetWorkflowCurrentActivities(Guid workflowInstanceId, int state)
        {
            //获取当前活动列表
            IList<ActivityInstance> currentActivities = activityService.GetCurrentActivities(workflowInstanceId);
            ActivityInstance currentActivityInstance = null;
            StringBuilder currentNames = new StringBuilder();
            StringBuilder currentActors = new StringBuilder();
            if (currentActivities != null && currentActivities.Count > 0)
            {
                IDictionary<string, Guid> dictActors = new Dictionary<string, Guid>();
                foreach (ActivityInstance activityInstance in currentActivities)
                {
                    Guid currentActivityInstanceId = activityInstance.ActivityInstanceId;
                    currentActivityInstance = activityService.GetActivity(currentActivityInstanceId);
                    string activityActor = currentActivityInstance.Actor;
                    if (currentNames.ToString().IndexOf(currentActivityInstance.ActivityName) == -1)
                        currentNames.Append("," + currentActivityInstance.ActivityName);

                    if (string.IsNullOrEmpty(activityActor))
                    {
                        IList<BasicUser> users = taskAssignService.GetTodoActors(currentActivityInstanceId);
                        if (users == null || users.Count == 0)
                            continue;
                        foreach (BasicUser item in users)
                        {
                            if (!dictActors.ContainsKey(item.UserName))
                            {
                                dictActors.Add(item.UserName, currentActivityInstanceId);
                                currentActors.AppendFormat(",<span tooltip=\"{0}\">{1}</span>", item.UserName, item.RealName);
                            }
                        }
                    }
                    else
                    {
                        if (!dictActors.ContainsKey(activityActor))
                        {
                            dictActors.Add(activityActor, currentActivityInstanceId);
                            ActorDetail actorUser = workflowUserService.GetActorDetail(activityActor);
                            currentActors.AppendFormat(",<span tooltip=\"{0}\">{1}</span>", actorUser.UserName, actorUser.RealName);
                        }
                    }
                }
            }

            if (currentNames.Length > 0)
                currentNames = currentNames.Remove(0, 1);
            if (currentActors.Length > 0)
                currentActors = currentActors.Remove(0, 1);

            if (currentNames.Length == 0 && state == 2)
                currentNames = new StringBuilder("完成");
            else if (state == 99)
                currentNames = new StringBuilder("取消");

            return string.Format(@"<tr><th width=""13%"">当前步骤:</th>
            <td width=""20%"">{0}</td>
            <th width=""13%"">当前处理人:</th>
            <td colspan=""3"">{1}</td></tr>", currentNames, currentActors);
        }

        private static XmlAttribute CreateAttribute(XmlDocument doc, string name, string value)
        {
            if (doc == null || string.IsNullOrEmpty(name))
                return null;
            XmlAttribute attribute = doc.CreateAttribute(name);
            attribute.Value = value;
            return attribute;
        }

        private static string BuildAttachments(Guid workflowInstanceId, string actor, out DataTable attachments)
        {
            attachments = Botwave.XQP.Domain.CZWorkflowRelation.GetRelationAttachments(workflowInstanceId, actor, false);
            if (attachments == null || attachments.Rows.Count == 0)
                return null;
            StringBuilder result = new StringBuilder();
            result.AppendLine(@"<tr><th style=""text-align:center;"">附件名称</th>
                <th style=""text-align:center;"" width=""100px"">上传人</th><th style=""text-align:center;"" width=""130px"">上传时间</th>
                <th style=""text-align:center;"" width=""80px"">附件大小</th><th style=""text-align:center;"" width=""80px"">来源</th></tr>");
            int count = attachments.Rows.Count;
            for (int i = 0; i < count; i++)
            {
                DataRow row = attachments.Rows[i];
                bool isRefAttachment = Botwave.Commons.DbUtils.ToBoolean(row["IsRef"]);

                result.AppendFormat("<tr><td align=\"left\"><a href=\"{0}contrib/workflow/pages/download.ashx?id={1}\">{2}{3}</a></td>", AppUrl, row["ID"], row["Title"], row["MimeType"]);
                result.AppendFormat("<td>{0}</td>", row["RealName"]);
                result.AppendFormat("<td style=\"text-align:center;\">{0:yyyy-MM-dd HH:mm:ss}</td>", row["CreatedTime"]);
                result.AppendFormat("<td style=\"text-align:right;\">{0}</td>", GetFormatFileSize(DbUtils.ToInt32(row["FileSize"])));
                result.AppendFormat("<td style=\"text-align:center;\">{0}</td></tr>", isRefAttachment ? "引用附件" : "用户上传");
            }
            return result.ToString();
        }

        private static string BuildProcessHistory(Guid workflowInstanceId)
        {
            ProcessHistoryResult history = new ProcessHistoryResult();
            history.Initialize(workflowInstanceId);
            StringBuilder result = new StringBuilder();
            result.AppendLine(@"<tr><th style=""text-align:center;"" width=""100px"">步骤</th>
		        <th style=""text-align:center;"" colspan=""2"" width=""200px"">处理人</th>
		        <th style=""text-align:center;"" width=""130px"">执行时间</th>
		        <th style=""text-align:center;"" width=""80px"">处理意见</th></tr>");

            string value = history.Result;
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Replace("<th>转交人</th><th>被转交人</th><th>转交时间</th><th>转交信息</th></tr>",
                    "<th style=\"text-align:center;\" width=\"80px\">转交人</th><th style=\"text-align:center;\" width=\"80px\">被转交人</th><th style=\"text-align:center;\">转交时间</th><th style=\"text-align:center;\">转交信息</th></tr>");
            }
            result.AppendLine(value);
            return result.ToString();
        }

        private static string GetFormatFileSize(long size)
        {
            string result = "";
            if (size < 1024)
                result = size.ToString("F2") + " Byte";
            else if (size >= 1024.00 && size < 1048576)
                result = (size / 1024.00).ToString("F2") + " K";
            else if (size >= 1048576 && size < 1073741824)
                result = (size / 1024.00 / 1024.00).ToString("F2") + " M";
            else if (size >= 1073741824)
                result = (size / 1024.00 / 1024.00 / 1024.00).ToString("F2") + " G";
            return result;
        }

        internal class ProcessHistoryResult : Botwave.XQP.Web.Controls.WorkflowProcessHistory
        {
            public ProcessHistoryResult()
            { }

            public string Result { get; set; }

            protected override void DataBind(string historyHtml)
            {
                this.Result = historyHtml;
            }
        }
    }
}
