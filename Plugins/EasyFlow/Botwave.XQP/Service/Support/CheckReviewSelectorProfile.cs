using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Botwave.XQP.Domain;

namespace Botwave.XQP.Service.Support
{
    /// <summary>
    /// 文本框形式的抄送选择器.
    /// </summary>
    public class CheckReviewSelectorProfile : DefaultReviewSelectorProfile
    {
        #region override

        public override string BuildSelectorHtml(HttpContext webContext, ReviewSelectorContext selecotrContext)
        {
            if (selecotrContext == null || selecotrContext.NextProfiles == null || selecotrContext.NextProfiles.Count == 0)
                return string.Empty;

            StringBuilder builder = new StringBuilder();
            foreach(ActivityProfile next in selecotrContext.NextProfiles)
            {
                if (next.IsReview == false)
                    continue;
                string itemText = BuildItem(next);
                if (string.IsNullOrEmpty(itemText))
                    continue;
                builder.AppendLine("<li>" + itemText + "</li>");
            }
            return builder.ToString();
        }
        #endregion

        protected virtual string BuildItem(ActivityProfile item)
        {
            if (item == null || string.IsNullOrEmpty(item.ReviewActors))
                return string.Empty;
            string[] actorArray = item.ReviewActors.Split(',', '，');

            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("<span>◆ {0}</span>", item.ActivityName);
            return builder.ToString();
        }
    }
}
