using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.ITSupport
{
	public class Consumable_InputList
	{
		public int ID { get; set;}
		/// <summary>
		/// ����
		/// <summary>
		public string Title { get; set;}
		/// <summary>
		/// ��ⵥ��
		/// <summary>
		public string Code { get; set;}
		/// <summary>
		/// ����ʱ��
		/// <summary>
		public Nullable<DateTime> CreateTime { get; set;}
		/// <summary>
		/// ���ʱ��
		/// <summary>
		public Nullable<DateTime> InputTime { get; set;}
		/// <summary>
		/// ������
		/// <summary>
		public Nullable<Guid> Operator { get; set; }
        /// <summary>
        /// �ύ��
        /// <summary>
        public Nullable<Guid> SumbitUser { get; set; }
        /// <summary>
        /// ��ע
        /// <summary>
        public string Remark { get; set; }
        /// <summary>
        /// ��ⵥ״̬��0-���棬1-�ύ���
        /// <summary>
        public Nullable<int> State { get; set; }

    }
}
