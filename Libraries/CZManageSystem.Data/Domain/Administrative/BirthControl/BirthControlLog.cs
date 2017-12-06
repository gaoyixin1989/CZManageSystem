using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.Administrative.BirthControl
{
    public class BirthControlLog
    {
        public int Id { get; set; }
        public Nullable<Guid> UserId { get; set; }
        public string UserName { get; set; }
        public string UserIp { get; set; }
        public string OpType { get; set; }
        public Nullable<DateTime> OpTime { get; set; }
        public string Description { get; set; }


    }
}
