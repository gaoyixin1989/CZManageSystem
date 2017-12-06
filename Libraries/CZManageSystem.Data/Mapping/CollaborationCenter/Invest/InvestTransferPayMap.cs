using CZManageSystem.Data.Domain.CollaborationCenter.Invest;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// ��ת�ʺ�ͬ��
/// </summary>
namespace CZManageSystem.Data.Mapping.CollaborationCenter.Invest
{
	public class InvestTransferPayMap : EntityTypeConfiguration<InvestTransferPay>
	{
		public InvestTransferPayMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// ��ĿID
		/// <summary>
			  this.Property(t => t.ProjectID)

			.HasMaxLength(500);
		/// <summary>
		/// ��ͬID
		/// <summary>
			  this.Property(t => t.ContractID)

			.HasMaxLength(100);
		/// <summary>
		/// �Ƿ���ת��
		/// <summary>
			  this.Property(t => t.IsTransfer)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("InvestTransferPay"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// ��ĿID
			this.Property(t => t.ProjectID).HasColumnName("ProjectID"); 
			// ��ͬID
			this.Property(t => t.ContractID).HasColumnName("ContractID"); 
			// �Ƿ���ת��
			this.Property(t => t.IsTransfer).HasColumnName("IsTransfer"); 
			// ת�ʽ��
			this.Property(t => t.TransferPay).HasColumnName("TransferPay"); 
			// �༭ʱ��
			this.Property(t => t.EditTime).HasColumnName("EditTime"); 
			// �༭��ID
			this.Property(t => t.EditorID).HasColumnName("EditorID"); 
		 }
	 }
}
