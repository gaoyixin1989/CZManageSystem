using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.ITSupport
{
	public class Consumable_InputDetail
	{
		public int ID { get; set;}
		/// <summary>
		/// 入库列表单ID
		/// <summary>
		public Nullable<int> InputListID { get; set;}
		/// <summary>
		/// 耗材ID
		/// <summary>
		public Nullable<int> ConsumableID { get; set;}
		/// <summary>
		/// 入库数量
		/// <summary>
		public Nullable<int> Amount { get; set;}

	}
}
