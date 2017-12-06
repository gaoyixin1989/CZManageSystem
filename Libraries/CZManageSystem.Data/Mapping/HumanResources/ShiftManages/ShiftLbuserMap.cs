using CZManageSystem.Data.Domain.HumanResources.ShiftManages;
using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.HumanResources.ShiftManages
{
    /// <summary>
    /// �ְ��û�
    /// </summary>
	public class ShiftLbuserMap : EntityTypeConfiguration<ShiftLbuser>
	{
		public ShiftLbuserMap()
		{
			// Primary Key
			 this.HasKey(t => t.Id);
			// Table & Column Mappings
 			 this.ToTable("ShiftLbuser"); 
			// ���ID
			this.Property(t => t.Id).HasColumnName("Id"); 
			// �ְ���Ϣ��Id
			this.Property(t => t.LunbanId).HasColumnName("LunbanId");
            // �ְ��û�id
            this.Property(t => t.UserId).HasColumnName("UserId");


            // Relationships
            //this.HasOptional(t => t.UserObj).WithMany().HasForeignKey(d => d.UserId);
            this.HasRequired(t => t.UserObj).WithMany().HasForeignKey(d => d.UserId);

        }
	 }
}
