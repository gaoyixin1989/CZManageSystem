using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Botwave.Commons;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow;
using Botwave.Workflow.Service;
using Botwave.Workflow.Allocator;

namespace Botwave.Workflow.Extension.Service.Plugins
{
    /// <summary>
    /// 用户代理（授权）类.
    /// </summary>
    public class UserProxy : IUserProxy
    {
        private const string SQLTEMPLATE_GETPROXY = @"select top 1 t.UserName
             from bw_Authorizations as a 
                left join bw_Users as f on a.FromUserId = f.UserId
            	left join bw_Users as t on a.ToUserId = t.UserId
             where a.Enabled = 1 and f.UserName = '{0}' and a.BeginTime <= '{1}' and a.EndTime >= '{2}'";

        private const string SQLTEMPLATE_GETPROXIES = @"select
             f.UserName as FromUserName,t.UserName as ToUserName
             from bw_Authorizations as a
            	left join bw_Users as f on a.FromUserId = f.UserId
            	left join bw_Users as t on a.ToUserId = t.UserId
             where a.Enabled = 1 and a.BeginTime <= '{0}' and a.EndTime >= '{1}'";

        #region IUserProxy Members

        /// <summary>
        /// 获取指定用户名的委托授权人.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public string GetProxy(string username)
        {
            if (String.IsNullOrEmpty(username))
            {
                return username;
            }

            string stime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string sql = String.Format(SQLTEMPLATE_GETPROXY, DbUtils.FilterSQL(username), stime, stime);
            object obj = IBatisDbHelper.ExecuteScalar(System.Data.CommandType.Text, sql);
            if (null == obj)
            {
                return username;
            }

            return obj.ToString();
        }

        /// <summary>
        /// 获取指定用户集合的委托授权人列表.
        /// </summary>
        /// <param name="usernames"></param>
        /// <returns></returns>
        public ICollection<string> GetProxies(ICollection<string> usernames)
        {
            bool hasProxies = false;
            ICollection<string> list = GetProxies(usernames, out hasProxies);
            return list;
        }

        /// <summary>
        ///  获取指定用户集合的委托授权人列表.
        /// </summary>
        /// <param name="usernames"></param>
        /// <param name="hasProxies"></param>
        /// <returns></returns>
        public ICollection<string> GetProxies(ICollection<string> usernames, out bool hasProxies)
        {
            hasProxies = false;
            if (null == usernames || usernames.Count == 0)
            {
                return new List<string>();
            }

            int count = usernames.Count;
            ICollection<string> list = new List<string>(count);

            IDictionary<string, string> proxyUsers = GetProxyUsers();

            foreach (string name in usernames)
            {
                if (proxyUsers.ContainsKey(name))
                {
                    list.Add(proxyUsers[name]);
                    hasProxies = true;
                }
                else
                {
                    list.Add(name);
                }
            }

            return list;
        }

        /// <summary>
        ///  获取指定用户集合的委托授权人字典.
        /// </summary>
        /// <param name="usernames"></param>
        /// <returns></returns>
        public IDictionary<string, string> GetDistinctProxies(ICollection<string> usernames)
        {
            bool hasProxies = false;
            IDictionary<string, string> dict = GetDistinctProxies(usernames, out hasProxies);
            return dict;
        }

        /// <summary>
        /// 获取指定用户集合的委托授权人字典.
        /// </summary>
        /// <param name="usernames"></param>
        /// <param name="hasProxies"></param>
        /// <returns></returns>
        public IDictionary<string, string> GetDistinctProxies(ICollection<string> usernames, out bool hasProxies)
        {
            hasProxies = false;
            if (null == usernames || usernames.Count == 0)
            {
                return new Dictionary<string, string>();
            }

            int count = usernames.Count;
            IDictionary<string, string> dict = new Dictionary<string, string>(count);

            IDictionary<string, string> proxyUsers = GetProxyUsers();

            foreach (string name in usernames)
            {
                if (proxyUsers.ContainsKey(name))//如果有代理

                {
                    string destName = proxyUsers[name];
                    if (!dict.ContainsKey(destName)) //如果此用户还没有加入目标用户中

                    {
                        dict.Add(destName, name);
                        hasProxies = true;
                    }
                }
                else
                {
                    if (!dict.ContainsKey(name))
                    {
                        dict.Add(name, null);
                    }
                }
            }

            return dict;
        }

        #endregion

        private static IDictionary<string, string> GetProxyUsers()
        {
            string stime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string sql = String.Format(SQLTEMPLATE_GETPROXIES, stime, stime);
            IDictionary<string, string> proxyUsers = new Dictionary<string, string>();
            using (IDataReader reader = IBatisDbHelper.ExecuteReader(System.Data.CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    proxyUsers.Add(reader.GetString(0), reader.GetString(1));
                }
            }
            return proxyUsers;
        }
    }
}
