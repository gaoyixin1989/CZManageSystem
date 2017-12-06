
using System.Data.Entity.ModelConfiguration;
using CZManageSystem.Data.Domain.Administrative;

namespace CZManageSystem.Data.Mapping.Administrative
{
    public class BoardroomInfoMap : EntityTypeConfiguration<BoardroomInfo>
    {
        public BoardroomInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.BoardroomID);
            /// <summary>
            /// 编辑者
            /// <summary>
            this.Property(t => t.Editor)
          .HasMaxLength(50);
            /// <summary>
            /// 编号
            /// <summary>
            this.Property(t => t.Code)
          .HasMaxLength(50);
            /// <summary>
            /// 名称
            /// <summary>
            this.Property(t => t.Name)
            .HasMaxLength(50);
            /// <summary>
            /// 地点
            /// <summary>
            this.Property(t => t.Address)
          .HasMaxLength(200);
            /// <summary>
            /// 设备
            /// <summary>
            this.Property(t => t.Equip)
          .HasMaxLength(50);
            /// <summary>
            /// 其他设备
            /// <summary>
            this.Property(t => t.EquipOther)
          .HasMaxLength(200);
            /// <summary>
            /// 用途
            /// <summary>
            this.Property(t => t.Purpose)
          .HasMaxLength(200);
            /// <summary>
            /// 预订模式
            /// <summary>
            this.Property(t => t.BookMode)
          .HasMaxLength(200);
          //  /// <summary>
          //  /// 管理单位
          //  /// <summary>
          //  this.Property(t => t.ManagerUnit)
          //.HasMaxLength(200);
          //  /// <summary>
          //  /// 管理人
          //  /// <summary>
          //  this.Property(t => t.ManagerPerson)
          //.HasMaxLength(200);
            /// <summary>
            /// 状态
            /// <summary>
            this.Property(t => t.State)
          .HasMaxLength(50);
            /// <summary>
            /// 备注
            /// <summary>
            this.Property(t => t.Remark)
          .HasMaxLength(500);
            /// <summary>
            /// 自定义字段
            /// <summary>
            this.Property(t => t.Field00)
          .HasMaxLength(200);
            this.Property(t => t.Field01)
          .HasMaxLength(200);
            this.Property(t => t.Field02)
          .HasMaxLength(200);
            // Table & Column Mappings
            this.ToTable("BoardroomInfo");
            // 会议室ID
            this.Property(t => t.BoardroomID).HasColumnName("BoardroomID");
            // 编辑人
            this.Property(t => t.Editor).HasColumnName("Editor");
            // 编辑时间
            this.Property(t => t.EditTime).HasColumnName("EditTime");
            // 所属单位
            this.Property(t => t.CorpID).HasColumnName("CorpID");
            // 编号
            this.Property(t => t.Code).HasColumnName("Code");
            // 名称
            this.Property(t => t.Name).HasColumnName("Name");
            // 地点
            this.Property(t => t.Address).HasColumnName("Address");
            // 最大人数
            this.Property(t => t.MaxMan).HasColumnName("MaxMan");
            // 设备
            this.Property(t => t.Equip).HasColumnName("Equip");
            // 其他设备
            this.Property(t => t.EquipOther).HasColumnName("EquipOther");
            // 用途
            this.Property(t => t.Purpose).HasColumnName("Purpose");
            // 预订模式
            this.Property(t => t.BookMode).HasColumnName("BookMode");
            // 管理单位
            this.Property(t => t.ManagerUnit).HasColumnName("ManagerUnit");
            // 管理人
            this.Property(t => t.ManagerPerson).HasColumnName("ManagerPerson");
            // 状态
            this.Property(t => t.State).HasColumnName("State");
            // 备注
            this.Property(t => t.Remark).HasColumnName("Remark");
            // 停用开始时间
            this.Property(t => t.StartTime).HasColumnName("StartTime");
            // 停用结束时间
            this.Property(t => t.EndTime).HasColumnName("EndTime");
            // 自定义字段
            this.Property(t => t.Field00).HasColumnName("Field00");
            this.Property(t => t.Field01).HasColumnName("Field01");
            this.Property(t => t.Field02).HasColumnName("Field02");
        }
    }
}
