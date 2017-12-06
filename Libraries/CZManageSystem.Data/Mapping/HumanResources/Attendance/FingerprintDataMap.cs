using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using CZManageSystem.Data.Domain.HumanResources.Attendance;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Attendance
{
	public class FingerprintDataMap : EntityTypeConfiguration<FingerprintData>
	{
		public FingerprintDataMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
			  this.Property(t => t.DevicePhyAddr)

			.HasMaxLength(50);
			  this.Property(t => t.RecTypes)

			.HasMaxLength(50);
			  this.Property(t => t.RecStatuss)

			.HasMaxLength(50);
			  this.Property(t => t.CardSerno)

			.HasMaxLength(50);
			  this.Property(t => t.EmpNumber)

			.HasMaxLength(50);
			  this.Property(t => t.Operate)

			.HasMaxLength(50);
			  this.Property(t => t.DoorName)

			.HasMaxLength(50);
			  this.Property(t => t.DeviceserNo)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("FingerprintData"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			this.Property(t => t.DevicePhyAddr).HasColumnName("DevicePhyAddr"); 
			this.Property(t => t.RecType).HasColumnName("RecType"); 
			this.Property(t => t.RecStatus).HasColumnName("RecStatus"); 
			this.Property(t => t.RecTypes).HasColumnName("RecTypes"); 
			this.Property(t => t.RecStatuss).HasColumnName("RecStatuss"); 
			this.Property(t => t.CardSerno).HasColumnName("CardSerno"); 
			this.Property(t => t.EmpNumber).HasColumnName("EmpNumber"); 
			this.Property(t => t.Rectime).HasColumnName("Rectime"); 
			this.Property(t => t.EnsureTime).HasColumnName("EnsureTime"); 
			this.Property(t => t.Operate).HasColumnName("Operate"); 
			this.Property(t => t.Status).HasColumnName("Status"); 
			this.Property(t => t.OpDatetime).HasColumnName("OpDatetime"); 
			this.Property(t => t.DoorName).HasColumnName("DoorName"); 
			this.Property(t => t.DeviceserNo).HasColumnName("DeviceserNo"); 
			this.Property(t => t.DoorId).HasColumnName("DoorId"); 
			this.Property(t => t.EmpId).HasColumnName("EmpId"); 
			this.Property(t => t.Tid).HasColumnName("Tid"); 
			this.Property(t => t.ActionStatus).HasColumnName("ActionStatus"); 
		 }
	 }
}
