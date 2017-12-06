using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CZManageSystem.Data.Domain.SysManger;

/// <summary>
/// ��ʷ��Ŀ�ݹ������
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.Invest
{
    //�ݹ������ѯ
    public class AgoEstimateApplyQueryBuilder
    {
        public string Title { get; set; }
        public int? State { get; set; }
        public Guid? AppPerson { get; set; }
        public DateTime? ApplyTime_start { get; set; }
        public DateTime? ApplyTime_end { get; set; }
        public int? WF_State { get; set; }
    }

    public class InvestAgoEstimateApply
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
		public Nullable<Guid> AppPerson { get; set;}
		/// <summary>
		/// �������ֻ�
		/// <summary>
		public string Mobile { get; set;}
		/// <summary>
		/// �������
		/// <summary>
		public string Title { get; set;}
		/// <summary>
		/// ԭ��
		/// <summary>
		public string Content { get; set;}
		/// <summary>
		/// ״̬��0���桢1�ύ
		/// <summary>
		public Nullable<int> State { get; set;}

        //���
        public virtual Tracking_Workflow Tracking_Workflow { get; set; }
        public virtual Depts AppDeptObj { get; set; }
        public virtual Users AppPersonObj { get; set; }

    }
}
