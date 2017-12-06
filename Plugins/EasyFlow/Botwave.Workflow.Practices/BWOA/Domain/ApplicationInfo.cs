using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Practices.BWOA.Domain
{
    public class ApplicationInfo
    {
        #region Model
        private Guid _id;
        private Guid _workflowinstanceid;
        private string _applyname;
        private DateTime _applydate;
        private string _deptname;
        private string _budgetman;
        private decimal _applymoney;
        private string _applytype;
        private string _explain;
        private DateTime _begindate;
        private DateTime _enddate;


        /// <summary>
        /// 流程ID
        /// </summary>
        public Guid ID
        {
            set { _id = value; }
            get { return _id; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid WorkFlowInstanceID
        {
            set { _workflowinstanceid = value; }
            get { return _workflowinstanceid; }
        }

        /// <summary>
        /// 申请人
        /// </summary>
        public string ApplyName
        {
            set { _applyname = value; }
            get { return _applyname; }
        }
        /// <summary>
        /// 申请日期
        /// </summary>
        public DateTime ApplyDate
        {
            set { _applydate = value; }
            get { return _applydate; }
        }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string DeptName
        {
            set { _deptname = value; }
            get { return _deptname; }
        }
        /// <summary>
        /// 预算人
        /// </summary>
        public string BudgetMan
        {
            set { _budgetman = value; }
            get { return _budgetman; }
        }
        /// <summary>
        /// 申请金额
        /// </summary>
        public decimal ApplyMoney
        {
            set { _applymoney = value; }
            get { return _applymoney; }
        }
        /// <summary>
        /// 申请类型
        /// </summary>
        public string ApplyType
        {
            set { _applytype = value; }
            get { return _applytype; }
        }
        /// <summary>
        /// 说明
        /// </summary>
        public string Explain
        {
            set { _explain = value; }
            get { return _explain; }
        }
        public DateTime BeginDate
        {
            set { _begindate = value; }
            get { return _begindate; }
        }
        public DateTime EndDate
        {
            set { _enddate = value; }
            get { return _enddate; }
        }
        #endregion
    }
}
