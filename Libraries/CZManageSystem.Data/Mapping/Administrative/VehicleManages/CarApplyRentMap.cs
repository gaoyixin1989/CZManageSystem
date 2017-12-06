using CZManageSystem.Data.Domain.Administrative.VehicleManages;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.Administrative.VehicleManages
{
	public class CarApplyRentMap : EntityTypeConfiguration<CarApplyRent>
	{
		public CarApplyRentMap()
		{
			// Primary Key
			 this.HasKey(t => t.ApplyRentId);
		/// <summary>
		/// ���ڲ���
		/// <summary>
			  this.Property(t => t.DeptName)

			.HasMaxLength(100);
            this.Property(t => t.ApplySn)
               .HasMaxLength(50);
            /// <summary>
            /// ������
            /// <summary>
            this.Property(t => t.ApplyCant)

			.HasMaxLength(100);
            this.Property(t => t.ApplySn)
               .HasMaxLength(50);
            /// <summary>
            /// ����
            /// <summary>
            this.Property(t => t.ApplyTitle)
            .HasMaxLength(150);

            /// <summary>
            /// ��ʻ�ˡ�ʹ����
            /// <summary>
            this.Property(t => t.Driver)

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
		/// ��ע
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(500);
		/// <summary>
		/// �⳵����
		/// <summary>
			  this.Property(t => t.Htype)

			.HasMaxLength(50);
		/// <summary>
		/// ��λ/����
		/// <summary>
			  this.Property(t => t.CarTonnage)

			.HasMaxLength(100);
		/// <summary>
		/// �⳵ѯ��
		/// <summary>
			  this.Property(t => t.Enquiry)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("CarApplyRent"); 
			this.Property(t => t.ApplyRentId).HasColumnName("ApplyRentId"); 
			// ����ʵ��Id
			this.Property(t => t.WorkflowInstanceId).HasColumnName("WorkflowInstanceId");
            this.Property(t => t.ApplySn).HasColumnName("ApplySn");
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
			// ��ʻ�ˡ�ʹ����
			this.Property(t => t.Driver).HasColumnName("Driver"); 
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
			// ��ע
			this.Property(t => t.Remark).HasColumnName("Remark"); 
			// �⳵����
			this.Property(t => t.Htype).HasColumnName("Htype"); 
			// ��λ/����
			this.Property(t => t.CarTonnage).HasColumnName("CarTonnage"); 
			// �⳵ѯ��
			this.Property(t => t.Enquiry).HasColumnName("Enquiry"); 
			// �Ƿ��ڷ���Ԥ������
			this.Property(t => t.AllotRight).HasColumnName("AllotRight");
            this.HasOptional(t => t.TrackingWorkflow)
        .WithMany()
        .HasForeignKey(d => d.WorkflowInstanceId);
        }
	 }
}
