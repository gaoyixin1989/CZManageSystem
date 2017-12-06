using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CZManageSystem.Data.Domain.SysManger;

/// <summary>
/// ���ŷ��ͱ�
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.SmsManager
{
    //��ͬ��Ϣ��ѯ����
    public class SendSmsQueryBuilder
    {
        public string Context { get; set; }//��������
        public string UserName { get; set; }//������
        public string DeptName { get; set; }//���Ͳ���
        public DateTime? Date_start { get; set; }//��������
        public DateTime? Date_end { get; set; }//��������
        public bool? Error { get; set; }//����״��
    }

    public class SendSms
	{
		/// <summary>
		/// id
		/// <summary>
		public Guid ID { get; set;}
		/// <summary>
		/// �ֻ�����
		/// <summary>
		public string Mobile { get; set;}
		/// <summary>
		/// ��������
		/// <summary>
		public string Context { get; set;}
		/// <summary>
		/// ����ʱ��
		/// <summary>
		public Nullable<DateTime> Time { get; set;}
		/// <summary>
		/// ������
		/// <summary>
		public Nullable<Guid> Sender { get; set;}
		/// <summary>
		/// �Ƿ����
		/// <summary>
		public Nullable<bool> Error { get; set;}
		/// <summary>
		/// ����
		/// <summary>
		public Nullable<int> Count { get; set;}
		/// <summary>
		/// ��������
		/// <summary>
		public Nullable<DateTime> Date { get; set;}
		/// <summary>
		/// �Ƿ���ʾ����
		/// <summary>
		public string ShowName { get; set; }
        /// <summary>
        /// ���Ͳ���id
        /// <summary>
        public string Dept { get; set; }

        //���
        public virtual Users SenderObj { get; set; }
        public virtual Depts DeptObj { get; set; }

    }
}
