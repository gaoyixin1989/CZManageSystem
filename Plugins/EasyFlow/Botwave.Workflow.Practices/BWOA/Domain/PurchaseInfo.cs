using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Practices.BWOA.Domain
{
   public class PurchaseInfo
    {
        #region Model
        private Guid _id;
        private Guid _workflowid;
        private Guid _workflowinstanceid;
        private string _waterid;
        private string _equipmentname;
        private decimal _quantity;
        private decimal _unitprice;
        private decimal _total;
        private string _remark;
        private DateTime _purchasedate;
        private string _purchaseman;
        private string _tel;


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
        /// 流水ID
        /// </summary>
        public string WaterID
        {
            set { _waterid = value; }
            get { return _waterid; }
        }

        /// <summary>
        /// 采购人

        /// </summary>
        public string PurchaseMan
        {
            set { _purchaseman = value; }
            get { return _purchaseman; }
        }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName
        {
            set { _equipmentname = value; }
            get { return _equipmentname; }
        }
        /// <summary>
        /// 采购日期
        /// </summary>
        public DateTime PurchaseDate
        {
            set { _purchasedate = value; }
            get { return _purchasedate; }
        }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Quantity
        {
            set { _quantity = value; }
            get { return _quantity; }
        }
        /// <summary>
        /// 单价

        /// </summary>
        public decimal  UnitPrice
        {
            set { _unitprice = value; }
            get { return _unitprice; }
        }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Total
        {
            set { _total = value; }
            get { return _total; }
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
        /// 联系人电话
        /// </summary>
        public string Tel
        {
            set { _tel = value; }
            get { return _tel; }
        }      
        
        #endregion
    }
}
