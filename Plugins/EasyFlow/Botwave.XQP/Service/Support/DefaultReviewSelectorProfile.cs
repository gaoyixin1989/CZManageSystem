using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Botwave.XQP.Service.Support
{
    /// <summary>
    /// 抄送选择器对象.
    /// </summary>
    public class DefaultReviewSelectorProfile : IReviewSelectorProfile
    {
        #region IReviewSelectorProfile 成员

        /// <summary>
        /// 生成 html.
        /// </summary>
        /// <param name="webContext"></param>
        /// <param name="selecotrContext"></param>
        /// <returns></returns>
        public virtual string BuildSelectorHtml(HttpContext webContext, ReviewSelectorContext selecotrContext)
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

            builder.AppendFormat("<input type=\"hidden\" id=\"{0}\" name=\"{0}\" />", ReviewSelectorContext.VariableName);

            builder.AppendLine("</div>");

            builder.AppendLine("</td></tr>");
            builder.AppendLine("</table>");

            builder.AppendLine("</div>");

            builder.AppendLine(BuildScript());

            return builder.ToString();
        }

        #endregion

        private static string BuildScript()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<script type=\"text/javascript\">");

            builder.AppendFormat(@"<!--//
                function onOpenReviewPicker(){
                    var h = 450;
                    var w = 700;
                    var iTop = (window.screen.availHeight-30-h)/2;    
                    var iLeft = (window.screen.availWidth-10-w)/2; 
                    window.open('{0}contrib/security/pages/popupUserPicker2.aspx?func=onCompletePickReviews', '', 'height='+ h +', width='+ w+', top='+ iTop +', left='+ iLeft +', toolbar=no, menubar=no, scrollbars=yes, resizable=no,location=no, status=no');	
                    return false;
                }",Botwave.Web.WebUtils.GetAppPath());

            builder.AppendLine(@"
                function onClearReviews(){
                    if(confirm(""确定要清除已选择的抄送人？"")){
                        $(""#" + ReviewSelectorContext.VariableName + @""").val("""");
                        $(""#txtDisplyReviewActors"").val("""");
                    }
                    return true;
                }");

            builder.AppendLine(@"
                function onCompletePickReviews(result){
                    var values = $(""#" + ReviewSelectorContext.VariableName + @""").val();
                    var names = $(""#txtDisplyReviewActors"").val();
                    for(var i=0; i<result.length;i++){
                        values += ("","" + result[i].key);
                        names += ("","" + result[i].value);
                    }
                    if(values.substring(0, 1) == "","")
                        values = values.substring(1, values.length);
                    if(names.substring(0, 1) == "","")
                        names = names.substring(1, names.length);
                    $(""#" + ReviewSelectorContext.VariableName + @""").val(values);
                    $(""#txtDisplyReviewActors"").val(names);
                }");
            
            builder.AppendLine("//--></script>");
            return builder.ToString();
        }
    }
}
