
using System.Data.Entity.ModelConfiguration;
using CZManageSystem.Data.Domain.Administrative;

namespace CZManageSystem.Data.Mapping.Administrative
{
	public class BoardroomApplyMap : EntityTypeConfiguration<BoardroomApply>
	{
		public BoardroomApplyMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// 主题
		/// <summary>
			  this.Property(t => t.ApplyTitle)

			.HasMaxLength(200);
            /// <summary>
            /// 申请ID
            /// <summary>
            this.Property(t => t.WorkflowInstanceId)
          .HasMaxLength(50);
            /// <summary>
            /// 流程单号
            /// <summary>
            this.Property(t => t.Code)
			.HasMaxLength(50);
		/// <summary>
		/// 状态备注，撤销状态时必须填写撤销原因
		/// <summary>
			  this.Property(t => t.StateRemark)

			.HasMaxLength(200);
		/// <summary>
		/// 申请部门
		/// <summary>
			  this.Property(t => t.AppDept)

			.HasMaxLength(200);
		/// <summary>
		/// 申请人
		/// <summary>
			  this.Property(t => t.AppPerson)

			.HasMaxLength(200);
		/// <summary>
		/// 联系电话
		/// <summary>
			  this.Property(t => t.ContactMobile)

			.HasMaxLength(200);
		/// <summary>
		/// 与会人数
		/// <summary>
			  this.Property(t => t.JoinNum)

			.HasMaxLength(200);
            /// <summary>
            /// 与会人员
            /// <summary>
            this.Property(t => t.JoinPeople);
		/// <summary>
		/// 开始时间
		/// <summary>
			  this.Property(t => t.StartTime)

			.HasMaxLength(50);
		/// <summary>
		/// 结束时间
		/// <summary>
			  this.Property(t => t.EndTime)

			.HasMaxLength(50);
		/// <summary>
		/// 会议时间段
		/// <summary>
			  this.Property(t => t.UseFor)

			.HasMaxLength(200);
		/// <summary>
		/// 领导参加情况
		/// <summary>
			  this.Property(t => t.Fugle)
			.HasMaxLength(200);
		/// <summary>
		/// 所需设备
		/// <summary>
			  this.Property(t => t.NeedEquip)

			.HasMaxLength(200);
		/// <summary>
		/// 需要的其他设备
		/// <summary>
			  this.Property(t => t.OtherEquip)

			.HasMaxLength(200);
		/// <summary>
		/// 横幅内容
		/// <summary>
			  this.Property(t => t.BannerContent)

			.HasMaxLength(200);
		/// <summary>
		/// 横幅长度
		/// <summary>
			  this.Property(t => t.BannerLength)

			.HasMaxLength(50);
		/// <summary>
		/// 横幅宽度
		/// <summary>
			  this.Property(t => t.BannerWidth)

			.HasMaxLength(50);
		/// <summary>
		/// 横幅模式
		/// <summary>
			  this.Property(t => t.BannerMode)

			.HasMaxLength(50);
		/// <summary>
		/// 欢迎词
		/// <summary>
			  this.Property(t => t.WelcomeContent)

			.HasMaxLength(200);
		/// <summary>
		/// 播放段
		/// <summary>
			  this.Property(t => t.WelcoomeSect)

			.HasMaxLength(200);
		/// <summary>
		/// 备注
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(200);
		/// <summary>
		/// 自定义字段
		/// <summary>
			  this.Property(t => t.Field00)

			.HasMaxLength(200);
			  this.Property(t => t.Field01)

			.HasMaxLength(200);
			  this.Property(t => t.Field02)

			.HasMaxLength(200);
		/// <summary>
		/// 编辑人
		/// <summary>
			  this.Property(t => t.Editor)

			.HasMaxLength(200);
		/// <summary>
		/// 评价：服务质量：好、较好、一般、差
		/// <summary>
			  this.Property(t => t.JudgeServiceQuality)

			.HasMaxLength(200);
		/// <summary>
		/// 评价：环境卫生：好、较好、一般、差
		/// <summary>
			  this.Property(t => t.JudgeEnvironmental)

			.HasMaxLength(200);
		/// <summary>
		/// 其他建议
		/// <summary>
			  this.Property(t => t.JudgeOtherSuggest)

