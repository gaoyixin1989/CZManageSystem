using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Botwave.Commons;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow.Allocator;

namespace Botwave.Workflow.Extension.Service.Plugins
{
    /// <summary>
    /// 按资源任务分配算符.
    /// </summary>
    public class ResourceTaskAllocator : ITaskAllocator
    {
        #region ITaskAllocator Members

        /// <summary>
        /// 获取指定参数的目标处理用户列表.
        /// </summary>
        /// <param name="variable"></param>
        /// <returns></returns>
        public IList<string> GetTargetUsers(TaskVariable variable)
        {
            string expression = (string)variable.Expression;
            if (string.IsNullOrEmpty(expression))
            {
                return new List<string>();
            }

            expression = DbUtils.FilterSQL(expression).Trim('\r', '\n', '\t', ' ');// 去除空白
            string sql = string.Format(@"SELECT UserName FROM bw_Users with(nolock) WHERE Status = 0 and UserId IN
(SELECT UserId FROM bw_UsersInRoles WHERE RoleId IN
    (SELECT RoleId FROM bw_RolesInResources WHERE ResourceId = '{0}')
) ORDER BY UserName", expression);
            IList<string> list = new List<string>();
            using (IDataReader reader = IBatisDbHelper.ExecuteReader(System.Data.CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    list.Add(reader.GetString(0));
                }
            }
            return list;
        }

        #endregion
    }
}
