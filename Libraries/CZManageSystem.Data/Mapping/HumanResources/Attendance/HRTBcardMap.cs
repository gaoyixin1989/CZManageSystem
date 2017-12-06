using CZManageSystem.Data.Domain.HumanResources.Attendance;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Integral
{
	public class HRTBcardMap : EntityTypeConfiguration<HRTBcard>
	{
		public HRTBcardMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// �˺�
		/// <summary>
			  this.Property(t => t.EmployeeId)

			.HasMaxLength(50);
		/// <summary>
		/// �û�ID
		/// <summary>
			  this.Property(t => t.EmpNo)

			.HasMaxLength(50);
		/// <summary>
		/// ͨ������
		/// <summary>
			  this.Property(t => t.CardNo)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("HRTBcard"); 
			// ����
			this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Tid).HasColumnName("Tid");
            // �˺�
            this.Property(t => t.EmployeeId).HasColumnName("EmployeeId"); 
			// ʱ��
			this.Property(t => t.SkTime).HasColumnName("SkTime"); 
			// ״̬
			this.Property(t => t.ActionStatus).HasColumnName("ActionStatus"); 
			// �û�ID
			this.Property(t => t.EmpNo).HasColumnName("EmpNo"); 
			// ͨ������
			this.Property(t => t.CardNo).HasColumnName("CardNo"); 
		 }
	 }
}
