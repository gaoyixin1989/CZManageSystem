using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CZManageSystem.Data.Domain.SysManger;

/// <summary>
/// ���ŷ���ͳ����ͼ
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.SmsManager
{
    //��ͬ��Ϣ��ѯ����
    public class V_SendSmsCountQueryBuilder
    {
        public string SenderName { get; set; }//������
        public string DeptFullName { get; set; }//���Ͳ���
        public DateTime? Date_start { get; set; }//��������
        public DateTime? Date_end { get; set; }//��������
    }

    public class V_SendSmsCount
	{
		public string Dept { get; set;}
		public Nullable<Guid> Sender { get; set;}
		public Nullable<DateTime> Date { get; set;}
		public Nullable<int> Count { get; set; }
        public string SenderName { get; set; }
        public string DeptFullName { get; set; }
        

    }
}
