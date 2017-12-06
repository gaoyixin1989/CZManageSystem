 using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;

public partial class test_test : System.Web.UI.Page
{
    public static string WeekNames = "日一二三四五六";

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    /// <summary>
    /// 获取上传的文件保存的根路径.
    /// </summary>
    /// <returns></returns>
    private static string GetSaveFileDirectory()
    {
        string path = ConfigurationManager.AppSettings["UploadVirtualPath"];
        if (string.IsNullOrEmpty(path))
        {
            path = ConfigurationManager.AppSettings["SaveFileDir"];
            if (string.IsNullOrEmpty(path))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(HttpContext.Current.Server.MapPath("~/FileSever.config"));
                if (doc != null)
                {
                    XmlNode urlNode = doc.DocumentElement.SelectSingleNode("url");
                    if (urlNode != null)
                    {
                        path = urlNode.InnerText;
                    }
                }
            }
        }
        if (!string.IsNullOrEmpty(path) && !path.EndsWith("/"))
            path = path + "/";
        return path;
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        this.txtEncode.Text = (HttpUtility.UrlEncode(this.txtUrl.Text));
    }

}
