
using System.Data.Entity.ModelConfiguration;
using CZManageSystem.Data.Domain.Administrative;

namespace CZManageSystem.Data.Mapping.Administrative
{
	public class BoardroomApplyMap : EntityTypeConfiguration<BoardroomApply>
	{
		public BoardroomApplyMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// ����
		/// <summary>
			  this.Property(t => t.ApplyTitle)

			.HasMaxLength(200);
            /// <summary>
            /// ����ID
            /// <summary>
            this.Property(t => t.WorkflowInstanceId)
          .HasMaxLength(50);
            /// <summary>
            /// ���̵���
            /// <summary>
            this.Property(t => t.Code)
			.HasMaxLength(50);
		/// <summary>
		/// ״̬��ע������״̬ʱ������д����ԭ��
		/// <summary>
			  this.Property(t => t.StateRemark)

			.HasMaxLength(200);
		/// <summary>
		/// ���벿��
		/// <summary>
			  this.Property(t => t.AppDept)

			.HasMaxLength(200);
		/// <summary>
		/// ������
		/// <summary>
			  this.Property(t => t.AppPerson)

			.HasMaxLength(200);
		/// <summary>
		/// ��ϵ�绰
		/// <summary>
			  this.Property(t => t.ContactMobile)

			.HasMaxLength(200);
		/// <summary>
		/// �������
		/// <summary>
			  this.Property(t => t.JoinNum)

			.HasMaxLength(200);
            /// <summary>
            /// �����Ա
            /// <summary>
            this.Property(t => t.JoinPeople);
		/// <summary>
		/// ��ʼʱ��
		/// <summary>
			  this.Property(t => t.StartTime)

			.HasMaxLength(50);
		/// <summary>
		/// ����ʱ��
		/// <summary>
			  this.Property(t => t.EndTime)

			.HasMaxLength(50);
		/// <summary>
		/// ����ʱ���
		/// <summary>
			  this.Property(t => t.UseFor)

			.HasMaxLength(200);
		/// <summary>
		/// �쵼�μ����
		/// <summary>
			  this.Property(t => t.Fugle)
			.HasMaxLength(200);
		/// <summary>
		/// �����豸
		/// <summary>
			  this.Property(t => t.NeedEquip)

			.HasMaxLength(200);
		/// <summary>
		/// ��Ҫ�������豸
		/// <summary>
			  this.Property(t => t.OtherEquip)

			.HasMaxLength(200);
		/// <summary>
		/// �������
		/// <summary>
			  this.Property(t => t.BannerContent)

			.HasMaxLength(200);
		/// <summary>
		/// �������
		/// <summary>
			  this.Property(t => t.BannerLength)

			.HasMaxLength(50);
		/// <summary>
		/// ������
		/// <summary>
			  this.Property(t => t.BannerWidth)

			.HasMaxLength(50);
		/// <summary>
		/// ���ģʽ
		/// <summary>
			  this.Property(t => t.BannerMode)

			.HasMaxLength(50);
		/// <summary>
		/// ��ӭ��
		/// <summary>
			  this.Property(t => t.WelcomeContent)

			.HasMaxLength(200);
		/// <summary>
		/// ���Ŷ�
		/// <summary>
			  this.Property(t => t.WelcoomeSect)

			.HasMaxLength(200);
		/// <summary>
		/// ��ע
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(200);
		/// <summary>
		/// �Զ����ֶ�
		/// <summary>
			  this.Property(t => t.Field00)

			.HasMaxLength(200);
			  this.Property(t => t.Field01)

			.HasMaxLength(200);
			  this.Property(t => t.Field02)

			.HasMaxLength(200);
		/// <summary>
		/// �༭��
		/// <summary>
			  this.Property(t => t.Editor)

			.HasMaxLength(200);
		/// <summary>
		/// ���ۣ������������á��Ϻá�һ�㡢��
		/// <summary>
			  this.Property(t => t.JudgeServiceQuality)

			.HasMaxLength(200);
		/// <summary>
		/// ���ۣ������������á��Ϻá�һ�㡢��
		/// <summary>
			  this.Property(t => t.JudgeEnvironmental)

			.HasMaxLength(200);
		/// <summary>
		/// ��������
		/// <summary>
			  this.Property(t => t.JudgeOtherSuggest)

