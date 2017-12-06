using CZManageSystem.Data.Domain.Administrative.VehicleManages;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.Administrative.VehicleManages
{
	public class CarFeeYearMap : EntityTypeConfiguration<CarFeeYear>
	{
		public CarFeeYearMap()
		{
			// Primary Key
			 this.HasKey(t => t.CarFeeYearId);
		/// <summary>
		/// ʹ�õ�λ
		/// <summary>
			  this.Property(t => t.CorpName)

			.HasMaxLength(100);
		/// <summary>
		/// ������
		/// <summary>
			  this.Property(t => t.Person)

			.HasMaxLength(100);
		/// <summary>
		/// ��ע
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(500);
			// Table & Column Mappings
 			 this.ToTable("CarFeeYear"); 
			// ����
			this.Property(t => t.CarFeeYearId).HasColumnName("CarFeeYearId"); 
			// �༭��ID
			this.Property(t => t.EditorId).HasColumnName("EditorId"); 
			// �༭����
			this.Property(t => t.EditTime).HasColumnName("EditTime"); 
			// ������λ
			this.Property(t => t.CorpId).HasColumnName("CorpId"); 
			// ʹ�õ�λ
			this.Property(t => t.CorpName).HasColumnName("CorpName"); 
			// ����ID
			this.Property(t => t.CarId).HasColumnName("CarId"); 
			// �ɷ�����
			this.Property(t => t.PayTime).HasColumnName("PayTime"); 
			// ����С��
			this.Property(t => t.TotalFee).HasColumnName("TotalFee"); 
			// ������
			this.Property(t => t.Person).HasColumnName("Person"); 
			// ��ע
			this.Property(t => t.Remark).HasColumnName("Remark");

            this.HasOptional(t => t.CarInfo).WithMany().HasForeignKey(t => t.CarId);
        }
	 }
}
