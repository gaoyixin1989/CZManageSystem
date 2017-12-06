using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.SysManger
{
	public class DataIdToIntMap : EntityTypeConfiguration<DataIdToInt>
	{
		public DataIdToIntMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// ������Դ
		/// <summary>
			  this.Property(t => t.DataSource)
			 .IsRequired()
			.HasMaxLength(50);
		/// <summary>
		/// ����ID
		/// <summary>
			  this.Property(t => t.DataId)
			 .IsRequired()
			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("DataIdToInt"); 
			// int���
			this.Property(t => t.ID).HasColumnName("ID"); 
			// ������Դ
			this.Property(t => t.DataSource).HasColumnName("DataSource"); 
			// ����ID
			this.Property(t => t.DataId).HasColumnName("DataId"); 
		 }
	 }
}
