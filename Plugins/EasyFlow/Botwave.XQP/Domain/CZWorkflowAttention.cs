using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Botwave.Commons;
using Botwave.Extension.IBatisNet;

namespace Botwave.XQP.Domain
{
    /// <summary>
    /// 我的关注信息类.
    /// </summary>
    [Serializable]
    public class CZWorkflowAttention : Botwave.Entities.TrackedEntity
    {
        public int Id { get; set; }

        public Guid WorkflowInstanceId { get; set; }

        public string Comment { get; set; }

        public int Type { get; set; }

        public int Status { get; set; }

        public CZWorkflowAttention()
        { }

        public CZWorkflowAttention(Guid workflowInstanceId, int type, string comment, string creator)
        {
            this.WorkflowInstanceId = workflowInstanceId;
            this.Type = type;
            this.Comment = comment;
            this.LastModifier = this.Creator = creator;
            this.LastModTime = this.CreatedTime = DateTime.Now;
        }

        public static bool Create(Guid workflowInstanceId, int type, string comment, string creator)
        {
            CZWorkflowAttention item = new CZWorkflowAttention(workflowInstanceId, type, comment, creator);
            item.Status = 1;
            if (Exists(workflowInstanceId, creator) > -1)
            {
                IBatisMapper.Update("cz_WorkflowAttention_Update", item);
            }
            else
            {
                IBatisMapper.Insert("cz_WorkflowAttention_Insert", item);
            }
            return true;
        }

        public static int Exists(Guid workflowInstanceId, string creator)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("WorkflowInstanceId", workflowInstanceId);
            parameters.Add("Creator", creator);

            object value = IBatisMapper.Load<object>("cz_WorkflowAttention_ExistsStatus", parameters);
            return DbUtils.ToInt32(value, -1);
        }

        public static bool Delete(Guid workflowInstanceId, string creator)
        {
            if (string.IsNullOrEmpty(creator))
                return false;

            Hashtable parameters = new Hashtable();
            parameters.Add("WorkflowInstanceId", workflowInstanceId);
            parameters.Add("Creator", creator);
            parameters.Add("Status", 0);

            IBatisMapper.Update("cz_WorkflowAttention_UpdateStatus", parameters);

            return true;
        }

        public static int Delete(params string[] ids)
        {
            if (ids == null || ids.Length == 0)
                return 0;
            StringBuilder command = new StringBuilder();
            foreach (string id in ids)
            {
                command.AppendFormat("update cz_WorkflowAttention set Status=0, LastModTime=getdate() where [ID]='{0}';", id);
            }
            return IBatisDbHelper.ExecuteNonQuery(CommandType.Text, command.ToString());
        }

        public static DataTable GetView(string username, string type, string keywords, int pageIndex, int pageSize, ref int recordCount)
        {
            username = DbUtils.FilterSQL(username);
            keywords = keywords == null ? null : DbUtils.FilterSQL(keywords);

            string tableName = "vw_cz_WorkflowAttention_Detail";
            string fieldKey = "Id";
            string fieldShow = "Id, WorkflowAlias, WorkflowInstanceId, SheetId, Type, Status, Creator, LastModTime, State, StartedTime, ExpectFinishedTime, Title, CreatorName, CurrentActivities, CurrentActors";
            string fieldOrder = "StartedTime desc, ExpectFinishedTime";
            string where = string.Format(" Creator='{0}'", username);
            if (!string.IsNullOrEmpty(keywords))
                where = where + string.Format(" and (SheetId like '%{0}%' or [Title] like '%{0}%' or [CreatorName] like '%{0}%')", keywords);
            if (type != null)
                where = where + string.Format(" and ([Type] in({0}))", type == string.Empty ? "-1" : type);

            return IBatisDbHelper.GetPagedList(tableName, fieldKey, pageIndex, pageSize, fieldShow, fieldOrder, where, ref recordCount);
        }
    }
}
