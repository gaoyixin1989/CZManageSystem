using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Botwave.Commons;
using Botwave.Extension.IBatisNet;

namespace Botwave.XQP.Domain
{
    [Serializable]
    public class CZWorkflowRelation : Botwave.Entities.TrackedEntity
    {
        public int Id { get; set; }

        public Guid WorkflowInstanceId { get; set; }

        public Guid RelationWorkflowInstanceId { get; set; }

        public bool IsRefAttachment { get; set; }

        public int Status { get; set; }

        public CZWorkflowRelation()
        { }

        public CZWorkflowRelation(Guid workflowInstanceId, Guid relationWorkflowInstanceId, bool isRefAttachment, string creator)
        {
            this.WorkflowInstanceId = workflowInstanceId;
            this.RelationWorkflowInstanceId = relationWorkflowInstanceId;
            this.IsRefAttachment = isRefAttachment;
            this.Status = 1;
            this.LastModifier = this.Creator = creator;
            this.LastModTime = this.LastModTime = DateTime.Now;
        }

        public void Update()
        {
            if (this.Id > 0)
                IBatisMapper.Update("cz_WorkflowRelations_Update_RefAttachment", this);
            else
                IBatisMapper.Insert("cz_WorkflowRelations_Insert", this);
        }

        public static void UpdateWorkflowInstanceId(Guid workflowDefinitionId, string creator, Guid newWorkflowInstanceId)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("OldWorkflowInstanceId", workflowDefinitionId);
            parameters.Add("NewWorkflowInstanceId", newWorkflowInstanceId);
            parameters.Add("Creator", creator);

            IBatisMapper.Update("cz_WorkflowRelations_Update_WorkflowInstanceId", parameters);
        }

        public static void Delete(int id)
        {
            IBatisMapper.Delete("CZWorkflowRelation_Delete", id);
        }

        public static DataSet GetRelations(Guid workflowInstanceId, string actor, bool started)
        {
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("WorkflowInstanceId", SqlDbType.UniqueIdentifier),
                new SqlParameter("Actor", SqlDbType.NVarChar, 50),
                new SqlParameter("Started", SqlDbType.Bit)
            };

            parameters[0].Value = workflowInstanceId;
            parameters[1].Value = actor;
            parameters[2].Value = started;

            return IBatisDbHelper.ExecuteDataset(CommandType.StoredProcedure, "bwwf_cz_GetRelationWorkflowInstance", parameters);
        }

        public static DataTable GetRelationAttachments(Guid workflowInstanceId, string actor, bool started)
        {
            string sql = string.Format(@"SELECT a.Id, a.Title, a.[FileName], a.MimeType, a.FileSize, a.Remark, a.Creator, a.CreatedTime, u.RealName, t.IsRef
                FROM xqp_Attachment_Entity e
	                LEFT JOIN xqp_Attachment a ON a.Id=e.AttachmentId
	                LEFT JOIN bw_Users u ON a.Creator = u.UserName
	                LEFT JOIN (select '{0}' as WorkflowInstanceId, 0 IsRef
		                union
		                select RelationWorkflowInstanceId as WorkflowInstanceId, 1 IsRef
		                from cz_WorkflowRelations
		                where (Status=1 and IsRefAttachment=1) and WorkflowInstanceId='{0}' and ({1}=0 or Creator='{2}')
		                ) as t ON t.WorkflowInstanceId=e.EntityId
                WHERE (EntityType = 'W_A' and t.WorkflowInstanceId is not null)
                ORDER BY IsRef, a.CreatedTime", workflowInstanceId, started ? 1 : 0, actor);

            return IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        public static DataTable GetWorkflowInstances(string actor, string keywords, int pageIndex, int pageSize, ref int recordCount)
        {
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("Actor", SqlDbType.NVarChar, 50),
                new SqlParameter("Keywords", SqlDbType.NVarChar, 128),
                new SqlParameter("PageIndex", SqlDbType.Int),
                new SqlParameter("PageSize", SqlDbType.Int),
                new SqlParameter("RecordCount", SqlDbType.Int)
            };

            parameters[0].Value = actor;
            parameters[1].Value = keywords;
            parameters[2].Value = pageIndex;
            parameters[3].Value = pageSize;
            parameters[4].Direction = ParameterDirection.ReturnValue;

            DataTable result = IBatisDbHelper.ExecuteDataset(CommandType.StoredProcedure, "bwwf_cz_GetHistoryWorkflowInstance", parameters).Tables[0];
            recordCount = (int)parameters[4].Value;

            return result;
        }

        public static string BuildRelations(Guid workflowInstanceId, string actor, bool started, bool editable)
        {
            DataTable workflowTable = CZWorkflowRelation.GetRelations(workflowInstanceId, actor, started).Tables[0];
            if (workflowTable == null)
                return null;
            int workflowCount = workflowTable.Rows.Count;
            if (workflowCount == 0)
                return null;
            string pathView = Botwave.Web.WebUtils.GetAppPath() + "contrib/workflow/pages/workflowview.aspx";
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < workflowCount; i++)
            {
                DataRow row = workflowTable.Rows[i];
                Guid? id = DbUtils.ToGuid(row["WorkflowInstanceId"]);
                if (!id.HasValue)
                    continue;
                bool isRefAttachment = DbUtils.ToBoolean(row["IsRefAttachment"]);
                // 输出行.
                result.AppendFormat("<tr{0}>", i % 2 == 1 ? " class=\"trClass\"" : string.Empty);
                result.AppendFormat("<td><a href=\"{1}?wiid={2}\" target=\"_blank\">{0}</a></td>", DbUtils.ToString(row["SheetId"]), pathView, id);
                result.AppendFormat("<td style=\"text-align:left;\"><a href=\"{1}?wiid={2}\" target=\"_blank\">{0}</a></td>", DbUtils.ToString(row["Title"]), pathView, id);
                result.AppendFormat("<td>{0}</td>", DbUtils.ToString(row["CurrentActivityNames"]));
                result.AppendFormat("<td><input type=\"checkbox\" disabled=\"disabled\"{0} /></td>", isRefAttachment ? " checked=\"checked\"" : string.Empty);

                result.AppendFormat("<td>{0}</td>", DbUtils.ToString(row["Creator"]));
                result.AppendFormat("<td>{0:yy-MM-dd HH:mm:ss}</td>", DbUtils.ToDateTime(row["StartedTime"]));
                if (editable)
                {
                    result.AppendFormat("<td style=\"vertical-align:middle;padding-top:3px;\"><a class=\"ico_del\" title=\"删除关联\" href=\"javascript:void(0);\" onclick=\"deleteHistoryRelation(this,'{0}');\">删除</a></td>", DbUtils.ToInt32(row["Id"]));
                }
                result.AppendLine("</tr>");
            }
            return result.ToString();
        }
    }
}
