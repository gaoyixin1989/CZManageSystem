using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Workflow.Domain;

namespace Botwave.Workflow.Allocator
{
    /// <summary>
    /// 用户代理接口.
    /// </summary>
    public interface IUserProxy
    {
        /// <summary>
        /// 获取代理用户，如果没有则直接返回传入的userName.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        string GetProxy(string userName);

        /// <summary>
        /// 获取代理用户列表
        /// 对代理用户或被代理用户重复的情况不作处理
        /// </summary>
        /// <param name="userNames"></param>
        /// <returns></returns>
        ICollection<string> GetProxies(ICollection<string> userNames);

        /// <summary>
        /// 获取代理用户列表.
        /// 对代理用户或被代理用户重复的情况不作处理.
        /// </summary>
        /// <param name="userNames"></param>
        /// <param name="hasProxies"></param>
        /// <returns></returns>
        ICollection<string> GetProxies(ICollection<string> userNames, out bool hasProxies);

        /// <summary>
        /// 获取代理用户列表.
        /// 对代理用户重复的情况作处理，不会存在代理用户有重复的情况.
        /// </summary>
        /// <param name="userNames"></param>
        /// <returns>返回值字典类型&lt;用户名, 代理人用户名&gt;.</returns>
        IDictionary<string, string> GetDistinctProxies(ICollection<string> userNames);

        /// <summary>
        /// 获取代理用户列表.
        /// 对代理用户重复的情况作处理，不会存在代理用户有重复的情况.
        /// </summary>
        /// <param name="userNames"></param>
        /// <param name="hasProxies"></param>
        /// <returns></returns>
        IDictionary<string, string> GetDistinctProxies(ICollection<string> userNames, out bool hasProxies);
    }
}
