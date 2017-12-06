using CZManageSystem.Data.Domain.MarketPlan;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Models.Mapping
{
	public class Ucs_MarketPlan2TmpMap : EntityTypeConfiguration<Ucs_MarketPlan2_Tmp>
	{
		public Ucs_MarketPlan2TmpMap()
		{
			// Primary Key
			 this.HasKey(t => t.Id);
		/// <summary>
		/// Ӫ����������
		/// <summary>
			  this.Property(t => t.Coding)

			.HasMaxLength(64);
		/// <summary>
		/// Ӫ����������
		/// <summary>
			  this.Property(t => t.Name)

			.HasMaxLength(200);
		/// <summary>
		/// ��������
		/// <summary>
			  this.Property(t => t.Channel)

			.HasMaxLength(200);
		/// <summary>
		/// ָ��
		/// <summary>
			  this.Property(t => t.Orders)

			.HasMaxLength(64);
		/// <summary>
		/// ��������ǼǶ˿�
		/// <summary>
			  this.Property(t => t.RegPort)

			.HasMaxLength(64);
		/// <summary>
		/// Ӫ���ϸ��
		/// <summary>
			  this.Property(t => t.DetialInfo)

			.HasMaxLength(500);
		/// <summary>
		/// ��ע
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(200);
		/// <summary>
		/// ��������
		/// <summary>
			  this.Property(t => t.PlanType)

			.HasMaxLength(100);
		/// <summary>
		/// Ŀ���û�Ⱥ
		/// <summary>
			  this.Property(t => t.TargetUsers)

			.HasMaxLength(100);
		/// <summary>
		/// н�������Ҫ
		/// <summary>
			  this.Property(t => t.PaysRlues)

			.HasMaxLength(200);
		/// <summary>
		/// ����ģ��1
		/// <summary>
			  this.Property(t => t.Templet1)

			.HasMaxLength(200);
		/// <summary>
		/// ����ģ��2
		/// <summary>
			  this.Property(t => t.Templet2)

			.HasMaxLength(200);
		/// <summary>
		/// ����ģ��3
		/// <summary>
			  this.Property(t => t.Templet3)

			.HasMaxLength(200);
		/// <summary>
		/// ����ģ��4
		/// <summary>
			  this.Property(t => t.Templet4)

			.HasMaxLength(200);
		/// <summary>
		/// �Ƿ��׼����
		/// <summary>
			  this.Property(t => t.IsMarketing)

			.HasMaxLength(10);
			// Table & Column Mappings
 			 this.ToTable("Ucs_MarketPlan2_Tmp"); 
			this.Property(t => t.Id).HasColumnName("Id"); 
			// Ӫ����������
			this.Property(t => t.Coding).HasColumnName("Coding"); 
			// Ӫ����������
			this.Property(t => t.Name).HasColumnName("Name"); 
			// ��ʼʱ��
			this.Property(t => t.StartTime).HasColumnName("StartTime"); 
			// ����ʱ��
			this.Property(t => t.EndTime).HasColumnName("EndTime"); 
			// ��������
			this.Property(t => t.Channel).HasColumnName("Channel"); 
			// ָ��
			this.Property(t => t.Orders).HasColumnName("Orders"); 
			// ��������ǼǶ˿�
			this.Property(t => t.RegPort).HasColumnName("RegPort"); 
			// Ӫ���ϸ��
			this.Property(t => t.DetialInfo).HasColumnName("DetialInfo"); 
			// ��ע
			this.Property(t => t.Remark).HasColumnName("Remark"); 
			// ��������
			this.Property(t => t.PlanType).HasColumnName("PlanType"); 
			// Ŀ���û�Ⱥ
			this.Property(t => t.TargetUsers).HasColumnName("TargetUsers"); 
			// н�������Ҫ
			this.Property(t => t.PaysRlues).HasColumnName("PaysRlues"); 
			// ����ģ��1
			this.Property(t => t.Templet1).HasColumnName("Templet1"); 
			// ����ģ��2
			this.Property(t => t.Templet2).HasColumnName("Templet2"); 
			// ����ģ��3
			this.Property(t => t.Templet3).HasColumnName("Templet3"); 
			// ����ģ��4
			this.Property(t => t.Templet4).HasColumnName("Templet4"); 
			// ��������
			this.Property(t => t.NumCount).HasColumnName("NumCount"); 
			// �Ƿ��׼����
			this.Property(t => t.IsMarketing).HasColumnName("IsMarketing"); 
		 }
	 }
}
