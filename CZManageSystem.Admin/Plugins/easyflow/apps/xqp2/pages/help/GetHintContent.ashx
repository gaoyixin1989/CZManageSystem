<%@ WebHandler Language="C#" Class="GetHintContent" %>

using System;
using System.Web;
using System.Data;
using Botwave.Extension.IBatisNet;

/// <summary>
/// 获取帮助提示内容
/// </summary>
public class GetHintContent : IHttpHandler
{    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        string title = context.Request.QueryString["t"];

        if (string.IsNullOrEmpty(title))
        {
            context.Response.Write("null");
        }
        else
        {
            title = HttpUtility.UrlDecode(title);
            string url = context.Request.Path.Replace("GetHintContent.ashx", "ViewHelp.aspx");
            string content = GetHintContentByTitle(title, url);
            context.Response.Write(content);
        }
        context.Response.End();
    }
    private string GetHintContentByTitle(string title, string url)
    {
        string sql = string.Format("select top 1 id,content from xqp_help where title = '{0}'", title);
        string result = string.Empty;
        using (IDataReader dr = IBatisDbHelper.ExecuteReader(System.Data.CommandType.Text, sql))
        {
            while (dr.Read())
            {
                result = dr.GetString(1);
                if (result.Length > 200)
                {
                    url = string.Format("{0}?id={1}", url, dr.GetInt32(0));
                    result = string.Format("{0} <a href='{1}' title='点击查看更多'>...</a>", result.Substring(0, 200), url);
                }
            }
        }

        if (!string.IsNullOrEmpty(result))
            return result;

        return "null";
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}