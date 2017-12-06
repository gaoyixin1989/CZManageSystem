using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Domain.Administrative.Dinning
{
    public class OrderMeal_MenuPackageCommand
    {
        public Guid Id { get; set; }
        public Guid MenuId { get; set; }
        public Guid PackageId { get; set; }
        public string PackageName { get; set; }
        public Nullable<Guid> CommandId { get; set; }
        public string Command { get; set; }
        public Nullable<int> State { get; set; }

    }

}
