using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// ÓªÏú¶©µ¥-ºÅÂë¶ÎÎ¬»¤
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder
{
	public class MarketOrder_Number
	{
		public Guid ID { get; set;}
		/// <summary>
		/// ĞòºÅ
		/// <summary>
		public Nullable<decimal> Order { get; set;}
		/// <summary>
		/// ºÅÂë¶Î
		/// <summary>
		public string Number { get; set;}

	}
}
