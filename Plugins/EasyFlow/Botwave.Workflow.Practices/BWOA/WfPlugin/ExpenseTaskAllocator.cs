using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Botwave.Commons;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Allocator;
using Botwave.Workflow.Service;
using Botwave.Workflow.Plugin;
using Botwave.DynamicForm.Domain;
using Botwave.DynamicForm.Services;
using Botwave.Extension.IBatisNet;

namespace Botwave.Workflow.Practices.BWOA.WfPlugin
{
    /// <summary>
    /// 报销预算任务分派类.
    /// </summary>
    public class ExpenseTaskAllocator : ITaskAllocator
    {
        private IFormInstanceService formInstanceService;

        public IFormInstanceService FormInstanceService
        {
            set { formInstanceService = value; }
        }
        /// <summary>
        /// 预算人前缀.
        /// </summary>
        public const string BudgetMan = "UserNameN";
        /// <summary>
        /// 预算部门前缀.
        /// </summary>
        public const string BudgetDept = "DeptsN";

        /// <summary>
        /// 字段键名.
        /// </summary>
        const string FieldName = "F1";

        #region ITaskAllocator Members

        public IList<string> GetTargetUsers(TaskVariable variable)
        {            
            IDictionary<string, object> variableProperties = variable.Properties;
            if (variableProperties == null || variableProperties.Count == 0)
            {
                return new List<string>();
                //throw new WorkflowAllocateException("未找到表单定义参数变量.");
            }

            //return GetBudgetMans(variableProperties); // 表单数据

            return GetBudgetMans(variable, variableProperties); // 数据库

        }

        #endregion

        /// <summary>
        /// 从实例表组合出表单字典

        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected IDictionary<string, object> GetFormDictFromInstance(string id)
        {
            IDictionary<string, object> dict = new Dictionary<string, object>();
            if (string.IsNullOrEmpty(id))
                return dict;

            string fName = String.Empty;
            string value = String.Empty;

            Guid formInstanceId = new Guid(id);
            IList<FormItemInstance> itemList = formInstanceService.GetFormItemInstancesByFormInstanceId(formInstanceId, true);
            foreach (FormItemInstance item in itemList)
            {
                fName = item.Definition.FName;
                if (item.Definition.ItemDataType == FormItemDefinition.DataType.String || item.Definition.ItemDataType == FormItemDefinition.DataType.Decimal
                    || item.Definition.ItemDataType == FormItemDefinition.DataType.File)
                    value = item.Value;
                else if (item.Definition.ItemDataType == FormItemDefinition.DataType.Text)
                    value = item.TextValue;
                if (!dict.Keys.Contains(fName))
                    dict.Add(fName, value);
            }

            return dict;
        }

        /// <summary>
        /// 根据字段获取处理人.
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="variableProperties"></param>
        /// <returns></returns>
        private IList<string> GetBudgetMans(TaskVariable variable, IDictionary<string, object> variableProperties)
        {
            string workflowInstanceId = DbUtils.FilterSQL(variable.Id);
            string activityName = DbUtils.FilterSQL(variable.Expression.ToString());

            // 获取字段值条件(即项目/部门).
            IList<string> depts = GetBudgetDepts(variableProperties);
            if(depts.Count == 0)
                return new List<string>();
                        
            string sqlTemplate = @"SELECT FieldValue, ExtCondition, TargetUsers, CreatedTime
                FROM xqp_WorkflowFieldControl
                WHERE (FieldName = '{0}') AND (ActivityName = '{1}') AND WorkflowName = (
	                SELECT WorkflowName FROM bwwf_Workflows WHERE WorkflowId = (
		                SELECT WorkflowId FROM bwwf_Tracking_Workflows WHERE WorkflowInstanceId = '{2}')
	                )";
            string sql = string.Format(sqlTemplate, FieldName, activityName, workflowInstanceId);

            IDictionary<string, string> dict = new Dictionary<string, string>();
            using (IDataReader reader = IBatisDbHelper.ExecuteReader(CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    string fieldValue = reader.IsDBNull(0) ? null : reader.GetString(0);
                    string extCondition = reader.IsDBNull(1) ? null : reader.GetString(1);
                    string targetUsers = reader.IsDBNull(2) ? null : reader.GetString(2);
                    if (string.IsNullOrEmpty(fieldValue) || string.IsNullOrEmpty(targetUsers))
                        continue;

                    fieldValue = fieldValue.Trim().ToLower();
                    targetUsers = targetUsers.Trim().ToLower();
                    if (!dict.ContainsKey(fieldValue))
                    {
                        dict.Add(fieldValue, targetUsers);
                    }
                }
                reader.Close();
            }
            if (dict.Count == 0)
                return new List<string>();

            IList<string> results = new List<string>();
            foreach (string dept in depts)
            {
                string key = dept.Trim().ToLower();
                if (dict.ContainsKey(key))
                {
                    string targetUsers = dict[key];
                    this.AddUsers(results, targetUsers);
                }
            }
            return results;
        }

        /// <summary>
        /// 根据表单项属性获取处理人.
        /// </summary>
        /// <param name="variableProperties"></param>
        /// <returns></returns>
        private IList<string> GetBudgetMans(IDictionary<string, object> variableProperties)
        {
            return GetPropertyValues(variableProperties, BudgetMan);
        }

        /// <summary>
        /// 获取预算部门列表.
        /// </summary>
        /// <param name="variableProperties"></param>
        /// <returns></returns>
        private IList<string> GetBudgetDepts(IDictionary<string, object> variableProperties)
        {
            return GetPropertyValues(variableProperties, BudgetDept);
        }

        /// <summary>
        /// 添加用户列表中.
        /// </summary>
        /// <param name="users"></param>
        /// <param name="targetUsers"></param>
        private void AddUsers(IList<string> users, string targetUsers)
        {
            if (string.IsNullOrEmpty(targetUsers))
                return;
            string[] userArray = targetUsers.Split(',','，');
            if (userArray == null)
                return;
            foreach (string userName in userArray)
            {
                if (!users.Contains(userName))
                {
                    users.Add(userName);
                }
            }
        }

        /// <summary>
        /// 从属性字典中获取指定匹配字符串且有规律的属性值列表.
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="patternText"></param>
        /// <returns></returns>
        public static IList<string> GetPropertyValues(IDictionary<string, object> properties, string patternText)
        {
            IList<string> results = new List<string>();

            int index = 0;
            foreach (string key in properties.Keys)
            {
                string postfix = patternText + index;
                if (key.EndsWith(postfix, StringComparison.OrdinalIgnoreCase))
                {
                    index++;
                    string propertyValue = properties[key].ToString();
                    if (string.IsNullOrEmpty(propertyValue) || results.Contains(propertyValue))
                        continue;
                    results.Add(propertyValue.Trim());
                }
            }
            return results;
        }
    }
}
