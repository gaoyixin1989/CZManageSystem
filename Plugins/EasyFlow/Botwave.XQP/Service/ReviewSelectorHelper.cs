using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using Botwave.Entities;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow;
using Botwave.XQP.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Domain;

namespace Botwave.XQP.Service
{
    /// <summary>
    /// 抄送选择器服务类.
    /// </summary>
    public sealed class ReviewSelectorHelper
    {
        /// <summary>
        /// 抄送人的控件变量名称.
        /// </summary>
        public static readonly string ReviewAcotrs_ControlID = "reviewActors_values";

        /// <summary>
        /// 获取当前流程的抄送类型.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        public static ReviewType GeteviewType(Guid workflowId)
        {
            WorkflowProfile workflowProfile = WorkflowProfile.LoadByWorkflowId(workflowId);
            return GeteviewType(workflowProfile);
        }

        /// <summary>
        /// 获取当前流程的抄送类型.
        /// </summary>
        /// <param name="workflowProfile"></param>
        /// <returns></returns>
        public static ReviewType GeteviewType(WorkflowProfile workflowProfile)
        {
            if (workflowProfile == null || workflowProfile.IsReview == false)
                return ReviewType.None;

            return (workflowProfile.IsClassicReviewType ? ReviewType.Classic : ReviewType.CheckBox);
        }

        /// <summary>
        /// 解析抄送人的数据值.
        /// </summary>
        /// <param name="reviewValue"></param>
        /// <param name="selectedActivities"></param>
        /// <returns></returns>
        public static IList<ToReview.ReviewActor> ParserReiewActors(string reviewValue, ICollection<Guid> selectedActivities)
        {
            return ParserReiewActors(reviewValue, Guid.Empty, selectedActivities);
        }

        /// <summary>
        /// 解析抄送人的数据值.
        /// </summary>
        /// <param name="reviewValue"></param>
        /// <param name="defaultActivityInstanceId"></param>
        /// <param name="selectedActivities"></param>
        /// <returns></returns>
        public static IList<ToReview.ReviewActor> ParserReiewActors(string reviewValue, Guid defaultActivityInstanceId, ICollection<Guid> selectedActivities)
        {
            if (string.IsNullOrEmpty(reviewValue))
                return new List<ToReview.ReviewActor>();
            selectedActivities = (selectedActivities == null ? new List<Guid>() : selectedActivities);
            IList<ToReview.ReviewActor> actors = new List<ToReview.ReviewActor>();
            IList<string> freeActors = new List<string>();  // 自由抄送人，即未指定抄送步骤.

            string[] values = reviewValue.Split(',', '，');
            foreach (string value in values)
            {
                string[] data = value.Split('$');
                if (data == null || data.Length == 0)
                    continue;
                string actor = data[0].Trim();
                Guid? activityId = Botwave.Commons.DbUtils.ToGuid(data.Length == 2 ? data[1].Trim() : string.Empty);
                if (activityId.HasValue && !selectedActivities.Contains(activityId.Value))
                    continue;
                if (!activityId.HasValue)
                {
                    freeActors.Add(actor);
                }
                else
                {
                    actors.Add(new ToReview.ReviewActor(activityId.Value, defaultActivityInstanceId, actor));
                }
            }
            // 对未选择步骤的抄送人，每个步骤都发送一份待阅.
            if (freeActors.Count > 0 && selectedActivities.Count > 0)
            {
                foreach (Guid activityId in selectedActivities)
                {
                    foreach (string actor in freeActors)
                    {
                        actors.Add(new ToReview.ReviewActor(activityId, defaultActivityInstanceId, actor));
                    }
                }
            }
            return actors;
        }

        #region classic

        /// <summary>
        /// 旧有方式的抄送显示 HTML.
        /// </summary>
        /// <returns></returns>
        public static string BuildClassicHtml()
        {
            return BuildClassicHtml(ReviewAcotrs_ControlID);
        }

        /// <summary>
        /// 旧有方式的抄送显示 HTML.
        /// </summary>
        /// <param name="actorsControlID"></param>
        /// <returns></returns>
        public static string BuildClassicHtml(string actorsControlID)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("<div class=\"dataTable\" id=\"divReadersContainer\">");

