using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Practices.BWOA.Domain
{
    public class SysDeployInfo
    {
        #region Model
        private Guid _id;
        private Guid _workflowid;
        private Guid _workflowinstanceid;
        private string _title;
        private string _deployer;
        private string _englishname;
        private DateTime _deploydt;
        private string _deptname;
        private string _pm;
        private string _remark;
        private string _url;
        private string _reason;
        private string _doway;
        private string _result;
        private string _feedback;
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
        /// 部署人
        /// </summary>
        public string Deployer
        {
            set { _deployer = value; }
            get { return _deployer; }
        }
        /// <summary>
        /// 部署人英文名
        /// </summary>
        public string EnglishName
        {
            set { _englishname = value; }
            get { return _englishname; }
        }
        /// <summary>
        /// 部署日期
        /// </summary>
        public DateTime DeployDT
        {
            set { _deploydt = value; }
            get { return _deploydt; }
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
        /// 部署内容
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// 更新路径
        /// </summary>
        public string URL
        {
            set { _url  = value; }
            get { return _url; }
        }
        /// <summary>
        /// 更新原因
        /// </summary>
        public string Reason
        {
            set { _reason = value; }
            get { return _reason; }
        }
        /// <summary>
        /// 更新步骤
        /// </summary>
        public string DoWay
        {
            set { _doway = value; }
            get { return _doway; }
        }
        /// <summary>
        /// 更新结果
        /// </summary>
        public string Result
        {
            set { _result = value; }
            get { return _result; }
        }
        /// <summary>
        /// 部署反馈结果
        /// </summary>
        public string FeedBack
        {
            set { _feedback = value; }
            get { return _feedback; }
        }
        public DateTime CreateDT
        {
            set { _createdt = value; }
            get { return _createdt; }
        }
        #endregion
    }
}
