using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Practices.BWOA.Domain
{
    public class ApplyStat
    {
        #region Model
        private Guid _id;
        private string _applyname;
        private string _applytype;
        private string _depts;
        private string _invoicenum;
        private string _rreceiptnum;
        private string _chequenum;
        private string _summary;
        private decimal _banknum;
        private decimal _cashnum;
        private string _budgetman;
        private DateTime _happenDate;
        private DateTime _applydate;
        private string _remark;
        private Guid _workflowinstanceid;
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
        public string ApplyName
        {
            set { _applyname = value; }
            get { return _applyname; }
        }
        /// <summary>
        /// 报销类别
        /// </summary>
        public string ApplyType
        {
            set { _applytype = value; }
            get { return _applytype; }
        }
        /// <summary>
        /// 项目/部门
        /// </summary>
        public string Depts
        {
            set { _depts = value; }
            get { return _depts; }
        }
        /// <summary>
        /// 发票号
        /// </summary>
        public string InvoiceNum
        {
            set { _invoicenum = value; }
            get { return _invoicenum; }
        }
        /// <summary>
        /// 收据号
        /// </summary>
        public string RreceiptNum
        {
            set { _rreceiptnum = value; }
            get { return _rreceiptnum; }
        }
        /// <summary>
        /// 支票号
        /// </summary>
        public string ChequeNum
        {
            set { _chequenum = value; }
            get { return _chequenum; }
        }
        /// <summary>
        /// 摘要
        /// </summary>
        public string Summary
        {
            set { _summary = value; }
            get { return _summary; }
        }
        /// <summary>
        /// 银行账
        /// </summary>
        public decimal BankNum
        {
            set { _banknum = value; }
            get { return _banknum; }
        }
        /// <summary>
        /// 现金账
        /// </summary>
        public decimal CashNum
        {
            set { _cashnum = value; }
            get { return _cashnum; }
        }
        /// <summary>
        /// 预算人
        /// </summary>
        public string BudgetMan
        {
            set { _budgetman = value; }
            get { return _budgetman; }
        }
        public DateTime HappenDate
        {
            set { _happenDate = value; }
            get { return _happenDate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime ApplyDate
        {
            set { _applydate = value; }
            get { return _applydate; }
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
        /// 
        /// </summary>
        public Guid WorkFlowInstanceID
        {
            set { _workflowinstanceid = value; }
            get { return _workflowinstanceid; }
        }
        #endregion Model
    }
}
