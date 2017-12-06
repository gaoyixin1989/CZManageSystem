using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CZManageSystem.Data.Domain.SysManger;

namespace CZManageSystem.Data.Domain.HumanResources.Vacation
{
	public class HROverTimeApply
	{
		/// <summary>
		/// ����
		/// <summary>
		public Guid ApplyID { get; set;}
		/// <summary>
		/// ����
		/// <summary>
		public string ApplyTitle { get; set;}
		/// <summary>
		/// ����ʵ��Id
		/// <summary>
		public Nullable<Guid> WorkflowInstanceId { get; set;}
		public string ApplySn { get; set;}
		/// <summary>
		/// ������
		/// <summary>
		public string ApplyUserName { get; set;}
		/// <summary>
		/// ��ʼʱ��
		/// <summary>
		public Nullable<DateTime> StartTime { get; set;}
		/// <summary>
		/// ����ʱ��
		/// <summary>
		public Nullable<DateTime> EndTime { get; set;}
		/// <summary>
		/// ����
		/// <summary>
		public Nullable<decimal> PeriodTime { get; set;}
		/// <summary>
		/// ְ��
		/// <summary>
		public string ApplyPost { get; set;}
		/// <summary>
		/// ֱ����������
		/// <summary>
		public string ManageName { get; set;}
		/// <summary>
		/// ֱ������ְ��
		/// <summary>
		public string ManagePost { get; set;}
		/// <summary>
		/// �Ӱ�ص�
		/// <summary>
		public string Address { get; set;}
		/// <summary>
		/// �Ӱ�����
		/// <summary>
		public string OvertimeType { get; set;}
		/// <summary>
		/// �Ӱ�ԭ��
		/// <summary>
		public string Reason { get; set;}
		/// <summary>
		/// �༭��
		/// <summary>
		public Nullable<Guid> Editor { get; set;}
        /// <summary>
        /// ����ʱ��
        /// <summary>
        public Nullable<DateTime> CreateTime { get; set; }
        public string Newpt { get; set;}

        public virtual Tracking_Workflow TrackingWorkflow { get; set; }
    }
}
