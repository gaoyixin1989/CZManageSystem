using CZManageSystem.Data.Domain.HumanResources.Attendance;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Attendance
{
	public class HRTimeConfigMap : EntityTypeConfiguration<HRTimeConfig>
	{
		public HRTimeConfigMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
			// Table & Column Mappings
 			 this.ToTable("HRTimeConfig"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 时间数量
			this.Property(t => t.SpanTime).HasColumnName("SpanTime"); 
			// 上班可提前打卡时间
			this.Property(t => t.LeadTime).HasColumnName("LeadTime"); 
			// 指纹考勤下班可推迟时间
			this.Property(t => t.LatestTime).HasColumnName("LatestTime"); 
		 }
	 }
}
