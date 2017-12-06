using System;
using System.Collections.Generic;
using System.Configuration;

namespace Botwave.Workflow
{
    /// <summary>
    /// 流程配置节点类.
    /// </summary>
    public class WorkflowConfig : ConfigurationSection
    {
        private static WorkflowConfig instance = null;

        /// <summary>
        /// 实例对象.
        /// </summary>
        public static WorkflowConfig Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (typeof(WorkflowConfig))
                    {
                        if (instance == null)
                        {
                            instance = ConfigurationManager.GetSection("botwave/workflow") as WorkflowConfig;
                            if(instance == null)
                                instance = ConfigurationManager.GetSection("botwave.workflow") as WorkflowConfig;
                            if (instance == null)
                            {
                                instance = new WorkflowConfig();
                                instance.AllowExecutionHandler = true;
                                instance.AllowDecisionParser = true;
                                instance.AllowCommandRules = true;
                                instance.AllowCountersigned = true;
                                instance.AllowContinuousApprove = false;
                                instance.AllowComplexFlow = true;
                                instance.AllowUserProxy = true;
                                instance.DefaultRejectOption = "initial";
                                instance.CommandsDisabledRules = null;
                            }
                        }
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// 是否允许自定义活动执行逻辑.
        /// </summary>
        [ConfigurationProperty("allowExecutionHandler", DefaultValue = true)]
        public bool AllowExecutionHandler
        {
            get { return (bool)this["allowExecutionHandler"]; }
            set { this["allowExecutionHandler"] = value; }
        }

        /// <summary>
        /// 是否允许任务分配决策解析器.
        /// </summary>
        [ConfigurationProperty("allowDecisionParser", DefaultValue = true)]
        public bool AllowDecisionParser
        {
            get { return (bool)this["allowDecisionParser"]; }
            set { this["allowDecisionParser"] = value; }
        }

        /// <summary>
        /// 是否允许自定义规则解析.
        /// </summary>
        [ConfigurationProperty("allowCommandRules", DefaultValue = true)]
        public bool AllowCommandRules
        {
            get { return (bool)this["allowCommandRules"]; }
            set { this["allowCommandRules"] = value; }
        }

        /// <summary>
        /// 是否允许会签.
        /// </summary>
        [ConfigurationProperty("allowCountersigned", DefaultValue = true)]
        public bool AllowCountersigned
        {
            get { return (bool)this["allowCountersigned"]; }
            set { this["allowCountersigned"] = value; }
        }

        /// <summary>
        /// 是否允许连续审批.
        /// </summary>
        [ConfigurationProperty("allowContinuousApprove", DefaultValue = false)]
        public bool AllowContinuousApprove
        {
            get { return (bool)this["allowContinuousApprove"]; }
            set { this["allowContinuousApprove"] = value; }
        }

        /// <summary>
        /// 是否允许复杂流程.
        /// </summary>
        [ConfigurationProperty("allowComplexFlow", DefaultValue = true)]
        public bool AllowComplexFlow
        {
            get { return (bool)this["allowComplexFlow"]; }
            set { this["allowComplexFlow"] = value; }
        }

        /// <summary>
        /// 是否允许用户代理.
        /// </summary>
        [ConfigurationProperty("allowUserProxy", DefaultValue = true)]
        public bool AllowUserProxy
        {
            get { return (bool)this["allowUserProxy"]; }
            set { this["allowUserProxy"] = value; }
        }

        /// <summary>
        /// 默认的退回选择        
        /// </summary>
        [ConfigurationProperty("defaultRejectOption", DefaultValue = "initial")]
        public string DefaultRejectOption
        {
            get { return (string)this["defaultRejectOption"]; }
            set { this["defaultRejectOption"] = value; }
        }

        /// <summary>
        /// 禁用了规则的命令列表        
        /// 格式:命令1,命令2,...
        /// </summary>
        [ConfigurationProperty("commandsDisabledRules", DefaultValue = null)]
        public string CommandsDisabledRules
        {
            get { return (string)this["commandsDisabledRules"]; }
            set { this["commandsDisabledRules"] = value; }
        }
    }
}
