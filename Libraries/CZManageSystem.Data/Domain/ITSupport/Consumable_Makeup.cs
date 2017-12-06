using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CZManageSystem.Data.Domain.SysManger;

namespace CZManageSystem.Data.Domain.ITSupport
{
    public class Consumable_Makeup
    {
        /// <summary>
        /// ���
        /// <summary>
        public Guid ID { get; set; }
        /// <summary>
        /// ����ʵ��ID
        /// <summary>
        public Nullable<Guid> WorkflowInstanceId { get; set; }
        /// <summary>
        /// ���̵���
        /// <summary>
        public string Series { get; set; }
        /// <summary>
        /// �����ύʱ��
        /// <summary>
        public Nullable<DateTime> AppTime { get; set; }
        /// <summary>
        /// ���벿��id
        /// <summary>
        public string AppDept { get; set; }
        /// <summary>
        /// ������
        /// <summary>
        public Nullable<Guid> AppPerson { get; set; }
        /// <summary>
        /// �������ֻ�
        /// <summary>
        public string Mobile { get; set; }
        /// <summary>
        /// �������
        /// <summary>
        public string Title { get; set; }
        /// <summary>
        /// �˿�ԭ��
        /// <summary>
        public string Content { get; set; }
        /// <summary>
        /// ״̬��0���桢1�ύ
        /// <summary>
        public Nullable<int> State { get; set; }
        /// <summary>
        /// ʹ����ID
        /// <summary>
        public Nullable<Guid> UsePerson { get; set; }
        
                
        public virtual Users UsersForApp { get; set; }//���������
        public virtual Users UsersForUse { get; set; }//ʹ�������
        public virtual Depts DeptsForApp { get; set; }//�����˲������
        public virtual Tracking_Workflow Tracking_Workflow { get; set; }

    }
}
