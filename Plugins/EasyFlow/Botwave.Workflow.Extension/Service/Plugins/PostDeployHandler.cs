using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Plugin;
using Botwave.Workflow.Service;
using Botwave.Workflow.Extension.Util;

namespace Botwave.Workflow.Extension.Service.Plugins
{
    /// <summary>
    /// 流程部署的后续处理对象.
    /// </summary>
    public class PostDeployHandler : IPostDeployHandler
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(PostDeployHandler));

        #region service interfaces

        private IWorkflowSettingService workflowSettingService;
        private IWorkflowFormService workflowFormService;

        /// <summary>
        /// 流程设置服务.
        /// </summary>
        public IWorkflowSettingService WorkflowSettingService
        {
            get { return workflowSettingService; }
            set { workflowSettingService = value; }
        }

        /// <summary>
        /// 流程表单服务.
        /// </summary>
        public IWorkflowFormService WorkflowFormService
        {
            get { return workflowFormService; }
            set { workflowFormService = value; }
        }

        #endregion

        #region IPostDeployHandler 成员

        private IPostDeployHandler next = null;

        /// <summary>
        /// 下一后续处理器对象.
        /// </summary>
        public IPostDeployHandler Next
        {
            get { return next; }
            set { next = value; }
        }

        /// <summary>
        /// 执行.
        /// </summary>
        /// <param name="oldWorkflow"></param>
        /// <param name="newWorkflow"></param>
        /// <param name="newActivities"></param>
        public void Execute(WorkflowDefinition oldWorkflow, WorkflowDefinition newWorkflow, ICollection<DeployActivityDefinition> newActivities)
        {
            string workflowName = newWorkflow.WorkflowName;
            log.Warn("deploy handler:" + workflowName);

            ParallelActivityManager.Build(newWorkflow.WorkflowId);

            this.UpdateForm(oldWorkflow, newWorkflow);
            this.UpdateSetting(workflowName);
        }

        #endregion

        #region private methods

        /// <summary>
        /// 更新流程表单.
        /// </summary>
        /// <param name="oldWorkflow"></param>
        /// <param name="newWorkflow"></param>
        private void UpdateForm(WorkflowDefinition oldWorkflow, WorkflowDefinition newWorkflow)
        {
            if (this.workflowFormService != null)
            {
                this.workflowFormService.DeployForm(oldWorkflow, newWorkflow);
            }
        }

        /// <summary>
        /// 更新流程设置.
        /// </summary>
        /// <param name="workflowName"></param>
        private void UpdateSetting(string workflowName)
        {
            if (workflowSettingService == null || string.IsNullOrEmpty(workflowName))
                return;
            WorkflowSetting item = workflowSettingService.GetWorkflowSetting(workflowName);
            if (item == null)
            {
                item = WorkflowSetting.Default;
                item.WorkflowName = workflowName;
                workflowSettingService.InsertSetting(item);
            }
        }

        #endregion

        /// <summary>
        /// 管理/维护并行结点
        /// </summary>
        static class ParallelActivityManager
        {
            /*
             * 根据条件获取活动列表（全部或指定流程）activities
             * for 活动activity in activities
             *  查找activity的前继活动集合prevSets
             *  if prevSets.Count > 1   (如果尺寸大于1，说明有并行分支)
             *      for 活动a in prevSets
             *          查找a的并行活动集合parallelSets
             *          if parallelSets 为空    
             *              创建parallelSets并将prevSets中除a之外的元素都加入parallelSets中
             *          else
             *              将prevSets中除a之外的元素都并入parallelSets中(有重复则跳过);
             *          保存a的并行活动集合(创建 or 更新)
             */
            //public static void Build()
            //{
            //    Build(Guid.Empty);
            //}

            public static void Build(Guid workflowId)
            {
                IList<KeyValuePair<Guid, Guid>> activities = new List<KeyValuePair<Guid, Guid>>();
                string sql = "select ActivityId,PrevActivitySetId from bwwf_Activities where State = 1";
                if (workflowId != Guid.Empty)
                {
                    sql = String.Format("select ActivityId,PrevActivitySetId from bwwf_Activities where State = 1 and WorkflowId = '{0}'", workflowId);
                }
                using (IDataReader reader = IBatisDbHelper.ExecuteReader(CommandType.Text, sql))
                {
                    while (reader.Read())
                    {
                        KeyValuePair<Guid, Guid> pair = new KeyValuePair<Guid, Guid>(reader.GetGuid(0), reader.GetGuid(1));
                        activities.Add(pair);
                    }
                }

                Build(activities);
            }

            public static void Build(IList<KeyValuePair<Guid, Guid>> activities)
            {
                foreach (KeyValuePair<Guid, Guid> activity in activities)
                {
                    IList<Guid> prevSet = GetActivitySet(activity.Value);
                    if (prevSet.Count > 1)
                    {
                        foreach (Guid activityId in prevSet)
                        {
                            Guid parallelSetId = GetParallelSetId(activityId);
                            if (parallelSetId == Guid.Empty)
                            {
                                IList<Guid> newParallelSet = new List<Guid>();
                                foreach (Guid tempId in prevSet)
                                {
                                    if (tempId != activityId)
                                    {
                                        newParallelSet.Add(tempId);
                                    }
                                }
                                if (newParallelSet.Count > 0)
                                {
                                    Guid setId = Guid.NewGuid();
                                    AddParallelActivitySet(setId, newParallelSet);
                                    UpdateParallelSetIdOfActivity(activityId, setId);
                                }
                            }
                            else
                            {
                                IList<Guid> parallelSet = GetActivitySet(parallelSetId);
                                IList<Guid> newParallelSet = new List<Guid>();
                                foreach (Guid tempId in prevSet)
                                {
                                    if (tempId != activityId && !parallelSet.Contains(tempId))
                                    {
                                        newParallelSet.Add(tempId);
                                    }
                                }
                                if (newParallelSet.Count > 0)
                                {
                                    AddParallelActivitySet(parallelSetId, newParallelSet);
                                }
                            }
                        }
                    }
                }
            }

            static IList<Guid> GetActivitySet(Guid setId)
            {
                string sql = String.Format("select ActivityId from bwwf_ActivitySet where SetId = '{0}'", setId);
                IList<Guid> list = new List<Guid>();
                using (IDataReader reader = IBatisDbHelper.ExecuteReader(CommandType.Text, sql))
                {
                    while (reader.Read())
                    {
                        list.Add(reader.GetGuid(0));
                    }
                }
                return list;
            }

            static Guid GetParallelSetId(Guid activityId)
            {
                string sql = String.Format("select ParallelActivitySetId from bwwf_Activities where State = 1 and ActivityId = '{0}'", activityId);
                object obj = IBatisDbHelper.ExecuteScalar(CommandType.Text, sql);
                if (null == obj)
                {
                    return Guid.Empty;
                }

                Guid id = new Guid(obj.ToString());
                return id;
            }

            static void AddParallelActivitySet(Guid setId, IList<Guid> parallelSet)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Guid id in parallelSet)
                {
                    sb.AppendFormat("insert into bwwf_ActivitySet (SetId,ActivityId) values ('{0}','{1}');", setId, id);
                }
                IBatisDbHelper.ExecuteNonQuery(CommandType.Text, sb.ToString());
            }

            static void UpdateParallelSetIdOfActivity(Guid activityId, Guid parallelSetId)
            {
                string sql = String.Format("update bwwf_Activities set ParallelActivitySetId = '{0}' where ActivityId = '{1}'", parallelSetId, activityId);
                IBatisDbHelper.ExecuteNonQuery(CommandType.Text, sql);
            }
        }
    }
}
