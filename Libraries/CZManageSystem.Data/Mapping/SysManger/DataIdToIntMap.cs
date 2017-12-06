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
		/// 数据来源
		/// <summary>
			  this.Property(t => t.DataSource)
			 .IsRequired()
			.HasMaxLength(50);
		/// <summary>
		/// 数据ID
		/// <summary>
			  this.Property(t => t.DataId)
			 .IsRequired()
			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("DataIdToInt"); 
			// int编号
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 数据来源
			this.Property(t => t.DataSource).HasColumnName("DataSource"); 
			// 数据ID
			this.Property(t => t.DataId).HasColumnName("DataId"); 
		 }
	 }
}
