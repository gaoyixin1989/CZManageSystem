using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.MarketPlan
{
	public class Ucs_MarketPlanMonitor
	{
		public Guid Id { get; set;}
		/// <summary>
		/// �ļ�����
		/// <summary>
		public string ImportName { get; set;}
		/// <summary>
		/// ����ʱ��
		/// <summary>
		public Nullable<DateTime> Creattime { get; set;}
		/// <summary>
		/// ��¼��
		/// <summary>
		public Nullable<int> Count { get; set;}
		/// <summary>
		/// ״̬
		/// <summary>
		public string Status { get; set;}
		/// <summary>
		/// ����
		/// <summary>
		public string Remark { get; set;}
		public string ReDownload { get; set;}

	}

    public class MarketPlanMonitorQueryBuilder
    {
        public string ImportName { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
    }
}
