using System;
using System.Collections.Generic;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Parser;

namespace Botwave.Workflow
{
    /// <summary>
    /// 流程辅助类.
    /// </summary>
    public class WorkflowHelper
    {
        /// <summary>
        /// 紧急类型数组.
        /// </summary>
        private static readonly string[] urgencyArray = { "一般", "紧急", "很紧急", "最紧急" };

        /// <summary>
        /// 将数字表示的紧急程度值(urgency)转换为字符串描述
        /// </summary>
        /// <param name="urgency">紧急程度值.</param>
        /// <returns></returns>
        public static string ConvertUrgency2String(int urgency)
        {
            if (urgency <= 0 || urgency >= urgencyArray.Length)
            {
                return urgencyArray[0];
            }

            return urgencyArray[urgency];
        }

        /// <summary>
        /// 将bool类型的保密值(secrecy)转换为字符串描述
        /// </summary>
        /// <param name="secrecy">保密值.</param>
        /// <returns></returns>
        public static string ConvertSecrecy2String(int secrecy)
        {
            if (secrecy == 0)
                return "不保密";
            return (secrecy == 1 ? "保密" : "高级保密");
        }

        /// <summary>
        /// 检查活动集合列表中是否存在某一项.
        /// </summary>
        /// <param name="sources"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool ActivitySetContains(ICollection<DeployActivitySet> sources, DeployActivitySet item)
        {
            foreach (DeployActivitySet source in sources)
            {
                if (item.ActivityName == source.ActivityName)
                    return true;
            }
            return false;
        }

        #region 流程步骤操作描述

        /// <summary>
        /// 流程步骤操作人描述模板.
        /// </summary>
        private const string Template_Actor_Description = "{0}(代{1})";
        /// <summary>
        /// 流程步骤操作人显示模板.
        /// </summary>
        private const string Template_Actor_Display = "<b>{0}</b>";
        /// <summary>
        /// 流程步骤操作人的委托人显示模板.
        /// </summary>
        private const string Template_Actor_Display_Proxy = " 代 <b>{0}</b> 处理";
        /// <summary>
        /// 流程步骤操作人带有提示的显示模板.
        /// </summary>
        private const string Template_Actor_Display_Tooltip = "<span tooltip=\"{0}\"><b>{1}</b></span>";

        /// <summary>
        /// 获取指定的流程步骤操作人描述.
        /// 格式：@操作人姓名(代@委托人姓名).
        /// </summary>
        /// <param name="actorName">操作人真实姓名.</param>
        /// <param name="proxyName">委托人真实姓名.</param>
        /// <returns></returns>
        public static string GetActivityActorDescription(string actorName, string proxyName)
        {
            if (string.IsNullOrEmpty(actorName))
                return null;
            return (string.IsNullOrEmpty(proxyName) ? actorName : string.Format(Template_Actor_Description, actorName, proxyName));
        }

        /// <summary>
        /// 解析指定的流程步骤操作人描述格式.
        /// </summary>
        /// <param name="actor">流程步骤操作人.</param>
        /// <param name="actorDescription">流程步骤操作人描述.</param>
        /// <param name="visibleTooltip">是否显示流程步骤操作人的提示.</param>
        /// <returns></returns>
        public static string ParserActivityActorDescription(string actor, string actorDescription, bool visibleTooltip)
        {
            if (string.IsNullOrEmpty(actor))
                return string.Empty;

            string actorName = actor;
            string proxyName = string.Empty;
            if (!string.IsNullOrEmpty(actorDescription))
            {
                actorDescription = actorDescription.Trim();
                int index = actorDescription.IndexOf("(代");
                if (index > 0)
                {
                    // 有委托人时.
                    actorName = actorDescription.Substring(0, index);
                    proxyName = actorDescription.Substring(index + 2);
                    if (!string.IsNullOrEmpty(proxyName) && proxyName.EndsWith(")"))
                    {
                        proxyName = proxyName.Substring(0, proxyName.Length - 1);
                        if (!string.IsNullOrEmpty(proxyName))
                            proxyName = string.Format(Template_Actor_Display_Proxy, proxyName);
                    }
                }
                else
                {
                    // 无委托人时.
                    actorName = actorDescription;
                }
            }
            actorDescription = (visibleTooltip ? string.Format(Template_Actor_Display_Tooltip, actor, actorName) : string.Format(Template_Actor_Display, actorName));
            return actorDescription + proxyName;
        }
        #endregion
    }
}
