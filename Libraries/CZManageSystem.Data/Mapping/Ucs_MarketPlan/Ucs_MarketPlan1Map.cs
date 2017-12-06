using CZManageSystem.Data.Domain.MarketPlan;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Models.Mapping
{
	public class Ucs_MarketPlan1Map : EntityTypeConfiguration<Ucs_MarketPlan1>
	{
		public Ucs_MarketPlan1Map()
		{
			// Primary Key
			 this.HasKey(t => t.Id);
		/// <summary>
		/// Ӫ����������
		/// <summary>
			  this.Property(t => t.Coding)

			.HasMaxLength(64);
		/// <summary>
		/// �û��ֻ���
		/// <summary>
			  this.Property(t => t.Tel)

			.HasMaxLength(200);
		
			// Table & Column Mappings
 			 this.ToTable("Ucs_MarketPlan1"); 
			this.Property(t => t.Id).HasColumnName("Id"); 
			// Ӫ����������
			this.Property(t => t.Coding).HasColumnName("Coding"); 
			// �û��ֻ���
			this.Property(t => t.Tel).HasColumnName("Tel"); 
			
		 }
	 }
}
