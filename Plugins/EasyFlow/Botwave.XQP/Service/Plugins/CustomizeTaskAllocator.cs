using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Botwave.Commons;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow.Allocator;
using System.Configuration;
using System.Data.SqlClient;
namespace Botwave.XQP.Service.Plugins
{
    /// <summary>
    /// 按自定义组织结构分派任务.
    /// </summary>
    public class CustomizeTaskAllocator : ITaskAllocator
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(CustomizeTaskAllocator));

        private const string SQL_SELECT_ACTORS = @"";

        private string spName = "bwwf_cz_GetAuditUsersByCus";

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
            IList<string> list = new List<string>();
            string department = string.Empty;
            string office = string.Empty;
            string arg = string.Empty;
            
            if (null == variable || null == variable.Expression)
            {
                return null;
            }
            foreach (string groups in variable.Expression.ToString().Split(',', '，'))
            {
                if (groups.Length == 0)
                    continue;
                string[] args = groups.Split('$');
                switch (args.Length)
                {
                    case 1:
                        department = args[0];
                        break;
                    case 2:
                        department = args[0];
                        arg = args[1];
                        break;
                    case 3:
                        department = args[0];
                        office = args[1];
                        arg = args[2];
                        break;
                    default:
                        break;
                }
                SqlConnection conn = new SqlConnection(IBatisDbHelper.ConnectionString);
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = spName;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Department", SqlDbType.NVarChar, 50);    //部门名称 
                    cmd.Parameters["@Department"].Direction = ParameterDirection.Input;
                    cmd.Parameters["@Department"].Value = department;
                    cmd.Parameters.Add("@Office", SqlDbType.NVarChar, 50);    //科室名称 
                    cmd.Parameters["@Office"].Direction = ParameterDirection.Input;
                    cmd.Parameters["@Office"].Value = office;

                    cmd.Parameters.Add("@Args", SqlDbType.NVarChar, 2000);   //查询那几列 
                    cmd.Parameters["@Args"].Direction = ParameterDirection.Input;
                    cmd.Parameters["@Args"].Value = arg;
                    DataSet ds = new DataSet();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                    adapter.Fill(ds);
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            if (!string.IsNullOrEmpty(row["USERNAME"].ToString()))
                            {
                                if (!list.Contains(row["USERNAME"].ToString()))
                                    list.Add(row["USERNAME"].ToString());
                            }
                        }
                    }
                    cmd.Parameters.Clear();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    conn.Close();
                    log.Error("Exception occur in CustomizeTaskAllocator" + ex.ToString());
                }
            }
            return list;
        }
        #endregion
    }
}
