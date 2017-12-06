using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Text.RegularExpressions;

using log4net;

using Botwave.Commons;

namespace Botwave.Web.RequestTracker
{
    /// <summary>
    /// 请求日志记录器.
    /// </summary>
    public static class RequestLogger
    {
        private const int DEFAULT_CACHE_SIZE = 35;

        private static IList<string> cache = new List<string>();

        private static readonly int cacheSize;

        private static readonly Regex matchPatternRegex = null;

        private static readonly ILog log = LogManager.GetLogger(typeof(RequestLogger));

        //private static readonly Object syncObj = new object();

        private const string sqlTemplate = "insert into Tracking_Request (Path,Query,UserHostAddress,UserAgent,BrowserType,BrowserName,BrowserVersion,Platform,RequestTime) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')";

        /// <summary>
        /// 静态构造方法.
        /// </summary>
        static RequestLogger()
        {
            string sCacheSize = ConfigurationManager.AppSettings["Botwave.RequestTracker.CacheSize"];
            if (String.IsNullOrEmpty(sCacheSize))
            {                
                cacheSize = DEFAULT_CACHE_SIZE;
            }
            else
            {
                try
                {
                    cacheSize = Convert.ToInt32(sCacheSize);
                }
                catch (FormatException ex)
                {
                    cacheSize = DEFAULT_CACHE_SIZE;
                    log.ErrorFormat("appSettings/Botwave.RequestTracker.CacheSize error:{0}.", ex);
                }
            }

            string matchPattern = ConfigurationManager.AppSettings["Botwave.RequestTracker.MatchPattern"];
            if (String.IsNullOrEmpty(matchPattern))
            {
                //.aspx|.ashx|.asmx
                //默认只对普通的aspx页面进行跟踪
                matchPatternRegex = new Regex(".aspx", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            }
            else
            {
                matchPatternRegex = new Regex(matchPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            }
        }

        /// <summary>
        /// 记录请求信息.
        /// </summary>
        /// <param name="req"></param>
        public static void Log(HttpRequest req)
        {
            if (matchPatternRegex.IsMatch(req.Url.PathAndQuery))
            {
                string sql = String.Format(sqlTemplate, 
                    req.Url.LocalPath,
                    DbUtils.FilterSQL(req.Url.Query),
                    req.UserHostAddress,
                    req.UserAgent,
                    req.Browser.Type,
                    req.Browser.Browser,
                    req.Browser.Version,
                    req.Browser.Platform,
                    DateTime.Now);
                cache.Add(sql);

                if (cache.Count >= cacheSize)
                {
                    string allSql = CollectionUtils.IList2String(cache, ";");

                    try
                    {
                        SqlHelper.ExecuteNonQuery(System.Data.CommandType.Text, allSql);
                    }
                    catch (DbException ex)
                    {
#if DEBUG
                        throw ex;
#else
                        log.Error(ex);
#endif
                    }

                    cache.Clear();
                }
            }            
        }
    }
}
