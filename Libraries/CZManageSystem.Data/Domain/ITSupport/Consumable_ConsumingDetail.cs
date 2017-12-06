using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.SysManger
{
	public class Consumable_ConsumingDetail
	{
		public Nullable<Guid> ID { get; set;}
		public Nullable<Guid> ApplyID { get; set;}
		public Nullable<int> ConsumableID { get; set;}
		public Nullable<int> ClaimsNumber { get; set;}
		public Nullable<int> AllotNumber { get; set;}

	}
}
