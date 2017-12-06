using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.Administrative.BirthControl
{
    public class BirthControlConfig
    {
        public int id { get; set; }
        public Nullable<DateTime> ConfirmStartdate { get; set; }
        public Nullable<DateTime> ConfirmEnddate { get; set; }
        public string ManAge { get; set; }
        public string WomenAge { get; set; }
        public string IsPush { get; set; }

    }
}
