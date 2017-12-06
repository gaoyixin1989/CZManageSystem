using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.SysManger
{
	public class InvestCorrectApply
	{
		public Guid ID { get; set;}
        public Nullable<Guid> WorkflowInstanceId { get; set; }
        public string Series { get; set;}
		public Nullable<DateTime> ApplyTime { get; set;}
		public string ApplyDpCode { get; set;}
		public string Applicant { get; set;}
		public string Mobile { get; set;}
        public Nullable<int> State { get; set; }
        public string Title { get; set;}
		public string Content { get; set;}

	}
}
