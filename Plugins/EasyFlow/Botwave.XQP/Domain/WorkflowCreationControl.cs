using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Botwave.Extension.IBatisNet;

namespace Botwave.XQP.Domain
{
    /// <summary>
    /// 流程发单控制.
    /// </summary>
    public class WorkflowCreationControl
    {
        #region gets / sets

        private int id;
        private string workflowName;
        private int urgency;
        private string creationControlType;
        private int maxCreationInMonth;
        private int maxCreationInWeek;

        /// <summary>
        /// ID.
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 流程Name.
        /// </summary>
        public string WorkflowName
        {
            get { return workflowName; }
            set { workflowName = value; }
        }

        /// <summary>
        /// 工单紧急程度值.
        /// 0:表示一般;
        /// 1:表示紧急;
        /// 2:表示最紧急.
        /// </summary>
        public int Urgency
        {
            get { return urgency; }
            set { urgency = value; }
        }

        /// <summary>
        /// 发单数量控制类型.
        /// 默认(NULL 或空值)为总体控件.
        /// user:用户控制.
        /// dept:部门控制.
        /// room:科室控制.
        /// </summary>
        public string CreationControlType
        {
            get { return creationControlType; }
            set { creationControlType = value; }
        }

        /// <summary>
        /// 每月的最大发单数量.
        /// </summary>
        public int MaxCreationInMonth
        {
            get { return maxCreationInMonth; }
            set { maxCreationInMonth = value; }
        }

        /// <summary>
        /// 每周的最大发单数量.
        /// </summary>
        public int MaxCreationInWeek
        {
            get { return maxCreationInWeek; }
            set { maxCreationInWeek = value; }
        }
        #endregion

        #region methods

        /// <summary>
        /// 创建发单控制.
        /// </summary>
        public void Insert()
        {
            if (!IsExists(workflowName, urgency))
            {
                IBatisMapper.Insert("xqp_WorkflowCreationControl_Insert", this);
            }
            else
            {
                this.Update();
            }
        }

        /// <summary>
        /// 更新发单控制.
        /// </summary>
        /// <returns></returns>
        public int Update()
        {
            return IBatisMapper.Update("xqp_WorkflowCreationControl_Update_ByWorkflowName", this);
        }

        /// <summary>
        /// 检查指定发单控制是否存在.
        /// </summary>
        /// <param name="workflowName"></param>
        /// <param name="urgency"></param>
        /// <returns></returns>
        public static bool IsExists(string workflowName, int urgency)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("WorkflowName", workflowName);
            parameters.Add("Urgency", urgency);
            object result = IBatisMapper.Mapper.QueryForObject("xqp_WorkflowCreationControl_Select_IsExists", parameters);
            return (result != null);
        }

        /// <summary>
        /// 获取知道流程名称的流程发单控制列表.
        /// </summary>
        /// <param name="workflowName"></param>
        /// <returns></returns>
        public static IList<WorkflowCreationControl> GetWorkflowCreationControls(string workflowName)
        {
            return IBatisMapper.Select<WorkflowCreationControl>("xqp_WorkflowCreationControl_Select_ByWorkflowName", workflowName);
        }

        /// <summary>
        /// 获取知道流程编号的流程发单控制列表.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        public static IList<WorkflowCreationControl> GetWorkflowCreationControls(Guid workflowId)
        {
            return IBatisMapper.Select<WorkflowCreationControl>("xqp_WorkflowCreationControl_Select_ByWorkflowId", workflowId);
        }

        /// <summary>
        /// 获取指定流程编号以及紧急重要程度值的流程发单控制对象.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="urgency"></param>
        /// <returns></returns>
        public static WorkflowCreationControl GetWorkflowCreationControl(Guid workflowId, int urgency)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("WorkflowId", workflowId);
            parameters.Add("Urgency", urgency);

            return IBatisMapper.Load<WorkflowCreationControl>("xqp_WorkflowCreationControl_Select_ByWorkflowIdAndUrgency", parameters);
        }
        #endregion
    }
}