			.HasMaxLength(200);
			// Table & Column Mappings
 			 this.ToTable("BoardroomApply"); 
			// 编号
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 申请ID
			this.Property(t => t.WorkflowInstanceId).HasColumnName("WorkflowInstanceId"); 
			// 主题
			this.Property(t => t.ApplyTitle).HasColumnName("ApplyTitle"); 
			// 所属单位
			this.Property(t => t.CorpID).HasColumnName("CorpID"); 
			// 会议结束日期
			this.Property(t => t.EndDate).HasColumnName("EndDate");
            // 会议的真实结束时间(默认为申请时的结束时间)
            this.Property(t => t.EndDate_Real).HasColumnName("EndDate_Real");
            // 流程单号
            this.Property(t => t.Code).HasColumnName("Code"); 
			// 状态：0待提交、1待审核、2待评价、3完成、-1撤销
			this.Property(t => t.State).HasColumnName("State"); 
			// 状态备注，撤销状态时必须填写撤销原因
			this.Property(t => t.StateRemark).HasColumnName("StateRemark"); 
			// 申请部门
			this.Property(t => t.AppDept).HasColumnName("AppDept"); 
			// 申请人
			this.Property(t => t.AppPerson).HasColumnName("AppPerson");
            // 申请时间
            this.Property(t => t.AppTime).HasColumnName("AppTime");
            // 联系电话
            this.Property(t => t.ContactMobile).HasColumnName("ContactMobile"); 
			// 会议室ID
			this.Property(t => t.Room).HasColumnName("Room"); 
			// 与会人数
			this.Property(t => t.JoinNum).HasColumnName("JoinNum"); 
			// 与会人员
			this.Property(t => t.JoinPeople).HasColumnName("JoinPeople"); 
			// 会议开始日期
			this.Property(t => t.MeetingDate).HasColumnName("MeetingDate"); 
			// 开始时间
			this.Property(t => t.StartTime).HasColumnName("StartTime"); 
			// 结束时间
			this.Property(t => t.EndTime).HasColumnName("EndTime"); 
			// 会议提醒时间
			this.Property(t => t.AwokeTime).HasColumnName("AwokeTime"); 
			// 会议时间段
			this.Property(t => t.UseFor).HasColumnName("UseFor"); 
			// 领导参加情况
			this.Property(t => t.Fugle).HasColumnName("Fugle");
            // 是否有省公司领导参加
            this.Property(t => t.Fugle_pro).HasColumnName("Fugle_pro");
			// 所需设备
			this.Property(t => t.NeedEquip).HasColumnName("NeedEquip"); 
			// 需要的其他设备
			this.Property(t => t.OtherEquip).HasColumnName("OtherEquip"); 
			// 横幅内容
			this.Property(t => t.BannerContent).HasColumnName("BannerContent"); 
			// 横幅长度
			this.Property(t => t.BannerLength).HasColumnName("BannerLength"); 
			// 横幅宽度
			this.Property(t => t.BannerWidth).HasColumnName("BannerWidth"); 
			// 横幅模式
			this.Property(t => t.BannerMode).HasColumnName("BannerMode"); 
			// 欢迎词
			this.Property(t => t.WelcomeContent).HasColumnName("WelcomeContent"); 
			// 欢迎词播放时间
			this.Property(t => t.WelcoomeTime).HasColumnName("WelcoomeTime"); 
			// 播放段
			this.Property(t => t.WelcoomeSect).HasColumnName("WelcoomeSect"); 
			// 订会时间
			this.Property(t => t.BookTime).HasColumnName("BookTime"); 
			// 备注
			this.Property(t => t.Remark).HasColumnName("Remark"); 
			// 自定义字段
			this.Property(t => t.Field00).HasColumnName("Field00"); 
			this.Property(t => t.Field01).HasColumnName("Field01"); 
			this.Property(t => t.Field02).HasColumnName("Field02"); 
			// 编辑人
			this.Property(t => t.Editor).HasColumnName("Editor"); 
			// 评价：服务质量：好、较好、一般、差
			this.Property(t => t.JudgeServiceQuality).HasColumnName("JudgeServiceQuality"); 
			// 评价：环境卫生：好、较好、一般、差
			this.Property(t => t.JudgeEnvironmental).HasColumnName("JudgeEnvironmental"); 
			// 其他建议
			this.Property(t => t.JudgeOtherSuggest).HasColumnName("JudgeOtherSuggest"); 
			// 评价状态：0或空未评价，1已经评价，2自动评价
			this.Property(t => t.JudgeState).HasColumnName("JudgeState");
            // 是否电视会议
            this.Property(t => t.ISTVMeeting).HasColumnName("ISTVMeeting");
            // 评价时间
            this.Property(t => t.JudgeTime).HasColumnName("JudgeTime");
        }
	 }
}
