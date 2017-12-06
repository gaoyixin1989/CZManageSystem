using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.CollaborationCenter.Payment
{
    public class V_PaymentPaymentApplySub
    {
        public Guid ApplyID { get; set; }
        public string ApplyTitle { get; set; }
        public Nullable<Guid> WorkflowInstanceId { get; set; }
        public string ApplySn { get; set; }
        public string Mobile { get; set; }
        public Nullable<Guid> MainApplyID { get; set; }
        public Nullable<DateTime> PayDay { get; set; }
        public string Series { get; set; }
        public string AppliCant { get; set; }
        public Nullable<DateTime> ApplyTime { get; set; }
        public string Status { get; set; }
        public string SubStatus { get; set; }
        public string HallID { get; set; }
        public Nullable<Guid> CompanyID { get; set; }
        public string Remark { get; set; }

    }
}
