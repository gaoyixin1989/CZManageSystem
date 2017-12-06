using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.SysManger
{
    public class Uum_Organizationinfo_LogMap : EntityTypeConfiguration<Uum_Organizationinfo_Log>
    {
        public Uum_Organizationinfo_LogMap()
        {
            // Primary Key
            this.HasKey(t => t.OUGUID);
            /// <summary>
            /// 父组织ID(针对APP)
            /// <summary>
            this.Property(t => t.parentOUGUID)

          .HasMaxLength(50);
            /// <summary>
            /// 组织类型(UN: 公司  UM: 部门,TD:团队)
            /// <summary>
            this.Property(t => t.orgTypeID)

          .HasMaxLength(50);
            /// <summary>
            /// 组织状态（1 启用  0停用）
            /// <summary>
            this.Property(t => t.orgState)

          .HasMaxLength(50);
            /// <summary>
            /// 组织名称(portal)/团队名称
            /// <summary>
            this.Property(t => t.OUName)

          .HasMaxLength(100);
            /// <summary>
            /// 组织全名(portal)
            /// <summary>
            this.Property(t => t.OUFullName)

          .HasMaxLength(255);
            /// <summary>
            /// 组织排序(portal)
            /// <summary>
            this.Property(t => t.OUOrder)

          .HasMaxLength(255);
            /// <summary>
            /// 组织中的DN(portal)
            /// <summary>
            this.Property(t => t.OrgDN)

          .HasMaxLength(255);
            /// <summary>
            /// 所属地区(省公司)
            /// <summary>
            this.Property(t => t.region)

          .HasMaxLength(100);
            /// <summary>
            /// 地区序号
            /// <summary>
            this.Property(t => t.regionID)

          .HasMaxLength(50);
            /// <summary>
            /// 所属公司
            /// <summary>
            this.Property(t => t.company)

          .HasMaxLength(100);
            /// <summary>
            /// 公司序号
            /// <summary>
            this.Property(t => t.companyID)

          .HasMaxLength(50);
            /// <summary>
            /// 所属分公司
            /// <summary>
            this.Property(t => t.branch)

          .HasMaxLength(100);
            /// <summary>
            /// 分公司序号
            /// <summary>
            this.Property(t => t.branchID)

          .HasMaxLength(50);
            /// <summary>
            /// 所属部门
            /// <summary>
            this.Property(t => t.department)

          .HasMaxLength(100);
            /// <summary>
            /// 部门序号
            /// <summary>
            this.Property(t => t.departmentID)

          .HasMaxLength(50);
            /// <summary>
            /// 所属科室
            /// <summary>
            this.Property(t => t.userGroup)

          .HasMaxLength(100);
            /// <summary>
            /// 科室序号
            /// <summary>
            this.Property(t => t.userGroupID)

          .HasMaxLength(50);
            /// <summary>
            /// 服务厅
            /// <summary>
            this.Property(t => t.hall)

          .HasMaxLength(100);
            /// <summary>
            /// 服务厅ID
            /// <summary>
            this.Property(t => t.hallid)

          .HasMaxLength(50);
            /// <summary>
            /// 销售点（班、组）
            /// <summary>
            this.Property(t => t.salepoint)

          .HasMaxLength(100);
            /// <summary>
            /// 销售点ID
            /// <summary>
            this.Property(t => t.salepointid)

          .HasMaxLength(50);
            /// <summary>
            /// 所属组（多个组以英文逗号分隔），如组织是代维团队，则此值为供应商ID和移动接口人，示例：11992233,wuyuming
            ///如组织是渠道树，则值此为渠道编码，示例：ZQFN0014
            /// <summary>
            this.Property(t => t.teams)

          .HasMaxLength(255);
            
            /// <summary>
            /// 日志类型(ADD、UPT、DEL)
            /// <summary>
            this.Property(t => t.operationType).HasMaxLength(50);
            /// <summary>
            /// 组织ID(兼容SMAP平台，其他系统不使用)
            /// <summary>
            this.Property(t => t.OrgID).HasMaxLength(50);
            /// <summary>
            /// 组织名称(兼容SMAP平台，其他系统不使用)
            /// <summary>
            this.Property(t => t.OrgName).HasMaxLength(100);
            /// <summary>
            /// 父组织ID(兼容SMAP平台，其他系统不使用)
            /// <summary>
            this.Property(t => t.ParentOrgID).HasMaxLength(50);


            // Table & Column Mappings
            this.ToTable("UUM_ORGANIZATIONINFO_LOG");
            // 组织ID(针对APP)
            this.Property(t => t.OUGUID).HasColumnName("OUGUID");
            // 父组织ID(针对APP)
            this.Property(t => t.parentOUGUID).HasColumnName("parentOUGUID");
            // 组织ID (portal),兼容portal，现不推荐使用
            this.Property(t => t.OUID).HasColumnName("OUID");
            // 组织类型(UN: 公司  UM: 部门,TD:团队)
            this.Property(t => t.orgTypeID).HasColumnName("orgTypeID");
            // 组织状态（1 启用  0停用）
            this.Property(t => t.orgState).HasColumnName("orgState");
            // 组织名称(portal)/团队名称
            this.Property(t => t.OUName).HasColumnName("OUName");
            // 上级组织ID(portal), 兼容portal，现不推荐使用
            this.Property(t => t.parentOUID).HasColumnName("parentOUID");
            // 组织全名(portal)
            this.Property(t => t.OUFullName).HasColumnName("OUFullName");
            // 组织层次(portal)
            this.Property(t => t.OULevel).HasColumnName("OULevel");
            // 组织排序(portal)
            this.Property(t => t.OUOrder).HasColumnName("OUOrder");
            // 组织中的DN(portal)
            this.Property(t => t.OrgDN).HasColumnName("OrgDN");
            // 所属地区(省公司)
            this.Property(t => t.region).HasColumnName("region");
            // 地区序号
            this.Property(t => t.regionID).HasColumnName("regionID");
            // 所属公司
            this.Property(t => t.company).HasColumnName("company");
            // 公司序号
            this.Property(t => t.companyID).HasColumnName("companyID");
            // 所属分公司
            this.Property(t => t.branch).HasColumnName("branch");
            // 分公司序号
            this.Property(t => t.branchID).HasColumnName("branchID");
            // 所属部门
            this.Property(t => t.department).HasColumnName("department");
            // 部门序号
            this.Property(t => t.departmentID).HasColumnName("departmentID");
            // 所属科室
            this.Property(t => t.userGroup).HasColumnName("userGroup");
            // 科室序号
            this.Property(t => t.userGroupID).HasColumnName("userGroupID");
            // 服务厅
            this.Property(t => t.hall).HasColumnName("hall");
            // 服务厅ID
            this.Property(t => t.hallid).HasColumnName("hallid");
            // 销售点（班、组）
            this.Property(t => t.salepoint).HasColumnName("salepoint");
            // 销售点ID
            this.Property(t => t.salepointid).HasColumnName("salepointid");
            // 所属组（多个组以英文逗号分隔），如组织是代维团队，则此值为供应商ID和移动接口人，示例：11992233,wuyuming
            //如组织是渠道树，则值此为渠道编码，示例：ZQFN0014
            this.Property(t => t.teams).HasColumnName("teams");
            // 记录创建日期
            this.Property(t => t.createTime).HasColumnName("createTime");
            // 最后修改日期
            this.Property(t => t.lastModifyTime).HasColumnName("lastModifyTime");
            // 日志ID
            this.Property(t => t.logID).HasColumnName("logID");
            // 日志类型(ADD、UPT、DEL)
            this.Property(t => t.operationType).HasColumnName("operationType");
            // 组织ID(兼容SMAP平台，其他系统不使用)
            this.Property(t => t.OrgID).HasColumnName("OrgID");
            // 组织名称(兼容SMAP平台，其他系统不使用)
            this.Property(t => t.OrgName).HasColumnName("OrgName");
            // 父组织ID(兼容SMAP平台，其他系统不使用)
            this.Property(t => t.ParentOrgID).HasColumnName("ParentOrgID");
        }
    }
}
