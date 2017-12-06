using CZManageSystem.Data.Domain.CollaborationCenter.Invest;
using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using CZManageSystem.Data.Domain.Composite;
using CZManageSystem.Data.Domain.HumanResources.Vacation;
using CZManageSystem.Data.Domain.ITSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Domain.SysManger
{
    /// <summary>
    ///流程实例
    /// </summary>
    public partial class Tracking_Workflow
    {
        public Tracking_Workflow()
        {
            this.VoteApplys = new List<VoteApply>();
        }
        public Guid WorkflowInstanceId { get; set; }
        public Guid WorkflowId { get; set; }
        public string SheetId { get; set; }
        public int State { get; set; }
        public string Creator { get; set; }
        public DateTime StartedTime { get; set; }
        public Nullable<DateTime> FinishedTime { get; set; }
        public string Title { get; set; }
        public int Secrecy { get; set; }
        public Nullable<byte> Urgency { get; set; }
        public Nullable<byte> Importance { get; set; }
        public Nullable<DateTime> ExpectFinishedTime { get; set; }
        public string Requirement { get; set; }
        public Nullable<int> CommentCount { get; set; }
        public Nullable<int> PrintCount { get; set; }
        public virtual ICollection<VoteApply> VoteApplys { get; set; }
        public virtual ICollection<Consumable_Cancelling> Consumable_Cancellings { get; set; }
        public virtual ICollection<Consumable_Levelling> Consumable_Levellings { get; set; }
        public virtual ICollection<InvestAgoEstimateApply> InvestAgoEstimateApplys { get; set; }

        public virtual ICollection<HRVacationApply> HRVacationApplys { get; set; }
        public virtual ICollection<HRVacationCloseApply> HRVacationCloseApplys { get; set; }
        public virtual ICollection<HRReVacationApply> HRReVacationApplys { get; set; }

        public virtual ICollection<MarketOrder_OrderApply> MarketOrder_OrderApplys { get; set; }


    }
}
