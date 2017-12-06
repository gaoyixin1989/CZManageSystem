using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.SysManger
{
	public class ReturnsReason
	{
		public Guid ID { get; set;}
		/// <summary>
		/// ÕÀ∑—‘≠“Ú
		/// <summary>
		public string Reason { get; set;}
		/// <summary>
		/// –Ú∫≈
		/// <summary>
		public Nullable<int> Order { get; set;}

	}
}
