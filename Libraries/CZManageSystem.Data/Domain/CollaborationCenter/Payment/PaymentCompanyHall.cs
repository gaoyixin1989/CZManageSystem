using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CZManageSystem.Data.Domain.SysManger;

namespace CZManageSystem.Data.Domain.CollaborationCenter.Payment
{
    public class PaymentCompanyHall
    {
        public Guid DcId { get; set; }
        public string DpId { get; set; }
        public Nullable<Guid> CompanyId { get; set; }
        public virtual Depts Depts { get; set; }
    }
}
