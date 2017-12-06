using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Practices.BWOA.Domain
{
    public class AppLeaveInfo
    {
        #region Model
        private Guid _id;
        private Guid _workflowid;
        private string _applyname;
        private string _leavetype;
        private float _applytotal;
        private DateTime _begindate;
        private DateTime _enddate;
        private DateTime _applydate;


        /// <summary>
        /// 流程ID
        /// </summary>
        public Guid ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 流程ID
        /// </summary>
        public Guid WorkFlowID
        {
            set { _workflowid = value; }
            get { return _workflowid; }
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
        /// 天数
        /// </summary>
        public float ApplyTotal
        {
            set { _applytotal = value; }
            get { return _applytotal; }
        }
        /// <summary>
        /// 类型
        /// </summary>
        public string LeaveType
        {
            set { _leavetype = value; }
            get { return _leavetype; }
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
