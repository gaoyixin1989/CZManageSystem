using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Practices.BWOA.Domain
{
    public class DocInspectInfo
    {
        #region Model
        private Guid _id;
        private Guid _workflowid;
        private Guid _workflowinstanceid;
        private string _title;
        private string _sender;
        private string _englishname;
        private DateTime _senddt;
        private string _deptname;
        private string _pm;
        private string _doctype;
        private string _remark;
        private DateTime _createdt;


        /// <summary>
        /// ID
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
        /// 
        /// </summary>
        public Guid WorkFlowInstanceID
        {
            set { _workflowinstanceid = value; }
            get { return _workflowinstanceid; }
        }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            set { _title = value; }
            get { return _title; }
        }
        /// <summary>
        /// 文档发起人
        /// </summary>
        public string Sender
        {
            set { _sender = value; }
            get { return _sender; }
        }
        /// <summary>
        /// 英文名
        /// </summary>
        public string EnglishName
        {
            set { _englishname = value; }
            get { return _englishname; }
        }
        /// <summary>
        /// 发起日期
        /// </summary>
        public DateTime SendDT
        {
            set { _senddt = value; }
            get { return _senddt; }
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
        /// 项目负责人
        /// </summary>
        public string PM
        {
            set { _pm = value; }
            get { return _pm; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// 文档类型
        /// </summary>
        public string DocType
        {
            set { _doctype = value; }
            get { return _doctype; }
        }
        public DateTime CreateDT
        {
            set { _createdt = value; }
            get { return _createdt; }
        }
        #endregion
    }
}
