using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Botwave.Workflow.Domain;
using Botwave.XQP.Designer;

namespace Botwave.XQP.Domain
{
    /// <summary>
    /// 可视化步骤信息集合
    /// </summary>
    [Serializable]
    public class ProcessData
    {
        /// <summary>
        /// 步骤数量
        /// </summary>
        private int total;

        private string owner;
        /// <summary>
        /// 流程ID
        /// </summary>
        private string flow_id;

        /// <summary>
        /// 流程管理员集合
        /// </summary>
        private string managerIds;

        /// <summary>
        /// 流程备注
        /// </summary>
        private string remark;
        /// <summary>
        /// 步骤信息集合
        /// </summary>
        private IList<list> list;
        private WorkflowSetting setting;
        private WorkflowComponent profile;
        public int Total { 
            get { return total; }
            set { total = value; }
        }

        public string Flow_id
        {
            get { return flow_id; }
            set { flow_id = value; }
        }

        /// <summary>
        /// 创建人
        /// </summary>
        public string Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        /// <summary>
        /// 流程管理员userid集合
        /// </summary>
        public string ManagerIds
        {
            get { return managerIds; }
            set { managerIds = value; }
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }
        /// <summary>
        /// 流程基本设置
        /// </summary>
        public WorkflowSetting Setting
        {
            get { return setting; }
            set { setting = value; }
        }
        /// <summary>
        /// 流程配置信息
        /// </summary>
        public WorkflowComponent Profile
        {
            get { return profile; }
            set { profile = value; }
        }

        public IList<list> List
        {
            get { return list; }
            set { list = value; }
        }
    }

    /// <summary>
    /// 步骤信息
    /// </summary>
    [Serializable]
    public class list
    {
        /// <summary>
        /// 步骤标志
        /// </summary>
        private int id;
        /// <summary>
        /// 步骤类型
        /// </summary>
        private string process_type;
        private string process_name;
        private Guid process_id;
        private IList<string> process_to;
        private WorkflowActivity activity;
        private AllocatorOption assignment;
        private string rules;
        private int top;

        private int left;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Process_type
        {
            get { return process_type; }
            set { process_type = value; }
        }
        public string Process_name
        {
            get { return process_name; }
            set { process_name = value; }
        }
        public Guid Process_id
        {
            get { return process_id; }
            set { process_id = value; }
        }

        public IList<string> Process_to
        {
            get { return process_to; }
            set { process_to = value; }
        }

        public WorkflowActivity Activity
        {
            get { return activity; }
            set { activity = value; }
        }

        public AllocatorOption Assignment
        {
            get { return assignment; }
            set { assignment = value; }
        }

        public string Rules
        {
            get { return rules; }
            set { rules = value; }
        }

        public int Top
        {
            get { return top; }
            set { top = value; }
        }

        public int Left
        {
            get { return left; }
            set { left = value; }
        }
    }
}
