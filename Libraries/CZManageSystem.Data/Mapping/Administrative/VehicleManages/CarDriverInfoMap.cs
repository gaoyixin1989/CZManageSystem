using CZManageSystem.Data.Domain.Administrative.VehicleManages;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.Administrative.VehicleManages
{
	public class CarDriverInfoMap : EntityTypeConfiguration<CarDriverInfo>
	{
		public CarDriverInfoMap()
		{
			// Primary Key
			 this.HasKey(t => t.DriverId);
		/// <summary>
		/// ˾�����
		/// <summary>
			  this.Property(t => t.SN)

			.HasMaxLength(100);
			  this.Property(t => t.Name)

			.HasMaxLength(100);
		/// <summary>
		/// �ֻ���
		/// <summary>
			  this.Property(t => t.Mobile)

			.HasMaxLength(100);
		/// <summary>
		/// ��������
		/// <summary>
			  this.Property(t => t.DeptName)

			.HasMaxLength(100);
		/// <summary>
		/// ��ע
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(500);
			// Table & Column Mappings
 			 this.ToTable("CarDriverInfo"); 
			// ����
			this.Property(t => t.DriverId).HasColumnName("DriverId"); 
			// �༭��
			this.Property(t => t.EditorId).HasColumnName("EditorId"); 
			// �༭ʱ��
			this.Property(t => t.EditTime).HasColumnName("EditTime"); 
			// ������λ
			this.Property(t => t.CorpId).HasColumnName("CorpId"); 
			// ˾�����
			this.Property(t => t.SN).HasColumnName("SN"); 
			this.Property(t => t.Name).HasColumnName("Name"); 
			// �ֻ���
			this.Property(t => t.Mobile).HasColumnName("Mobile"); 
			// ��������
			this.Property(t => t.DeptName).HasColumnName("DeptName"); 
			// ��ʼ��ʻʱ��
			this.Property(t => t.CarAge).HasColumnName("CarAge"); 
			// ����
			this.Property(t => t.Birthday).HasColumnName("Birthday"); 
			// ��ע
			this.Property(t => t.Remark).HasColumnName("Remark"); 
		 }
	 }
}
