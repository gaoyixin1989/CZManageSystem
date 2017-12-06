using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Botwave.Web.UrlRewriter
{
    /// <summary>
    /// URL 重写属性类.
    /// </summary>
    [Serializable]
    public class RewriterProperty
    {
        /// <summary>
        /// 重写 URL 的正则表达式对象.
        /// </summary>
        private Regex rewriterRegex;

        #region properties

        private string pattern;
        private string url;

        /// <summary>
        /// 实际的 URL 路径径表达式.
        /// </summary>
        public string Pattern
        {
            get { return pattern; }
            set { pattern = value; }
        }

        /// <summary>
        /// 重写的 URL 路径表达式.
        /// </summary>
        public string Url
        {
            get { return url; }
            set { url = value; }
        }
        #endregion

        #region constructor

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="pattern">实际的 URL 路径径表达式.</param>
        /// <param name="url">重写的 URL 路径表达式.</param>
        public RewriterProperty(string pattern, string url)
        {
            this.pattern = pattern;
            this.url = url;
            this.rewriterRegex = new Regex("^" + pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }
        #endregion

        #region methods

        /// <summary>
        /// 检查是否匹配指定的 URL 路径.
        /// </summary>
        /// <param name="url">URL 路径.</param>
        /// <returns></returns>
        public bool IsMatch(string url)
        {
            return rewriterRegex.IsMatch(url);
        }

        /// <summary>
        /// 重写 URL.
        /// </summary>
        /// <param name="requestUrl"></param>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public string RewriteUrl(string requestUrl, string queryString)
        {
            string rewrittenUrl = rewriterRegex.Replace(requestUrl, this.url);

            // 设置 URL 的查询字符串.
            if (!string.IsNullOrEmpty(queryString) && queryString.StartsWith("?"))
            {
                if (rewrittenUrl.IndexOf('?') > -1 && (rewrittenUrl.LastIndexOf('?') > rewrittenUrl.LastIndexOf('/')))
                {
                    queryString = queryString.Replace("?", "&");
                }
                rewrittenUrl = rewrittenUrl + queryString;
            }
            return rewrittenUrl;
        }

        #endregion
    }
}
