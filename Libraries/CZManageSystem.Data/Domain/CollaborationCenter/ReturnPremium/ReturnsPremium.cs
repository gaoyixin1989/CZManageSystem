using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.SysManger
{
	public class ReturnsPremium
	{
		public Guid ID { get; set;}
		/// <summary>
		/// �ֻ�����
		/// <summary>
		public string Mobile { get; set;}
		/// <summary>
		/// �˷ѽ��
		/// <summary>
		public Nullable<decimal> Money { get; set;}
		/// <summary>
		/// �˷�����
		/// <summary>
		public string Range { get; set;}
		/// <summary>
		/// �˷ѷ�ʽ
		/// <summary>
		public string Type { get; set;}
		/// <summary>
		/// �˷�ԭ��
		/// <summary>
		public string Causation { get; set;}
		/// <summary>
		/// ���˵��
		/// <summary>
		public string Explain { get; set;}
		/// <summary>
		/// �·�
		/// <summary>
		public string Month { get; set;}
		/// <summary>
		/// ����
		/// <summary>
		public Nullable<DateTime> Date { get; set;}
		/// <summary>
		/// �Ǽ�����
		/// <summary>
		public string Channel { get; set;}
		/// <summary>
		/// SP�˿ں�
		/// <summary>
		public string SpPort { get; set;}
		/// <summary>
		/// �˷���ϸԭ��
		/// <summary>
		public string Remark { get; set;}
		public string Series { get; set;}

	}
}
