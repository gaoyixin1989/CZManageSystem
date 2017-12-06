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
			// 车辆ID
			this.Property(t => t.CarId).HasColumnName("CarId"); 
			// 车辆申请ID
			this.Property(t => t.CarApplyId).HasColumnName("CarApplyId"); 
			// 预计结束时间
			this.Property(t => t.TimeOut).HasColumnName("TimeOut"); 
			// 用车结束时间
			this.Property(t => t.FinishTime).HasColumnName("FinishTime"); 
		 }
	 }
}
