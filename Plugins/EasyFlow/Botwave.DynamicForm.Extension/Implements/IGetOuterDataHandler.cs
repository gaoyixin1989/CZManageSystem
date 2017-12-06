using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Botwave.DynamicForm.Extension.Domain;
using Botwave.Workflow.Domain;
using System.Data;

namespace Botwave.DynamicForm.Extension.Implements
{
    public interface IGetOuterDataHandler
    {
        string getDataJson(FormItemExtension item, string dataType, IDictionary<string, string> dict);

        /// <summary>
        /// 自动计算 加减乘除
        /// </summary>
        /// <param name="activityDefinition"></param>
        /// <param name="workflowInstanceid"></param>
        /// <returns></returns>
        string GenerAutoFull(ActivityDefinition activityDefinition, Guid workflowInstanceid);

        /// <summary>
        /// 过滤参数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dataSet"></param>
        //string FilterSqlString(string sql,IDictionary<string, string> dataSet);

        /// <summary>
        /// 通过sql获取数据源，返回DataTable集合
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="sql"></param>
        /// <param name="dataType"></param>
        /// <param name="dict"></param>
        /// <returns></returns>
        DataTable GetDataTableBySQL(string dataSource, string sql, IDictionary<string, string> dict);
    }
}
