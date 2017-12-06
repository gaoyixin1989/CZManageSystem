using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Botwave.Commons;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Plugin;
using Botwave.Extension.IBatisNet;
using Botwave.Entities;
using ibatis = IBatisNet;

namespace Botwave.Workflow.IBatisNet
{
    public class TaskAssignService : ITaskAssignService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(TaskAssignService));

        private IPostAssignHandler postAssignHandler;
        private IActivityAllocationService activityAllocationService;

        public IPostAssignHandler PostAssignHandler
        {
            set { postAssignHandler = value; }
        }

        public IActivityAllocationService ActivityAllocationService
        {
            set { activityAllocationService = value; }
        }

        #region ITaskAssignService Members

        public void Assign(Assignment assignment)
        {
            Guid activityInstanceId = assignment.ActivityInstanceId.Value;
            string actor = assignment.AssigningUser;     // 转交人
            string newActor = assignment.AssignedUser; // 被转交人

            TodoInfo todo = new TodoInfo();
            todo.ActivityInstanceId = activityInstanceId;
            todo.OperateType = TodoInfo.OpAssign;
            todo.UserName = newActor;

            ibatis.DataMapper.ISqlMapper mapper = IBatisMapper.Mapper;
            try
            {
                mapper.BeginTransaction();

                // 插入被转交人的待办信息
                mapper.Insert("bwwf_Todo_Insert", todo);

                // 插入转交记录信息
                mapper.Insert("bwwf_Assignment_Insert", assignment);

                // 删除转交人的原待办信息.
                Hashtable parameters = new Hashtable();
                parameters.Add("ActivityInstanceId", activityInstanceId);
                parameters.Add("UserName", actor);
                mapper.Delete("bwwf_Todo_Delete_Item_ByActivityUserName", parameters);

                mapper.CommitTransaction();
            }
            catch (Exception ex)
            {
                mapper.RollBackTransaction();
                log.Error(ex);
                throw ex;
            }

            ProcessPostChain(assignment);
        }

        public IList<Assignment> GetAssignments(Guid workflowInstanceId)
        {
            return IBatisMapper.Select<Assignment>("bwwf_Assignment_Select_By_WorkflowInstanceId", workflowInstanceId);
        }

        public AllocatorOption GetAssignmentAllocator(Guid activityId)
        {
            return IBatisMapper.Load<AllocatorOption>("bwwf_AssignmentAllocator_Select_ByActivityId", activityId);
        }

        public int UpdateAssignmentAllocators(AllocatorOption option)
        {
            return IBatisMapper.Update("bwwf_AssignmentAllocator_Update", option);
        }

        public IList<BasicUser> GetUsers4Assignment(Guid activityInstanceId)
        {
            return IBatisMapper.Select<BasicUser>("bwwf_Assignment_Select_Users_By_ActivityInstanceId", activityInstanceId);
        }

        public IDictionary<string, string> GetAssignmentActors(Guid workflowInstnaceId, Guid activityInstanceId, string actor)
        {
            AllocatorOption options = IBatisMapper.Load<AllocatorOption>("bwwf_AssignmentAllocator_Select_ByActivityInstanceId", activityInstanceId);
            if (options == null)
                return new Dictionary<string, string>();
            return activityAllocationService.GetTargetUsers(workflowInstnaceId, options, actor, false);
        }

        public DataTable GetAssignmentTasks(string actor, string workflowName, string keywords, string beginTime, string endTime, int pageIndex, int pageSize, ref int recordCount)
        {
            string tableName = "vw_bwwf_Tracking_Assignments_Tasks";
            string fieldKey = "Title";
            string fieldShow = "ActivityInstanceId, AssignedUser, AssigningUser, AssignedTime,  AssignedRealName, IsCompleted, ActivityName, CreatorName, SheetId, Title, WorkflowAlias, AliasImage, CurrentActivityNames, CurrentActors";
            string fieldOrder = "AssignedTime desc";

            StringBuilder where = new StringBuilder();
            where.AppendFormat("(AssigningUser = '{0}')", actor);
            if (!string.IsNullOrEmpty(workflowName))
                where.AppendFormat("AND (WorkflowName = '{0}')", workflowName);
            if (!string.IsNullOrEmpty(keywords))
            {
                keywords = DbUtils.FilterSQL(keywords);
                where.AppendFormat(" AND (Title LIKE '%{0}%')", keywords);
            }
            if (!string.IsNullOrEmpty(beginTime))
            {
                where.AppendFormat(" AND (AssignedTime >= '{0}')", beginTime);
            }
            if (!string.IsNullOrEmpty(endTime))
            {
                where.AppendFormat(" AND (AssignedTime <= '{0}')", endTime);
            }

            DataTable dt = IBatisDbHelper.GetPagedList(tableName, fieldKey, pageIndex, pageSize, fieldShow, fieldOrder, where.ToString(), ref recordCount);
            return dt;
        }

        public IList<BasicUser> GetTodoActors(Guid activityInstanceId)
        {
            return IBatisMapper.Select<BasicUser>("bwwf_Todo_Select_CurrentActors", activityInstanceId);
        }

        public IList<TodoInfo> GetNextTodoInfo(Guid activityInstanceId)
        {
            return IBatisMapper.Select<TodoInfo>("bwwf_Todo_Select_Next_By_ActivityInstanceId", activityInstanceId);
        }

        public void InsertTodo(TodoInfo item)
        {
            IBatisMapper.Insert("bwwf_Todo_Insert", item);
        }

        public int DeleteTodo(Guid activityInstanceId)
        {
            return IBatisMapper.Delete("bwwf_Todo_Delete", activityInstanceId);
        }

        public int DeleteTodo(Guid activityInstanceId, string userName)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("ActivityInstanceId", activityInstanceId);
            parameters.Add("UserName", userName);
            return IBatisMapper.Delete("bwwf_Todo_Delete_Item_ByActivityUserName", parameters);
        }

        public int UpdateTodoReaded(Guid activityInstanceId, string userName, bool isReaded)
        {
            Hashtable parameters = new Hashtable(3);
            parameters.Add("ActivityInstanceId", activityInstanceId);
            parameters.Add("UserName", userName);
            parameters.Add("State", isReaded ? 1 : 0);

            return IBatisMapper.Update("bwwf_Todo_UpdateReaded", parameters);
        }

        public bool IsExistsTodo(Guid activityInstanceId, string userName)
        {
            TodoInfo todo = GetTodoInfo(activityInstanceId, userName);
            return (null != todo);
        }

        public TodoInfo GetTodoInfo(Guid activityInstanceId, string userName)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("ActivityInstanceId", activityInstanceId);
            parameters.Add("UserName", userName);
            return IBatisMapper.Load<TodoInfo>("bwwf_Todo_Select_By_ActivityInstanceId_UserName", parameters);
        }

        public bool IsAssignFromCompany(Guid activityInstanceId)
        {
            AllocatorOption options = IBatisMapper.Load<AllocatorOption>("bwwf_AssignmentAllocator_Select_ByActivityInstanceId", activityInstanceId);
            if (null == options)
                return false;

            string extAllocatorArgs = options.ExtendAllocatorArgs;
            if (string.IsNullOrEmpty(extAllocatorArgs))
                return false;

            IList<object> args = new List<object>();
            string[] allocatorArgs = extAllocatorArgs.Replace(" ", "").Split(';');
            foreach (string allocatorArg in allocatorArgs)
            {
                string[] allocatorArray = allocatorArg.Split(':');
                int lengthOfAllocatorArray = allocatorArray.Length;
                if (lengthOfAllocatorArray == 0)
                    continue;

                string expression = lengthOfAllocatorArray > 1 ? allocatorArray[1] : string.Empty;
                if (expression.Length > 0)
                {
                    string[] argsArray = expression.Split(',');
                    foreach (string item in argsArray)
                    {
                        if (!string.IsNullOrEmpty(item))
                            args.Add(item);
                    }
                }
            }
            if (null != args && args.Contains("9"))
                return true;
            return false;
        }

        #endregion

        private void ProcessPostChain(Assignment assignment)
        {
            if (null != postAssignHandler)
            {
                postAssignHandler.Execute(assignment);
                IPostAssignHandler next = postAssignHandler.Next;
                while (null != next)
                {
                    next.Execute(assignment);
                    next = next.Next;
                }
            }
        }
    }
}
