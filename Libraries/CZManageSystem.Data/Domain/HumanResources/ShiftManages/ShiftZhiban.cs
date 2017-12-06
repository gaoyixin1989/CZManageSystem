using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CZManageSystem.Data.Domain.SysManger;

namespace CZManageSystem.Data.Domain.HumanResources.ShiftManages
{

    public class ZhibanQueryBuilder
    {
        public Guid? Editor { get; set; }//�༭��
        public string Year { get; set; }//���
        public string Month { get; set; }//�·�
        public string State { get; set; }//״̬
        public List<string> DeptId { get; set; }//����ID
    }

    /// <summary>
    /// �Ű���Ϣ��
    /// </summary>
    public class ShiftZhiban
    {

        public ShiftZhiban()
        {
            this.ShiftBancis = new List<ShiftBanci>();
            this.ShiftLunbans = new List<ShiftLunban>();
        }
        /// <summary>
        /// �Ű�ID
        /// <summary>
        public Guid Id { get; set; }
        /// <summary>
        /// ����
        /// <summary>
        public string Title { get; set; }
        /// <summary>
        /// �༭��
        /// <summary>
        public Nullable<Guid> Editor { get; set; }
        /// <summary>
        /// �༭ʱ��
        /// <summary>
        public Nullable<DateTime> EditTime { get; set; }
        /// <summary>
        /// ����ID
        /// <summary>
        public string DeptId { get; set; }
        /// <summary>
        /// ��
        /// <summary>
        public string Year { get; set; }
        /// <summary>
        /// ��
        /// <summary>
        public string Month { get; set; }
        /// <summary>
        /// ��ע
        /// <summary>
        public string Remark { get; set; }
        /// <summary>
        /// ״̬��0-δ�ύ��1-�ύ
        /// <summary>
        public string State { get; set; }


        public virtual Users EditorObj { get; set; }
        public virtual Depts DeptObj { get; set; }
        public virtual ICollection<ShiftBanci> ShiftBancis { get; set; }
        public virtual ICollection<ShiftLunban> ShiftLunbans { get; set; }
    }
}