            builder.AppendLine("<table class=\"tblGrayClass grayBackTable\" cellpadding=\"4\" cellspacing=\"1\" style=\"margin-top: 6px;\">");
            builder.AppendLine("<tr><td style=\"padding-left:120px\">");

            builder.AppendLine("<div style=\"font-size:14px; font-weight:bold; padding-top:3px; padding-bottom:3px\">抄送人列表</div>");
            builder.AppendLine("<div>");
            builder.AppendLine("<input type=\"text\" style=\"width:360px\" id=\"txtDisplyReviewActors\" readonly=\"readonly\" />");
            builder.AppendLine("<a href=\"javascript:void(0);\" onclick=\"return onOpenReviewPicker();\" style=\"font-weight:bold;\" title=\"选择抄送人\">选择抄送人</a> -");
            builder.AppendLine("<a href=\"javascript:void(0);\" onclick=\"return onClearReviews();\" style=\"font-weight:bold;\" title=\"清除已选择的抄送人\">清除已选择</a>");

            builder.AppendFormat("<input type=\"hidden\" id=\"{0}\" name=\"{0}\" />", actorsControlID);

            builder.AppendLine("</div>");

            builder.AppendLine("</td></tr>");
            builder.AppendLine("</table>");

            builder.AppendLine("</div>");

            builder.AppendLine(BuildClassicScripts(actorsControlID));

            return builder.ToString();
        }

        private static string BuildClassicScripts(string actorsControlID)
        {
            return BuildClassicScripts(Botwave.Web.WebUtils.GetAppPath(), actorsControlID);
        }

        private static string BuildClassicScripts(string appPath, string actorsControlID)
        {
            if (!appPath.EndsWith("/"))
                appPath = appPath + "/";
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<script type=\"text/javascript\">");

