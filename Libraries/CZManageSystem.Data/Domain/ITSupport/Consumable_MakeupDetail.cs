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
		/// ��¼�鵵ID
		/// <summary>
		public Nullable<Guid> ApplyID { get; set;}
		/// <summary>
		/// �Ĳ�ID
		/// <summary>
		public Nullable<int> ConsumableID { get; set;}
		/// <summary>
		/// ����
		/// <summary>
		public Nullable<int> Amount { get; set;}

	}
}
