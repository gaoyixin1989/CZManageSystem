using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.Administrative
{
    public class BoardroomInfoQueryBuilder
    {
        public int[] BoardroomID { get; set; }//������
        public string[] State { get; set; }//״̬
        public int? MaxMan_min { get; set; }//�������_����
        public int? MaxMan_max { get; set; }//�������_����

        public string Address { get; set; }//�ص�
        public string Code { get; set; }//����
        public string Name { get; set; }//����
    }

    public class BoardroomInfo
	{
		/// <summary>
		/// ������ID
		/// <summary>
		public int BoardroomID { get; set;}
		/// <summary>
		/// �༭��
		/// <summary>
		public string Editor { get; set;}
		/// <summary>
		/// �༭ʱ��
		/// <summary>
		public Nullable<DateTime> EditTime { get; set;}
		/// <summary>
		/// ������λ
		/// <summary>
		public Nullable<int> CorpID { get; set;}
		/// <summary>
		/// ���
		/// <summary>
		public string Code { get; set;}
		/// <summary>
		/// ����
		/// <summary>
		public string Name { get; set;}
		/// <summary>
		/// �ص�
		/// <summary>
		public string Address { get; set;}
		/// <summary>
		/// �������
		/// <summary>
		public Nullable<int> MaxMan { get; set;}
		/// <summary>
		/// �豸
		/// <summary>
		public string Equip { get; set;}
		/// <summary>
		/// �����豸
		/// <summary>
		public string EquipOther { get; set;}
		/// <summary>
		/// ��;
		/// <summary>
		public string Purpose { get; set;}
		/// <summary>
		/// Ԥ��ģʽ
		/// <summary>
		public string BookMode { get; set;}
		/// <summary>
		/// ����λ
		/// <summary>
		public string ManagerUnit { get; set;}
		/// <summary>
		/// ������
		/// <summary>
		public string ManagerPerson { get; set;}
		/// <summary>
		/// ״̬
		/// <summary>
		public string State { get; set;}
		/// <summary>
		/// ��ע
		/// <summary>
		public string Remark { get; set;}
		/// <summary>
		/// ͣ�ÿ�ʼʱ��
		/// <summary>
		public Nullable<DateTime> StartTime { get; set;}
		/// <summary>
		/// ͣ�ý���ʱ��
		/// <summary>
		public Nullable<DateTime> EndTime { get; set;}
		/// <summary>
		/// �Զ����ֶ�
		/// <summary>
		public string Field00 { get; set;}
		public string Field01 { get; set;}
		public string Field02 { get; set;}

	}
}
