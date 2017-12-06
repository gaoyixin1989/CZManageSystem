using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Domain.Administrative.BirthControl
{
    public class BirthControlApply
    {
        public int Id { get; set; }
        public Nullable<Guid> WorkflowInstanceId { get; set; }
        public Nullable<Guid> UserId { get; set; }
        public string SheetId { get; set; }
        public string Title { get; set; }
        public Nullable<DateTime> ApplyTime { get; set; }
        public string CurrentActivity { get; set; }
        public string CurrentActors { get; set; }
        public Nullable<int> State { get; set; }
    }

    public class BirthControlApplyUser
    {
        public Nullable<Guid> UserId { get; set; }
        public string RealName { get; set; }
        public string UserName { get; set; }
    }
}
