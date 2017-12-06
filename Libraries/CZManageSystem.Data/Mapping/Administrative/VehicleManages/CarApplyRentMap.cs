using CZManageSystem.Data.Domain.Administrative.VehicleManages;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.Administrative.VehicleManages
{
	public class CarApplyRentMap : EntityTypeConfiguration<CarApplyRent>
	{
		public CarApplyRentMap()
		{
			// Primary Key
			 this.HasKey(t => t.ApplyRentId);
		/// <summary>
		/// 所在部门
		/// <summary>
			  this.Property(t => t.DeptName)

			.HasMaxLength(100);
            this.Property(t => t.ApplySn)
               .HasMaxLength(50);
            /// <summary>
            /// 申请人
            /// <summary>
            this.Property(t => t.ApplyCant)

			.HasMaxLength(100);
            this.Property(t => t.ApplySn)
               .HasMaxLength(50);
            /// <summary>
            /// 标题
            /// <summary>
            this.Property(t => t.ApplyTitle)
            .HasMaxLength(150);

            /// <summary>
            /// 驾驶人、使用人
            /// <summary>
            this.Property(t => t.Driver)

			.HasMaxLength(100);
		/// <summary>
		/// 联系电话
		/// <summary>
			  this.Property(t => t.Mobile)

			.HasMaxLength(100);
		/// <summary>
		/// 出发地点
		/// <summary>
			  this.Property(t => t.Starting)

			.HasMaxLength(100);
		/// <summary>
		/// 目的地1
		/// <summary>
			  this.Property(t => t.Destination1)

			.HasMaxLength(100);
		/// <summary>
		/// 目的地2
		/// <summary>
			  this.Property(t => t.Destination2)

			.HasMaxLength(100);
		/// <summary>
		/// 目的地3
		/// <summary>
			  this.Property(t => t.Destination3)

			.HasMaxLength(100);
		/// <summary>
		/// 目的地4
		/// <summary>
			  this.Property(t => t.Destination4)

			.HasMaxLength(100);
		/// <summary>
		/// 目的地5
		/// <summary>
			  this.Property(t => t.Destination5)

			.HasMaxLength(100);
		/// <summary>
		/// 总人数
		/// <summary>
			  this.Property(t => t.PersonCount)

			.HasMaxLength(100);
		/// <summary>
		/// 路途类别
		/// <summary>
			  this.Property(t => t.Road)

			.HasMaxLength(100);
		/// <summary>
		/// 车辆用途
		/// <summary>
			  this.Property(t => t.UseType)

			.HasMaxLength(100);
		/// <summary>
		/// 请求？
		/// <summary>
			  this.Property(t => t.Request)

			.HasMaxLength(500);
		/// <summary>
		/// 附加
		/// <summary>
			  this.Property(t => t.Attach)

			.HasMaxLength(2000);
		/// <summary>
		/// 领域00
		/// <summary>
			  this.Property(t => t.Field00)

			.HasMaxLength(200);
		/// <summary>
		/// 领域01
		/// <summary>
			  this.Property(t => t.Field01)

			.HasMaxLength(200);
		/// <summary>
		/// 领域02
		/// <summary>
			  this.Property(t => t.Field02)

			.HasMaxLength(200);
		/// <summary>
		/// 安排人、分配者
		/// <summary>
			  this.Property(t => t.Allocator)

			.HasMaxLength(100);
		/// <summary>
		/// 备注
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(500);
		/// <summary>
		/// 租车类型
		/// <summary>
			  this.Property(t => t.Htype)

			.HasMaxLength(50);
		/// <summary>
		/// 吨位/人数
		/// <summary>
			  this.Property(t => t.CarTonnage)

			.HasMaxLength(100);
		/// <summary>
		/// 租车询价
		/// <summary>
			  this.Property(t => t.Enquiry)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("CarApplyRent"); 
			this.Property(t => t.ApplyRentId).HasColumnName("ApplyRentId"); 
			// 流程实例Id
			this.Property(t => t.WorkflowInstanceId).HasColumnName("WorkflowInstanceId");
            this.Property(t => t.ApplySn).HasColumnName("ApplySn");
            // 标题
            this.Property(t => t.ApplyTitle).HasColumnName("ApplyTitle");
            this.Property(t => t.ApplySn).HasColumnName("ApplySn");
            // 所属单位
            this.Property(t => t.CorpId).HasColumnName("CorpId"); 
			// 申请时间
			this.Property(t => t.CreateTime).HasColumnName("CreateTime"); 
			// 所在部门
			this.Property(t => t.DeptName).HasColumnName("DeptName"); 
			// 申请人
			this.Property(t => t.ApplyCant).HasColumnName("ApplyCant"); 
			// 驾驶人、使用人
			this.Property(t => t.Driver).HasColumnName("Driver"); 
			// 联系电话
			this.Property(t => t.Mobile).HasColumnName("Mobile"); 
			// 预计结束时间
			this.Property(t => t.TimeOut).HasColumnName("TimeOut"); 
			// 出车开始时间
			this.Property(t => t.StartTime).HasColumnName("StartTime"); 
			// 结束时间
			this.Property(t => t.EndTime).HasColumnName("EndTime"); 
			// 出发地点
			this.Property(t => t.Starting).HasColumnName("Starting"); 
			// 目的地1
			this.Property(t => t.Destination1).HasColumnName("Destination1"); 
			// 目的地2
			this.Property(t => t.Destination2).HasColumnName("Destination2"); 
			// 目的地3
			this.Property(t => t.Destination3).HasColumnName("Destination3"); 
			// 目的地4
			this.Property(t => t.Destination4).HasColumnName("Destination4"); 
			// 目的地5
			this.Property(t => t.Destination5).HasColumnName("Destination5"); 
			// 总人数
			this.Property(t => t.PersonCount).HasColumnName("PersonCount"); 
			// 路途类别
			this.Property(t => t.Road).HasColumnName("Road"); 
			// 车辆用途
			this.Property(t => t.UseType).HasColumnName("UseType"); 
			// 请求？
			this.Property(t => t.Request).HasColumnName("Request"); 
			// 附加
			this.Property(t => t.Attach).HasColumnName("Attach"); 
			// 领域00
			this.Property(t => t.Field00).HasColumnName("Field00"); 
			// 领域01
			this.Property(t => t.Field01).HasColumnName("Field01"); 
			// 领域02
			this.Property(t => t.Field02).HasColumnName("Field02"); 
			// 安排人、分配者
			this.Property(t => t.Allocator).HasColumnName("Allocator"); 
			// 分配人审批的时间
			this.Property(t => t.AllotTime).HasColumnName("AllotTime"); 
			// 备注
			this.Property(t => t.Remark).HasColumnName("Remark"); 
			// 租车类型
			this.Property(t => t.Htype).HasColumnName("Htype"); 
			// 吨位/人数
			this.Property(t => t.CarTonnage).HasColumnName("CarTonnage"); 
			// 租车询价
			this.Property(t => t.Enquiry).HasColumnName("Enquiry"); 
			// 是否在费用预算额度内
			this.Property(t => t.AllotRight).HasColumnName("AllotRight");
            this.HasOptional(t => t.TrackingWorkflow)
        .WithMany()
        .HasForeignKey(d => d.WorkflowInstanceId);
        }
	 }
}
