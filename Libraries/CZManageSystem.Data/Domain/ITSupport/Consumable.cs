using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.ITSupport
{
    public class ConsumableQueryBuilder
    {
        //public int ID { get; set; }
        public string Type { get; set; }

        public string Name { get; set; }
        public string Model { get; set; }

        public string IsValue { get; set; }

        public string IsDelete { get; set; }

        public DateTime? ApplyTime_start { get; set; }
        public DateTime? ApplyTime_end { get; set; }

        public bool? hasStock { get; set; }
    }

    public class Consumable
	{
		public int ID { get; set;}
		/// <summary>
		/// �Ĳ����
		/// <summary>
		public string Type { get; set;}
		/// <summary>
		/// �Ĳ��ͺ�
		/// <summary>
		public string Model { get; set;}
		/// <summary>
		/// �Ĳ�����
		/// <summary>
		public string Name { get; set;}
		/// <summary>
		/// �����豸
		/// <summary>
		public string Equipment { get; set;}
		/// <summary>
		/// ��λ
		/// <summary>
		public string Unit { get; set;}
		/// <summary>
		/// �Ĳ�Ʒ��
		/// <summary>
		public string Trademark { get; set;}
        /// <summary>
        /// ��ֵ���ͣ�0�ͼ�ֵ��1�ǵͼ�ֵ
        /// <summary>
        public string IsValue { get; set;}
        /// <summary>
        /// �Ĳĵ�ǰӵ����
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// �Ƿ�ɾ��  0�����  1������
        /// </summary>
        public string IsDelete { get; set; }


    }
}
