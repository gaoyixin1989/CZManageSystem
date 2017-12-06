using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow.Extension.Domain;
using Botwave.Workflow.Extension.Service;

namespace Botwave.XQP.Service.Plugins
{
    public class WorkflowFieldService : IWorkflowFieldService
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(WorkflowFieldService));

        #region IWorkflowFieldService 成员

        public FieldInfo GetField(Guid workflowId, string fieldName)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("WorkflowId", workflowId);
            parameters.Add("FieldName", fieldName);

            return IBatisMapper.Load<FieldInfo>("bwwf_ControllableFields_Select_ByWorkflowIdAndField", parameters);
        }

        public IList<FieldInfo> GetControllableFields(Guid workflowId)
        {
            return IBatisMapper.Select<FieldInfo>("bwwf_ControllableFields_Select_ByWorkflowId", workflowId);
        }

        public void UpdateFieldControls(IList<FieldControlInfo> items)
        {
            if (items == null || items.Count == 0)
                return;
            try
            {
                IBatisMapper.Mapper.BeginTransaction();
                foreach (FieldControlInfo item in items)
                {
                    if (item.Id <= 0) // -1 时，为新增
                    {
                        // 新增
                        IBatisMapper.Insert("xqp_WorkflowFieldControl_Insert", item);
                    }
                    else
                    {
                        // 更新.
                        IBatisMapper.Update("xqp_WorkflowFieldControl_Update_ById", item);
                    }
                }
                IBatisMapper.Mapper.CommitTransaction();
            }
            catch
            {
                IBatisMapper.Mapper.RollBackTransaction();
            }
        }

        public IList<FieldControlInfo> GetFieldControls(string workflowName, string activityName, string fieldName)
        {
            Hashtable paramters = new Hashtable();
            paramters.Add("WorkflowName", workflowName);
            paramters.Add("ActivityName", activityName);
            paramters.Add("FieldName", fieldName);

            return IBatisMapper.Select<FieldControlInfo>("xqp_WorkflowFieldControl_Select_ByFieldAndActivity", paramters);
        }

        #endregion
    }
}
