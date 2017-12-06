using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.SysManger
{
	public class InvestCorrectApplySubList
	{
		public Guid ID { get; set;}
		public Nullable<Guid> EstimateID { get; set;}
		public Nullable<Guid> ApplyID { get; set;}
		public Nullable<int> Year { get; set;}
		public Nullable<int> Month { get; set;}
		public string ProjectName { get; set;}
		public string ProjectID { get; set;}
		public string ContractName { get; set;}
		public string ContractID { get; set;}
		public string Supply { get; set;}
		public Nullable<decimal> SignTotal { get; set;}
		public Nullable<decimal> PayTotal { get; set;}
		public string Study { get; set;}
		public Nullable<Guid> ManagerID { get; set;}
		public string Course { get; set;}
		public Nullable<decimal> BackRate { get; set;}
		public Nullable<decimal> Rate { get; set;}
		public Nullable<decimal> Pay { get; set;}
		public Nullable<decimal> NotPay { get; set;}
		public Nullable<Guid> EstimateUserID { get; set;}

	}
}