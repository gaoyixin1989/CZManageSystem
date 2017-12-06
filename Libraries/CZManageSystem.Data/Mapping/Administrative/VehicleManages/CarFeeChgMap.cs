using CZManageSystem.Data.Domain.Administrative.VehicleManages;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.Administrative.VehicleManages
{
	public class CarFeeChgMap : EntityTypeConfiguration<CarFeeChg>
	{
		public CarFeeChgMap()
		{
			// Primary Key
			 this.HasKey(t => t.CarFeeChgId);
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
 			 this.ToTable("CarFeeChg"); 
			this.Property(t => t.CarFeeChgId).HasColumnName("CarFeeChgId"); 
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
			// ���¹�����
			this.Property(t => t.RoadLast).HasColumnName("RoadLast"); 
			// ���¹�����
			this.Property(t => t.RoadThis).HasColumnName("RoadThis"); 
			// ������ʻ������
			this.Property(t => t.RoadCount).HasColumnName("RoadCount"); 
			// ʵ������
			this.Property(t => t.OilCount).HasColumnName("OilCount"); 
			// �ͼ�
			this.Property(t => t.OilPrice).HasColumnName("OilPrice"); 
			// ���ͷ�
			this.Property(t => t.OilFee).HasColumnName("OilFee"); 
			// ά�޷�
			this.Property(t => t.FixFee).HasColumnName("FixFee"); 
			// ·��/ͣ����
			this.Property(t => t.RoadFee).HasColumnName("RoadFee"); 
			// ס�޷�
			this.Property(t => t.LiveFee).HasColumnName("LiveFee"); 
			// �ͷ�
			this.Property(t => t.EatFee).HasColumnName("EatFee"); 
			// �����ӷ�
			this.Property(t => t.OtherFee).HasColumnName("OtherFee"); 
			// ����С��
			this.Property(t => t.TotalFee).HasColumnName("TotalFee"); 
			// ������
			this.Property(t => t.Person).HasColumnName("Person"); 
			// ��ע
			this.Property(t => t.Remark).HasColumnName("Remark");

            this.HasOptional(t => t.CarInfo).WithMany()
               .HasForeignKey(d => d.CarId);
        }
	 }
}
