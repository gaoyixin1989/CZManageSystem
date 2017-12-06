using CZManageSystem.Data.Domain.Administrative.VehicleManages;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.Administrative.VehicleManages
{
	public class CarStatusMap : EntityTypeConfiguration<CarStatus>
	{
		public CarStatusMap()
		{
			// Primary Key
			 this.HasKey(t => t.Id);
			// Table & Column Mappings
 			 this.ToTable("CarStatus"); 
			this.Property(t => t.Id).HasColumnName("Id"); 
			// ����ID
			this.Property(t => t.CarId).HasColumnName("CarId"); 
			// ��������ID
			this.Property(t => t.CarApplyId).HasColumnName("CarApplyId"); 
			// Ԥ�ƽ���ʱ��
			this.Property(t => t.TimeOut).HasColumnName("TimeOut"); 
			// �ó�����ʱ��
			this.Property(t => t.FinishTime).HasColumnName("FinishTime"); 
		 }
	 }
}
