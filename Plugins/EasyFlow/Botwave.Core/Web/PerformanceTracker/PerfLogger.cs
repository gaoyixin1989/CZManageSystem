using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.Common;

using log4net;

using Botwave.Commons;

namespace Botwave.Web.PerformanceTracker
{
    /// <summary>
    /// 性能日志记录器.
    /// </summary>
    public static class PerfLogger
    {
        private const int DEFAULT_CACHE_SIZE = 35;
        private const int DEFAULT_MIN_CAST_INTERVAL = 500;//0.5秒

        private static IList<string> cache = new List<string>();

        private static readonly int cacheSize;
        private static readonly int minCostInterval;

        private static readonly Regex matchPatternRegex = null;

        private static readonly ILog log = LogManager.GetLogger(typeof(PerfLogger));

        //private static readonly Object syncObj = new object();

        private const string sqlTemplate = "insert into Tracking_Performance (Resource, Arguments, ActionTime, CostInterval) values ('{0}','{1}','{2}','{3}')";

        /// <summary>
        /// 静态构造方法.
        /// </summary>
        static PerfLogger()
        {
            string sCacheSize = ConfigurationManager.AppSettings["Botwave.PerformanceTracker.CacheSize"];
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
                    log.ErrorFormat("appSettings/Botwave.PerformanceTracker.CacheSize error:{0}.", ex);
                }
            }

            string sMinCostInterval = ConfigurationManager.AppSettings["Botwave.PerformanceTracker.MinCostInterval"];
            if (String.IsNullOrEmpty(sMinCostInterval))
            {
                minCostInterval = DEFAULT_MIN_CAST_INTERVAL;
            }
            else
            {
                try
                {
                    minCostInterval = Convert.ToInt32(sMinCostInterval);
                }
                catch (FormatException ex)
                {
                    minCostInterval = DEFAULT_MIN_CAST_INTERVAL;
                    log.ErrorFormat("appSettings/Botwave.PerformanceTracker.MinCostInterval error:{0}.", ex);
                }
            }

            string matchPattern = ConfigurationManager.AppSettings["Botwave.PerformanceTracker.MatchPattern"];
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
        /// 记录日志.
        /// </summary>
        /// <param name="req"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        public static void Log(HttpRequest req, DateTime beginTime, DateTime endTime)
        {
            if (matchPatternRegex.IsMatch(req.Url.PathAndQuery))
            {                
                TimeSpan ts = endTime - beginTime;
                int costInterval = (int)ts.TotalMilliseconds;

                if (costInterval < minCostInterval)
                {
                    return;
                }

                string resource = DbUtils.FilterSQL(req.Url.LocalPath);
                string arguments = DbUtils.FilterSQL(req.Url.Query);
                string sql = String.Format(sqlTemplate, resource, arguments, beginTime, costInterval);
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