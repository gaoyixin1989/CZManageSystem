using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CZManageSystem.Data.Domain.SysManger;

namespace CZManageSystem.Data.Domain.CollaborationCenter.Payment
{
	public class PaymentHall
    {
        public PaymentHall()
        {
            this.PaymentPayees = new List<PaymentPayee>();
           
        }

        /// <summary>
        /// 服营厅ID
        /// <summary>
        public Guid HallID { get; set;}
		/// <summary>
		/// 服营厅名称
		/// <summary>
		public string HallName { get; set;}
        /// <summary>
        /// 关联服营厅DpId
        /// <summary>
        public string DpId { get; set;}
        public virtual ICollection<PaymentPayee> PaymentPayees { get; set; }
        public virtual Depts Depts { get; set; }
    }
}
