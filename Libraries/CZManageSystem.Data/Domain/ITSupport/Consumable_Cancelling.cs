using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CZManageSystem.Data.Domain.SysManger;

namespace CZManageSystem.Data.Domain.ITSupport
{
	public class Consumable_Cancelling
	{
		/// <summary>
		/// ���
		/// <summary>
		public Guid ID { get; set;}
		/// <summary>
		/// ����ʵ��ID
		/// <summary>
		public Nullable<Guid> WorkflowInstanceId { get; set;}
		/// <summary>
		/// ���̵���
		/// <summary>
		public string Series { get; set;}
		/// <summary>
		/// ����ʱ��
		/// <summary>
		public Nullable<DateTime> ApplyTime { get; set;}
		/// <summary>
		/// ����id
		/// <summary>
		public string AppDept { get; set;}
		/// <summary>
		/// ������
		/// <summary>
		public string AppPerson { get; set;}
		/// <summary>
		/// �������ֻ�
		/// <summary>
		public string Mobile { get; set;}
		/// <summary>
		/// �������
		/// <summary>
		public string Title { get; set;}
        /// <summary>
        /// �˿�ԭ��
        /// <summary>
        public string Content { get; set;}
		/// <summary>
		/// ״̬��0���桢1�ύ
		/// <summary>
		public Nullable<int> State { get; set;}

        public virtual Tracking_Workflow Tracking_Workflow { get; set; }

    }
}
