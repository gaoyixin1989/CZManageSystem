using CZManageSystem.Data.Domain.HumanResources.Integral;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Integral
{
	public class HRIntegralTempMap : EntityTypeConfiguration<HRIntegralTemp>
	{
		public HRIntegralTempMap()
		{
            // Primary Key
            this.HasKey(t=>t.IntegralId );
		/// <summary>
		/// ��ע
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(1073741823);
		/// <summary>
		/// ����
		/// <summary>
			  this.Property(t => t.IntegralType)

			.HasMaxLength(100);
			  this.Property(t => t.FinishTime)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("HRIntegralTemp"); 
			this.Property(t => t.IntegralId).HasColumnName("IntegralId"); 
			// �ô�ID
			this.Property(t => t.UserId).HasColumnName("UserId"); 
			// ����
			this.Property(t => t.Integral).HasColumnName("Integral"); 
			// ���
			this.Property(t => t.YearDate).HasColumnName("YearDate"); 
			// ��ע
			this.Property(t => t.Remark).HasColumnName("Remark"); 
			// ������Դ
			this.Property(t => t.Source).HasColumnName("Source"); 
			// ɾ��
			this.Property(t => t.Del).HasColumnName("Del"); 
			// ����
			this.Property(t => t.IntegralType).HasColumnName("IntegralType"); 
			// ��ѵ����
			this.Property(t => t.CIntegral).HasColumnName("CIntegral"); 
			// �ڿλ���
			this.Property(t => t.TIntegral).HasColumnName("TIntegral"); 
			// ���뵥ID
			this.Property(t => t.ApplyId).HasColumnName("ApplyId"); 
			this.Property(t => t.TPeriodTime).HasColumnName("TPeriodTime"); 
			this.Property(t => t.Daoid).HasColumnName("Daoid"); 
			this.Property(t => t.FinishTime).HasColumnName("FinishTime"); 
		 }
	 }
}
