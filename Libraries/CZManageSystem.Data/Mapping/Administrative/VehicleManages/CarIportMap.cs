using CZManageSystem.Data.Domain.Administrative.VehicleManages;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.Administrative.VehicleManages
{
	public class CarIportMap : EntityTypeConfiguration<CarIport>
	{
		public CarIportMap()
		{
			// Primary Key
			 this.HasKey(t => t.CarIportId);
		/// <summary>
		/// �û���
		/// <summary>
			  this.Property(t => t.UserName)

			.HasMaxLength(50);
		/// <summary>
		/// �ֻ�
		/// <summary>
			  this.Property(t => t.Mobile)

			.HasMaxLength(100);
			// Table & Column Mappings
 			 this.ToTable("CarIport"); 
			// ����
			this.Property(t => t.CarIportId).HasColumnName("CarIportId"); 
			// ����ʱ��
			this.Property(t => t.EditTime).HasColumnName("EditTime"); 
			// �û���
			this.Property(t => t.UserName).HasColumnName("UserName"); 
			// �ֻ�
			this.Property(t => t.Mobile).HasColumnName("Mobile"); 
			// �ڣ�
			this.Property(t => t.Iport).HasColumnName("Iport"); 
			// ��������ID
			this.Property(t => t.CarApplyId).HasColumnName("CarApplyId"); 
			// ����ID
			this.Property(t => t.CarId).HasColumnName("CarId"); 
			// ״̬
			this.Property(t => t.Status).HasColumnName("Status"); 
		 }
	 }
}
