using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.ITSupport
{
    //�Ĳĵ�ƽ������ϸ��
    public class Consumable_LevellingDetail
	{
		public Guid ID { get; set;}
		/// <summary>
		/// ���뵥ID
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
