using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.HumanResources.ShiftManages
{
    /// <summary>
    /// �ְ���Ϣ
    /// </summary>
	public class ShiftLunban
	{
        public ShiftLunban() {
            this.ShiftLbusers = new List<ShiftLbuser>();
        }
		/// <summary>
		/// �ְ�ID
		/// <summary>
		public Guid Id { get; set;}
		/// <summary>
		/// �Ű���Ϣ��ID
		/// <summary>
		public Guid ZhibanId { get; set;}
		/// <summary>
		/// ��ʼ��
		/// <summary>
		public string StartDay { get; set;}
		/// <summary>
		/// ������
		/// <summary>
		public string EndDay { get; set;}


        public virtual ICollection<ShiftLbuser> ShiftLbusers { get; set; }

    }
}
