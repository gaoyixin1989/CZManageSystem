using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CZManageSystem.Data.Domain.SysManger;

namespace CZManageSystem.Data.Domain.HumanResources.Vacation
{

    //���������ѯ
    public class VacationCloseApplyQueryBuilder
    {
        public string ApplyTitle { get; set; }//����
        public int? State { get; set; }//״̬
        public Guid? Editor { get; set; }//�ύ��
        public DateTime? EditTime_start { get; set; }//�ύʱ��
        public DateTime? EditTime_end { get; set; }
        public List<int> WF_State { get; set; }
    }

    public class HRVacationCloseApply
    {
        /// <summary>
        /// ����
        /// <summary>
        public Guid ApplyId { get; set; }
        /// <summary>
        /// ����ʵ��Id
        /// <summary>
        public Nullable<Guid> WorkflowInstanceId { get; set; }
        /// <summary>
        /// ���̵���
        /// <summary>
        public string ApplySn { get; set; }
        /// <summary>
        /// ����
        /// <summary>
        public string ApplyTitle { get; set; }
        /// <summary>
        /// �༭��
        /// <summary>
        public Nullable<Guid> Editor { get; set; }
        public Nullable<DateTime> EditTime { get; set; }
        /// <summary>
        /// �ݼ�����
        /// <summary>
        public string VacationType { get; set; }
        /// <summary>
        /// ��������
        /// <summary>
        public string VacationClass { get; set; }
        /// <summary>
        /// ��������
        /// <summary>
        public Nullable<decimal> ClosedDays { get; set; }
        /// <summary>
        /// �ݼ�ԭ��
        /// <summary>
        public string Reason { get; set; }
        /// <summary>
        /// �ݼ����뵥id
        /// <summary>
        public Nullable<Guid> VacationID { get; set; }
        /// <summary>
        /// ����ԭ��
        /// <summary>
        public string Note { get; set; }
        /// <summary>
        /// ʵ�ʿ�ʼʱ��
        /// <summary>
        public Nullable<DateTime> Factst { get; set; }
        /// <summary>
        /// ʵ�ʽ���ʱ��
        /// <summary>
        public Nullable<DateTime> Factet { get; set; }
        /// <summary>
        /// ʵ������
        /// <summary>
        public Nullable<decimal> Factdays { get; set; }
        /// <summary>
        /// ״̬��0δ�ύ��1���ύ
        /// <summary>
        public Nullable<int> State { get; set; }

        //���
        public virtual Tracking_Workflow Tracking_Workflow { get; set; }
        public virtual Users EditorObj { get; set; }

    }
}
