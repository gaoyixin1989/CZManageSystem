using CZManageSystem.Data.Domain.Administrative.VehicleManages;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.Administrative.VehicleManages
{
	public class CarFeeFixMap : EntityTypeConfiguration<CarFeeFix>
	{
		public CarFeeFixMap()
		{
			// Primary Key
			 this.HasKey(t => t.CarFeeFixId);
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
 			 this.ToTable("CarFeeFix"); 
			// ����
			this.Property(t => t.CarFeeFixId).HasColumnName("CarFeeFixId"); 
			// �༭��ID
			this.Property(t => t.EditorId).HasColumnName("EditorId"); 
			// �༭����
			this.Property(t => t.EditTime).HasColumnName("EditTime"); 
			// ������λ
			this.Property(t => t.CorpId).HasColumnName("CorpId"); 
			// ����ID
			this.Property(t => t.CarId).HasColumnName("CarId"); 
			// �ɷ�����
			this.Property(t => t.PayTime).HasColumnName("PayTime"); 
			// ���շ�
			this.Property(t => t.FolicyFee).HasColumnName("FolicyFee"); 
			// ����˰
			this.Property(t => t.TaxFee).HasColumnName("TaxFee"); 
			// ��·����
			this.Property(t => t.RoadFee).HasColumnName("RoadFee"); 
			// �����ӷ�
			this.Property(t => t.OtherFee).HasColumnName("OtherFee"); 
			// ����С��
			this.Property(t => t.TotalFee).HasColumnName("TotalFee"); 
			// �Ʒѿ�ʼ����
			this.Property(t => t.StartTime).HasColumnName("StartTime"); 
			// ��������
			this.Property(t => t.EndTime).HasColumnName("EndTime"); 
			// ������
			this.Property(t => t.Person).HasColumnName("Person"); 
			// ��ע
			this.Property(t => t.Remark).HasColumnName("Remark");
            // Relationships
            this.HasOptional(t => t.CarInfo).WithMany()
                .HasForeignKey(d => d.CarId);
        }
	 }
}
