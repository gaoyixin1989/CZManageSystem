using CZManageSystem.Data.Domain.Administrative.VehicleManages;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.Administrative.VehicleManages
{
	public class CarApplyFeeMap : EntityTypeConfiguration<CarApplyFee>
	{
		public CarApplyFeeMap()
		{
			// Primary Key
			 this.HasKey(t => t.ApplyFeeId);
		/// <summary>
		/// ������
		/// <summary>
			  this.Property(t => t.BalUser)

			.HasMaxLength(100);
            
            this.Property(t => t.ApplySn)
               .HasMaxLength(50);
            /// <summary>
            /// ����
            /// <summary>
            this.Property(t => t.ApplyTitle)
           .HasMaxLength(150);
            /// <summary>
            /// ��ע
            /// <summary>
            this.Property(t => t.BalRemark)

			.HasMaxLength(500);
			// Table & Column Mappings
 			 this.ToTable("CarApplyFee"); 
			// ����
			this.Property(t => t.ApplyFeeId).HasColumnName("ApplyFeeId");
            // ����ʵ��Id
            this.Property(t => t.CarApplyId).HasColumnName("CarApplyId");
            this.Property(t => t.ApplySn).HasColumnName("ApplySn");
            // ����
            this.Property(t => t.ApplyTitle).HasColumnName("ApplyTitle");
            // ����ID
            this.Property(t => t.CarId).HasColumnName("CarId"); 
			// ������
			this.Property(t => t.BalUser).HasColumnName("BalUser"); 
			// ����ʱ��
			this.Property(t => t.BalTime).HasColumnName("BalTime"); 
			// ��ʼ���
			this.Property(t => t.KmNum1).HasColumnName("KmNum1"); 
			// ��ֹ���
			this.Property(t => t.KmNum2).HasColumnName("KmNum2"); 
			// ʹ�����
			this.Property(t => t.KmCount).HasColumnName("KmCount"); 
			// ��������
			this.Property(t => t.BalCount).HasColumnName("BalCount"); 
			// �ϼƽ��
			this.Property(t => t.BalTotal).HasColumnName("BalTotal"); 
			// ��ע
			this.Property(t => t.BalRemark).HasColumnName("BalRemark");

            this.HasOptional(t => t.CarApply).WithMany()
               .HasForeignKey(d => d.CarApplyId);

        }
	 }
}
