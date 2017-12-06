using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.HumanResources.Integral
{
	public class HRIntegralTemp
	{
		public Guid IntegralId { get; set;}
		/// <summary>
		/// �ô�ID
		/// <summary>
		public Nullable<Guid> UserId { get; set;}
		/// <summary>
		/// ����
		/// <summary>
		public Nullable<decimal> Integral { get; set;}
		/// <summary>
		/// ���
		/// <summary>
		public Nullable<int> YearDate { get; set;}
		/// <summary>
		/// ��ע
		/// <summary>
		public string Remark { get; set;}
		/// <summary>
		/// ������Դ
		/// <summary>
		public Nullable<decimal> Source { get; set;}
		/// <summary>
		/// ɾ��
		/// <summary>
		public Nullable<bool> Del { get; set;}
		/// <summary>
		/// ����
		/// <summary>
		public string IntegralType { get; set;}
		/// <summary>
		/// ��ѵ����
		/// <summary>
		public Nullable<decimal> CIntegral { get; set;}
		/// <summary>
		/// �ڿλ���
		/// <summary>
		public Nullable<int> TIntegral { get; set;}
		/// <summary>
		/// ���뵥ID
		/// <summary>
		public Nullable<Guid> ApplyId { get; set;}
		public Nullable<decimal> TPeriodTime { get; set;}
        public Nullable<Guid> Daoid { get; set; }
        public string FinishTime { get; set;}

	}
}
