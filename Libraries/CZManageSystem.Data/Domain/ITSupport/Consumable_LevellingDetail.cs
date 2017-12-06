using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.ITSupport
{
    //耗材调平申请明细表
    public class Consumable_LevellingDetail
	{
		public Guid ID { get; set;}
		/// <summary>
		/// 申请单ID
		/// <summary>
		public Nullable<Guid> ApplyID { get; set;}
		/// <summary>
		/// 耗材ID
		/// <summary>
		public Nullable<int> ConsumableID { get; set;}
		/// <summary>
		/// 数量
		/// <summary>
		public Nullable<int> Amount { get; set;}

	}
}
