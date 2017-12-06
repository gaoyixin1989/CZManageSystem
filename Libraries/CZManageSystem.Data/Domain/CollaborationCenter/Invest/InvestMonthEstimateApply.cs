using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.SysManger
{
	public class InvestMonthEstimateApply
	{
		public Guid ApplyID { get; set;}
        public Nullable<Guid> WorkflowInstanceId { get; set; }
        public string Series { get; set;}
		public Nullable<DateTime> ApplyTime { get; set;}
		public string ApplyDpCode { get; set;}
		public string Applicant { get; set;}
		public string Mobile { get; set;}
		public string Status { get; set;}
		public string Title { get; set;}
        public virtual Tracking_Workflow TrackingWorkflow { get; set; }
       // public virtual Depts DeptInfo { get; set; }

    }
}
