using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Botwave.Workflow.Routing.Implements
{
    public interface IActivityRulesHelper
    {
        /// <summary>
        /// 导入旧版本的规则
        /// </summary>
        /// <param name="workflowname"></param>
        /// <returns></returns>
        bool ImportOldRules(string workflowname);

        /// <summary>
        /// 获取规则信息
        /// </summary>
        /// <param name="workflowid"></param>
        /// <returns></returns>
        DataTable GetRulesInfo(string workflowid);

        /// <summary>
        /// 导入规则
        /// </summary>
        /// <param name="dtRules"></param>
        /// <param name="workflowId"></param>
        void ImportRules(DataTable dtRules, Guid workflowId);
    }
}
