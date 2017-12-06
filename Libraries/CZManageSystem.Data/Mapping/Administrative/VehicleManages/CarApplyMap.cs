using CZManageSystem.Data.Domain.Administrative.VehicleManages;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.Administrative.VehicleManages
{
	public class CarApplyMap : EntityTypeConfiguration<CarApply>
	{
		public CarApplyMap()
		{
			// Primary Key
			 this.HasKey(t => t.ApplyId);
		/// <summary>
		/// ���ڲ���
		/// <summary>
			  this.Property(t => t.DeptName)

			.HasMaxLength(100);
            this.Property(t => t.ApplySn)
               .HasMaxLength(50);
            /// <summary>
            /// ����
            /// <summary>
            this.Property(t => t.ApplyTitle) 
			.HasMaxLength(150);
      
		/// <summary>
		/// ������
		/// <summary>
			  this.Property(t => t.ApplyCant)

			.HasMaxLength(100); 
		/// <summary>
		/// ��ϵ�绰
		/// <summary>
			  this.Property(t => t.Mobile)

			.HasMaxLength(100);
		/// <summary>
		/// �����ص�
		/// <summary>
			  this.Property(t => t.Starting)

			.HasMaxLength(100);
		/// <summary>
		/// Ŀ�ĵ�1
		/// <summary>
			  this.Property(t => t.Destination1)

			.HasMaxLength(100);
		/// <summary>
		/// Ŀ�ĵ�2
		/// <summary>
			  this.Property(t => t.Destination2)

			.HasMaxLength(100);
		/// <summary>
		/// Ŀ�ĵ�3
		/// <summary>
			  this.Property(t => t.Destination3)

			.HasMaxLength(100);
		/// <summary>
		/// Ŀ�ĵ�4
		/// <summary>
			  this.Property(t => t.Destination4)

			.HasMaxLength(100);
		/// <summary>
		/// Ŀ�ĵ�5
		/// <summary>
			  this.Property(t => t.Destination5)

			.HasMaxLength(100);
		/// <summary>
		/// ������
		/// <summary>
			  this.Property(t => t.PersonCount)

			.HasMaxLength(100);
		/// <summary>
		/// ·;���
		/// <summary>
			  this.Property(t => t.Road)

			.HasMaxLength(100);
		/// <summary>
		/// ������;
		/// <summary>
			  this.Property(t => t.UseType)

			.HasMaxLength(100);
		/// <summary>
		/// ����
		/// <summary>
			  this.Property(t => t.Request)

			.HasMaxLength(500);
		/// <summary>
		/// ����
		/// <summary>
			  this.Property(t => t.Attach)

			.HasMaxLength(2000);
		/// <summary>
		/// ����00
		/// <summary>
			  this.Property(t => t.Field00)

			.HasMaxLength(200);
		/// <summary>
		/// ����01
		/// <summary>
			  this.Property(t => t.Field01)

			.HasMaxLength(200);
		/// <summary>
		/// ����02
		/// <summary>
			  this.Property(t => t.Field02)

			.HasMaxLength(200);
		/// <summary>
		/// �����ˡ�������
		/// <summary>
			  this.Property(t => t.Allocator)

			.HasMaxLength(100);
		/// <summary>
		/// ����������Ϣ
		/// <summary>
			  this.Property(t => t.AllotIntro)

			.HasMaxLength(2000);
		/// <summary>
		/// ��ע
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(500);
			  this.Property(t => t.UptUser)

			.HasMaxLength(100);
		/// <summary>
		/// ������
		/// <summary>
			  this.Property(t => t.BalUser)

			.HasMaxLength(100);
		/// <summary>
		/// ��ע��Ϣ
		/// <summary>
			  this.Property(t => t.BalRemark)

			.HasMaxLength(500);
		/// <summary>
		/// �����޸���
		/// <summary>
			  this.Property(t => t.OpinUser)

			.HasMaxLength(100);
		/// <summary>
		/// �����г���ȫ
		/// <summary>
			  this.Property(t => t.OpinGrade1)

			.HasMaxLength(100);
		/// <summary>
		/// ���۷�������
		/// <summary>
			  this.Property(t => t.OpinGrade2)

			.HasMaxLength(100);
		/// <summary>
		/// ���۳�������
		/// <summary>
			  this.Property(t => t.OpinGrade3)

			.HasMaxLength(100);
		/// <summary>
		/// ���۸����Ǳ�
		/// <summary>
			  this.Property(t => t.OpinGrade4)

			.HasMaxLength(100);
		/// <summary>
		/// ����ʱ�����
		/// <summary>
			  this.Property(t => t.OpinGrade5)

			.HasMaxLength(100);
		/// <summary>
		/// ���۷����
		/// <summary>
			  this.Property(t => t.OpinGrade6)

			.HasMaxLength(100);
		/// <summary>
		/// ����
		/// <summary>
			  this.Property(t => t.OpinGrade7)

			.HasMaxLength(100);
		/// <summary>
		/// ���۱�ע
		/// <summary>
			  this.Property(t => t.OpinRemark)

			.HasMaxLength(500);
		/// <summary>
		/// ����ԭ��˵��
		/// <summary>
			  this.Property(t => t.SpecialReason)

			.HasMaxLength(500);
		/// <summary>
		/// ��ͷ�����쵼
		/// <summary>
			  this.Property(t => t.Leader)

			.HasMaxLength(150);
			// Table & Column Mappings
 			 this.ToTable("CarApply"); 
			// ����
			this.Property(t => t.ApplyId).HasColumnName("ApplyId"); 
			// ����ʵ��Id
			this.Property(t => t.WorkflowInstanceId).HasColumnName("WorkflowInstanceId");
            // ����
            this.Property(t => t.ApplyTitle).HasColumnName("ApplyTitle");
            this.Property(t => t.ApplySn).HasColumnName("ApplySn");
            // ������λ
            this.Property(t => t.CorpId).HasColumnName("CorpId"); 
			// ����ʱ��
			this.Property(t => t.CreateTime).HasColumnName("CreateTime"); 
			// ���ڲ���
			this.Property(t => t.DeptName).HasColumnName("DeptName"); 
			// ������
			this.Property(t => t.ApplyCant).HasColumnName("ApplyCant");
            // ������ID
            this.Property(t => t.ApplyCantId).HasColumnName("ApplyCantId");
            // ��ʻ�ˡ�ʹ����
            this.Property(t => t.Driver).HasColumnName("Driver");
            // ��ʻ�ˡ�ʹ����Ids
            this.Property(t => t.DriverIds).HasColumnName("DriverIds");
            // ��ϵ�绰
            this.Property(t => t.Mobile).HasColumnName("Mobile"); 
			// Ԥ�ƽ���ʱ��
			this.Property(t => t.TimeOut).HasColumnName("TimeOut"); 
			// ������ʼʱ��
			this.Property(t => t.StartTime).HasColumnName("StartTime"); 
			// ����ʱ��
			this.Property(t => t.EndTime).HasColumnName("EndTime"); 
			// �����ص�
			this.Property(t => t.Starting).HasColumnName("Starting"); 
			// Ŀ�ĵ�1
			this.Property(t => t.Destination1).HasColumnName("Destination1"); 
			// Ŀ�ĵ�2
			this.Property(t => t.Destination2).HasColumnName("Destination2"); 
			// Ŀ�ĵ�3
			this.Property(t => t.Destination3).HasColumnName("Destination3"); 
			// Ŀ�ĵ�4
			this.Property(t => t.Destination4).HasColumnName("Destination4"); 
			// Ŀ�ĵ�5
			this.Property(t => t.Destination5).HasColumnName("Destination5"); 
			// ������
			this.Property(t => t.PersonCount).HasColumnName("PersonCount"); 
			// ·;���
			this.Property(t => t.Road).HasColumnName("Road"); 
			// ������;
			this.Property(t => t.UseType).HasColumnName("UseType"); 
			// ����
			this.Property(t => t.Request).HasColumnName("Request"); 
			// ����
			this.Property(t => t.Attach).HasColumnName("Attach"); 
			// ����00
			this.Property(t => t.Field00).HasColumnName("Field00"); 
			// ����01
			this.Property(t => t.Field01).HasColumnName("Field01"); 
			// ����02
			this.Property(t => t.Field02).HasColumnName("Field02"); 
			// �����ˡ�������
			this.Property(t => t.Allocator).HasColumnName("Allocator"); 
			// ������������ʱ��
			this.Property(t => t.AllotTime).HasColumnName("AllotTime"); 
			this.Property(t => t.CarIds).HasColumnName("CarIds"); 
			// ����������Ϣ
			this.Property(t => t.AllotIntro).HasColumnName("AllotIntro"); 
			// ��ע
			this.Property(t => t.Remark).HasColumnName("Remark"); 
			// �ó�����ʱ��
			this.Property(t => t.FinishTime).HasColumnName("FinishTime"); 
			this.Property(t => t.UptUser).HasColumnName("UptUser"); 
			this.Property(t => t.UptTime).HasColumnName("UptTime"); 
			// ������
			this.Property(t => t.BalUser).HasColumnName("BalUser"); 
			// ����ʱ��
			this.Property(t => t.BalTime).HasColumnName("BalTime"); 
			// ��ʼ������
			this.Property(t => t.KmNum1).HasColumnName("KmNum1"); 
			// ��ֹ������
			this.Property(t => t.KmNum2).HasColumnName("KmNum2"); 
			// ����ʹ�����
			this.Property(t => t.KmCount).HasColumnName("KmCount"); 
			// ·�ŷѹ�����
			this.Property(t => t.BalCount).HasColumnName("BalCount"); 
			// �ϼƽ��
			this.Property(t => t.BalTotal).HasColumnName("BalTotal"); 
			// ��ע��Ϣ
			this.Property(t => t.BalRemark).HasColumnName("BalRemark"); 
			// �����޸���
			this.Property(t => t.OpinUser).HasColumnName("OpinUser"); 
			// �����޸�ʱ��
			this.Property(t => t.OpinTime).HasColumnName("OpinTime"); 
			// �����г���ȫ
			this.Property(t => t.OpinGrade1).HasColumnName("OpinGrade1"); 
			// ���۷�������
			this.Property(t => t.OpinGrade2).HasColumnName("OpinGrade2"); 
			// ���۳�������
			this.Property(t => t.OpinGrade3).HasColumnName("OpinGrade3"); 
			// ���۸����Ǳ�
			this.Property(t => t.OpinGrade4).HasColumnName("OpinGrade4"); 
			// ����ʱ�����
			this.Property(t => t.OpinGrade5).HasColumnName("OpinGrade5"); 
			// ���۷����
			this.Property(t => t.OpinGrade6).HasColumnName("OpinGrade6"); 
			// ����
			this.Property(t => t.OpinGrade7).HasColumnName("OpinGrade7"); 
			// ���۱�ע
			this.Property(t => t.OpinRemark).HasColumnName("OpinRemark"); 
			// ����ԭ��˵��
			this.Property(t => t.SpecialReason).HasColumnName("SpecialReason"); 
			// �Ƿ��ѿ�ͷ����
			this.Property(t => t.Boral).HasColumnName("Boral"); 
			// ��ͷ�����쵼
			this.Property(t => t.Leader).HasColumnName("Leader"); 
            // �ó��������� 
            this.Property(t => t.ApplyType).HasColumnName("ApplyType");
            //��λ����
            this.Property(t => t.CarTonnage).HasColumnName("CarTonnage");

            this.HasOptional(t => t.TrackingWorkflow)
          .WithMany()
          .HasForeignKey(d => d.WorkflowInstanceId);
        }
	 }
}
