using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.Administrative
{
    public class BoardroomApplyQueryBuilder
    {
        public int[] Room { get; set; }//������
        public int[] State { get; set; }//״̬
        public int[] State_without { get; set; }//��������״̬
        public string[] AppPerson { get; set; }//������
        public int[] JudgeState { get; set; }//����״̬

        public DateTime? AppTime_start { get; set; }//����ʱ��
        public DateTime? AppTime_end { get; set; }
        public DateTime? MeetingDate_start { get; set; }//��ѯ��������
        public DateTime? MeetingDate_end { get; set; }
        public DateTime? MeetingTime_start { get; set; }//��ѯ����ʱ��
        public DateTime? MeetingTime_end { get; set; }

        public DateTime? EndDate_Real_start { get; set; }//�������ʱ�䷶Χ
        public DateTime? EndDate_Real_end { get; set; }

    }

    public class BoardroomApply
    {
        /// <summary>
        /// ���
        /// <summary>
        public int ID { get; set; }
        /// <summary>
        /// ����ID
        /// <summary>
        public string WorkflowInstanceId { get; set; }
        /// <summary>
        /// ����
        /// <summary>
        public string ApplyTitle { get; set; }
        /// <summary>
        /// ������λ
        /// <summary>
        public Nullable<int> CorpID { get; set; }
        /// <summary>
        /// �����������
        /// <summary>
        public Nullable<DateTime> EndDate { get; set; }
        /// <summary>
        /// �������ʵ����ʱ��(Ĭ��Ϊ����ʱ�Ľ���ʱ��)
        /// <summary>
        public Nullable<DateTime> EndDate_Real { get; set; }
        /// <summary>
        /// ���̵���
        /// <summary>
        public string Code { get; set; }
        /// <summary>
        /// ״̬��0���ύ��1�Ѿ��ύ��-1����
        /// <summary>
        public Nullable<int> State { get; set; }
        /// <summary>
        /// ״̬��ע������״̬ʱ������д����ԭ��
        /// <summary>
        public string StateRemark { get; set; }
        /// <summary>
        /// ���벿��
        /// <summary>
        public string AppDept { get; set; }
        /// <summary>
        /// ������
        /// <summary>
        public string AppPerson { get; set; }
        /// <summary>
        /// ����ʱ��
        /// <summary>
        public Nullable<DateTime> AppTime { get; set; }
        /// <summary>
        /// ��ϵ�绰
        /// <summary>
        public string ContactMobile { get; set; }
        /// <summary>
        /// ������ID
        /// <summary>
        public Nullable<int> Room { get; set; }
        /// <summary>
        /// �������
        /// <summary>
        public string JoinNum { get; set; }
        /// <summary>
        /// �����Ա
        /// <summary>
        public string JoinPeople { get; set; }
        /// <summary>
        /// ���鿪ʼ����
        /// <summary>
        public Nullable<DateTime> MeetingDate { get; set; }
        /// <summary>
        /// ��ʼʱ��
        /// <summary>
        public string StartTime { get; set; }
        /// <summary>
        /// ����ʱ��
        /// <summary>
        public string EndTime { get; set; }
        /// <summary>
        /// ��������ʱ��
        /// <summary>
        public Nullable<DateTime> AwokeTime { get; set; }
        /// <summary>
        /// ����ʱ���
        /// <summary>
        public string UseFor { get; set; }
        /// <summary>
        /// �쵼�μ����
        /// <summary>
        public string Fugle { get; set; }
        /// <summary>
        /// �Ƿ���ʡ��˾�쵼�μ�
        /// <summary>
        public Nullable<bool> Fugle_pro { get; set; }
        /// <summary>
        /// �����豸
        /// <summary>
        public string NeedEquip { get; set; }
        /// <summary>
        /// ��Ҫ�������豸
        /// <summary>
        public string OtherEquip { get; set; }
        /// <summary>
        /// �������
        /// <summary>
        public string BannerContent { get; set; }
        /// <summary>
        /// �������
        /// <summary>
        public string BannerLength { get; set; }
        /// <summary>
        /// ������
        /// <summary>
        public string BannerWidth { get; set; }
        /// <summary>
        /// ���ģʽ
        /// <summary>
        public string BannerMode { get; set; }
        /// <summary>
        /// ��ӭ��
        /// <summary>
        public string WelcomeContent { get; set; }
        /// <summary>
        /// ��ӭ�ʲ���ʱ��
        /// <summary>
        public Nullable<DateTime> WelcoomeTime { get; set; }
        /// <summary>
        /// ���Ŷ�
        /// <summary>
        public string WelcoomeSect { get; set; }
        /// <summary>
        /// ����ʱ��
        /// <summary>
        public Nullable<DateTime> BookTime { get; set; }
        /// <summary>
        /// ��ע
        /// <summary>
        public string Remark { get; set; }
        /// <summary>
        /// �Զ����ֶ�
        /// <summary>
        public string Field00 { get; set; }
        public string Field01 { get; set; }
        public string Field02 { get; set; }
        /// <summary>
        /// �༭��
        /// <summary>
        public string Editor { get; set; }
        /// <summary>
        /// ���ۣ������������á��Ϻá�һ�㡢��
        /// <summary>
        public string JudgeServiceQuality { get; set; }
        /// <summary>
        /// ���ۣ������������á��Ϻá�һ�㡢��
        /// <summary>
        public string JudgeEnvironmental { get; set; }
        /// <summary>
        /// ��������
        /// <summary>
        public string JudgeOtherSuggest { get; set; }
        /// <summary>
        /// ����״̬��0���δ���ۣ�1�Ѿ����ۣ�2�Զ�����
        /// <summary>
        public Nullable<int> JudgeState { get; set; }
        /// <summary>
        /// �Ƿ���ӻ���
        /// <summary>
        public Nullable<bool> ISTVMeeting { get; set; }
        /// <summary>
        /// ����ʱ��
        /// <summary>
        public Nullable<DateTime> JudgeTime { get; set; }
    }
}
