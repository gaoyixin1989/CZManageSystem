using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Botwave.Commons;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow.Allocator;

namespace Botwave.XQP.Service.Plugins
{
    /// <summary>
    /// 按组织结构分派任务.
    /// </summary>
    public class SuperiorTaskAllocator : ITaskAllocator
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(SuperiorTaskAllocator));

        private const string SQL_SELECT_ACTORS = @"";

        private string spName = "bwwf_ext_GetAuditUsersByOrg";

        public string SpName
        {
            set { spName = value; }
        }

        #region ITaskAllocator Members

        /// <summary>
        /// 参数说明.
        /// variable.Args值说明:
        /// 1: 所有上级主管;
        /// 2: 同部门上级主管;
        /// 3: 直接主管;
        /// 4: 同部门其他人员;
        /// 5: 同科室其他人员;
        /// 6: 室审核,即同部门内所有室经理;
        /// 7: 部门审核,即同部门内所有部门经理;
        /// 8: 公司领导审核,即所有公司领导.
        /// </summary>
        /// <param name="variable">
        /// variable.Args:
        /// 1: 所有上级主管;
        /// 2: 同部门上级主管;
        /// 3: 直接主管;
        /// 4: 同部门其他人员;
        /// 5: 同科室其他人员;
        /// 6: 室审核,即同部门内所有室经理;
        /// 7: 部门审核,即同部门内所有部门经理;
        /// 8: 公司领导审核,即所有公司领导.
        /// </param>
        /// <returns></returns>
        public IList<string> GetTargetUsers(TaskVariable variable)
        {
            if (null == variable || null == variable.Expression)
            {
                return null;
            }

            IList<string> list = new List<string>();
            IDbDataParameter[] parms = IBatisDbHelper.CreateParameterSet(2);
            parms[0].ParameterName = "@UserName";
            parms[0].DbType = DbType.String;
            parms[0].Value = variable.Actor;
            parms[1].ParameterName = "@Args";
            parms[1].DbType = DbType.String;
            parms[1].Value = variable.Expression;
            using (IDataReader reader = IBatisDbHelper.ExecuteReader(CommandType.StoredProcedure, spName, parms))
            {
                //返回的数据列为UserName
                while (reader.Read())
                {
                    list.Add(reader.GetString(0));
                }
                reader.Close();
            }
            return list;
        }

        #endregion
    }
}
