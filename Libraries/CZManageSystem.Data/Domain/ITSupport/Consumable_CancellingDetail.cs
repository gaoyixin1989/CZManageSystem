using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.ITSupport
{
	public class Consumable_CancellingDetail
	{
		public Guid ID { get; set;}
		/// <summary>
		/// ÍË¿âÉêÇëµ¥ID
		/// <summary>
		public Nullable<Guid> ApplyID { get; set;}
		/// <summary>
		/// ºÄ²ÄID
		/// <summary>
		public Nullable<int> ConsumableID { get; set;}
		/// <summary>
		/// ÍË¿âÊıÁ¿
		/// <summary>
		public Nullable<int> CancelNumber { get; set;}

	}
}
