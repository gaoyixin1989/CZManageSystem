using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class contrib_workflow_pages_PopupAliasImages : Botwave.Web.PageBase
{
    public string OpenerInputId = string.Empty;
    public string OpenerInputValue = string.Empty;

    /// <summary>
    /// 别名图片路径.
    /// </summary>
    static readonly string GroupImageFolder = AppPath + "contrib/workflow/res/groups/";
    const string HtmlTemplate_Image = "<input type=\"radio\" name=\"chkAlias\" id=\"alia_{0}\" value=\"{1}\" /><label for=\"alia_{0}\"><img src=\"../res/groups/{1}\" /></label>";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string inputId = Server.HtmlEncode(Request.QueryString["inputId"]);
            string inputValue = Server.HtmlEncode(Request.QueryString["inputValue"]);
            this.OpenerInputId = inputId;
            this.OpenerInputValue = inputValue;
        }
    }

    #region build alias html

    /// <summary>
    /// 重复列数.
    /// </summary>
    const int Image_RepeateColumns = 8;

    /// <summary>
    /// 生成别名图片的 HTML.
    /// </summary>
    /// <returns></returns>
    private static string BuildImageHtml()
    {
        string imageFolder = HttpContext.Current.Server.MapPath(GroupImageFolder);
        return BuildImageHtml(GetAliasImages(imageFolder));
    }

    /// <summary>
    /// 生成别名图片的 HTML.
    /// </summary>
    /// <returns></returns>
    private static string BuildImageHtml(IDictionary<string, string> images)
    {
        if (images == null || images.Count == 0)
            return string.Empty;
        StringBuilder builder = new StringBuilder();
        using (StringWriter writer = new StringWriter(builder))
        {
            int index = 0;
            writer.WriteLine("<table>");
            foreach (string key in images.Keys)
            {
                int columnIndex = index % Image_RepeateColumns;
                if (columnIndex == 0)
                {
                    writer.WriteLine("\t<tr>");
                }
                writer.Write("\t\t<td>");
                writer.Write(string.Format(HtmlTemplate_Image, index, key));
                writer.Write("</td>");
                if (columnIndex == Image_RepeateColumns - 1)
                {
                    writer.WriteLine("</tr>");
                }
                index++;
            }
            int modValue = images.Count % Image_RepeateColumns;
            if (modValue > 0)
            {
                while (modValue < Image_RepeateColumns)
                {
                    writer.Write("<td></td>");
                    modValue++;
                }
                writer.WriteLine("</tr>");
            }
            writer.WriteLine("</table>");
            return writer.ToString();
        }
    }

    /// <summary>
    /// 获取别名图片字典(key:名称，value:路径).
    /// </summary>
    /// <param name="folderPath"></param>
    /// <returns></returns>
    private static IDictionary<string, string> GetAliasImages(string folderPath)
    {
        IDictionary<string, string> results = new Dictionary<string, string>();
        if (Directory.Exists(folderPath))
        {
            DirectoryInfo dir = new DirectoryInfo(folderPath);
            FileInfo[] images = dir.GetFiles("*.gif");
            foreach (FileInfo item in images)
            {
                string key = item.Name.ToLower();
                if (!results.ContainsKey(key))
                {
                    results.Add(key, key);
                }
            }
        }
        return results;
    }
    #endregion
}