using System;
using System.Collections.Generic;

namespace CZManageSystem.Data.Models
{
    public partial class TB_Log_ExceptionCatalog
    {
        public TB_Log_ExceptionCatalog()
        {
            this.TB_Log_OperateLog = new List<TB_Log_OperateLog>();
        }

        public string exceptionID { get; set; }
        public string parentID { get; set; }
        public string exceptionName { get; set; }
        public System.DateTime startTime { get; set; }
        public Nullable<System.DateTime> endTime { get; set; }
        public string description { get; set; }
        public bool available { get; set; }
        public virtual ICollection<TB_Log_OperateLog> TB_Log_OperateLog { get; set; }
    }
}
