using CZManageSystem.Data.Domain.HumanResources.Attendance;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Attendance
{
	public class HRRfsimMap : EntityTypeConfiguration<HRRfsim>
	{
		public HRRfsimMap()
		{
			// Primary Key
			 this.HasKey(t => t.RecordId);
			  this.Property(t => t.SysNo)

			.HasMaxLength(50);
			  this.Property(t => t.Serial)

			.HasMaxLength(50);
			  this.Property(t => t.RecordType)

			.HasMaxLength(50);
		/// <summary>
		/// 操作员ID
		/// <summary>
			  this.Property(t => t.OperatorId)

			.HasMaxLength(50);
			  this.Property(t => t.EmplyName)

			.HasMaxLength(50);
			  this.Property(t => t.EmplyId)

			.HasMaxLength(50);
            this.Property(t => t.DptId)

            .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("HRRfsim"); 
			this.Property(t => t.RecordId).HasColumnName("RecordId");
            this.Property(t => t.Tid).HasColumnName("Tid");
            this.Property(t => t.SysNo).HasColumnName("SysNo"); 
			this.Property(t => t.Serial).HasColumnName("Serial"); 
			this.Property(t => t.CDateTime).HasColumnName("CDateTime"); 
			this.Property(t => t.DeviceSysId).HasColumnName("DeviceSysId"); 
			this.Property(t => t.RecordType).HasColumnName("RecordType"); 
			// 操作员ID
			this.Property(t => t.OperatorId).HasColumnName("OperatorId"); 
			this.Property(t => t.EmplyName).HasColumnName("EmplyName"); 
			this.Property(t => t.DptId).HasColumnName("DptId"); 
			this.Property(t => t.EmplyId).HasColumnName("EmplyId"); 
			this.Property(t => t.ActionStatus).HasColumnName("ActionStatus"); 
		 }
	 }
}
