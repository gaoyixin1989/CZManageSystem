using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Botwave.Web.UrlRewriter
{
    /// <summary>
    /// URL 重写上下文.
    /// </summary>
    public class UrlRewriterContext
    {
        #region fields

        /// <summary>
        /// Web 应用程序路径.
        /// </summary>
        public static readonly string WebApplicationPath;

        /// <summary>
        /// 是否启用 URL 重写.
        /// </summary>
        public static bool EnableRewriter = true;

        /// <summary>
        /// URL 重写属性列表集合.
        /// </summary>
        public static IList<RewriterProperty> RewriterProperties;
        
        /// <summary>
        /// URL 重写路径过滤表达式列表.
        /// </summary>
        public static IList<string> PathFilters;

        /// <summary>
        /// 重写 URL 过滤正则表达式.
        /// </summary>
        private static Regex RewriteFilter = null;

        /// <summary>
        /// 是否跳过重写 URL 的过滤验证.
        /// </summary>
        private static bool SkipFilter = false;

        #endregion

        /// <summary>
        /// 静态构造方法.
        /// </summary>
        static UrlRewriterContext()
        {
            WebApplicationPath = HttpContext.Current.Request.ApplicationPath;
            RewriterProperties = new List<RewriterProperty>();
            PathFilters = new List<string>();

            UrlRewriterSectionHandler.Init();

            // URL 过滤.
            RewriteFilter = GetRewriteFilter(PathFilters);
            SkipFilter = (RewriteFilter == null);
        }

        #region methods

        /// <summary>
        /// 获取当前上下文的重写 URL 路径.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetRewrittenUrl(HttpContext context)
        {
            string newPath = null;
            string path = context.Request.Path;

            if (SkipFilter || RewriteFilter.IsMatch(path))
            {
                string queryString = context.Request.Url.Query;
                foreach (RewriterProperty item in RewriterProperties)
                {
                    if (item.IsMatch(path))
                    {
                        newPath = item.RewriteUrl(path, queryString);
                    }
                }
            }
            return newPath;
        }

        /// <summary>
        /// 重写 URL 路径.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool RewriteUrl(HttpContext context)
        {
            string rewrittenUrl = UrlRewriterContext.GetRewrittenUrl(context);
            if (!string.IsNullOrEmpty(rewrittenUrl))
            {
                string queryString = null;
                int index = rewrittenUrl.IndexOf('?');
                if (index >= 0)
                {
                    queryString = (index < (rewrittenUrl.Length - 1)) ? rewrittenUrl.Substring(index + 1) : string.Empty;
                    rewrittenUrl = rewrittenUrl.Substring(0, index);
                }
                UrlRewriterContext.RewriteUrl(context, rewrittenUrl, queryString);
            }
            return (!string.IsNullOrEmpty(rewrittenUrl));
        }

        /// <summary>
        /// 重写 URL 路径.
        /// </summary>
        /// <param name="context">HTTP 当前上下文.</param>
        /// <param name="path"></param>
        /// <param name="queryString">查询字符串.</param>
        public static void RewriteUrl(HttpContext context, string path, string queryString)
        {
            if (path.StartsWith("/"))
                path = "~" + path;
            else if (!path.StartsWith("~/"))
                path = "~/" + path;
            context.RewritePath(path, null, queryString, true);
        }
        #endregion

        #region private methods

        /// <summary>
        /// 获取重写 URL 过滤的正则表达式.
        /// </summary>
        /// <param name="filters">过滤的路径表达式列表.</param>
        /// <returns></returns>
        private static Regex GetRewriteFilter(IList<string> filters)
        {
            string pattern = GetRewriteFilterPattern(filters);
            if (string.IsNullOrEmpty(pattern))
                return null;
            return new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        /// <summary>
        /// 获取重写 URL 过滤的表达式字符串.
        /// </summary>
        /// <param name="filters">过滤的路径表达式列表.</param>
        /// <returns></returns>
        private static string GetRewriteFilterPattern(IList<string> filters)
        {
            // 不存在过滤时.
            if (filters == null || filters.Count == 0)
                return null;

            StringBuilder patternBuilder = new StringBuilder();
            foreach (string item in filters)
            {
                patternBuilder.AppendFormat("|({0})", item.Replace("*", @"[\w\W]*"));
            }

            string pattern = (patternBuilder.Length > 0) ? patternBuilder.Remove(0, 1).ToString() : null;
            return (string.IsNullOrEmpty(pattern) ? pattern : string.Format("({0})", pattern));
        }

        #endregion
    }
}
