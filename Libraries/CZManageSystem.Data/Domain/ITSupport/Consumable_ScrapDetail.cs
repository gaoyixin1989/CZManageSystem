using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.SysManger
{
	public class Consumable_ScrapDetail
	{
		public Nullable<Guid> ID { get; set;}
		public string ApplyID { get; set;}
		public Nullable<int> ConsumableID { get; set;}
		public Nullable<int> ScrapNumber { get; set;}

	}
}
