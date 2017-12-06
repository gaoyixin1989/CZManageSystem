using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CZManageSystem.Data.Domain.SysManger;

namespace CZManageSystem.Data.Domain.HumanResources.ShiftManages
{
    /// <summary>
    /// �����Ϣ
    /// </summary>
	public class ShiftBanci
    {
        public ShiftBanci()
        {
            this.ShiftRichs = new List<ShiftRich>();
        }

        /// <summary>
        /// ���ID
        /// <summary>
        public Guid Id { get; set; }
        /// <summary>
        /// �༭��
        /// <summary>
        public Nullable<Guid> Editor { get; set; }
        /// <summary>
        /// �༭ʱ��
        /// <summary>
        public Nullable<DateTime> EditTime { get; set; }
        /// <summary>
        /// �Ű���Ϣ��Id
        /// <summary>
        public Guid ZhibanId { get; set; }
        /// <summary>
        /// �������
        /// <summary>
        public string BcName { get; set; }
        /// <summary>
        /// ��ʼСʱֵ
        /// <summary>
        public string StartHour { get; set; }
        /// <summary>
        /// ��ʼ����ֵ
        /// <summary>
        public string StartMinute { get; set; }
        /// <summary>
        /// ����Сʱֵ
        /// <summary>
        public string EndHour { get; set; }
        /// <summary>
        /// ��������ֵ
        /// <summary>
        public string EndMinute { get; set; }
        /// <summary>
        /// ֵ������
        /// <summary>
        public Nullable<int> StaffNum { get; set; }
        /// <summary>
        /// �������
        /// <summary>
        public Nullable<int> OrderNo { get; set; }
        /// <summary>
        /// ��ע
        /// <summary>
        public string Remark { get; set; }


        public virtual Users EditorObj { get; set; }
        public virtual ICollection<ShiftRich> ShiftRichs { get; set; }

    }
}
