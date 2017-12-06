using System;
using System.Collections.Generic;

namespace CZManageSystem.Data.Models
{
    public partial class TB_Log_OperateLog
    {
        public int uid { get; set; }
        public System.DateTime opStartTime { get; set; }
        public string portalID { get; set; }
        public string clientIP { get; set; }
        public string clientComputerName { get; set; }
        public string serverIP { get; set; }
        public Nullable<System.DateTime> opEndTime { get; set; }
        public string operationID { get; set; }
        public string exceptionID { get; set; }
        public string description { get; set; }
        public virtual TB_Log_ExceptionCatalog TB_Log_ExceptionCatalog { get; set; }
        public virtual TB_Log_OperationCatalog TB_Log_OperationCatalog { get; set; }
    }
}
