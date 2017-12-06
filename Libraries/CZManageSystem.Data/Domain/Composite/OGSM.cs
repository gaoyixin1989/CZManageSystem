using CZManageSystem.Core;
using System;
using System.Collections.Generic;

namespace CZManageSystem.Data.Domain.Composite
{
    public partial class OGSM 
    {
        public int Id { get; set; }
        public string Group_Name { get; set; }
        public string Town { get; set; }
        public string USR_NBR { get; set; }
        public string PowerStation { get; set; }
        public string BaseStation { get; set; }
        public string PowerType { get; set; }
        public string PropertyRight { get; set; }
        public string IsRemove { get; set; }
        public Nullable<System.DateTime> RemoveTime { get; set; }
        public string IsShare { get; set; }
        public Nullable<System.DateTime> ContractStartTime { get; set; }
        public Nullable<System.DateTime> ContractEndTime { get; set; }
        public string Price { get; set; }
        public string Address { get; set; }
        public Nullable<int> PAY_CYC { get; set; }
        public string Property { get; set; }
        public string LinkMan { get; set; }
        public string Mobile { get; set; }
        public string IsWarn { get; set; }
        public Nullable<int> WarnCount { get; set; }
        public string Remark { get; set; }
        public string IsNew { get; set; }

        public System.Guid AttachmentId { get; set; }
    }
    public class OGSMQueryBuilder
    {
        public string USR_NBR { get; set; }
        public string[] Group_Name { get; set; }
        public string[] BaseStation { get; set; }
        public string PowerType { get; set; }
        public string[] PropertyRight { get; set; }
        public string IsShare { get; set; }
        public string IsRemove { get; set; }
        public DateTime? ContractEndTime_Start { get; set; }
        public DateTime? ContractEndTime_End { get; set; }
    }
}
