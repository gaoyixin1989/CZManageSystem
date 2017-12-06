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
			// ʱ������
			this.Property(t => t.SpanTime).HasColumnName("SpanTime"); 
			// �ϰ����ǰ��ʱ��
			this.Property(t => t.LeadTime).HasColumnName("LeadTime"); 
			// ָ�ƿ����°���Ƴ�ʱ��
			this.Property(t => t.LatestTime).HasColumnName("LatestTime"); 
		 }
	 }
}
