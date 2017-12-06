using CZManageSystem.Data.Domain.CollaborationCenter.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Admin.Models
{

    public class HallViewModels
    {  
        public string DpId { get; set; }
        public Guid? HallID { get; set; }
        public string HallName { get; set; }
        public List<PaymentPayee> PaymentPayees { get; set; }
        public string DpFullName { get; set; }

    }
    public class CompanyViewModels
    {
        public CompanyViewModels()
        {
            this.PaymentPayers = new List<PaymentPayer>();

        }
        public Guid DpId { get; set; }
        public Nullable<Guid> ParentDpId { get; set; }
        public string DpCode { get; set; }
        public Nullable<int> DpLv { get; set; }
        public string DpName { get; set; }
        public string DpFullName { get; set; } 
        public Nullable<DateTime> CreateTime { get; set; }
        
        public virtual ICollection<PaymentPayer> PaymentPayers { get; set; }

    }
}
