using CZManageSystem.Data.Domain.Administrative.VehicleManages;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.Administrative.VehicleManages
{
	public class CarFeeRentMap : EntityTypeConfiguration<CarFeeRent>
	{
		public CarFeeRentMap()
		{
			// Primary Key
			 this.HasKey(t => t.CarFeeRentId);
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
 			 this.ToTable("CarFeeRent"); 
			// ����
			this.Property(t => t.CarFeeRentId).HasColumnName("CarFeeRentId"); 
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
			// ����
			this.Property(t => t.SortId).HasColumnName("SortId"); 
			// ���޷���
			this.Property(t => t.RentFee).HasColumnName("RentFee"); 
			// ���޹���
			this.Property(t => t.RentCount).HasColumnName("RentCount"); 
			// ʵ����ʻ����
			this.Property(t => t.RoadCount).HasColumnName("RoadCount"); 
			// ���������
			this.Property(t => t.MoreRoad).HasColumnName("MoreRoad"); 
			// ��������̷���
			this.Property(t => t.MoreFee).HasColumnName("MoreFee"); 
			// ���ͷ�
			this.Property(t => t.GasFee).HasColumnName("GasFee"); 
			// ·��/ͣ����
			this.Property(t => t.RoadFee).HasColumnName("RoadFee"); 
			// ��ʻԱ����
			this.Property(t => t.DriverFee).HasColumnName("DriverFee"); 
			// ����С��
			this.Property(t => t.TotalFee).HasColumnName("TotalFee"); 
			// ��ʼ����
			this.Property(t => t.StartTime).HasColumnName("StartTime"); 
			// ��������
			this.Property(t => t.EndTime).HasColumnName("EndTime"); 
			// ������
			this.Property(t => t.Person).HasColumnName("Person"); 
			// ��ע
			this.Property(t => t.Remark).HasColumnName("Remark"); 
		 }
	 }
}
