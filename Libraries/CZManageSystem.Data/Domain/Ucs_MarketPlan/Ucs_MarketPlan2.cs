using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.MarketPlan
{
	public class Ucs_MarketPlan2
    {
		public Guid Id { get; set;}
		/// <summary>
		/// Ӫ����������
		/// <summary>
		public string Coding { get; set;}
		/// <summary>
		/// Ӫ����������
		/// <summary>
		public string Name { get; set;}
		/// <summary>
		/// ��ʼʱ��
		/// <summary>
		public Nullable<DateTime> StartTime { get; set;}
		/// <summary>
		/// ����ʱ��
		/// <summary>
		public Nullable<DateTime> EndTime { get; set;}
		/// <summary>
		/// ��������
		/// <summary>
		public string Channel { get; set;}
		/// <summary>
		/// ָ��
		/// <summary>
		public string Orders { get; set;}
		/// <summary>
		/// ��������ǼǶ˿�
		/// <summary>
		public string RegPort { get; set;}
		/// <summary>
		/// Ӫ���ϸ��
		/// <summary>
		public string DetialInfo { get; set;}
		/// <summary>
		/// ��ע
		/// <summary>
		public string Remark { get; set;}
		/// <summary>
		/// ��������
		/// <summary>
		public string PlanType { get; set;}
		/// <summary>
		/// Ŀ���û�Ⱥ
		/// <summary>
		public string TargetUsers { get; set;}
		/// <summary>
		/// н�������Ҫ
		/// <summary>
		public string PaysRlues { get; set;}
		/// <summary>
		/// ����ģ��1
		/// <summary>
		public string Templet1 { get; set;}
		/// <summary>
		/// ����ģ��2
		/// <summary>
		public string Templet2 { get; set;}
		/// <summary>
		/// ����ģ��3
		/// <summary>
		public string Templet3 { get; set;}
		/// <summary>
		/// ����ģ��4
		/// <summary>
		public string Templet4 { get; set;}
		/// <summary>
		/// ��������
		/// <summary>
		public Nullable<int> NumCount { get; set;}
		/// <summary>
		/// �Ƿ��׼����
		/// <summary>
		public string IsMarketing { get; set;}

	}
}
