using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.MarketPlan
{
	public class Ucs_MarketPlan3
	{
		public Guid Id { get; set;}
		public string Coding { get; set;}
		public string Name { get; set;}
		public Nullable<DateTime> StartTime { get; set;}
		public Nullable<DateTime> EndTime { get; set;}
		public string Channel { get; set;}
		public string Orders { get; set;}
		public string RegPort { get; set;}
		public string DetialInfo { get; set;}
		public string Remark { get; set;}
		public string PlanType { get; set;}
		public string TargetUsers { get; set;}
		public string PaysRlues { get; set;}
		public string Templet1 { get; set;}
		public string Templet2 { get; set;}
		public string Templet3 { get; set;}
		public string Templet4 { get; set;}
		public string Tel { get; set;}
		public string IsMarketing { get; set;}
		public Nullable<int> status { get; set;}

	}
}
