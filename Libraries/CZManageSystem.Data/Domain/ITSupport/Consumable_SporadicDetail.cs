using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.SysManger
{
	public class Consumable_SporadicDetail
	{
		public Nullable<Guid> ID { get; set;}
		public Nullable<Guid> ApplyID { get; set;}
		public string Relation { get; set;}
		public Nullable<int> ApplyCount { get; set;}
		public Nullable<decimal> Amount { get; set;}

    }
}
