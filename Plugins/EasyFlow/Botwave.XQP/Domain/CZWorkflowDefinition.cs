using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Botwave.Extension.IBatisNet;

namespace Botwave.XQP.Domain
{
    [Serializable]
    public class CZWorkflowDefinition : Botwave.Workflow.Domain.WorkflowDefinition
    {
        /// <summary>
        /// 获取当前正在使用的流程的历史版本
        /// </summary>
        /// <returns></returns>
        public static DataTable GetHistoryWorkflowDefinition(string workflows,string keywork, int pageIndex, int pageSize, ref int recordCount)
        {
            string tableName = "vw_bwwf_Workflows_Detail";
            string fieldKey = "WorkflowId";
            string fieldShow = @"WorkflowId, WorkflowName, Owner, Enabled, IsCurrent, Version,Creator, Remark,
          LastModifier, CreatedTime, LastModTime, IsDeleted, WorkflowAlias";
            string fieldOrder = "WorkflowAlias asc,Version desc,LastModTime desc";
            StringBuilder where = new StringBuilder("isdeleted=0");
            if (!string.IsNullOrEmpty(keywork))
                where.AppendFormat(" and ((WorkflowAlias like '%{0}%') OR (WorkflowName like '%{1}%'))", keywork.ToUpper(),keywork);
            if (!string.IsNullOrEmpty(workflows))
            {
                where.AppendFormat(" and workflowname in ('{0}')", workflows.Replace(",", "','"));
            }
            DataTable dt = IBatisDbHelper.GetPagedList(tableName, fieldKey, pageIndex, pageSize, fieldShow, fieldOrder, where.ToString(), ref recordCount);
            return dt;
        }

        public static int UpdateWorklowRemark(Botwave.Workflow.Domain.WorkflowDefinition defintion)
        {
            return Botwave.Extension.IBatisNet.IBatisMapper.Update("bwwf_Workflow_UpdateRemark", defintion);
        }
    }
}
