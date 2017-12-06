using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Allocator
{
    /// <summary>
    /// 按用户任务分配算符.
    /// </summary>
    public class UsersTaskAllocator : ITaskAllocator
    {
        #region ITaskAllocator Members

        /// <summary>
        /// 获取指定变量表达式的用户名列表.
        /// </summary>
        /// <param name="variable"></param>
        /// <returns></returns>
        public IList<string> GetTargetUsers(TaskVariable variable)
        {
            IList<string> list = new List<string>();
            string expression = (string)variable.Expression;
            if (!string.IsNullOrEmpty(expression))
            {
                string[] ss = expression.Replace(" ", "").Split( ',', '，');
                foreach (string s in ss)
                {
                    string user = s.Trim();
                    if (user.Length > 0)
                    {
                        list.Add(user);
                    }
                }
            }
            return list;
        }

        #endregion
    }
}
