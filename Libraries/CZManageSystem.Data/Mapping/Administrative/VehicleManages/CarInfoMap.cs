using CZManageSystem.Data.Domain.Administrative.VehicleManages;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.Administrative.VehicleManages
{
	public class CarInfoMap : EntityTypeConfiguration<CarInfo>
	{
		public CarInfoMap()
		{
			// Primary Key
			 this.HasKey(t => t.CarId);
		/// <summary>
		/// ���
		/// <summary>
			  this.Property(t => t.SN)

			.HasMaxLength(100);
		/// <summary>
		/// ���ƺ���
		/// <summary>
			  this.Property(t => t.LicensePlateNum)

			.HasMaxLength(100);
		/// <summary>
		/// ����Ʒ��
		/// <summary>
			  this.Property(t => t.CarBrand)

			.HasMaxLength(100);
		/// <summary>
		/// �ͺ�
		/// <summary>
			  this.Property(t => t.CarModel)

			.HasMaxLength(100);
		/// <summary>
		/// ��������
		/// <summary>
			  this.Property(t => t.CarEngine)

			.HasMaxLength(100);
		/// <summary>
		/// ���ܺ�
		/// <summary>
			  this.Property(t => t.CarNum)

			.HasMaxLength(100);
		/// <summary>
		/// ��������
		/// <summary>
			  this.Property(t => t.CarType)

			.HasMaxLength(100);
		/// <summary>
		/// ��λ/����
		/// <summary>
			  this.Property(t => t.CarTonnage)

			.HasMaxLength(100);
		/// <summary>
		/// ������
		/// <summary>
			  this.Property(t => t.DeptName)

			.HasMaxLength(100);
		/// <summary>
		/// �����
		/// <summary>
			  this.Property(t => t.CarPrice)

			.HasMaxLength(100);
		/// <summary>
		/// �۾�����
		/// <summary>
			  this.Property(t => t.CarLimit)

			.HasMaxLength(100);
		/// <summary>
		/// ÿ���۾�
		/// <summary>
			  this.Property(t => t.Depre)

			.HasMaxLength(100);
			  this.Property(t => t.Field00)

			.HasMaxLength(200);
			  this.Property(t => t.Field01)

			.HasMaxLength(200);
			  this.Property(t => t.Field02)

			.HasMaxLength(200);
		/// <summary>
		/// ��ע
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(500);
			// Table & Column Mappings
 			 this.ToTable("CarInfo"); 
			// ����
			this.Property(t => t.CarId).HasColumnName("CarId"); 
			// �༭��
			this.Property(t => t.EditorId).HasColumnName("EditorId"); 
			// �༭����
			this.Property(t => t.EditTime).HasColumnName("EditTime"); 
			// ������λ
			this.Property(t => t.CorpId).HasColumnName("CorpId"); 
			// ���
			this.Property(t => t.SN).HasColumnName("SN"); 
			// ���ƺ���
			this.Property(t => t.LicensePlateNum).HasColumnName("LicensePlateNum"); 
			// ����Ʒ��
			this.Property(t => t.CarBrand).HasColumnName("CarBrand"); 
			// �ͺ�
			this.Property(t => t.CarModel).HasColumnName("CarModel"); 
			// ��������
			this.Property(t => t.CarEngine).HasColumnName("CarEngine"); 
			// ���ܺ�
			this.Property(t => t.CarNum).HasColumnName("CarNum"); 
			// ��������
			this.Property(t => t.CarType).HasColumnName("CarType"); 
			// ��λ/����
			this.Property(t => t.CarTonnage).HasColumnName("CarTonnage"); 
			// ������
			this.Property(t => t.DeptName).HasColumnName("DeptName"); 
			// ��������
			this.Property(t => t.BuyDate).HasColumnName("BuyDate"); 
			// �����
			this.Property(t => t.CarPrice).HasColumnName("CarPrice"); 
			// �۾�����
			this.Property(t => t.CarLimit).HasColumnName("CarLimit"); 
			// ÿ���۾�
			this.Property(t => t.Depre).HasColumnName("Depre"); 
			// ���޿�ʼʱ��
			this.Property(t => t.RentTime1).HasColumnName("RentTime1"); 
			// ���޽���ʱ��
			this.Property(t => t.RentTime2).HasColumnName("RentTime2"); 
			// ���տ�ʼʱ��
			this.Property(t => t.PolicyTime1).HasColumnName("PolicyTime1"); 
			// ���ս���ʱ��
			this.Property(t => t.PolicyTime2).HasColumnName("PolicyTime2"); 
			// ����ʼʱ��
			this.Property(t => t.AnnualTime1).HasColumnName("AnnualTime1"); 
			// �������ʱ��
			this.Property(t => t.AnnualTime2).HasColumnName("AnnualTime2"); 
			// ��ʻԱ
			this.Property(t => t.DriverId).HasColumnName("DriverId"); 
			// ״̬
			this.Property(t => t.Status).HasColumnName("Status"); 
			this.Property(t => t.Field00).HasColumnName("Field00"); 
			this.Property(t => t.Field01).HasColumnName("Field01"); 
			this.Property(t => t.Field02).HasColumnName("Field02"); 
			// ��ע
			this.Property(t => t.Remark).HasColumnName("Remark");

            this.HasOptional(t => t.CarDriverInfo).WithMany().HasForeignKey(t => t.DriverId);


         }
	 }
}
