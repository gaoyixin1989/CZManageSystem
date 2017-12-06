using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.Administrative.VehicleManages
{
	public class CarApplyFee
	{
		/// <summary>
		/// ����
		/// <summary>
		public Guid ApplyFeeId { get; set;}
        /// <summary>
		/// ����ʵ��Id
		/// <summary>
		public Nullable<Guid> CarApplyId { get; set; }
        /// <summary>
        /// ���̵���
        /// </summary>
        public string ApplySn { get; set; }
        /// <summary>
        /// ����
        /// <summary>
        public string ApplyTitle { get; set; }
        /// <summary>
        /// ����ID
        /// <summary>
        public Nullable<Guid> CarId { get; set;}
		/// <summary>
		/// ������
		/// <summary>
		public string BalUser { get; set;}
		/// <summary>
		/// ����ʱ��
		/// <summary>
		public Nullable<DateTime> BalTime { get; set;}
		/// <summary>
		/// ��ʼ���
		/// <summary>
		public Nullable<int> KmNum1 { get; set;}
		/// <summary>
		/// ��ֹ���
		/// <summary>
		public Nullable<int> KmNum2 { get; set;}
		/// <summary>
		/// ʹ�����
		/// <summary>
		public Nullable<int> KmCount { get; set;}
		/// <summary>
		/// ��������
		/// <summary>
		public Nullable<int> BalCount { get; set;}
		/// <summary>
		/// �ϼƽ��
		/// <summary>
		public Nullable<decimal> BalTotal { get; set;}
		/// <summary>
		/// ��ע
		/// <summary>
		public string BalRemark { get; set;}

        public virtual CarApply CarApply { get; set; }

    }
}
