using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// ���Ͷ�ʽ��
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.Invest
{
	public class InvestProjectYearTotal
	{
		public Guid ID { get; set;}
		/// <summary>
		/// ��ĿID
		/// <summary>
		public string ProjectID { get; set;}
		/// <summary>
		/// ���
		/// <summary>
		public Nullable<int> Year { get; set;}
		/// <summary>
		/// ���ܼ�
		/// <summary>
		public Nullable<decimal> YearTotal { get; set;}

	}
}
