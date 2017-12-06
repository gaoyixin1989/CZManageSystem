using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.SysManger
{
    public class PendingData
    {
        /// <summary>
        /// ���
        /// <summary>
        public int ID { get; set; }
        /// <summary>
        /// ������Դ�����졢���ġ��Ѱ졢����...
        /// <summary>
        public string DataSource { get; set; }
        /// <summary>
        /// ���ݱ�ʶID
        /// <summary>
        public string DataID { get; set; }
        /// <summary>
        /// ����ID
        /// <summary>
        public int SendID { get; set; }
        /// <summary>
        /// ����������
        /// <summary>
        public string Owner { get; set; }
        /// <summary>
        /// ״̬��1-δ���ͣ�2���ͣ�Ĭ��1��
        /// <summary>
        public Nullable<int> State { get; set; }
        /// <summary>
        /// ���Դ���
        /// <summary>
        public int TryTime { get; set; }
        /// <summary>
        /// ����ʱ��
        /// <summary>
        public Nullable<DateTime> DealTime { get; set; }
        /// <summary>
        /// ��ע
        /// <summary>
        public string Remark { get; set; }

    }
}