            builder.AppendLine(@"<!--//
                function onOpenReviewPicker(){
                    var h = 450;
                    var w = 700;
                    var iTop = (window.screen.availHeight-30-h)/2;    
                    var iLeft = (window.screen.availWidth-10-w)/2; 
                    window.open('" + appPath + @"contrib/security/pages/popupUserPicker2.aspx?func=onCompletePickReviews', '', 'height='+ h +', width='+ w+', top='+ iTop +', left='+ iLeft +', toolbar=no, menubar=no, scrollbars=yes, resizable=no,location=no, status=no');	
                    return false;
                }");

            builder.AppendLine(@"
                function onClearReviews(){
                    if(confirm(""确定要清除已选择的抄送人？"")){
                        $(""#" + actorsControlID + @""").val("""");
                        $(""#txtDisplyReviewActors"").val("""");
                    }
                    return true;
                }");

            builder.AppendLine(@"
                function onCompletePickReviews(result){
                    var values = $(""#" + actorsControlID + @""").val();
                    var names = $(""#txtDisplyReviewActors"").val();
                    for(var i=0; i<result.length;i++){
                        values += ("","" + result[i].key);
                        names += ("","" + result[i].value);
                    }
                    if(values.substring(0, 1) == "","")
                        values = values.substring(1, values.length);
                    if(names.substring(0, 1) == "","")
                        names = names.substring(1, names.length);
                    $(""#" + actorsControlID + @""").val(values);
                    $(""#txtDisplyReviewActors"").val(names);
                }");

            builder.AppendLine("//--></script>");
            return builder.ToString();
        }

        #endregion

        #region activity profile

        /// <summary>
        /// 生成复选框形式的抄送 HTML.
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        public static string BuildProfileItemHtml(ActivityProfile profile)
        {
            return BuildProfileItemHtml(profile, ReviewAcotrs_ControlID);
        }

        /// <summary>
        /// 生成复选框形式的抄送 HTML.
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="rowRepeatCount"></param>
        /// <returns></returns>
        public static string BuildProfileItemHtml(ActivityProfile profile, int rowRepeatCount)
        {
            return BuildProfileItemHtml(profile, ReviewAcotrs_ControlID, rowRepeatCount);
        }

        /// <summary>
        /// 生成复选框形式的抄送 HTML.
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="actorsControlID"></param>
        /// <returns></returns>
        public static string BuildProfileItemHtml(ActivityProfile profile, string actorsControlID)
        {
            return BuildProfileItemHtml(profile, actorsControlID, 8);
        }

        /// <summary>
        /// 生成复选框形式的抄送 HTML.
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="actorsControlID"></param>
        /// <param name="rowRepeatCount"></param>
        /// <returns></returns>
        public static string BuildProfileItemHtml(ActivityProfile profile, string actorsControlID, int rowRepeatCount)
        {
            if (profile == null || profile.IsReview == false || string.IsNullOrEmpty(profile.ReviewActors))
                return string.Empty;

            string[] actorArray = profile.ReviewActors.Split(',', '，');
            NameValueCollection actors = GetActorNames(actorArray);
            if (actors == null || actors.Count == 0)
                return string.Empty;

            int count = actors.Count;
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<div style=\"margin-top:8px\">");
            builder.AppendLine("<div><span style=\"font-weight:bold;\">抄送列表</span></div>");
            for (int i = 0; i < count; i++)
            {
                if (i > 0 && i % rowRepeatCount == 0)
                    builder.Append("<br />");
                string userName = actors.GetKey(i);
                string realName = actors[i];
                if (string.IsNullOrEmpty(userName))
                    continue;
                realName = (string.IsNullOrEmpty(realName) ? userName : realName);
                builder.AppendFormat("<input type=\"checkbox\" id=\"{0}_{1}\" name=\"{0}\" value=\"{2}${4}\" checked=\"checked\"><label for=\"{0}_{1}\">{3}</label>", actorsControlID, i, userName, realName, profile.ActivityId);
            }
            builder.AppendLine("</div>");
            return builder.ToString();
        }

        /// <summary>
        /// 生成复选框形式的抄送 HTML.
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="actorsControlID"></param>
        /// <param name="rowRepeatCount"></param>
        /// <returns></returns>
        public static string BuildProfileItemHtml(ActivityProfile profile, Guid workflowInstanceId, string currentActor)
        {
            //if (profile == null || profile.IsReview == false || string.IsNullOrEmpty(profile.ReviewActors))
            if (profile == null || profile.IsReview == false)
                return string.Empty;
            int rowRepeatCount=8;
            AllocatorOption options= new AllocatorOption();
            string[] actorArray = profile.ReviewActors.Split(',', '，');
            NameValueCollection actors = GetActorNames(actorArray);
            IActivityAllocationService activityAllocationService = (Spring.Context.Support.WebApplicationContext.Current["activityAllocationService"] as IActivityAllocationService);
            IActivityService activityService = (Spring.Context.Support.WebApplicationContext.Current["activityService"] as IActivityService);
            if (!string.IsNullOrEmpty(profile.ExtendAllocators))
            {
                if (profile.ExtendAllocators.Contains("activity"))
                {
                    if (!string.IsNullOrEmpty(profile.ExtendAllocatorArgs))//增加过程控制，解决个性化定制权限角色(针对组织控制类型和历史处理人类型)
                    {
                        foreach (string allocatorArg in profile.ExtendAllocatorArgs.Replace(" ", "").Split(';', '；'))
                        {
                            string[] ocatorArray = allocatorArg.Split(':', '：');
                            int lengthOfAllocatorArray = ocatorArray.Length;
                            if (lengthOfAllocatorArray == 0)
                                continue;
                            if (ocatorArray[0] == "activity")
                            {
                                string sql = string.Format(@"select actor from bwwf_tracking_activities_completed ta
                    inner join bwwf_activities ba 
                    on ta.activityid = ba.activityid 
                    where workflowInstanceId = '{0}' and activityname = '{1}'", workflowInstanceId.ToString().ToUpper(), ocatorArray[1]);
                                object actor = IBatisDbHelper.ExecuteScalar(CommandType.Text, sql);
                                currentActor = string.IsNullOrEmpty(Botwave.Commons.DbUtils.ToString(actor)) ? currentActor : actor.ToString();
                                break;
                            }
                        }
                    }
                }
                options.ExtendAllocators = profile.ExtendAllocators;
                options.ExtendAllocatorArgs = profile.ExtendAllocatorArgs;
            }
            IDictionary<string, string> dict = activityAllocationService.GetTargetUsers(workflowInstanceId, options, currentActor, true);
            IList<BasicUser> actorDetails;
            IDictionary<string, string> superiorActors = WorkflowProfileHelper.GetActorNames(dict.Keys, out actorDetails);
            foreach (KeyValuePair<string, string> pair in superiorActors)
            {
                if (actors[pair.Key] == null)
                    actors[pair.Key] = pair.Value;
            }
            if (actors == null || actors.Count == 0)
                return string.Empty;

            int count = actors.Count;
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<div style=\"margin-top:8px\">");
            builder.AppendLine("<div><span style=\"font-weight:bold;\">抄送列表</span></div>");
            for (int i = 0; i < count; i++)
            {
                if (i > 0 && i % rowRepeatCount == 0)
                    builder.Append("<br />");
                string userName = actors.GetKey(i);
                string realName = actors[i];
                if (string.IsNullOrEmpty(userName))
                    continue;
                realName = (string.IsNullOrEmpty(realName) ? userName : realName);
                builder.AppendFormat(" <input type=\"checkbox\" id=\"{0}_{1}\" name=\"{0}\" value=\"{2}${4}\" checked=\"checked\"><span tooltip=\"{2}\"><label for=\"{0}_{1}\">{3}</label></span>", ReviewAcotrs_ControlID, i, userName, realName, profile.ActivityId);
            }
            builder.AppendLine("</div>");
            return builder.ToString();
        }

        /// <summary>
        /// 获取用户真实姓名.
        /// </summary>
        /// <param name="actors"></param>
        /// <returns></returns>
        private static NameValueCollection GetActorNames(string[] actors)
        {
            NameValueCollection result = new NameValueCollection(StringComparer.OrdinalIgnoreCase);
            // 获取可用的用户列表.
            StringBuilder where = new StringBuilder();
            foreach (string actor in actors)
            {
                where.AppendFormat("'{0}',", actor.Trim());
            }
            where.Length = where.Length - 1;

            string sql = string.Format("SELECT [UserName], [RealName] FROM bw_Users WHERE UserName IN ({0}) Order By SortOrder, [Type], UserName", where);
            DataTable resultTable = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
            if (resultTable == null || resultTable.Rows.Count == 0)
                return result;
            foreach (DataRow row in resultTable.Rows)
            {
                string actor = Botwave.Commons.DbUtils.ToString(row[0]);
                string actorName = Botwave.Commons.DbUtils.ToString(row[1]);
                result[actor] = actorName;
            }
            return result;
        }
        #endregion


        /// <summary>
        /// 生成验证抄送人数的脚本.
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="activityProfile"></param>
        /// <param name="visible"></param>
        /// <returns></returns>
        public static string BuildValidateScript(WorkflowProfile profile, bool visible)
        {
            return BuildValidateScript((profile != null && profile.IsReview), visible, ReviewAcotrs_ControlID);
        }

        /// <summary>
        /// 生成验证抄送人数的脚本(只是 Javascript 方法体内容).
        /// </summary>
        /// <param name="isReview">是否支持抄送设置.</param>
        /// <param name="visible">是否可见(被隐藏或不允许抄送则为 false).</param>
        /// <param name="controlName"></param>
        /// <returns></returns>
        public static string BuildValidateScript(bool isReview, bool visible, string controlName)
        {
            return BuildValidateScript(isReview, visible, controlName, "validateReviewActors");
        }

        /// <summary>
        /// 生成验证抄送人数的脚本(只是 Javascript 方法体内容).
        /// </summary>
        /// <param name="isReview">是否支持抄送设置.</param>
        /// <param name="visible">是否可见(被隐藏或不允许抄送则为 false).</param>
        /// <param name="controlName"></param>
        /// <param name="functionName"></param>
        /// <returns></returns>
        public static string BuildValidateScript(bool isReview, bool visible, string controlName, string functionName)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("function " + functionName + "(max){");
            if (isReview == false || visible == false)
            {
                builder.AppendLine("     return true;");
            }
            else
            {
                builder.AppendLine("     if(max<=-1){ return true; }");
                builder.AppendLine("     var type =  $(\"input[name='" + controlName + "']\").attr(\"type\");");
                builder.AppendLine("     if(max == 0){");
                builder.AppendLine("         var checkCount=0;");
                builder.AppendLine("         if(type == \"checkbox\"){");
                builder.AppendLine("             $(\"input[name='" + controlName + "']\").each(function(){");
                builder.AppendLine("                 if(this.checked){ checkCount++; }");
                builder.AppendLine("             });");
                builder.AppendLine("         }");
                builder.AppendLine("         if(checkCount>0 && checkCount < $(\"input[name='" + controlName + "']\").length){");
                builder.AppendLine("             alert(\"错误：必须选择全部的抄送人或者全部不选！\")");
                builder.AppendLine("             return false;");
                builder.AppendLine("         }");
                builder.AppendLine("     }else{");
                builder.AppendLine("         var values = \"\";");
                builder.AppendLine("         if(type == \"checkbox\"){");
                builder.AppendLine("             $(\"input[name='" + controlName + "']\").each(function(){");
                builder.AppendLine("                 if(this.checked){ values+=(values == \"\" ? ($(this).val()) : (\",\" + $(this).val())); }");
                builder.AppendLine("             });");
                builder.AppendLine("         }else{");
                builder.AppendLine("             values = $(\"input[name='" + controlName + "']\").val();");
                builder.AppendLine("         }");
                builder.AppendLine("         var count = (values == \"\"? 0 : values.split(\",\").length);");
                builder.AppendLine("         if(count > max){");
                builder.AppendLine("             alert(\"错误：选择的抄送人过多，抄送人数不能超过 \" + max + \" 人。请重新选择！\");");
                builder.AppendLine("             return false;");
                builder.AppendLine("         }");
                builder.AppendLine("     }");
                builder.AppendLine("     return true;");

                //int count = reviewActorCount;
                //if (count == 0)
                //{
                //}
                //else
                //{
                //    builder.AppendLine("var values = \"\";");
                //    builder.AppendLine("var type =  $(\"input[name='" + controlName + "']\").attr(\"type\");");
                //    builder.AppendLine("if(type == \"checkbox\"){");
                //    builder.AppendLine("     $(\"input[name='" + controlName + "']\").each(function(){");
                //    builder.AppendLine("             if(this.checked){");
                //    builder.AppendLine("                 values+=(values == \"\" ? ($(this).val()) : (\",\" + $(this).val()));");
                //    builder.AppendLine("             }");
                //    builder.AppendLine("     });");
                //    builder.AppendLine("}else{");
                //    builder.AppendLine("     values = $(\"input[name='" + controlName + "']\").val();");
                //    builder.AppendLine("}");
                //    builder.AppendLine("var count = (values == \"\"? 0 : values.split(\",\").length);");
                //    builder.AppendLine("if(count > " + count.ToString() + "){");
                //    builder.AppendLine("     alert(\"错误：选择的抄送人过多，抄送人数不能超过 " + count.ToString() + " 人。请重新选择！\");");
                //    builder.AppendLine("     return false;");
                //    builder.AppendLine("}");
                //    builder.AppendLine("return true;");
                //}
            }
            builder.AppendLine("}");
            return builder.ToString();
        }
    }

    #region ReviewType

    /// <summary>
    /// 抄送类型.
    /// </summary>
    public enum ReviewType
    {
        /// <summary>
        /// 不支持抄送.
        /// </summary>
        None = 0,
        /// <summary>
        /// 旧有方式的抄送.
        /// </summary>
        Classic = 1,
        /// <summary>
        /// 复选框方式的抄送.
        /// </summary>
        CheckBox = 2
    }

    #endregion
}
