using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Botwave.XQP.Domain
{
    /// <summary>
    /// 公告(通知)类.
    /// </summary>
    [Serializable]
    public class Notice
    {
        /// <summary>
        /// 日志类型名称.
        /// </summary>
        public const string TypeName_Notice = "notice";

        #region gets / sets

        private int noticeId;
        private string title;
        private string content;
        private bool enabled = true;
        private DateTime? startTime;
        private DateTime? endTime;
        private string creator;
        private string lastModifier;
        private DateTime createdTime;
        private DateTime lastModTime;
        private string entityId;
        private string entityType;

        /// <summary>
        /// 公告编号.
        /// </summary>
        public int NoticeId
        {
            get { return noticeId; }
            set { noticeId = value; }
        }

        /// <summary>
        /// 标题.
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// 内容.
        /// </summary>
        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        /// <summary>
        /// 是否启用.
        /// </summary>
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        /// <summary>
        /// 公告启用时间.
        /// </summary>
        public DateTime? StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        /// <summary>
        /// 公告关闭时间.
        /// </summary>
        public DateTime? EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

        /// <summary>
        /// 创建人.
        /// </summary>
        public string Creator
        {
            get { return creator; }
            set { creator = value; }
        }

        /// <summary>
        /// 最近一次的修改人.
        /// </summary>
        public string LastModifier
        {
            get { return lastModifier; }
            set { lastModifier = value; }
        }

        /// <summary>
        /// 创建时间.
        /// </summary>
        public DateTime CreatedTime
        {
            get { return createdTime; }
            set { createdTime = value; }
        }

        /// <summary>
        /// 最近一次的更新时间.
        /// </summary>
        public DateTime LastModTime
        {
            get { return lastModTime; }
            set { lastModTime = value; }
        }

        /// <summary>
        /// 关联公告的外部实体编号.
        /// </summary>
        public string EntityId
        {
            get { return entityId; }
            set { entityId = value; }
        }

        /// <summary>
        /// 关联公告的外部实体类型.
        /// </summary>
        public string EntityType
        {
            get { return entityType; }
            set { entityType = value; }
        }
        #endregion

        /// <summary>
        /// 构造方法.
        /// </summary>
        public Notice()
        { }

        /// <summary>
        /// 在指定页面弹出显示指定公告列表.
        /// </summary>
        /// <param name="ownerPage">指定弹出公告的页面.</param>
        /// <param name="noticeViewPath">公告信息浏览页面(包含查询字符串).</param>
        /// <param name="isDirectOutput">是否直接输出弹出公告窗口的脚本.</param>
        /// <param name="notices">要显示的公告列表.</param>
        public static void RenderPopupNotices(Page ownerPage, string noticeViewPath, IList<Notice> notices, bool isDirectOutput)
        {
            if (notices == null || notices.Count == 0)
                return;
            StringBuilder scriptBuilder = new StringBuilder();
            scriptBuilder.AppendLine("<script type=\"text/javascript\">");
            scriptBuilder.AppendLine("var iTop = (window.screen.availHeight-30-400)/2;");
            scriptBuilder.AppendLine("var iLeft = (window.screen.availWidth-10-800)/2;");
            foreach (Notice item in notices)
            {
                scriptBuilder.AppendLine(string.Format("window.open('{0}','', 'height=400, width=800, top='+iTop+', left='+iLeft+', toolbar=no, menubar=no, scrollbars=yes, resizable=no,location=no, status=no');", noticeViewPath + item.NoticeId.ToString()));
            }
            scriptBuilder.AppendLine("</script>");
            if (isDirectOutput)
                ownerPage.Response.Write(scriptBuilder.ToString());
            else
                ownerPage.ClientScript.RegisterClientScriptBlock(ownerPage.GetType(), "popup_notices", scriptBuilder.ToString(), false);
        }
    }
}
