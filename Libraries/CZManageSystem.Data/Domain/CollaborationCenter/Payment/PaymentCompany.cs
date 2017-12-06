using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.CollaborationCenter.Payment
{
    public class PaymentCompany
    {
        public PaymentCompany()
        {
            this.PaymentPayers = new List<PaymentPayer>();
            this.PaymentCompanyHalls = new List<PaymentCompanyHall>();
        }
        public Guid DpId { get; set; }
        public Nullable<Guid> ParentDpId { get; set; }
        public string DpCode { get; set; }
        public Nullable<int> DpLv { get; set; }
        public string DpName { get; set; }
        public string DpFullName { get; set; }
        public string DpDesc { get; set; }
        public Nullable<int> IsShow { get; set; }
        public string Status { get; set; }
        public string OrderNo { get; set; }
        public string IsTmpDP { get; set; }
        public Nullable<DateTime> CreateTime { get; set; }
        public Nullable<DateTime> UpdateTime { get; set; }
        public Nullable<int> AllowDist { get; set; }
        public virtual ICollection<PaymentPayer> PaymentPayers { get; set; }
        public virtual ICollection<PaymentCompanyHall> PaymentCompanyHalls { get; set; }

    }
}
