using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow.Extension.Domain;

namespace Botwave.Workflow.Extension.Service.Support
{
    /// <summary>
    /// 流程用户服务的默认实现类.
    /// </summary>
    public class DefaultWorkflowUserService : IWorkflowUserService
    {
        #region IActorService 成员

        /// <summary>
        /// 获取指定操作人的详细信息.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public ActorDetail GetActorDetail(string userName)
        {
            return IBatisMapper.Load<ActorDetail>("bwwf_Users_Select_ActorDetail", userName);
        }

        /// <summary>
        /// 获取指定操作人的提示信息.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public TooltipInfo GetActorTooltip(string userName)
        {
            return IBatisMapper.Load<TooltipInfo>("bwwf_Users_Select_TooltipInfo", userName);
        }

        /// <summary>
        /// 获取指定用户名前缀的用户名列表.
        /// </summary>
        /// <param name="prefixName"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public IList<string> GetActorsLikeName(string prefixName, int count)
        {
            if (string.IsNullOrEmpty(prefixName))
                return new List<string>();
            if (count <= 0 || count > 100)
                count = 20;

            string sql = string.Format(@"SELECT DISTINCT TOP {0} RealName FROM bw_Users 
                WHERE RealName LIKE '{1}%' OR UserName LIKE '{1}%' ORDER BY RealName", count, prefixName);

            DataTable sourceTable = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
            if (sourceTable == null || sourceTable.Rows.Count == 0)
                return new List<string>();

            IList<string> results = new List<string>();
            foreach (DataRow row in sourceTable.Rows)
            {
                string name = row[0].ToString();
                if (!results.Contains(name))
                    results.Add(name);
            }
            return results;
        }

        /// <summary>
        /// 获取指定用户名集合的真实姓名字典.
        /// </summary>
        /// <param name="actorNames"></param>
        /// <returns></returns>
        public IDictionary<string, string> GetActorRealNames(ICollection<string> actorNames)
        {
            IDictionary<string, string> actors = new Dictionary<string, string>();
            if (actorNames == null || actorNames.Count == 0)
                return actors;
            foreach (string name in actorNames)
            {
                if (string.IsNullOrEmpty(name) || actors.ContainsKey(name))
                    continue;
                //string realName = IBatisMapper.Mapper.QueryForObject<string>("bwwf_Users_Select_RealName_ByUserName", name);
                //当name不存在时,iBatis处理有些问题(缓存及类型转换),所以使用以下模式

                object obj = IBatisMapper.Mapper.QueryForObject("bwwf_Users_Select_RealName_ByUserName", name);
                string realName = obj as string;
                if (string.IsNullOrEmpty(realName))
                    continue;
                actors.Add(name, realName);
            }
            return actors;
        }

        #endregion
    }
}
