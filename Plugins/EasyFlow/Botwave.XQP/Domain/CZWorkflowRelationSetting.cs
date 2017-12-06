using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Botwave.Commons;
using Botwave.Extension.IBatisNet;
using Botwave.XQP.Commons;

namespace Botwave.XQP.Domain
{
    /// <summary>
    /// 子流程设置业务类
    /// </summary>
    [Serializable]
    public class CZWorkflowRelationSetting : Botwave.Entities.TrackedEntity
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(CZWorkflowRelationSetting));
        public int Id { get; set; }

        public Guid WorkflowId { get; set; }

         public Guid ActivityId { get; set; }

        public string RelationWorkflowName { get; set; }

        public int SettingType { get; set; }

        public int TriggerType { get; set; }

        public int OperateType { get; set; }

        public bool Status { get; set; }

        public bool IsRefAttachment { get; set; }

        public string FieldsAssemble { get; set; }

        public string RelationCreator { get; set; }

        public string WorkflowName { get; set; }

        public string ActivityName { get; set; }

        public CZWorkflowRelationSetting()
        { }

        public CZWorkflowRelationSetting(Guid workflowId, Guid activityId,string relationWorkflowName,int settringType,int triggerType,int operateType, bool status,string fieldsAssemble,string relationCreator, string creator)
        {
            this.WorkflowId = workflowId;
            this.ActivityId = activityId;
            this.RelationWorkflowName = relationWorkflowName;
            this.SettingType = settringType;
            this.TriggerType = triggerType;
            this.OperateType = operateType;
            this.Status = status;
            this.FieldsAssemble = fieldsAssemble;
            this.RelationCreator = relationCreator;
            this.LastModifier = this.Creator = creator;
            this.LastModTime = this.CreatedTime = DateTime.Now;
        }

        public void Update()
        {
            if (this.Id > 0)
                IBatisMapper.Update("cz_WorkflowRelationSetting_Update", this);
            else
                IBatisMapper.Insert("cz_WorkflowRelationSetting_Insert", this);
        }

        public static CZWorkflowRelationSetting SelectById(int id)
        {
            return IBatisMapper.Load<CZWorkflowRelationSetting>("cz_WorkflowRelationSetting_Select_By_Id", id);
        }

        public static IList<CZWorkflowRelationSetting> Select(Guid workflowId)
        {
            return IBatisMapper.Select<CZWorkflowRelationSetting>("cz_WorkflowRelationSetting_Select", workflowId);
        }

        public static void Delete(int id)
        {
            IBatisMapper.Delete("cz_WorkflowRelationSetting_Delete", id);
        }

        public static CZWorkflowRelationSetting GetRelationSetting(Guid activityId)
        {
            return IBatisMapper.Load<CZWorkflowRelationSetting>("cz_WorkflowRelationSetting_Select_By_Aid", activityId);
        }

        public static DataTable GetRelations(Guid workflowInstanceId)
        {
            return APIServiceSQLHelper.QueryForDataSet("cz_RelationWorkflowInstance_Select", workflowInstanceId);
        }

        /// <summary>
        /// 获取指定工单子流程的工单数
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        public static int GetRelationsCnt(Guid workflowInstanceId)
        {
            return IBatisMapper.Load<int>("cz_RelationWorkflowInstance_Select_Count", workflowInstanceId);
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

        public static string SetWorkflowInstanceRelation(Guid relationWorkflowInstanceId, string creator, Guid WorkflowInstanceId)
        {
            try
            {
                Hashtable parameters = new Hashtable();
                parameters.Add("RelationWorkflowInstanceId", relationWorkflowInstanceId);
                parameters.Add("WorkflowInstanceId", WorkflowInstanceId);
                parameters.Add("Creator", creator);
                IBatisMapper.Insert("cz_WorkflowInstanceRelation_Insert", parameters);
                return "关联子工单["+relationWorkflowInstanceId+"]成功";
            }
            catch (Exception ex)
            {
                log.Error("关联子工单[" + relationWorkflowInstanceId + "]失败，" + ex.ToString());
                return "关联子工单[" + relationWorkflowInstanceId + "]失败，" + ex.Message;
            }
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
            DataTable workflowTable = GetRelations(workflowInstanceId);
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
                Guid? id = DbUtils.ToGuid(row["RelationWorkflowInstanceId"]);
                if (!id.HasValue)
                    continue;
                bool isRefAttachment = DbUtils.ToBoolean(row["IsRefAttachment"]);
                // 输出行.
                result.AppendFormat("<tr{0}>", i % 2 == 1 ? " class=\"trClass\"" : string.Empty);
                result.AppendFormat("<td><a href=\"{1}?wiid={2}\" target=\"_blank\">{0}</a></td>", DbUtils.ToString(row["SheetId"]), pathView, id);
                result.AppendFormat("<td style=\"text-align:left;\"><a href=\"{1}?wiid={2}\" target=\"_blank\">{0}</a></td>", DbUtils.ToString(row["Title"]), pathView, id);
                result.AppendFormat("<td>{0}</td>", DbUtils.ToString(row["CurrentActivityNames"]));
                //result.AppendFormat("<td><input type=\"checkbox\" disabled=\"disabled\"{0} /></td>", isRefAttachment ? " checked=\"checked\"" : string.Empty);

                result.AppendFormat("<td>{0}</td>", DbUtils.ToString(row["Creator"]));
                result.AppendFormat("<td>{0:yy-MM-dd HH:mm:ss}</td>", DbUtils.ToDateTime(row["StartedTime"]));
                result.AppendFormat("<td>{0}</td>", FormatActors(DbUtils.ToString(row["CurrentActors"])));
                if (editable)
                {
                   // result.AppendFormat("<td style=\"vertical-align:middle;padding-top:3px;\"><a class=\"ico_del\" title=\"删除关联\" href=\"javascript:void(0);\" onclick=\"deleteHistoryRelation(this,'{0}');\">删除</a></td>", DbUtils.ToInt32(row["Id"]));
                }
                result.AppendLine("</tr>");
            }
            return result.ToString();
        }

        public static string FormatActors(string currentActors)
        {
            if (string.IsNullOrEmpty(currentActors))
                return string.Empty;
            StringBuilder builder = new StringBuilder();
            string[] actors = currentActors.Split(',', '，');
            foreach (string item in actors)
            {
                int index = item.LastIndexOf('/');
                builder.AppendFormat(",{0}", (index > -1 && index < item.Length - 2) ? item.Substring(index + 1) : item);
            }
            if (builder.Length > 0)
                builder = builder.Remove(0, 1);
            return builder.ToString();
        }
    }
}
