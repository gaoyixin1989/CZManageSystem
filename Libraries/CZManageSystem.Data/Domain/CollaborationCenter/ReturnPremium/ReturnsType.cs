using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.SysManger
{
	public class ReturnsType
	{
		public Guid ID { get; set;}
		/// <summary>
		/// ÕÀ∑—∑Ω Ω
		/// <summary>
		public string Type { get; set;}
		/// <summary>
		/// ≈≈–Ú
		/// <summary>
		public Nullable<int> Order { get; set;}

	}
}
