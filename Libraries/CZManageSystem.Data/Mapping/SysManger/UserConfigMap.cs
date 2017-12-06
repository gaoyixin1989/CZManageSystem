using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.SysManger
{
	public class UserConfigMap : EntityTypeConfiguration<UserConfig>
	{
		public UserConfigMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// ���ñ�ʶ
		/// <summary>
			  this.Property(t => t.ConfigName)

			.HasMaxLength(500);
		/// <summary>
		/// ����ֵ
		/// <summary>
			  this.Property(t => t.ConfigValue)
;
			// Table & Column Mappings
 			 this.ToTable("UserConfig"); 
			// ���
			this.Property(t => t.ID).HasColumnName("ID"); 
			// �û�id
			this.Property(t => t.UserID).HasColumnName("UserID"); 
			// ���ñ�ʶ
			this.Property(t => t.ConfigName).HasColumnName("ConfigName"); 
			// ����ֵ
			this.Property(t => t.ConfigValue).HasColumnName("ConfigValue");

            //���
           this.HasOptional(t => t.UserObj).WithMany().HasForeignKey(d => d.UserID);
        }
	 }
}
