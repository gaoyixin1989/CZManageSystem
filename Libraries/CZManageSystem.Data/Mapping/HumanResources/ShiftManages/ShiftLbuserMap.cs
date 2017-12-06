using CZManageSystem.Data.Domain.HumanResources.ShiftManages;
using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.HumanResources.ShiftManages
{
    /// <summary>
    /// 轮班用户
    /// </summary>
	public class ShiftLbuserMap : EntityTypeConfiguration<ShiftLbuser>
	{
		public ShiftLbuserMap()
		{
			// Primary Key
			 this.HasKey(t => t.Id);
			// Table & Column Mappings
 			 this.ToTable("ShiftLbuser"); 
			// 编号ID
			this.Property(t => t.Id).HasColumnName("Id"); 
			// 轮班信息表Id
			this.Property(t => t.LunbanId).HasColumnName("LunbanId");
            // 轮班用户id
            this.Property(t => t.UserId).HasColumnName("UserId");


            // Relationships
            //this.HasOptional(t => t.UserObj).WithMany().HasForeignKey(d => d.UserId);
            this.HasRequired(t => t.UserObj).WithMany().HasForeignKey(d => d.UserId);

        }
	 }
}
