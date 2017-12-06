using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Ӫ������-Ӫ������ά��
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder
{
	public class MarketOrder_Market
	{
		/// <summary>
		/// ���
		/// <summary>
		public Guid ID { get; set;}
		/// <summary>
		/// Ӫ����������
		/// <summary>
		public string Market { get; set;}
        /// <summary>
        /// Ӫ���������
        /// <summary>
        public string Order { get; set;}
		/// <summary>
		/// ��Чʱ��
		/// <summary>
		public Nullable<DateTime> AbleTime { get; set;}
		/// <summary>
		/// ʧЧʱ��
		/// <summary>
		public Nullable<DateTime> DisableTime { get; set;}
		/// <summary>
		/// ��ע˵��
		/// <summary>
		public string Remark { get; set;}
		/// <summary>
		/// �Żݷ���
		/// <summary>
		public Nullable<decimal> PlanPay { get; set;}
		/// <summary>
		/// ʵ�շ���
		/// <summary>
		public Nullable<decimal> MustPay { get; set;}
		/// <summary>
		/// �Ƿ�ҿ�ҵ��
		/// <summary>
		public Nullable<bool> isJK { get; set;}

	}
}
