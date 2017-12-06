using CZManageSystem.Data.Domain.Administrative.VehicleManages;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.Administrative.VehicleManages
{
	public class CarApplyMap : EntityTypeConfiguration<CarApply>
	{
		public CarApplyMap()
		{
			// Primary Key
			 this.HasKey(t => t.ApplyId);
		/// <summary>
		/// 所在部门
		/// <summary>
			  this.Property(t => t.DeptName)

			.HasMaxLength(100);
            this.Property(t => t.ApplySn)
               .HasMaxLength(50);
            /// <summary>
            /// 标题
            /// <summary>
            this.Property(t => t.ApplyTitle) 
			.HasMaxLength(150);
      
		/// <summary>
		/// 申请人
		/// <summary>
			  this.Property(t => t.ApplyCant)

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
		/// 车辆分配信息
		/// <summary>
			  this.Property(t => t.AllotIntro)

			.HasMaxLength(2000);
		/// <summary>
		/// 备注
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(500);
			  this.Property(t => t.UptUser)

			.HasMaxLength(100);
		/// <summary>
		/// 结算人
		/// <summary>
			  this.Property(t => t.BalUser)

			.HasMaxLength(100);
		/// <summary>
		/// 备注信息
		/// <summary>
			  this.Property(t => t.BalRemark)

			.HasMaxLength(500);
		/// <summary>
		/// 后续修改人
		/// <summary>
			  this.Property(t => t.OpinUser)

			.HasMaxLength(100);
		/// <summary>
		/// 评价行车安全
		/// <summary>
			  this.Property(t => t.OpinGrade1)

			.HasMaxLength(100);
		/// <summary>
		/// 评价服务质量
		/// <summary>
			  this.Property(t => t.OpinGrade2)

			.HasMaxLength(100);
		/// <summary>
		/// 评价车容卫生
		/// <summary>
			  this.Property(t => t.OpinGrade3)

			.HasMaxLength(100);
		/// <summary>
		/// 评价个人仪表
		/// <summary>
			  this.Property(t => t.OpinGrade4)

			.HasMaxLength(100);
		/// <summary>
		/// 评价时间观念
		/// <summary>
			  this.Property(t => t.OpinGrade5)

			.HasMaxLength(100);
		/// <summary>
		/// 评价方向感
		/// <summary>
			  this.Property(t => t.OpinGrade6)

			.HasMaxLength(100);
		/// <summary>
		/// 评价
		/// <summary>
			  this.Property(t => t.OpinGrade7)

			.HasMaxLength(100);
		/// <summary>
		/// 评价备注
		/// <summary>
			  this.Property(t => t.OpinRemark)

			.HasMaxLength(500);
		/// <summary>
		/// 特殊原因说明
		/// <summary>
			  this.Property(t => t.SpecialReason)

			.HasMaxLength(500);
		/// <summary>
		/// 口头申请领导
		/// <summary>
			  this.Property(t => t.Leader)

			.HasMaxLength(150);
			// Table & Column Mappings
 			 this.ToTable("CarApply"); 
			// 主键
			this.Property(t => t.ApplyId).HasColumnName("ApplyId"); 
			// 流程实例Id
			this.Property(t => t.WorkflowInstanceId).HasColumnName("WorkflowInstanceId");
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
            // 申请人ID
            this.Property(t => t.ApplyCantId).HasColumnName("ApplyCantId");
            // 驾驶人、使用人
            this.Property(t => t.Driver).HasColumnName("Driver");
            // 驾驶人、使用人Ids
            this.Property(t => t.DriverIds).HasColumnName("DriverIds");
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
			this.Property(t => t.CarIds).HasColumnName("CarIds"); 
			// 车辆分配信息
			this.Property(t => t.AllotIntro).HasColumnName("AllotIntro"); 
			// 备注
			this.Property(t => t.Remark).HasColumnName("Remark"); 
			// 用车结束时间
			this.Property(t => t.FinishTime).HasColumnName("FinishTime"); 
			this.Property(t => t.UptUser).HasColumnName("UptUser"); 
			this.Property(t => t.UptTime).HasColumnName("UptTime"); 
			// 结算人
			this.Property(t => t.BalUser).HasColumnName("BalUser"); 
			// 结算时间
			this.Property(t => t.BalTime).HasColumnName("BalTime"); 
			// 起始公里数
			this.Property(t => t.KmNum1).HasColumnName("KmNum1"); 
			// 终止公里数
			this.Property(t => t.KmNum2).HasColumnName("KmNum2"); 
			// 本次使用里程
			this.Property(t => t.KmCount).HasColumnName("KmCount"); 
			// 路桥费共几张
			this.Property(t => t.BalCount).HasColumnName("BalCount"); 
			// 合计金额
			this.Property(t => t.BalTotal).HasColumnName("BalTotal"); 
			// 备注信息
			this.Property(t => t.BalRemark).HasColumnName("BalRemark"); 
			// 后续修改人
			this.Property(t => t.OpinUser).HasColumnName("OpinUser"); 
			// 后续修改时间
			this.Property(t => t.OpinTime).HasColumnName("OpinTime"); 
			// 评价行车安全
			this.Property(t => t.OpinGrade1).HasColumnName("OpinGrade1"); 
			// 评价服务质量
			this.Property(t => t.OpinGrade2).HasColumnName("OpinGrade2"); 
			// 评价车容卫生
			this.Property(t => t.OpinGrade3).HasColumnName("OpinGrade3"); 
			// 评价个人仪表
			this.Property(t => t.OpinGrade4).HasColumnName("OpinGrade4"); 
			// 评价时间观念
			this.Property(t => t.OpinGrade5).HasColumnName("OpinGrade5"); 
			// 评价方向感
			this.Property(t => t.OpinGrade6).HasColumnName("OpinGrade6"); 
			// 评价
			this.Property(t => t.OpinGrade7).HasColumnName("OpinGrade7"); 
			// 评价备注
			this.Property(t => t.OpinRemark).HasColumnName("OpinRemark"); 
			// 特殊原因说明
			this.Property(t => t.SpecialReason).HasColumnName("SpecialReason"); 
			// 是否已口头申请
			this.Property(t => t.Boral).HasColumnName("Boral"); 
			// 口头申请领导
			this.Property(t => t.Leader).HasColumnName("Leader"); 
            // 用车申请类型 
            this.Property(t => t.ApplyType).HasColumnName("ApplyType");
            //吨位人数
            this.Property(t => t.CarTonnage).HasColumnName("CarTonnage");

            this.HasOptional(t => t.TrackingWorkflow)
          .WithMany()
          .HasForeignKey(d => d.WorkflowInstanceId);
        }
	 }
}
