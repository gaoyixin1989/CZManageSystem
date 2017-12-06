using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace CZManageSystem.Data.Domain.SysManger
{
    public partial class SysServiceStrategyLog
    {
        public ObjectId Id { get; set; }
        public Nullable<System.DateTime> LogTime { get; set; }
        public Nullable<int> ServiceStrategyId { get; set; }
        public Nullable<bool> Result { get; set; }
        public string Description { get; set; }
    }
}
