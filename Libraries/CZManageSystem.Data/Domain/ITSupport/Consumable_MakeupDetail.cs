using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.ITSupport
{
	public class Consumable_MakeupDetail
	{
		public Guid ID { get; set;}
		/// <summary>
		/// ²¹Â¼¹éµµID
		/// <summary>
		public Nullable<Guid> ApplyID { get; set;}
		/// <summary>
		/// ºÄ²ÄID
		/// <summary>
		public Nullable<int> ConsumableID { get; set;}
		/// <summary>
		/// ÊýÁ¿
		/// <summary>
		public Nullable<int> Amount { get; set;}

	}
}