			.HasMaxLength(200);
			// Table & Column Mappings
 			 this.ToTable("BoardroomApply"); 
			// ���
			this.Property(t => t.ID).HasColumnName("ID"); 
			// ����ID
			this.Property(t => t.WorkflowInstanceId).HasColumnName("WorkflowInstanceId"); 
			// ����
			this.Property(t => t.ApplyTitle).HasColumnName("ApplyTitle"); 
			// ������λ
			this.Property(t => t.CorpID).HasColumnName("CorpID"); 
			// �����������
			this.Property(t => t.EndDate).HasColumnName("EndDate");
            // �������ʵ����ʱ��(Ĭ��Ϊ����ʱ�Ľ���ʱ��)
            this.Property(t => t.EndDate_Real).HasColumnName("EndDate_Real");
            // ���̵���
            this.Property(t => t.Code).HasColumnName("Code"); 
			// ״̬��0���ύ��1����ˡ�2�����ۡ�3��ɡ�-1����
			this.Property(t => t.State).HasColumnName("State"); 
			// ״̬��ע������״̬ʱ������д����ԭ��
			this.Property(t => t.StateRemark).HasColumnName("StateRemark"); 
			// ���벿��
			this.Property(t => t.AppDept).HasColumnName("AppDept"); 
			// ������
			this.Property(t => t.AppPerson).HasColumnName("AppPerson");
            // ����ʱ��
            this.Property(t => t.AppTime).HasColumnName("AppTime");
            // ��ϵ�绰
            this.Property(t => t.ContactMobile).HasColumnName("ContactMobile"); 
			// ������ID
			this.Property(t => t.Room).HasColumnName("Room"); 
			// �������
			this.Property(t => t.JoinNum).HasColumnName("JoinNum"); 
			// �����Ա
			this.Property(t => t.JoinPeople).HasColumnName("JoinPeople"); 
			// ���鿪ʼ����
			this.Property(t => t.MeetingDate).HasColumnName("MeetingDate"); 
			// ��ʼʱ��
			this.Property(t => t.StartTime).HasColumnName("StartTime"); 
			// ����ʱ��
			this.Property(t => t.EndTime).HasColumnName("EndTime"); 
			// ��������ʱ��
			this.Property(t => t.AwokeTime).HasColumnName("AwokeTime"); 
			// ����ʱ���
			this.Property(t => t.UseFor).HasColumnName("UseFor"); 
			// �쵼�μ����
			this.Property(t => t.Fugle).HasColumnName("Fugle");
            // �Ƿ���ʡ��˾�쵼�μ�
            this.Property(t => t.Fugle_pro).HasColumnName("Fugle_pro");
			// �����豸
			this.Property(t => t.NeedEquip).HasColumnName("NeedEquip"); 
			// ��Ҫ�������豸
			this.Property(t => t.OtherEquip).HasColumnName("OtherEquip"); 
			// �������
			this.Property(t => t.BannerContent).HasColumnName("BannerContent"); 
			// �������
			this.Property(t => t.BannerLength).HasColumnName("BannerLength"); 
			// ������
			this.Property(t => t.BannerWidth).HasColumnName("BannerWidth"); 
			// ���ģʽ
			this.Property(t => t.BannerMode).HasColumnName("BannerMode"); 
			// ��ӭ��
			this.Property(t => t.WelcomeContent).HasColumnName("WelcomeContent"); 
			// ��ӭ�ʲ���ʱ��
			this.Property(t => t.WelcoomeTime).HasColumnName("WelcoomeTime"); 
			// ���Ŷ�
			this.Property(t => t.WelcoomeSect).HasColumnName("WelcoomeSect"); 
			// ����ʱ��
			this.Property(t => t.BookTime).HasColumnName("BookTime"); 
			// ��ע
			this.Property(t => t.Remark).HasColumnName("Remark"); 
			// �Զ����ֶ�
			this.Property(t => t.Field00).HasColumnName("Field00"); 
			this.Property(t => t.Field01).HasColumnName("Field01"); 
			this.Property(t => t.Field02).HasColumnName("Field02"); 
			// �༭��
			this.Property(t => t.Editor).HasColumnName("Editor"); 
			// ���ۣ������������á��Ϻá�һ�㡢��
			this.Property(t => t.JudgeServiceQuality).HasColumnName("JudgeServiceQuality"); 
			// ���ۣ������������á��Ϻá�һ�㡢��
			this.Property(t => t.JudgeEnvironmental).HasColumnName("JudgeEnvironmental"); 
			// ��������
			this.Property(t => t.JudgeOtherSuggest).HasColumnName("JudgeOtherSuggest"); 
			// ����״̬��0���δ���ۣ�1�Ѿ����ۣ�2�Զ�����
			this.Property(t => t.JudgeState).HasColumnName("JudgeState");
            // �Ƿ���ӻ���
            this.Property(t => t.ISTVMeeting).HasColumnName("ISTVMeeting");
            // ����ʱ��
            this.Property(t => t.JudgeTime).HasColumnName("JudgeTime");
        }
	 }
}
