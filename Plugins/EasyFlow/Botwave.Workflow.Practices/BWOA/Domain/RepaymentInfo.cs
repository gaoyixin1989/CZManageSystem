using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Practices.BWOA.Domain
{
    public class RepaymentInfo
    {
        #region Model
        private Guid _id;
        private Guid _activityid;
        private DateTime _paydate;
        private string _payman;
        private string _paynum;
        private string _arrearsnum;
        private string _explain;
        /// <summary>
        /// 
        /// </summary>
        public Guid ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Guid ActivityID
        {
            set { _activityid = value; }
            get { return _activityid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime PayDate
        {
            set { _paydate = value; }
            get { return _paydate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PayMan
        {
            set { _payman = value; }
            get { return _payman; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string payNum
        {
            set { _paynum = value; }
            get { return _paynum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ArrearsNum
        {
            set { _arrearsnum = value; }
            get { return _arrearsnum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Explain
        {
            set { _explain = value; }
            get { return _explain; }
        }
        #endregion Model
    }
}
