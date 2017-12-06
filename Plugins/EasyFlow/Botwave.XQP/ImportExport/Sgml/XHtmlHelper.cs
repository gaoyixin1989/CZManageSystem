using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Botwave.XQP.ImportExport.Sgml
{
    /// <summary>
    /// XHTML 辅助类.
    /// </summary>
    internal class XHtmlHelper
    {
        #region regex

        /// <summary>
        /// 文档标题的正则表达式.
        /// </summary>
        private static readonly Regex Regex_Title = new Regex("<head[^>]*>[\\s\\S]*<title[^>]*>(?<title>[^<]*)</title>", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex Regex_Body = new Regex("<html[^>]*>[\\s\\S]*<body>(?<body>[\\s\\S]*)</body>[^<]*</html>", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        #endregion

        #region util

        /// <summary>
        /// 将 HTML 内容格式化为 XHTML 内.
        /// </summary>
        /// <param name="html"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static string ToXHtml(string html, out string error)
        {
            error = string.Empty;
            if (string.IsNullOrEmpty(html))
                return html;

            string output = String.Empty;
            using (SgmlReader reader = new SgmlReader())
            {
                reader.DocType = "HTML";
                using (StringReader inputStream = new System.IO.StringReader(html))
                {
                    reader.InputStream = inputStream;
                    using (StringWriter writer = new StringWriter())
                    {
                        XmlTextWriter xmlWriter = new XmlTextWriter(writer);
                        reader.Read();
                        while (!reader.EOF)
                            xmlWriter.WriteNode(reader, true);

                        xmlWriter.Flush();
                        xmlWriter.Close();
                        output = writer.ToString();
                        writer.Close();
                    }
                    if (reader.ErrorLog != null)
                        error = reader.ErrorLog.ToString();
                }
            }
            return output;
        }

        /// <summary>
        /// 获取文档标题.
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string GetTitle(string html)
        {
            if (string.IsNullOrEmpty(html))
                return null;
            Match match = Regex_Title.Match(html);
            if (match != null && match.Groups["title"] != null)
                return match.Groups["title"].Value.Trim();
            return null;
        }

        public static string GetBody(string html)
        {
            if (string.IsNullOrEmpty(html))
                return null;
            Match match = Regex_Body.Match(html);
            if (match != null && match.Groups["body"] != null)
                return match.Groups["body"].Value.Trim();
            return null;
        }
        #endregion
    }
}
